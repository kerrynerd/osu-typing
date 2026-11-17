// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Audio;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Mods
{
    public abstract class ModNightcore : ModDoubleTime
    {
        public override string Name => "升调加速";
        public override string Acronym => "NC";
        public override IconUsage Icon => OsuIcon.ModNightcore;
        public override string Description => "感受高音与加速的节奏！";

        public override void ApplyToClock(IAdjustableClock clock)
        {
            if (clock is IHasPitchAdjust pitchAdjust)
                pitchAdjust.PitchAdjust *= RateAdjust;
            else
                base.ApplyToClock(clock);
        }
    }
}
