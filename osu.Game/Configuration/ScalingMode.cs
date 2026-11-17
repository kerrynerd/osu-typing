// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Game.Configuration
{
    public enum ScalingMode
    {
        [Description("关闭")]
        Off,
        [Description("全部")]
        Everything,

        [Description("不缩放浮层")]
        ExcludeOverlays,
        [Description("仅游戏画面")]
        Gameplay,
    }
}
