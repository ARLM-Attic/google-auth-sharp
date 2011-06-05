﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoogleAuthClone
{
    public class PassPhrase
    {
        private byte[] _key = null;
        private byte[] _tweak = null;
        private int bruteCount = 10;

        public const string ComplexitySpecialChars = "~!@#$%^&*()_+-=[]{}<>/?.";
        
        public bool Initialized { get { return _key != null; } }

        public event EventHandler BruteForceDetected;

        public static bool MeetsComplexity(string input)
        {
            bool hasUpper = false;
            bool hasLower = false;
            bool hasNumeral = false;
            bool hasSpecial = false;

            foreach (char thing in input.ToCharArray())
            {
                if (ComplexitySpecialChars.Contains(thing))
                    hasSpecial = true;
                short test = -1;
                if (short.TryParse(thing.ToString(), out test) && test >= 0)
                    hasNumeral = true;
                if (thing.ToString() == thing.ToString().ToLower())
                    hasLower = true;
                if (thing.ToString() == thing.ToString().ToUpper())
                    hasUpper = true;
            }
            if (hasUpper && hasLower && hasSpecial && hasNumeral)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Access the passphrase in the raw to use during encryption routines.
        /// </summary>
        /// <returns>byte array of key material</returns>
        public byte[] UseRaw()
        {
            if (!Initialized)
                throw new InvalidOperationException("There is no passphrase to use.  Try using Set first.");
            return (byte[])_key.Clone();
        }

        /// <summary>
        /// Use when passphrase is already in place but hasn't been entered by the user. (existing accounts, on program first start)
        /// </summary>
        /// <param name="originalOwner"></param>
        /// <returns>true if user entered passphrase.</returns>
        public bool GetFromUser(IWin32Window originalOwner)
        {
            if (Initialized)
                throw new InvalidOperationException("Cannot set a new passphrase if it has already been set, use Change instead, or Clear first.");

            string resultingPassPhrase = string.Empty;
            frmPassPhrase myPass = new frmPassPhrase();
            myPass.Tag = null;
            DialogResult ret = DialogResult.OK;

            myPass.Text = "Please enter the account passphrase:";
            ret = myPass.ShowDialog(originalOwner);
            resultingPassPhrase = (string)myPass.Tag;
            myPass.Dispose();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                _key = SkeinLib.Skein512Ex.DeriveKeyFromPassword(
                       resultingPassPhrase,
                       new byte[] { 0xff, 0xee, 0xdd, 0xcc, 0x00, 0x11, 0x22, 0x33 });
                SetTweak();
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
        public bool Set(IWin32Window originalOwner)
        {
            if (Initialized)
                throw new InvalidOperationException("Cannot set a new passphrase if it has already been set, use Change instead, or Clear first.");

            string resultingPassPhrase = string.Empty;
            frmPassPhrase myNewPass = new frmPassPhrase();
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
                _key = SkeinLib.Skein512Ex.DeriveKeyFromPassword(
                       resultingPassPhrase,
                       new byte[] { 0xff, 0xee, 0xdd, 0xcc, 0x00, 0x11, 0x22, 0x33 });
                SetTweak();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Set(byte[] key)
        {
            if (Initialized)
                throw new InvalidOperationException("Cannot set a new passphrase if it has already been set, use Change instead, or Clear first.");
            _key = (byte[])key.Clone();
            SetTweak();
        }

        private void SetTweak()
        {
            if (_key == null)
                return;
            _tweak = SkeinLib.Skein512Ex.DeriveKey(_key, "TWEAK", 128);
        }

        /// <summary>
        /// Use this to challange the user against the stored/processed passphrase.
        /// </summary>
        /// <param name="originalOwner">The calling window handle.</param>
        /// <returns>true if user provides correct passphrase</returns>
        public bool Challange(IWin32Window originalOwner)
        {
            if (!Initialized)
                throw new InvalidOperationException("Cannot challange a non-existant passphrase");

            string resultingPassPhrase = string.Empty;
            string first = Convert.ToBase64String(_key);
            frmPassPhrase thePass = new frmPassPhrase();
            thePass.Tag = null;
            DialogResult ret = DialogResult.OK;
            byte[] thePassBytes = null;
            while (ret == DialogResult.OK)
            {
                thePass.Text = "Please enter the CURRENT account passphrase:";
                ret = thePass.ShowDialog(originalOwner);
                if (ret == DialogResult.OK)
                {
                    resultingPassPhrase = (string)thePass.Tag;
                    thePassBytes = SkeinLib.Skein512Ex.DeriveKeyFromPassword(
                           resultingPassPhrase,
                           new byte[] { 0xff, 0xee, 0xdd, 0xcc, 0x00, 0x11, 0x22, 0x33 });
                    if (string.Compare(first, Convert.ToBase64String(thePassBytes), false) != 0)
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
        /// </summary>
        /// <param name="account">The generic list of TOTPAccounts to change.</param>
        /// <param name="originalOwner">The calling window handle.</param>
        /// <returns>true if the user provides the correct existing passphrase and succsessfully provides a new passphrase.</returns>
        public bool Change(ref List<TOTPAccount> accounts, IWin32Window originalOwner)
        {
            if (!this.Initialized)
                throw new InvalidOperationException("Cannot change passphrase if one has not been established!");
            if (accounts == null)
                throw new InvalidOperationException("Cannot change passphrase on the existing accounts if they are not passed in.");
            if (!this.Challange(originalOwner))
                return false;

            string resultingPassPhrase = string.Empty;
            byte[] NEWSECRETKEY = null;
            frmPassPhrase myNewPass = new frmPassPhrase();
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
                    if (string.Compare((string)myNewPass.Tag, resultingPassPhrase, false) != 0)
                        MessageBox.Show(originalOwner, "The two passphrases did not match.  Please try again.", "PASSPHRASE NOT ACCEPTED",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            myNewPass.Dispose();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                NEWSECRETKEY = SkeinLib.Skein512Ex.DeriveKeyFromPassword(
                       resultingPassPhrase,
                       new byte[] { 0xff, 0xee, 0xdd, 0xcc, 0x00, 0x11, 0x22, 0x33 });
                if (accounts.Count > 0)
                {
                    for (int i = 0; i < accounts.Count; i++)
                    {
                        byte[] buffer = accounts[i].GetDecryptedSecret(_key);
                        accounts[i].SetEncryptedSecret(buffer, NEWSECRETKEY);
                    }
                }
                Buffer.BlockCopy(NEWSECRETKEY, 0, _key, 0, NEWSECRETKEY.Length);
                SkeinLib.SkeinCore.ClearBytes(NEWSECRETKEY);
                SetTweak();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Apply(string stuff)
        {
            if (!Initialized)
                throw new InvalidOperationException("There is no passphrase to apply.  Try using Set first.");
            UTF8Encoding myEncoder = new UTF8Encoding();
            return this.Apply(myEncoder.GetBytes(stuff));
        }

        public string Apply(byte[] stuff)
        {
            if (!Initialized)
                throw new InvalidOperationException("There is no passphrase to apply.  Try using Set first.");
            return Convert.ToBase64String(
                SkeinLib.ThreeFish.ThreeFish512Bytes(stuff, _key, _tweak),
                Base64FormattingOptions.InsertLineBreaks);
        }

        public string RemoveFromString(string inBase64Stuff)
        {
            if (!Initialized)
                throw new InvalidOperationException("There is no passphrase to remove.  Try using Set first.");
            byte[] buffer = Convert.FromBase64String(inBase64Stuff);
            UTF8Encoding myEncoder = new UTF8Encoding();
            return myEncoder.GetString(
                SkeinLib.ThreeFish.InvThreeFish512Bytes(buffer, _key, _tweak));
        }

        public byte[] RemoveFromBytes(string inBase64Stuff)
        {
            if (!Initialized)
                throw new InvalidOperationException("There is no passphrase to remove.  Try using Set first.");
            return SkeinLib.ThreeFish.InvThreeFish512Bytes(
                Convert.FromBase64String(inBase64Stuff),
                _key,
                _tweak);
        }

        public void Clear()
        {
            SkeinLib.SkeinCore.ClearBytes(_key);
            _key = null;
            SkeinLib.SkeinCore.ClearBytes(_tweak);
            _tweak = null;
        }

    }//end class
}
