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
    /// UploadFile_Handler 的摘要描述
    /// </summary>
    public class UploadFile_Handler : IHttpHandler
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
            this.MyUtility.InitLog("UploadFile");

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            string year_and_month = DateTime.Now.ToString("yyyyMM");

            string json = string.Empty, data = string.Empty, message = string.Empty;

            SysCode code = SysCode.A000;

            XmlDocument xmlDoc = null;

            string xPath = string.Empty, strSql = string.Empty;

            List<IDataParameter> para = null;

            DataTable dt = null;

            Dictionary<string, string> session_key = new Dictionary<string, string>();

            session_key.Add("SESSION_KEY", Guid.NewGuid().GenerateSessionKey(year_and_month));
            session_key.Add("YM", year_and_month);

            Hashtable para_data = null;

            string USERID = string.Empty;

            int result = -1;

            List<string> delete_list = new List<string>();

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

                this.MyUtility.DBLog(context, SysCode.A001, "UploadFile", session_key["SESSION_KEY"], USERID, string.Empty);

                this.MyUtility.CheckAndCreateTable(context, year_and_month, ref code);

                #region WACH_PATH

                strSql = this.MyUtility.Select.SETTING("WACH_PATH", ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                string wach_dir_path = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteByColumnName(strSql, para, "PARAMETER");

                exists = Directory.Exists(wach_dir_path);

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("WACH_PATH:{0},{1}", wach_dir_path, exists));

                if (!exists) throw new Utility.ProcessException(string.Format("目錄不存在:{0}", wach_dir_path), ref code, SysCode.E001);

                #endregion

                string FILE_EXTENSION = xmlNodeProcessInfo.SelectSingleNode(string.Format("FILE_EXTENSION"), ref code).ToLower();
                string PDF_BASE64 = xmlNodeProcessInfo.SelectSingleNode(string.Format("PDF_BASE64"), ref code);
                string INI_BASE64 = xmlNodeProcessInfo.SelectSingleNode(string.Format("INI_BASE64"), ref code, false);
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

                bool case_data_check = dt != null && dt.Rows.Count > 0;

                string case_state = string.Empty, file_path = string.Empty;

                int case_version = 1;

                if (case_data_check)
                {
                    #region 重覆件處理 (FILE_TABLE)

                    case_version = Convert.ToInt32(dt.Rows[0]["VERSION"].ToString().Trim());

                    case_state = dt.Rows[0]["CASE_STATE"].ToString().Trim();

                    if (case_state.Equals("1")) throw new Utility.ProcessException(string.Format("重覆上傳"), ref code, SysCode.E008);

                    session_key["SESSION_KEY"] = dt.Rows[0]["SESSION_KEY"].ToString().Trim();

                    strSql = this.MyUtility.Select.FILE_TABLE(year_and_month, CHANNEL_CODE, TXN_TYPE, TXN_ID, REPORT_TYPE, GUID, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    dt = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("File.Total:{0}", dt.Rows.Count));

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        delete_list.Add(dt.Rows[i]["FILE_ROOT"].ToString().FilePathCombine(dt.Rows[i]["FILE_PATH"].ToString()));
                    }
                    strSql = this.MyUtility.Delete.FILE_TABLE(year_and_month, session_key["SESSION_KEY"].ToString(), case_version.ToString(), ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                    #endregion
                }
                file_path = Path.Combine(wach_dir_path, string.Format("[ANEW]_{0}_{1}_{2}_{3}.{4}", year_and_month, case_version.ToString(), session_key["SESSION_KEY"].ToString(), USERID, FILE_EXTENSION));

                file_path.DeleteSigleFile();

                string ini_file_path = Path.Combine(wach_dir_path, string.Format("[ANEW]_{0}_{1}_{2}_{3}.ini", year_and_month, case_version.ToString(), session_key["SESSION_KEY"].ToString(), USERID));

                ini_file_path.DeleteSigleFile();

                if (!String.IsNullOrEmpty(INI_BASE64)) this.MyUtility.CreateFile(ini_file_path, INI_BASE64, ref code);

                para_data = new Hashtable();
                para_data.Add("SESSION_KEY", session_key["SESSION_KEY"].ToString());
                para_data.Add("CHANNEL_CODE", CHANNEL_CODE);
                para_data.Add("TXN_TYPE", TXN_TYPE);
                para_data.Add("TXN_ID", TXN_ID);
                para_data.Add("REPORT_TYPE", REPORT_TYPE);
                para_data.Add("GUID", GUID);
                para_data.Add("VERSION", case_version.ToString());
                para_data.Add("CREATE_USERID", USERID);
                para_data.Add("MODIFY_USERID", USERID);

                if (File.Exists(ini_file_path))
                {
                    #region ReadINI

                    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("ReadINI.Start"));

                    string line = string.Empty;

                    using (StreamReader file = new StreamReader(ini_file_path, Encoding.UTF8))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            if (line.IndexOf('=') == -1) continue;

                            string[] line_temp = line.Trim().Split('=');

                            this.MyUtility.WriteLog(Mode.LogMode.DEBUG, context, line);

                            if (!para_data.ContainsKey(line_temp[0].Trim().ToUpper())) para_data.Add(line_temp[0].Trim().ToUpper(), line_temp[1].Trim());
                        }
                        file.Close();
                    }
                    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("ReadINI.End"));

                    #endregion
                }

                if (case_data_check)
                {
                    #region 重覆件處理 (CASE_TABLE)

                    strSql = this.MyUtility.Delete.CASE_TABLE(year_and_month, session_key["SESSION_KEY"].ToString(), case_version.ToString(), ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                    #endregion
                }
                strSql = this.MyUtility.Insert.CASE_TABLE(para_data, year_and_month, ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                if (!result.Equals(1)) throw new Utility.ProcessException(string.Format("案件資料新增失敗"), ref code, SysCode.E002);

                this.MyUtility.CreateFile(file_path, PDF_BASE64, ref code);

                this.MyUtility.DBConnTransac.GeneralSqlCmd.Transaction.Commit();

                this.MyUtility.DBLog(context, SysCode.A002, "UploadFile", session_key["SESSION_KEY"], USERID, string.Empty);
                try
                {
                    foreach (string path in delete_list) path.DeleteSigleFile();
                }
                catch (System.Exception ex) { this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, string.Format("Delete.File.Exception::\r\n{0}", ex.ToString())); }
            }
            catch (System.Exception ex)
            {
                this.MyUtility.Rollback();

                message = ex.Message;

                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.A000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "UploadFile", session_key["SESSION_KEY"], USERID, ex.Message);

                this.MyUtility.SendEMail(context, "UploadFile", session_key["SESSION_KEY"], code);
            }
            finally
            {
                json = JsonConvert.SerializeObject(new UPLOAD_FILE_RESPOSE()
                {
                    CHANGINGTEC = new UPLOAD_FILE_SYSTEM_CLASS()
                    {
                        SYSTEM = new UPLOAD_FILE_SYSTEM_INFO_CLASS()
                            {
                                CODE = code.ToString(),
                                MESSAGE = message.EncryptBase64(),
                                PROCESS_INFO = session_key
                            }
                    }
                });
                dt = null;

                xmlDoc = null;

                session_key = null;

                para = null;

                para_data = null;

                this.MyUtility.CloseConnTransac();

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