namespace GoogleAuthClone
{
    partial class frmGetBarcode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGetBarcode));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pbxBarcode = new System.Windows.Forms.PictureBox();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.ofdFile = new System.Windows.Forms.OpenFileDialog();
            this.butBrowse = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbxBarcode)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(307, 361);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 30);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Lime;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(134, 361);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(151, 30);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pbxBarcode
            // 
            this.pbxBarcode.BackColor = System.Drawing.Color.White;
            this.pbxBarcode.Image = ((System.Drawing.Image)(resources.GetObject("pbxBarcode.Image")));
            this.pbxBarcode.Location = new System.Drawing.Point(149, 12);
            this.pbxBarcode.Name = "pbxBarcode";
            this.pbxBarcode.Size = new System.Drawing.Size(289, 266);
            this.pbxBarcode.TabIndex = 12;
            this.pbxBarcode.TabStop = false;
            // 
            // txtFilename
            // 
            this.txtFilename.Location = new System.Drawing.Point(12, 286);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(532, 20);
            this.txtFilename.TabIndex = 13;
            // 
            // ofdFile
            // 
            this.ofdFile.FileName = "openFileDialog1";
            // 
            // butBrowse
            // 
            this.butBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butBrowse.Location = new System.Drawing.Point(550, 284);
            this.butBrowse.Name = "butBrowse";
            this.butBrowse.Size = new System.Drawing.Size(33, 23);
            this.butBrowse.TabIndex = 14;
            this.butBrowse.Text = "...";
            this.butBrowse.UseVisualStyleBackColor = true;
            this.butBrowse.Click += new System.EventHandler(this.butBrowse_Click);
            // 
            // lblResult
            // 
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.ForeColor = System.Drawing.Color.White;
            this.lblResult.Location = new System.Drawing.Point(12, 309);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(571, 22);
            this.lblResult.TabIndex = 15;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.ForeColor = System.Drawing.Color.White;
            this.lblNote.Location = new System.Drawing.Point(103, 343);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(385, 15);
            this.lblNote.TabIndex = 17;
            this.lblNote.Text = "Note:  File must contain NO MORE THAN ONE (1) BARCODE!";
            // 
            // frmGetBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(593, 399);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.butBrowse);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.pbxBarcode);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGetBarcode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmGetBarcode";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmGetBarcode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxBarcode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.PictureBox pbxBarcode;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.OpenFileDialog ofdFile;
        private System.Windows.Forms.Button butBrowse;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblNote;

    }
}