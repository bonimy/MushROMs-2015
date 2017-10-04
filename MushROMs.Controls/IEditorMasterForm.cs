using System;
using System.Collections.ObjectModel;

namespace MushROMs.Controls
{
    public interface IEditorMasterForm
    {
        ReadOnlyCollection<IEditorForm> Editors { get; }
        IEditorForm CurrentEditor { get; }

        _IEditorData CopyData { get; set; }

        string Status { get; set; }

        event EventHandler<EditorFormEventArgs> ActiveEditorFormChanged;
        event EventHandler<EditorFormEventArgs> EditorFormAdded;
        event EventHandler<EditorFormEventArgs> EditorFormRemoved;
    }
}
