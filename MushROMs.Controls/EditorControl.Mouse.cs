using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    partial class EditorControl
    {
        private const MouseButtons FallbackSelectMouseButton = MouseButtons.Left;
        private const int MouseWheelScrollRows = 4;

        private bool sameTile;

        private MouseButtons selectRegionMouseButton;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected virtual bool IsSameTile
        {
            get
            {
                if (this.Editor == null)
                    return false;
                return this.sameTile && this.Editor.IsActiveInMap;
            }
        }

        [Browsable(true)]
        [Category("Editor")]
        [Description("The mouse buttons to be held when selecting a region.")]
        [DefaultValue(FallbackSelectMouseButton)]
        public MouseButtons SelectMouseButtons
        {
            get { return this.selectRegionMouseButton; }
            set { this.selectRegionMouseButton = value; }
        }

        protected override void OnInRangeMouseMove(MouseEventArgs e)
        {
            ProcessEditorMouseMove(e);

            base.OnInRangeMouseMove(e);
        }

        protected virtual void ProcessEditorMouseMove(MouseEventArgs e)
        {
            if (this.Editor == null || this.Editor.MapLength <= 0)
                return;

            if (e == null)
                throw new ArgumentNullException("e");

            // Determinde if the selection needs to be finalized.
            if (this.Editor.Selecting &&
                e.Button != this.selectRegionMouseButton)
                this.Editor.FinalizeSelection();

            // Get the current tile.
            Point tile = this.GetTileFromPixel(e.Location);

            // Determine whether we are still on the same tile.
            this.sameTile &= this.Editor.Active.RelativePoint == tile;

            // Update the active tile.
            this.Editor.SetActiveTile(tile);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            ProcessEditorMouseDown(e);
            base.OnMouseDown(e);
        }

        protected virtual void ProcessEditorMouseDown(MouseEventArgs e)
        {
            if (this.Editor == null || this.Editor.MapLength <= 0)
                return;

            if (e == null)
                throw new ArgumentNullException("e");

            // Update the active tile.
            this.Editor.SetActiveTile(this.GetTileFromPixel(e.Location));

            // Process possible selections.
            if (e.Button == this.selectRegionMouseButton)
            {
                // Create a new selection
                this.Editor.InitializeSelection();

                // Initialize this boolean to true for tile mouse click processing
                this.sameTile = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            ProcessEditorMouseUp(e);
            base.OnMouseUp(e);
        }

        protected virtual void ProcessEditorMouseUp(MouseEventArgs e)
        {
            if (this.Editor == null || this.Editor.MapLength <= 0)
                return;

            // The mouse button is no longer being held down.
            this.sameTile = false;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            // Determine whether the click event happened on the same tile.
            if (this.IsSameTile)
                OnTileMouseClick(e);

            base.OnMouseClick(e);
        }

        protected virtual void OnTileMouseClick(MouseEventArgs e)
        {
            if (TileMouseClick != null)
                TileMouseClick(this, e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            // Determine whether the click event happened on the same tile.
            if (this.IsSameTile)
                OnEditorMouseDoubleClick(e);

            base.OnMouseDoubleClick(e);
        }

        protected virtual void OnEditorMouseDoubleClick(MouseEventArgs e)
        {
            if (TileMouseDoubleClick != null)
                TileMouseDoubleClick(this, e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ProcessEditorMouseWheel(e);
            base.OnMouseWheel(e);
        }

        protected virtual void ProcessEditorMouseWheel(MouseEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (this.Editor != null)
                this.Editor.Scroll(new Size(0, (-MouseWheelScrollRows * e.Delta) / DesignerControl.MouseWheelThreshold));
        }

        [Browsable(true)]
        [Category("Editor")]
        [Description("Occurs when a tile is clicked by the mouse.")]
        public event MouseEventHandler TileMouseClick;

        [Browsable(true)]
        [Category("Editor")]
        [Description("Occurs when a tile is clicked by the mouse.")]
        public event MouseEventHandler TileMouseDoubleClick;
    }
}