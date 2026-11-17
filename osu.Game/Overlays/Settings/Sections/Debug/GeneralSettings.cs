// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;

namespace osu.Game.Overlays.Settings.Sections.Debug
{
    public class GeneralSettings : SettingsSubsection
    {
        protected override string Header => "常规";

        [BackgroundDependencyLoader]
        private void load(FrameworkDebugConfigManager config, FrameworkConfigManager frameworkConfig)
        {
            Children = new Drawable[]
            {
                new SettingsCheckbox
                {
                    LabelText = "显示日志面板",
                    Bindable = frameworkConfig.GetBindable<bool>(FrameworkSetting.ShowLogOverlay)
                },
                new SettingsCheckbox
                {
                    LabelText = "记录性能日志",
                    Bindable = frameworkConfig.GetBindable<bool>(FrameworkSetting.PerformanceLogging)
                },
                new SettingsCheckbox
                {
                    LabelText = "跳过从前到后的渲染过程",
                    Bindable = config.GetBindable<bool>(DebugSetting.BypassFrontToBackPass)
                }
            };
        }
    }
}
