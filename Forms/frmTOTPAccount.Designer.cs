namespace GoogleAuthClone
{
    partial class frmTOTPAccount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTOTPAccount));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtSecret = new System.Windows.Forms.TextBox();
            this.lblSecret = new System.Windows.Forms.Label();
            this.rbB64 = new System.Windows.Forms.RadioButton();
            this.rbB32 = new System.Windows.Forms.RadioButton();
            this.lblAlgorithm = new System.Windows.Forms.Label();
            this.cbAlgorithm = new System.Windows.Forms.ComboBox();
            this.lblDigits = new System.Windows.Forms.Label();
            this.cbDigits = new System.Windows.Forms.ComboBox();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.txtPeriod = new System.Windows.Forms.TextBox();
            this.butBarcode = new System.Windows.Forms.Button();
            this.lblNameChecksum = new System.Windows.Forms.Label();
            this.lblNameChecksumValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Lime;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(12, 262);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(151, 31);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(169, 262);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 31);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(12, 28);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(301, 22);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.White;
            this.lblName.Location = new System.Drawing.Point(9, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(211, 16);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Name of this account or thing:";
            // 
            // txtSecret
            // 
            this.txtSecret.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSecret.Location = new System.Drawing.Point(12, 114);
            this.txtSecret.Name = "txtSecret";
            this.txtSecret.Size = new System.Drawing.Size(301, 22);
            this.txtSecret.TabIndex = 3;
            // 
            // lblSecret
            // 
            this.lblSecret.AutoSize = true;
            this.lblSecret.BackColor = System.Drawing.Color.Transparent;
            this.lblSecret.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSecret.ForeColor = System.Drawing.Color.White;
            this.lblSecret.Location = new System.Drawing.Point(9, 89);
            this.lblSecret.Name = "lblSecret";
            this.lblSecret.Size = new System.Drawing.Size(154, 16);
            this.lblSecret.TabIndex = 8;
            this.lblSecret.Text = "The shared secret is:";
            // 
            // rbB64
            // 
            this.rbB64.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbB64.AutoSize = true;
            this.rbB64.BackColor = System.Drawing.Color.DimGray;
            this.rbB64.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rbB64.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.rbB64.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.rbB64.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbB64.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbB64.ForeColor = System.Drawing.Color.White;
            this.rbB64.Location = new System.Drawing.Point(238, 83);
            this.rbB64.Name = "rbB64";
            this.rbB64.Size = new System.Drawing.Size(64, 27);
            this.rbB64.TabIndex = 2;
            this.rbB64.Text = "BASE64";
            this.rbB64.UseVisualStyleBackColor = false;
            // 
            // rbB32
            // 
            this.rbB32.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbB32.AutoSize = true;
            this.rbB32.BackColor = System.Drawing.Color.DimGray;
            this.rbB32.Checked = true;
            this.rbB32.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rbB32.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.rbB32.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.rbB32.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbB32.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbB32.ForeColor = System.Drawing.Color.White;
            this.rbB32.Location = new System.Drawing.Point(169, 83);
            this.rbB32.Name = "rbB32";
            this.rbB32.Size = new System.Drawing.Size(64, 27);
            this.rbB32.TabIndex = 1;
            this.rbB32.TabStop = true;
            this.rbB32.Text = "BASE32";
            this.rbB32.UseVisualStyleBackColor = false;
            // 
            // lblAlgorithm
            // 
            this.lblAlgorithm.AutoSize = true;
            this.lblAlgorithm.BackColor = System.Drawing.Color.Transparent;
            this.lblAlgorithm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlgorithm.ForeColor = System.Drawing.Color.White;
            this.lblAlgorithm.Location = new System.Drawing.Point(9, 148);
            this.lblAlgorithm.Name = "lblAlgorithm";
            this.lblAlgorithm.Size = new System.Drawing.Size(115, 16);
            this.lblAlgorithm.TabIndex = 11;
            this.lblAlgorithm.Text = "What algorithm:";
            // 
            // cbAlgorithm
            // 
            this.cbAlgorithm.BackColor = System.Drawing.Color.White;
            this.cbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAlgorithm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAlgorithm.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAlgorithm.ForeColor = System.Drawing.Color.Black;
            this.cbAlgorithm.Items.AddRange(new object[] {
            "SHA1",
            "SHA256",
            "SHA512",
            "MD5"});
            this.cbAlgorithm.Location = new System.Drawing.Point(12, 167);
            this.cbAlgorithm.Name = "cbAlgorithm";
            this.cbAlgorithm.Size = new System.Drawing.Size(96, 24);
            this.cbAlgorithm.TabIndex = 4;
            // 
            // lblDigits
            // 
            this.lblDigits.AutoSize = true;
            this.lblDigits.BackColor = System.Drawing.Color.Transparent;
            this.lblDigits.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigits.ForeColor = System.Drawing.Color.White;
            this.lblDigits.Location = new System.Drawing.Point(140, 148);
            this.lblDigits.Name = "lblDigits";
            this.lblDigits.Size = new System.Drawing.Size(179, 16);
            this.lblDigits.TabIndex = 13;
            this.lblDigits.Text = "Display how many digits:";
            // 
            // cbDigits
            // 
            this.cbDigits.BackColor = System.Drawing.Color.White;
            this.cbDigits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDigits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbDigits.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDigits.ForeColor = System.Drawing.Color.Black;
            this.cbDigits.Items.AddRange(new object[] {
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cbDigits.Location = new System.Drawing.Point(143, 167);
            this.cbDigits.Name = "cbDigits";
            this.cbDigits.Size = new System.Drawing.Size(67, 24);
            this.cbDigits.TabIndex = 5;
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.BackColor = System.Drawing.Color.Transparent;
            this.lblPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.ForeColor = System.Drawing.Color.White;
            this.lblPeriod.Location = new System.Drawing.Point(9, 206);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(271, 16);
            this.lblPeriod.TabIndex = 15;
            this.lblPeriod.Text = "Time period PINs are valid (seconds):";
            // 
            // txtPeriod
            // 
            this.txtPeriod.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPeriod.Location = new System.Drawing.Point(12, 225);
            this.txtPeriod.Name = "txtPeriod";
            this.txtPeriod.Size = new System.Drawing.Size(72, 22);
            this.txtPeriod.TabIndex = 7;
            this.txtPeriod.Text = "30";
            // 
            // butBarcode
            // 
            this.butBarcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.butBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butBarcode.Location = new System.Drawing.Point(12, 299);
            this.butBarcode.Name = "butBarcode";
            this.butBarcode.Size = new System.Drawing.Size(301, 29);
            this.butBarcode.TabIndex = 16;
            this.butBarcode.Text = "Use Barcode Image";
            this.butBarcode.UseVisualStyleBackColor = false;
            this.butBarcode.Click += new System.EventHandler(this.butBarcode_Click);
            // 
            // lblNameChecksum
            // 
            this.lblNameChecksum.AutoSize = true;
            this.lblNameChecksum.ForeColor = System.Drawing.Color.White;
            this.lblNameChecksum.Location = new System.Drawing.Point(12, 53);
            this.lblNameChecksum.Name = "lblNameChecksum";
            this.lblNameChecksum.Size = new System.Drawing.Size(134, 13);
            this.lblNameChecksum.TabIndex = 17;
            this.lblNameChecksum.Text = "As stored in Accounts.xml :";
            // 
            // lblNameChecksumValue
            // 
            this.lblNameChecksumValue.AutoSize = true;
            this.lblNameChecksumValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameChecksumValue.ForeColor = System.Drawing.Color.White;
            this.lblNameChecksumValue.Location = new System.Drawing.Point(152, 53);
            this.lblNameChecksumValue.Name = "lblNameChecksumValue";
            this.lblNameChecksumValue.Size = new System.Drawing.Size(140, 14);
            this.lblNameChecksumValue.TabIndex = 18;
            this.lblNameChecksumValue.Text = "[type a name first]";
            // 
            // frmTOTPAccount
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(325, 342);
            this.ControlBox = false;
            this.Controls.Add(this.lblNameChecksumValue);
            this.Controls.Add(this.lblNameChecksum);
            this.Controls.Add(this.butBarcode);
            this.Controls.Add(this.txtPeriod);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.cbDigits);
            this.Controls.Add(this.lblDigits);
            this.Controls.Add(this.cbAlgorithm);
            this.Controls.Add(this.lblAlgorithm);
            this.Controls.Add(this.rbB32);
            this.Controls.Add(this.rbB64);
            this.Controls.Add(this.lblSecret);
            this.Controls.Add(this.txtSecret);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTOTPAccount";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ACCOUNT INFORMATION";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmTOTPAccount_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtSecret;
        private System.Windows.Forms.Label lblSecret;
        private System.Windows.Forms.RadioButton rbB64;
        private System.Windows.Forms.RadioButton rbB32;
        private System.Windows.Forms.Label lblAlgorithm;
        private System.Windows.Forms.ComboBox cbAlgorithm;
        private System.Windows.Forms.Label lblDigits;
        private System.Windows.Forms.ComboBox cbDigits;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.TextBox txtPeriod;
        private System.Windows.Forms.Button butBarcode;
        private System.Windows.Forms.Label lblNameChecksum;
        private System.Windows.Forms.Label lblNameChecksumValue;
    }
}