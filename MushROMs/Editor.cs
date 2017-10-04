using System.ComponentModel;
using System.IO;
using System.Timers;

namespace MushROMs
{
    [DefaultEvent("WritePixels")]
    public partial class Editor
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Title
        {
            get
            {
                string title = this.FileDataType != FileDataType.ProgramCreated ? this.Untitled : Path.GetFileName(this.FilePath);
                if (this.History.Unsaved)
                    title += UnsavedNotification;
                return title;
            }
        }

        public Editor()
        {
            // Initialize the component container.
            this.components = new Container();

            // Initialize the data.
            this.data = new byte[0];

            // Initialize the zero point.
            this.zero = new ZeroPoint_(this);
            this.Zero.AddressChanged += Zero_AddressChanged;

            // Initialize the view region.
            this.viewW = FallbackViewWidth;
            this.viewH = FallbackViewHeight;

            // Initialize the tile size.
            this.tileW = FallbackTileWidth;
            this.tileH = FallbackTileHeight;
            this.zoomW = FallbackZoomWidth;
            this.zoomH = FallbackZoomHeight;

            // Initialize the active index.
            this.active = new IndexPoint(this.Zero);
            this.Active.IndexChanged += Active_IndexChanged;

            // Initialize scrolling parameters.
            this.scroll = FallbackCanScrollSelection;
            this.selectType = FallbackSelectType;
            this.hScrollEnd = FallbackHScrollEnd;
            this.vScrollEnd = FallbackVScrollEnd;

            // Assume there is no file data type.
            this.fileDataType = FileDataType.NotAFile;

            // Initialize the file path to an empty string.
            this.fp = string.Empty;

            // Initialize the undo/redo data.
            this.history = new History<_IEditorData>();
            this.history.Undo += History_Undo;
            this.history.Redo += History_Redo;

            // Initialize timer.
            this.timer = new Timer(FallbackRedrawSleepTime);
            this.timer.Elapsed += Timer_Elapsed;
            this.components.Add(this.timer);
        }

        public Editor(IContainer container)
            : this()
        {
            if (container == null)
                return;

            container.Add(this);
        }
    }
}