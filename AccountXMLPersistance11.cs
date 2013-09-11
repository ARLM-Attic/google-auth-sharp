using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace GoogleAuthClone
{
    public class AccountXMLPersistance11
    {
        const string PersistedFileName = "Accounts.xml";
        [Obsolete]
        const string PersistedLegacyFileName = "Accounts.dat";

        [Obsolete]
        static public bool CheckForLegacyAccounts(out bool FileExists, out Exception ExceptionArg)
        {
            ExceptionArg = null;
            FileExists = false;
            try
            {
                string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance11)).CodeBase;
                string thePath = Path.GetFullPath(
                    theAssembly.Replace("file:///", "")).Replace(
                    Path.GetFileName(theAssembly), "");
                string theFile = thePath + PersistedLegacyFileName; // it's fine here, we are LOOKING for legacy data
                if (!File.Exists(theFile))
                {
                    return false;
                }
                FileExists = true;
                string rawStuff = File.ReadAllText(theFile);
                if (!string.IsNullOrWhiteSpace(rawStuff))
                {
                    byte[] buffer = Convert.FromBase64String(rawStuff);
                    if (buffer != null && buffer.Length > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (System.ArgumentException aex)
            {
                if (aex.Message == "URI formats are not supported.")
                {
                    aex = null;
                    ExceptionArg = new System.IO.FileLoadException("Network folders (or redirected folders that point to the network) are not currently supported.  If a network location must be used, map it to a drive letter.");
                }
                else
                    ExceptionArg = aex;
                return false;
            }
            catch (Exception ex)
            {
                ExceptionArg = ex;
                return false;
            }
        }//*/

        [Obsolete]
        static public bool RenameLegacy(out Exception ExceptionArg)
        {
            ExceptionArg = null;
            try
            {
                string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance11)).CodeBase;
                string thePath = Path.GetFullPath(
                    theAssembly.Replace("file:///", "")).Replace(
                    Path.GetFileName(theAssembly), "");
                string theFile = thePath + PersistedLegacyFileName; // this is ok, we're LOOKING for lagacy stuff
                if (!File.Exists(theFile))
                {
                    return false;
                }
                FileInfo fi = new FileInfo(theFile);
                string importDateString =
                    DateTime.Now.ToString("yyyyMMMdd_HHmmss");

                fi.MoveTo(theFile + "IMPORTED_" + importDateString);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionArg = ex;
                return false;
            } //*/
        }

        static public bool RenameInvalidAccountsFile(out Exception ExceptionArg)
        {
            ExceptionArg = null;
            try
            {
                string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance11)).CodeBase;
                string thePath = Path.GetFullPath(
                    theAssembly.Replace("file:///", "")).Replace(
                    Path.GetFileName(theAssembly), "");
                string theFile = thePath + PersistedFileName;
                if (!File.Exists(theFile))
                {
                    return false;
                }
                FileInfo fi = new FileInfo(theFile);
                string importDateString =
                    DateTime.Now.ToString("yyyyMMMdd_HHmmss");

                fi.MoveTo(theFile + "NOTVALID_" + importDateString);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionArg = ex;
                return false;
            } //*/
        }

        static public bool CheckForEncryptedAccounts(string filename, out bool FileExists, out bool IsValidXml, out byte[] MasterSalt, out Exception ExceptionArg)
        {
            ExceptionArg = null;
            MasterSalt = null;
            FileExists = false;
            IsValidXml = false;
            try
            {
                string theFile = filename;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance11)).CodeBase;
                    string thePath = Path.GetFullPath(
                        theAssembly.Replace("file:///", "")).Replace(
                        Path.GetFileName(theAssembly), "");
                    theFile = thePath + PersistedFileName;
                }
                XDocument xdoc = null;
                if (!File.Exists(theFile))
                {
                    return false;
                }
                FileExists = true;
                xdoc = XDocument.Parse(File.ReadAllText(theFile));
                if (xdoc != null)
                {
                    IsValidXml = true;
                    IEnumerable<XElement> xeAccounts =
                        from el in xdoc.Root.Elements("EncryptedTOTPAccount")
                        select el;
                    if (xeAccounts.Count() > 0)
                    {
                        if (xdoc.Root.Attribute("version") != null &&
                            !string.IsNullOrWhiteSpace(xdoc.Root.Attribute("version").Value) &&
                            xdoc.Root.Attribute("version").Value == "1.1")
                        {
                            if (xdoc.Root.Attribute("mastersalt") != null &&
                                !string.IsNullOrWhiteSpace(xdoc.Root.Attribute("mastersalt").Value))
                            {
                                MasterSalt = Convert.FromBase64String(xdoc.Root.Attribute("mastersalt").Value);
                                return true;
                            }
                            else 
                                return false;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (System.ArgumentException aex)
            {
                if (aex.Message == "URI formats are not supported.")
                {
                    aex = null;
                    ExceptionArg = new System.IO.FileLoadException("Network folders (or redirected folders that point to the network) are not currently supported.  If a network location must be used, map it to a drive letter.");
                }
                else
                    ExceptionArg = aex;
                return false;
            }
            catch (Exception ex)
            {
                ExceptionArg = ex;
                return false;
            }//*/
        }

        static public bool CheckForAccounts(string Filename, out bool FileExists, out bool IsValidXml, out Exception ExceptionArg)
        {
            ExceptionArg = null;
            FileExists = false;
            IsValidXml = false;
            try
            {
                string theFile = Filename;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance11)).CodeBase;
                    string thePath = Path.GetFullPath(
                        theAssembly.Replace("file:///", "")).Replace(
                        Path.GetFileName(theAssembly), "");
                    theFile = thePath + PersistedFileName;
                }
                XDocument xdoc = null;
                if (!File.Exists(theFile))
                {
                    return false;
                }
                FileExists = true;
                xdoc = XDocument.Parse(File.ReadAllText(theFile));
                if (xdoc != null)
                {
                    IsValidXml = true;
                    IEnumerable<XElement> xeAccounts =
                        from el in xdoc.Root.Elements("TOTPAccount")
                        select el;
                    if (xeAccounts.Count() > 0)
                    {
                        //Don't worry about the version 1.1 requriement for UNENCRYPTED ACCOUNTS ONLY, this is an upgrade path
                        /*if (xdoc.Root.Attribute("version") != null &&
                            !string.IsNullOrWhiteSpace(xdoc.Root.Attribute("version").Value) &&
                            xdoc.Root.Attribute("version").Value == "1.1") */
                            return true;
                        //else
                        //    return false;
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (System.ArgumentException aex)
            {
                if (aex.Message == "URI formats are not supported.")
                {
                    aex = null;
                    ExceptionArg = new System.IO.FileLoadException("Network folders (or redirected folders that point to the network) are not currently supported.  If a network location must be used, map it to a drive letter.");
                }
                else
                    ExceptionArg = aex;
                return false;
            }
            catch (Exception ex)
            {
                ExceptionArg = ex;
                return false;
            }//*/
        }

        static public bool PutEncryptedAccounts(string Filename, Dictionary<string, TOTPAccount> Accounts, AccountPassPhrase11 appStore, bool overwrite, out Exception exceptionArg)
        {
            //if Filename is null/empty/whitespace, then save to the program directory
            exceptionArg = null;
            try
            {
                string theFile = Filename;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance11)).CodeBase;
                    string thePath = Path.GetFullPath(
                        theAssembly.Replace("file:///", "")).Replace(
                        Path.GetFileName(theAssembly), "");
                    theFile = thePath + PersistedFileName;
                }
                if (System.IO.File.Exists(Filename) && !overwrite)
                {
                    exceptionArg = new InvalidOperationException("Cannot overwrite an existing file if the 'overwrite' parameter is set to false.");
                    return false;
                }
                XElement xeAccounts = new XElement("Accounts");
                xeAccounts.SetAttributeValue("version", "1.1");
                xeAccounts.SetAttributeValue("mastersalt", Convert.ToBase64String(appStore.MasterSalt));
                foreach (TOTPAccount acc in Accounts.Values)
                {
                    // actual encryption is handled in the AccountPassPhrase class now
                    xeAccounts.Add(appStore.EncryptXElement(acc.ToXElement(true), "EncryptedTOTPAccount", acc.SaltedNameHash));
                }
                XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), xeAccounts);
                StreamWriter myStream = File.CreateText(theFile);
                myStream.Write(xdoc);
                myStream.Flush();
                myStream.Close();
                return true;
            }
            catch (System.ArgumentException aex)
            {
                if (aex.Message == "URI formats are not supported.")
                {
                    aex = null;
                    exceptionArg = new System.IO.FileLoadException("Network folders (or redirected folders that point to the network) are not currently supported.  If a network location must be used, map it to a drive letter.");
                }
                else
                    exceptionArg = aex;
                return false;
            }
            catch (Exception ex)
            {
                exceptionArg = ex;
                return false;
            }//*/
        }

        static public bool PutAccounts(string Filename, Dictionary<string, TOTPAccount> Accounts, bool overwrite, out Exception exceptionArg)
        {
            //if Filename is null/empty/whitespace, then save to the program directory
            exceptionArg = null;
            try
            {
                string theFile = Filename;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance11)).CodeBase;
                    string thePath = Path.GetFullPath(
                        theAssembly.Replace("file:///", "")).Replace(
                        Path.GetFileName(theAssembly), "");
                    theFile = thePath + PersistedFileName;
                }
                if (System.IO.File.Exists(Filename) && !overwrite)
                {
                    exceptionArg = new InvalidOperationException("Cannot overwrite an existing file if the 'overwrite' parameter is set to false.");
                    return false;
                }
                XElement xeAccounts = new XElement("Accounts");
                xeAccounts.SetAttributeValue("version", "1.1");
                foreach (TOTPAccount acc in Accounts.Values)
                {
                    // actual encryption is handled in the AccountPassPhrase class now
                    xeAccounts.Add(acc.ToXElement(false));
                }
                XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), xeAccounts);
                StreamWriter myStream = File.CreateText(theFile);
                myStream.Write(xdoc);
                myStream.Flush();
                myStream.Close();
                return true;
            }
            catch (System.ArgumentException aex)
            {
                if (aex.Message == "URI formats are not supported.")
                {
                    aex = null;
                    exceptionArg = new System.IO.FileLoadException("Network folders (or redirected folders that point to the network) are not currently supported.  If a network location must be used, map it to a drive letter.");
                }
                else
                    exceptionArg = aex;
                return false;
            }
            catch (Exception ex)
            {
                exceptionArg = ex;
                return false;
            }//*/
        }

        static public Dictionary<string, TOTPAccount> GetEncryptedAccounts(
            string Filename, AccountPassPhrase11 AppStore, out bool FoundData, out bool WasAbleToDecryptData, out Exception ExceptionArg)
        {
            ExceptionArg = null;
            Dictionary<string, TOTPAccount> results = new Dictionary<string,TOTPAccount>();
            FoundData = false;
            WasAbleToDecryptData = false;
            if (!AppStore.Initialized || AppStore.MasterSalt == null)
            {
                ExceptionArg = new InvalidOperationException("User credentials not set or do not include a Master Salt. Unable to proceed.");
                return null;
            }   
            try
            {
                string theFile = Filename;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance11)).CodeBase;
                    string thePath = Path.GetFullPath(
                        theAssembly.Replace("file:///", "")).Replace(
                        Path.GetFileName(theAssembly), "");
                    theFile = thePath + PersistedFileName;
                }
                XDocument xdoc = null;
                if (!File.Exists(theFile))
                {
                    ExceptionArg = new InvalidOperationException("File specified does not exist.");
                    return null;
                }
                else
                    xdoc = XDocument.Parse(File.ReadAllText(theFile));
                if (xdoc != null)
                {
                    if (xdoc.Root.Attribute("version") != null &&
                            !string.IsNullOrWhiteSpace(xdoc.Root.Attribute("version").Value) &&
                            xdoc.Root.Attribute("version").Value == "1.1")
                    {

                        FoundData = true;
                        IEnumerable<XElement> xeAccounts =
                            from el in xdoc.Root.Elements("EncryptedTOTPAccount")
                            select el;
                        foreach (XElement xeAccount in xeAccounts)
                        {
                            //actual decryption is handled in the AccountPassPhrase class now
                            XElement thing = AppStore.DecryptXElement(xeAccount, "TOTPAccount");
                            TOTPAccount newAcc = TOTPAccount.FromXElement(thing);
                            if (newAcc != null)
                            {
                                WasAbleToDecryptData = true;
                                results.Add(newAcc.Name, newAcc);
                            }
                            // be more fault tolerant, allow a few bad apples
                            //else
                            //    return null;
                        }
                    }
                }

                return results;
            }
            catch (System.ArgumentException aex)
            {
                if (aex.Message == "URI formats are not supported.")
                {
                    aex = null;
                    ExceptionArg = new System.IO.FileLoadException("Network folders (or redirected folders that point to the network) are not currently supported.  If a network location must be used, map it to a drive letter.");
                }
                else
                    ExceptionArg = aex;
                return null;
            }
            catch (Exception ex)
            {
                ExceptionArg = ex;
                return null;
            }//*/
        }

        static public Dictionary<string, TOTPAccount> GetAccounts(string Filename, out bool FoundData, out Exception ExceptionArg)
        {
            ExceptionArg = null;
            Dictionary<string, TOTPAccount> results = new Dictionary<string, TOTPAccount>();
            FoundData = false;
            try
            {
                string theFile = Filename;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance11)).CodeBase;
                    string thePath = Path.GetFullPath(
                        theAssembly.Replace("file:///", "")).Replace(
                        Path.GetFileName(theAssembly), "");
                    theFile = thePath + PersistedFileName;
                }
                XDocument xdoc = null;
                if (!File.Exists(theFile))
                {
                    ExceptionArg = new InvalidOperationException("File specified does not exist.");
                    return null;
                }
                else
                    xdoc = XDocument.Parse(File.ReadAllText(theFile));
                if (xdoc != null)
                {
                    // don't worry about the 1.1 version requirement for UNENCRYPTED ACCOUNTS ONLY, this is an upgrade path
                    /*if (xdoc.Root.Attribute("version") != null &&
                            !string.IsNullOrWhiteSpace(xdoc.Root.Attribute("version").Value) &&
                            xdoc.Root.Attribute("version").Value == "1.1")
                    {*/
                        IEnumerable<XElement> xeAccounts =
                            from el in xdoc.Root.Elements("TOTPAccount")
                            select el;
                        foreach (XElement xeAccount in xeAccounts)
                        {
                            TOTPAccount newAcc = TOTPAccount.FromXElement(xeAccount);
                            if (newAcc != null)
                            {
                                FoundData = true;
                                results.Add(newAcc.Name, newAcc);
                            }
                            // be more fault tolerant, allow a few bad apples
                            //else
                            //    return null;
                        }
                    //}
                }
                return results;
            }
            catch (System.ArgumentException aex)
            {
                if (aex.Message == "URI formats are not supported.")
                {
                    aex = null;
                    ExceptionArg = new System.IO.FileLoadException("Network folders (or redirected folders that point to the network) are not currently supported.  If a network location must be used, map it to a drive letter.");
                }
                else
                    ExceptionArg = aex;
                return null;
            }
            catch (Exception ex)
            {
                ExceptionArg = ex;
                return null;
            }//*/
        }
    }
}
