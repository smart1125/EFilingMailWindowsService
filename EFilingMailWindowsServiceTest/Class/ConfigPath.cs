using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFilingMailWindowsServiceTest

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
                return ConfigPath._SettingPath = Path.Combine(ConfigPath.SettingDirPath, "Setting.xml");
            }
            set { ConfigPath._SettingPath = value; }
        }
        #endregion

        #region FtpDirPath
        /// <summary>
        /// 
        /// </summary>
        private static string _FtpDirPath;
        /// <summary>
        /// 
        /// </summary>
        public static string FtpDirPath
        {
            get
            {
                ConfigPath._FtpDirPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "FTP");

                if (!Directory.Exists(ConfigPath._FtpDirPath)) Directory.CreateDirectory(ConfigPath._FtpDirPath);

                return ConfigPath._FtpDirPath;
            }
            set { ConfigPath._FtpDirPath = value; }
        }
        #endregion

        #region TempDirPath
        /// <summary>
        /// 
        /// </summary>
        private static string _TempDirPath;
        /// <summary>
        /// 
        /// </summary>
        public static string TempDirPath
        {
            get
            {
                ConfigPath._TempDirPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "Temps");

                if (!Directory.Exists(ConfigPath._TempDirPath)) Directory.CreateDirectory(ConfigPath._TempDirPath);

                return ConfigPath._TempDirPath;
            }
            set { ConfigPath._TempDirPath = value; }
        }
        #endregion
    }
}
