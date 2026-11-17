// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.MathUtils;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Online.API;
using osu.Game.Overlays.Settings;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Overlays.AccountCreation
{
    public class ScreenEntry : AccountCreationScreen
    {
        private ErrorTextFlowContainer usernameDescription;
        private ErrorTextFlowContainer emailAddressDescription;
        private ErrorTextFlowContainer passwordDescription;

        private OsuTextBox usernameTextBox;
        private OsuTextBox emailTextBox;
        private OsuPasswordTextBox passwordTextBox;

        private IAPIProvider api;
        private ShakeContainer registerShake;
        private IEnumerable<Drawable> characterCheckText;

        private OsuTextBox[] textboxes;
        private ProcessingOverlay processingOverlay;
        private GameHost host;

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, IAPIProvider api, GameHost host)
        {
            this.api = api;
            this.host = host;

            InternalChildren = new Drawable[]
            {
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Padding = new MarginPadding(20),
                    Spacing = new Vector2(0, 10),
                    Children = new Drawable[]
                    {
                        new OsuSpriteText
                        {
                            Margin = new MarginPadding { Vertical = 10 },
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Font = OsuFont.GetFont(size: 20),
                            Text = "创建一个账号吧！",
                        },
                        usernameTextBox = new OsuTextBox
                        {
                            PlaceholderText = "用户名",
                            RelativeSizeAxes = Axes.X,
                            TabbableContentContainer = this
                        },
                        usernameDescription = new ErrorTextFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y
                        },
                        emailTextBox = new OsuTextBox
                        {
                            PlaceholderText = "电子邮箱",
                            RelativeSizeAxes = Axes.X,
                            TabbableContentContainer = this
                        },
                        emailAddressDescription = new ErrorTextFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y
                        },
                        passwordTextBox = new OsuPasswordTextBox
                        {
                            PlaceholderText = "密码",
                            RelativeSizeAxes = Axes.X,
                            TabbableContentContainer = this,
                        },
                        passwordDescription = new ErrorTextFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Children = new Drawable[]
                            {
                                registerShake = new ShakeContainer
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Child = new SettingsButton
                                    {
                                        Text = "注册",
                                        Margin = new MarginPadding { Vertical = 20 },
                                        Action = performRegistration
                                    }
                                }
                            }
                        },
                    },
                },
                processingOverlay = new ProcessingOverlay { Alpha = 0 }
            };

            textboxes = new[] { usernameTextBox, emailTextBox, passwordTextBox };

            usernameDescription.AddText("这是你对外展示的名称。请勿使用不雅词语或冒充他人，也请避免透露个人隐私。");

            emailAddressDescription.AddText("用于通知、账号验证和密码找回，绝不会发送垃圾邮件。");
            emailAddressDescription.AddText("请确保填写正确！", cp => cp.Font = cp.Font.With(Typeface.Exo, weight: FontWeight.Bold));

            passwordDescription.AddText("密码至少需要");
            characterCheckText = passwordDescription.AddText("8 个字符");
            passwordDescription.AddText("。请使用较长且容易记住的密码，例如你喜欢的歌词。");

            passwordTextBox.Current.ValueChanged += password => { characterCheckText.ForEach(s => s.Colour = password.NewValue.Length == 0 ? Color4.White : Interpolation.ValueAt(password.NewValue.Length, Color4.OrangeRed, Color4.YellowGreen, 0, 8, Easing.In)); };
        }

        protected override void Update()
        {
            base.Update();

            if (host?.OnScreenKeyboardOverlapsGameWindow != true && !textboxes.Any(t => t.HasFocus))
                focusNextTextbox();
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);
            processingOverlay.Hide();
        }

        private void performRegistration()
        {
            if (focusNextTextbox())
            {
                registerShake.Shake();
                return;
            }

            usernameDescription.ClearErrors();
            emailAddressDescription.ClearErrors();
            passwordDescription.ClearErrors();

            processingOverlay.Show();

            Task.Run(() =>
            {
                bool success;
                RegistrationRequest.RegistrationRequestErrors errors = null;

                try
                {
                    errors = api.CreateAccount(emailTextBox.Text, usernameTextBox.Text, passwordTextBox.Text);
                    success = errors == null;
                }
                catch (Exception)
                {
                    success = false;
                }

                Schedule(() =>
                {
                    if (!success)
                    {
                        if (errors != null)
                        {
                            usernameDescription.AddErrors(errors.User.Username);
                            emailAddressDescription.AddErrors(errors.User.Email);
                            passwordDescription.AddErrors(errors.User.Password);
                        }
                        else
                        {
                            passwordDescription.AddErrors(new[] { "发生了未知错误，请稍后重试。" });
                        }

                        registerShake.Shake();
                        processingOverlay.Hide();
                        return;
                    }

                    api.Login(emailTextBox.Text, passwordTextBox.Text);
                });
            });
        }

        private bool focusNextTextbox()
        {
            var nextTextbox = nextUnfilledTextbox();

            if (nextTextbox != null)
            {
                Schedule(() => GetContainingInputManager().ChangeFocus(nextTextbox));
                return true;
            }

            return false;
        }

        private OsuTextBox nextUnfilledTextbox() => textboxes.FirstOrDefault(t => string.IsNullOrEmpty(t.Text));
    }
}
