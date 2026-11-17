// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Online.API;
using osu.Game.Online.API.Requests;
using osu.Game.Online.API.Requests.Responses;
using osu.Game.Online.Chat;
using osu.Game.Online.Leaderboards;

namespace osu.Game.Overlays.Profile.Sections.Recent
{
    public class DrawableRecentActivity : DrawableProfileRow
    {
        private IAPIProvider api;

        private readonly APIRecentActivity activity;

        private LinkFlowContainer content;

        public DrawableRecentActivity(APIRecentActivity activity)
        {
            this.activity = activity;
        }

        [BackgroundDependencyLoader]
        private void load(IAPIProvider api)
        {
            this.api = api;

            LeftFlowContainer.Padding = new MarginPadding { Left = 10, Right = 160 };

            LeftFlowContainer.Add(content = new LinkFlowContainer
            {
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
            });

            RightFlowContainer.Add(new DrawableDate(activity.CreatedAt)
            {
                Font = OsuFont.GetFont(size: 13),
                Colour = OsuColour.Gray(0xAA),
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
            });

            var formatted = createMessage();

            content.AddLinks(formatted.Text, formatted.Links);
        }

        protected override Drawable CreateLeftVisual()
        {
            switch (activity.Type)
            {
                case RecentActivityType.Rank:
                    return new UpdateableRank(activity.ScoreRank)
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = 60,
                        FillMode = FillMode.Fit,
                    };

                case RecentActivityType.Achievement:
                    return new MedalIcon(activity.Achievement.Slug)
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = 60,
                        FillMode = FillMode.Fit,
                    };

                default:
                    return new Container
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = 60,
                        FillMode = FillMode.Fit,
                    };
            }
        }

        private string toAbsoluteUrl(string url) => $"{api.Endpoint}{url}";

        private MessageFormatter.MessageFormatterResult createMessage()
        {
            string userLinkTemplate() => $"[{toAbsoluteUrl(activity.User?.Url)} {activity.User?.Username}]";
            string beatmapLinkTemplate() => $"[{toAbsoluteUrl(activity.Beatmap?.Url)} {activity.Beatmap?.Title}]";
            string beatmapsetLinkTemplate() => $"[{toAbsoluteUrl(activity.Beatmapset?.Url)} {activity.Beatmapset?.Title}]";

            string message;

            switch (activity.Type)
            {
                case RecentActivityType.Achievement:
                    message = $"{userLinkTemplate()} 解锁了 {activity.Achievement.Name} 奖章！";
                    break;

                case RecentActivityType.BeatmapPlaycount:
                    message = $"{beatmapLinkTemplate()} 已被游玩 {activity.Count} 次！";
                    break;

                case RecentActivityType.BeatmapsetApprove:
                    message = $"{beatmapsetLinkTemplate()} 的状态已变为 {activity.Approval.GetDescription()}！";
                    break;

                case RecentActivityType.BeatmapsetDelete:
                    message = $"{beatmapsetLinkTemplate()} 已被删除。";
                    break;

                case RecentActivityType.BeatmapsetRevive:
                    message = $"{beatmapsetLinkTemplate()} 已被 {userLinkTemplate()} 从坟场中恢复。";
                    break;

                case RecentActivityType.BeatmapsetUpdate:
                    message = $"{userLinkTemplate()} 更新了谱面 {beatmapsetLinkTemplate()}！";
                    break;

                case RecentActivityType.BeatmapsetUpload:
                    message = $"{userLinkTemplate()} 提交了新谱面 {beatmapsetLinkTemplate()}！";
                    break;

                case RecentActivityType.Medal:
                    // apparently this shouldn't exist look at achievement instead (https://github.com/ppy/osu-web/blob/master/resources/assets/coffee/react/profile-page/recent-activity.coffee#L111)
                    message = string.Empty;
                    break;

                case RecentActivityType.Rank:
                    message = $"{userLinkTemplate()} 获得第 {activity.Rank} 名，谱面：{beatmapLinkTemplate()}（{activity.Mode}）";
                    break;

                case RecentActivityType.RankLost:
                    message = $"{userLinkTemplate()} 失去了 {beatmapLinkTemplate()} 的第一名（{activity.Mode}）";
                    break;

                case RecentActivityType.UserSupportAgain:
                    message = $"{userLinkTemplate()} 再次支持了 osu!，感谢你的慷慨支持！";
                    break;

                case RecentActivityType.UserSupportFirst:
                    message = $"{userLinkTemplate()} 成为了 osu!supporter，感谢你的慷慨支持！";
                    break;

                case RecentActivityType.UserSupportGift:
                    message = $"{userLinkTemplate()} 收到了 osu!supporter 礼物！";
                    break;

                case RecentActivityType.UsernameChange:
                    message = $"{activity.User?.PreviousUsername} 将用户名改为了 {userLinkTemplate()}！";
                    break;

                default:
                    message = string.Empty;
                    break;
            }

            return MessageFormatter.FormatText(message);
        }
    }
}
