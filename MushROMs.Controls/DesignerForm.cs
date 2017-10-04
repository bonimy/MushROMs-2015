using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    public partial class DesignerForm : Form
    {
        private const int WM_SIZING = 0x0214;
        private const int WM_ENTERSIZEMOVE = 0x231;
        private const int WM_EXITSIZEMOVE = 0x232;

        private Keys[] overrideInputKeys;

        private bool resizing;
        private Rectangle szRect;
        private SizingDirections szDir;

        [Browsable(true)]
        [Category("Editor")]
        [Description("Provides a collection of keys to override as input keys.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Keys[] OverrideInputKeys
        {
            get { return this.overrideInputKeys; }
            set { this.overrideInputKeys = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Resizing
        {
            get { return this.resizing; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SizingDirections SizingDirection
        {
            get { return this.szDir; }
        }

        public DesignerForm()
        {
            this.KeyPreview = true;

            // Initialize override input keys
            this.overrideInputKeys = new Keys[DesignerControl.FallbackOverrideInputKeys.Length];
            for (int i = DesignerControl.FallbackOverrideInputKeys.Length; --i >= 0; )
                this.overrideInputKeys[i] = DesignerControl.FallbackOverrideInputKeys[i];
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (this.overrideInputKeys != null)
                for (int i = this.overrideInputKeys.Length; --i >= 0; )
                    if (this.overrideInputKeys[i] == keyData)
                        return true;

            return base.IsInputKey(keyData);
        }

        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (this.overrideInputKeys != null)
                for (int i = this.overrideInputKeys.Length; --i >= 0; )
                    if (this.overrideInputKeys[i] == keyData)
                        return false;

            return base.ProcessDialogKey(keyData);
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            this.resizing = true;
            base.OnResizeBegin(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            this.resizing = false;
            base.OnResizeEnd(e);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_SIZING:
                    Rectangle r = GetRectangleFromPRECT(m.LParam);
                    SetSizingDirection(r);
                    SetSizingRectangle(r);
                    SetPRECTFromRectangle(this.szRect, m.LParam);
                    break;
                case WM_EXITSIZEMOVE:
                    this.szRect = Rectangle.Empty;
                    this.szDir = SizingDirections.None;
                    break;
            }
            base.DefWndProc(ref m);
        }

        private void SetSizingDirection(Rectangle r)
        {
            this.szDir = SizingDirections.None;
            if (this.szRect == Rectangle.Empty)
                return;

            if (r.Left != this.szRect.Left)
                this.szDir |= SizingDirections.Left;

            if (r.Top != this.szRect.Top)
                this.szDir |= SizingDirections.Top;

            if (r.Right != this.szRect.Right)
                this.szDir |= SizingDirections.Right;

            if (r.Bottom != this.szRect.Bottom)
                this.szDir |= SizingDirections.Bottom;
        }

        protected virtual void SetSizingRectangle(Rectangle rect)
        {
            this.szRect = rect;
        }

        private static Rectangle GetRectangleFromPRECT(IntPtr pRECT)
        {
            unsafe
            {
                int* rect = (int*)pRECT;
                return Rectangle.FromLTRB(rect[0], rect[1], rect[2], rect[3]);
            }
        }

        private static void SetPRECTFromRectangle(Rectangle r, IntPtr pRECT)
        {
            unsafe
            {
                int* rect = (int*)pRECT;
                rect[0] = r.Left;
                rect[1] = r.Top;
                rect[2] = r.Right;
                rect[3] = r.Bottom;
            }
        }
    }
}