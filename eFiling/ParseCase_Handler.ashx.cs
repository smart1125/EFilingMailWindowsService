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
    /// ParseCase_Handler 的摘要描述
    /// </summary>
    public class ParseCase_Handler : IHttpHandler
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
            this.MyUtility.InitLog("ParseCase");

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            string json = string.Empty, data = string.Empty, message = string.Empty;

            SysCode code = SysCode.A000;

            XmlDocument xmlDoc = null;

            string xPath = string.Empty, strSql = string.Empty;

            List<IDataParameter> para = null;

            DataTable dt = null;

            string session_key = string.Empty;

            string year_and_month = DateTime.Now.ToString("yyyyMM");
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

                session_key = xmlNodeProcessInfo.SelectSingleNode(string.Format("SESSION_KEY"), ref code, false);

                year_and_month = xmlNodeProcessInfo.SelectSingleNode(string.Format("YM"), ref code);

                this.MyUtility.CheckAndCreateTable(context, year_and_month, ref code);

                strSql = this.MyUtility.Select.CASE_TABLE(year_and_month, session_key, ref para);

                #region SQL Debug

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                #endregion

                dt = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                if (dt.Rows.Count.Equals(0)) throw new Utility.ProcessException(string.Format("查詢資料不存在"), ref code, SysCode.E007);

                string case_state = dt.Rows[0]["CASE_STATE"].ToString();

                switch (case_state)
                {
                    case "0": code = SysCode.A000; message = string.Empty; break;
                    case "1": code = SysCode.A003; message = "拆檔中"; break;
                    case "9": code = SysCode.E005; message = "拆檔發生錯誤"; break;
                    case "99": code = SysCode.E010; message = "案件已刪除"; break;
                }
                this.MyUtility.DBLog(context, code, "ParseCase", session_key, string.Empty, string.Empty);
            }
            catch (System.Exception ex)
            {
                message = ex.Message;

                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.A000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "ParseCase", session_key, string.Empty, ex.Message);

                this.MyUtility.SendEMail(context, "ParseCase", session_key, code);
            }
            finally
            {
                json = JsonConvert.SerializeObject(new PARSE_CASE_RESPOSE()
                {
                    CHANGINGTEC = new PARSE_CASE_SYSTEM_CLASS()
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