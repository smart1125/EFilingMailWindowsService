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
    /// UpdateState_Handler 的摘要描述
    /// </summary>
    public class UpdateState_Handler : IHttpHandler
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
            this.MyUtility.InitLog("UpdateState");

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            string json = string.Empty, data = string.Empty, message = string.Empty;

            SysCode code = SysCode.A000;

            XmlDocument xmlDoc = null;

            string xPath = string.Empty, strSql = string.Empty;

            List<IDataParameter> para = null;

            DataTable dt = null;

            string USERID = string.Empty;

            string year_and_month = DateTime.Now.ToString("yyyyMM");

            string session_key = string.Empty;

            int result = -1;
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

                year_and_month = xmlNodeProcessInfo.SelectSingleNode(string.Format("YM"), ref code);

                string CHANNEL_CODE = xmlNodeProcessInfo.SelectSingleNode(string.Format("CHANNEL_CODE"), ref code);
                string TXN_TYPE = xmlNodeProcessInfo.SelectSingleNode(string.Format("TXN_TYPE"), ref code);
                string TXN_ID = xmlNodeProcessInfo.SelectSingleNode(string.Format("TXN_ID"), ref code);
                string REPORT_SERIAL_NO = xmlNodeProcessInfo.SelectSingleNode(string.Format("REPORT_SERIAL_NO"), ref code);
                string GUID = xmlNodeProcessInfo.SelectSingleNode(string.Format("GUID"), ref code);

                string TRANS_STATE = xmlNodeProcessInfo.SelectSingleNode(string.Format("TRANS_STATE"), ref code);
                string CHARGE = xmlNodeProcessInfo.SelectSingleNode(string.Format("CHARGE"), ref code);
                string CHARGE_DATE = DateTime.Parse(xmlNodeProcessInfo.SelectSingleNode(string.Format("CHARGE_DATE"), ref code)).ToString("yyyy/MM/dd HH:mm:ss");
                string SUPERVISOR = xmlNodeProcessInfo.SelectSingleNode(string.Format("SUPERVISOR"), ref code);
                string SUPERVISOR_DATE = DateTime.Parse(xmlNodeProcessInfo.SelectSingleNode(string.Format("SUPERVISOR_DATE"), ref code)).ToString("yyyy/MM/dd HH:mm:ss");

                this.MyUtility.CheckAndCreateTable(context, year_and_month, ref code);

                strSql = this.MyUtility.Select.CASE_TABLE(year_and_month, CHANNEL_CODE, TXN_TYPE, TXN_ID, REPORT_SERIAL_NO, GUID, ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                dt = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                if (dt.Rows.Count.Equals(0)) throw new Utility.ProcessException(string.Format("查詢資料不存在"), ref code, SysCode.E007);

                bool is_delete = TRANS_STATE.Trim().ToUpper().Equals("AF") || TRANS_STATE.Trim().ToUpper().Equals("AG");

                session_key = dt.Rows[0]["SESSION_KEY"].ToString().Trim();

                string case_state = dt.Rows[0]["CASE_STATE"].ToString().Trim();

                string case_version = dt.Rows[0]["VERSION"].ToString().Trim();

                strSql = this.MyUtility.Update.CASE_TABLE_TRANSSTATE(year_and_month
                    , TRANS_STATE
                    , CHARGE
                    , CHARGE_DATE
                    , SUPERVISOR
                    , SUPERVISOR_DATE
                    , is_delete ? "99" : case_state
                    , USERID
                    , CHANNEL_CODE
                    , TXN_TYPE
                    , TXN_ID
                    , REPORT_SERIAL_NO
                    , GUID
                    , ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                if (!result.Equals(1)) throw new Utility.ProcessException(string.Format("案件狀態變更失敗"), ref code, SysCode.E002);

                if (is_delete)
                {
                    strSql = this.MyUtility.Update.FILE_TABLE_STOP(year_and_month, session_key, case_version, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    result = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                    if (result.Equals(0)) throw new Utility.ProcessException(string.Format("檔案刪除失敗"), ref code, SysCode.E002);
                }
                this.MyUtility.DBLog(context, code, "UpdateState", session_key, USERID, string.Empty);
            }
            catch (System.Exception ex)
            {
                message = ex.Message;

                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.A000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "UpdateState", session_key, string.Empty, ex.Message);

                this.MyUtility.SendEMail(context, "UpdateState", session_key, code);
            }
            finally
            {
                json = JsonConvert.SerializeObject(new UPLOAD_STATE_RESPOSE()
                {
                    CHANGINGTEC = new UPLOAD_STATE_SYSTEM_CLASS()
                    {
                        SYSTEM = new SYSTEM_INFO_CLASS()
                        {
                            CODE = code.ToString(),
                            MESSAGE = message.EncryptBase64()
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