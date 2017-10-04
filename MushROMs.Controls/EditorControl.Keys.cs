using System;
using System.Drawing;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    partial class EditorControl
    {
        private bool hold;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            ProcessEditorKeyDown(e);
            base.OnKeyDown(e);
        }

        protected virtual void ProcessEditorKeyDown(KeyEventArgs e)
        {
            if (this.Editor == null || this.Editor.MapLength <= 0)
                return;

            if (e == null)
                throw new ArgumentNullException("e");

            // Get the active point.
            Point p = this.Editor.Active.RelativePoint;

            // Scroll the point if possible.
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (p.X > 0 || this.Editor.Selecting)
                        p.X--;
                    break;
                case Keys.Right:
                    if (p.X < this.Editor.MapSize.Width - 1 || this.Editor.Selecting)
                        p.X++;
                    break;
                case Keys.Up:
                    if (p.Y > 0 || this.Editor.Selecting)
                        p.Y--;
                    break;
                case Keys.Down:
                    if (p.Y < this.Editor.MapSize.Height - 1 || this.Editor.Selecting)
                        p.Y++;
                    break;
            }

            // Set the active tile if we moved it.
            if (p != this.Editor.Active.RelativePoint)
            {
                // Finish the selection if the modifier key is released.
                if (!e.Shift)
                    this.Editor.FinalizeSelection();

                this.Editor.SetActiveTile(p);
            }

            // Initialize selection if modifier key was pressed.
            if (!hold && e.Shift && !e.Alt)
            {
                hold = true;
                this.Editor.InitializeSelection();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            ProcessEditorKeyUp(e);
            base.OnKeyUp(e);
        }

        protected virtual void ProcessEditorKeyUp(KeyEventArgs e)
        {
            if (this.Editor == null || this.Editor.MapLength <= 0)
                return;

            if (e == null)
                throw new ArgumentNullException("e");

            // Finalize selection if modifier key was released.
            if (hold && !e.Shift && !e.Alt)
            {
                hold = false;
                this.Editor.FinalizeSelection();
            }
            else
                hold = e.Shift;
        }
    }
}