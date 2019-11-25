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
using System.Threading.Tasks;

namespace eFiling
{
    /// <summary>
    /// PDFUploadFile_Handler 的摘要描述
    /// </summary>
    public class PDFUploadFile_Handler : IHttpHandler
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
           

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            string json = string.Empty, data = string.Empty, message = string.Empty;

            SysCode code = SysCode.A000;

            XmlDocument xmlDoc = null;

            string xPath = string.Empty, strSql = string.Empty;

            List<IDataParameter> para = null;

            string USERID = string.Empty;

            string year_and_month = DateTime.Now.ToString("yyyyMM");

            string session_key = string.Empty;

            bool exists = false;

            DataTable dt = null;

            Hashtable para_data = null;

            int result = -1;

            List<string> delete_list = new List<string>();
            try
            {
                #region 取得參數

                data = context.GetRequest("data");

                if (String.IsNullOrEmpty(data)) throw new Utility.ProcessException(string.Format("參數為空值"), ref code, SysCode.E004);

                #endregion

                xmlDoc = JsonConvert.DeserializeXmlNode(data);

                XmlNode xmlNodeProcessInfo = xmlDoc.SelectSingleNode(xPath = string.Format("./CHANGINGTEC/PROCESS_INFO"));

                if (xmlNodeProcessInfo == null) throw new Utility.ProcessException(xPath, ref code, SysCode.E003);

                USERID = xmlNodeProcessInfo.SelectSingleNode(string.Format("USERID"), ref code, false);

                string TRANSACTION_ID = xmlNodeProcessInfo.SelectSingleNode(string.Format("TRANSACTION_ID"), ref code, false);

                this.MyUtility.InitPDFUploadLog(string.Format("PDFUploadFile_{0}", DateTime.Now.Hour.ToString()));

                this.MyUtility.WritePDFUploadLog(Mode.LogMode.DEBUG, context, string.Format("DATA.JSON:{0}", data));

                this.MyUtility.DBLog(context, SysCode.A001, "PDFUploadFile", session_key, USERID, string.Empty);

                string PDF_BASE64 = xmlNodeProcessInfo.SelectSingleNode(string.Format("PDF_BASE64"), ref code);

                string INI_BASE64 = xmlNodeProcessInfo.SelectSingleNode(string.Format("INI_BASE64"), ref code);

                string MODE = xmlNodeProcessInfo.SelectSingleNode(string.Format("MODE"), ref code).ToUpper();

                if (!String.IsNullOrEmpty(MODE)) year_and_month = xmlNodeProcessInfo.SelectSingleNode(string.Format("YM"), ref code);

                #region WACH_PATH

                strSql = this.MyUtility.Select.SETTING("WACH_PATH", ref para);

                #region SQL Debug

                this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                string wach_dir_path = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteByColumnName(strSql, para, "PARAMETER");

                exists = Directory.Exists(wach_dir_path);

                this.MyUtility.WritePDFUploadLog(Mode.LogMode.INFO, context, string.Format("WACH_PATH:{0},{1}", wach_dir_path, exists));

                if (!exists) throw new Utility.ProcessException(string.Format("目錄不存在:{0}", wach_dir_path), ref code, SysCode.E001);

                #endregion

                if (String.IsNullOrEmpty(PDF_BASE64) || String.IsNullOrEmpty(INI_BASE64)) throw new Utility.ProcessException(string.Format("缺少檔案內容"), ref code, SysCode.E006);

                int case_version = 1;

                para_data = new Hashtable();
                para_data.Add("VERSION", case_version.ToString());
                para_data.Add("CREATE_USERID", USERID);
                para_data.Add("MODIFY_USERID", USERID);

                #region ReadINI

                this.MyUtility.WritePDFUploadLog(Mode.LogMode.INFO, context, string.Format("ReadINI.Start"));

                string line = string.Empty;

                //using (MemoryStream memStream = new MemoryStream(Convert.FromBase64String(INI_BASE64.Replace(" ", "+"))))
                //{
                //    using (StreamReader file = new StreamReader(memStream, Encoding.UTF8))
                //    {
                //        string[] lines = file.ReadToEnd().Split(new char[] { '\n' });

                //        Parallel.ForEach(lines, (currentLines) =>
                //        {
                //            if (currentLines.IndexOf('=') == -1) return;

                //            string[] line_temp = currentLines.Trim().Split('=');

                //            this.MyUtility.WritePDFUploadLog(Mode.LogMode.DEBUG, context, line);

                //            if (!para_data.ContainsKey(line_temp[0].Trim().ToUpper()))
                //            {
                //                this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.INFO, context, string.Format("INI:{0}", line));

                //                para_data.Add(line_temp[0].Trim().ToUpper(), line_temp[1].Trim());
                //            }

                //        });
 
                //    }
                //}
                using (StreamReader file = new StreamReader(new MemoryStream(Convert.FromBase64String(INI_BASE64.Replace(" ", "+"))), Encoding.UTF8))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.IndexOf('=') == -1) continue;

                        string[] line_temp = line.Trim().Split('=');

                        this.MyUtility.WritePDFUploadLog(Mode.LogMode.DEBUG, context, line);

                        if (!para_data.ContainsKey(line_temp[0].Trim().ToUpper()))
                        {
                            this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.INFO, context, string.Format("INI:{0}", line));

                            para_data.Add(line_temp[0].Trim().ToUpper(), line_temp[1].Trim());
                        }
                    }
                    file.Close();
                }
                this.MyUtility.WritePDFUploadLog(Mode.LogMode.INFO, context, string.Format("ReadINI.End"));

                #endregion

                if (!para_data.ContainsKey("TXN_DATE")) throw new Utility.ProcessException(string.Format("TXN_DATE is Null"), ref code, SysCode.E004);

                this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.INFO, context, string.Format("TXN_DATE:{0}", para_data.ContainsKey("TXN_DATE")));

                if (para_data.ContainsKey("TXN_DATE")) year_and_month = DateTime.Parse(para_data["TXN_DATE"].ToString().ToDate()).ToString("yyyyMM");

                this.MyUtility.CheckAndCreateTable(context, year_and_month, ref code);

                session_key = Guid.NewGuid().GenerateSessionKey(year_and_month);

                para_data.Add("SESSION_KEY", session_key);
                
                this.MyUtility.WritePDFUploadLog(Mode.LogMode.INFO, context, string.Format("SESSION_KEY:{0}", session_key));

                string case_state = string.Empty, file_path = string.Empty, ini_file_path = string.Empty;

                bool pdf_split = true;

                if (String.IsNullOrEmpty(MODE))
                {
                    #region Anew

                    if (!para_data.ContainsKey("TXN_ID") ||
                        !para_data.ContainsKey("GUID") ||
                        !para_data.ContainsKey("CHANNEL_CODE") ||
                        !para_data.ContainsKey("TXN_TYPE") ||
                        !para_data.ContainsKey("REPORT_SERIAL_NO")) throw new Utility.ProcessException(string.Format("INI內容必要資料不足"), ref code, SysCode.E004);

                    strSql = this.MyUtility.Select.CASE_TABLE(year_and_month, para_data["CHANNEL_CODE"].ToString(), para_data["TXN_TYPE"].ToString(), para_data["TXN_ID"].ToString(), para_data["REPORT_SERIAL_NO"].ToString(), para_data["GUID"].ToString(), ref para);

                    #region SQL Debug

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    dt = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                    bool case_data_check = dt != null && dt.Rows.Count > 0;

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.INFO, context, string.Format("CheckCase:{0}", case_data_check));

                    string case_session_key = session_key;

                    if (case_data_check)
                    {
                        pdf_split = false;

                        if (!para_data.ContainsKey("TRANS_STATE")) para_data.Add("TRANS_STATE", "REPT");
                        else para_data["TRANS_STATE"] = "REPT";
                    }
                    bool check = para_data["TRANS_STATE"].ToString().Equals("LIVE") && para_data.ContainsKey("SUPERVISOR_ID") && !String.IsNullOrEmpty(para_data["SUPERVISOR_ID"].ToString());

                    this.MyUtility.WritePDFUploadLog(Mode.LogMode.INFO, context, string.Format("TRANS_STATE.LIVE:{0},{1}", para_data["TRANS_STATE"].ToString(), check));

                    //if (check)
                    //{
                    //    if (!para_data.ContainsKey("AUTHORIZE_ID")) para_data.Add("AUTHORIZE_ID", para_data["SUPERVISOR_ID"].ToString());
                    //    else para_data["AUTHORIZE_ID"] = para_data["SUPERVISOR_ID"].ToString();

                    //    if (!para_data.ContainsKey("AUTHORIZE_ID_DATE")) para_data.Add("AUTHORIZE_ID_DATE", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    //    else para_data["AUTHORIZE_ID_DATE"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    //    this.MyUtility.WritePDFUploadLog(Mode.LogMode.INFO, context, string.Format("AUTHORIZE_ID:{0}", para_data["AUTHORIZE_ID"].ToString()));
                    //}
                    strSql = this.MyUtility.Insert.CASE_TABLE(para_data, year_and_month, ref para);

                    #region SQL Debug

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                    this.MyUtility.WritePDFUploadLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                    if (!result.Equals(1)) throw new Utility.ProcessException(string.Format("案件資料新增失敗"), ref code, SysCode.E002);

                    file_path = Path.Combine(wach_dir_path, string.Format("[ANEW]_{0}_{1}_{2}_{3}.pdf", year_and_month, case_version.ToString(), case_session_key, USERID));

                    file_path.DeleteSigleFile();

                    ini_file_path = Path.Combine(wach_dir_path, string.Format("[ANEW]_{0}_{1}_{2}_{3}.ini", year_and_month, case_version.ToString(), case_session_key, USERID));

                    ini_file_path.DeleteSigleFile();

                    #endregion
                }
                else if (MODE.Equals("updateTxNo".ToUpper()))
                {
                    #region updateTxNo

                    if (!para_data.ContainsKey("CHANNEL_CODE") ||
                        !para_data.ContainsKey("TXN_TYPE") ||
                        !para_data.ContainsKey("TXN_ID") ||
                        !para_data.ContainsKey("REPORT_SERIAL_NO") ||
                        !para_data.ContainsKey("GUID")) throw new Utility.ProcessException(string.Format("INI內容必要資料不足"), ref code, SysCode.E004);

                    strSql = this.MyUtility.Select.CASE_TABLE_UPDATE_TXNO(year_and_month, para_data["CHANNEL_CODE"].ToString(), para_data["TXN_TYPE"].ToString(), para_data["TXN_ID"].ToString(), para_data["REPORT_SERIAL_NO"].ToString(), ref para);

                    #region SQL Debug

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    dt = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                    bool case_data_check = dt != null && dt.Rows.Count > 0;

                    if (!case_data_check)
                    {
                        year_and_month = year_and_month.GetPreviousYM();

                        strSql = this.MyUtility.Select.CASE_TABLE_UPDATE_TXNO(year_and_month, para_data["CHANNEL_CODE"].ToString(), para_data["TXN_TYPE"].ToString(), para_data["TXN_ID"].ToString(), para_data["REPORT_SERIAL_NO"].ToString(), ref para);

                        #region SQL Debug

                        this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, strSql);

                        this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                        #endregion

                        dt = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                        case_data_check = dt != null && dt.Rows.Count > 0;
                    }
                    if (!case_data_check) throw new Utility.ProcessException(string.Format("查詢資料不存在"), ref code, SysCode.E007);

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.INFO, context, string.Format("Data.Count:{0}", dt.Rows.Count));

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["REPORT_SERIAL_NO"] != DBNull.Value && !String.IsNullOrEmpty(dt.Rows[i]["REPORT_SERIAL_NO"].ToString().Trim())) continue;

                        strSql = this.MyUtility.Update.CASE_TABLE_REPORT_SERIAL_NO(year_and_month, dt.Rows[i]["SESSION_KEY"].ToString().Trim(), para_data["REPORT_SERIAL_NO"].ToString(), USERID, ref para);

                        #region SQL Debug

                        this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, strSql);

                        this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                        #endregion

                        result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                        this.MyUtility.WritePDFUploadLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                        if (!result.Equals(1)) throw new Utility.ProcessException(string.Format("變更REPORT_SERIAL_NO失敗,SESSION_KEY={0}", dt.Rows[i]["SESSION_KEY"].ToString().Trim()), ref code, SysCode.E002);
                    }
                    strSql = this.MyUtility.Insert.CASE_TABLE(para_data, year_and_month, ref para);

                    #region SQL Debug

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                    this.MyUtility.WritePDFUploadLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                    if (!result.Equals(1)) throw new Utility.ProcessException(string.Format("案件資料新增失敗"), ref code, SysCode.E002);

                    file_path = Path.Combine(wach_dir_path, string.Format("[UPDATE_TXNO]_{0}_{1}_{2}_{3}.pdf", year_and_month, case_version.ToString(), session_key, USERID));

                    file_path.DeleteSigleFile();

                    ini_file_path = Path.Combine(wach_dir_path, string.Format("[UPDATE_TXNO]_{0}_{1}_{2}_{3}.ini", year_and_month, case_version.ToString(), session_key, USERID));

                    ini_file_path.DeleteSigleFile();

                    #endregion
                }
                else if (MODE.Equals("rewrite".ToUpper()))
                {
                    #region rewrite

                    if (!para_data.ContainsKey("CHANNEL_CODE") ||
                        !para_data.ContainsKey("TXN_TYPE") ||
                        !para_data.ContainsKey("TXN_ID") ||
                        !para_data.ContainsKey("REPORT_SERIAL_NO") ||
                        !para_data.ContainsKey("GUID")) throw new Utility.ProcessException(string.Format("INI內容必要資料不足"), ref code, SysCode.E004);

                    strSql = this.MyUtility.Select.CASE_TABLE(year_and_month, para_data["CHANNEL_CODE"].ToString(), para_data["TXN_TYPE"].ToString(), para_data["TXN_ID"].ToString(), para_data["REPORT_SERIAL_NO"].ToString(), para_data["GUID"].ToString(), ref para);

                    #region SQL Debug

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    dt = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                    bool case_data_check = dt != null && dt.Rows.Count > 0;

                    if (!case_data_check)
                    {
                        year_and_month = year_and_month.GetPreviousYM();

                        strSql = this.MyUtility.Select.CASE_TABLE(year_and_month, para_data["CHANNEL_CODE"].ToString(), para_data["TXN_TYPE"].ToString(), para_data["TXN_ID"].ToString(), para_data["REPORT_SERIAL_NO"].ToString(), para_data["GUID"].ToString(), ref para);

                        #region SQL Debug

                        this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, strSql);

                        this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                        #endregion

                        dt = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                        case_data_check = dt != null && dt.Rows.Count > 0;
                    }
                    if (!case_data_check) throw new Utility.ProcessException(string.Format("查詢資料不存在"), ref code, SysCode.E007);

                    this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.INFO, context, string.Format("Data.Count:{0}", dt.Rows.Count));

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strSql = this.MyUtility.Update.CASE_TABLE_GUID(year_and_month, dt.Rows[i]["SESSION_KEY"].ToString().Trim(), para_data["GUID"].ToString(), para_data["TXN_TIME"].ToString(), para_data["TRANS_STATE"].ToString(), USERID, ref para);

                        #region SQL Debug

                        this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, strSql);

                        this.MyUtility.WritePDFUploadLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                        #endregion

                        result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                        this.MyUtility.WritePDFUploadLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                        if (!result.Equals(1)) throw new Utility.ProcessException(string.Format("變更GUID失敗,SESSION_KEY={0}", dt.Rows[i]["SESSION_KEY"].ToString().Trim()), ref code, SysCode.E002);
                    }
                    file_path = Path.Combine(wach_dir_path, string.Format("[REWRITE]_{0}_{1}_{2_{3}}.pdf", year_and_month, case_version.ToString(), session_key, USERID));

                    file_path.DeleteSigleFile();

                    ini_file_path = Path.Combine(wach_dir_path, string.Format("[REWRITE]_{0}_{1}_{2}_{3}.ini", year_and_month, case_version.ToString(), session_key, USERID));

                    ini_file_path.DeleteSigleFile();

                    #endregion
                }
                else throw new Utility.ProcessException(string.Format("MODE參數錯誤"), ref code, SysCode.E004);

                if (pdf_split)
                {
                    this.MyUtility.CreateFile(ini_file_path, INI_BASE64, ref code);

                    this.MyUtility.CreateFile(file_path, PDF_BASE64, ref code);
                }
                this.MyUtility.DBConnTransac.GeneralSqlCmd.Transaction.Commit();

                this.MyUtility.DBLog(context, SysCode.A002, "PDFUploadFile", session_key, USERID, string.Empty);
                try
                {
                    foreach (string path in delete_list) path.DeleteSigleFile();
                }
                catch (System.Exception ex) { this.MyUtility.WritePDFUploadLog(Mode.LogMode.ERROR, context, string.Format("Delete.File.Exception::\r\n{0}", ex.ToString())); }
            }
            catch (System.Exception ex)
            {
                this.MyUtility.Rollback();

                message = ex.Message;

                this.MyUtility.WritePDFUploadLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.A000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "PDFUploadFile", session_key, USERID, ex.Message);

                this.MyUtility.SendEMail(context, "PDFUploadFile", session_key, code);
            }
            finally
            {
                json = JsonConvert.SerializeObject(new PDF_UPLOAD_FILE_RESPOSE()
                {
                    CHANGINGTEC = new PDF_UPLOAD_FILE_SYSTEM_CLASS()
                    {
                        SYSTEM = new PDF_UPLOAD_FILE_SYSTEM_INFO_CLASS()
                        {
                            CODE = code.ToString(),
                            MESSAGE = message.EncryptBase64(),
                            PROCESS_INFO = new Dictionary<string, string>() { { "SESSION_KEY", session_key }, { "YM", year_and_month } }
                        }
                    }
                });
                dt = null;

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