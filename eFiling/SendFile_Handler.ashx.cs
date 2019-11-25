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
    /// SendFile_Handler 的摘要描述
    /// </summary>
    public class SendFile_Handler : IHttpHandler
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
            this.MyUtility.InitLog("SendFile");

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            string json = string.Empty, data = string.Empty, message = string.Empty;

            SysCode code = SysCode.A000;

            XmlDocument xmlDoc = null;

            string xPath = string.Empty, strSql = string.Empty;

            List<IDataParameter> para = null;

            string USERID = string.Empty;

            string session_key = string.Empty, session_temp_dir_path = string.Empty;

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

                USERID = xmlNodeProcessInfo.SelectSingleNode(string.Format("SESSION_KEY"), ref code);

                session_key = xmlNodeProcessInfo.SelectSingleNode(string.Format("SESSION_KEY"), ref code);

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

                #region SESSION_TEMP_PATH

                session_temp_dir_path = Path.Combine(session_temp_root_path, session_key);

                exists = Directory.Exists(session_temp_dir_path);

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("SESSION_TEMP_PATH:{0},{1}", session_temp_dir_path, exists));

                if (!exists) throw new Utility.ProcessException(string.Format("目錄不存在:{0}", session_temp_dir_path), ref code, SysCode.E001);

                #endregion

                string FILE_BODY = xmlNodeProcessInfo.SelectSingleNode(string.Format("FILE_BODY"), ref code);
                
                int FILE_SEQ = Convert.ToInt32(xmlNodeProcessInfo.SelectSingleNode(string.Format("FILE_SEQ"), ref code));
                
                string FILE_NAME = xmlNodeProcessInfo.SelectSingleNode(string.Format("FILE_NAME"), ref code, true);
                
                string FILE_EXTENSION = xmlNodeProcessInfo.SelectSingleNode(string.Format("FILE_EXTENSION"), ref code);

                int FILE_TYPE = Convert.ToInt32(xmlNodeProcessInfo.SelectSingleNode(string.Format("FILE_TYPE"), ref code));

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, message = string.Format("FILE_SEQ={0},FILE_NAME={1},FILE_EXTENSION={2},FILE_TYPE={3},FILE_BODY={4}", FILE_SEQ, FILE_NAME, FILE_EXTENSION, FILE_TYPE, FILE_BODY.Length));

                string file_path = Path.Combine(session_temp_dir_path, string.Format("[FILE]_{0}_{1}_{2}_{3}.{4}", FILE_TYPE, FILE_SEQ.ToString(), !String.IsNullOrEmpty(FILE_NAME) ? FILE_NAME : FILE_SEQ.ToString().PadLeft(3, '0'), USERID, FILE_EXTENSION));

                file_path.DeleteSigleFile();

                this.MyUtility.CreateFile(file_path, FILE_BODY, ref code);

                this.MyUtility.DBLog(context, code, "SendFile", session_key, USERID, message);
            }
            catch (System.Exception ex)
            {
                session_key = string.Empty;

                message = ex.Message;

                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.A000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "SendFile", session_key, USERID, ex.Message);

                this.MyUtility.SendEMail(context, "SendFile", session_key, code);
            }
            finally
            {
                json = JsonConvert.SerializeObject(new SEND_FILE_RESPOSE()
                {
                    CHANGINGTEC = new SEND_FILE_SYSTEM_CLASS()
                    {
                        SYSTEM = new SYSTEM_INFO_CLASS()
                        {
                            CODE = code.ToString(),
                            MESSAGE = message.EncryptBase64()
                        }
                    }
                });
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