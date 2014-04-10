using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SSC = System.Security.Cryptography;

namespace GoogleAuthClone
{   
    public class TOTPAccount
    {
        private static Guid nameHashSalt = new Guid("83C3DFDC-8A16-C1B2-9D6A-58421FD883A5");
        //"otpauth://totp/Microsoft:dsparks@colossusconsulting.com?secret=XEX4J3VYKZG5SVP5&issuer=Microsoft"
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
                _nameHash = PreviewNameHash(_name);
                //else
                //    throw new System.InvalidOperationException("Cannot rename account after secret has been set.");
            }
        }

        private string _nameHash = string.Empty;
        public string SaltedNameHash { get { return _nameHash; } }

        private string _encodedSecret = string.Empty;
        public string EncodedSecret { get { return _encodedSecret; } }

        private string _issuer = string.Empty;
        public string Issuer { get { return _issuer; } set { _issuer = value; } }

        public static string PreviewNameHash(string name)
        {
            string result = string.Empty;
            UTF8Encoding myEncoder = new UTF8Encoding();
            if (name.Length < 256)
                name += new string('$', 256 - name.Length);
            SSC.HMACSHA256 mySha = new SSC.HMACSHA256(nameHashSalt.ToByteArray());
            byte[] buffer = mySha.ComputeHash(myEncoder.GetBytes(name));
            mySha.Clear();
            for (int i = 0; i < 256; i++)
            {
                mySha = new SSC.HMACSHA256();
                buffer[0] = (byte)(i % 256);
                buffer[1] = (byte)((buffer[1] + i) % 256);
                mySha.Key = buffer;
                buffer = mySha.ComputeHash(myEncoder.GetBytes(name));
                mySha.Clear();
            }
            mySha = new SSC.HMACSHA256(nameHashSalt.ToByteArray());
            result = Base32EncoderAlt.ToBase32String(mySha.ComputeHash(myEncoder.GetBytes(name))).Substring(0, 20);
            return result;
        }

        public void SetEncodedSecret(string base32Secret)
        {
            _encodedSecret = base32Secret;
        }

        public void SetEncodedSecret(byte[] rawSecret)
        {
            _encodedSecret = Base32EncoderAlt.ToBase32String(rawSecret);
        }

        public byte[] GetSecret()
        {
            if (string.IsNullOrWhiteSpace(EncodedSecret))
                return null;
            return Base32EncoderAlt.FromBase32String(EncodedSecret);
        }

        public void Clear()
        {
            _name = string.Empty;
            _nameHash = string.Empty;
            _encodedSecret = string.Empty;
            _issuer = string.Empty;
        }

        public TOTPAccount Clone()
        {
            return TOTPAccount.FromUriString(this.ToUriString());
        }

        public override string ToString()
        {
            return this.Name;
        }

        //example URI for ToUriString and FromUriString
        //   otpauth://totp/some.email.address@gmail.com?secret=cbtu2gs6uesagw3p&digits=6&period=30

        public string ToUriString(bool verbose = false)
        {
            System.Text.UTF8Encoding myEncoder = new UTF8Encoding();
            StringBuilder sb = new StringBuilder();

            sb.Append(URIHeader); // REQUIRED
            sb.Append(this.Name); // REQUIRED
            sb.Append("?secret=" + this.EncodedSecret); // REQUIRED
            if (verbose || this.Algorithm != TOTPAlgorithm.SHA1)
                sb.Append("&algorithm=" + this.Algorithm.ToString()); //OPTIONAL
            if (verbose || this.Digits != 6)
                sb.Append("&digits=" + this.Digits.ToString()); //OPTIONAL
            if (verbose || this.Period != 30)
                sb.Append("&period=" + this.Period.ToString()); //OPTIONAL
            if (verbose || !string.IsNullOrWhiteSpace(this.Issuer)) // OPTIONAL
                sb.Append("&issuer=" + this.Issuer); //OPTIONAL
            return sb.ToString();
        }

        public XElement ToXElement(bool includeNameHash)
        {
            XElement xeAccount = new XElement("TOTPAccount");
            if (includeNameHash)
                xeAccount.SetAttributeValue("saltednamehash", this.SaltedNameHash);
            xeAccount.Add(new XElement("name", this.Name));
            xeAccount.Add(new XElement("secret", this.EncodedSecret));
            if (this.Algorithm != TOTPAlgorithm.SHA1)
                xeAccount.Add(new XElement("algorithm", this.Algorithm.ToString()));
            if (this.Digits != 6)
                xeAccount.Add(new XElement("digits", this.Digits));
            if (this.Period != 30)
                xeAccount.Add(new XElement("period", this.Period));
            if (!string.IsNullOrWhiteSpace(this.Issuer))
                xeAccount.Add(new XElement("issuer", this.Issuer));
            return xeAccount;
        }

        public static TOTPAccount FromUriString(string inString)
        {
            if (string.IsNullOrWhiteSpace(inString))
                return null;
            if (!inString.StartsWith(URIHeader, false, System.Globalization.CultureInfo.InvariantCulture))
                return null;
            if (!inString.Contains("?secret=") && !inString.Contains("&secret="))
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
                        case "secret": 
                            //Bug Fix:  make sure secret is lower case
                            tempSecret = Base32EncoderAlt.FromBase32String(pieces[1].ToLower()); 
                            break;
                        case "digits": tempAcc.Digits = byte.Parse(pieces[1]); break;
                        case "period": tempAcc.Period = int.Parse(pieces[1]); break;
                        case "issuer": tempAcc.Issuer = pieces[1]; break;
                        case "algorithm":
                            if (Enum.IsDefined(typeof(TOTPAlgorithm), pieces[1].ToUpperInvariant()))
                            {
                                tempAcc.Algorithm = (TOTPAlgorithm)Enum.Parse(
                                   typeof(TOTPAlgorithm), pieces[1].ToUpperInvariant());
                            }
                            else
                                return null;
                            break;
                    }
                }
                //check for malformation or missing required information
                if (tempSecret == null)
                    return null;
                if (string.IsNullOrWhiteSpace(tempAcc.Name))
                    return null;
                
                tempAcc.SetEncodedSecret(tempSecret);

                return tempAcc;
            }
            catch //(Exception ex)
            {
                return null;
            }
        }

        public static TOTPAccount FromXElement(XElement source)
        {
            if (source == null)
                return null;
            try
            {
                string tryThis = URIHeader +
                    source.Element("name").Value +
                    "?secret=" + source.Element("secret").Value;
                if (source.Element("algorithm") != null)
                    tryThis += "&algorithm=" + source.Element("algorithm").Value;
                if (source.Element("period") != null)
                    tryThis += "&period=" + source.Element("period").Value;
                if (source.Element("digits") != null)
                    tryThis += "&digits=" + source.Element("digits").Value;
                if (source.Element("issuer") != null)
                    tryThis += "&issuer=" + source.Element("issuer").Value;
                return FromUriString(tryThis);
            }
            catch
            {
                return null;
            }//*/
        }

        //ACTUAL CALCULATION METHODS
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        public string ComputePIN(DateTime when)
        {
            SSC.HMAC myAlg = null;
            switch (Algorithm)
            {
                case TOTPAlgorithm.SHA1: myAlg = new SSC.HMACSHA1(this.GetSecret()); break;
                case TOTPAlgorithm.SHA256: myAlg = new SSC.HMACSHA256(this.GetSecret()); break;
                case TOTPAlgorithm.SHA512: myAlg = new SSC.HMACSHA512(this.GetSecret()); break;
                case TOTPAlgorithm.MD5: myAlg = new SSC.HMACMD5(this.GetSecret()); break;
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
                throw new InvalidOperationException("The DateTime object must represent a date after Jan 1, 1970 at midnight (UTC)!");
            UInt64 epoch = (UInt64)((theDateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
            return epoch / (UInt64)(interval);
        }

        public Single PercentIntervalElapsed(DateTime theDateTime)
        {
            long epoch = ((theDateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
            long rem;
            Math.DivRem(epoch, this.Period, out rem);
            Single result = (Single)rem / (long)this.Period;
            return result;
        }

    }
}
