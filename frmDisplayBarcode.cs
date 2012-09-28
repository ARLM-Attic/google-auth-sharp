using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MessagingToolkit.QRCode;

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
            MessagingToolkit.QRCode.Codec.QRCodeEncoder myEncoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
            myEncoder.QRCodeEncodeMode = MessagingToolkit.QRCode.Codec.QRCodeEncoder.ENCODE_MODE.BYTE;
            myEncoder.QRCodeErrorCorrect = MessagingToolkit.QRCode.Codec.QRCodeEncoder.ERROR_CORRECTION.L;
            myEncoder.QRCodeVersion = 4;
            myEncoder.QRCodeScale = 5;
            Bitmap theBarcodeImage = myEncoder.Encode(displayMe);
            pbxBarcode.Image = theBarcodeImage;
            lblKey.Text = displayKey;
            lblNote.Text = "Note:  It is common for this QR barcode to not match the one produced by Google or other providers.  " +
                "The barcode still contains the necessary information describing the account in question.  " +
                "Scan this barcode to store the information to your phone, or enter the key (without spaces) directly.  " +
                "*Key entry functionality was added to the Authenticor app late September 2012.";
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

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblKey_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
