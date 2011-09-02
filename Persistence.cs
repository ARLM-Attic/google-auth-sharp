using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
namespace GoogleAuthClone
{
    public static class Persistence
    {
        const string PersistedFileName = "Accounts.dat";

        static public bool PutAccountBlob(string blob, out Exception exception)
        {
            exception = null;
            try
            {
                string theAssembly = Assembly.GetAssembly(typeof(Persistence)).CodeBase;
                string thePath = Path.GetFullPath(
                    theAssembly.Replace("file:///", "")).Replace(
                    Path.GetFileName(theAssembly), "");
                string theFile = thePath + PersistedFileName;

                StreamWriter myStream = File.CreateText(theFile);
                myStream.Write(blob);
                myStream.Close();
                return true;
            }
            catch (System.ArgumentException aex)
            {
                if (aex.Message == "URI formats are not supported.")
                {
                    aex = null;
                    exception = new System.IO.FileLoadException("Network folders (or redirected folders that point to the network) are not currently supported.  If a network location must be used, map it to a drive letter.");
                }
                else
                    exception = aex;
                return false;
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        static public string GetAccountBlob(out Exception exception)
        {
            exception = null;
            try
            {
                string theAssembly = Assembly.GetAssembly(typeof(Persistence)).CodeBase;
                string thePath = Path.GetFullPath(
                    theAssembly.Replace("file:///","")).Replace(
                    Path.GetFileName(theAssembly),"");
                string theFile = thePath + PersistedFileName;
                
                if (!File.Exists(theFile))
                    return null;
                else
                    return File.ReadAllText(theFile);
            }
            catch (System.ArgumentException aex)
            {
                if (aex.Message == "URI formats are not supported.")
                {
                    aex = null;
                    exception = new System.IO.FileLoadException("Network folders (or redirected folders that point to the network) are not currently supported.  If a network location must be used, map it to a drive letter.");
                }
                else
                    exception = aex;
                return null;
            }
            catch (Exception ex)
            {
                exception = ex;
                return null;
            }
        }
    }
}
