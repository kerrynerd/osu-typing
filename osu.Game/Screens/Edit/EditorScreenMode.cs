// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Game.Screens.Edit
{
    public enum EditorScreenMode
    {
        [Description("设置")]
        SongSetup,

        [Description("编排")]
        Compose,

        [Description("设计")]
        Design,

        [Description("节拍")]
        Timing,
    }
}
