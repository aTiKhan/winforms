﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Forms;
using Xunit;

namespace System.Resources.Tests;

public class ResXResourceReaderTests
{
    [Fact]
    public void ResXResourceReader_Deserialize_AxHost_FormatterEnabled_Throws()
    {
        using var formatterScope = new BinaryFormatterScope(enable: true);

        string resxPath = Path.GetFullPath(@".\Resources\AxHosts.resx");
        using MemoryStream resourceStream = new();

        // ResourceWriter Dispose calls Generate method which will throw.
        Assert.Throws<PlatformNotSupportedException>(() =>
        {
            using ResourceWriter resourceWriter = new(resourceStream);
            using ResXResourceReader resxReader = new(resxPath);
            var enumerator = resxReader.GetEnumerator();

            Assert.True(enumerator.MoveNext());
            string key = enumerator.Key.ToString();
            object value = enumerator.Value;
            Assert.Equal("axWindowsMediaPlayer1.OcxState", key);
            Assert.Equal(typeof(AxHost.State), value.GetType());
            Assert.False(enumerator.MoveNext());

            resourceWriter.AddResource(key, value);

            // ResourceWriter no longer supports BinaryFormatter in core.
            Assert.Throws<PlatformNotSupportedException>(resourceWriter.Generate);
        });
    }

    [Fact]
    public void ResXResourceReader_Deserialize_AxHost_FormatterDisabled_Throws()
    {
        using var formatterScope = new BinaryFormatterScope(enable: false);

        string resxPath = Path.GetFullPath(@".\Resources\AxHosts.resx");
        using MemoryStream resourceStream = new();

        using ResXResourceReader resxReader = new(resxPath);
        Assert.Throws<ArgumentException>(() => resxReader.GetEnumerator());
    }
}
