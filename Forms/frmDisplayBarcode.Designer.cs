namespace GoogleAuthClone
{
    partial class frmDisplayBarcode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDisplayBarcode));
            this.pbxBarcode = new System.Windows.Forms.PictureBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.lblKey = new System.Windows.Forms.Label();
            this.lblClickReminder = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbxBarcode)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxBarcode
            // 
            this.pbxBarcode.Image = ((System.Drawing.Image)(resources.GetObject("pbxBarcode.Image")));
            this.pbxBarcode.Location = new System.Drawing.Point(106, 12);
            this.pbxBarcode.Name = "pbxBarcode";
            this.pbxBarcode.Size = new System.Drawing.Size(170, 170);
            this.pbxBarcode.TabIndex = 0;
            this.pbxBarcode.TabStop = false;
            this.pbxBarcode.Click += new System.EventHandler(this.pbxBarcode_Click);
            // 
            // lblNote
            // 
            this.lblNote.BackColor = System.Drawing.Color.Navy;
            this.lblNote.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblNote.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.ForeColor = System.Drawing.Color.White;
            this.lblNote.Location = new System.Drawing.Point(12, 228);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(354, 145);
            this.lblNote.TabIndex = 1;
            this.lblNote.Text = "Note:";
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNote.Click += new System.EventHandler(this.lblNote_Click);
            // 
            // lblKey
            // 
            this.lblKey.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKey.Location = new System.Drawing.Point(84, 195);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(216, 33);
            this.lblKey.TabIndex = 2;
            this.lblKey.Text = "xxxxxxxxxxxxxxxx";
            this.lblKey.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblKey.Click += new System.EventHandler(this.lblKey_Click);
            // 
            // lblClickReminder
            // 
            this.lblClickReminder.AutoSize = true;
            this.lblClickReminder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClickReminder.Location = new System.Drawing.Point(81, 379);
            this.lblClickReminder.Name = "lblClickReminder";
            this.lblClickReminder.Size = new System.Drawing.Size(219, 16);
            this.lblClickReminder.TabIndex = 3;
            this.lblClickReminder.Text = "Click anywhere to close the window.";
            this.lblClickReminder.Click += new System.EventHandler(this.lblClickReminder_Click);
            // 
            // frmDisplayBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(378, 404);
            this.ControlBox = false;
            this.Controls.Add(this.lblClickReminder);
            this.Controls.Add(this.lblKey);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.pbxBarcode);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmDisplayBarcode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ACCOUNT QR BARCODE";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmDisplayBarcode_Load);
            this.Shown += new System.EventHandler(this.frmDisplayBarcode_Shown);
            this.Click += new System.EventHandler(this.frmDisplayBarcode_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pbxBarcode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxBarcode;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.Label lblClickReminder;
    }
}