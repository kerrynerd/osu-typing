// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Overlays.Chat
{
    public class ExternalLinkDialog : PopupDialog
    {
        public ExternalLinkDialog(string url, Action openExternalLinkAction)
        {
            HeaderText = "请确认……";
            BodyText = $"即将离开 osu!，并在浏览器中打开以下链接：\n\n{url}";

            Icon = FontAwesome.Solid.ExclamationTriangle;

            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = "确认",
                    Action = openExternalLinkAction
                },
                new PopupDialogCancelButton
                {
                    Text = "取消"
                },
            };
        }
    }
}
