using System;
using System.Collections.Generic;
using System.Text;

namespace MushROMs.Controls
{
    public interface IEditorControl : IEditorContainer
    {
        EditorHScrollBar EditorHScrollBar { get; set; }
        EditorVScrollBar EditorVScrollBar { get; set; }
    }
}
