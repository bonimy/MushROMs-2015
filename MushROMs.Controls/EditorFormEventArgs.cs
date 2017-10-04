using System;

namespace MushROMs.Controls
{

    //public delegate void EditorFormEventHandler(object sender, EditorFormEventArgs e);

    public class EditorFormEventArgs : EventArgs
    {
        private readonly IEditorForm editorForm;

        public IEditorForm EditorForm
        {
            get { return this.editorForm; }
        }

        public EditorFormEventArgs(IEditorForm form)
        {
            this.editorForm = form;
        }
    }
}