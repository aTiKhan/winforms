﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Forms.Design
{
    public partial class ControlDesigner
    {
        public class ControlDesignerAccessibleObject : AccessibleObject
        {
            private readonly ControlDesigner _designer;
            private readonly Control _control;
            private IDesignerHost? _host;
            private ISelectionService? _selectionService;

            public ControlDesignerAccessibleObject(ControlDesigner designer, Control control)
            {
                _designer = designer;
                _control = control;
            }

            public override Rectangle Bounds => _control.AccessibilityObject.Bounds;

            public override string? Description => _control.AccessibilityObject.Description;

            private IDesignerHost DesignerHost
                => _host ??= (IDesignerHost)_designer.GetService(typeof(IDesignerHost));

            public override string DefaultAction => string.Empty;

            public override string Name => _control.Name;

            public override AccessibleObject? Parent => _control.AccessibilityObject.Parent;

            public override AccessibleRole Role => _control.AccessibilityObject.Role;

            private ISelectionService SelectionService
                => _selectionService ??= _designer.GetService<ISelectionService>();

            public override AccessibleStates State
            {
                get
                {
                    AccessibleStates state = _control.AccessibilityObject.State;
                    ISelectionService s = SelectionService;
                    if (s is not null)
                    {
                        if (s.GetComponentSelected(_control))
                        {
                            state |= AccessibleStates.Selected;
                        }

                        if (s.PrimarySelection == _control)
                        {
                            state |= AccessibleStates.Focused;
                        }
                    }

                    return state;
                }
            }

            public override string? Value => _control.AccessibilityObject.Value;

            public override AccessibleObject? GetChild(int index)
            {
                Debug.WriteLineIf(
                    CompModSwitches.MSAA.TraceInfo,
                    $"ControlDesignerAccessibleObject.GetChild({index})");

                if (_control.AccessibilityObject.GetChild(index) is Control.ControlAccessibleObject childAccObj)
                {
                    AccessibleObject? cao = GetDesignerAccessibleObject(childAccObj);
                    if (cao is not null)
                    {
                        return cao;
                    }
                }

                return _control.AccessibilityObject.GetChild(index);
            }

            public override int GetChildCount() => _control.AccessibilityObject.GetChildCount();

            private AccessibleObject? GetDesignerAccessibleObject(Control.ControlAccessibleObject cao)
            {
                if (cao is null)
                {
                    return null;
                }

                if (DesignerHost.GetDesigner(cao.Owner) is ControlDesigner ctlDesigner)
                {
                    return ctlDesigner.AccessibilityObject;
                }

                return null;
            }

            public override AccessibleObject? GetFocused()
                => (State & AccessibleStates.Focused) != 0 ? this : base.GetFocused();

            public override AccessibleObject? GetSelected()
                => (State & AccessibleStates.Selected) != 0 ? this : base.GetFocused();

            public override AccessibleObject? HitTest(int x, int y) => _control.AccessibilityObject.HitTest(x, y);
        }
    }
}
