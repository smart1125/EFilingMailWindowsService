using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eFilingWindowsServiceSetting
{
    public class ConfigPath
    {
        public static string SettingDirPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "Setting");

        #region SettingPath
        /// <summary>
        /// 
        /// </summary>
        private static string _SettingPath;
        /// <summary>
        /// 
        /// </summary>
        public static string SettingPath
        {
            get
            {
                if (!Directory.Exists(ConfigPath.SettingDirPath)) Directory.CreateDirectory(ConfigPath.SettingDirPath);

                ConfigPath._SettingPath = Path.Combine(ConfigPath.SettingDirPath, "Setting.xml");

                if (!File.Exists(ConfigPath._SettingPath))
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    xmlDoc.LoadXml(eFilingWindowsServiceSetting.Properties.Resources.Setting);

                    xmlDoc.Save(ConfigPath._SettingPath);
                }
                return ConfigPath._SettingPath;
            }
            set { ConfigPath._SettingPath = value; }
        }
        #endregion
    }
}
