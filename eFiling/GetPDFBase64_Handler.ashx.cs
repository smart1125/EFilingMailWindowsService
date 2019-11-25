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
    /// GetPDFBase64Handler 的摘要描述
    /// </summary>
    public class GetPDFBase64_Handler : IHttpHandler
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
            this.MyUtility.InitLog("GetPDFBase64");

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            string json = string.Empty, data = string.Empty, message = string.Empty;

            SysCode code = SysCode.B000;

            XmlDocument xmlDoc = null;

            string xPath = string.Empty, strSql = string.Empty;

            List<IDataParameter> para = null;

            DataTable dt = null;

            string USERID = string.Empty;

            string year_and_month = string.Empty;

            string session_key = string.Empty, session_info = string.Empty;

            PDF_BASE64_CLASS.PDF_BASE64_ITEM pdf_info_item = new PDF_BASE64_CLASS.PDF_BASE64_ITEM();
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

                session_key = xmlNodeProcessInfo.SelectSingleNode(string.Format("SESSION_KEY"), ref code, false);

                this.MyUtility.WriteLog(context, string.Format("SESSION_KEY:{0}", session_key));

                if (String.IsNullOrEmpty(session_key))
                {
                    year_and_month = xmlNodeProcessInfo.SelectSingleNode(string.Format("YM"), ref code);

                    this.MyUtility.CheckAndCreateTable(context, year_and_month, ref code);

                    string CHANNEL_CODE = xmlNodeProcessInfo.SelectSingleNode(string.Format("CHANNEL_CODE"), ref code);
                    string TXN_TYPE = xmlNodeProcessInfo.SelectSingleNode(string.Format("TXN_TYPE"), ref code);
                    string TXN_ID = xmlNodeProcessInfo.SelectSingleNode(string.Format("TXN_ID"), ref code);
                    string REPORT_SERIAL_NO = xmlNodeProcessInfo.SelectSingleNode(string.Format("REPORT_SERIAL_NO"), ref code);
                    string GUID = xmlNodeProcessInfo.SelectSingleNode(string.Format("GUID"), ref code);

                    this.MyUtility.DBLog(context, SysCode.B001, "GetPDFBase64", string.Empty, USERID, session_info = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", year_and_month, CHANNEL_CODE, TXN_TYPE, TXN_ID, REPORT_SERIAL_NO, GUID));

                    strSql = this.MyUtility.Select.FILE_TABLE(year_and_month, CHANNEL_CODE, TXN_TYPE, TXN_ID, REPORT_SERIAL_NO, GUID, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    dt = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                    if (dt.Rows.Count.Equals(0))
                    {
                        year_and_month = year_and_month.GetPreviousYM();

                        this.MyUtility.CheckAndCreateTable(context, year_and_month, ref code);

                        strSql = this.MyUtility.Select.FILE_TABLE(year_and_month, CHANNEL_CODE, TXN_TYPE, TXN_ID, REPORT_SERIAL_NO, GUID, ref para);

                        #region SQL Debug

                        this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                        this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                        #endregion

                        dt = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, para);
                    }
                }
                else
                {
                    year_and_month = session_key.Split('-')[0];

                    this.MyUtility.CheckAndCreateTable(context, year_and_month, ref code);

                    this.MyUtility.DBLog(context, SysCode.B001, "GetPDFBase64", string.Empty, USERID, session_info = string.Format("{0}_{1}", year_and_month, session_key));

                    strSql = this.MyUtility.Select.FILE_TABLE(year_and_month, session_key, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    dt = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, para);
                }
                if (dt.Rows.Count.Equals(0)) throw new Utility.ProcessException(string.Format("查無資料"), ref code, SysCode.E007);

                this.MyUtility.DBLog(context, SysCode.A001, "GetPDFBase64", session_key = dt.Rows[0]["SESSION_KEY"].ToString(), USERID, string.Empty);

                this.MyUtility.WriteLog(context, string.Format("SESSION_KEY:{0}", session_key));

                DataRow[] dr_pdf = dt.Select(string.Format("FILE_TYPE=1"));

                string file_id = string.Empty, file_full_path = string.Empty, file_root = string.Empty, file_path = string.Empty;

                bool exists = false;

                this.MyUtility.WriteLog(context, string.Format("PDF.Count:{0}", dr_pdf.Length));

                if (dr_pdf.Length > 0)
                {
                    file_root = dr_pdf[0]["FILE_ROOT"].ToString();
                    file_path = dr_pdf[0]["FILE_PATH"].ToString();
                    file_id = dr_pdf[0]["FILE_ID"].ToString();
                    file_full_path = file_root.FilePathCombine(file_path);

                    exists = File.Exists(file_full_path);

                    this.MyUtility.WriteLog(context, string.Format("{0},{1},{2}", file_id, file_full_path, exists));
                }

                if (!exists)
                {
                    DataRow[] dr_file = dt.Select(string.Format("FILE_TYPE=3"));

                    this.MyUtility.WriteLog(context, string.Format("File.Count:{0}", dr_file.Length));

                    if (dr_file.Length.Equals(0))
                    {
                        file_root = dr_file[0]["FILE_ROOT"].ToString();
                        file_path = dr_file[0]["FILE_PATH"].ToString();
                        file_id = dr_file[0]["FILE_ID"].ToString();
                        file_full_path = file_root.FilePathCombine(file_path);

                        exists = File.Exists(file_full_path);

                        this.MyUtility.WriteLog(context, string.Format("{0},{1},{2}", file_id, file_full_path, exists));
                    }
                }
                if (!File.Exists(file_full_path)) throw new Utility.ProcessException(string.Format("檔案不存在"), ref code, SysCode.E006);

                pdf_info_item.FILE_ID = file_id;
                
                pdf_info_item.BODY = file_full_path.FileSerialize();

                this.MyUtility.DBLog(context, SysCode.B000, "GetPDFBase64", session_key, USERID, session_info);
            }
            catch (System.Exception ex)
            {
                message = ex.Message;

                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.B000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "GetPDFBase64", session_key, USERID, ex.Message);

                this.MyUtility.SendEMail(context, "GetPDFBase64", session_key, code);
            }
            finally
            {
                json = JsonConvert.SerializeObject(new PDF_BASE64_RESPOSE()
                {
                    CHANGINGTEC = new PDF_BASE64_SYSTEM_CLASS()
                    {
                        SYSTEM = new PDF_BASE64_SYSTEM_INFO_CLASS()
                        {
                            //CODE = code.Equals(SysCode.B000) ? SysCode.A000.ToString() : code.Equals(SysCode.E007) ? SysCode.A003.ToString() : code.ToString(),
                            CODE = code.Equals(SysCode.B000) ? SysCode.A000.ToString() : code.ToString(),
                            MESSAGE = message.EncryptBase64(),
                            CASE_INFO = new PDF_BASE64_CLASS() { PDF_ITEM = pdf_info_item }
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