using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    partial class DesignerControl
    {
        internal static readonly Keys[] FallbackOverrideInputKeys = new Keys[] {
            Keys.Up,    Keys.Up    | Keys.Shift, Keys.Up    | Keys.Control, Keys.Up    | Keys.Shift | Keys.Control,
            Keys.Left,  Keys.Left  | Keys.Shift, Keys.Left  | Keys.Control, Keys.Left  | Keys.Shift | Keys.Control,
            Keys.Down,  Keys.Down  | Keys.Shift, Keys.Down  | Keys.Control, Keys.Down  | Keys.Shift | Keys.Control,
            Keys.Right, Keys.Right | Keys.Shift, Keys.Right | Keys.Control, Keys.Right | Keys.Shift | Keys.Control };

        private static Keys currentKeys;
        private static Keys previousKeys;
        private static Keys activeKeys;

        private Collection<Keys> overrideInputKeys;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Keys CurrentKeys
        {
            get { return DesignerControl.currentKeys; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Keys PreviousKeys
        {
            get { return DesignerControl.previousKeys; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Keys ActiveKeys
        {
            get { return DesignerControl.activeKeys; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool ControlKeyHeld
        {
            get { return (Control.ModifierKeys & Keys.Control) == Keys.Control; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool ShiftKeyHeld
        {
            get { return (Control.ModifierKeys & Keys.Shift) == Keys.Shift; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool AltKeyHeld
        {
            get { return (Control.ModifierKeys & Keys.Alt) == Keys.Alt; }
        }

        [Browsable(true)]
        [Category("Editor")]
        [Description("Provides a collection of keys to override as input keys.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Collection<Keys> OverrideInputKeys
        {
            get { return this.overrideInputKeys; }
            set { this.overrideInputKeys = value; }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            // Determine which additional keys will be input keys.
            if (this.OverrideInputKeys != null)
                for (int i = this.OverrideInputKeys.Count; --i >= 0; )
                    if (this.OverrideInputKeys[i] == keyData)
                        return true;

            return base.IsInputKey(keyData);
        }

        [UIPermission(SecurityAction.LinkDemand, Window=UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (this.OverrideInputKeys != null)
                for (int i = this.OverrideInputKeys.Count; --i >= 0; )
                    if (this.OverrideInputKeys[i] == keyData)
                        return false;

            return base.ProcessDialogKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            // Update current, previous, and active keys.
            DesignerControl.previousKeys = DesignerControl.currentKeys;
            DesignerControl.currentKeys = e.KeyCode;
            DesignerControl.activeKeys = DesignerControl.currentKeys & ~DesignerControl.previousKeys;

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            // Update current, previous, and active keys.
            DesignerControl.previousKeys = DesignerControl.currentKeys;
            DesignerControl.currentKeys &= ~e.KeyCode;
            DesignerControl.activeKeys = Keys.None;

            base.OnKeyUp(e);
        }
    }
}
