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
    /// CloseSession_Handler 的摘要描述
    /// </summary>
    public class CloseSession_Handler : IHttpHandler
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
            this.MyUtility.InitLog("CloseSession");

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            string json = string.Empty, data = string.Empty, message = string.Empty;

            SysCode code = SysCode.A000;

            XmlDocument xmlDoc = null;

            string xPath = string.Empty, strSql = string.Empty;

            List<IDataParameter> para = null;

            string session_key = string.Empty, session_temp_dir_path = string.Empty;

            string year_and_month = string.Empty;

            DirectoryInfo dirInfo = null;

            DataTable dt = null;

            Hashtable para_data = null;

            string USERID = string.Empty;

            int result = -1;

            List<string> delete_image_list = new List<string>();

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

                this.MyUtility.CheckAndCreateTable(context, year_and_month, ref code);

                #region SESSION_TEMP_PATH

                strSql = this.MyUtility.Select.SETTING("SESSION_TEMP_PATH", ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                string session_temp_root_path = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteByColumnName(strSql, para, "PARAMETER");

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

                #region FILE_ROOT

                strSql = this.MyUtility.Select.SETTING("FILE_ROOT", ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                string file_root = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteByColumnName(strSql, para, "PARAMETER");

                exists = Directory.Exists(file_root);

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("FILE_ROOT:{0},{1}", file_root, exists));

                if (!exists) throw new Utility.ProcessException(string.Format("目錄不存在:{0}", file_root), ref code, SysCode.E001);

                #endregion

                dirInfo = new DirectoryInfo(session_temp_dir_path);

                FileInfo xml_file = dirInfo.GetFiles("*.xml")[0];

                year_and_month = xml_file.Name.Replace(xml_file.Extension, "").Split('_')[1];

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("YM:{0}", year_and_month));

                xmlDoc = new XmlDocument();
                xmlDoc.Load(xml_file.FullName);

                xml_file = null;

                xmlNodeProcessInfo = xmlDoc.SelectSingleNode(xPath = string.Format("./PROCESS_INFO"));

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

                    session_key = dt.Rows[0]["SESSION_KEY"].ToString().Trim();

                    if (case_state.Equals("1")) throw new Utility.ProcessException(string.Format("重覆上傳"), ref code, SysCode.E008);

                    strSql = this.MyUtility.Select.FILE_TABLE(year_and_month, CHANNEL_CODE, TXN_TYPE, TXN_ID, REPORT_TYPE, GUID, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    dt = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("File.Total:{0}", dt.Rows.Count));

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        delete_image_list.Add(dt.Rows[i]["FILE_ROOT"].ToString().FilePathCombine(dt.Rows[i]["FILE_PATH"].ToString()));
                    }
                    strSql = this.MyUtility.Delete.FILE_TABLE(year_and_month, session_key, case_version.ToString(), ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                    #endregion
                }

                #region INSERT.CASE_TABLE

                para_data = new Hashtable();
                para_data.Add("SESSION_KEY", session_key);
                para_data.Add("CHANNEL_CODE", CHANNEL_CODE);
                para_data.Add("TXN_TYPE", TXN_TYPE);
                para_data.Add("TXN_ID", TXN_ID);
                para_data.Add("REPORT_TYPE", REPORT_TYPE);
                para_data.Add("GUID", GUID);
                para_data.Add("VERSION", case_version.ToString());
                para_data.Add("CREATE_USERID", USERID);
                para_data.Add("MODIFY_USERID", USERID);

                string ini_file_path = Path.Combine(session_temp_dir_path, string.Format("{0}.ini", session_key));

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
                strSql = this.MyUtility.Insert.CASE_TABLE(para_data, year_and_month, ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                if (!result.Equals(1)) throw new Utility.ProcessException(string.Format("案件資料新增失敗"), ref code, SysCode.E002);

                #endregion

                FileInfo[] files = dirInfo.GetFiles();

                string file_dir_path = file_root.CreateSubDirectory();

                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo fi = files[i];

                    if (fi.Extension.ToLower().EndsWith(".xml")) continue;

                    bool is_image_file = fi.Name.ToUpper().StartsWith("[FILE]");

                    bool is_ini_file = fi.Extension.ToLower().EndsWith(".ini");

                    string[] file_infos = fi.Name.Replace(fi.Extension, "").Replace("[FILE]_", "").Split('_');

                    #region IMAGE

                    string file_id = Guid.NewGuid().ToString();

                    string image_full_path = Path.Combine(file_dir_path, string.Format("{0}{1}", file_id, fi.Extension));

                    file_path = image_full_path.Replace(file_root, "");

                    if (file_path.StartsWith(@"\") || file_path.StartsWith("/")) file_path = file_path.Remove(0, 1);

                    Hashtable ht = new Hashtable();
                    ht.Add("FILE_ID", file_id);
                    ht.Add("SESSION_KEY", session_key);
                    ht.Add("VERSION", case_version);
                    ht.Add("FILE_ROOT", file_root);
                    ht.Add("FILE_PATH", file_path);
                    ht.Add("FILE_SIZE", fi.Length);

                    if (is_image_file)
                    {
                        ht.Add("FILE_TYPE", Convert.ToInt16(file_infos[0]));
                        ht.Add("FILE_SEQ", Convert.ToInt16(file_infos[1]));
                        ht.Add("FILE_NAME", file_infos[2]);
                    }
                    else
                    {
                        ht.Add("FILE_TYPE", is_ini_file ? 2 : 3);
                        ht.Add("FILE_SEQ", 1);
                        ht.Add("FILE_NAME", "001");
                    }
                    strSql = this.MyUtility.Insert.FILE_TABLE(ht, year_and_month, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                    if (!result.Equals(1)) throw new Exception(string.Format("資料新增失敗::{0}", fi.Name));

                    fi.CopyTo(image_full_path);

                    fi = null;

                    if (!File.Exists(image_full_path)) throw new Exception(string.Format("檔案搬移失敗::{0}", fi.Name));

                    delete_image_list.Add(image_full_path);

                    #endregion
                }
                files = null;

                this.MyUtility.DBConnTransac.GeneralSqlCmd.Transaction.Commit();

                this.MyUtility.DBLog(context, code, "CloseSession", session_key, USERID, message);

                try { session_temp_dir_path.DeleteDirectory(); } catch (System.Exception ex) { this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString()); }
            }
            catch (System.Exception ex)
            {
                this.MyUtility.Rollback();

                message = ex.Message;

                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.A000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "CloseSession", session_key, USERID, ex.Message);

                if (delete_image_list != null) foreach (string e in delete_image_list) e.DeleteSigleFile();

                this.MyUtility.SendEMail(context, "CloseSession", session_key, code);
            }
            finally
            {
                json = JsonConvert.SerializeObject(new CLOSE_SESSION_RESPOSE()
                {
                    CHANGINGTEC = new CLOSE_SESSION_SYSTEM_CLASS()
                    {
                        SYSTEM = new CLOSE_SESSION_SYSTEM_INFO_CLASS()
                        {
                            CODE = code.ToString(),
                            MESSAGE = message.EncryptBase64(),
                            SESSION_KEY = session_key,
                            YM = year_and_month
                        }
                    }
                });
                dt = null;

                dirInfo = null;

                xmlDoc = null;

                para = null;

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