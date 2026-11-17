// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Mods
{
    public abstract class ModPerfect : ModSuddenDeath
    {
        public override string Name => "完美";
        public override string Acronym => "PF";
        public override IconUsage Icon => OsuIcon.ModPerfect;
        public override string Description => "达不到 SS 就重新开始。";

        protected override bool FailCondition(ScoreProcessor scoreProcessor, JudgementResult result) => scoreProcessor.Accuracy.Value != 1;
    }
}
