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
    public class AccountXMLPersistance
    {
        const string PersistedFileName = "Accounts.xml";
        [Obsolete]
        const string PersistedLegacyFileName = "Accounts.dat";

        [Obsolete]
        static public bool CheckForLegacyAccounts(out bool fileExists, out Exception exceptionArg)
        {
            exceptionArg = null;
            fileExists = false;
            try
            {
                string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance)).CodeBase;
                string thePath = Path.GetFullPath(
                    theAssembly.Replace("file:///", "")).Replace(
                    Path.GetFileName(theAssembly), "");
                string theFile = thePath + PersistedLegacyFileName;
                if (!File.Exists(theFile))
                {
                    return false;
                }
                fileExists = true;
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
            }
        }//*/

        static public bool RenameLegacy(out Exception exceptionArg)
        {
            exceptionArg = null;
            try
            {
                string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance)).CodeBase;
                string thePath = Path.GetFullPath(
                    theAssembly.Replace("file:///", "")).Replace(
                    Path.GetFileName(theAssembly), "");
                string theFile = thePath + PersistedLegacyFileName;
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
                exceptionArg = ex;
                return false;
            } //*/
        }

        static public bool RenameInvalidAccountsFile(out Exception exceptionArg)
        {
            exceptionArg = null;
            try
            {
                string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance)).CodeBase;
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
                exceptionArg = ex;
                return false;
            } //*/
        }

        static public bool CheckForEncryptedAccounts(string filename, out bool fileExists, out bool isValidXml, out Exception exceptionArg)
        {
            exceptionArg = null;
            fileExists = false;
            isValidXml = false;
            try
            {
                string theFile = filename;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance)).CodeBase;
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
                fileExists = true;
                xdoc = XDocument.Parse(File.ReadAllText(theFile));
                if (xdoc != null)
                {
                    isValidXml = true;
                    IEnumerable<XElement> xeAccounts =
                        from el in xdoc.Root.Elements("EncryptedTOTPAccount")
                        select el;
                    if (xeAccounts.Count() > 0)
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

        static public bool CheckForAccounts(string filename, out bool fileExists, out bool isValidXml, out Exception exceptionArg)
        {
            exceptionArg = null;
            fileExists = false;
            isValidXml = false;
            try
            {
                string theFile = filename;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance)).CodeBase;
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
                fileExists = true;
                xdoc = XDocument.Parse(File.ReadAllText(theFile));
                if (xdoc != null)
                {
                    isValidXml = true;
                    IEnumerable<XElement> xeAccounts =
                        from el in xdoc.Root.Elements("TOTPAccount")
                        select el;
                    if (xeAccounts.Count() > 0)
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


        static public bool PutEncryptedAccounts(string fileName, Dictionary<string, TOTPAccount> theAccounts, AccountPassPhrase appStore, bool overwrite, out Exception exceptionArg)
        {
            //if fileName is null/empty/whitespace, then save to the program directory
            exceptionArg = null;
            try
            {
                string theFile = fileName;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance)).CodeBase;
                    string thePath = Path.GetFullPath(
                        theAssembly.Replace("file:///", "")).Replace(
                        Path.GetFileName(theAssembly), "");
                    theFile = thePath + PersistedFileName;
                }
                if (System.IO.File.Exists(fileName) && !overwrite)
                {
                    exceptionArg = new InvalidOperationException("Cannot overwrite an existing file if the 'overwrite' parameter is set to false.");
                    return false;
                }
                XElement xeAccounts = new XElement("Accounts");
                foreach (TOTPAccount acc in theAccounts.Values)
                {
                    // actual encryption is handled in the AccountPassPhrase class now
                    xeAccounts.Add(appStore.EncryptXElement(acc.ToXElement(true), acc.SaltedNameHash));
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

        static public bool PutAccounts(string fileName, Dictionary<string, TOTPAccount> theAccounts, bool overwrite, out Exception exceptionArg)
        {
            //if fileName is null/empty/whitespace, then save to the program directory
            exceptionArg = null;
            try
            {
                string theFile = fileName;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance)).CodeBase;
                    string thePath = Path.GetFullPath(
                        theAssembly.Replace("file:///", "")).Replace(
                        Path.GetFileName(theAssembly), "");
                    theFile = thePath + PersistedFileName;
                }
                if (System.IO.File.Exists(fileName) && !overwrite)
                {
                    exceptionArg = new InvalidOperationException("Cannot overwrite an existing file if the 'overwrite' parameter is set to false.");
                    return false;
                }
                XElement xeAccounts = new XElement("Accounts");
                foreach (TOTPAccount acc in theAccounts.Values)
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

        static public Dictionary<string, TOTPAccount> GetEncryptedAccounts(string filename, AccountPassPhrase appStore, out bool foundData, out bool wasAbleToDecryptData, out Exception exceptionArg)
        {
            exceptionArg = null;
            Dictionary<string, TOTPAccount> results = new Dictionary<string,TOTPAccount>();
            foundData = false;
            wasAbleToDecryptData = false;
            try
            {
                string theFile = filename;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance)).CodeBase;
                    string thePath = Path.GetFullPath(
                        theAssembly.Replace("file:///", "")).Replace(
                        Path.GetFileName(theAssembly), "");
                     theFile = thePath + PersistedFileName;
                }
                XDocument xdoc = null;
                if (!File.Exists(theFile))
                {
                    exceptionArg = new InvalidOperationException("File specified does not exist.");
                    return null;
                }
                else
                    xdoc = XDocument.Parse(File.ReadAllText(theFile));
                if (xdoc != null)
                {
                    foundData = true;
                    IEnumerable<XElement> xeAccounts =
                        from el in xdoc.Root.Elements("EncryptedTOTPAccount")
                        select el;
                    foreach (XElement xeAccount in xeAccounts)
                    {
                        //actual decryption is handled in the AccountPassPhrase class now
                        XElement thing = appStore.DecryptXElement(xeAccount);
                        TOTPAccount newAcc = TOTPAccount.FromXElement(thing);
                        if (newAcc != null)
                        {
                            wasAbleToDecryptData = true;
                            results.Add(newAcc.Name, newAcc);
                        }
                        // be more fault tolerant, allow a few bad apples
                        //else
                        //    return null;
                    }
                }
                return results;
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
                return null;
            }
            catch (Exception ex)
            {
                exceptionArg = ex;
                return null;
            }//*/
        }

        static public Dictionary<string, TOTPAccount> GetAccounts(string filename, out bool foundData, out Exception exceptionArg)
        {
            exceptionArg = null;
            Dictionary<string, TOTPAccount> results = new Dictionary<string, TOTPAccount>();
            foundData = false;
            try
            {
                string theFile = filename;
                if (string.IsNullOrWhiteSpace(theFile))
                {
                    string theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance)).CodeBase;
                    string thePath = Path.GetFullPath(
                        theAssembly.Replace("file:///", "")).Replace(
                        Path.GetFileName(theAssembly), "");
                    theFile = thePath + PersistedFileName;
                }
                XDocument xdoc = null;
                if (!File.Exists(theFile))
                {
                    exceptionArg = new InvalidOperationException("File specified does not exist.");
                    return null;
                }
                else
                    xdoc = XDocument.Parse(File.ReadAllText(theFile));
                if (xdoc != null)
                {
                    IEnumerable<XElement> xeAccounts =
                        from el in xdoc.Root.Elements("TOTPAccount")
                        select el;
                    foreach (XElement xeAccount in xeAccounts)
                    {
                        TOTPAccount newAcc = TOTPAccount.FromXElement(xeAccount);
                        if (newAcc != null)
                        {
                            foundData = true;
                            results.Add(newAcc.Name, newAcc);
                        }
                        // be more fault tolerant, allow a few bad apples
                        //else
                        //    return null;
                    }
                }
                return results;
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
                return null;
            }
            catch (Exception ex)
            {
                exceptionArg = ex;
                return null;
            }//*/
        }
    }
}
