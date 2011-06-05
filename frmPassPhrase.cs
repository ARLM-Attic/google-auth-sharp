using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GoogleAuthClone
{
    public partial class frmPassPhrase : Form
    {
        public frmPassPhrase()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtPassphrase.Text.Length < 8 || !PassPhrase.MeetsComplexity(txtPassphrase.Text))
            {
                MessageBox.Show(this, "Your passphrase MUST be at least 8 characters long and contain the following: \r\n"+
                    "  At least one number\r\n" +
                    "  Upper and lower case letters\r\n"+
                    "  At least one special character, such as " + PassPhrase.ComplexitySpecialChars, "PASSPHRASE NOT COMPLEX ENOUGH", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.Tag = txtPassphrase.Text;
            txtPassphrase.Text = string.Empty;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Tag = null;
            this.Close();
        }

        private void frmPassPhrase_Load(object sender, EventArgs e)
        {
            if (this.Text.ToLower().Contains("confirm"))
            {
                //this.StartPosition = FormStartPosition.Manual;
                this.Top += 15;
                this.Left += 15;
            }
            
        }
    }
}
