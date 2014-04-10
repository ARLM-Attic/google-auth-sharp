using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoogleAuthClone
{
    public partial class frmTOTPAccount : Form
    {
        private TOTPAccount theAccount = null;
        //private PassPhrase theLocalPass = null;

        public frmTOTPAccount()
        {
            
            throw new InvalidOperationException("Cannot instantiate a new form without parameters!");
        }

        public frmTOTPAccount(TOTPAccount someAccount = null)
        {
            if (someAccount != null)
                theAccount = someAccount.Clone();
            else
                theAccount = new TOTPAccount();
            //theLocalPass = thePass;
            InitializeComponent();
        }

        public void frmTOTPAccount_Load(object sender, EventArgs e)
        {
            try
            {
                txtName.Text = theAccount.Name;
                txtPeriod.Text = theAccount.Period.ToString();
                txtSecret.Text = theAccount.EncodedSecret;
                txtIssuer.Text = theAccount.Issuer;
                switch (theAccount.Algorithm)
                {
                    case TOTPAccount.TOTPAlgorithm.SHA1: cbAlgorithm.SelectedIndex = 0; break;
                    case TOTPAccount.TOTPAlgorithm.SHA256: cbAlgorithm.SelectedIndex = 1; break;
                    case TOTPAccount.TOTPAlgorithm.SHA512: cbAlgorithm.SelectedIndex = 2; break;
                    case TOTPAccount.TOTPAlgorithm.MD5: cbAlgorithm.SelectedIndex = 3; break;
                    default: cbAlgorithm.SelectedIndex = 0; break;
                }
                switch (theAccount.Digits)
                {
                    case 6: cbDigits.SelectedIndex = 0; break;
                    case 7: cbDigits.SelectedIndex = 1; break;
                    case 8: cbDigits.SelectedIndex = 2; break;
                    case 9: cbDigits.SelectedIndex = 3; break;
                    case 10: cbDigits.SelectedIndex = 4; break;
                    default: cbDigits.SelectedIndex = 0; break;
                }
            }
            catch (Exception ex)
            {
                this.Tag = ex;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int tempPeriod;
            if (!int.TryParse(txtPeriod.Text, out tempPeriod))
            {
                MessageBox.Show(this, "Period must be a number between 5 and 86400!", "BAD INPUT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (tempPeriod < 5 || tempPeriod > 86400)
                {
                    MessageBox.Show(this, "Period must be a number between 5 and 86400!", "BAD INPUT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(txtName.Text) || txtSecret.Text.Length < 10)
            {
                MessageBox.Show(this, "Secret must be at least 10 non-whitespace characters long!", "BAD INPUT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] tempSecret = null;
            try
            {
                if (rbB32.Checked)
                    tempSecret = Base32EncoderAlt.FromBase32String(txtSecret.Text);
                else if (rbB64.Checked)
                    tempSecret = Convert.FromBase64String(txtSecret.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("EXCEPTION ON VALIDATE INPUT:" + ex.Message + ex.StackTrace);
                return;
            }
            theAccount.Name = this.txtName.Text;
            theAccount.SetEncodedSecret(tempSecret);
            switch (cbAlgorithm.Text)
            {
                case "SHA1": theAccount.Algorithm = TOTPAccount.TOTPAlgorithm.SHA1; break;
                case "SHA256": theAccount.Algorithm = TOTPAccount.TOTPAlgorithm.SHA256; break;
                case "SHA512": theAccount.Algorithm = TOTPAccount.TOTPAlgorithm.SHA512; break;
                case "MD5": theAccount.Algorithm = TOTPAccount.TOTPAlgorithm.MD5; break;
            }
            int tempByte = byte.Parse(cbDigits.Text);
            theAccount.Digits = (byte)tempByte;
            theAccount.Period = tempPeriod;
            theAccount.Issuer = txtIssuer.Text;
            this.DialogResult = DialogResult.OK;
            this.Tag = theAccount;
            this.Close();
        }

        private void butBarcode_Click(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show(this, "Using a QR barcode image will overwrite the settings entered.  Overwrite?",
                "USE BARCODE TO OVERWRITE SETTINGS?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                frmGetBarcode myBarCode = new frmGetBarcode();
                myBarCode.Tag = null;
                ret = myBarCode.ShowDialog(this);
                if (ret == System.Windows.Forms.DialogResult.OK)
                {
                    theAccount = myBarCode.Tag as TOTPAccount;
                    if (theAccount == null)
                        theAccount = new TOTPAccount();
                    frmTOTPAccount_Load(this, new EventArgs());
                }
                myBarCode.Dispose();
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
                lblNameChecksumValue.Text = TOTPAccount.PreviewNameHash(txtName.Text);
            else
                lblNameChecksumValue.Text = "[type a name first]";
        }

    }
}
