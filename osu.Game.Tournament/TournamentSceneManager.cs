// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Platform;
using osu.Game.Graphics.UserInterface;
using osu.Game.Tournament.Components;
using osu.Game.Tournament.Models;
using osu.Game.Tournament.Screens;
using osu.Game.Tournament.Screens.Drawings;
using osu.Game.Tournament.Screens.Editors;
using osu.Game.Tournament.Screens.Gameplay;
using osu.Game.Tournament.Screens.Ladder;
using osu.Game.Tournament.Screens.MapPool;
using osu.Game.Tournament.Screens.Schedule;
using osu.Game.Tournament.Screens.Showcase;
using osu.Game.Tournament.Screens.TeamIntro;
using osu.Game.Tournament.Screens.TeamWin;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Tournament
{
    [Cached]
    public class TournamentSceneManager : CompositeDrawable
    {
        private Container screens;
        private TourneyVideo video;

        [Cached]
        private TournamentMatchChatDisplay chat = new TournamentMatchChatDisplay();

        private Container chatContainer;

        public TournamentSceneManager()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(LadderInfo ladder, Storage storage)
        {
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    X = 200,
                    FillMode = FillMode.Fit,
                    FillAspectRatio = 16 / 9f,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Size = new Vector2(0.8f, 1),
                    //Masking = true,
                    Children = new Drawable[]
                    {
                        video = new TourneyVideo(storage.GetStream("BG Logoless - OWC.m4v"))
                        {
                            Loop = true,
                            RelativeSizeAxes = Axes.Both,
                        },
                        screens = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new SetupScreen(),
                                new ScheduleScreen(),
                                new LadderScreen(),
                                new LadderEditorScreen(),
                                new TeamEditorScreen(),
                                new RoundEditorScreen(),
                                new ShowcaseScreen(),
                                new MapPoolScreen(),
                                new TeamIntroScreen(),
                                new DrawingsScreen(),
                                new GameplayScreen(),
                                new TeamWinScreen()
                            }
                        },
                        chatContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = chat
                        },
                    }
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = 200,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = Color4.Black,
                            RelativeSizeAxes = Axes.Both,
                        },
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Children = new Drawable[]
                            {
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "设置", Action = () => SetScreen(typeof(SetupScreen)) },
                                new Container { RelativeSizeAxes = Axes.X, Height = 50 },
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "队伍编辑", Action = () => SetScreen(typeof(TeamEditorScreen)) },
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "轮次编辑", Action = () => SetScreen(typeof(RoundEditorScreen)) },
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "对阵编辑", Action = () => SetScreen(typeof(LadderEditorScreen)) },
                                new Container { RelativeSizeAxes = Axes.X, Height = 50 },
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "抽签", Action = () => SetScreen(typeof(DrawingsScreen)) },
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "展示", Action = () => SetScreen(typeof(ShowcaseScreen)) },
                                new Container { RelativeSizeAxes = Axes.X, Height = 50 },
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "赛程", Action = () => SetScreen(typeof(ScheduleScreen)) },
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "对阵表", Action = () => SetScreen(typeof(LadderScreen)) },
                                new Container { RelativeSizeAxes = Axes.X, Height = 50 },
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "队伍介绍", Action = () => SetScreen(typeof(TeamIntroScreen)) },
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "图池", Action = () => SetScreen(typeof(MapPoolScreen)) },
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "比赛", Action = () => SetScreen(typeof(GameplayScreen)) },
                                new Container { RelativeSizeAxes = Axes.X, Height = 50 },
                                new OsuButton { RelativeSizeAxes = Axes.X, Text = "获胜", Action = () => SetScreen(typeof(TeamWinScreen)) },
                            }
                        },
                    },
                },
            };

            SetScreen(typeof(SetupScreen));
        }

        public void SetScreen(Type screenType)
        {
            var screen = screens.FirstOrDefault(s => s.GetType() == screenType);
            if (screen == null) return;

            foreach (var s in screens.Children)
            {
                if (s == screen)
                {
                    s.Show();
                    if (s is IProvideVideo)
                        video.FadeOut(200);
                    else
                        video.Show();
                }
                else
                    s.Hide();
            }

            switch (screen)
            {
                case GameplayScreen _:
                case MapPoolScreen _:
                    chatContainer.FadeIn(100);
                    break;

                default:
                    chatContainer.FadeOut(100);
                    break;
            }
        }
    }
}
