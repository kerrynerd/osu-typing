// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osuTK.Graphics;

namespace osu.Game.Overlays.BeatmapSet
{
    public class BeatmapAvailability : Container
    {
        private BeatmapSetInfo beatmapSet;

        private bool downloadDisabled => BeatmapSet?.OnlineInfo.Availability?.DownloadDisabled ?? false;
        private bool hasExternalLink => !string.IsNullOrEmpty(BeatmapSet?.OnlineInfo.Availability?.ExternalLink);

        private readonly LinkFlowContainer textContainer;

        public BeatmapAvailability()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Padding = new MarginPadding { Top = 10, Right = 20 };

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black.Opacity(0.6f),
                },
                textContainer = new LinkFlowContainer(t => t.Font = OsuFont.GetFont(size: 14))
                {
                    Direction = FillDirection.Full,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding(10),
                },
            };
        }

        public BeatmapSetInfo BeatmapSet
        {
            get => beatmapSet;

            set
            {
                if (value == beatmapSet)
                    return;

                beatmapSet = value;

                if (downloadDisabled || hasExternalLink)
                {
                    Show();
                    updateText();
                }
                else
                    Hide();
            }
        }

        private void updateText()
        {
            textContainer.Clear();
            textContainer.AddParagraph(downloadDisabled
                ? "此谱面暂不可下载。"
                : "应谱师或第三方权利人的要求，此谱面的部分内容已被移除。", t => t.Colour = Color4.Orange);

            if (hasExternalLink)
            {
                textContainer.NewParagraph();
                textContainer.NewParagraph();
                textContainer.AddLink("点击此处了解详情。", BeatmapSet.OnlineInfo.Availability.ExternalLink, creationParameters: t => t.Font = OsuFont.GetFont(size: 10));
            }
        }
    }
}
