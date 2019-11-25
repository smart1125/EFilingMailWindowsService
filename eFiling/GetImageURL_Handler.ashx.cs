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
    /// GetImageURL_Handler 的摘要描述
    /// </summary>
    public class GetImageURL_Handler : IHttpHandler
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
            this.MyUtility.InitLog("GetImageURL");

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            string json = string.Empty, data = string.Empty, message = string.Empty;

            SysCode code = SysCode.B000;

            XmlDocument xmlDoc = null;

            string xPath = string.Empty, strSql = string.Empty;

            List<IDataParameter> para = null;

            DataTable dt = null;

            string USERID = string.Empty;

            string year_and_month = DateTime.Now.ToString("yyyyMM");

            string session_key = string.Empty, session_info = string.Empty;

            IMAGE_URL_CLASS.IMAGE_URL_ITEM[] image_item_list = null;
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

                    this.WaitingCase(context, year_and_month, CHANNEL_CODE, TXN_TYPE, TXN_ID, REPORT_SERIAL_NO, GUID);

                    this.MyUtility.DBLog(context, SysCode.B001, "GetImageURL", string.Empty, USERID, session_info = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", year_and_month, CHANNEL_CODE, TXN_TYPE, TXN_ID, REPORT_SERIAL_NO, GUID));

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

                    this.WaitingCase(context, year_and_month, session_key);

                    this.MyUtility.DBLog(context, SysCode.B001, "GetImageURL", string.Empty, USERID, session_info = string.Format("{0}_{1}", year_and_month, session_key));

                    strSql = this.MyUtility.Select.FILE_TABLE(year_and_month, session_key, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    dt = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, para);
                }
                if (dt.Rows.Count.Equals(0)) throw new Utility.ProcessException(string.Format("查無資料"), ref code, SysCode.E007);

                this.MyUtility.DBLog(context, SysCode.A001, "GetImageURL", session_key = dt.Rows[0]["SESSION_KEY"].ToString(), USERID, string.Empty);

                this.MyUtility.WriteLog(context, string.Format("SESSION_KEY:{0}", session_key));

                DataRow[] dr_image = dt.Select(string.Format("FILE_TYPE=11"));

                this.MyUtility.WriteLog(context, string.Format("Image.Count:{0}", dr_image.Length));

                if (dr_image.Length.Equals(0)) throw new Utility.ProcessException(string.Format("查無資料"), ref code, SysCode.E007);

                image_item_list = new IMAGE_URL_CLASS.IMAGE_URL_ITEM[dr_image.Length];

                for (int i = 0; i < dr_image.Length; i++)
                {
                    session_key = dr_image[i]["SESSION_KEY"].ToString();

                    string file_id = dr_image[i]["FILE_ID"].ToString();

                    int file_seq = Convert.ToInt16(dr_image[i]["FILE_SEQ"].ToString());

                    int file_type = Convert.ToInt16(dr_image[i]["FILE_TYPE"].ToString());

                    string file_root = dr_image[i]["FILE_ROOT"].ToString();

                    string file_path = dr_image[i]["FILE_PATH"].ToString();

                    string file_full_path = file_root.FilePathCombine(file_path);

                    this.MyUtility.WriteLog(context, string.Format("AbsoluteUri:{0}", context.Request.Url.AbsoluteUri));

                    string image_url = context.Request.Url.AbsoluteUri.Replace("GetImageURL", "");

                    image_url = image_url.EndsWith("/") ? image_url.Remove(image_url.Length - 1, 1) : image_url;

                    image_url = string.Format("{0}/{1}?data={2}", image_url, "Images", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", file_id, file_type, year_and_month, session_key, file_full_path, file_seq.ToString(), USERID).EncryptDES());

                    this.MyUtility.WriteLog(context, string.Format("ImageUrl:{0}", image_url));

                    image_item_list[i] = new IMAGE_URL_CLASS.IMAGE_URL_ITEM()
                    {
                        FILE_ID = file_id, URL = image_url.EncryptBase64()
                    };
                }
                this.MyUtility.DBLog(context, SysCode.B000, "GetImageURL", session_key, USERID, session_info);
            }
            catch (System.Exception ex)
            {
                message = ex.Message;

                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.B000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "GetImageURL", session_key, USERID, ex.Message);

                this.MyUtility.SendEMail(context, "GetImageURL", session_key, code);
            }
            finally
            {
                json = JsonConvert.SerializeObject(new IMAGE_URL_RESPOSE()
                {
                    CHANGINGTEC = new IMAGE_URL_SYSTEM_CLASS()
                    {
                        SYSTEM = new IMAGE_URL_SYSTEM_INFO_CLASS()
                        {
                            //CODE = code.Equals(SysCode.B000) ? SysCode.A000.ToString() : code.Equals(SysCode.E007) ? SysCode.A003.ToString() : code.ToString(),
                            CODE = code.Equals(SysCode.B000) ? SysCode.A000.ToString() :  code.ToString(),
                            MESSAGE = message.EncryptBase64(),
                            CASE_INFO = new IMAGE_URL_CLASS() { JPG_ITEM = image_item_list }
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

        #region WaitingCase()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="YM"></param>
        /// <param name="CHANNEL_CODE"></param>
        /// <param name="TXN_TYPE"></param>
        /// <param name="TXN_ID"></param>
        /// <param name="REPORT_SERIAL_NO"></param>
        /// <param name="GUID"></param>
        private void WaitingCase(HttpContext Context, string YM, string CHANNEL_CODE, string TXN_TYPE, string TXN_ID, string REPORT_SERIAL_NO, string GUID)
        {
            SysCode code = SysCode.B000;

            List<IDataParameter> para = null;

            string year_and_month = YM, year_and_month_previous = year_and_month.GetPreviousYM();

            int count = 0;

            while (true)
            {
                if (count.Equals(0)) this.MyUtility.CheckAndCreateTable(Context, year_and_month, ref code);

                string strSql = this.MyUtility.Select.CASE_TABLE(year_and_month, CHANNEL_CODE, TXN_TYPE, TXN_ID, REPORT_SERIAL_NO, GUID, ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, Context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, Context, para.ToLog());

                #endregion

                string case_state = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteByColumnName(strSql, para, "CASE_STATE");

                this.MyUtility.WriteLog(Context, string.Format("CASE_STATE:{0},COUNT:{1}", case_state, count));

                if (String.IsNullOrEmpty(case_state))
                {
                    if (count.Equals(0)) this.MyUtility.CheckAndCreateTable(Context, year_and_month_previous, ref code);

                    strSql = this.MyUtility.Select.CASE_TABLE(year_and_month_previous, CHANNEL_CODE, TXN_TYPE, TXN_ID, REPORT_SERIAL_NO, GUID, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, Context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, Context, para.ToLog());

                    #endregion

                    case_state = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteByColumnName(strSql, para, "CASE_STATE");

                    this.MyUtility.WriteLog(Context, string.Format("CASE_STATE:{0},COUNT:{1}", case_state, count));
                }
                if (case_state.Equals("0")) break;

                System.Threading.Thread.Sleep(100);

                count++;
            }
            para = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="YM"></param>
        /// <param name="SESSION_KEY"></param>
        private void WaitingCase(HttpContext Context, string YM, string SESSION_KEY)
        {
            List<IDataParameter> para = null;

            string year_and_month = YM;

            int count = 0;

            DataTable dt = null;

            while (true)
            {
                string strSql = this.MyUtility.Select.CASE_TABLE(year_and_month, SESSION_KEY, ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, Context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, Context, para.ToLog());

                #endregion

                dt = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                this.MyUtility.WriteLog(Context, string.Format("CASE_STATE:{0},COUNT:{1}", dt.Rows[0]["CASE_STATE"].ToString(), count));

                if (dt.Rows[0]["TRANS_STATE"].ToString().Equals("REPT")) break;

                if (dt.Rows[0]["CASE_STATE"].ToString().Equals("0")) break;

                System.Threading.Thread.Sleep(100);

                count++;
            }
            para = null;
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}