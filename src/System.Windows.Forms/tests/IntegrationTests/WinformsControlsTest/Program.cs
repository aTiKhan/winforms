﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;
using System.Windows.Forms;
using WinformsControlsTest;

// Set STAThread
Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
ApplicationConfiguration.Initialize();

Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

try
{
    MainForm form = new()
    {
        Icon = SystemIcons.GetStockIcon(StockIconId.Shield, StockIconOptions.SmallIcon)
    };

    Application.Run(form);
}
catch (Exception)
{
    Environment.Exit(-1);
}

Environment.Exit(0);

