namespace MushROMs.GenericEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.paletteControl1 = new MushROMs.Controls.SNES.PaletteEditor.PaletteControl();
            this.editor1 = new MushROMs.Editor(this.components);
            this.SuspendLayout();
            // 
            // paletteControl1
            // 
            this.paletteControl1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.paletteControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.paletteControl1.DoubleBuffered = false;
            this.paletteControl1.EditorHScrollBar = null;
            this.paletteControl1.EditorVScrollBar = null;
            this.paletteControl1.Location = new System.Drawing.Point(0, 0);
            this.paletteControl1.Margin = new System.Windows.Forms.Padding(0);
            this.paletteControl1.Name = "paletteControl1";
            this.paletteControl1.OverrideInputKeys = ((System.Collections.ObjectModel.Collection<System.Windows.Forms.Keys>)(resources.GetObject("paletteControl1.OverrideInputKeys")));
            this.paletteControl1.TabIndex = 0;
            // 
            // editor1
            // 
            this.editor1.TileSize = new System.Drawing.Size(8, 8);
            this.editor1.ViewSize = new System.Drawing.Size(16, 8);
            this.editor1.ZoomSize = new System.Drawing.Size(1, 1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 384);
            this.Controls.Add(this.paletteControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SNES.PaletteEditor.PaletteControl paletteControl1;
        private Editor editor1;


    }
}

