// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Audio;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;

namespace osu.Game.Rulesets.Mods
{
    public abstract class ModDaycore : ModHalfTime
    {
        public override string Name => "降调减速";
        public override string Acronym => "DC";
        public override IconUsage Icon => FontAwesome.Solid.Question;
        public override string Description => "慢下来，享受低沉的旋律……";

        public override void ApplyToClock(IAdjustableClock clock)
        {
            if (clock is IHasPitchAdjust pitchAdjust)
                pitchAdjust.PitchAdjust *= RateAdjust;
            else
                base.ApplyToClock(clock);
        }
    }
}
