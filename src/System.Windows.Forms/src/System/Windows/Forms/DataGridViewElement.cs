﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace System.Windows.Forms
{
    /// <summary>
    ///  Identifies an element in the dataGridView (base class for TCell, TBand, TRow, TColumn.
    /// </summary>
    public class DataGridViewElement
    {
        private DataGridView? _dataGridView;

        /// <summary>
        ///  Initializes a new instance of the <see cref="DataGridViewElement"/> class.
        /// </summary>
        public DataGridViewElement()
        {
            // These are subclasses of the DataGridViewElement for which we don't need to call the finalizer, because it's empty.
            // See https://github.com/dotnet/winforms/issues/6858.
            if (GetType() == typeof(DataGridViewBand) || GetType() == typeof(DataGridViewColumn) ||
                GetType() == typeof(DataGridViewButtonColumn) || GetType() == typeof(DataGridViewCheckBoxColumn) ||
                GetType() == typeof(DataGridViewComboBoxColumn) || GetType() == typeof(DataGridViewImageColumn) ||
                GetType() == typeof(DataGridViewLinkColumn) || GetType() == typeof(DataGridViewTextBoxColumn) ||
                GetType() == typeof(DataGridViewRow) || GetType() == typeof(DataGridViewCell) ||
                GetType() == typeof(DataGridViewButtonCell) || GetType() == typeof(DataGridViewCheckBoxCell) ||
                GetType() == typeof(DataGridViewComboBoxCell) || GetType() == typeof(DataGridViewHeaderCell) ||
                GetType() == typeof(DataGridViewColumnHeaderCell) || GetType() == typeof(DataGridViewTopLeftHeaderCell) ||
                GetType() == typeof(DataGridViewRowHeaderCell) || GetType() == typeof(DataGridViewImageCell) ||
                GetType() == typeof(DataGridViewLinkCell) || GetType() == typeof(DataGridViewTextBoxCell))
            {
                GC.SuppressFinalize(this);
            }

            State = DataGridViewElementStates.Visible;
        }

        internal DataGridViewElement(DataGridViewElement dgveTemplate)
        {
            // Selected and Displayed states are not inherited
            State = dgveTemplate.State & (DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.ResizableSet | DataGridViewElementStates.Visible);
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual DataGridViewElementStates State { get; internal set; }

        internal bool StateIncludes(DataGridViewElementStates elementState)
        {
            return (State & elementState) == elementState;
        }

        internal bool StateExcludes(DataGridViewElementStates elementState)
        {
            return (State & elementState) == 0;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridView? DataGridView
        {
            get => _dataGridView;
            internal set
            {
                if (_dataGridView != value)
                {
                    _dataGridView = value;
                    OnDataGridViewChanged();
                }
            }
        }

        protected virtual void OnDataGridViewChanged()
        {
        }

        protected void RaiseCellClick(DataGridViewCellEventArgs e)
        {
            _dataGridView?.OnCellClickInternal(e);
        }

        protected void RaiseCellContentClick(DataGridViewCellEventArgs e)
        {
            _dataGridView?.OnCellContentClickInternal(e);
        }

        protected void RaiseCellContentDoubleClick(DataGridViewCellEventArgs e)
        {
            _dataGridView?.OnCellContentDoubleClickInternal(e);
        }

        protected void RaiseCellValueChanged(DataGridViewCellEventArgs e)
        {
            _dataGridView?.OnCellValueChangedInternal(e);
        }

        protected void RaiseDataError(DataGridViewDataErrorEventArgs e)
        {
            _dataGridView?.OnDataErrorInternal(e);
        }

        protected void RaiseMouseWheel(MouseEventArgs e)
        {
            _dataGridView?.OnMouseWheelInternal(e);
        }
    }
}
