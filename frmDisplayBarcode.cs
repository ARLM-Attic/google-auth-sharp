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
        
        public frmDisplayBarcode(string barcode2display)
        {
            InitializeComponent();
            displayMe = barcode2display;
        }

        private void frmDisplayBarcode_Load(object sender, EventArgs e)
        {
            MessagingToolkit.QRCode.Codec.QRCodeEncoder myEncoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
            myEncoder.QRCodeEncodeMode = MessagingToolkit.QRCode.Codec.QRCodeEncoder.ENCODE_MODE.BYTE;
            myEncoder.QRCodeErrorCorrect = MessagingToolkit.QRCode.Codec.QRCodeEncoder.ERROR_CORRECTION.L;
            myEncoder.QRCodeVersion = 4;
            myEncoder.QRCodeScale = 5;
            Bitmap theBarcodeImage = myEncoder.Encode(displayMe);
            pbxBarcode.Image = theBarcodeImage;
            lblNote.Text = "Note:  It is common for this QR barcode to not match the one produced by Google or other providers.  " +
                "The barcode still contains the necessary information describing the account in question.  " +
                "Scan this barcode to store the information to your phone.  Click anywhere to close the window.";
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
    }
}
