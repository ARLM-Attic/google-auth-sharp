using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using SSC = System.Security.Cryptography;

namespace GoogleAuthClone
{
    public partial class frmMain : Form
    {
        private Single timeOutFullWidth = 0;
        private AccountPassPhrase appStore = new AccountPassPhrase();
        private Dictionary<string, TOTPAccount> accounts = new Dictionary<string, TOTPAccount>();
        private bool exitImmediately = false;

        public frmMain()
        {
            InitializeComponent();
            appStore.BruteForceDetected += new System.EventHandler(this.OnBruteForce);
            this.Shown += new EventHandler(frmMain_Shown);
        }

        private void OnBruteForce(object sender, EventArgs e)
        {
            exitImmediately = true;
            Application.Exit();
        }

        private void tmrGetCodes_Tick(object sender, EventArgs e)
        {
            object locker = new object();
            tmrGetCodes.Enabled = false;
            lock (locker)
            {
                if (lbAccounts.SelectedIndex != -1)
                {
                    txtCode.Text = accounts[lbAccounts.SelectedItem as string].ComputePIN(DateTime.Now);

                    Single percent = accounts[lbAccounts.SelectedItem as string].PercentIntervalElapsed(DateTime.Now);
                    if (percent > 0.65 && percent < 0.8)
                        pbTimeOut.BackColor = Color.Yellow;
                    else if (percent >= 0.8)
                        pbTimeOut.BackColor = Color.Red;
                    else
                        pbTimeOut.BackColor = Color.Blue;
                    pbTimeOut.Width = (int)(timeOutFullWidth * percent);
                    lblTimeOut.Text = "TIMEOUT IN\r\n";
                    try
                    {
                        lblTimeOut.Text +=
                            float.Parse(
                            ((1 - percent) * accounts[lbAccounts.SelectedItem.ToString()].Period)
                            .ToString()).ToString("0") + " SECONDS";
                    }
                    catch { }
                }
                else
                {
                    txtCode.Text = "==========";
                    lblTimeOut.Text = "No Code Selected";
                    pbTimeOut.Width = (int)(timeOutFullWidth);
                    pbTimeOut.BackColor = Color.Blue;
                }
                tmrGetCodes.Enabled = true;
            }
            return;
        }

        // NEW WAY
        private void frmMain_Shown(object sender, EventArgs e)
        {
            if (appStore.Initialized)
                return; // this is because we've ALREADY gotten the user passphrase

            Exception readProblem = null;
            bool foundStuff = false;
            bool decryptedOk = false;
            bool containsXML = false;
            DialogResult ret = DialogResult.Retry;
            if (!AccountXMLPersistance.CheckForEncryptedAccounts(null, out foundStuff, out containsXML, out readProblem))
            {
                if (readProblem != null)
                {
                    // uh oh, an exception happened, better tell the user and then get the heck out of dodge!
                    KickUser(readProblem.Message);
                    return;
                }
                //this is weird!  Not sure what happened here?  empty file, not xml, something else?
                if (foundStuff)
                {
                    MessageBox.Show(this, "An invalid or empty Accounts.xml file was found on start up.  It will be renamed.",
                    "INVALID ACCOUNTS.XML FILE", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    AccountXMLPersistance.RenameInvalidAccountsFile(out readProblem);
                    if (readProblem != null)
                    {
                        // uh oh, an exception happened, better tell the user and then get the heck out of dodge!
                        KickUser(readProblem.Message);
                        return;
                    }
                }
                //anyways, create a new passphrase for any new accounts
                if (!appStore.SetNew(this))
                {
                    // not the time to be rude
                    exitImmediately = true;
                    Application.Exit();
                    return;
                }
            }
            else
            {
                // ok we've got encrypted stuff to find so get a passphrase from the user
                if (!appStore.GetExisting(this))
                {
                    KickUser();
                    return;
                }
                accounts = AccountXMLPersistance.GetEncryptedAccounts(null, appStore, out foundStuff, out decryptedOk, out readProblem);
                if (!decryptedOk && readProblem == null)
                {
                    //hmmm bad passphrase... ask again, and again...
                    while (decryptedOk == false && ret == DialogResult.Retry)
                    {
                        ret = MessageBox.Show(this, "Sorry, but that passphrase doesn't work, please try again?",
                            "Incorrect passphrase, try again", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                        if (ret == DialogResult.Retry)
                        {
                            appStore.Clear();
                            if (!appStore.GetExisting(this))
                            {
                                KickUser();
                                return;
                            }
                            else
                            {
                                accounts = AccountXMLPersistance.GetEncryptedAccounts(null, appStore, out foundStuff, out decryptedOk, out readProblem);
                            }
                        }
                        else
                        {
                            KickUser();
                            return;
                        }
                    }

                }
                else if (readProblem != null)
                {
                    // uh oh, an exception happened, better tell the user and then get the heck out of dodge!
                    KickUser(readProblem.Message);
                    return;
                }
                // ok we have accounts, display them
                foreach (TOTPAccount acc in accounts.Values)
                {
                    lbAccounts.Items.Add(acc.Name);
                }
            }
            tmrGetCodes.Enabled = true;
        }


        // OLD WAY
        //private void frmMain_Shown(object sender, EventArgs e)
        //{   //get old code from CodePlex change sets September 2012 and older
        // removed for brevity
        //}

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!exitImmediately && e.CloseReason != CloseReason.UserClosing)
            {
                SaveSettings();
                return;
            }
            else if (exitImmediately)
            {
                return;
            }

            DialogResult ret = MessageBox.Show(
               this, "Are you sure you want to exit the program?", "ABOUT TO CLOSE PROGRAM", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (ret == DialogResult.Yes)
                SaveSettings();
            else
                e.Cancel = true;

        }

        private void SaveSettings()
        {
            Exception blobProblem = null;
            //old way removed for brevity
            if (accounts.Count > 0)
            {
                if (!AccountXMLPersistance.PutEncryptedAccounts(null, accounts, appStore, true, out blobProblem))
                    throw blobProblem;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            timeOutFullWidth = pbTimeOut.Width; // grab the full width at startup and use this as the reference
        }

        private void KickUser(string additionalMessages = null)
        {
            exitImmediately = true;
            if (additionalMessages != null)
                MessageBox.Show("NO SOUP FOR YOU!\r\n" + additionalMessages);
            else
                MessageBox.Show("NO SOUP FOR YOU!");
            Application.Exit();
        }

        private void lbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmrGetCodes_Tick(this, new EventArgs());
        }

        private void DeleteAccount()
        {
            if (lbAccounts.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Nothing selected!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            this.Enabled = false;
            DialogResult ret = MessageBox.Show(this,
                "Are you really sure you want to DELETE this account?\r\n\r\n" +
                "THIS ACTION CANNOT BE UNDONE AND IS PERMANENT!",
                "WARNING! THIS ACTION IS PERMANENT, REALLY DELETE?", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (ret == DialogResult.Yes)
            {
                ret = MessageBox.Show(this,
                    "No, really, I'm serious here!  Are you REALLY sure you want to DELETE this account?\r\n\r\n" +
                    "THERE ARE NO UNDO's HERE, NO CTRL-Z TO SAVE YOU!\r\n\r\n" +
                    "It's not too late to Cancel and export your accounts in case you change your mind later!",
                    "SECOND-CHANCE! THIS ACTION IS PERMANENT, REALLY DELETE?", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (ret == DialogResult.Yes)
                {
                    accounts.Remove(lbAccounts.SelectedItem as string);
                    lbAccounts.Items.RemoveAt(lbAccounts.SelectedIndex);
                    SaveSettings();
                    MessageBox.Show("Ok, it's gone... forever.");
                }
            }
            this.Enabled = true;
        }

        private void AddAccount()
        {
            frmTOTPAccount newAcc = null;
            try
            {
                this.Enabled = false;
                tmrGetCodes.Enabled = false;
                newAcc = new frmTOTPAccount(null);
                DialogResult ret = newAcc.ShowDialog(this);
                if (ret == DialogResult.OK)
                {
                    TOTPAccount theNewAcc = newAcc.Tag as TOTPAccount;
                    if (accounts.ContainsKey(theNewAcc.Name))
                    {
                        MessageBox.Show(this, "This account already exists (same name). " +
                            "If this was intended, use EDIT instead.", "ALREADY EXISTS! NOT ADDED / DISCARDED!",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    accounts.Add(theNewAcc.Name, theNewAcc);
                    int newItem = lbAccounts.Items.Add(theNewAcc.Name);
                    lbAccounts.ClearSelected();
                    SaveSettings();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("EXCEPTION: " + ex.Message + ex.StackTrace);
                return;
            }
            finally
            {
                newAcc.Dispose();
                this.Enabled = true;
                tmrGetCodes_Tick(this, new EventArgs());
                tmrGetCodes.Enabled = true;
            }
        }

        private void EditAccount()
        {
            if (lbAccounts.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Nothing selected!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            this.Enabled = false;
            tmrGetCodes.Enabled = false;
            string accountName = lbAccounts.SelectedItem as string;
            frmTOTPAccount existingAccInput = new frmTOTPAccount(accounts[accountName]);
            DialogResult ret = existingAccInput.ShowDialog(this);
            if (ret == DialogResult.OK)
            {
                accounts[accountName] = existingAccInput.Tag as TOTPAccount;
                existingAccInput.Dispose();
                lbAccounts.ClearSelected();
                SaveSettings();
            }
            this.Enabled = true;
            tmrGetCodes_Tick(this, new EventArgs());
            tmrGetCodes.Enabled = true;
        }

        private void butQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblGetCode_Click(object sender, EventArgs e)
        {
            if (lbAccounts.SelectedIndex > -1)
            {
                frmDisplayBarcode theBarcodeForm = new frmDisplayBarcode(
                    accounts[lbAccounts.SelectedItem as string].ToUriString(),
                    accounts[lbAccounts.SelectedItem as string].EncodedSecret);
                theBarcodeForm.ShowDialog(this);
            }
        }

        void txtCode_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (lbAccounts.SelectedIndex != -1)
            {
                Clipboard.SetText(txtCode.Text, TextDataFormat.Text);
                if (Clipboard.GetText(TextDataFormat.Text) == txtCode.Text)
                {
                    //MessageBox.Show(this, "Code now in clipboard.  Paste away!", "CLIPBOARD", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Media.SystemSounds.Beep.Play();
                }
            }
        }

        private void menuWindowAlwaysOnTop_Click(object sender, EventArgs e)
        {
            this.TopMost = menuWindowAlwaysOnTop.Checked;
        }

        private void menuPinCopy_Click(object sender, EventArgs e)
        {
            if (lbAccounts.SelectedIndex != -1)
            {
                Clipboard.SetText(txtCode.Text, TextDataFormat.Text);
                if (Clipboard.GetText(TextDataFormat.Text) == txtCode.Text)
                {
                    //MessageBox.Show(this, "Code now in clipboard.  Paste away!", "CLIPBOARD", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Media.SystemSounds.Beep.Play();
                }
            }
        }

        private void menuFileChangePassphrase_Click(object sender, EventArgs e)
        {
            //rely on Change to challange the user for the old passphrase and then save the acccounts using the new passphrase
            appStore.Change(ref accounts, this);
        }

        private void menuPinBarcode_Click(object sender, EventArgs e)
        {
            if (lbAccounts.SelectedIndex > -1)
            {
                frmDisplayBarcode theBarcodeForm = new frmDisplayBarcode(
                    accounts[lbAccounts.SelectedItem as string].ToUriString(),
                    accounts[lbAccounts.SelectedItem as string].EncodedSecret);
                theBarcodeForm.ShowDialog(this);
            }
        }

        private void menuFileImportLegacy_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("OOPS!  I haven't actually coded that part up yet!  Sorry :(");
            //return;
            bool foundLegacyStuff;
            Exception readProblem = null;
            if (!AccountXMLPersistance.CheckForLegacyAccounts(out foundLegacyStuff, out readProblem) && readProblem == null)
            {
                MessageBox.Show("Sorry, there doesn't appear to be a legacy Accounts.dat file in the program directory.");
                return;
            }
            else if (!foundLegacyStuff && readProblem == null)
            {
                MessageBox.Show("Sorry, there doesn't appear to be a (valid) legacy Accounts.dat file in the program directory.");
                return;
            }
            else if (readProblem != null)
            {

            }
            
            Dictionary<string, TOTPAccount> tempAccounts = new Dictionary<string, TOTPAccount>();
            DialogResult ret = System.Windows.Forms.DialogResult.Ignore;
            GoogleAuthClone.Deprecated.PassPhrase old_ppStore =
                new GoogleAuthClone.Deprecated.PassPhrase();  // THIS IS USED FOR LEGACY DATA ONLY!
            MessageBox.Show("Ok, on the next pop-up screen, enter the OLD passphrase that was used to store the Legacy Data," +
                    " and that data will be converted to the new version.",
                    "About to Import Legacy Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (!old_ppStore.GetFromUser(this))
            {
                return;
            }
            string blob = GoogleAuthClone.Deprecated.Persistence.GetAccountBlob(out readProblem); // THIS IS USED FOR LEGACY DATA ONLY
            ret = System.Windows.Forms.DialogResult.Retry;
            string buffer = old_ppStore.RemoveFromString(blob);
            while (!buffer.Contains("?secret="))
            {
                ret = MessageBox.Show(this, "Sorry, but that passphrase doesn't work, please try again?",
                    "Incorrect passphrase, try again", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                if (ret == DialogResult.Retry)
                {
                    old_ppStore.Clear();
                    if (!old_ppStore.GetFromUser(this))
                    {
                        return;
                    }
                    else
                    {
                        buffer = old_ppStore.RemoveFromString(blob);
                    }
                }
                else
                {
                    return;
                }
            }
            // lastly, rehydrate from stored material
            string[] things = buffer.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string thing in things)
            {
                TOTPAccount acc = TOTPAccount.FromUriString(thing);
                if (acc != null)
                {
                    acc.Name += "[LEGACY]";
                    tempAccounts.Add(acc.Name, acc);
                    //lbAccounts.Items.Add(acc.Name);
                }
            }
            ret = MessageBox.Show(this, "A total of " + tempAccounts.Count + " accounts were found, continue to import? (Imported accounts will be named with [LEGACY]" +
                " for easy identification later. Deletion of duplicates is up to you!)", "FOUND SOME LEGACY ACCOUNTS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                List<string> duplicates = new List<string>();
                foreach (string a in tempAccounts.Keys)
                {
                    if (accounts.ContainsKey(a))
                        duplicates.Add(a);
                    else
                    {
                        accounts.Add(a, tempAccounts[a]);
                        lbAccounts.Items.Add(a);
                    }
                }
                if (duplicates.Count > 0)
                {
                    string duplicateList = "";
                    foreach (string d in duplicates)
                        duplicateList += d + "\r\n";
                    MessageBox.Show("Oops, despite adding the [LEGACY] tag to the title, one or more accounts was/were found that already existed. \r\n" + 
                        "Did you already import this file, perhaps?\r\n\r\n" + duplicateList);
                }
                duplicates.Clear();
            }
            tempAccounts.Clear();
        }

        private void menuFileAdd_Click(object sender, EventArgs e)
        {
            AddAccount();
        }

        private void menuFileEdit_Click(object sender, EventArgs e)
        {
            EditAccount();
        }

        private void menuFileQuit_Click(object sender, EventArgs e)
        {
            exitImmediately = false;
            this.Close();
        }

        private void menuFileDelete_Click(object sender, EventArgs e)
        {
            DeleteAccount();
        }

        private void menuFileImportXML_Click(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show(this,
                    "This will import any accounts listed in an XML file that you specify.  Any accounts found will be added to the list with " +
                    "[IMPORTED] added to the names for easy identification.  It is up to you do delete any duplicates!\r\n\r\n" +
                    "If any are found that are encrypted, you will be prompted for the passphrase to unlock them.  Any unencrypted accounts will " +
                    "simply be imported.",
                    "IMPORTING ACCOUNTS", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ret == DialogResult.OK)
            {
                dlgLoadXML.InitialDirectory = Application.StartupPath;
                ret = dlgLoadXML.ShowDialog(this);
                Dictionary<string, TOTPAccount> tempEAccounts = null;
                Dictionary<string, TOTPAccount> tempPAccounts = null;
                if (ret == DialogResult.OK)
                {
                    bool fileExists;
                    bool isXML;
                    bool isDecryptable;
                    Exception readError = null;
                    if (!AccountXMLPersistance.CheckForAccounts(dlgLoadXML.FileName, out fileExists, out isXML, out readError))
                    {
                        if (readError != null)
                        {
                            MessageBox.Show(this, "Something went wrong!\r\n\r\n" + readError.Message,
                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else if (!fileExists)
                        {
                            MessageBox.Show(this, "Something went wrong! File doesn't actually exist maybe?",
                                "UNEXPECTED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else if (!isXML)
                        {
                            MessageBox.Show(this, "Doesn't appear to be any valid XML in there, pick a different file.",
                                "NO XML FOUND", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                    else
                    {
                        tempPAccounts = AccountXMLPersistance.GetAccounts(dlgLoadXML.FileName, out isXML, out readError);
                        if (readError != null)
                        {
                            MessageBox.Show(this, "Something went wrong!\r\n\r\n" + readError.Message,
                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    if (!AccountXMLPersistance.CheckForEncryptedAccounts(dlgLoadXML.FileName, out fileExists, out isXML, out readError))
                    {
                        if (readError != null)
                        {
                            MessageBox.Show(this, "Something went wrong!\r\n\r\n" + readError.Message,
                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else if (!fileExists)
                        {
                            MessageBox.Show(this, "Something went wrong! File doesn't actually exist maybe?",
                                "UNEXPECTED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else if (!isXML)
                        {
                            MessageBox.Show(this, "Doesn't appear to be any valid XML in there, please pick a different file.",
                                "NO XML FOUND", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                    else
                    {
                        AccountPassPhrase tempAppStore = new AccountPassPhrase();
                        if (tempAppStore.GetExisting(this))
                        {
                            tempEAccounts = AccountXMLPersistance.GetEncryptedAccounts(dlgLoadXML.FileName, tempAppStore,
                               out fileExists, out isDecryptable, out readError);
                            if (readError != null)
                            {
                                MessageBox.Show(this, "Something went wrong!\r\n\r\n" + readError.Message,
                                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            if (tempEAccounts == null || isDecryptable == false)
                            {
                                MessageBox.Show("It looks like that passphrase doesn't work for the encrypted accounts. " +
                                    "Try again later with a different passphrase.  If you've forgotten it... well... there is nothing I nor the " +
                                    "program's author can do :(  sorry!");
                            }
                        }
                        else
                            MessageBox.Show("Ok, nevermind, I won't try to find any encrypted accounts afterall... " + 
                                "try the file again later if you change your mind.");
                    }

                    if (tempEAccounts == null && tempPAccounts == null)
                    {
                        MessageBox.Show("There doesn't appear to be anything in that file, encrypted or otherwise... Please try a different file.");
                        return;
                    }
                    int eCount = 0;
                    if (tempEAccounts != null)
                        eCount = tempEAccounts.Count;
                    int pCount = 0;
                    if (tempPAccounts != null)
                        pCount = tempPAccounts.Count;
                    ret = MessageBox.Show(this, "A total of " + eCount + " encrypted and " + pCount + " unencrypted " + 
                        "accounts were found, continue to import? (Imported accounts will be named with [IMPORTED]" +
                        " for easy identification later. Deletion of duplicates is up to you!)", 
                        "FOUND SOME ACCOUNTS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ret == DialogResult.Yes)
                    {
                        List<string> duplicates = new List<string>();
                        if (tempEAccounts != null)
                        {
                            foreach (string a in tempEAccounts.Keys)
                            {
                                string tempName = a + "[IMPORTED]";
                                tempEAccounts[a].Name += "[IMPORTED]";
                                if (accounts.ContainsKey(tempName))
                                    duplicates.Add(a);
                                else
                                {
                                    accounts.Add(tempName, tempEAccounts[a]);
                                    lbAccounts.Items.Add(tempName);
                                }
                            }
                            tempEAccounts.Clear();
                        }
                        if (tempPAccounts != null)
                        {
                            foreach (string a in tempPAccounts.Keys)
                            {
                                string tempName = a + "[IMPORTED]";
                                tempPAccounts[a].Name += "[IMPORTED]";
                                if (accounts.ContainsKey(tempName))
                                    duplicates.Add(a);
                                else
                                {
                                    accounts.Add(tempName, tempPAccounts[a]);
                                    lbAccounts.Items.Add(tempName);
                                }
                            }
                            tempPAccounts.Clear();
                        }
                        if (duplicates.Count > 0)
                        {
                            string duplicateList = "";
                            foreach (string d in duplicates)
                                duplicateList += d + "\r\n";
                            MessageBox.Show("Oops, despite adding the [IMPORTED] tag to the title, one or more accounts was/were found that already existed. \r\n" +
                                "Did you already import this file, perhaps?\r\n\r\n" + duplicateList);
                        }
                        duplicates.Clear();
                    }
                }
            }
        }

        private void menuFileExportEncrypted_Click(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show(this,
                    "This will export all listed accounts to a separate XML file.  That separate file is considered OFFLINE " +
                    "(as it exists outside the control or concern of the program and is for your benefit and piece of mind).  It will be saved " +
                    "using your CURRENT PASSPHRASE, so remember it!",
                    "EXPORTING ALL ACCOUNTS", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ret == DialogResult.OK)
            {
                dlgSaveBackup.InitialDirectory = Application.StartupPath;
                ret = dlgSaveBackup.ShowDialog(this);
                if (ret == DialogResult.OK)
                {
                    Exception fileProblem = null;
                    if (AccountXMLPersistance.PutEncryptedAccounts(dlgSaveBackup.FileName, accounts, appStore, true, out fileProblem))
                    {
                        MessageBox.Show(this, "Ok, all done. Remember the passphrase for this file!  Seriously...");
                    }
                    else
                    {
                        if (fileProblem != null)
                            MessageBox.Show(this, "Uh oh... something went wrong!\r\n\r\n" + fileProblem.Message,
                                "ERROR ON SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            MessageBox.Show(this, "Uh oh... something went wrong, but I don't know what!", 
                                "ERROR ON SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                dlgSaveBackup.FileName = "";
            }
        }

        private void menuFileExportPlain_Click(object sender, EventArgs e)
        {
            //PLAIN TEXT=============================================
            DialogResult ret = MessageBox.Show(this,
                "This will export all listed accounts to a separate XML file.  That separate file is considered OFFLINE " +
                "(as it exists outside the control or concern of the program and is for your benefit and piece of mind).\r\n"+
                "\r\nIt will be saved WITHOUT ENCRYPTION!  PLAIN TEXT!  ANYBODY CAN READ IT!  \r\n\r\n" +
                "So protect it, as anyone with the details to your accounts can potentionally log into them!\r\n\r\n" + 
                "You've been warned",
                "EXPORTING ALL ACCOUNTS - PLAIN TEXT!", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            if (ret == DialogResult.OK)
            {
                dlgSaveBackup.InitialDirectory = Application.StartupPath;
                ret = dlgSaveBackup.ShowDialog(this);
                if (ret == DialogResult.OK)
                {
                    Exception fileProblem = null;
                    if (AccountXMLPersistance.PutAccounts(dlgSaveBackup.FileName, accounts, true, out fileProblem))
                    {
                        MessageBox.Show(this, "Ok, all done. Remember to *protect* this file!  Seriously...");
                    }
                    else
                    {
                        if (fileProblem != null)
                            MessageBox.Show(this, "Uh oh... something went wrong!\r\n\r\n" + fileProblem.Message,
                                "ERROR ON SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            MessageBox.Show(this, "Uh oh... something went wrong, but I don't know what!",
                                "ERROR ON SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                dlgSaveBackup.FileName = "";
            }
        }

        //*/
    }
}
