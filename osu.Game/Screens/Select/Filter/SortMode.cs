// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Game.Screens.Select.Filter
{
    public enum SortMode
    {
        [Description("艺术家")]
        Artist,

        [Description("谱师")]
        Author,

        [Description("BPM")]
        BPM,

        [Description("添加时间")]
        DateAdded,

        [Description("难度")]
        Difficulty,

        [Description("时长")]
        Length,

        [Description("已获评级")]
        RankAchieved,

        [Description("曲名")]
        Title
    }
}
