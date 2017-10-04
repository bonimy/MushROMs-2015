using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MushROMs.Controls.Properties;

namespace MushROMs.Controls
{
    public class EditorMdiForm : Form, IEditorMasterForm
    {
        public event EventHandler<EditorFormEventArgs> EditorFormAdded;
        public event EventHandler<EditorFormEventArgs> EditorFormRemoved;
        public event EventHandler<EditorFormEventArgs> ActiveEditorFormChanged;

        private List<IEditorForm> editors;
        private _IEditorData copyData;
        private OpenFileDialog ofd;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ReadOnlyCollection<IEditorForm> Editors
        {
            get { return new ReadOnlyCollection<IEditorForm>(this.editors); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEditorForm CurrentEditor
        {
            get
            {
                for (int i = this.editors.Count; --i >= 0; )
                    if (this.ActiveMdiChild == this.editors[i])
                        return this.editors[i];
                return null;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public _IEditorData CopyData
        {
            get { return this.copyData; }
            set { this.copyData = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string Status
        {
            get { return null; }
            set { throw new NotImplementedException(); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected OpenFileDialog OpenFileDialog
        {
            get { return this.ofd; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual StringCollection RecentFiles
        {
            get { return null; }
        }

        protected override Size DefaultSize
        {
            get { return new Size(1000, 600); }
        }

        public EditorMdiForm()
        {
            this.IsMdiContainer = true;
            this.AllowDrop = true;
            this.KeyPreview = true;

            this.editors = new List<IEditorForm>();

            this.ofd = new OpenFileDialog();
            this.ofd.Multiselect = true;
        }

        public virtual void NewEditorForm()
        {
            throw new NotImplementedException();
        }

        public virtual void OpenEditorForm()
        {
            if (this.RecentFiles != null &&
                this.RecentFiles.Count > 0)
            {
                string path = this.RecentFiles[0];
                this.ofd.DefaultExt = Path.GetExtension(path);
                this.ofd.InitialDirectory = Path.GetDirectoryName(path);
                this.ofd.FileName = Path.GetFileName(this.RecentFiles[0]);
            }

            if (this.ofd.ShowDialog() == DialogResult.OK)
                for (int i = this.ofd.FileNames.Length; --i >= 0; )
                    OpenEditorForm(this.ofd.FileNames[i]);
        }

        public virtual void OpenEditorForm(string path)
        {
            // Determine whether the file exists first.
            if (!File.Exists(path))
            {
                //If the file does not exist, prompt to find it.
                StringBuilder sb = new StringBuilder(Resources.ErrorPathDoesNotExist);
                sb.Append("\n\n");
                sb.Append(path);
                sb.Append("\n\n");
                sb.Append(Resources.OptionLocateFile);

                if (RightToLeftAwareMessageBox.Show(sb.ToString(), Resources.DialogTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    OpenEditorForm();
                else
                    this.Status = Resources.StatusFileNotFound;
                return;
            }

            // Determine that this file is not already open.
            string fullpath = Path.GetFullPath(path);
            for (int i = this.editors.Count; --i >= 0; )
            {
                // Ignore new files.
                if (this.editors[i].ActiveEditor.FileDataType != FileDataType.FromFile)
                    continue;

                string fn = Path.GetFullPath(this.editors[i].ActiveEditor.FilePath);
                if (!String.IsNullOrEmpty(fn) && fullpath == Path.GetFullPath(fn))
                {
                    this.editors[i].Activate();
                    this.Status = Resources.StatusFileAlreadyOpen;
                    return;
                }
            }

            // Create a new editor
            EditorForm form = InitializeNewEditor();

            try // Everything loves to go wrong with file I/O.
            {
                form.ActiveEditor.Open(path);
            }
            catch (IOException ex)
            {
                // Let the user try to open the file again if it is an I/O error.
                if (RightToLeftAwareMessageBox.Show(ex.Message, Resources.DialogTitle, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    OpenEditorForm(path);
                return;
            }
#if !DEBUG
            catch (Exception ex)
            {
                // Catch any other errors and cancel opening.
                MessageBox.Show(ex.Message, Resources.DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
#endif
            OnEditorFormAdded(new EditorFormEventArgs(form));
        }

        protected virtual EditorForm InitializeNewEditor()
        {
            throw new NotImplementedException();
        }

        public virtual void SaveEditorForm()
        {
            if (this.CurrentEditor != null)
                this.CurrentEditor.SaveEditor();
        }

        public virtual void SaveEditorFormAs()
        {
            if (this.CurrentEditor != null)
                this.CurrentEditor.SaveEditorAs();
        }

        public virtual void SaveEditorForm(string path)
        {
            if (this.CurrentEditor != null)
                this.CurrentEditor.SaveEditor(path);
        }

        public virtual void SaveAllEditorForms()
        {
            for (int i = this.editors.Count; --i >= 0; )
                if (this.editors[i].ActiveEditor.FileDataType != FileDataType.NotAFile)
                    if (this.editors[i].ActiveEditor.History.Unsaved)
                        this.editors[i].SaveEditor();
        }

        public virtual void CloseAllEditorForms()
        {
            for (int i = this.editors.Count; --i >= 0; )
                if (this.editors[i].ActiveEditor.FileDataType != FileDataType.NotAFile)
                    this.editors[i].Close();
        }

        public virtual void RedrawAllEditorForms()
        {
            for (int i = this.editors.Count; --i >= 0; )
                this.editors[i].ActiveEditor.Redraw();
        }

        public virtual void Cut()
        {
            if (this.CurrentEditor != null)
                this.CurrentEditor.Cut();
        }

        public virtual void Copy()
        {
            if (this.CurrentEditor != null)
                this.CurrentEditor.Copy();
        }

        public virtual void Paste()
        {
            if (this.CurrentEditor != null)
                this.CurrentEditor.Paste();
        }

        public virtual void DeleteSelection()
        {
            if (this.CurrentEditor != null)
                this.CurrentEditor.DeleteSelection();
        }

        public virtual void GoToAddress()
        {
            throw new NotImplementedException();
        }

        public virtual void CustomizeSettings()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnEditorFormAdded(EditorFormEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            // Add the form if its not already in the list of editors.
            IEditorForm form = e.EditorForm;
            for (int i = this.editors.Count; --i >= 0; )
                if (this.editors[i] == form)
                    return;
            this.editors.Add(form);

            // Set properties to the form and show it.
            form.Activated += EditorForm_Activated;
            form.FormClosed += EditorForm_FormClosed;
            form.EditorMasterForm = this;
            form.Show();

            if (EditorFormAdded != null)
                EditorFormAdded(this, e);
        }

        protected virtual void OnEditorFormRemoved(EditorFormEventArgs e)
        {
            if (EditorFormRemoved != null)
                EditorFormRemoved(this, e);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            ProcessEditorDragEnter(drgevent);
            base.OnDragEnter(drgevent);
        }

        protected virtual void ProcessEditorDragEnter(DragEventArgs drgevent)
        {
            if (drgevent == null)
                throw new ArgumentNullException("drgevent");

            // By default, only allow file drops.
            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                drgevent.Effect = DragDropEffects.All;
                this.Status = Resources.StatusDragValid;
            }
            else
            {
                drgevent.Effect = DragDropEffects.None;
                this.Status = Resources.StatusDragInvalid;
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            ProcessEditorDragDrop(drgevent);
            base.OnDragDrop(drgevent);
        }

        protected virtual void ProcessEditorDragDrop(DragEventArgs drgevent)
        {
            if (drgevent == null)
                throw new ArgumentNullException("drgevent");

            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Invoke opening all of the files.
                string[] files = (string[])drgevent.Data.GetData(DataFormats.FileDrop, false);
                for (int i = 0; i < files.Length; i++)
                    OpenEditorForm(files[i]);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ProcessEditorClosing(e);
            base.OnFormClosing(e);
        }

        protected virtual void ProcessEditorClosing(FormClosingEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            List<string> names = new List<string>();
            for (int i = this.Editors.Count; --i >= 0; )
                if (this.Editors[i].ActiveEditor.History.Unsaved)
                    names.Add(this.Editors[i].ActiveEditor.Title);

            if (names.Count > 0)
            {
                using (UnsavedDialog dlg = new UnsavedDialog())
                {
                    dlg.PopulateListBox(names.ToArray());
                    DialogResult result = dlg.ShowDialog();
                    if (result == DialogResult.Yes)
                        SaveAllEditorForms();
                    else if (result == DialogResult.Cancel)
                        e.Cancel = true;
                }
            }
        }

        private void EditorForm_Activated(object sender, EventArgs e)
        {
            if (ActiveEditorFormChanged != null)
                ActiveEditorFormChanged(this, new EditorFormEventArgs((EditorForm)this.ActiveMdiChild));
        }

        private void EditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            EditorForm form = (EditorForm)sender;
            this.editors.Remove(form);
            OnEditorFormRemoved(new EditorFormEventArgs(form));
        }
    }
}