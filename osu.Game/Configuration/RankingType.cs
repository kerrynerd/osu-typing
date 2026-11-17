// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Game.Configuration
{
    public enum RankingType
    {
        Local,

        [Description("全球")]
        Top,

        [Description("所选模组")]
        SelectedMod,
        Friends,
        Country
    }
}
