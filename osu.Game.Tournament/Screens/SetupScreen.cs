// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Online.API;
using osu.Game.Overlays;
using osu.Game.Tournament.IPC;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Tournament.Screens
{
    public class SetupScreen : TournamentScreen, IProvideVideo
    {
        private FillFlowContainer fillFlow;

        private LoginOverlay loginOverlay;

        [Resolved]
        private MatchIPCInfo ipc { get; set; }

        [Resolved]
        private IAPIProvider api { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = fillFlow = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Padding = new MarginPadding(10),
                Spacing = new Vector2(10),
            };

            api.LocalUser.BindValueChanged(_ => Schedule(reload));
            reload();
        }

        private void reload()
        {
            var fileBasedIpc = ipc as FileBasedIPC;

            fillFlow.Children = new Drawable[]
            {
                new ActionableInfo
                {
                    Label = "当前 IPC 数据源",
                    ButtonText = "刷新",
                    Action = () =>
                    {
                        fileBasedIpc?.LocateStableStorage();
                        reload();
                    },
                    Value = fileBasedIpc?.Storage?.GetFullPath(string.Empty) ?? "未找到",
                    Failing = fileBasedIpc?.Storage == null,
                    Description = "当前用于提供数据的 osu! 稳定版安装。若未找到数据源，请确认已在 cutting-edge 版本目录中创建空的 ipc.txt，并将其注册为默认 osu! 安装。"
                },
                new ActionableInfo
                {
                    Label = "当前用户",
                    ButtonText = "切换账号",
                    Action = () =>
                    {
                        api.Logout();

                        if (loginOverlay == null)
                        {
                            AddInternal(loginOverlay = new LoginOverlay
                            {
                                Anchor = Anchor.TopRight,
                                Origin = Anchor.TopRight,
                            });
                        }

                        loginOverlay.State.Value = Visibility.Visible;
                    },
                    Value = api?.LocalUser.Value.Username,
                    Failing = api?.IsLoggedIn != true,
                    Description = "访问 API 和显示谱面信息需要先登录。"
                }
            };
        }

        private class ActionableInfo : LabelledComponent<Drawable>
        {
            private OsuButton button;

            public ActionableInfo()
                : base(true)
            {
            }

            public string ButtonText
            {
                set => button.Text = value;
            }

            public string Value
            {
                set => valueText.Text = value;
            }

            public bool Failing
            {
                set => valueText.Colour = value ? Color4.Red : Color4.White;
            }

            public Action Action;

            private OsuSpriteText valueText;

            protected override Drawable CreateComponent() => new Container
            {
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Children = new Drawable[]
                {
                    valueText = new OsuSpriteText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                    },
                    button = new TriangleButton
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Size = new Vector2(100, 30),
                        Action = () => Action?.Invoke()
                    },
                }
            };
        }
    }
}
