using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MTK = MessagingToolkit;
using MTKQRC = MessagingToolkit.QRCode;

namespace GoogleAuthClone
{
    public partial class frmGetBarcode : Form
    {
        private GoogleAuthClone.PassPhrase thePPStore = null;

        public frmGetBarcode()
        {
            throw new InvalidOperationException("Cannot instantiate a new form without parameters!");
        }

        public frmGetBarcode(ref GoogleAuthClone.PassPhrase thePass)
        {
            InitializeComponent();
            thePPStore = thePass;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Tag = null;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.Tag == null)
            {
                MessageBox.Show(this, "Sorry, need a valid QR Barcode first...", "OOPS!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void butBrowse_Click(object sender, EventArgs e)
        {
            DialogResult ret = ofdFile.ShowDialog(this);
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    this.Tag = null;
                    this.Enabled = false;
                    txtFilename.Text = ofdFile.FileName;
                    try
                    {
                        pbxBarcode.Load(txtFilename.Text);
                        Application.DoEvents();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "There was a problem with the file: \r\n\r\n" +
                            ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try
                    {
                        MTKQRC.Codec.QRCodeDecoder myQRDecoder1 = new MTKQRC.Codec.QRCodeDecoder();
                        Bitmap myBitmap1 = new Bitmap(pbxBarcode.Image);
                        MTK.QRCode.Codec.Data.QRCodeBitmapImage myImage1 = new MTK.QRCode.Codec.Data.QRCodeBitmapImage(myBitmap1);
                        lblResult.Text = myQRDecoder1.decode(myImage1);
                        Application.DoEvents();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "There was a problem decoding the file: \r\n\r\n" +
                           ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    TOTPAccount QRAccount = null;
                    try
                    {
                        QRAccount = TOTPAccount.FromString(lblResult.Text, thePPStore.UseRaw());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "There was a problem with the account info: \r\n\r\n" +
                           ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (QRAccount == null)
                    {
                        MessageBox.Show(this, "There was a problem with the account info: \r\n\r\n" +
                           "The string doesn't parse for some reason.  Missing parameters?  Malformed?",
                           "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                        this.Tag = QRAccount;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "There was a problem somewhere: \r\n\r\n" +
                        ex.Message + "\r\n" + ex.StackTrace, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    this.Enabled = true;
                }
            }
        }

        private void frmGetBarcode_Load(object sender, EventArgs e)
        {
            ofdFile.CheckFileExists = true;
            ofdFile.CheckPathExists = true;
            ofdFile.AddExtension = false;
            ofdFile.DefaultExt = "BMP";
            ofdFile.DereferenceLinks = true;
            ofdFile.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.WMF;*.PNG;*.GIF)|*.BMP;*.JPG;*.JPEG;*.WMF;*.PNG;*.GIF|All files (*.*)|*.*";
            ofdFile.Multiselect = false;
            ofdFile.RestoreDirectory = true;
            ofdFile.ShowReadOnly = false;
            ofdFile.SupportMultiDottedExtensions = true;
            ofdFile.Title = "Image file containing a QR barcode:";
            ofdFile.FileName = "";
        }
    }
}
