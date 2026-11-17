// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Online.API.Requests.Responses;
using osu.Game.Online.Chat;
using System;
using osuTK;

namespace osu.Game.Overlays.Profile.Sections.Kudosu
{
    public class DrawableKudosuHistoryItem : CompositeDrawable
    {
        private const int height = 25;

        [Resolved]
        private OsuColour colours { get; set; }

        private readonly APIKudosuHistory historyItem;
        private readonly LinkFlowContainer linkFlowContainer;
        private readonly DrawableDate date;

        public DrawableKudosuHistoryItem(APIKudosuHistory historyItem)
        {
            this.historyItem = historyItem;

            Height = height;
            RelativeSizeAxes = Axes.X;
            AddRangeInternal(new Drawable[]
            {
                linkFlowContainer = new LinkFlowContainer
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    AutoSizeAxes = Axes.Both,
                    Spacing = new Vector2(0, 3),
                },
                date = new DrawableDate(historyItem.CreatedAt)
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            date.Colour = colours.GreySeafoamLighter;
            var formattedSource = MessageFormatter.FormatText(getString(historyItem));
            linkFlowContainer.AddLinks(formattedSource.Text, formattedSource.Links);
        }

        private string getString(APIKudosuHistory item)
        {
            string amount = $"{Math.Abs(item.Amount)} kudosu";
            string post = $"[{item.Post.Title}]({item.Post.Url})";

            switch (item.Source)
            {
                case KudosuSource.AllowKudosu:
                    switch (item.Action)
                    {
                        case KudosuAction.Give:
                            return $"恢复了 {amount}，来自撤销摸图帖 {post} 的 kudosu 否决";
                    }

                    break;

                case KudosuSource.DenyKudosu:
                    switch (item.Action)
                    {
                        case KudosuAction.Reset:
                            return $"被否决 {amount}，来自摸图帖 {post}";
                    }

                    break;

                case KudosuSource.Delete:
                    switch (item.Action)
                    {
                        case KudosuAction.Reset:
                            return $"失去 {amount}，由于摸图帖 {post} 被删除";
                    }

                    break;

                case KudosuSource.Restore:
                    switch (item.Action)
                    {
                        case KudosuAction.Give:
                            return $"获得 {amount}，由于摸图帖 {post} 被恢复";
                    }

                    break;

                case KudosuSource.Vote:
                    switch (item.Action)
                    {
                        case KudosuAction.Give:
                            return $"获得 {amount}，由于摸图帖 {post} 获得投票";

                        case KudosuAction.Reset:
                            return $"失去 {amount}，由于摸图帖 {post} 失去投票";
                    }

                    break;

                case KudosuSource.Recalculate:
                    switch (item.Action)
                    {
                        case KudosuAction.Give:
                            return $"获得 {amount}，由于摸图帖 {post} 重新计票";

                        case KudosuAction.Reset:
                            return $"失去 {amount}，由于摸图帖 {post} 重新计票";
                    }

                    break;

                case KudosuSource.Forum:

                    string giver = $"[{item.Giver?.Username}]({item.Giver?.Url})";

                    switch (historyItem.Action)
                    {
                        case KudosuAction.Give:
                            return $"获得 {amount}，由 {giver} 给予，来自帖子 {post}";

                        case KudosuAction.Reset:
                            return $"kudosu 被 {giver} 重置，来自帖子 {post}";

                        case KudosuAction.Revoke:
                            return $"kudosu 被 {giver} 否决，来自帖子 {post}";
                    }

                    break;
            }

            return $"未知事件（变动 {amount}）";
        }
    }
}
