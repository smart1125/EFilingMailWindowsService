﻿using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace eFiling
{
    public class SqlCommand
    {
        #region Select
        /// <summary>
        /// Select
        /// </summary>
        public class Select
        {
            #region SETTING()
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string SETTING(string FUNCTION_CODE, ref List<IDataParameter> Parameters)
            {
                string strSql = "Select FUNCTION_CODE\n"
                    + "	,PARAMETER\n"
                    + "	,DESCRIPTION\n"
                    + "From SETTING\n"
                    + "Where FUNCTION_CODE = @FUNCTION_CODE";

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "FUNCTION_CODE"), FUNCTION_CODE));

                return strSql;
            }
            #endregion

            #region CHECK_CASETABLE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Year"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CHECK_CASETABLE(string YM, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = "SELECT * FROM sysobjects WHERE NAME = @tablename\n";

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "tablename"), string.Format("CASE_TABLE_{0}", YM)));

                return strSql;
            }
            #endregion

            #region CHECK_FILETABLE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Year"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CHECK_FILETABLE(string YM, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = "SELECT * FROM sysobjects WHERE NAME = @tablename\n";

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "tablename"), string.Format("FILE_TABLE_{0}", YM)));

                return strSql;
            }
            #endregion

            #region FILE_TABLE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="CHANNEL_CODE"></param>
            /// <param name="TXN_TYPE"></param>
            /// <param name="TXN_ID"></param>
            /// <param name="REPORT_SERIAL_NO"></param>
            /// <param name="GUID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string FILE_TABLE(string YM, string CHANNEL_CODE, string TXN_TYPE, string TXN_ID, string REPORT_SERIAL_NO, string GUID, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Select ft.FILE_ID\n"
                    + "	,ft.SESSION_KEY\n"
                    + "	,ft.VERSION\n"
                    + "	,ft.FILE_STATUS\n"
                    + "	,ft.FILE_ROOT\n"
                    + "	,ft.FILE_PATH\n"
                    + "	,ft.FILE_NAME\n"
                    + "	,ft.FILE_SEQ\n"
                    + "	,ft.FILE_SIZE\n"
                    + "	,ft.FILE_CREATE_DATETIME\n"
                    + "	,ft.FILE_TYPE\n"
                    + "From FILE_TABLE_{0} ft\n"
                    + "Inner Join CASE_TABLE_{0} ct On (\n"
                    + "	ct.SESSION_KEY = ft.SESSION_KEY\n"
                    + "	And ct.CASE_STATE = 0\n"
                    + "	And ct.CHANNEL_CODE = @CHANNEL_CODE\n"
                    + "	And ct.TXN_TYPE = @TXN_TYPE\n"
                    + "	And ct.TXN_ID = @TXN_ID\n"
                    + "	And ct.REPORT_SERIAL_NO = @REPORT_SERIAL_NO\n"
                    + "	And ct.GUID = @GUID\n"
                    + "	And UPPER(ct.TRANS_STATE) <> 'REPT'\n"
                    + "	And UPPER(ct.TRANS_STATE) <> 'DELE' \n"
                    + ")\n"
                    + "Where 1 = 1\n"
                    + "And ft.FILE_STATUS = 0\n"
                    + "Order By ft.FILE_TYPE, ft.FILE_SEQ\n", YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CHANNEL_CODE"), CHANNEL_CODE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_TYPE"), TXN_TYPE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_ID"), TXN_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "REPORT_SERIAL_NO"), REPORT_SERIAL_NO));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "GUID"), GUID));

                return strSql;
            }
            #endregion

            #region FILE_TABLE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="SESSION_KEY"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string FILE_TABLE(string YM, string SESSION_KEY, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Select ft.FILE_ID\n"
                    + "	,ft.SESSION_KEY\n"
                    + "	,ft.VERSION\n"
                    + "	,ft.FILE_STATUS\n"
                    + "	,ft.FILE_ROOT\n"
                    + "	,ft.FILE_PATH\n"
                    + "	,ft.FILE_NAME\n"
                    + "	,ft.FILE_SEQ\n"
                    + "	,ft.FILE_SIZE\n"
                    + "	,ft.FILE_CREATE_DATETIME\n"
                    + "	,ft.FILE_TYPE\n"
                    + "	,ct.TRANS_STATE\n"
                    + "From FILE_TABLE_{0} ft\n"
                    + "Inner Join CASE_TABLE_{0} ct On (\n"
                    + "ct.SESSION_KEY = ft.SESSION_KEY\n"
                    + " And ct.SESSION_KEY = @SESSION_KEY\n"
                    + "	And UPPER(ct.TRANS_STATE) <> 'REPT'\n"
                    + "	And UPPER(ct.TRANS_STATE) <> 'DELE' \n"
                    + ")\n"
                    + "Where 1 = 1\n"
                    + "And ft.SESSION_KEY = @SESSION_KEY\n"
                    + "And ft.FILE_STATUS = 0\n"
                    + "Order By ft.FILE_TYPE, ft.FILE_SEQ\n", YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SESSION_KEY"), SESSION_KEY));

                return strSql;
            }
            #endregion

            #region CASE_TABLE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="SESSION_KEY"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE(string YM, string SESSION_KEY, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Select SESSION_KEY\n"
                    + "	,VERSION\n"
                    + "	,TXN_ID\n"
                    + "	,GUID\n"
                    + "	,CHANNEL_CODE\n"
                    + "	,TXN_TYPE\n"
                    + "	,REPORT_TYPE\n"
                    + "	,CASE_STATE\n"
                    + "	,BRANCH_ID\n"
                    + "	,CURRENCY_ID\n"
                    + "	,TXNREF_NO\n"
                    + "	,TRANSACTION_ID\n"
                    + "	,USERID\n"
                    + "	,TXN_DATE\n"
                    + "	,CIF_ID\n"
                    + "	,REPORT_SERIAL_NO\n"
                    + "	,TXN_COUNT\n"
                    + "	,DEBIT_CREDIT\n"
                    + "	,VOUCHER_NO\n"
                    + "	,AUTHORIZE_ID\n"
                    + "	,CONFIRM_ID\n"
                    + "	,TXN_MEMO\n"
                    + "	,AUTHORIZE_ID_DATE\n"
                    + "	,CONFIRM_ID_DATE\n"
                    + "	,INDEX15\n"
                    + "	,INDEX16\n"
                    + "	,INDEX17\n"
                    + "	,TXN_TIME\n"
                    + "	,TXN_AMOUNT\n"
                    + "	,DECLARANT_ID\n"
                    + "	,TRANSACTION_TYPE\n"
                    + "	,TXN_ACCOUNT\n"
                    + "	,SUPERVISOR_ID\n"
                    + "	,TRANSACTION_20\n"
                    + "	,TRANS_STATE\n"
                    + "	,CHARGE\n"
                    + "	,CHARGE_DATE\n"
                    + "	,SUPERVISOR\n"
                    + "	,SUPERVISOR_DATE\n"
                    + "	,MODE\n"
                    + "	,Replace(Convert(NVarchar, CREATE_DATETIME, 120), '-', '/') As CREATE_DATETIME\n"
                    + "	,CREATE_USERID\n"
                    + "	,Replace(Convert(NVarchar, MODIFY_DATETIME, 120), '-', '/') As MODIFY_DATETIME\n"
                    + "	,MODIFY_USERID\n"
                    + "From CASE_TABLE_{0}\n"
                    + "Where SESSION_KEY = @SESSION_KEY\n", YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SESSION_KEY"), SESSION_KEY));

                return strSql;
            }
            #endregion

            #region CASE_TABLE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="CHANNEL_CODE"></param>
            /// <param name="TXN_TYPE"></param>
            /// <param name="TXN_ID"></param>
            /// <param name="REPORT_SERIAL_NO"></param>
            /// <param name="GUID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE(string YM, string CHANNEL_CODE, string TXN_TYPE, string TXN_ID, string REPORT_SERIAL_NO, string GUID, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Select SESSION_KEY\n"
                    + "	,VERSION\n"
                    + "	,TXN_ID\n"
                    + "	,GUID\n"
                    + "	,CHANNEL_CODE\n"
                    + "	,TXN_TYPE\n"
                    + "	,REPORT_TYPE\n"
                    + "	,CASE_STATE\n"
                    + "	,BRANCH_ID\n"
                    + "	,CURRENCY_ID\n"
                    + "	,TXNREF_NO\n"
                    + "	,TRANSACTION_ID\n"
                    + "	,USERID\n"
                    + "	,TXN_DATE\n"
                    + "	,CIF_ID\n"
                    + "	,REPORT_SERIAL_NO\n"
                    + "	,TXN_COUNT\n"
                    + "	,DEBIT_CREDIT\n"
                    + "	,VOUCHER_NO\n"
                    + "	,AUTHORIZE_ID\n"
                    + "	,CONFIRM_ID\n"
                    + "	,TXN_MEMO\n"
                    + "	,AUTHORIZE_ID_DATE\n"
                    + "	,CONFIRM_ID_DATE\n"
                    + "	,INDEX15\n"
                    + "	,INDEX16\n"
                    + "	,INDEX17\n"
                    + "	,TXN_TIME\n"
                    + "	,TXN_AMOUNT\n"
                    + "	,DECLARANT_ID\n"
                    + "	,TRANSACTION_TYPE\n"
                    + "	,TXN_ACCOUNT\n"
                    + "	,SUPERVISOR_ID\n"
                    + "	,TRANSACTION_20\n"
                    + "	,TRANS_STATE\n"
                    + "	,CHARGE\n"
                    + "	,CHARGE_DATE\n"
                    + "	,SUPERVISOR\n"
                    + "	,SUPERVISOR_DATE\n"
                    + "	,MODE\n"
                    + "	,Replace(Convert(NVarchar, CREATE_DATETIME, 120), '-', '/') As CREATE_DATETIME\n"
                    + "	,CREATE_USERID\n"
                    + "	,Replace(Convert(NVarchar, MODIFY_DATETIME, 120), '-', '/') As MODIFY_DATETIME\n"
                    + "	,MODIFY_USERID\n"
                    + "From CASE_TABLE_{0}\n"
                    + "Where CHANNEL_CODE = @CHANNEL_CODE\n"
                    + "And REPORT_SERIAL_NO = @REPORT_SERIAL_NO\n"
                    + "And TXN_ID = @TXN_ID\n"
                    + "And TXN_TYPE = @TXN_TYPE\n"
                    + "And GUID = @GUID\n"
                    + "Order By CREATE_DATETIME Desc\n"
                    , YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CHANNEL_CODE"), CHANNEL_CODE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_TYPE"), TXN_TYPE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_ID"), TXN_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "REPORT_SERIAL_NO"), REPORT_SERIAL_NO));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "GUID"), GUID));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_T24_STATE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="TRANSACTION_ID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_T24_STATE(string YM, string TRANSACTION_ID, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Select SESSION_KEY\n"
                    +",CASE_STATE\n"                    
                    + "From CASE_TABLE_{0}\n"
                    + "Where CASE_STATE='0'"
                    + "And UPPER(TRANS_STATE) <> 'REPT'\n"
                    + "And UPPER(TRANS_STATE) <> 'DELE' \n"
                    + "And TRANSACTION_ID = @TRANSACTION_ID\n"
                    , YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_T24()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="TRANSACTION_ID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_T24(string YM, string TRANSACTION_ID, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Select SESSION_KEY\n"
                    + "	,VERSION\n"
                    + "	,TXN_ID\n"
                    + "	,GUID\n"
                    + "	,CHANNEL_CODE\n"
                    + "	,TXN_TYPE\n"
                    + "	,REPORT_TYPE\n"
                    + "	,CASE_STATE\n"
                    + "	,BRANCH_ID\n"
                    + "	,CURRENCY_ID\n"
                    + "	,TXNREF_NO\n"
                    + "	,TRANSACTION_ID\n"
                    + "	,USERID\n"
                    + "	,TXN_DATE\n"
                    + "	,CIF_ID\n"
                    + "	,REPORT_SERIAL_NO\n"
                    + "	,TXN_COUNT\n"
                    + "	,DEBIT_CREDIT\n"
                    + "	,VOUCHER_NO\n"
                    + "	,AUTHORIZE_ID\n"
                    + "	,CONFIRM_ID\n"
                    + "	,TXN_MEMO\n"
                    + "	,AUTHORIZE_ID_DATE\n"
                    + "	,CONFIRM_ID_DATE\n"
                    + "	,INDEX15\n"
                    + "	,INDEX16\n"
                    + "	,INDEX17\n"
                    + "	,TXN_TIME\n"
                    + "	,TXN_AMOUNT\n"
                    + "	,DECLARANT_ID\n"
                    + "	,TRANSACTION_TYPE\n"
                    + "	,TXN_ACCOUNT\n"
                    + "	,SUPERVISOR_ID\n"
                    + "	,TRANSACTION_20\n"
                    + "	,TRANS_STATE\n"
                    + "	,CHARGE\n"
                    + "	,CHARGE_DATE\n"
                    + "	,SUPERVISOR\n"
                    + "	,SUPERVISOR_DATE\n"
                    + "	,MODE\n"
                    + "	,Replace(Convert(NVarchar, CREATE_DATETIME, 120), '-', '/') As CREATE_DATETIME\n"
                    + "	,CREATE_USERID\n"
                    + "	,Replace(Convert(NVarchar, MODIFY_DATETIME, 120), '-', '/') As MODIFY_DATETIME\n"
                    + "	,MODIFY_USERID\n"
                    + "From CASE_TABLE_{0}\n"
                    // 移除CASE_STATE='0'
                    + "Where UPPER(TRANS_STATE) <> 'REPT'\n"
                    + "And UPPER(TRANS_STATE) <> 'DELE' \n"
                    + "And TRANSACTION_ID = @TRANSACTION_ID\n"
                    , YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_T24_EXISTS()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="TRANSACTION_ID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_T24_EXISTS(string YM, string TRANSACTION_ID, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Select TRANSACTION_ID\n"
                    + "From CASE_TABLE_{0}\n"
                    + "Where 1=1\n"
                    + "And TRANSACTION_ID = @TRANSACTION_ID\n"
                    , YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_UPDATE_TXNO()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="CHANNEL_CODE"></param>
            /// <param name="TXN_TYPE"></param>
            /// <param name="TXN_ID"></param>
            /// <param name="REPORT_SERIAL_NO"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_UPDATE_TXNO(string YM, string CHANNEL_CODE, string TXN_TYPE, string TXN_ID, string REPORT_SERIAL_NO, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Select SESSION_KEY\n"
                    + "	,VERSION\n"
                    + "	,TXN_ID\n"
                    + "	,GUID\n"
                    + "	,CHANNEL_CODE\n"
                    + "	,TXN_TYPE\n"
                    + "	,REPORT_TYPE\n"
                    + "	,CASE_STATE\n"
                    + "	,BRANCH_ID\n"
                    + "	,CURRENCY_ID\n"
                    + "	,TXNREF_NO\n"
                    + "	,TRANSACTION_ID\n"
                    + "	,USERID\n"
                    + "	,TXN_DATE\n"
                    + "	,CIF_ID\n"
                    + "	,REPORT_SERIAL_NO\n"
                    + "	,TXN_COUNT\n"
                    + "	,DEBIT_CREDIT\n"
                    + "	,VOUCHER_NO\n"
                    + "	,AUTHORIZE_ID\n"
                    + "	,CONFIRM_ID\n"
                    + "	,TXN_MEMO\n"
                    + "	,AUTHORIZE_ID_DATE\n"
                    + "	,CONFIRM_ID_DATE\n"
                    + "	,INDEX15\n"
                    + "	,INDEX16\n"
                    + "	,INDEX17\n"
                    + "	,TXN_TIME\n"
                    + "	,TXN_AMOUNT\n"
                    + "	,DECLARANT_ID\n"
                    + "	,TRANSACTION_TYPE\n"
                    + "	,TXN_ACCOUNT\n"
                    + "	,SUPERVISOR_ID\n"
                    + "	,TRANSACTION_20\n"
                    + "	,TRANS_STATE\n"
                    + "	,CHARGE\n"
                    + "	,CHARGE_DATE\n"
                    + "	,SUPERVISOR\n"
                    + "	,SUPERVISOR_DATE\n"
                    + "	,MODE\n"
                    + "	,Replace(Convert(NVarchar, CREATE_DATETIME, 120), '-', '/') As CREATE_DATETIME\n"
                    + "	,CREATE_USERID\n"
                    + "	,Replace(Convert(NVarchar, MODIFY_DATETIME, 120), '-', '/') As MODIFY_DATETIME\n"
                    + "	,MODIFY_USERID\n"
                    + "From CASE_TABLE_{0}\n"
                    + "Where CHANNEL_CODE = @CHANNEL_CODE\n"
                    + "And CASE_STATE = 0\n"
                    + "And TXN_TYPE = @TXN_TYPE\n"
                    + "And TXN_ID = @TXN_ID\n"
                    + "And REPORT_SERIAL_NO = @REPORT_SERIAL_NO\n"
                    , YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CHANNEL_CODE"), CHANNEL_CODE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_TYPE"), TXN_TYPE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_ID"), TXN_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "REPORT_SERIAL_NO"), REPORT_SERIAL_NO));

                return strSql;
            }
            #endregion

            #region FILE_TABLE_NOT_IMAGE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="SESSION_KEY"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string FILE_TABLE_NOT_IMAGE(string YM, string FILE_ID, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Select FILE_ID\n"
                    + "	,SESSION_KEY\n"
                    + "	,VERSION\n"
                    + "	,FILE_STATUS\n"
                    + "	,FILE_ROOT\n"
                    + "	,FILE_PATH\n"
                    + "	,FILE_NAME\n"
                    + "	,FILE_SEQ\n"
                    + "	,FILE_SIZE\n"
                    + "	,FILE_CREATE_DATETIME\n"
                    + "	,FILE_TYPE\n"
                    + "From FILE_TABLE_{0}\n"
                    + "Where SESSION_KEY = (\n"
                    + "	Select ft.SESSION_KEY\n"
                    + "	From FILE_TABLE_{0} ft\n"
                    + "	Where ft.FILE_ID = @FILE_ID\n"
                    + ")\n"
                    + "And FILE_STATUS = 0\n"
                    + "Order By FILE_TYPE\n", YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "FILE_ID"), FILE_ID));

                return strSql;
            }
            #endregion

            #region SMTP()
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string SMTP()
            {
                #region SQL Command

                string strSql = "Select FUNCTION_CODE, PARAMETER\n"
                    + "From SETTING\n"
                    + "Where FUNCTION_CODE Like 'SMTP_%'\n";

                #endregion

                return strSql;
            }
            #endregion

            #region T24_UPDATE_STATE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="TRANSACTION_ID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string T24_UPDATE_STATE(string TRANSACTION_ID, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = "Select *\n"
                    + "From T24_UPDATE_STATE\n"
                    + "Where 1=1\n"
                    + "And TRANSACTION_ID = @TRANSACTION_ID\n";

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));

                return strSql;
            }
            #endregion
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        public class Update
        {
            #region FILE_TABLE_STOP()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="SESSION_KEY"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string FILE_TABLE_STOP(string YM, string SESSION_KEY, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Update FILE_TABLE_{0} Set FILE_STATUS = 99 Where SESSION_KEY=@SESSION_KEY", YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SESSION_KEY"), SESSION_KEY));

                return strSql;
            }
            #endregion

            #region FILE_TABLE_STOP()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="SESSION_KEY"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string FILE_TABLE_STOP(string YM, string SESSION_KEY, string FILE_SEQ, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Update FILE_TABLE_{0} Set FILE_STATUS = 99 Where SESSION_KEY=@SESSION_KEY And FILE_SEQ=@FILE_SEQ", YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SESSION_KEY"), SESSION_KEY));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "FILE_SEQ"), FILE_SEQ));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_TRANSSTATE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="SESSION_KEY"></param>
            /// <param name="VERSION"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_TRANSSTATE(
                string YM,
                string TRANS_STATE,
                string CHARGE,
                string CHARGE_DATE,
                string SUPERVISOR,
                string SUPERVISOR_DATE,
                string CASE_STATE,
                string USERID,
                string CHANNEL_CODE,
                string TXN_TYPE,
                string TXN_ID,
                string REPORT_SERIAL_NO,
                string GUID,
                ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Update CASE_TABLE_{0} Set\n"
                    + "	TRANS_STATE = @TRANS_STATE\n"
                    + "	,CHARGE = @CHARGE\n"
                    + "	,CHARGE_DATE = @CHARGE_DATE\n"
                    + "	,SUPERVISOR = @SUPERVISOR\n"
                    + "	,SUPERVISOR_DATE = @SUPERVISOR_DATE\n"
                    + "	,CASE_STATE = @CASE_STATE \n"
                    + "	,MODIFY_DATETIME = GetDate()\n"
                    + "	,MODIFY_USERID = @USERID\n"
                    + "Where CASE_STATE = 0\n"
                    + "And CHANNEL_CODE = @CHANNEL_CODE\n"
                    + "And TXN_TYPE = @TXN_TYPE\n"
                    + "And TXN_ID = @TXN_ID\n"
                    + "And REPORT_SERIAL_NO = @REPORT_SERIAL_NO\n"
                    + "And GUID = @GUID\n", YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANS_STATE"), TRANS_STATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CHARGE"), CHARGE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CHARGE_DATE"), CHARGE_DATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SUPERVISOR"), SUPERVISOR));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SUPERVISOR_DATE"), SUPERVISOR_DATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CASE_STATE"), CASE_STATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "USERID"), USERID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CHANNEL_CODE"), CHANNEL_CODE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_TYPE"), TXN_TYPE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_ID"), TXN_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "REPORT_SERIAL_NO"), REPORT_SERIAL_NO));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "GUID"), GUID));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_REPORT_SERIAL_NO()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="SESSION_KEY"></param>
            /// <param name="REPORT_SERIAL_NO"></param>
            /// <param name="USERID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_REPORT_SERIAL_NO(string YM, string SESSION_KEY, string REPORT_SERIAL_NO, string USERID, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Update CASE_TABLE_{0} Set\n"
                    + "	REPORT_SERIAL_NO = @REPORT_SERIAL_NO\n"
                    + "	,MODIFY_DATETIME = GetDate()\n"
                    + "	,MODIFY_USERID = @USERID\n"
                    + "Where CASE_STATE = 0\n"
                    + "And SESSION_KEY = @SESSION_KEY\n", YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SESSION_KEY"), SESSION_KEY));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "REPORT_SERIAL_NO"), REPORT_SERIAL_NO));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "USERID"), USERID));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_GUID()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="SESSION_KEY"></param>
            /// <param name="GUID"></param>
            /// <param name="TXN_TIME"></param>
            /// <param name="TRANS_STATE"></param>
            /// <param name="MODIFY_USERID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_GUID(string YM, string SESSION_KEY, string GUID, string TXN_TIME, string TRANS_STATE, string MODIFY_USERID, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Update CASE_TABLE_{0} Set\n"
                    + "	GUID = @GUID\n"
                    + "	,TXN_TIME = @TXN_TIME\n"
                    + "	,TRANS_STATE = @TRANS_STATE\n"
                    + "	,MODIFY_DATETIME = GetDate()\n"
                    + "	,MODIFY_USERID = @MODIFY_USERID\n"
                    + "Where CASE_STATE = 0\n"
                    + "And SESSION_KEY = @SESSION_KEY\n", YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SESSION_KEY"), SESSION_KEY));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "GUID"), GUID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_TIME"), TXN_TIME));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANS_STATE"), TRANS_STATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "USERID"), MODIFY_USERID));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_T24()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <param name="YM"></param>
            /// <param name="TRANSACTION_ID"></param>
            /// <param name="CHARGE"></param>
            /// <param name="SUPERVISOR"></param>
            /// <param name="MODIFY_USERID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_T24(string YM, string TRANSACTION_ID, string CHARGE, string SUPERVISOR, string MODIFY_USERID, ref List<IDataParameter> Parameters)
            {
                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "MODIFY_USERID"), MODIFY_USERID));

                StringBuilder set = new StringBuilder();
                
                if (!String.IsNullOrEmpty(CHARGE))
                {
                    Parameters.Add(new SqlParameter(string.Format("@{0}", "USERID"), CHARGE));

                    set.AppendLine(string.Format("	,USERID = @CHARGE"));
                }
                else if (!String.IsNullOrEmpty(SUPERVISOR))
                {
                    Parameters.Add(new SqlParameter(string.Format("@{0}", "USERID"), SUPERVISOR));

                    set.AppendLine(string.Format("	,USERID = @SUPERVISOR"));
                }

                if (!String.IsNullOrEmpty(CHARGE))
                {
                    Parameters.Add(new SqlParameter(string.Format("@{0}", "CHARGE"), CHARGE));

                    set.AppendLine(string.Format("	,CHARGE = @CHARGE"));
                    set.AppendLine(string.Format("	,CHARGE_DATE = GetDate()"));
                }

                if (!String.IsNullOrEmpty(SUPERVISOR))
                {
                    Parameters.Add(new SqlParameter(string.Format("@{0}", "SUPERVISOR"), SUPERVISOR));

                    set.AppendLine(string.Format("	,SUPERVISOR = @CHARGE"));
                    set.AppendLine(string.Format("	,SUPERVISOR_DATE = GetDate()"));
                }
                
                #region SQL Command

                string strSql = string.Format("Update CASE_TABLE_{0} Set\n"
                    + "	MODIFY_DATETIME = GetDate()\n"
                    + "	,MODIFY_USERID = @MODIFY_USERID\n"
                    + "{1}"
                    + "Where TRANSACTION_ID = @TRANSACTION_ID\n", YM, set.ToString());

                #endregion

                return strSql;
            }
            #endregion

            #region CASE_TABLE_T24_TRANS_STATE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="TRANSACTION_ID"></param>
            /// <param name="TRANS_STATE"></param>
            /// <param name="MODIFY_USERID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_T24_TRANS_STATE(string YM, string TRANSACTION_ID, string TRANS_STATE, string MODIFY_USERID, string AUTHORIZE_ID, string CONFIRM_ID, string TXN_MEMO,  ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Update CASE_TABLE_{0} Set\n"
                    + "	MODIFY_DATETIME = GetDate()\n"
                    + "	,MODIFY_USERID = @MODIFY_USERID\n"
                    + " ,TRANS_STATE = @TRANS_STATE\n"
                    + "{1}\n"
                    + "{2}\n"
                    + "{3}\n"
                    + "Where TRANSACTION_ID = @TRANSACTION_ID\n"
                    + "And (TRANS_STATE = '' Or TRANS_STATE Is Null)\n"
                      , YM
                      , !String.IsNullOrEmpty(AUTHORIZE_ID) ? ",AUTHORIZE_ID = @AUTHORIZE_ID,AUTHORIZE_ID_DATE = @AUTHORIZE_ID_DATE" : string.Empty
                      , !String.IsNullOrEmpty(CONFIRM_ID) ? ",CONFIRM_ID = @CONFIRM_ID,CONFIRM_ID_DATE = @CONFIRM_ID_DATE" : string.Empty
                      , !String.IsNullOrEmpty(TXN_MEMO) ? ",TXN_MEMO = @TXN_MEMO" : string.Empty);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "MODIFY_USERID"), MODIFY_USERID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANS_STATE"), TRANS_STATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "AUTHORIZE_ID"), AUTHORIZE_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "AUTHORIZE_ID_DATE"), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CONFIRM_ID"), CONFIRM_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CONFIRM_ID_DATE"), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_MEMO"), TXN_MEMO));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_T24_TRANS_STATE_BY_LIVE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="TRANSACTION_ID"></param>
            /// <param name="TRANS_STATE"></param>
            /// <param name="MODIFY_USERID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_T24_TRANS_STATE_BY_LIVE(string YM, string TRANSACTION_ID, string TRANS_STATE, string MODIFY_USERID, string AUTHORIZE_ID, string CONFIRM_ID, string TXN_MEMO, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Update CASE_TABLE_{0} Set\n"
                    + "	MODIFY_DATETIME = GetDate()\n"
                    + "	,MODIFY_USERID = @MODIFY_USERID\n"
                    + " ,TRANS_STATE = @TRANS_STATE\n"
                    + "{1}\n"
                    + "{2}\n"
                    + "Where TRANSACTION_ID = @TRANSACTION_ID\n"
                    + "And UPPER(TRANS_STATE) = 'INAU' And (AUTHORIZE_ID is null or Len(AUTHORIZE_ID) = 0)\n"
                      , YM
                      , !String.IsNullOrEmpty(AUTHORIZE_ID) ? ",AUTHORIZE_ID = @AUTHORIZE_ID,AUTHORIZE_ID_DATE = @AUTHORIZE_ID_DATE" : string.Empty
                      , !String.IsNullOrEmpty(TXN_MEMO) ? ",TXN_MEMO = @TXN_MEMO" : string.Empty);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "MODIFY_USERID"), MODIFY_USERID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANS_STATE"), TRANS_STATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "AUTHORIZE_ID"),AUTHORIZE_ID ));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "AUTHORIZE_ID_DATE"), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_MEMO"), TXN_MEMO));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_T24_TRANS_STATE_BY_INAU()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="TRANSACTION_ID"></param>
            /// <param name="TRANS_STATE"></param>
            /// <param name="MODIFY_USERID"></param>
            /// <param name="CONFIRM_ID_DATE"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_T24_TRANS_STATE_BY_INAU(string YM, string TRANSACTION_ID, string TRANS_STATE, string MODIFY_USERID, string AUTHORIZE_ID, string CONFIRM_ID, string TXN_MEMO, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Update CASE_TABLE_{0} Set\n"
                    + "	MODIFY_DATETIME = GetDate()\n"
                    + "	,MODIFY_USERID = @MODIFY_USERID\n"
                    + " ,TRANS_STATE = @TRANS_STATE\n"
                    +"{1}\n"
                    +"{2}\n"
                    + "Where TRANSACTION_ID = @TRANSACTION_ID\n"
                    + "And UPPER(TRANS_STATE) = 'INCF' And ( CONFIRM_ID Is Null or Len(CONFIRM_ID) = 0 )\n"
                      , YM
                      , !String.IsNullOrEmpty(CONFIRM_ID) ? ",CONFIRM_ID = @CONFIRM_ID,CONFIRM_ID_DATE = @CONFIRM_ID_DATE" : string.Empty
                      , !String.IsNullOrEmpty(TXN_MEMO) ? ",TXN_MEMO = @TXN_MEMO" : string.Empty);
                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "MODIFY_USERID"), MODIFY_USERID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANS_STATE"), TRANS_STATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CONFIRM_ID"), CONFIRM_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CONFIRM_ID_DATE"), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_MEMO"), TXN_MEMO));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_T24_TRANS_STATE_BY_REVE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="TRANSACTION_ID"></param>
            /// <param name="TRANS_STATE"></param>
            /// <param name="MODIFY_USERID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_T24_TRANS_STATE_BY_REVE(string YM, string TRANSACTION_ID, string TRANS_STATE, string MODIFY_USERID, string AUTHORIZE_ID, string CONFIRM_ID, string TXN_MEMO, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Update CASE_TABLE_{0} Set\n"
                    + "	MODIFY_DATETIME = GetDate()\n"
                    + "	,MODIFY_USERID = @MODIFY_USERID\n"
                    + " ,TRANS_STATE = @TRANS_STATE\n"
                    + "{1}\n"
                    + "{2}\n"
                    + "{3}\n"
                    + "Where TRANSACTION_ID = @TRANSACTION_ID\n"
                    + "And (TRANS_STATE = '' Or TRANS_STATE Is Null Or UPPER(TRANS_STATE) = 'LIVE' Or UPPER(TRANS_STATE) = 'INAU')\n"
                      , YM
                      , !String.IsNullOrEmpty(AUTHORIZE_ID) ? ",AUTHORIZE_ID = @AUTHORIZE_ID,AUTHORIZE_ID_DATE = @AUTHORIZE_ID_DATE" : string.Empty
                      , !String.IsNullOrEmpty(CONFIRM_ID) ? ",CONFIRM_ID = @CONFIRM_ID,CONFIRM_ID_DATE = @CONFIRM_ID_DATE" : string.Empty
                      , !String.IsNullOrEmpty(TXN_MEMO) ? ",TXN_MEMO = @TXN_MEMO" : string.Empty);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "MODIFY_USERID"), MODIFY_USERID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANS_STATE"), TRANS_STATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "AUTHORIZE_ID"), AUTHORIZE_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "AUTHORIZE_ID_DATE"), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CONFIRM_ID"), CONFIRM_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CONFIRM_ID_DATE"), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_MEMO"), TXN_MEMO));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_T24_TRANS_STATE_BY_DELE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="TRANSACTION_ID"></param>
            /// <param name="TRANS_STATE"></param>
            /// <param name="MODIFY_USERID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_T24_TRANS_STATE_BY_DELE(string YM, string TRANSACTION_ID, string TRANS_STATE, string MODIFY_USERID, string AUTHORIZE_ID, string CONFIRM_ID, string TXN_MEMO, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Update CASE_TABLE_{0} Set\n"
                    + "	MODIFY_DATETIME = GetDate()\n"
                    + "	,MODIFY_USERID = @MODIFY_USERID\n"
                    + " ,TRANS_STATE = @TRANS_STATE\n"
                    + "{1}\n"
                    + "{2}\n"
                    + "{3}\n"
                    + "Where TRANSACTION_ID = @TRANSACTION_ID\n"
                    + "And (TRANS_STATE = '' Or TRANS_STATE Is Null Or UPPER(TRANS_STATE) = 'INAU' Or UPPER(TRANS_STATE) = 'INCF' Or UPPER(TRANS_STATE) = 'LIVE')\n"
                      , YM
                      , !String.IsNullOrEmpty(AUTHORIZE_ID) ? ",AUTHORIZE_ID = @AUTHORIZE_ID,AUTHORIZE_ID_DATE = @AUTHORIZE_ID_DATE" : string.Empty
                      , !String.IsNullOrEmpty(CONFIRM_ID) ? ",CONFIRM_ID = @CONFIRM_ID,CONFIRM_ID_DATE = @CONFIRM_ID_DATE" : string.Empty
                      , !String.IsNullOrEmpty(TXN_MEMO) ? ",TXN_MEMO = @TXN_MEMO" : string.Empty);
                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "MODIFY_USERID"), MODIFY_USERID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANS_STATE"), TRANS_STATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "AUTHORIZE_ID"), AUTHORIZE_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "AUTHORIZE_ID_DATE"), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CONFIRM_ID"), CONFIRM_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CONFIRM_ID_DATE"), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_MEMO"), TXN_MEMO));

                return strSql;
            }
            #endregion

            #region CASE_TABLE_REPT()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="CHANNEL_CODE"></param>
            /// <param name="TXN_TYPE"></param>
            /// <param name="TXN_ID"></param>
            /// <param name="REPORT_SERIAL_NO"></param>
            /// <param name="GUID"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE_REPT(string YM, string CHANNEL_CODE, string TXN_TYPE, string TXN_ID, string REPORT_SERIAL_NO, string GUID, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Update CASE_TABLE_{0} Set\n"
                    + "	TRANS_STATE = 'REPT'\n"
                    + "Where CHANNEL_CODE = @CHANNEL_CODE\n"
                    + "And REPORT_SERIAL_NO = @REPORT_SERIAL_NO\n"
                    + "And TXN_ID = @TXN_ID\n"
                    + "And TXN_TYPE = @TXN_TYPE\n"
                    + "And GUID = @GUID\n"
                    + "And CASE_STATE = 1\n"
                    + "And CREATE_DATETIME = (\n"
                    + "	Select Max(ct.CREATE_DATETIME)\n"
                    + "	From CASE_TABLE_{0} ct\n"
                    + "	Where ct.REPORT_SERIAL_NO = @REPORT_SERIAL_NO\n"
                    + "	And ct.TXN_ID = @TXN_ID\n"
                    + "	And ct.TXN_TYPE = @TXN_TYPE\n"
                    + "	And ct.GUID = @GUID\n"
                    + "	And ct.CASE_STATE = 1\n"
                    + ")\n"
                    , YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CHANNEL_CODE"), CHANNEL_CODE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_TYPE"), TXN_TYPE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_ID"), TXN_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "REPORT_SERIAL_NO"), REPORT_SERIAL_NO));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "GUID"), GUID));

                return strSql;
            }
            #endregion

            #region T24_UPDATE_STATE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="TRANSACTION_ID"></param>
            /// <param name="TRANS_STATE"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string T24_UPDATE_STATE(string TRANSACTION_ID, string TRANS_STATE, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = "Update T24_UPDATE_STATE Set\n"
                    + "	TRANS_STATE = @TRANS_STATE\n"
                    + "Where TRANSACTION_ID = @TRANSACTION_ID\n";

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANS_STATE"), TRANS_STATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));

                return strSql;
            }
            #endregion
        }
        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        public class Insert
        {
            #region CASE_TABLE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <param name="YearAndMonth"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE(Hashtable Data, string YearAndMonth, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Insert Into CASE_TABLE_{0} (\n"
                    + "	SESSION_KEY\n"
                    + "	,VERSION\n"
                    + "	,TXN_ID\n"
                    + "	,GUID\n"
                    + "	,CHANNEL_CODE\n"
                    + "	,TXN_TYPE\n"
                    + "	,REPORT_TYPE\n"
                    + "	,CASE_STATE\n"
                    + "	,BRANCH_ID\n"
                    + "	,CURRENCY_ID\n"
                    + "	,TXNREF_NO\n"
                    + "	,TRANSACTION_ID\n"
                    + "	,USERID\n"
                    + "	,TXN_DATE\n"
                    + "	,CIF_ID\n"
                    + "	,REPORT_SERIAL_NO\n"
                    + "	,TXN_COUNT\n"
                    + "	,DEBIT_CREDIT\n"
                    + "	,VOUCHER_NO\n"
                    + "	,AUTHORIZE_ID\n"
                    + "	,CONFIRM_ID\n"
                    + "	,TXN_MEMO\n"
                    + "	,AUTHORIZE_ID_DATE\n"
                    + "	,CONFIRM_ID_DATE\n"
                    + "	,INDEX15\n"
                    + "	,INDEX16\n"
                    + "	,INDEX17\n"
                    + "	,TXN_TIME\n"
                    + "	,TXN_AMOUNT\n"
                    + "	,DECLARANT_ID\n"
                    + "	,TRANSACTION_TYPE\n"
                    + "	,TXN_ACCOUNT\n"
                    + "	,SUPERVISOR_ID\n"
                    + "	,TRANSACTION_20\n"
                    + "	,TRANS_STATE\n"
                    + "	,CHARGE\n"
                    + "	,CHARGE_DATE\n"
                    + "	,SUPERVISOR\n"
                    + "	,SUPERVISOR_DATE\n"
                    + "	,MODE\n"
                    + "	,CREATE_DATETIME\n"
                    + "	,CREATE_USERID\n"
                    + "	,MODIFY_DATETIME\n"
                    + "	,MODIFY_USERID\n"
                    + ")\n"
                    + "Values (\n"
                    + "	@SESSION_KEY\n"
                    + "	,@VERSION\n"
                    + "	,@TXN_ID\n"
                    + "	,@GUID\n"
                    + "	,@CHANNEL_CODE\n"
                    + "	,@TXN_TYPE\n"
                    + "	,@REPORT_TYPE\n"
                    + "	,@CASE_STATE\n"
                    + "	,@BRANCH_ID\n"
                    + "	,@CURRENCY_ID\n"
                    + "	,@TXNREF_NO\n"
                    + "	,@TRANSACTION_ID\n"
                    + "	,@USERID\n"
                    + "	,@TXN_DATE\n"
                    + "	,@CIF_ID\n"
                    + "	,@REPORT_SERIAL_NO\n"
                    + "	,@TXN_COUNT\n"
                    + "	,@DEBIT_CREDIT\n"
                    + "	,@VOUCHER_NO\n"
                    + "	,@AUTHORIZE_ID\n"
                    + "	,@CONFIRM_ID\n"
                    + "	,@TXN_MEMO\n"
                    + "	,@AUTHORIZE_ID_DATE\n"
                    + "	,@CONFIRM_ID_DATE\n"
                    + "	,@INDEX15\n"
                    + "	,@INDEX16\n"
                    + "	,@INDEX17\n"
                    + "	,CAST(@TXN_TIME AS time)\n"
                    + "	,@TXN_AMOUNT\n"
                    + "	,@DECLARANT_ID\n"
                    + "	,@TRANSACTION_TYPE\n"
                    + "	,@TXN_ACCOUNT\n"
                    + "	,@SUPERVISOR_ID\n"
                    + "	,@TRANSACTION_20\n"
                    + "	,@TRANS_STATE\n"
                    + "	,@CHARGE\n"
                    + "	,@CHARGE_DATE\n"
                    + "	,@SUPERVISOR\n"
                    + "	,@SUPERVISOR_DATE\n"
                    + "	,@MODE\n"
                    + "	,GetDate()\n"
                    + "	,@CREATE_USERID\n"
                    + "	,GetDate()\n"
                    + "	,@MODIFY_USERID\n"
                    + ")\n", YearAndMonth);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SESSION_KEY"), Data["SESSION_KEY"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "VERSION"), Data["VERSION"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_ID"), Data["TXN_ID"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "GUID"), Data["GUID"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CHANNEL_CODE"), Data["CHANNEL_CODE"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TXN_TYPE"), Data["TXN_TYPE"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "REPORT_TYPE"), Data["REPORT_TYPE"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CASE_STATE"), "1"));
                Parameters.ParametersAdd(Data, "BRANCH_ID");
                Parameters.ParametersAdd(Data, "CURRENCY_ID");
                Parameters.ParametersAdd(Data, "TXNREF_NO");
                Parameters.ParametersAdd(Data, "TRANSACTION_ID");
                Parameters.ParametersAdd(Data, "USERID");
                Parameters.ParametersAdd(Data, "TXN_DATE", true, false);
                Parameters.ParametersAdd(Data, "CIF_ID");
                Parameters.ParametersAdd(Data, "REPORT_SERIAL_NO");
                Parameters.ParametersAdd(Data, "TXN_COUNT");
                Parameters.ParametersAdd(Data, "DEBIT_CREDIT");
                Parameters.ParametersAdd(Data, "VOUCHER_NO");
                Parameters.ParametersAdd(Data, "AUTHORIZE_ID");
                Parameters.ParametersAdd(Data, "CONFIRM_ID");
                Parameters.ParametersAdd(Data, "TXN_MEMO");
                Parameters.ParametersAdd(Data, "AUTHORIZE_ID_DATE");
                Parameters.ParametersAdd(Data, "CONFIRM_ID_DATE");
                Parameters.ParametersAdd(Data, "INDEX15");
                Parameters.ParametersAdd(Data, "INDEX16");
                Parameters.ParametersAdd(Data, "INDEX17");
                Parameters.ParametersAdd(Data, "TXN_TIME", false, true);
                Parameters.ParametersAdd(Data, "TXN_AMOUNT");
                Parameters.ParametersAdd(Data, "DECLARANT_ID");
                Parameters.ParametersAdd(Data, "TRANSACTION_TYPE");
                Parameters.ParametersAdd(Data, "TXN_ACCOUNT");
                Parameters.ParametersAdd(Data, "SUPERVISOR_ID");
                Parameters.ParametersAdd(Data, "TRANSACTION_20");
                Parameters.ParametersAdd(Data, "TRANS_STATE");
                Parameters.ParametersAdd(Data, "CHARGE");
                Parameters.ParametersAdd(Data, "CHARGE_DATE");
                Parameters.ParametersAdd(Data, "SUPERVISOR");
                Parameters.ParametersAdd(Data, "SUPERVISOR_DATE");
                Parameters.ParametersAdd(Data, "MODE");
                Parameters.ParametersAdd(Data, "CREATE_USERID");
                Parameters.ParametersAdd(Data, "MODIFY_USERID");

                return strSql;
            }
            #endregion

            #region SYSLOG()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="CODE"></param>
            /// <param name="SYSAP"></param>
            /// <param name="SESSION_KEY"></param>
            /// <param name="USERIP"></param>
            /// <param name="USERID"></param>
            /// <param name="COMMENTS"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string SYSLOG(SysCode CODE, string SYSAP, string SESSION_KEY, string USERIP, string USERID, string COMMENTS, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = "Insert Into SYSLOG (\n"
                    + "	CODE\n"
                    + "	,SESSION_KEY\n"
                    + "	,DATETIME\n"
                    + "	,SYSAP\n"
                    + "	,USERIP\n"
                    + "	,USERID\n"
                    + "	,COMMENTS\n"
                    + ")\n"
                    + "Values (\n"
                    + "	@CODE\n"
                    + "	,@SESSION_KEY\n"
                    + "	,GetDate()\n"
                    + "	,@SYSAP\n"
                    + "	,@USERIP\n"
                    + "	,@USERID\n"
                    + "	,@COMMENTS\n"
                    + ")\n";

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "CODE"), CODE.ToString().ToUpper()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SYSAP"), SYSAP));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SESSION_KEY"), SESSION_KEY));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "USERIP"), USERIP));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "USERID"), USERID));
                Parameters.ParametersAdd("COMMENTS", COMMENTS);

                return strSql;
            }
            #endregion

            #region FILE_TABLE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <param name="YearAndMonth"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string FILE_TABLE(Hashtable Data, string YearAndMonth, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Insert Into FILE_TABLE_{0} (\n"
                    + "	FILE_ID\n"
                    + "	,SESSION_KEY\n"
                    + "	,VERSION\n"
                    + "	,FILE_STATUS\n"
                    + "	,FILE_ROOT\n"
                    + "	,FILE_PATH\n"
                    + "	,FILE_NAME\n"
                    + "	,FILE_SEQ\n"
                    + "	,FILE_SIZE\n"
                    + "	,FILE_CREATE_DATETIME\n"
                    + "	,FILE_TYPE\n"
                    + ")\n"
                    + "Values (\n"
                    + "	@FILE_ID\n"
                    + "	,@SESSION_KEY\n"
                    + "	,@VERSION\n"
                    + "	,0\n"
                    + "	,@FILE_ROOT\n"
                    + "	,@FILE_PATH\n"
                    + "	,@FILE_NAME\n"
                    + "	,@FILE_SEQ\n"
                    + "	,@FILE_SIZE\n"
                    + "	,GetDate()\n"
                    + "	,@FILE_TYPE\n"
                    + ")\n", YearAndMonth);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "FILE_ID"), Data["FILE_ID"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SESSION_KEY"), Data["SESSION_KEY"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "VERSION"), Data["VERSION"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "FILE_ROOT"), Data["FILE_ROOT"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "FILE_PATH"), Data["FILE_PATH"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "FILE_NAME"), Data["FILE_NAME"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "FILE_SEQ"), Data["FILE_SEQ"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "FILE_SIZE"), Data["FILE_SIZE"].ToString()));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "FILE_TYPE"), Data["FILE_TYPE"].ToString()));

                return strSql;
            }
            #endregion

            #region T24_UPDATE_STATE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <param name="YearAndMonth"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string T24_UPDATE_STATE(string YM, string TRANSACTION_ID, string TRANS_STATE, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Insert Into T24_UPDATE_STATE (\n"
                    + "TABLE_NAME\n"
                    + ",TRANSACTION_ID\n"
                    + ",TRANS_STATE\n"
                    + ")\n"
                    + "Values (\n"
                    + "	@TABLE_NAME\n"
                    + "	,@TRANSACTION_ID\n"
                    + "	,@TRANS_STATE\n"
                    + ")\n");

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TABLE_NAME"), string.Format("CASE_TABLE_{0}", YM)));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANSACTION_ID"), TRANSACTION_ID));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "TRANS_STATE"), TRANS_STATE));
               
                return strSql;
            }
            #endregion
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete
        /// </summary>
        public class Delete
        {
            #region FILE_TABLE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="SESSION_KEY"></param>
            /// <param name="VERSION"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string FILE_TABLE(string YM, string SESSION_KEY, string VERSION, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Delete FILE_TABLE_{0} Where SESSION_KEY=@SESSION_KEY And VERSION=@VERSION", YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SESSION_KEY"), SESSION_KEY));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "VERSION"), VERSION));

                return strSql;
            }
            #endregion

            #region CASE_TABLE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="YM"></param>
            /// <param name="SESSION_KEY"></param>
            /// <param name="VERSION"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string CASE_TABLE(string YM, string SESSION_KEY, string VERSION, ref List<IDataParameter> Parameters)
            {
                #region SQL Command

                string strSql = string.Format("Delete CASE_TABLE_{0} Where SESSION_KEY=@SESSION_KEY And VERSION=@VERSION", YM);

                #endregion

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "SESSION_KEY"), SESSION_KEY));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "VERSION"), VERSION));

                return strSql;
            }
            #endregion
        }
        #endregion
    }
}