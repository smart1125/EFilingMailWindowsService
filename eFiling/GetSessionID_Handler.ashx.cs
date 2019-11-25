using Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Collections;

namespace eFiling
{
    /// <summary>
    /// GetSessionID_Handler 的摘要描述
    /// </summary>
    public class GetSessionID_Handler : IHttpHandler
    {
        #region MyUtility
        /// <summary>
        /// 
        /// </summary>
        private Utility _MyUtility = null;
        /// <summary>
        /// 
        /// </summary>
        public Utility MyUtility
        {
            get { if (this._MyUtility == null) this._MyUtility = new Utility(); return this._MyUtility; }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            this.MyUtility.InitLog("GetSessionID");

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            string json = string.Empty, data = string.Empty, message = string.Empty;

            SysCode code = SysCode.A000;

            XmlDocument xmlDoc = null;

            string xPath = string.Empty, strSql = string.Empty;

            List<IDataParameter> para = null;

            string USERID = string.Empty;

            string session_key = string.Empty, session_temp_dir_path = string.Empty;

            string year_and_month = DateTime.Now.ToString("yyyyMM");

            DataTable dt = null;

            bool exists = false;
            try
            {
                #region 取得參數

                data = context.GetRequest("data");

                if (String.IsNullOrEmpty(data)) throw new Utility.ProcessException(string.Format("參數為空值"), ref code, SysCode.E004);

                this.MyUtility.WriteLog(Mode.LogMode.DEBUG, context, string.Format("DATA.JSON:{0}", data));

                #endregion

                xmlDoc = JsonConvert.DeserializeXmlNode(data);

                XmlNode xmlNodeProcessInfo = xmlDoc.SelectSingleNode(xPath = string.Format("./CHANGINGTEC/PROCESS_INFO"));

                if (xmlNodeProcessInfo == null) throw new Utility.ProcessException(xPath, ref code, SysCode.E003);

                USERID = xmlNodeProcessInfo.SelectSingleNode(string.Format("USERID"), ref code, false);

                string CHANNEL_CODE = xmlNodeProcessInfo.SelectSingleNode(string.Format("CHANNEL_CODE"), ref code);
                string TXN_TYPE = xmlNodeProcessInfo.SelectSingleNode(string.Format("TXN_TYPE"), ref code);
                string TXN_ID = xmlNodeProcessInfo.SelectSingleNode(string.Format("TXN_ID"), ref code);
                string REPORT_TYPE = xmlNodeProcessInfo.SelectSingleNode(string.Format("REPORT_TYPE"), ref code);
                string GUID = xmlNodeProcessInfo.SelectSingleNode(string.Format("GUID"), ref code);

                strSql = this.MyUtility.Select.CASE_TABLE(year_and_month, CHANNEL_CODE, TXN_TYPE, TXN_ID, REPORT_TYPE, GUID, ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                dt = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["CASE_STATE"].ToString().Trim().Equals("1")) throw new Utility.ProcessException(string.Format("重覆上傳"), ref code, SysCode.E008);

                #region SESSION_TEMP_PATH

                strSql = this.MyUtility.Select.SETTING("SESSION_TEMP_PATH", ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                string session_temp_root_path = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteByColumnName(strSql, para, "PARAMETER");

                exists = Directory.Exists(session_temp_root_path);

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("SESSION_TEMP_PATH (ROOT):{0},{1}", session_temp_root_path, exists));

                if (!exists) throw new Utility.ProcessException(string.Format("目錄不存在:{0}", session_temp_root_path), ref code, SysCode.E001);

                #endregion

                session_key = Guid.NewGuid().GenerateSessionKey(year_and_month);

                session_temp_dir_path = Path.Combine(session_temp_root_path, session_key);

                if (!Directory.Exists(session_temp_dir_path)) Directory.CreateDirectory(session_temp_dir_path);

                string ini_file_path = Path.Combine(session_temp_dir_path, string.Format("{0}.ini", session_key));

                ini_file_path.DeleteSigleFile();

                string INI_BASE64 = xmlNodeProcessInfo.SelectSingleNode(string.Format("INI_BASE64"), ref code, false);

                if (!String.IsNullOrEmpty(INI_BASE64)) this.MyUtility.CreateFile(ini_file_path, INI_BASE64, ref code);

                string xml_path = Path.Combine(session_temp_dir_path, string.Format("{0}_{1}.xml", session_key, year_and_month));

                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>{0}", xmlNodeProcessInfo.OuterXml));
                xmlDoc.Save(xml_path);

                this.MyUtility.DBLog(context, code, "GetSessionID", session_key, USERID, string.Empty);
            }
            catch (System.Exception ex)
            {
                session_key = string.Empty;

                message = ex.Message;

                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.A000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "GetSessionID", session_key, USERID, ex.Message);

                session_temp_dir_path.DeleteDirectory();

                this.MyUtility.SendEMail(context, "GetSessionID", session_key, code);
            }
            finally
            {
                json = JsonConvert.SerializeObject(new GET_SESSION_ID_RESPOSE()
                {
                    CHANGINGTEC = new GET_SESSION_ID_SYSTEM_CLASS()
                    {
                        SYSTEM = new GET_SESSION_ID_SYSTEM_INFO_CLASS()
                        {
                            CODE = code.ToString(),
                            MESSAGE = message.EncryptBase64(),
                            SESSION_KEY = session_key
                        }
                    }
                });
                dt = null;

                xmlDoc = null;

                para = null;

                this.MyUtility.CloseConn();

                xmlDoc = null;

                GC.Collect(); GC.WaitForPendingFinalizers();

                context.Response.Write(json);
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}