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

            string USERID = string.Empty, TRANSACTION_ID = string.Empty, TRANS_STATE = string.Empty, CHARGE = string.Empty, SUPERVISOR = string.Empty;
            string AUTHORIZE_ID = string.Empty, CONFIRM_ID = string.Empty, TXN_MEMO = string.Empty;

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

                TRANSACTION_ID = xmlNodeProcessInfo.SelectSingleNode("TRANSACTION_ID", ref code);

                TRANS_STATE = xmlNodeProcessInfo.SelectSingleNode("TRANS_STATE", ref code);

                CHARGE = xmlNodeProcessInfo.SelectSingleNode("CHARGE", ref code, false);

                SUPERVISOR = xmlNodeProcessInfo.SelectSingleNode("SUPERVISOR", ref code, false);

                AUTHORIZE_ID = xmlNodeProcessInfo.SelectSingleNode("AUTHORIZE_ID", ref code, false);

                CONFIRM_ID = xmlNodeProcessInfo.SelectSingleNode("CONFIRM_ID", ref code, false);

                TXN_MEMO = xmlNodeProcessInfo.SelectSingleNode("TXN_MEMO", ref code, false);

                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("TRANS_STATE:{0}", TRANS_STATE));
                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("AUTHORIZE_ID:{0}", AUTHORIZE_ID));
                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("CONFIRM_ID:{0}", CONFIRM_ID));
                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("TXN_MEMO:{0}", TXN_MEMO));

                this.MyUtility.DBLog(context, SysCode.A001, "T24UpdateState", TRANSACTION_ID, USERID, string.Empty);

                bool is_delete = TRANS_STATE.Trim().ToUpper().Equals("DELE");

                //bool is_reve = TRANS_STATE.Trim().ToUpper().Equals("REVE");

                //bool is_inau = TRANS_STATE.Trim().ToUpper().Equals("INAU");

                //bool is_live = TRANS_STATE.Trim().ToUpper().Equals("LIVE");

                this.MyUtility.CheckAndCreateTable(context, year_and_month, ref code);

                strSql = this.MyUtility.Select.CASE_TABLE_T24(year_and_month, TRANSACTION_ID, ref para);

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

                    strSql = this.MyUtility.Select.CASE_TABLE_T24(year_and_month, TRANSACTION_ID, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    dt = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                    case_data_check = dt != null && dt.Rows.Count > 0;
                }
                if (!case_data_check) throw new Utility.ProcessException(string.Format("查詢資料不存在"), ref code, SysCode.E007);

                //bool is_dril = is_delete || is_reve || is_inau || is_live;
                bool is_dril = is_delete;

                if (is_dril)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string session_key = dt.Rows[i]["SESSION_KEY"].ToString();

                        string trans_state = dt.Rows[i]["TRANS_STATE"] != DBNull.Value ? dt.Rows[i]["TRANS_STATE"].ToString().Trim().ToUpper() : string.Empty;

                        this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("SESSION_KEY:{0},TRANS_STATE:{1}", session_key, trans_state));

                        bool check = false;

                        if (is_delete)
                        {
                            check = String.IsNullOrEmpty(trans_state) || trans_state.Equals("INAU") || trans_state.Equals("LIVE") || trans_state.Equals("INCF");
                        }
                        //else if (is_reve)
                        //{
                        //    check = String.IsNullOrEmpty(trans_state) || trans_state.Equals("LIVE");
                        //}
                        //else if (is_inau)
                        //{
                        //    string confirm_id = dt.Rows[i]["CONFIRM_ID"] != DBNull.Value ? dt.Rows[i]["CONFIRM_ID"].ToString().Trim() : string.Empty;

                        //    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("CONFIRM_ID:{0},TRANS_STATE:{1}", confirm_id, trans_state));

                        //    check = (String.IsNullOrEmpty(confirm_id) && trans_state.Equals("INCF")) || trans_state.Equals("INAU");
                        //}
                        //else if (is_live)
                        //{
                        //    string authorize_id = dt.Rows[i]["AUTHORIZE_ID"] != DBNull.Value ? dt.Rows[i]["AUTHORIZE_ID"].ToString().Trim() : string.Empty;

                        //    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("AUTHORIZE_ID:{0},TRANS_STATE:{1}", authorize_id, trans_state));

                        //    check = String.IsNullOrEmpty(authorize_id) && trans_state.Equals("INAU");
                        //}
                        this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Check:{0}", check));

                        if (!check) throw new Utility.ProcessException(string.Format("TRANS_STATE 不符合規範"), ref code, SysCode.E012);
                    }
                }
                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Data.Count:{0}", dt.Rows.Count));

                if (is_delete)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
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
                this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("TRANS_STATE:{0}", TRANS_STATE));

                if (!String.IsNullOrEmpty(TRANS_STATE))
                {
                    //if (is_live) strSql = this.MyUtility.Update.CASE_TABLE_T24_TRANS_STATE_BY_LIVE(year_and_month, TRANSACTION_ID, TRANS_STATE.ToUpper(), USERID, AUTHORIZE_ID, CONFIRM_ID, TXN_MEMO, ref para);
                    //else if (is_inau) strSql = this.MyUtility.Update.CASE_TABLE_T24_TRANS_STATE_BY_INAU(year_and_month, TRANSACTION_ID, TRANS_STATE.ToUpper(), USERID, AUTHORIZE_ID, CONFIRM_ID, TXN_MEMO, ref para);
                    //else if (is_reve) strSql = this.MyUtility.Update.CASE_TABLE_T24_TRANS_STATE_BY_REVE(year_and_month, TRANSACTION_ID, TRANS_STATE.ToUpper(), USERID, AUTHORIZE_ID, CONFIRM_ID, TXN_MEMO, ref para);
                    //else 
                    if (is_delete) strSql = this.MyUtility.Update.CASE_TABLE_T24_TRANS_STATE_BY_DELE_Trans(year_and_month, TRANSACTION_ID, TRANS_STATE.ToUpper(), USERID, AUTHORIZE_ID, CONFIRM_ID, TXN_MEMO, ref para);
                    else strSql = this.MyUtility.Update.CASE_TABLE_T24_TRANS_STATE_Trans(year_and_month, TRANSACTION_ID, TRANS_STATE.ToUpper(), USERID, AUTHORIZE_ID, CONFIRM_ID, TXN_MEMO, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));
                }

                if (!String.IsNullOrEmpty(CHARGE) || !String.IsNullOrEmpty(SUPERVISOR))
                {
                    strSql = this.MyUtility.Update.CASE_TABLE_T24(year_and_month, TRANSACTION_ID, CHARGE, SUPERVISOR, USERID, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    result = this.MyUtility.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                    this.MyUtility.WriteLog(Mode.LogMode.INFO, context, string.Format("Result:{0}", result));
                }
                this.MyUtility.DBConnTransac.GeneralSqlCmd.Transaction.Commit();

                this.MyUtility.DBLog(context, SysCode.A002, "T24UpdateTransState", TRANSACTION_ID, USERID, string.Empty);
            }
            catch (System.Exception ex)
            {
                this.MyUtility.Rollback();

                message = ex.Message;

                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.A000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "T24UpdateTransState", TRANSACTION_ID, USERID, ex.Message);

                this.MyUtility.SendEMail(context, "T24UpdateTransState", TRANSACTION_ID, code);
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