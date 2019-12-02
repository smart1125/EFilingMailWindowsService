using Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace eFiling
{
    /// <summary>
    /// T24UpdateTransState_Handler 的摘要描述
    /// </summary>
    public class T24UpdateTransState_Handler : IHttpHandler
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
            this.MyUtility.InitLog("T24UpdateTransState");

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            string json = string.Empty, data = string.Empty, message = string.Empty;

            SysCode code = SysCode.A000;

            XmlDocument xmlDoc = null;

            string xPath = string.Empty, strSql = string.Empty;

            List<IDataParameter> para = null;

            string USERID = string.Empty, SESSION_KEY = string.Empty, TRANS_STATE = string.Empty;

            string year_and_month = DateTime.Now.ToString("yyyyMM");

            DataTable dt = null;

            int result = -1;

            List<string> delete_list = new List<string>();
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

                SESSION_KEY = xmlNodeProcessInfo.SelectSingleNode("SESSION_KEY", ref code);

                TRANS_STATE = xmlNodeProcessInfo.SelectSingleNode("TRANS_STATE", ref code);

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("SESSION_KEY:{0}", SESSION_KEY));

                this.MyUtility.DBLog(context, SysCode.A001, "T24UpdateTransState", SESSION_KEY, USERID, string.Empty);

                bool is_delete = TRANS_STATE.Trim().ToUpper().Equals("DELE");

                this.MyUtility.CheckAndCreateTable(context, year_and_month, ref code);

                strSql = this.MyUtility.Select.CASE_TABLE_T24_Trans(year_and_month, SESSION_KEY, ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                dt = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("dt.Rows.Count:{0}", dt.Rows.Count.ToString()));

                bool case_data_check = dt != null && dt.Rows.Count > 0;

                if (!case_data_check)
                {
                    year_and_month = year_and_month.GetPreviousYM();

                    strSql = this.MyUtility.Select.CASE_TABLE_T24_Trans(year_and_month, SESSION_KEY, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    dt = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                    case_data_check = dt != null && dt.Rows.Count > 0;
                }
                if (!case_data_check) throw new Utility.ProcessException(string.Format("查詢資料不存在"), ref code, SysCode.E007);

                if (is_delete)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["CASE_STATE"].ToString() == "0")
                        {
                            strSql = this.MyUtility.Update.FILE_TABLE_STOP(year_and_month, dt.Rows[i]["SESSION_KEY"].ToString(), ref para);

                            #region SQL Debug

                            this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                            this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                            #endregion

                            result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                            this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));
                        }
                    }
                }
                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("TRANS_STATE:{0}", TRANS_STATE));

                strSql = this.MyUtility.Update.CASE_TABLE_T24_TRANS_STATE_Trans(year_and_month, SESSION_KEY, TRANS_STATE.ToUpper(), USERID, ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));

                this.MyUtility.DBConnTransac.GeneralSqlCmd.Transaction.Commit();

                this.MyUtility.DBLog(context, SysCode.A002, "T24UpdateTransState", SESSION_KEY, USERID, string.Empty);
            }
            catch (System.Exception ex)
            {
                this.MyUtility.Rollback();

                message = ex.Message;

                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.A000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "T24UpdateTransState", SESSION_KEY, USERID, ex.Message);

                this.MyUtility.SendEMail(context, "T24UpdateTransState", SESSION_KEY, code);
            }
            finally
            {
                json = JsonConvert.SerializeObject(new T24UPDATE_STATE_RESPOSE_RESPOSE()
                {
                    CHANGINGTEC = new T24UPDATE_STATE_RESPOSE_SYSTEM_CLASS()
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