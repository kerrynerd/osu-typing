// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Game.Rulesets.Osu.Judgements
{
    public enum ComboResult
    {
        [Description(@"")]
        None,

        [Description("良好")]
        Good,

        [Description("惊艳")]
        Perfect
    }
}
