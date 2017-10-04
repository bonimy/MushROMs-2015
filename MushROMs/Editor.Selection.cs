using System;
using System.ComponentModel;

namespace MushROMs
{
    partial class Editor
    {
        private const SelectType FallbackSelectType = SelectType.Rectangular;

        private IndexPoint active;

        private bool selecting;
        private SelectType selectType;
        private Selection_ selection;
        private bool[] selectedTiles;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IndexPoint Active
        {
            get { return this.active; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool IsActiveInMap
        {
            get { return this.Active.Index >= 0 && this.Active.Index < this.MapWidth; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Selecting
        {
            get { return this.selecting; }
        }

        [Category("Selection")]
        [DefaultValue(FallbackSelectType)]
        public SelectType SelectType
        {
            get { return this.selectType; }
            set
            {
                if (Helper.IsEnumValid((int)value, (int)SelectType.Single, (int)SelectType.Square))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(SelectType));
                this.selectType = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Selection_ Selection
        {
            get { return this.selection; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool[] SelectedTiles
        {
            get { return this.selectedTiles; }
        }

        [Category("Selection")]
        [Description("Occurs when the selection is initialized.")]
        public event EventHandler SelectionInitialized;

        [Category("Selection")]
        [Description("Occurs when the selection is modified.")]
        public event EventHandler SelectionModified;

        [Category("Selection")]
        [Description("Occurs when the selection is finialized.")]
        public event EventHandler SelectionFinalized;

        [Category("Selection")]
        [Description("Occurs when the selected tiles change.")]
        public event EventHandler SelectedTilesChanged;

        [Category("Selection")]
        [Description("Occurs when the active index changes.")]
        public event EventHandler ActiveIndexChanged
        {
            add { this.active.IndexChanged += value; }
            remove { this.active.IndexChanged -= value; }
        }

        private void Active_IndexChanged(object sender, EventArgs e)
        {
            UpdateSelection();
        }

        public virtual void InitializeSelection()
        {
            this.selection = new Selection_(this.active);
            OnSelectionInitialized(EventArgs.Empty);
        }

        protected virtual void OnSelectionInitialized(EventArgs e)
        {
            this.selecting = true;
            SetSelectedTiles();
            if (SelectionInitialized != null)
                SelectionInitialized(this, e);
        }

        protected virtual void UpdateSelection()
        {
            // Make sure we are selecting.
            if (this.Selecting)
            {
                if (this.SelectType != SelectType.Single)
                {
                    // Get the tile to select to
                    IndexPoint ip = this.Active.Copy();

                    // Make a square selection if necessary.
                    if (this.SelectType == SelectType.Square)
                    {
                        // Get the vertical parameters
                        int firstX = this.Selection.First.RelativeX;
                        int width = ip.RelativeX - firstX;
                        int xSign = Math.Sign(width);
                        if (xSign == 0)
                            xSign = 1;
                        width = Math.Abs(width);

                        // Get the horizontal parameters
                        int firstY = this.Selection.First.RelativeY;
                        int height = ip.RelativeY - firstY;
                        int ySign = Math.Sign(height);
                        if (ySign == 0)
                            ySign = -1;
                        height = Math.Abs(height);

                        // Make a square selection from the largest value
                        if (height > width)
                            ip.RelativeX = firstX + xSign * height;
                        else
                            ip.RelativeY = firstY + ySign * width;
                    }

                    // Update the selection.
                    this.Selection.Update(ip);
                }
                else
                    InitializeSelection();

                OnSelectionModified(EventArgs.Empty);
            }
        }

        protected virtual void OnSelectionModified(EventArgs e)
        {
            SetSelectedTiles();
            if (SelectionModified != null)
                SelectionModified(this, e);
        }

        public virtual void FinalizeSelection()
        {
            OnSelectionFinalized(EventArgs.Empty);
        }

        protected virtual void OnSelectionFinalized(EventArgs e)
        {
            this.selecting = false;
            if (SelectionFinalized != null)
                SelectionFinalized(this, e);
        }

        protected virtual void SetSelectedTiles()
        {
            int mapL = this.MapLength;
            int index = this.Selection.Min.Address;
            int height = this.Selection.Height;
            int width = this.Selection.Width;
            int container = this.Selection.ContainerWidth;

            // Create the selected tiles array.
            unsafe
            {
                fixed (bool* tiles = this.SelectedTiles)
                {
                    // Clear the selection map.
                    for (int i = mapL; --i >= 0; )
                        tiles[i] = false;

                    // Iterate across every row.
                    for (int y = height; --y >= 0; )
                    {
                        // Get the start index for this row.
                        int j = index + (y * container);

                        // Iterate across every selected tile for the row.
                        for (int x = width; --x >= 0; j++)
                            if (j >= 0 && j < mapL)
                                tiles[j] = true;
                    }
                }
            }

            OnSelectedTilesChanged(EventArgs.Empty);
        }

        protected virtual void OnSelectedTilesChanged(EventArgs e)
        {
            if (SelectedTilesChanged != null)
                SelectedTilesChanged(this, e);

            Redraw();
        }
    }
}