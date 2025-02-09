﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.ComponentModel.Design
{
    public class DesignerActionTextItem : DesignerActionItem
    {
        public DesignerActionTextItem(string? displayName, string? category) : base(displayName, category, description: null)
        {
        }
    }
}
