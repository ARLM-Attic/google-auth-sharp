using System;
using System.Text;
using System.Xml;
using SSC = System.Security.Cryptography;

namespace GoogleAuthClone
{   
    public class TOTPAccount
    {
        public enum TOTPAlgorithm : byte
        {
            SHA1 = 0,
            SHA256 = 1,
            SHA512 = 2,
            MD5 = 3
        }

        private const string URIHeader = "otpauth://totp/";

        private int _period = 30;
        public int Period
        {
            get { return _period; }
            set
            {
                _period = Math.Abs(value) % 86400;
                if (_period < 5)
                    _period = 5;
            }
        }

        private byte _digits = 6;
        public byte Digits
        {
            get { return _digits; }
            set 
            {
                _digits = value;
                if (value < 6)
                    _digits = 6;
                if (value > 10)
                    _digits = 10;
            }
        }

        public TOTPAlgorithm Algorithm { get; set; }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set 
            {
                // if statement only used if the name is part of the encryption process for the secret
                //if (string.IsNullOrWhiteSpace(_encryptedSecret))
                _name = value;
                //else
                //    throw new System.InvalidOperationException("Cannot rename account after secret has been set.");
            }
        }
  
        private string _encryptedSecret = string.Empty;
        public string EncryptedSecret { get { return _encryptedSecret; } }

        public void SetEncryptedSecret(string base32Secret, byte[] key)
        {
            this.SetEncryptedSecret(Base32Encoder.FromBase32String(base32Secret), key);
        }

        public void SetEncryptedSecret(byte[] secret, byte[] key)
        {
            if (key == null || key.Length != 64)
                throw new ArgumentException("Key must be length 64 bytes (512 bits) and not null","key");
            if (secret == null || secret.Length < 10)
                throw new ArgumentException("Secret must be length 10 bytes (80 bits) or greater and not null", "secret");
            // only required if using name as part of encryption
            //if (string.IsNullOrEmpty(Name))
            //    throw new System.InvalidOperationException("This account must be named first before setting its secret");
            
            _encryptedSecret = System.Convert.ToBase64String( 
                SkeinLib.ThreeFish.ThreeFish512Bytes(
                secret, 
                key, 
                new byte[] { 0xFF, 0xEE, 0xDD })
            );
        }

        public byte[] GetDecryptedSecret(byte[] key)
        {
            if (string.IsNullOrWhiteSpace(EncryptedSecret))
                return null;
            if (key == null || key.Length != 64)
                throw new ArgumentException("Key must be length 64 bytes (512 bits) and not null", "key");
            return SkeinLib.ThreeFish.InvThreeFish512Bytes(
                System.Convert.FromBase64String(EncryptedSecret), 
                key, 
                new byte[] { 0xFF, 0xEE, 0xDD });
        }

        public string GetDecryptedSecret(byte[] key, bool asBase32OrBase64)
        {
            if (string.IsNullOrWhiteSpace(EncryptedSecret))
                return string.Empty;
            if (asBase32OrBase64)
                return Base32Encoder.ToBase32String(this.GetDecryptedSecret(key));
            else
                return Convert.ToBase64String(this.GetDecryptedSecret(key));
        }

        public void Clear()
        {
            _name = string.Empty;
            _encryptedSecret = string.Empty;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string ToString(byte[] key)
        {
            System.Text.UTF8Encoding myEncoder = new UTF8Encoding();
            StringBuilder sb = new StringBuilder();

            sb.Append(URIHeader); // REQUIRED
            sb.Append(Name); // REQUIRED
            sb.Append("?secret=" + GetDecryptedSecret(key, true)); // REQUIRED
            if (Algorithm != TOTPAlgorithm.SHA1)
                sb.Append("&algorithm=" + Algorithm.ToString()); //OPTIONAL
            if (Digits != 6)
                sb.Append("&digits=" + Digits.ToString()); //OPTIONAL
            if (Period != 30)
                sb.Append("&period=" + Period.ToString()); //OPTIONAL
            return sb.ToString();
        }

        public static TOTPAccount FromString(string inString, byte[] key)
        {
            if (!inString.StartsWith(URIHeader, false, System.Globalization.CultureInfo.InvariantCulture))
                return null;
            if (!inString.Contains("?secret="))
                return null;
            TOTPAccount tempAcc = new TOTPAccount();
            byte[] tempSecret = null;
            try
            {
                inString = inString.Replace(URIHeader, "");
                tempAcc.Name = inString.Substring(0, inString.IndexOf("?"));
                inString = inString.Replace(tempAcc.Name + '?', "");

                string[] inParams = inString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string thing in inParams)
                {
                    string[] pieces = thing.Split(new char[] { '=' });
                    switch (pieces[0].ToLower())
                    {
                        case "secret": tempSecret = Base32Encoder.FromBase32String(pieces[1]); break;
                        case "digits": tempAcc.Digits = byte.Parse(pieces[1]); break;
                        case "period": tempAcc.Period = int.Parse(pieces[1]); break;
                        case "algorithm":
                            switch (pieces[1].ToUpper())
                            {
                                case "SHA1": tempAcc.Algorithm = TOTPAlgorithm.SHA1; break;
                                case "SHA256": tempAcc.Algorithm = TOTPAlgorithm.SHA256; break;
                                case "SHA512": tempAcc.Algorithm = TOTPAlgorithm.SHA512; break;
                                case "MD5": tempAcc.Algorithm = TOTPAlgorithm.MD5; break;
                                default:
                                    return null;
                            }
                            break;
                    }
                }
                //check for malformation or missing required information
                if (tempSecret == null)
                    return null;
                if (string.IsNullOrWhiteSpace(tempAcc.Name))
                    return null;
                
                tempAcc.SetEncryptedSecret(tempSecret, key);

                return tempAcc;
            }
            catch
            {
                return null;
            }
        }

        //ACTUAL CALCULATION METHODS
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ComputePIN(byte[] key, DateTime when)
        {
            SSC.HMAC myAlg = null;
            switch (Algorithm)
            {
                case TOTPAlgorithm.SHA1: myAlg = new SSC.HMACSHA1(this.GetDecryptedSecret(key)); break;
                case TOTPAlgorithm.SHA256: myAlg = new SSC.HMACSHA256(this.GetDecryptedSecret(key)); break;
                case TOTPAlgorithm.SHA512: myAlg = new SSC.HMACSHA512(this.GetDecryptedSecret(key)); break;
                case TOTPAlgorithm.MD5: myAlg = new SSC.HMACMD5(this.GetDecryptedSecret(key)); break;
            }        
            UInt64 theInterval = GetTimeCode(when, Period);
            byte[] theIntervalsBytes = BitConverter.GetBytes(theInterval);
            Array.Reverse(theIntervalsBytes);
            byte[] hashResult = myAlg.ComputeHash(theIntervalsBytes);
            myAlg.Clear();
            return ExtractDTB(hashResult, Digits);
        }

        /// <summary>
        /// Take a source byte array and extract an X digit code from a variable (but deterministic) location within the array.
        /// </summary>
        /// <param name="source">The source byte array from which to extract the digits.</param>
        /// <param name="digits">The number of digits to extract. Range = 6 to 9</param>
        /// <returns>The calculated digit string extracted from the byte array.</returns>
        internal static string ExtractDTB(byte[] source, byte digits)
        {
            if (source.Length < 15)
                throw new ArgumentException("The source array contains too few bytes to extract a DTB.");
            long[] pow10 = new long[] { 1000000, 10000000, 100000000, 1000000000, 10000000000 };
            byte offset = (byte)(source[source.GetUpperBound(0)] & 0x0f);
            long code = (source[offset] & 0x7f) << 24;
            code += (source[offset + 1] & 0xff) << 16;
            code += (source[offset + 2] & 0xff) << 8;
            code += (source[offset + 3] & 0xff);
            code %= pow10[digits - 6];
            string format = new string('0', digits);
            return code.ToString(format);
        }

        /// <summary>
        /// Get the specific interval count in time to authenticate.
        /// </summary>
        /// <param name="theDateTime">A DateTime object with the specific date after 1970-01-01 at midnight.</param>
        /// <param name="interval">The interval in seconds.</param>
        /// <returns>A 64 bit integer representing the specific interval in time against which to verify.</returns>
        internal static UInt64 GetTimeCode(DateTime theDateTime, int interval)
        {
            if (DateTime.Compare(DateTime.Parse("1970/01/01 12:00:00AM"), theDateTime) > 0)
                throw new InvalidOperationException("The DateTime object must represent a date after Jan 1, 1970 at midnight!");
            UInt64 epoch = (UInt64)((theDateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
            return epoch / (UInt64)(interval);
        }

    }
}
