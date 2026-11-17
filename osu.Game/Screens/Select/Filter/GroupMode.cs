// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Game.Screens.Select.Filter
{
    public enum GroupMode
    {
        [Description("全部")]
        All,

        [Description("艺术家")]
        Artist,

        [Description("谱师")]
        Author,

        [Description("BPM")]
        BPM,

        [Description("收藏夹")]
        Collections,

        [Description("添加时间")]
        DateAdded,

        [Description("难度")]
        Difficulty,

        [Description("收藏")]
        Favourites,

        [Description("时长")]
        Length,

        [Description("我的谱面")]
        MyMaps,

        [Description("不分组")]
        NoGrouping,

        [Description("已获评级")]
        RankAchieved,

        [Description("上榜状态")]
        RankedStatus,

        [Description("最近游玩")]
        RecentlyPlayed,

        [Description("曲名")]
        Title
    }
}
