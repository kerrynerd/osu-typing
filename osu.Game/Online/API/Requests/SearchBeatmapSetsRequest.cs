// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Game.Overlays;
using osu.Game.Overlays.Direct;
using osu.Game.Rulesets;

namespace osu.Game.Online.API.Requests
{
    public class SearchBeatmapSetsRequest : APIRequest<SearchBeatmapSetsResponse>
    {
        private readonly string query;
        private readonly RulesetInfo ruleset;
        private readonly BeatmapSearchCategory searchCategory;
        private readonly DirectSortCriteria sortCriteria;
        private readonly SortDirection direction;
        private string directionString => direction == SortDirection.Descending ? @"desc" : @"asc";

        public SearchBeatmapSetsRequest(string query, RulesetInfo ruleset, BeatmapSearchCategory searchCategory = BeatmapSearchCategory.Any, DirectSortCriteria sortCriteria = DirectSortCriteria.Ranked, SortDirection direction = SortDirection.Descending)
        {
            this.query = System.Uri.EscapeDataString(query);
            this.ruleset = ruleset;
            this.searchCategory = searchCategory;
            this.sortCriteria = sortCriteria;
            this.direction = direction;
        }

        // ReSharper disable once ImpureMethodCallOnReadonlyValueField
        protected override string Target => $@"beatmapsets/search?q={query}&m={ruleset.ID ?? 0}&s={searchCategory.ToString().ToLowerInvariant()}&sort={sortCriteria.ToString().ToLowerInvariant()}_{directionString}";
    }

    public enum BeatmapSearchCategory
    {
        [Description("全部")]
        Any,

        [Description("有排行榜")]
        Leaderboard,
        [Description("已上架")]
        Ranked,
        [Description("已过审")]
        Qualified,
        [Description("社区喜爱")]
        Loved,
        [Description("收藏")]
        Favourites,

        [Description("待定与制作中")]
        Pending,
        [Description("坟场")]
        Graveyard,

        [Description("我的谱面")]
        Mine,
    }
}
