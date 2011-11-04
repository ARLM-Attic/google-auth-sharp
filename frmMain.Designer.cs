﻿namespace GoogleAuthClone
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblCode = new System.Windows.Forms.Label();
            this.tmrGetCodes = new System.Windows.Forms.Timer(this.components);
            this.butAdd = new System.Windows.Forms.Button();
            this.butDel = new System.Windows.Forms.Button();
            this.butEdit = new System.Windows.Forms.Button();
            this.butQuit = new System.Windows.Forms.Button();
            this.butCPP = new System.Windows.Forms.Button();
            this.lbAccounts = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pbTimeOut = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbTimeOut)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCode
            // 
            this.lblCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCode.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCode.Location = new System.Drawing.Point(12, 129);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(293, 62);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "==========";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCode.UseMnemonic = false;
            this.lblCode.DoubleClick += new System.EventHandler(this.lblCode_DoubleClick);
            // 
            // tmrGetCodes
            // 
            this.tmrGetCodes.Interval = 2000;
            this.tmrGetCodes.Tick += new System.EventHandler(this.tmrGetCodes_Tick);
            // 
            // butAdd
            // 
            this.butAdd.BackColor = System.Drawing.Color.Lime;
            this.butAdd.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.butAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butAdd.Location = new System.Drawing.Point(12, 222);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(85, 23);
            this.butAdd.TabIndex = 2;
            this.butAdd.Text = "ADD +";
            this.butAdd.UseVisualStyleBackColor = false;
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // butDel
            // 
            this.butDel.BackColor = System.Drawing.Color.Red;
            this.butDel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.butDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butDel.Location = new System.Drawing.Point(103, 222);
            this.butDel.Name = "butDel";
            this.butDel.Size = new System.Drawing.Size(85, 23);
            this.butDel.TabIndex = 3;
            this.butDel.Text = "DEL -";
            this.butDel.UseVisualStyleBackColor = false;
            this.butDel.Click += new System.EventHandler(this.butDel_Click);
            // 
            // butEdit
            // 
            this.butEdit.BackColor = System.Drawing.Color.Cyan;
            this.butEdit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.butEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butEdit.Location = new System.Drawing.Point(194, 222);
            this.butEdit.Name = "butEdit";
            this.butEdit.Size = new System.Drawing.Size(85, 23);
            this.butEdit.TabIndex = 4;
            this.butEdit.Text = "EDIT";
            this.butEdit.UseVisualStyleBackColor = false;
            this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
            // 
            // butQuit
            // 
            this.butQuit.BackColor = System.Drawing.Color.Red;
            this.butQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butQuit.Location = new System.Drawing.Point(285, 222);
            this.butQuit.Name = "butQuit";
            this.butQuit.Size = new System.Drawing.Size(88, 52);
            this.butQuit.TabIndex = 5;
            this.butQuit.Text = "QUIT";
            this.butQuit.UseVisualStyleBackColor = false;
            this.butQuit.Click += new System.EventHandler(this.butQuit_Click);
            // 
            // butCPP
            // 
            this.butCPP.BackColor = System.Drawing.Color.Cyan;
            this.butCPP.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.butCPP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butCPP.Location = new System.Drawing.Point(12, 251);
            this.butCPP.Name = "butCPP";
            this.butCPP.Size = new System.Drawing.Size(267, 23);
            this.butCPP.TabIndex = 6;
            this.butCPP.Text = "CHANGE PASSPHRASE";
            this.butCPP.UseVisualStyleBackColor = false;
            this.butCPP.Click += new System.EventHandler(this.butCPP_Click);
            // 
            // lbAccounts
            // 
            this.lbAccounts.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAccounts.FormattingEnabled = true;
            this.lbAccounts.ItemHeight = 16;
            this.lbAccounts.Location = new System.Drawing.Point(12, 12);
            this.lbAccounts.Name = "lbAccounts";
            this.lbAccounts.ScrollAlwaysVisible = true;
            this.lbAccounts.Size = new System.Drawing.Size(361, 116);
            this.lbAccounts.TabIndex = 7;
            this.lbAccounts.SelectedIndexChanged += new System.EventHandler(this.lbAccounts_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(361, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "DOUBLE-CLICK CODE FOR A QR BARCODE FOR YOUR PHONE";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbTimeOut
            // 
            this.pbTimeOut.BackColor = System.Drawing.Color.Blue;
            this.pbTimeOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTimeOut.Location = new System.Drawing.Point(310, 166);
            this.pbTimeOut.Name = "pbTimeOut";
            this.pbTimeOut.Size = new System.Drawing.Size(63, 25);
            this.pbTimeOut.TabIndex = 9;
            this.pbTimeOut.TabStop = false;
            this.pbTimeOut.Visible = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(310, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 32);
            this.label2.TabIndex = 10;
            this.label2.Text = "CODE TIMEOUT:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(386, 290);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pbTimeOut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbAccounts);
            this.Controls.Add(this.butCPP);
            this.Controls.Add(this.butQuit);
            this.Controls.Add(this.butEdit);
            this.Controls.Add(this.butDel);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.lblCode);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GoogleAuthenticator[CLONE]";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbTimeOut)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Button butAdd;
        private System.Windows.Forms.Button butDel;
        private System.Windows.Forms.Button butEdit;
        private System.Windows.Forms.Button butQuit;
        private System.Windows.Forms.Button butCPP;
        private System.Windows.Forms.ListBox lbAccounts;
        private System.Windows.Forms.Timer tmrGetCodes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbTimeOut;
        private System.Windows.Forms.Label label2;
    }
}

