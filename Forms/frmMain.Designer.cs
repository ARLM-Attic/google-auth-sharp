namespace GoogleAuthClone
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
            this.tmrGetCodes = new System.Windows.Forms.Timer(this.components);
            this.lbAccounts = new System.Windows.Forms.ListBox();
            this.lblGetCode = new System.Windows.Forms.Label();
            this.pbTimeOut = new System.Windows.Forms.PictureBox();
            this.lblTimeOut = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblCopyClipboard = new System.Windows.Forms.Label();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileChangePassphrase = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileExportEncrypted = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileExportPlain = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileImport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileImportLegacy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileImportXML = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPinCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPinBarcode = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWindowAlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgSaveBackup = new System.Windows.Forms.SaveFileDialog();
            this.dlgLoadXML = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pbTimeOut)).BeginInit();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrGetCodes
            // 
            this.tmrGetCodes.Interval = 2000;
            this.tmrGetCodes.Tick += new System.EventHandler(this.tmrGetCodes_Tick);
            // 
            // lbAccounts
            // 
            this.lbAccounts.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAccounts.FormattingEnabled = true;
            this.lbAccounts.ItemHeight = 16;
            this.lbAccounts.Location = new System.Drawing.Point(12, 36);
            this.lbAccounts.Name = "lbAccounts";
            this.lbAccounts.ScrollAlwaysVisible = true;
            this.lbAccounts.Size = new System.Drawing.Size(361, 116);
            this.lbAccounts.TabIndex = 1;
            this.lbAccounts.SelectedIndexChanged += new System.EventHandler(this.lbAccounts_SelectedIndexChanged);
            // 
            // lblGetCode
            // 
            this.lblGetCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGetCode.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGetCode.ForeColor = System.Drawing.Color.White;
            this.lblGetCode.Location = new System.Drawing.Point(13, 210);
            this.lblGetCode.Name = "lblGetCode";
            this.lblGetCode.Size = new System.Drawing.Size(361, 20);
            this.lblGetCode.TabIndex = 3;
            this.lblGetCode.Text = "CLICK *HERE* FOR A QR BARCODE FOR YOUR PHONE";
            this.lblGetCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGetCode.Click += new System.EventHandler(this.lblGetCode_Click);
            // 
            // pbTimeOut
            // 
            this.pbTimeOut.BackColor = System.Drawing.Color.Blue;
            this.pbTimeOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTimeOut.Location = new System.Drawing.Point(285, 190);
            this.pbTimeOut.Name = "pbTimeOut";
            this.pbTimeOut.Size = new System.Drawing.Size(88, 17);
            this.pbTimeOut.TabIndex = 9;
            this.pbTimeOut.TabStop = false;
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOut.ForeColor = System.Drawing.Color.White;
            this.lblTimeOut.Location = new System.Drawing.Point(285, 155);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(88, 32);
            this.lblTimeOut.TabIndex = 10;
            this.lblTimeOut.Text = "No Code Selected";
            this.lblTimeOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCode.Font = new System.Drawing.Font("Courier New", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(12, 158);
            this.txtCode.MaxLength = 10;
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(267, 49);
            this.txtCode.TabIndex = 2;
            this.txtCode.Text = "==========";
            this.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCode.WordWrap = false;
            this.txtCode.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtCode_MouseDoubleClick);
            // 
            // lblCopyClipboard
            // 
            this.lblCopyClipboard.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyClipboard.ForeColor = System.Drawing.Color.White;
            this.lblCopyClipboard.Location = new System.Drawing.Point(13, 230);
            this.lblCopyClipboard.Name = "lblCopyClipboard";
            this.lblCopyClipboard.Size = new System.Drawing.Size(361, 20);
            this.lblCopyClipboard.TabIndex = 4;
            this.lblCopyClipboard.Text = "DOUBLE-CLICK THE CODE TO COPY IT TO THE CLIPBOARD";
            this.lblCopyClipboard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuPin,
            this.windowToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuMain.Size = new System.Drawing.Size(386, 24);
            this.menuMain.TabIndex = 0;
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileSave,
            this.menuFileAdd,
            this.menuFileEdit,
            this.menuFileDelete,
            this.menuSeparator1,
            this.menuFileChangePassphrase,
            this.menuSeparator3,
            this.menuFileExport,
            this.menuFileImport,
            this.menuSeparator2,
            this.menuFileQuit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuFileSave
            // 
            this.menuFileSave.Name = "menuFileSave";
            this.menuFileSave.Size = new System.Drawing.Size(285, 22);
            this.menuFileSave.Text = "&Save Accounts";
            this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
            // 
            // menuFileAdd
            // 
            this.menuFileAdd.Name = "menuFileAdd";
            this.menuFileAdd.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.menuFileAdd.Size = new System.Drawing.Size(285, 22);
            this.menuFileAdd.Text = "&Add Account";
            this.menuFileAdd.Click += new System.EventHandler(this.menuFileAdd_Click);
            // 
            // menuFileEdit
            // 
            this.menuFileEdit.Name = "menuFileEdit";
            this.menuFileEdit.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.E)));
            this.menuFileEdit.Size = new System.Drawing.Size(285, 22);
            this.menuFileEdit.Text = "&Edit Selected Account";
            this.menuFileEdit.Click += new System.EventHandler(this.menuFileEdit_Click);
            // 
            // menuFileDelete
            // 
            this.menuFileDelete.Name = "menuFileDelete";
            this.menuFileDelete.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Delete)));
            this.menuFileDelete.Size = new System.Drawing.Size(285, 22);
            this.menuFileDelete.Text = "&Delete Selected Account";
            this.menuFileDelete.Click += new System.EventHandler(this.menuFileDelete_Click);
            // 
            // menuSeparator1
            // 
            this.menuSeparator1.Name = "menuSeparator1";
            this.menuSeparator1.Size = new System.Drawing.Size(282, 6);
            // 
            // menuFileChangePassphrase
            // 
            this.menuFileChangePassphrase.Name = "menuFileChangePassphrase";
            this.menuFileChangePassphrase.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.menuFileChangePassphrase.Size = new System.Drawing.Size(285, 22);
            this.menuFileChangePassphrase.Text = "Change &Passphrase";
            this.menuFileChangePassphrase.Click += new System.EventHandler(this.menuFileChangePassphrase_Click);
            // 
            // menuSeparator3
            // 
            this.menuSeparator3.Name = "menuSeparator3";
            this.menuSeparator3.Size = new System.Drawing.Size(282, 6);
            // 
            // menuFileExport
            // 
            this.menuFileExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileExportEncrypted,
            this.menuFileExportPlain});
            this.menuFileExport.Name = "menuFileExport";
            this.menuFileExport.Size = new System.Drawing.Size(285, 22);
            this.menuFileExport.Text = "Export Accounts";
            // 
            // menuFileExportEncrypted
            // 
            this.menuFileExportEncrypted.Name = "menuFileExportEncrypted";
            this.menuFileExportEncrypted.Size = new System.Drawing.Size(269, 22);
            this.menuFileExportEncrypted.Text = "Encrypted with Current Passphrase";
            this.menuFileExportEncrypted.Click += new System.EventHandler(this.menuFileExportEncrypted_Click);
            // 
            // menuFileExportPlain
            // 
            this.menuFileExportPlain.Name = "menuFileExportPlain";
            this.menuFileExportPlain.Size = new System.Drawing.Size(269, 22);
            this.menuFileExportPlain.Text = "Plain Text/XML (Not recommended!)";
            this.menuFileExportPlain.Click += new System.EventHandler(this.menuFileExportPlain_Click);
            // 
            // menuFileImport
            // 
            this.menuFileImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileImportLegacy,
            this.menuFileImportXML});
            this.menuFileImport.Name = "menuFileImport";
            this.menuFileImport.Size = new System.Drawing.Size(285, 22);
            this.menuFileImport.Text = "Import Accounts";
            // 
            // menuFileImportLegacy
            // 
            this.menuFileImportLegacy.Name = "menuFileImportLegacy";
            this.menuFileImportLegacy.Size = new System.Drawing.Size(323, 22);
            this.menuFileImportLegacy.Text = "From Accounts.dat file (Sept 2012 -> Aug 2013)";
            this.menuFileImportLegacy.Click += new System.EventHandler(this.menuFileImportLegacy_Click);
            // 
            // menuFileImportXML
            // 
            this.menuFileImportXML.Name = "menuFileImportXML";
            this.menuFileImportXML.Size = new System.Drawing.Size(323, 22);
            this.menuFileImportXML.Text = "From previously exported XML file";
            this.menuFileImportXML.Click += new System.EventHandler(this.menuFileImportXML_Click);
            // 
            // menuSeparator2
            // 
            this.menuSeparator2.Name = "menuSeparator2";
            this.menuSeparator2.Size = new System.Drawing.Size(282, 6);
            // 
            // menuFileQuit
            // 
            this.menuFileQuit.Name = "menuFileQuit";
            this.menuFileQuit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuFileQuit.Size = new System.Drawing.Size(285, 22);
            this.menuFileQuit.Text = "&Quit";
            this.menuFileQuit.Click += new System.EventHandler(this.menuFileQuit_Click);
            // 
            // menuPin
            // 
            this.menuPin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuPinCopy,
            this.menuPinBarcode});
            this.menuPin.Name = "menuPin";
            this.menuPin.Size = new System.Drawing.Size(36, 20);
            this.menuPin.Text = "&Pin";
            // 
            // menuPinCopy
            // 
            this.menuPinCopy.Name = "menuPinCopy";
            this.menuPinCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menuPinCopy.Size = new System.Drawing.Size(303, 22);
            this.menuPinCopy.Text = "&Copy to Clipboard";
            this.menuPinCopy.Click += new System.EventHandler(this.menuPinCopy_Click);
            // 
            // menuPinBarcode
            // 
            this.menuPinBarcode.Name = "menuPinBarcode";
            this.menuPinBarcode.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.menuPinBarcode.Size = new System.Drawing.Size(303, 22);
            this.menuPinBarcode.Text = "Show &Barcode for Selected Account";
            this.menuPinBarcode.Click += new System.EventHandler(this.menuPinBarcode_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuWindowAlwaysOnTop});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "&Window";
            // 
            // menuWindowAlwaysOnTop
            // 
            this.menuWindowAlwaysOnTop.Checked = true;
            this.menuWindowAlwaysOnTop.CheckOnClick = true;
            this.menuWindowAlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuWindowAlwaysOnTop.Name = "menuWindowAlwaysOnTop";
            this.menuWindowAlwaysOnTop.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.menuWindowAlwaysOnTop.Size = new System.Drawing.Size(179, 22);
            this.menuWindowAlwaysOnTop.Text = "Always On Top";
            this.menuWindowAlwaysOnTop.Click += new System.EventHandler(this.menuWindowAlwaysOnTop_Click);
            // 
            // dlgSaveBackup
            // 
            this.dlgSaveBackup.DefaultExt = "xml";
            this.dlgSaveBackup.FileName = "Accounts.xml";
            this.dlgSaveBackup.Filter = "XML Files|*.xml|Text Files|*.txt|All files|*.*";
            this.dlgSaveBackup.RestoreDirectory = true;
            this.dlgSaveBackup.Title = "Export Accounts";
            // 
            // dlgLoadXML
            // 
            this.dlgLoadXML.DefaultExt = "xml";
            this.dlgLoadXML.FileName = "Accounts.xml";
            this.dlgLoadXML.Filter = "XML Files|*.xml|Text Files|*.txt|All Files|*.*";
            this.dlgLoadXML.RestoreDirectory = true;
            this.dlgLoadXML.Title = "Import Accounts";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(386, 257);
            this.Controls.Add(this.lblCopyClipboard);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.pbTimeOut);
            this.Controls.Add(this.lblGetCode);
            this.Controls.Add(this.lbAccounts);
            this.Controls.Add(this.menuMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GoogleAuthenticator[CLONE]";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbTimeOut)).EndInit();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbAccounts;
        private System.Windows.Forms.Timer tmrGetCodes;
        private System.Windows.Forms.Label lblGetCode;
        private System.Windows.Forms.PictureBox pbTimeOut;
        private System.Windows.Forms.Label lblTimeOut;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblCopyClipboard;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileAdd;
        private System.Windows.Forms.ToolStripMenuItem menuFileEdit;
        private System.Windows.Forms.ToolStripMenuItem menuFileDelete;
        private System.Windows.Forms.ToolStripSeparator menuSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuFileQuit;
        private System.Windows.Forms.ToolStripMenuItem menuPin;
        private System.Windows.Forms.ToolStripMenuItem menuPinCopy;
        private System.Windows.Forms.ToolStripMenuItem menuPinBarcode;
        private System.Windows.Forms.ToolStripSeparator menuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuFileChangePassphrase;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuWindowAlwaysOnTop;
        private System.Windows.Forms.ToolStripMenuItem menuFileExport;
        private System.Windows.Forms.SaveFileDialog dlgSaveBackup;
        private System.Windows.Forms.ToolStripMenuItem menuFileImport;
        private System.Windows.Forms.ToolStripMenuItem menuFileImportLegacy;
        private System.Windows.Forms.ToolStripMenuItem menuFileImportXML;
        private System.Windows.Forms.ToolStripSeparator menuSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuFileExportEncrypted;
        private System.Windows.Forms.ToolStripMenuItem menuFileExportPlain;
        private System.Windows.Forms.OpenFileDialog dlgLoadXML;
        private System.Windows.Forms.ToolStripMenuItem menuFileSave;
    }
}

