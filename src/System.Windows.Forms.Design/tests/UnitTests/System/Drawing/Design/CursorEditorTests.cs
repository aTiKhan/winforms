﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Moq;
using System.Windows.Forms.TestUtilities;
using Xunit;

namespace System.Drawing.Design.Tests
{
    public class CursorEditorTests
    {
        [Fact]
        public void CursorEditor_Ctor_Default()
        {
            var editor = new CursorEditor();
            Assert.True(editor.IsDropDownResizable);
        }

        public static IEnumerable<object[]> EditValue_TestData()
        {
            yield return new object[] { null };
            yield return new object[] { "value" };
            yield return new object[] { Cursors.Default };
            yield return new object[] { new object() };
        }

        [Theory]
        [MemberData(nameof(EditValue_TestData))]
        public void CursorEditor_EditValue_ValidProvider_ReturnsValue(object value)
        {
            var editor = new CursorEditor();
            var mockEditorService = new Mock<IWindowsFormsEditorService>(MockBehavior.Strict);
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IWindowsFormsEditorService)))
                .Returns(mockEditorService.Object)
                .Verifiable();
            mockEditorService
                .Setup(e => e.DropDownControl(It.IsAny<Control>()))
                .Verifiable();
            Assert.Equal(value, editor.EditValue(null, mockServiceProvider.Object, value));
            mockServiceProvider.Verify(p => p.GetService(typeof(IWindowsFormsEditorService)), Times.Once());
            mockEditorService.Verify(e => e.DropDownControl(It.IsAny<Control>()), Times.Once());

            // Edit again.
            Assert.Equal(value, editor.EditValue(null, mockServiceProvider.Object, value));
            mockServiceProvider.Verify(p => p.GetService(typeof(IWindowsFormsEditorService)), Times.Exactly(2));
            mockServiceProvider.Verify(p => p.GetService(typeof(IWindowsFormsEditorService)), Times.Exactly(2));
        }

        [Theory]
        [CommonMemberData(typeof(CommonTestHelperEx), nameof(CommonTestHelperEx.GetEditValueInvalidProviderTestData))]
        public void CursorEditor_EditValue_InvalidProvider_ReturnsValue(IServiceProvider provider, object value)
        {
            var editor = new CursorEditor();
            Assert.Same(value, editor.EditValue(null, provider, value));
        }

        [Theory]
        [CommonMemberData(typeof(CommonTestHelperEx), nameof(CommonTestHelperEx.GetITypeDescriptorContextTestData))]
        public void CursorEditor_GetEditStyle_Invoke_ReturnsModal(ITypeDescriptorContext context)
        {
            var editor = new CursorEditor();
            Assert.Equal(UITypeEditorEditStyle.DropDown, editor.GetEditStyle(context));
        }

        [Theory]
        [CommonMemberData(typeof(CommonTestHelperEx), nameof(CommonTestHelperEx.GetITypeDescriptorContextTestData))]
        public void CursorEditor_GetPaintValueSupported_Invoke_ReturnsFalse(ITypeDescriptorContext context)
        {
            var editor = new CursorEditor();
            Assert.False(editor.GetPaintValueSupported(context));
        }
    }
}
