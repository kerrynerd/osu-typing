// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Game.Configuration
{
    public enum ScoreMeterType
    {
        [Description("无")]
        None,

        [Description("击打误差（左侧）")]
        HitErrorLeft,

        [Description("击打误差（右侧）")]
        HitErrorRight,

        [Description("击打误差（两侧）")]
        HitErrorBoth,
    }
}
