using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using MushROMs.Controls.Properties;

namespace MushROMs.Controls
{
    public class EditorForm : DesignerForm, IEditorForm
    {
        private Editor editor;
        private IEditorContainer mainEditorControl;

        private SaveFileDialog sfd;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEditorMasterForm EditorMasterForm
        {
            get { return (IEditorMasterForm)this.MdiParent; }
            set { this.MdiParent = (EditorMdiForm)value; }
        }

        [Browsable(true)]
        [Category("Editor")]
        [Description("The main editor control of this editor form.")]
        public IEditorContainer EditorContainer
        {
            get { return this.mainEditorControl; }
            set
            {
                if (this.EditorContainer == value)
                    return;

                this.mainEditorControl = value;

                if (this.EditorContainer != null)
                {
                    this.ActiveEditor = this.EditorContainer.Editor;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Editor ActiveEditor
        {
            get { return this.editor; }
            private set
            {
                if (this.EditorContainer == null)
                {
                    if (value != null || this.EditorContainer.Editor != value)
                        throw new ArgumentException(Resources.ErrorNullEditorContainer);
                }

                if (this.ActiveEditor == value)
                    return;

                if (this.ActiveEditor != null)
                    this.Text = Resources.DialogTitle;

                this.editor = value;

                if (this.ActiveEditor != null)
                    SetFormNameAsEditorTitle();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual _IEditorData CopyData
        {
            get { return this.EditorMasterForm.CopyData; }
            set { this.EditorMasterForm.CopyData = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected SaveFileDialog SaveFileDialog
        {
            get { return this.sfd; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string Status
        {
            get { return null; }
            set { throw new NotImplementedException(); }
        }

        public EditorForm()
        {
            this.sfd = new SaveFileDialog();
        }

        private void MainEditorControl_EditorChanged(object sender, EventArgs e)
        {
            if (this.EditorContainer != null)
                this.ActiveEditor = this.EditorContainer.Editor;
            else
                this.ActiveEditor = null;
        }

        protected virtual void SetFormNameAsEditorTitle()
        {
            // The title of the form is the file name of the path
            this.Text = this.ActiveEditor.Title;
        }

        public virtual void SaveEditor()
        {
            SaveEditor(this.ActiveEditor.FilePath);
        }

        public virtual void SaveEditorAs()
        {
            if (this.sfd.ShowDialog() == DialogResult.OK)
                SaveEditor(sfd.FileName);
        }

        public virtual void SaveEditor(string path)
        {
            // Prompt "Save as" method if path is empty.
            if (String.IsNullOrEmpty(path))
            {
                SaveEditorAs();
                return;
            }

            try // Everything loves to go wrong with file I/O.
            {
                this.ActiveEditor.Save(path);
            }
            catch (IOException ex)
            {
                // Let the user try to open the file again if it is an I/O error.
                if (RightToLeftAwareMessageBox.Show(ex.Message, Resources.DialogTitle, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    SaveEditor(path);
                return;
            }
#if !DEBUG
            catch (Exception ex)
            {
                // Catch any other errors and cancel saving.
                MessageBox.Show(ex.Message, Resources.DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
#endif
        }

        public virtual void Cut()
        {
            Copy();
            DeleteSelection();
        }

        public virtual void Copy()
        {
            this.CopyData = this.ActiveEditor.CreateCopy();
        }

        public virtual void Paste()
        {
            this.ActiveEditor.Paste(this.CopyData);
        }

        public virtual void DeleteSelection()
        {
            this.ActiveEditor.DeleteSelection();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ProcessEditorFormClosing(e);

            base.OnFormClosing(e);
        }

        protected virtual void ProcessEditorFormClosing(FormClosingEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (this.ActiveEditor != null)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    if (this.ActiveEditor.History.Unsaved)
                    {
                        DialogResult result = RightToLeftAwareMessageBox.Show(Resources.UnsavedFile, Resources.DialogTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                            SaveEditor();
                        else if (result == DialogResult.Cancel)
                            e.Cancel = true;
                    }
                }
            }
        }
    }
}