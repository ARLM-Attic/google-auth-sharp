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
        //example with new 'issuer' query string
        //"otpauth://totp/Microsoft:dsparks@previousjob.com?secret=OOPSXXXXXXX&issuer=Microsoft"
        //OOPS!
        //Kids, don't forget to delete increminating, personal, dangerous, or otherwise not-for-public-consumption
        // comments BEFORE checking in your code... the above commment is now permanently part of the code-base,
        // and USED to actually work on my account.  It doesn't anymore and I've changed my password since then,
        // but this could have been REALLY bad!  #CBSCares

        internal DateTime lastTimeCodeBegins = DateTime.MinValue;
        internal DateTime lastTimeCodeEnds = DateTime.MinValue;
        internal UInt64 lastTimeCodeCalculated = UInt64.MinValue;   //internal cache
        internal string lastCodeCalculated = null;                  //internal cache

        public DateTime CodeExpires
        {
            get { return lastTimeCodeEnds; }
        }

        public enum TOTPAlgorithm : byte
        {
            SHA1 = 0,
            SHA256 = 1,
            SHA512 = 2,
            MD5 = 3
        }

        // July 31st, 2014
        // All parameter "set" commands removed.  
        // The only thing that can be set after object creation is the secret and the name.
        public TOTPAccount(string Name, string Base32Secret, 
            int Period = 30, Byte Digits = 6, TOTPAlgorithm Algorithm = TOTPAlgorithm.SHA1, string Issuer = "")
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Base32Secret))
            {
                throw new InvalidOperationException("Name and Base32Secret are Not Optional, and must contain valid information!");
            }

            _algorithm = Algorithm;
            _digits = Digits;
            _period = Period;
            _issuer = Issuer;
            _name = Name;
            _nameHash = PreviewNameHash(_name);
            SetEncodedSecret(Base32Secret);
        }

        public TOTPAccount(string Name, byte[] Secret,
            int Period = 30, Byte Digits = 6, TOTPAlgorithm Algorithm = TOTPAlgorithm.SHA1, string Issuer = "")
        {
            if (string.IsNullOrWhiteSpace(Name) || Secret == null || Secret.Length < 10)
            {
                throw new InvalidOperationException("Name and Secret are Not Optional, and must contain valid information (Secret must be at least 10 bytes)!");
            }

            _algorithm = Algorithm;
            _digits = Digits;
            _period = Period;
            _issuer = Issuer;
            _name = Name;
            _nameHash = PreviewNameHash(_name);
            SetEncodedSecret(Secret);
        }


        private const string URIHeader = "otpauth://totp/";

        private int _period = 30;
        public int Period
        {
            get { return _period; }
            /*set
            {
                _period = Math.Abs(value) % 86400;
                if (_period < 5)
                    _period = 5;
            }
             */
        }

        private byte _digits = 6;
        public byte Digits
        {
            get { return _digits; }
            /*set 
            {
                _digits = value;
                if (value < 6)
                    _digits = 6;
                if (value > 10)
                    _digits = 10;
            }*/
        }

        private TOTPAlgorithm _algorithm = TOTPAlgorithm.SHA1;
        public TOTPAlgorithm Algorithm 
        {
            get { return _algorithm; }
            //set;
        }

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
        public string Issuer
        { 
            get { return _issuer; }
            //set { _issuer = value; }
        }

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

        /*
        public void Clear() // no longer needed if individual settings cannot be changed, just send to garbage collector
        {
            _name = string.Empty;
            _nameHash = string.Empty;
            _encodedSecret = string.Empty;
            _issuer = string.Empty;
        }
        */

        public TOTPAccount Clone()
        {
            return TOTPAccount.FromUriString(this.ToUriString());
        }

        public override string ToString()
        {
            return this.Name;
        }

        //example URI for ToUriString and FromUriString
        //   otpauth://totp/some.email.address@gmail.com?secret=cb222666uuuccw3p&issuer=Microsoft&digits=6&period=30

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
            //TOTPAccount tempAcc = new TOTPAccount();
            string tempSecret = null;
            string tempName = null;
            int tempPeriod = 30;
            string tempIssuer = null;
            byte tempDigits = 6;
            TOTPAlgorithm tempAlgorithm = TOTPAlgorithm.SHA1;
            try
            {
                inString = inString.Replace(URIHeader, "");
                tempName = inString.Substring(0, inString.IndexOf("?"));
                inString = inString.Replace(tempName + '?', "");

                string[] inParams = inString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string thing in inParams)
                {
                    string[] pieces = thing.Split(new char[] { '=' });
                    switch (pieces[0].ToLower())
                    {
                        case "secret": 
                            //Bug Fix:  make sure secret is lower case
                            tempSecret = pieces[1].ToLower(); 
                            break;
                        case "digits": tempDigits = byte.Parse(pieces[1]); break;
                        case "period": tempPeriod = int.Parse(pieces[1]); break;
                        case "issuer": tempIssuer = pieces[1]; break;
                        case "algorithm":
                            if (Enum.IsDefined(typeof(TOTPAlgorithm), pieces[1].ToUpperInvariant()))
                            {
                                tempAlgorithm = (TOTPAlgorithm)Enum.Parse(
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
                if (string.IsNullOrWhiteSpace(tempName))
                    return null;

                TOTPAccount tempAcc = new TOTPAccount(tempName, tempSecret, 
                    tempPeriod, tempDigits, tempAlgorithm, tempIssuer);

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

        public string ComputePIN()
        {
            return ComputePIN(DateTime.Now);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        public string ComputePIN(DateTime when)
        {
            //updated to cache the last code to ease updating displays
            //load should spike when new time code is used, else should be low while using cached code
            UInt64 theInterval = GetTimeCode(when, Period);
            if (theInterval == lastTimeCodeCalculated && !string.IsNullOrWhiteSpace(lastCodeCalculated))
                return lastCodeCalculated;
            else
            {
                lastTimeCodeCalculated = theInterval;
                lastTimeCodeBegins = GetDateFromTimeCode(lastTimeCodeCalculated, Period);
                lastTimeCodeEnds = GetDateFromTimeCode(lastTimeCodeCalculated, Period).AddSeconds(Period);
                SSC.HMAC myAlg = null;
                switch (Algorithm)
                {
                    case TOTPAlgorithm.SHA1: myAlg = new SSC.HMACSHA1(this.GetSecret()); break;
                    case TOTPAlgorithm.SHA256: myAlg = new SSC.HMACSHA256(this.GetSecret()); break;
                    case TOTPAlgorithm.SHA512: myAlg = new SSC.HMACSHA512(this.GetSecret()); break;
                    case TOTPAlgorithm.MD5: myAlg = new SSC.HMACMD5(this.GetSecret()); break;
                }
                byte[] theIntervalsBytes = BitConverter.GetBytes(theInterval);
                Array.Reverse(theIntervalsBytes);
                byte[] hashResult = myAlg.ComputeHash(theIntervalsBytes);
                myAlg.Clear();
                lastCodeCalculated = ExtractDTB(hashResult, Digits); //cache it
                return lastCodeCalculated;
            }
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

        internal static DateTime GetDateFromTimeCode(UInt64 DateCode, int Interval)
        {
            Int64 tempTicks = (Int64)((DateCode * (ulong)Interval) * 10000000);
            return DateTime.MinValue.AddTicks(tempTicks + 621355968000000000).ToLocalTime();
        }

        #region User Interface Helper Functions
        public Single PercentIntervalElapsed()//(DateTime theDateTime)
        {
            TimeSpan elapsed = DateTime.Now - lastTimeCodeBegins;
            return (Single)(elapsed.TotalMilliseconds / (Period * 1000));
        }
        
        public Single PercentIntervalRemaining()//(DateTime theDateTime)
        {
            TimeSpan remaining = lastTimeCodeEnds - DateTime.Now;
            return (Single)(remaining.TotalMilliseconds / (Period * 1000));
        }

        public Single SecondsIntervalElapsed()
        {
            TimeSpan elapsed = DateTime.Now - lastTimeCodeBegins;
            return (Single)elapsed.TotalSeconds;
        }

        public Single SecondsIntervalRemaining()
        {
            TimeSpan remaining = lastTimeCodeEnds - DateTime.Now;
            return (Single)remaining.TotalSeconds;
        }
        #endregion
    }
}
