using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using MessagingToolkit.QRCode;

namespace GoogleAuthClone
{
    public partial class frmDisplayBarcode : Form
    {
        private string displayMe = string.Empty;
        private string displayKey = string.Empty;

        public frmDisplayBarcode(string barcode2display, string key)
        {
            InitializeComponent();
            displayMe = barcode2display;
            displayKey = key;
        }

        private void frmDisplayBarcode_Load(object sender, EventArgs e)
        {
           
        }

        private void frmDisplayBarcode_Shown(object sender, EventArgs e)
        {
            // old way
            /*
            MessagingToolkit.QRCode.Codec.QRCodeEncoder myEncoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
            myEncoder.QRCodeEncodeMode = MessagingToolkit.QRCode.Codec.QRCodeEncoder.ENCODE_MODE.BYTE;
            myEncoder.QRCodeErrorCorrect = MessagingToolkit.QRCode.Codec.QRCodeEncoder.ERROR_CORRECTION.L;
            myEncoder.QRCodeVersion = 4;
            myEncoder.QRCodeScale = 5;
            Bitmap theBarcodeImage = myEncoder.Encode(displayMe);
            //*/

            ZXing.BarcodeWriter zbw = new ZXing.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = pbxBarcode.Height,
                    Width = pbxBarcode.Width
                }

            };
            pbxBarcode.Image = zbw.Write(displayMe);//theBarcodeImage;
            lblKey.Text = displayKey;
            lblNote.Text = "Note:  While this barcode may not exactly match Google or other providers, " +
                "it still contains the necessary URI information describing the account in question.  " +
                "Scan this barcode to store the information to your phone, or enter the key (without spaces) directly.  " +
                "*Key entry functionality was added to the official Authenticator app late September 2012.";
        }

        private void frmDisplayBarcode_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbxBarcode_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblNote_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblClickReminder_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblKey_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
