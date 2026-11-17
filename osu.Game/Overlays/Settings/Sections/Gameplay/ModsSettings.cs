// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Configuration;

namespace osu.Game.Overlays.Settings.Sections.Gameplay
{
    public class ModsSettings : SettingsSubsection
    {
        protected override string Header => "模组";

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config)
        {
            Children = new[]
            {
                new SettingsCheckbox
                {
                    LabelText = "启用影响视野的模组时，提高第一个物件的可见度",
                    Bindable = config.GetBindable<bool>(OsuSetting.IncreaseFirstObjectVisibility)
                },
            };
        }
    }
}
