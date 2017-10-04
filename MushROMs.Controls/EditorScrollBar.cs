using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    public abstract class EditorScrollBar : ScrollBar
    {
        internal const int DIR_HORZ = 0x0000;
        internal const int DIR_VERT = 0x0001;

        private IEditorControl editorControl;

        [Browsable(true)]
        [Category("Editor")]
        [Description("The editor associated with this scroll bar.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public IEditorControl EditorControl
        {
            get { return this.editorControl; }
            set
            {
                if (this.EditorControl == value)
                    return;

                if (this.Editor != null)
                {
                    this.Editor.ZeroAddressChanged -= Editor_ZeroAddressChanged;
                    this.Editor.MapReset -= Editor_Reset;

                    if (this.Direction == DIR_HORZ)
                        this.Editor.HScrollEndChanged -= Editor_Reset;
                    else
                        this.Editor.VScrollEndChanged -= Editor_Reset;
                }

                // Set the editor control.
                this.editorControl = value;

                // Attach the events to the new editor if it exists.
                if (this.Editor != null)
                {
                    this.Editor.ZeroAddressChanged += Editor_ZeroAddressChanged;
                    this.Editor.MapReset += Editor_Reset;

                    if (this.Direction == DIR_HORZ)
                        this.Editor.HScrollEndChanged += Editor_Reset;
                    else
                        this.Editor.VScrollEndChanged += Editor_Reset;
                }

                // Set the properties of the scroll bar for the new editor.
                Reset();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Editor Editor
        {
            get { return this.editorControl.Editor; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected abstract int Direction { get; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ScrollEnd ScrollEnd
        {
            get
            {
                if (this.Direction == DIR_HORZ)
                    return this.Editor.HScrollEnd;
                else
                    return this.Editor.VScrollEnd;
            }
            set
            {
                if (this.Direction == DIR_HORZ)
                    this.Editor.HScrollEnd = value;
                else
                    this.Editor.VScrollEnd = value;
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private int ViewN
        {
            get
            {
                if (this.Direction == DIR_HORZ)
                    return this.Editor.ViewSize.Width;
                else
                    return this.Editor.ViewSize.Height;
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private int MapN
        {
            get
            {
                if (this.Direction == DIR_HORZ)
                    return this.Editor.MapSize.Width;
                else
                    return this.Editor.MapSize.Height;
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private int ZeroN
        {
            get
            {
                if (this.Direction == DIR_HORZ)
                    return this.Editor.Zero.X;
                else
                    return this.Editor.Zero.Y;
            }
            set
            {
                if (this.Direction == DIR_HORZ)
                    this.Editor.Zero.X = value;
                else
                    this.Editor.Zero.Y = value;
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private int NScrollLimit
        {
            get
            {
                if (this.Direction == DIR_HORZ)
                    return this.Editor.HScrollLimit;
                else
                    return this.Editor.VScrollLimit;
            }
        }

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= this.Direction;
                return cp;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Value
        {
            get { return base.Value; }
            protected set { base.Value = value; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int SmallChange
        {
            get { return base.SmallChange; }
            protected set { base.SmallChange = value; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int LargeChange
        {
            get { return base.LargeChange; }
            protected set { base.LargeChange = value; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Minimum
        {
            get { return base.Minimum; }
            protected set { base.Minimum = value; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Maximum
        {
            get { return base.Maximum; }
            protected set { base.Maximum = value; }
        }

        protected EditorScrollBar()
        {
            Reset();
        }

        public void Reset()
        {
            this.Enabled = this.Editor != null && this.ViewN < this.MapN + this.NScrollLimit;

            // Disable the scroll bar if no editor is present.
            if (!this.Enabled)
            {
                this.Enabled = false;
                this.SmallChange =
                this.LargeChange =
                this.Minimum =
                this.Minimum =
                this.Value = 1;
            }
            else
                SetScrollParameters();
        }

        protected virtual void SetScrollParameters()
        {
            // We have to check for Range > 0 or else visual studio throws designer errors
            if (this.Enabled && this.Editor != null)
            {
                this.SmallChange = 1;
                this.LargeChange = this.ViewN;
                this.Minimum = 0;
                this.Maximum = this.MapN + this.NScrollLimit - 1;
                this.Value = this.ZeroN;
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            ScrollEditor(se);
            base.OnScroll(se);
        }

        protected virtual void ScrollEditor(ScrollEventArgs se)
        {
            if (se == null)
                throw new ArgumentNullException("se");

            if (this.Editor != null)
                if (se.NewValue != se.OldValue)
                    this.ZeroN = se.NewValue;
        }

        private void Editor_ZeroAddressChanged(object sender, EventArgs e)
        {
            SetScrollParameters();
        }

        private void Editor_Reset(object sender, EventArgs e)
        {
            Reset();
        }
    }
}