// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Screens.Select
{
    public class ImportFromStablePopup : PopupDialog
    {
        public ImportFromStablePopup(Action importFromStable)
        {
            HeaderText = "你还没有谱面！";
            BodyText = "检测到了已有的 osu! 安装。\n是否导入其中的谱面、皮肤和成绩？";

            Icon = FontAwesome.Solid.Plane;

            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = "开始导入",
                    Action = importFromStable
                },
                new PopupDialogCancelButton
                {
                    Text = "不用，从头开始",
                },
            };
        }
    }
}
