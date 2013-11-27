using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace GoogleAuthClone
{
    public class ApplicationSettings
    {
        private bool appSettingsChanged = false;
        private string loadedFrom = null;
        private string loadedApplicationName = null;
        private string m_defaultSaveDirectory;
        private string m_defaultLoadDirectory;
        private bool m_alwaysOnTop;
        private System.Drawing.Point m_formLocation;

        // Properties used to access the application settings variables.
        public string DefaultSaveDirectory
        {
            get { return m_defaultSaveDirectory; }
            set
            {
                if (value != m_defaultSaveDirectory)
                {
                    m_defaultSaveDirectory = value;
                    appSettingsChanged = true;
                }
            }
        }

        public string DefaultLoadDirectory
        {
            get { return m_defaultLoadDirectory; }
            set
            {
                if (value != m_defaultLoadDirectory)
                {
                    m_defaultLoadDirectory = value;
                    appSettingsChanged = true;
                }
            }
        }

        public bool AlwaysOnTop
        {
            get { return m_alwaysOnTop; }
            set
            {
                if (value != m_alwaysOnTop)
                {
                    m_alwaysOnTop = value;
                    appSettingsChanged = true;
                }
            }
        }

        public System.Drawing.Point FormLocation
        {
            get { return m_formLocation; }
            set
            {
                if (value != m_formLocation)
                {
                    m_formLocation = value;
                    appSettingsChanged = true;
                }
            }
        }

        // Serializes the class to the config file
        // if any of the settings have changed.
        public bool SaveAppSettings()
        {
            //sanity check
            if (this.FormLocation.X < 0)
                this.FormLocation = new System.Drawing.Point(0, this.FormLocation.Y);
            if (this.FormLocation.Y < 0)
                this.FormLocation = new System.Drawing.Point(this.FormLocation.X, 0);

            Assembly theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance11));
            string theAssemblyPath = theAssembly.CodeBase;
            string thePath = Path.GetFullPath(
                theAssemblyPath.Replace("file:///", "")).Replace(
                Path.GetFileName(theAssemblyPath), "");
            string theApplicationName = theAssembly.ManifestModule.Name;
            if (this.appSettingsChanged)
            {
                StreamWriter myWriter = null;
                XmlSerializer mySerializer = null;
                try
                {
                    // Create an XmlSerializer for the 
                    // ApplicationSettings type.
                    mySerializer = new XmlSerializer(
                      typeof(ApplicationSettings));
                    myWriter =
                      new StreamWriter(thePath
                      + @"\" + theApplicationName + ".settings.xml", false);
                    // Serialize this instance of the ApplicationSettings 
                    // class to the config file.
                    mySerializer.Serialize(myWriter, this);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    // If the FileStream is open, close it.
                    if (myWriter != null)
                    {
                        myWriter.Close();
                    }
                }
                appSettingsChanged = false;
                return true;
            }
            return false;
        }

        // Deserializes the class from the config file.
        public bool LoadAppSettings()
        {
            Assembly theAssembly = Assembly.GetAssembly(typeof(AccountXMLPersistance11)); 
            string theAssemblyPath = theAssembly.CodeBase;
            string thePath = Path.GetFullPath(
                theAssemblyPath.Replace("file:///", "")).Replace(
                Path.GetFileName(theAssemblyPath), "");
            string theApplicationName = theAssembly.ManifestModule.Name;
            XmlSerializer mySerializer = null;
            FileStream myFileStream = null;
            bool fileExists = false;

            try
            {
                // Create an XmlSerializer for the ApplicationSettings type.
                mySerializer = new XmlSerializer(typeof(ApplicationSettings));
                FileInfo fi = new FileInfo(thePath
                   + @"\" + theApplicationName + ".settings.xml");
                // If the config file exists, open it.
                if (fi.Exists)
                {
                    myFileStream = fi.OpenRead();
                    // Create a new instance of the ApplicationSettings by
                    // deserializing the config file.
                    ApplicationSettings myAppSettings =
                      (ApplicationSettings)mySerializer.Deserialize(
                       myFileStream);
                    // Assign the property values to this instance of 
                    // the ApplicationSettings class.
                    this.m_alwaysOnTop = myAppSettings.AlwaysOnTop;
                    this.m_formLocation = myAppSettings.FormLocation;
                    this.m_defaultLoadDirectory = myAppSettings.DefaultLoadDirectory;
                    this.m_defaultSaveDirectory = myAppSettings.DefaultSaveDirectory;
                    fileExists = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // If the FileStream is open, close it.
                if (myFileStream != null)
                {
                    myFileStream.Close();
                }
            }

            if (m_defaultSaveDirectory == null)
            {
                m_defaultSaveDirectory = thePath;
                this.appSettingsChanged = true;
            }

            if (m_defaultLoadDirectory == null)
            {
                m_defaultLoadDirectory = thePath;
                this.appSettingsChanged = true;
            }
            //sanity checks
            if (this.FormLocation.X < 0)
                this.FormLocation = new System.Drawing.Point(0, this.FormLocation.Y);
            if (this.FormLocation.Y < 0)
                this.FormLocation = new System.Drawing.Point(this.FormLocation.X, 0);

            return fileExists;
        }
    }
}
