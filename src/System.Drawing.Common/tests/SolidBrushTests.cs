// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Xunit;

namespace System.Drawing.Tests
{
    public class SolidBrushTests
    {
        public static IEnumerable<object[]> Colors_TestData()
        {
            yield return new object[] { new Color(), Color.FromArgb(0) };
            yield return new object[] { Color.PapayaWhip, Color.PapayaWhip };
        }

        [Theory]
        [MemberData(nameof(Colors_TestData))]
        public void Ctor_Color(Color color, Color expectedColor)
        {
            var brush = new SolidBrush(color);
            Assert.Equal(expectedColor, brush.Color);
        }

        [Fact]
        public void Clone_Color_ReturnsClone()
        {
            var brush = new SolidBrush(Color.PeachPuff);
            SolidBrush clone = Assert.IsType<SolidBrush>(brush.Clone());

            Assert.NotSame(clone, brush);
            Assert.Equal(brush.Color.ToArgb(), clone.Color.ToArgb());

            // Known colors are not preserved across clones.
            Assert.NotEqual(Color.PeachPuff, clone.Color);

            // Modifying the original brush should not modify the clone.
            brush.Color = Color.PapayaWhip;
            Assert.NotEqual(Color.PapayaWhip, clone.Color);
        }

        [Fact]
        public void Clone_ImmutableColor_ReturnsMutableClone()
        {
            SolidBrush brush = Assert.IsType<SolidBrush>(Brushes.Bisque);
            SolidBrush clone = Assert.IsType<SolidBrush>(brush.Clone());

            clone.Color = SystemColors.AppWorkspace;
            Assert.Equal(SystemColors.AppWorkspace, clone.Color);
            Assert.Equal(Color.Bisque, brush.Color);
        }

        [Fact]
        public void Clone_Disposed_ThrowsArgumentException()
        {
            var brush = new SolidBrush(Color.LavenderBlush);
            brush.Dispose();

            AssertExtensions.Throws<ArgumentException>(null, () => brush.Clone());
        }

        [Fact]
        public void Color_EmptyAndGetDisposed_ThrowsArgumentException()
        {
            var brush = new SolidBrush(new Color());
            brush.Dispose();

            AssertExtensions.Throws<ArgumentException>(null, () => brush.Color);
        }

        [Fact]
        public void Color_NonEmptyAndGetDisposed_ReturnsExpected()
        {
            var brush = new SolidBrush(Color.Aquamarine);
            brush.Dispose();

            Assert.Equal(Color.Aquamarine, brush.Color);
        }

        [Fact]
        public void Color_SetValid_GetReturnsExpected()
        {
            var brush = new SolidBrush(Color.Goldenrod) { Color = Color.GhostWhite };
            Assert.Equal(Color.GhostWhite, brush.Color);
        }

        [Fact]
        public void Color_SetDisposed_ThrowsArgumentException()
        {
            var brush = new SolidBrush(new Color());
            brush.Dispose();

            AssertExtensions.Throws<ArgumentException>(null, () => brush.Color = Color.WhiteSmoke);
        }

        [Fact]
        public void Color_SetImmutable_ThrowsArgumentException()
        {
            SolidBrush brush = Assert.IsType<SolidBrush>(SystemBrushes.ActiveBorder);
            AssertExtensions.Throws<ArgumentException>(null, () => brush.Color = Color.AntiqueWhite);
        }

        [Fact]
        public void Dispose_MultipleTimes_Success()
        {
            var brush = new SolidBrush(Color.Plum);
            brush.Dispose();
            brush.Dispose();
        }

        [Fact]
        public void Dispose_SetImmutable_ThrowsArgumentException()
        {
            SolidBrush brush = Assert.IsType<SolidBrush>(SystemBrushes.ActiveBorder);
            AssertExtensions.Throws<ArgumentException>(null, () => brush.Dispose());
        }
    }
}
