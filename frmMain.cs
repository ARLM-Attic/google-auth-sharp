using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SSC = System.Security.Cryptography;

namespace GoogleAuthClone
{
    public partial class frmMain : Form
    {
        private PassPhrase ppStore = new PassPhrase();
        private List<TOTPAccount> accounts = new List<TOTPAccount>();
        private bool exitImmediately = false;

        public frmMain()
        {
            InitializeComponent();
            ppStore.BruteForceDetected += new System.EventHandler(this.OnBruteForce);
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
                    TOTPAccount theSelected = accounts.Find(
                        delegate(TOTPAccount ta)
                        {
                            return ta.Name == lbAccounts.SelectedItem.ToString();
                        });
                    lblCode.Text = theSelected.ComputePIN(ppStore.UseRaw(), DateTime.Now);
                }
                else
                    lblCode.Text = "==========";
                tmrGetCodes.Enabled = true;
            }
            return;
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {   
            Exception blobProblem = null;

            DialogResult ret = DialogResult.None;
            string buffer = Persistence.GetAccountBlob(out blobProblem);
            if (blobProblem != null)
            {
                MessageBox.Show("NO SOUP FOR YOU!\r\n" + blobProblem.Message + blobProblem.StackTrace);
                Application.Exit();
            }
            if (buffer == null)
            {
                //there are no previously saved accounts, time to get at least one.
                //first: the desired passphrase
                if (!ppStore.Set(this))
                {
                    MessageBox.Show("NO SOUP FOR YOU!");
                    Application.Exit();
                }

                //second: get at least one new account
                frmTOTPAccount myNewAcc = new frmTOTPAccount(ref ppStore);
                ret = myNewAcc.ShowDialog(this);
                Exception problem = myNewAcc.Tag as Exception;
                if (problem != null)
                    MessageBox.Show(problem.Message + problem.StackTrace);
                if (ret == System.Windows.Forms.DialogResult.Cancel)
                {
                    MessageBox.Show("NO SOUP FOR YOU!");
                    Application.Exit();
                    return;
                }
                TOTPAccount theNewAcc = myNewAcc.Tag as TOTPAccount;
                accounts.Add(theNewAcc);
                int newItem = lbAccounts.Items.Add(theNewAcc);
                myNewAcc.Dispose();
                //third: store it 
                SaveSettings();
            }
            else
            {
                //existing account information exists so get it
                // first: need passphrase from user
                if (!ppStore.GetFromUser(this))
                {
                    MessageBox.Show("NO SOUP FOR YOU!");
                    Application.Exit();
                    return;
                }
                // second: test passphrase against material, and ask for passphrase if no go
                string rawBlob = Persistence.GetAccountBlob(out blobProblem);
                buffer = ppStore.RemoveFromString(rawBlob);
                ret = System.Windows.Forms.DialogResult.Retry;
                while (!buffer.Contains("?secret="))
                {
                    ret = MessageBox.Show(this, "Sorry, but that passphrase doesn't work, please try again?", 
                        "Incorrect passphrase, try again", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                    if (ret == DialogResult.Retry)
                    {
                        ppStore.Clear();
                        if (!ppStore.GetFromUser(this))
                        {
                            MessageBox.Show("NO SOUP FOR YOU!");
                            Application.Exit();
                            return;
                        }
                        else
                        {
                            buffer = ppStore.RemoveFromString(rawBlob);
                        }
                    }
                    else
                    {
                        MessageBox.Show("NO SOUP FOR YOU!");
                        Application.Exit();
                        return;
                    }
                }
                // lastly:  rehydrate application from stored material
                string[] things = buffer.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string thing in things)
                {
                    TOTPAccount acc = TOTPAccount.FromString(thing, ppStore.UseRaw());
                    if (acc != null)
                    {
                        accounts.Add(acc);
                        lbAccounts.Items.Add(acc);
                    }
                }
            }
            tmrGetCodes.Enabled = true;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (exitImmediately)
            {
                SaveSettings();
                return;
            }
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                DialogResult ret = MessageBox.Show(
                   this, "Are you sure you want to exit the program?", "ABOUT TO CLOSE PROGRAM", MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (ret == DialogResult.Yes)
                    SaveSettings();
                else
                    e.Cancel = true;
            }
        }

        private void SaveSettings()
        {
            Exception blobProblem = null;
            StringBuilder sb = new StringBuilder();
            foreach (TOTPAccount thing in accounts)
            {
                sb.AppendLine(thing.ToString(ppStore.UseRaw()));
            }
            if (!string.IsNullOrWhiteSpace(sb.ToString()))
                Persistence.PutAccountBlob(ppStore.Apply(sb.ToString()), out blobProblem);
            else
                Persistence.PutAccountBlob(string.Empty, out blobProblem);
            if (blobProblem != null)
            {
                throw blobProblem;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void lbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmrGetCodes_Tick(this, new EventArgs());
        }

        private void butDel_Click(object sender, EventArgs e)
        {
            if (lbAccounts.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Nothing selected!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            this.Enabled = false;
            DialogResult ret = MessageBox.Show(this,
                "Are you really sure you want to DELETE this account? \r\n " +
                "THIS ACTION CANNOT BE UNDONE!  THIS ACTION IS PERMANENT!",
                "WARNING! THIS ACTION IS PERMANENT, REALLY DELETE?", MessageBoxButtons.OKCancel, 
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (ret == DialogResult.OK)
            {
                ret = MessageBox.Show(this,
                    "No, really, I'm serious here!  Are you REALLY sure you want to DELETE this account? \r\n "+
                    "THERE ARE NO UNDO's HERE, NO CTRL-Z TO SAVE YOU!",
                    "SECOND-CHANCE! THIS ACTION IS PERMANENT, REALLY DELETE?", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (ret == DialogResult.OK)
                {
                    int theGoner = accounts.FindIndex(
                        delegate(TOTPAccount ta)
                        { 
                            return ta.Name == lbAccounts.SelectedItem.ToString(); 
                        });
                    accounts.RemoveAt(theGoner);
                    lbAccounts.Items.RemoveAt(lbAccounts.SelectedIndex);
                    MessageBox.Show("Ok, it's gone... forever.");
                }
            }
            if (lbAccounts.Items.Count == 0)
            {
                butDel.Visible = false;
                butEdit.Visible = false;
            }
            SaveSettings();
            this.Enabled = true;
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            frmTOTPAccount newAcc = null;
            try
            {
                this.Enabled = false;
                tmrGetCodes.Enabled = false;
                newAcc = new frmTOTPAccount(ref ppStore);
                DialogResult ret = newAcc.ShowDialog(this);
                if (ret == DialogResult.OK)
                {
                    TOTPAccount theNewAcc = newAcc.Tag as TOTPAccount;
                    if (accounts.Find(
                            delegate(TOTPAccount ta)
                            {
                                return ta.Name == theNewAcc.Name;
                            }) != null)
                    {
                        MessageBox.Show(this, "This account already exists (same name). " +
                            "If this was intended, use EDIT instead.", "ALREADY EXISTS! NOT ADDED!",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    accounts.Add(theNewAcc);
                    int newItem = lbAccounts.Items.Add(theNewAcc);             
                    lbAccounts.SelectedIndex = newItem;
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
                if (lbAccounts.Items.Count == 0)
                {
                    butDel.Visible = false;
                    butEdit.Visible = false;
                }
                tmrGetCodes.Enabled = true;
            }
        }

        private void butEdit_Click(object sender, EventArgs e)
        {
            if (lbAccounts.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Nothing selected!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            this.Enabled = false;
            tmrGetCodes.Enabled = false;
            TOTPAccount existingAcc = accounts.Find(
                        delegate(TOTPAccount ta)
                        {
                            return ta.Name == lbAccounts.SelectedItem.ToString();
                        });
            frmTOTPAccount existingAccInput = new frmTOTPAccount(ref ppStore, existingAcc);
            DialogResult ret = existingAccInput.ShowDialog(this);
            if (ret == DialogResult.OK)
            {
                existingAcc = existingAccInput.Tag as TOTPAccount;
                int tempIndex = accounts.FindIndex(
                        delegate(TOTPAccount ta)
                        {
                            return ta.Name == lbAccounts.SelectedItem.ToString();
                        });
                accounts.RemoveAt(tempIndex);
                accounts.Insert(tempIndex, existingAcc);     
                tempIndex = lbAccounts.SelectedIndex;
                lbAccounts.Items.RemoveAt(tempIndex);
                lbAccounts.Items.Insert(tempIndex, existingAcc);
                existingAccInput.Dispose();
                lbAccounts.SelectedIndex = tempIndex;
            }
            this.Enabled = true;
            tmrGetCodes_Tick(this, new EventArgs());
            tmrGetCodes.Enabled = true;
        }

        private void butCPP_Click(object sender, EventArgs e)
        {
            ppStore.Change(ref accounts, this);
        }

        private void butQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblCode_DoubleClick(object sender, EventArgs e)
        {
            if (lbAccounts.SelectedIndex > -1)
            {
                TOTPAccount theAccount = accounts.Find(delegate(TOTPAccount ta)
                {
                    return ta.Name == lbAccounts.SelectedItem.ToString();
                });
                frmDisplayBarcode theBarcodeForm = new frmDisplayBarcode(theAccount.ToString(ppStore.UseRaw()));
                theBarcodeForm.ShowDialog(this);
            }

        }

    }
}
