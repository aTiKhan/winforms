﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Drawing.Interop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]

#if NET8_0_OR_GREATER
public
#else
internal
#endif
unsafe struct LOGFONT
{
    private const int LF_FACESIZE = 32;

    public int lfHeight;
    public int lfWidth;
    public int lfEscapement;
    public int lfOrientation;
    public int lfWeight;
    public byte lfItalic;
    public byte lfUnderline;
    public byte lfStrikeOut;
    public byte lfCharSet;
    public byte lfOutPrecision;
    public byte lfClipPrecision;
    public byte lfQuality;
    public byte lfPitchAndFamily;
    private fixed char _lfFaceName[LF_FACESIZE];

#if NET7_0_OR_GREATER
    [UnscopedRef]
#endif
    public Span<char> lfFaceName => MemoryMarshal.CreateSpan(ref _lfFaceName[0], LF_FACESIZE);

    internal string AsString()
#pragma warning disable format
        => $"lfHeight={lfHeight}, lfWidth={lfWidth}, lfEscapement={lfEscapement}, lfOrientation={lfOrientation
            }, lfWeight={lfWeight}, lfItalic={lfItalic}, lfUnderline={lfUnderline}, lfStrikeOut={lfStrikeOut
            }, lfCharSet={lfCharSet}, lfOutPrecision={lfOutPrecision}, lfClipPrecision={lfClipPrecision
            }, lfQuality={lfQuality}, lfPitchAndFamily={lfPitchAndFamily}, lfFaceName={lfFaceName}";
#pragma warning restore format
}
