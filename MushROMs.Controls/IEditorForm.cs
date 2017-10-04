using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    public interface IEditorForm
    {
        IEditorMasterForm EditorMasterForm { get; set; }
        IEditorContainer EditorContainer { get; set; }
        Editor ActiveEditor { get; }
        string Status { get; set; }

        event EventHandler Activated;
        event FormClosedEventHandler FormClosed;

        void Activate();
        void Show();
        void Close();

        void SaveEditor();
        void SaveEditorAs();
        void SaveEditor(string path);

        void Cut();
        void Copy();
        void Paste();
        void DeleteSelection();
    }
}
