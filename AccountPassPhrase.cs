using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using SSC = System.Security.Cryptography;
using GoogleAuthClone.Forms;

namespace GoogleAuthClone
{
    public class AccountPassPhrase
    {
        private byte[] _hashedSecret = null;
        private int bruteCount = 10;
        private static Guid _hashSalt = new Guid("{D64E17AB-F722-4F61-8BE6-F0A6E0385B49}");

        public const string ComplexitySpecialChars = "~!@#$%^&*()_+-=[]{}<>/?.";
        
        public bool Initialized { get { return _hashedSecret != null; } }

        public event EventHandler BruteForceDetected;

        private static byte[] GrindPassPhrase(string passphrase)
        {
            UTF8Encoding myEncoder = new UTF8Encoding();
            SSC.HMACSHA512 myHMAC = new SSC.HMACSHA512(_hashSalt.ToByteArray());
            SSC.SHA512Managed mySHA = new SSC.SHA512Managed();
            byte[] key = mySHA.ComputeHash(myEncoder.GetBytes(passphrase));
            byte[] buffer = myHMAC.ComputeHash(myEncoder.GetBytes(passphrase));
            myHMAC.Clear();
            for (int i = 0; i < 256; i++)
            {
                key[0] = (byte)(i % 256);
                key[1] += (byte)((key[1] + i) % 256);
                myHMAC = new SSC.HMACSHA512(key);
                buffer = myHMAC.ComputeHash(buffer); // churn, baby, churn
                myHMAC.Clear();
            }
            return buffer;
        }

        public static bool MeetsComplexity(string input)
        {
            bool hasUpper = false;
            bool hasLower = false;
            bool hasNumeral = false;
            bool hasSpecial = false;

            foreach (char thing in input.ToCharArray())
            {
                if (ComplexitySpecialChars.Contains(thing))
                {
                    hasSpecial = true;
                    continue;
                }
                if (Char.IsLetter(thing))
                {
                    if (Char.IsLower(thing))
                    {
                        hasLower = true;
                        continue;
                    }
                    else if (Char.IsUpper(thing))
                    {
                        hasUpper = true;
                        continue;
                    }
                }
                else if (Char.IsNumber(thing))
                    hasNumeral = true;
            }
            if (hasUpper && hasLower && hasSpecial && hasNumeral)
                return true;
            else
                return false;
        }

        public byte[] HashedPassPhrase { get {
            if (!Initialized)
                throw new InvalidOperationException("There is no passphrase to use.  Try using Set first.");
            return (byte[])_hashedSecret;
        } }

        /// <summary>
        /// Use when passphrase is already in place but hasn't been entered by the user. (existing accounts, on program first start)
        /// </summary>
        /// <param name="originalOwner"></param>
        /// <returns>true if user entered passphrase.</returns>
        public bool GetExisting(IWin32Window originalOwner)
        {
            if (Initialized)
                throw new InvalidOperationException("Cannot set a new passphrase if it has already been set, use Change instead, or Clear first.");

            string resultingPassPhrase = string.Empty;
            frmAccountPassPhrase myPass = new frmAccountPassPhrase();
            myPass.Tag = null;
            DialogResult ret = DialogResult.OK;

            myPass.Text = "Please enter the account passphrase:";
            ret = myPass.ShowDialog(originalOwner);
            resultingPassPhrase = (string)myPass.Tag;
            myPass.Dispose();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                _hashedSecret = GrindPassPhrase(resultingPassPhrase);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Use when passphrase has not been set yet, period. (no existing accounts, on program first start)
        /// </summary>
        /// <returns>true when a new passphrase has been accepted</returns>
        public bool SetNew(IWin32Window originalOwner)
        {
            if (Initialized)
                throw new InvalidOperationException("Cannot set a new passphrase if it has already been set, use Change instead, or Clear first.");

            string resultingPassPhrase = string.Empty;
            frmAccountPassPhrase myNewPass = new frmAccountPassPhrase();
            myNewPass.Tag = null;
            DialogResult ret = DialogResult.OK;

            while (string.Compare((string)myNewPass.Tag, resultingPassPhrase, false) != 0 && ret == DialogResult.OK)
            {
                myNewPass.Text = "Please enter the new passphrase:";
                ret = myNewPass.ShowDialog(originalOwner);
                resultingPassPhrase = (string)myNewPass.Tag;
                myNewPass.Tag = null;
                if (ret == System.Windows.Forms.DialogResult.OK)
                {
                    myNewPass.Text = "Please CONFIRM the entered passphrase:";
                    ret = myNewPass.ShowDialog(originalOwner);
                    if (string.Compare((string)myNewPass.Tag, resultingPassPhrase, false) != 0)
                    {
                        MessageBox.Show(originalOwner, "The two passphrases did not match.  Please try again.", "PASSPHRASE NOT ACCEPTED",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            myNewPass.Dispose();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                _hashedSecret = GrindPassPhrase(resultingPassPhrase);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Use this to challange the user against the stored/processed passphrase.
        /// </summary>
        /// <param name="originalOwner">The calling window handle.</param>
        /// <returns>true if user provides correct passphrase</returns>
        public bool Challange(IWin32Window originalOwner)
        {
            if (!Initialized)
                throw new InvalidOperationException("Nothing to challange, try Set first.");

            string resultingPassPhrase = string.Empty;
            string existingPass = Convert.ToBase64String(_hashedSecret);
            string theChallangePass = null;
            frmAccountPassPhrase thePass = new frmAccountPassPhrase();
            thePass.Tag = null;
            DialogResult ret = DialogResult.OK;
            while (ret == DialogResult.OK)
            {
                thePass.Text = "Please enter the CURRENT account passphrase:";
                ret = thePass.ShowDialog(originalOwner);
                if (ret == DialogResult.OK)
                {
                    resultingPassPhrase = (string)thePass.Tag;
                    theChallangePass = Convert.ToBase64String(GrindPassPhrase(resultingPassPhrase));
                    if (string.Compare(existingPass, theChallangePass, false) != 0)
                    {
                        if (--bruteCount == 0)
                        {
                            BruteForceDetected(this, new EventArgs());
                            return false;
                        }
                        MessageBox.Show(originalOwner, "The passphrases is incorrect.  Please try again.", "PASSPHRASE NOT ACCEPTED",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        return true;
                }
                else
                    return false;

            }
            thePass.Dispose();
            return false;
        }

        /// <summary>
        /// Use this to change the passphrase for existing accounts, NOT for first time use.
        /// This will challange the user to provide the existing passphrase first.
        /// This does NOT affect any backups!  Whatever was used to save those backups will not change!
        /// </summary>
        /// <param name="account">The generic list of TOTPAccounts to change.</param>
        /// <param name="originalOwner">The calling window handle.</param>
        /// <returns>true if the user provides the correct existing passphrase and succsessfully provides a new passphrase.</returns>
        public bool Change(ref Dictionary<string, TOTPAccount> theAccounts, IWin32Window originalOwner)
        {
            if (!this.Initialized)
                throw new InvalidOperationException("Cannot change passphrase if one has not been established!");
            if (theAccounts == null)
                throw new InvalidOperationException("Cannot change passphrase on the existing accounts if they are not passed in.");
            if (!this.Challange(originalOwner))
            {
                // make sure the user KNOWS the old passphrase (prevents nefarious activity if user walked away from unlocked terminal)
                return false;
            }
            string resultingPassPhrase = string.Empty;
            byte[] NEWSECRETKEY = null;
            frmAccountPassPhrase myNewPass = new frmAccountPassPhrase();
            myNewPass.Tag = null;
            DialogResult ret = DialogResult.OK;

            while (string.Compare((string)myNewPass.Tag, resultingPassPhrase, false) != 0 && ret == DialogResult.OK)
            {
                myNewPass.Text = "Please enter the NEW passphrase:";
                ret = myNewPass.ShowDialog(originalOwner);
                resultingPassPhrase = (string)myNewPass.Tag;
                myNewPass.Tag = null;
                if (ret == System.Windows.Forms.DialogResult.OK)
                {
                    myNewPass.Text = "Please CONFIRM the entered passphrase:";
                    ret = myNewPass.ShowDialog(originalOwner);
                    if (string.Compare((string)myNewPass.Tag, resultingPassPhrase, false) != 0 && ret == DialogResult.OK)
                        MessageBox.Show(originalOwner, "The two passphrases did not match.  Please try again.", "PASSPHRASE NOT ACCEPTED",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            myNewPass.Dispose();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                NEWSECRETKEY = GrindPassPhrase(resultingPassPhrase);
                //
                //    #warning This doesn't protect anything!
                //
                _hashedSecret = NEWSECRETKEY;
                Exception saveProblem = null;
                if (!AccountXMLPersistance.PutEncryptedAccounts(null, theAccounts, this, true, out saveProblem))
                    MessageBox.Show("FAILED TO SAVE SETTINGS!  NEW PASSPHRASE MAY NOT HAVE BEEN APPLIED! \r\n"
                        + saveProblem.Message,"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public void Clear()
        {
            _hashedSecret = null;
        }

        internal byte[] MakeSessionKey(byte[] iv)
        {
            byte[] buffer = null;
            // to set up a usable session key, we hash it a bunch of times on it's own, 
            // and then we fold in the iv and the original key again,
            // and hash it a bunch of times more
            buffer = this.HashedPassPhrase;
            SSC.HMACSHA256 myHmac = null;
            myHmac = new SSC.HMACSHA256(iv);
            buffer = myHmac.ComputeHash(buffer);
            myHmac.Clear();
            myHmac = new SSC.HMACSHA256(this.HashedPassPhrase);
            buffer = myHmac.ComputeHash(buffer);
            myHmac.Clear();
            SSC.SHA256Managed mySha = null;
            for (int i = 0; i < 256; i++)
            {
                mySha = new SSC.SHA256Managed();
                buffer = mySha.ComputeHash(buffer);
                mySha.Clear();
            }
            return buffer;
        }

        public XElement EncryptXElement(XElement source, string idAttribute)
        {
            XElement xeAccount = new XElement("EncryptedTOTPAccount");
            xeAccount.SetAttributeValue("id", idAttribute);
            byte[] buffer = null;
            byte[] useableKey = null;
            byte[] iv = new byte[16];
            // the payload will be encrypted and stuffed into XML later on
            SSC.RNGCryptoServiceProvider myRNG = new SSC.RNGCryptoServiceProvider();
            buffer = new byte[32]; // set up a buffer for noise
            myRNG.GetBytes(buffer); // get the noise
            // this noise is to randomize the contents of the file during encryption so that each block is unique.
            // since we'll be using a CBC mode, the first block and the IV are critical to be random.
            // the leading "n" is required because elements cannot have names starting with numbers
            XElement payload = new XElement("n" + Base32Encoder.ToBase32String(buffer).Replace("=", "") + "_noise");
            myRNG.GetBytes(iv); //the IV will be random, and will be folded into the key, which is provided by the user
            useableKey = MakeSessionKey(iv);
            //now we're ready to do this
            //payload.Add(new XElement("id", idAttribute));
            payload.Add(source);
            UTF8Encoding theEncoder = new UTF8Encoding();
            buffer = AESEncipher(theEncoder.GetBytes(payload.ToString()), useableKey, iv);
            // ok, last minute stuff and we're done

            xeAccount.Add(new XElement("payload", Convert.ToBase64String(buffer, Base64FormattingOptions.InsertLineBreaks)));
            xeAccount.Add(new XElement("iv", Convert.ToBase64String(iv))); // because the IV is random is necessary for the usable key, so give it here.
            // it's ok, this only leaks a small amount of information that would compromise the usable key, since the key is still 
            // user supplied and hashed before AND after the IV is used, and the IV is not derived from the key
            return xeAccount;
        }

        public XElement DecryptXElement(XElement source)
        {
            string payload = source.Element("payload").Value;
            byte[] buffer = Convert.FromBase64String(payload);
            string sourceIV = source.Element("iv").Value;
            byte[] iv = Convert.FromBase64String(sourceIV);
            byte[] useableKey = MakeSessionKey(iv);
            //string sourceSaltedNameHash = source.Attribute("saltednamehash").Value;

            buffer = AESDecipher(buffer, useableKey, iv);
            if (buffer == null) // something went wrong
                return null;
            UTF8Encoding encoder = new UTF8Encoding();
            XElement result = XElement.Parse(encoder.GetString(buffer));
            result = result.Element("TOTPAccount");
            return result;
        }

        internal byte[] AESEncipher(byte[] stuff, byte[] key, byte [] iv)
        {
            try
            {
                SSC.AesManaged a = new SSC.AesManaged();
                a.Mode = SSC.CipherMode.CBC;
                a.Padding = SSC.PaddingMode.ANSIX923;
                SSC.ICryptoTransform ict = a.CreateEncryptor(key, iv);
                byte[] ct = ict.TransformFinalBlock(stuff, 0, stuff.Length);
                return ct;
            }
            catch //(Exception ex)
            {
                //throw ex;
                //MessageBox.Show(frmMain.ActiveForm, ex.ToString(), "EXCEPTION ON DECRYPT");
                return null;
            }
        }

        internal byte[] AESDecipher(byte[] stuff, byte[] key, byte[] iv)
        {
            try
            {
                SSC.AesManaged a = new SSC.AesManaged();
                a.Mode = SSC.CipherMode.CBC;
                a.Padding = SSC.PaddingMode.ANSIX923;
                SSC.ICryptoTransform ict = a.CreateDecryptor(key, iv);
                byte[] ct = ict.TransformFinalBlock(stuff, 0, stuff.Length);
                return ct;
            }
            catch //(Exception ex)
            {
                //throw ex;
                //MessageBox.Show(frmMain.ActiveForm, ex.ToString(), "EXCEPTION ON DECRYPT");
                return null;
            }
        }
        // * */
    }
}
