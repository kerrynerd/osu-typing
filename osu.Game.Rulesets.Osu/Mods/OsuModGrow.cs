// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Osu.Mods
{
    internal class OsuModGrow : OsuModeObjectScaleTween
    {
        public override string Name => "放大";

        public override string Acronym => "GR";

        public override IconUsage Icon => FontAwesome.Solid.ArrowsAltV;

        public override string Description => "在大小合适时击打！";

        protected override float StartScale => 0.5f;
    }
}
