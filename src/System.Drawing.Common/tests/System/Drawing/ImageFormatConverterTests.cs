﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using Xunit;

namespace System.ComponentModel.TypeConverterTests
{
    public class ImageFormatConverterTest
    {
        private readonly ImageFormat _imageFmt;
        private readonly ImageFormatConverter _imgFmtConv;
        private readonly ImageFormatConverter _imgFmtConvFrmTD;
        private readonly string _imageFmtStr;

        public ImageFormatConverterTest()
        {
            _imageFmt = ImageFormat.Bmp;
            _imageFmtStr = _imageFmt.ToString();

            _imgFmtConv = new ImageFormatConverter();
            _imgFmtConvFrmTD = (ImageFormatConverter)TypeDescriptor.GetConverter(_imageFmt);
        }

        [Fact]
        public void TestCanConvertFrom()
        {
            Assert.True(_imgFmtConv.CanConvertFrom(typeof(string)), "string (no context)");
            Assert.True(_imgFmtConv.CanConvertFrom(null, typeof(string)), "string");
            Assert.False(_imgFmtConv.CanConvertFrom(null, typeof(ImageFormat)), "ImageFormat");
            Assert.False(_imgFmtConv.CanConvertFrom(null, typeof(Guid)), "Guid");
            Assert.False(_imgFmtConv.CanConvertFrom(null, typeof(object)), "object");
            Assert.False(_imgFmtConv.CanConvertFrom(null, typeof(int)), "int");

            Assert.True(_imgFmtConvFrmTD.CanConvertFrom(typeof(string)), "TD string (no context)");
            Assert.True(_imgFmtConvFrmTD.CanConvertFrom(null, typeof(string)), "TD string");
            Assert.False(_imgFmtConvFrmTD.CanConvertFrom(null, typeof(ImageFormat)), "TD ImageFormat");
            Assert.False(_imgFmtConvFrmTD.CanConvertFrom(null, typeof(Guid)), "TD Guid");
            Assert.False(_imgFmtConvFrmTD.CanConvertFrom(null, typeof(object)), "TD object");
            Assert.False(_imgFmtConvFrmTD.CanConvertFrom(null, typeof(int)), "TD int");
        }

        [Fact]
        public void TestCanConvertTo()
        {
            Assert.True(_imgFmtConv.CanConvertTo(typeof(string)), "string (no context)");
            Assert.True(_imgFmtConv.CanConvertTo(null, typeof(string)), "string");
            Assert.False(_imgFmtConv.CanConvertTo(null, typeof(ImageFormat)), "ImageFormat");
            Assert.False(_imgFmtConv.CanConvertTo(null, typeof(Guid)), "Guid");
            Assert.False(_imgFmtConv.CanConvertTo(null, typeof(object)), "object");
            Assert.False(_imgFmtConv.CanConvertTo(null, typeof(int)), "int");

            Assert.True(_imgFmtConvFrmTD.CanConvertTo(typeof(string)), "TD string (no context)");
            Assert.True(_imgFmtConvFrmTD.CanConvertTo(null, typeof(string)), "TD string");
            Assert.False(_imgFmtConvFrmTD.CanConvertTo(null, typeof(ImageFormat)), "TD ImageFormat");
            Assert.False(_imgFmtConvFrmTD.CanConvertTo(null, typeof(Guid)), "TD Guid");
            Assert.False(_imgFmtConvFrmTD.CanConvertTo(null, typeof(object)), "TD object");
            Assert.False(_imgFmtConvFrmTD.CanConvertTo(null, typeof(int)), "TD int");
        }

        [Fact]
        public void TestConvertFrom_ImageFormatToString()
        {
            Assert.Equal(_imageFmt, (ImageFormat)_imgFmtConv.ConvertFrom(null, CultureInfo.InvariantCulture, ImageFormat.Bmp.ToString()));
            Assert.Equal(_imageFmt, (ImageFormat)_imgFmtConvFrmTD.ConvertFrom(null, CultureInfo.InvariantCulture, ImageFormat.Bmp.ToString()));
        }

        [Fact]
        public void TestConvertFrom_ThrowsNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => _imgFmtConv.ConvertFrom(null, CultureInfo.InvariantCulture, ImageFormat.Bmp));
            Assert.Throws<NotSupportedException>(() => _imgFmtConv.ConvertFrom(null, CultureInfo.InvariantCulture, ImageFormat.Bmp.Guid));
            Assert.Throws<NotSupportedException>(() => _imgFmtConv.ConvertFrom(null, CultureInfo.InvariantCulture, new object()));
            Assert.Throws<NotSupportedException>(() => _imgFmtConv.ConvertFrom(null, CultureInfo.InvariantCulture, 10));

            Assert.Throws<NotSupportedException>(() => _imgFmtConvFrmTD.ConvertFrom(null, CultureInfo.InvariantCulture, ImageFormat.Bmp));
            Assert.Throws<NotSupportedException>(() => _imgFmtConvFrmTD.ConvertFrom(null, CultureInfo.InvariantCulture, ImageFormat.Bmp.Guid));
            Assert.Throws<NotSupportedException>(() => _imgFmtConvFrmTD.ConvertFrom(null, CultureInfo.InvariantCulture, new object()));
            Assert.Throws<NotSupportedException>(() => _imgFmtConvFrmTD.ConvertFrom(null, CultureInfo.InvariantCulture, 10));
        }

        private ImageFormat ConvertFromName(string imgFormatName)
        {
            return (ImageFormat)_imgFmtConvFrmTD.ConvertFrom(null, CultureInfo.InvariantCulture, imgFormatName);
        }

        [Fact]
        public void ConvertFrom_ShortName()
        {
            Assert.Equal(ImageFormat.Bmp, ConvertFromName("Bmp"));
            Assert.Equal(ImageFormat.Emf, ConvertFromName("Emf"));
            Assert.Equal(ImageFormat.Exif, ConvertFromName("Exif"));
            Assert.Equal(ImageFormat.Gif, ConvertFromName("Gif"));
            Assert.Equal(ImageFormat.Tiff, ConvertFromName("Tiff"));
            Assert.Equal(ImageFormat.Png, ConvertFromName("Png"));
            Assert.Equal(ImageFormat.MemoryBmp, ConvertFromName("MemoryBmp"));
            Assert.Equal(ImageFormat.Icon, ConvertFromName("Icon"));
            Assert.Equal(ImageFormat.Jpeg, ConvertFromName("Jpeg"));
            Assert.Equal(ImageFormat.Wmf, ConvertFromName("Wmf"));
#if NET
            Assert.Equal(ImageFormat.Heif, ConvertFromName("Heif"));
            Assert.Equal(ImageFormat.Webp, ConvertFromName("Webp"));
#endif
        }

        [Fact]
        public void ConvertFrom_LongName()
        {
            Guid testGuid = Guid.NewGuid();
            ImageFormat imageformat = ConvertFromName($"[ImageFormat: {testGuid}]");
            Assert.Equal(testGuid, imageformat.Guid);
        }

        [Fact]
        public void ConvertFrom_ThrowsFormatExceptionOnInvalidFormatString()
        {
            Assert.Throws<FormatException>(() => _imgFmtConv.ConvertFrom("System.Drawing.String"));
            Assert.Throws<FormatException>(() => _imgFmtConv.ConvertFrom(null, CultureInfo.InvariantCulture, "System.Drawing.String"));
            Assert.Throws<FormatException>(() => _imgFmtConv.ConvertFrom("[ImageFormat: abcdefgh-ijkl-mnop-qrst-uvwxyz012345]"));

            Assert.Throws<FormatException>(() => _imgFmtConvFrmTD.ConvertFrom("System.Drawing.String"));
            Assert.Throws<FormatException>(() => _imgFmtConvFrmTD.ConvertFrom(null, CultureInfo.InvariantCulture, "System.Drawing.String"));
            Assert.Throws<FormatException>(() => _imgFmtConvFrmTD.ConvertFrom("[ImageFormat: abcdefgh-ijkl-mnop-qrst-uvwxyz012345]"));
        }

        [Fact]
        public void TestConvertTo_String()
        {
            Assert.Equal(_imageFmtStr, (string)_imgFmtConv.ConvertTo(null, CultureInfo.InvariantCulture, _imageFmt, typeof(string)));
            Assert.Equal(_imageFmtStr, (string)_imgFmtConv.ConvertTo(_imageFmt, typeof(string)));

            Assert.Equal(_imageFmtStr, (string)_imgFmtConvFrmTD.ConvertTo(null, CultureInfo.InvariantCulture, _imageFmt, typeof(string)));
            Assert.Equal(_imageFmtStr, (string)_imgFmtConvFrmTD.ConvertTo(_imageFmt, typeof(string)));

            Assert.Equal(string.Empty, (string)_imgFmtConv.ConvertTo(null, typeof(string)));
            Assert.Equal(string.Empty, (string)_imgFmtConv.ConvertTo(null, CultureInfo.CreateSpecificCulture("ru-RU"), null, typeof(string)));

            Assert.Equal(string.Empty, (string)_imgFmtConvFrmTD.ConvertTo(null, typeof(string)));
            Assert.Equal(string.Empty, (string)_imgFmtConvFrmTD.ConvertTo(null, CultureInfo.CreateSpecificCulture("de-DE"), null, typeof(string)));
        }

        [Fact]
        public void TestConvertTo_ThrowsNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => _imgFmtConv.ConvertTo(null, CultureInfo.InvariantCulture, _imageFmt, typeof(ImageFormat)));
            Assert.Throws<NotSupportedException>(() => _imgFmtConv.ConvertTo(null, CultureInfo.InvariantCulture, _imageFmt, typeof(Guid)));
            Assert.Throws<NotSupportedException>(() => _imgFmtConv.ConvertTo(null, CultureInfo.InvariantCulture, _imageFmt, typeof(object)));
            Assert.Throws<NotSupportedException>(() => _imgFmtConv.ConvertTo(null, CultureInfo.InvariantCulture, _imageFmt, typeof(int)));

            Assert.Throws<NotSupportedException>(() => _imgFmtConvFrmTD.ConvertTo(null, CultureInfo.InvariantCulture, _imageFmt, typeof(ImageFormat)));
            Assert.Throws<NotSupportedException>(() => _imgFmtConvFrmTD.ConvertTo(null, CultureInfo.InvariantCulture, _imageFmt, typeof(Guid)));
            Assert.Throws<NotSupportedException>(() => _imgFmtConvFrmTD.ConvertTo(null, CultureInfo.InvariantCulture, _imageFmt, typeof(object)));
            Assert.Throws<NotSupportedException>(() => _imgFmtConvFrmTD.ConvertTo(null, CultureInfo.InvariantCulture, _imageFmt, typeof(int)));
        }

        [Fact]
        public void GetStandardValuesSupported()
        {
            Assert.True(_imgFmtConv.GetStandardValuesSupported(), "GetStandardValuesSupported()");
            Assert.True(_imgFmtConv.GetStandardValuesSupported(null), "GetStandardValuesSupported(null)");
        }

        private void CheckStandardValues(ICollection values)
        {
            bool memorybmp = false;
            bool bmp = false;
            bool emf = false;
            bool wmf = false;
            bool gif = false;
            bool jpeg = false;
            bool png = false;
            bool tiff = false;
            bool exif = false;
            bool icon = false;
#if NET
            bool heif = false;
            bool webp = false;
#endif

            foreach (ImageFormat iformat in values)
            {
                switch (iformat.Guid.ToString())
                {
                    case "b96b3caa-0728-11d3-9d7b-0000f81ef32e":
                        memorybmp = true;
                        break;
                    case "b96b3cab-0728-11d3-9d7b-0000f81ef32e":
                        bmp = true;
                        break;
                    case "b96b3cac-0728-11d3-9d7b-0000f81ef32e":
                        emf = true;
                        break;
                    case "b96b3cad-0728-11d3-9d7b-0000f81ef32e":
                        wmf = true;
                        break;
                    case "b96b3cb0-0728-11d3-9d7b-0000f81ef32e":
                        gif = true;
                        break;
                    case "b96b3cae-0728-11d3-9d7b-0000f81ef32e":
                        jpeg = true;
                        break;
                    case "b96b3caf-0728-11d3-9d7b-0000f81ef32e":
                        png = true;
                        break;
                    case "b96b3cb1-0728-11d3-9d7b-0000f81ef32e":
                        tiff = true;
                        break;
                    case "b96b3cb2-0728-11d3-9d7b-0000f81ef32e":
                        exif = true;
                        break;
                    case "b96b3cb5-0728-11d3-9d7b-0000f81ef32e":
                        icon = true;
                        break;
#if NET
                    case "b96b3cb6-0728-11d3-9d7b-0000f81ef32e":
                        heif = true;
                        break;
                    case "b96b3cb7-0728-11d3-9d7b-0000f81ef32e":
                        webp = true;
                        break;
#endif
                    default:
                        throw new InvalidOperationException($"Unknown GUID {iformat.Guid}.");
                }
            }
            Assert.True(memorybmp, "MemoryBMP");
            Assert.True(bmp, "Bmp");
            Assert.True(emf, "Emf");
            Assert.True(wmf, "Wmf");
            Assert.True(gif, "Gif");
            Assert.True(jpeg, "Jpeg");
            Assert.True(png, "Png");
            Assert.True(tiff, "Tiff");
            Assert.True(exif, "Exif");
            Assert.True(icon, "Icon");
#if NET
            Assert.True(heif, "Heif");
            Assert.True(webp, "Webp");
#endif
        }

        [Fact]
        public void GetStandardValues()
        {
            CheckStandardValues(_imgFmtConv.GetStandardValues());
            CheckStandardValues(_imgFmtConv.GetStandardValues(null));
        }
    }
}
