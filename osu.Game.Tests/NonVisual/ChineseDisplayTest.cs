// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using osu.Framework.Configuration;
using osu.Framework.Extensions;
using osu.Framework.Input;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Online.API.Requests;
using osu.Game.Online.Multiplayer;
using osu.Game.Utils;

namespace osu.Game.Tests.NonVisual
{
    [TestFixture]
    [SetCulture("en-US")]
    public class ChineseDisplayTest
    {
        [TestCase(WindowMode.Windowed, "窗口模式")]
        [TestCase(WindowMode.Borderless, "无边框窗口")]
        [TestCase(WindowMode.Fullscreen, "全屏")]
        public void TestWindowModeDisplay(WindowMode value, string expected)
        {
            using (var dropdown = new DisplayDropdown<WindowMode>())
                Assert.AreEqual(expected, dropdown.Display(value));
        }

        [TestCase(FrameSync.VSync, "垂直同步")]
        [TestCase(FrameSync.Unlimited, "无限制")]
        public void TestFrameSyncDisplay(FrameSync value, string expected)
        {
            using (var dropdown = new DisplayDropdown<FrameSync>())
                Assert.AreEqual(expected, dropdown.Display(value));
        }

        [Test]
        public void TestConfineMouseDisplay()
        {
            using (var dropdown = new DisplayDropdown<ConfineMouseMode>())
                Assert.AreEqual("仅全屏时", dropdown.Display(ConfineMouseMode.Fullscreen));
        }

        [Test]
        public void TestDescriptionFallback()
        {
            using (var dropdown = new DisplayDropdown<ScalingMode>())
                Assert.AreEqual("不缩放浮层", dropdown.Display(ScalingMode.ExcludeOverlays));

            using (var dropdown = new DisplayDropdown<ReleaseStream>())
                Assert.AreEqual("Lazer", dropdown.Display(ReleaseStream.Lazer));
        }

        [Test]
        public void TestDisplayDescriptionsDoNotChangeApiValues()
        {
            var converter = new StringEnumConverter();
            Assert.AreEqual("公开", RoomAvailability.Public.GetDescription());
            Assert.AreEqual("\"Public\"", JsonConvert.SerializeObject(RoomAvailability.Public, converter));
            Assert.AreEqual("已上架", BeatmapApproval.Ranked.GetDescription());
            Assert.AreEqual("\"Ranked\"", JsonConvert.SerializeObject(BeatmapApproval.Ranked, converter));
            Assert.AreEqual(BeatmapApproval.Ranked, JsonConvert.DeserializeObject<BeatmapApproval>("\"Ranked\"", converter));
        }

        [Test]
        public void TestRelativeDateUsesChineseWithoutChangingProcessCulture()
        {
            var culture = CultureInfo.CurrentCulture;
            var uiCulture = CultureInfo.CurrentUICulture;
            StringAssert.Contains("小时前", HumanizerUtils.Humanize(DateTimeOffset.UtcNow.AddHours(-2)));
            Assert.AreSame(culture, CultureInfo.CurrentCulture);
            Assert.AreSame(uiCulture, CultureInfo.CurrentUICulture);
            Assert.AreEqual(1.5, double.Parse("1.5"));
        }

        [Test]
        public void TestDateTooltipIncludesLocalUtcOffset()
        {
            var date = new DateTimeOffset(2020, 1, 2, 3, 4, 0, TimeSpan.Zero);
            using (var drawable = new DrawableDate(date))
                Assert.AreEqual(date.ToLocalTime().ToString("yyyy年M月d日 HH:mm 'UTC'zzz"), drawable.TooltipText);
        }

        private class DisplayDropdown<T> : OsuDropdown<T>
        {
            public string Display(T value) => GenerateItemText(value);
        }
    }
}
