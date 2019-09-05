using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using Log;
using System.Globalization;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace EFilingMailWindowsServiceTest
{
    public class Utility
    {
        #region Log
        /// <summary>
        /// 
        /// </summary>
        private ILog _Log = null;
        /// <summary>
        /// 
        /// </summary>
        public ILog Log
        {
            get
            {
                if (this._Log == null)
                {
                    this._Log = new WindowLog("Utility");
                    this._Log.MaxSize = 4;
                    try
                    {
                        DateTime now_date_time = DateTime.Now;
                        string check_clear_path = Path.Combine(this._Log.DirectoryPath, "clear.flag");
                        if (!File.Exists(check_clear_path)) File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));
                        DateTime date_time = DateTime.Parse(File.ReadAllText(check_clear_path).Trim(), CultureInfo.InvariantCulture);
                        bool clear = DateTime.Compare(DateTime.Now, date_time.AddDays(14)) > 0;

                        if (clear)
                        {
                            this._Log.Delete(14);

                            this._Log.DeleteBackup(14);

                            File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));

                            this._Log.WriteLog(Mode.LogMode.INFO, string.Format("ClearLog::{0}", now_date_time.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                    }
                    catch (System.Exception ex) { this._Log.WriteLog(Mode.LogMode.ERROR, string.Format("ClearLog.Exception::{0}", ex.ToString())); }
                }
                return this._Log;
            }
            set { this._Log = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public void WriteLog(string Message)
        {
            this.Log.WriteLog(Mode.LogMode.INFO, Message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="Message"></param>
        public void WriteLog(Mode.LogMode Mode, string Message)
        {
            this.Log.WriteLog(Mode, Message);
        }
        #endregion

        #region SqlCommand
        /// <summary>
        /// Sql Command (Select)
        /// </summary>
        private SqlCommand.Select _Select = null;
        /// <summary>
        /// Sql Command (Select)
        /// </summary>
        public SqlCommand.Select Select
        {
            get
            {
                if (this._Select == null) this._Select = new SqlCommand.Select(); return this._Select;
            }
        }
        /// <summary>
        /// Sql Command (Update)
        /// </summary>
        private SqlCommand.Update _Update = null;
        /// <summary>
        /// Sql Command (Update)
        /// </summary>
        public SqlCommand.Update Update
        {
            get
            {
                if (this._Update == null) this._Update = new SqlCommand.Update(); return this._Update;
            }
        }
        /// <summary>
        /// Sql Command (Delete)
        /// </summary>
        private SqlCommand.Delete _Delete = null;
        /// <summary>
        /// Sql Command (Delete)
        /// </summary>
        public SqlCommand.Delete Delete
        {
            get
            {
                if (this._Delete == null) this._Delete = new SqlCommand.Delete(); return this._Delete;
            }
        }
        /// <summary>
        /// Sql Command (Insert)
        /// </summary>
        private SqlCommand.Insert _Insert = null;
        /// <summary>
        /// Sql Command (Insert)
        /// </summary>
        public SqlCommand.Insert Insert
        {
            get
            {
                if (this._Insert == null) this._Insert = new SqlCommand.Insert(); return this._Insert;
            }
        }
        #endregion

        #region XmlDocSetting
        /// <summary>
        /// 
        /// </summary>
        public XDocument _XDocSetting = null;
        /// <summary>
        /// 
        /// </summary>
        public XDocument XDocSetting
        {
            get
            {
                if (this._XDocSetting == null && File.Exists(ConfigPath.SettingPath))
                {
                    this._XDocSetting = XDocument.Load(ConfigPath.SettingPath);
                }
                return this._XDocSetting;
            }
            set { this._XDocSetting = value; }
        }
        #endregion

        #region DBConn
        /// <summary>
        /// 資料庫物件介面
        /// </summary>
        public DBLib.IDBConn _DBConn = null;
        /// <summary>
        /// 資料庫物件介面
        /// </summary>
        public DBLib.IDBConn DBConn
        {
            get
            {
                if (this._DBConn == null)
                {
                    this._DBConn = new DBLib.DBConn(new DBLibUtility.Mode.FreeMode()
                    {
                        APMode = DBLibUtility.Mode.APMode.Window,
                        DBMode = DBLibUtility.Mode.DBMode.MSSQL,
                        ConnectionString = this.XDocSetting.Root.Element("ConnectionString").Value.DecryptDES()
                    });
                }
                return this._DBConn;
            }
            set { this._DBConn = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CloseConn()
        {
            if (this._DBConn != null)
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region GetHostIPAddress()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetHostIPAddress()
        {
            List<string> lstIPAddress = new List<string>();

            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            
            foreach (IPAddress ipa in IpEntry.AddressList)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork) lstIPAddress.Add(ipa.ToString());
            }
            return lstIPAddress.Count > 0 ? lstIPAddress[0] : System.Environment.MachineName;
        }
        #endregion

        #region DBLog()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CODE"></param>
        /// <param name="SYSAP"></param>
        /// <param name="SESSION_KEY"></param>
        /// <param name="COMMENTS"></param>
        public void DBLog(SysCode CODE, string SYSAP, string SESSION_KEY, string COMMENTS)
        {
            string strSql = string.Empty;

            List<IDataParameter> para = null;

            DBLib.DBConn dbConn = null;
            try
            {
                if (this.XDocSetting == null || String.IsNullOrEmpty(this.XDocSetting.Root.Element("ConnectionString").Value))
                {
                    this.WriteLog(Mode.LogMode.ERROR, string.Format("設定檔有錯")); return;
                }
                dbConn = new DBLib.DBConn(new DBLibUtility.Mode.FreeMode()
                {
                    APMode = DBLibUtility.Mode.APMode.Window,
                    DBMode = DBLibUtility.Mode.DBMode.MSSQL,
                    ConnectionString = this.XDocSetting.Root.Element("ConnectionString").Value.DecryptDES()
                });
                strSql = this.Insert.SYSLOG(CODE, SYSAP, SESSION_KEY, this.GetHostIPAddress(), System.Environment.MachineName, COMMENTS, ref para);

                #region SQL Debug

                this.WriteLog(Mode.LogMode.DEBUG, strSql);

                this.WriteLog(Mode.LogMode.DEBUG, para.ToLog());

                #endregion

                int result = dbConn.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                this.WriteLog(Mode.LogMode.INFO, string.Format("Result:{0}", result));
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                para = null;

                if (dbConn != null) dbConn.Dispose();

                dbConn = null;
            }
        }
        #endregion
    }
}
