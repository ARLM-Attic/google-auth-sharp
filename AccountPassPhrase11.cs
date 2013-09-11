using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using SSC = System.Security.Cryptography;
using GoogleAuthClone.Forms;
using System.Security.Cryptography;

namespace GoogleAuthClone
{
    public class AccountPassPhrase11
    {
        private byte[] _hashedSecret = null;
        private byte[] _salt = null;
        private int bruteCount = 10;
        private const int ITERATIONS = 8192;
        private const byte HASHEDSECRETLENGTH = 20;
        
        //private readonly Guid ACCOUNTSALT = new Guid("{96293D0A-C5E7-F2C3-8DE4-991118FB4A07}");

        public const string ComplexitySpecialChars = "~!@#$%^&*()_+-=[]{}<>/?.";
        
        public bool Initialized
        { 
            get 
            { 
                return _hashedSecret != null; 
            } 
        }

        public byte[] MasterSalt
        { 
            get
            {
                if (!Initialized)
                    throw new InvalidOperationException("There is no passphrase set, and thus no salt.  Try using Set first.");
                if (_salt != null)
                    return (byte[])_salt.Clone();
                else 
                    return null;
            } 
        }
        
        public byte[] HashedPassPhrase
        {
            get
            {
                if (!Initialized)
                    throw new InvalidOperationException("There is no passphrase to use.  Try using Set first.");
                return (byte[])_hashedSecret.Clone();
            }
        }

        public event EventHandler BruteForceDetected;

        public static bool MeetsComplexity(string Input)
        {
            bool hasUpper = false;
            bool hasLower = false;
            bool hasNumeral = false;
            bool hasSpecial = false;

            foreach (char thing in Input.ToCharArray())
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

        /// <summary>
        /// Use when passphrase is already in place but hasn't been entered by the user. (existing accounts, on program first start)
        /// </summary>
        /// <param name="OriginalOwner"></param>
        /// <returns>true if user entered passphrase.</returns>
        public bool GetExisting(IWin32Window OriginalOwner, byte[] ExistingSalt)
        {
            if (Initialized)
                throw new InvalidOperationException("Cannot set a new passphrase if it has already been set, use Change instead, or Clear first.");

            string resultingPassPhrase = string.Empty;
            frmAccountPassPhrase myPass = new frmAccountPassPhrase();
            myPass.Tag = null;
            DialogResult ret = DialogResult.OK;

            myPass.Text = "Please enter the account passphrase:";
            ret = myPass.ShowDialog(OriginalOwner);
            resultingPassPhrase = (string)myPass.Tag;
            myPass.Dispose();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                _hashedSecret = GrindPassPhrase(resultingPassPhrase, ExistingSalt); 
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
        public bool SetNew(IWin32Window OriginalOwner)
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
                ret = myNewPass.ShowDialog(OriginalOwner);
                resultingPassPhrase = (string)myNewPass.Tag;
                myNewPass.Tag = null;
                if (ret == System.Windows.Forms.DialogResult.OK)
                {
                    myNewPass.Text = "Please CONFIRM the entered passphrase:";
                    ret = myNewPass.ShowDialog(OriginalOwner);
                    if (string.Compare((string)myNewPass.Tag, resultingPassPhrase, false) != 0)
                    {
                        MessageBox.Show(OriginalOwner, "The two passphrases did not match.  Please try again.", "PASSPHRASE NOT ACCEPTED",
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

        private byte[] GrindPassPhrase(string PassPhrase, byte[] ExistingSalt = null)
        {
            PBKDF2<HMACSHA512> myPBKDF2 = null;
            if (ExistingSalt == null)
            {
                myPBKDF2 = new PBKDF2<HMACSHA512>(PassPhrase, 32, ITERATIONS);
                _salt = myPBKDF2.Salt;
            }
            else
            {
                myPBKDF2 = new PBKDF2<HMACSHA512>(PassPhrase, ExistingSalt, ITERATIONS);
                _salt = (byte[])ExistingSalt.Clone();
            }
            return myPBKDF2.GetBytes(HASHEDSECRETLENGTH);
        }

        /// <summary>
        /// Use this to challange the user against the stored/processed passphrase.
        /// </summary>
        /// <param name="OriginalOwner">The calling window handle.</param>
        /// <returns>true if user provides correct passphrase</returns>
        public bool Challange(IWin32Window OriginalOwner)
        {
            if (!Initialized)
                throw new InvalidOperationException("Nothing to challange, try Set or GetExisting first.");

            string resultingPassPhrase = string.Empty;
            string existingPass = Convert.ToBase64String(_hashedSecret);
            string theChallangePass = null;
            frmAccountPassPhrase thePass = new frmAccountPassPhrase();
            thePass.Tag = null;
            DialogResult ret = DialogResult.OK;
            while (ret == DialogResult.OK)
            {
                thePass.Text = "Please enter the CURRENT account passphrase:";
                ret = thePass.ShowDialog(OriginalOwner);
                if (ret == DialogResult.OK)
                {
                    resultingPassPhrase = (string)thePass.Tag;
                    theChallangePass = Convert.ToBase64String(GrindPassPhrase(resultingPassPhrase, _salt));
                    if (string.Compare(existingPass, theChallangePass, false) != 0)
                    {
                        if (--bruteCount == 0)
                        {
                            BruteForceDetected(this, new EventArgs());
                            return false;
                        }
                        MessageBox.Show(OriginalOwner, "The passphrases is incorrect.  Please try again.", "PASSPHRASE NOT ACCEPTED",
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
        /// <param name="OriginalOwner">The calling window handle.</param>
        /// <returns>true if the user provides the correct existing passphrase and succsessfully provides a new passphrase.</returns>
        public bool Change(ref Dictionary<string, TOTPAccount> TheAccounts, IWin32Window OriginalOwner)
        {
            if (!this.Initialized)
                throw new InvalidOperationException("Cannot change passphrase if one has not been established!");
            if (TheAccounts == null)
                throw new InvalidOperationException("Cannot change passphrase on the existing accounts if they are not passed in.");
            if (!this.Challange(OriginalOwner))
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
                ret = myNewPass.ShowDialog(OriginalOwner);
                resultingPassPhrase = (string)myNewPass.Tag;
                myNewPass.Tag = null;
                if (ret == System.Windows.Forms.DialogResult.OK)
                {
                    myNewPass.Text = "Please CONFIRM the entered passphrase:";
                    ret = myNewPass.ShowDialog(OriginalOwner);
                    if (string.Compare((string)myNewPass.Tag, resultingPassPhrase, false) != 0 && ret == DialogResult.OK)
                        MessageBox.Show(OriginalOwner, "The two passphrases did not match.  Please try again.", "PASSPHRASE NOT ACCEPTED",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            myNewPass.Dispose();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                NEWSECRETKEY = GrindPassPhrase(resultingPassPhrase);  //don't pass the old salt, create a new one
                //
                //    #warning This doesn't protect anything!
                //
                _hashedSecret = NEWSECRETKEY;
                Exception saveProblem = null;
                if (!AccountXMLPersistance11.PutEncryptedAccounts(null, TheAccounts, this, true, out saveProblem))
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
            _salt = null;
        }

        internal byte[] MakeSessionKey(ref byte[] sessionSalt)
        {
            PBKDF2<HMACSHA512> myPBKDF2 = null;
            if (sessionSalt == null)
            {
                myPBKDF2 = new PBKDF2<HMACSHA512>(this._hashedSecret, 16, ITERATIONS);
                sessionSalt = myPBKDF2.Salt;
            }
            else
            {
                myPBKDF2 = new PBKDF2<HMACSHA512>(this._hashedSecret, sessionSalt, ITERATIONS);
            }
            return myPBKDF2.GetBytes(32);
        }

        public XElement EncryptXElement(XElement Source, string ElementName, string IdAttribute)
        {
            XElement xeAccount = new XElement(ElementName);
            xeAccount.SetAttributeValue("id", IdAttribute);
            byte[] buffer = null;
            byte[] useableKey = null; // this will be generated by MakeSessionKey
            byte[] salt = null;  // this will be generated by MakeSessionKey
            // the payload will be encrypted and stuffed into XML later on
            SSC.RNGCryptoServiceProvider myRNG = new SSC.RNGCryptoServiceProvider();
            buffer = new byte[32]; // set up a buffer for noise
            myRNG.GetBytes(buffer); // get the noise
            // this noise is to randomize the contents of the file during encryption so that each block is unique.
            // since we'll be using a CBC mode, the first block and the IV (salt) are critical to be random.
            // the leading "n" is required because elements cannot have names starting with numbers
            XElement payload = new XElement("n" + Base32Encoder.ToBase32String(buffer).Replace("=", "") + "_noise");
            useableKey = MakeSessionKey(ref salt);
            //now we're ready to do this
            //payload.Add(new XElement("id", idAttribute));
            payload.Add(Source);
            UTF8Encoding theEncoder = new UTF8Encoding();
            buffer = AESEncipher(theEncoder.GetBytes(payload.ToString()), useableKey, salt);
            // ok, last minute stuff and we're done

            xeAccount.Add(new XElement("payload", Convert.ToBase64String(buffer, Base64FormattingOptions.InsertLineBreaks)));
            xeAccount.Add(new XElement("salt", Convert.ToBase64String(salt))); // because the IV is random is necessary for the usable key, so give it here.
            // it's ok, this only leaks a small amount of information that would compromise the usable key, since the key is still 
            // user supplied and hashed before AND after the IV is used, and the IV is not derived from the key
            return xeAccount;
        }

        public XElement DecryptXElement(XElement Source, string ExpectedElement)
        {
            string payload = Source.Element("payload").Value;
            byte[] buffer = Convert.FromBase64String(payload);
            string sourceIV = Source.Element("salt").Value;
            byte[] salt = Convert.FromBase64String(sourceIV);
            byte[] useableKey = MakeSessionKey(ref salt);
            //string sourceSaltedNameHash = source.Attribute("saltednamehash").Value;

            buffer = AESDecipher(buffer, useableKey, salt);
            if (buffer == null) // something went wrong
                return null;
            UTF8Encoding encoder = new UTF8Encoding();
            XElement result = XElement.Parse(encoder.GetString(buffer));
            result = result.Element(ExpectedElement); //"TOTPAccount");
            return result;
        }

        internal byte[] AESEncipher(byte[] Stuff, byte[] Key, byte [] Iv)
        {
            try
            {
                SSC.AesManaged a = new SSC.AesManaged();
                a.Mode = SSC.CipherMode.CBC;
                a.Padding = SSC.PaddingMode.ANSIX923;
                SSC.ICryptoTransform ict = a.CreateEncryptor(Key, Iv);
                byte[] ct = ict.TransformFinalBlock(Stuff, 0, Stuff.Length);
                return ct;
            }
            catch //(Exception ex)
            {
                //throw ex;
                //MessageBox.Show(frmMain.ActiveForm, ex.ToString(), "EXCEPTION ON DECRYPT");
                return null;
            }
        }

        internal byte[] AESDecipher(byte[] Stuff, byte[] Key, byte[] Iv)
        {
            try
            {
                SSC.AesManaged a = new SSC.AesManaged();
                a.Mode = SSC.CipherMode.CBC;
                a.Padding = SSC.PaddingMode.ANSIX923;
                SSC.ICryptoTransform ict = a.CreateDecryptor(Key, Iv);
                byte[] ct = ict.TransformFinalBlock(Stuff, 0, Stuff.Length);
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
