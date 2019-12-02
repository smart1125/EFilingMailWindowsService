using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EFilingMailWindowsService
{
    public class SqlCommand
    {
        #region Select
        /// <summary>
        /// Select
        /// </summary>
        public class Select
        {
            #region BATCH_CIF_TABLE()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="START_TXN_DATE"></param>
            /// <param name="END_TXN_DATE"></param>
            /// <param name="YM"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string BATCH_CIF_TABLE(string START_TXN_DATE, string END_TXN_DATE, string YM, ref List<IDataParameter> Parameters)
            {
                string strSql = "Select B.CIF_ID\n"
                    + "	,B.CREATE_DATETIME\n"
                    + "	,B.COMMENTS\n"
                    + "	,B.SESSION_KEY\n"
                    + "	,B.TXN_DATE\n"
                    + "	,B.TXN_ACCOUNT\n"
                    + "	,B.BRANCH_ID\n"
                    + "	,B.FILE_ID\n"
                    + "	,B.FILE_CREATE_DATETIME\n"
                    + "	,B.FILE_SEQ\n"
                    + "	,B.FILE_ROOT\n"
                    + "	,B.FILE_PATH\n"
                    + "	,B.PDF\n"
                    + "	,B.FILE_TYPE\n"
                    + "From (\n"
                    + "	Select bct.CIF_ID\n"
                    + "		,bct.CREATE_DATETIME\n"
                    + "		,bct.COMMENTS\n"
                    + "		,ct.SESSION_KEY\n"
                    + "		,ct.TXN_DATE\n"
                    + "		,ct.TXN_ACCOUNT\n"
                    + "		,ct.BRANCH_ID\n"
                    + "		,ft.FILE_ID\n"
                    + "		,ft.FILE_CREATE_DATETIME\n"
                    + "		,ft.FILE_SEQ\n"
                    + "		,ft.FILE_ROOT\n"
                    + "		,ft.FILE_PATH\n"
                    + "		,ft.FILE_TYPE\n"
                    + "		,'Y' As PDF\n"
                    + "	From BATCH_CIF_TABLE bct\n"
                    + "	Left Join CASE_TABLE_{0} ct On (ct.CIF_ID = bct.CIF_ID)\n"
                    + "	Left Join FILE_TABLE_{0} ft On (ft.SESSION_KEY = ct.SESSION_KEY)\n"
                    + "	Where ct.TXN_DATE >= @START_TXN_DATE And ct.TXN_DATE <= @END_TXN_DATE\n"
                    + "	And (ct.TRANS_STATE = '' Or Upper(ct.TRANS_STATE) = 'LIVE')\n"
                    + "	And ct.REPORT_TYPE = '水單'\n"
                    + "	Union All\n"
                    + "	Select bct.CIF_ID\n"
                    + "		,bct.CREATE_DATETIME\n"
                    + "		,bct.COMMENTS\n"
                    + "		,ct.SESSION_KEY\n"
                    + "		,ct.TXN_DATE\n"
                    + "		,ct.TXN_ACCOUNT\n"
                    + "		,ct.BRANCH_ID\n"
                    + "		,ft.FILE_ID\n"
                    + "		,ft.FILE_CREATE_DATETIME\n"
                    + "		,ft.FILE_SEQ\n"
                    + "		,ft.FILE_ROOT\n"
                    + "		,ft.FILE_PATH\n"
                    + "		,ft.FILE_TYPE\n"
                    + "		,'N' As PDF\n"
                    + "	From BATCH_CIF_TABLE bct\n"
                    + "	Left Join CASE_TABLE_{0} ct On (ct.CIF_ID = bct.CIF_ID)\n"
                    + "	Left Join FILE_TABLE_{0} ft On (ft.SESSION_KEY = ct.SESSION_KEY)\n"
                    + "	Where bct.CIF_ID Not In (\n"
                    + "					Select bct.CIF_ID\n"
                    + "					From BATCH_CIF_TABLE bct\n"
                    + "					Left Join CASE_TABLE_{0} ct On (bct.CIF_ID = ct.CIF_ID)\n"
                    + "					Left Join FILE_TABLE_{0} ft On (ft.SESSION_KEY = ct.SESSION_KEY)\n"
                    + "					Where ct.TXN_DATE >= @START_TXN_DATE And ct.TXN_DATE <= @END_TXN_DATE\n"
                    + "					And (ct.TRANS_STATE = '' Or Upper(ct.TRANS_STATE) = 'LIVE')\n"
                    + "					And ct.REPORT_TYPE = '水單'\n"
                    + "		)\n"
                    + ") B\n"
                    + "Order By B.CREATE_DATETIME Desc, B.SESSION_KEY, B.FILE_SEQ\n";

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "START_TXN_DATE"), START_TXN_DATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "END_TXN_DATE"), END_TXN_DATE));

                strSql = string.Format(strSql, YM);

                return strSql;
            }
            #endregion

            #region BATCH_CIF_TABLE_RealTime()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="START_TXN_DATE"></param>
            /// <param name="END_TXN_DATE"></param>
            /// <param name="YM"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string BATCH_CIF_TABLE_RealTime(string START_TXN_DATE, string END_TXN_DATE, string YM, ref List<IDataParameter> Parameters)
            {
                string strSql = "Select bct.CIF_ID\n"
                    + "	,bct.CREATE_DATETIME\n"
                    + "	,bct.COMMENTS\n"
                    + "	,ct.SESSION_KEY\n"
                    + "	,ct.TXN_DATE\n"
                    + "	,ct.TXN_ACCOUNT\n"
                    + "	,ct.CREATE_DATETIME\n"
                    + "	,ct.BRANCH_ID\n"
                    + "	,ft.FILE_ID\n"
                    + "	,ft.FILE_CREATE_DATETIME\n"
                    + "	,ft.FILE_SEQ\n"
                    + "	,ft.FILE_ROOT\n"
                    + "	,ft.FILE_PATH\n"
                    + "From BATCH_CIF_TABLE bct\n"
                    + "Inner Join CASE_TABLE_{0} ct On (ct.CIF_ID = bct.CIF_ID)\n"
                    + "Inner Join FILE_TABLE_{0} ft On (ft.SESSION_KEY = ct.SESSION_KEY And ft.FILE_TYPE = 11)\n"
                    + "Where ct.TXN_DATE >= @START_TXN_DATE And ct.TXN_DATE <= @END_TXN_DATE\n"
                    + "And (ct.TRANS_STATE = '' Or Upper(ct.TRANS_STATE) = 'LIVE')\n"
                    + "And ct.REPORT_TYPE = '水單'\n"
                    + "Order By ct.CREATE_DATETIME Desc, ct.SESSION_KEY, ft.FILE_SEQ\n";

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "START_TXN_DATE"), START_TXN_DATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "END_TXN_DATE"), END_TXN_DATE));

                strSql = string.Format(strSql, YM);

                return strSql;
            }
            #endregion

            #region BATCH_CIF_INFO()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="START_TXN_DATE"></param>
            /// <param name="END_TXN_DATE"></param>
            /// <param name="YM"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public string BATCH_CIF_INFO(string START_TXN_DATE, string END_TXN_DATE, string YM, ref List<IDataParameter> Parameters)
            {
                string strSql = "select  ex5103.T_MNEMONIC, ex5103.T_EMAIL_SIGN_1, ex5103.T_EMAIL_1,ct.CREATE_DATETIME, ct.SESSION_KEY, ct.TXN_DATE, ct.TXN_ACCOUNT, ct.BRANCH_ID, ft.FILE_ID, ft.FILE_CREATE_DATETIME, ft.FILE_SEQ, ft.FILE_ROOT, ft.FILE_PATH, 'Y' as PDF, ft.FILE_TYPE\n"
                                + "from CASE_TABLE_{0} ct\n"
                                + "Inner join (\n"
                                             + "select cut.T_MNEMONIC, cut.T_EMAIL_SIGN_1, cut.T_EMAIL_1, cut.RECID\n"
                                              + "from[ODS2-DB].[ODS2_DBU].[dbo].[CUSTOMER] cut\n"
                                            + " where cut.T_EMAIL_SIGN_1= '1' \n"
                                      + "and EXISTS(Select RECID From[ODS2-DB].[ODS2_DBU].[dbo].[TMB_CONS_STMT_CONF_C1] where CUST_ND_STMT_TYPE <> 'STMT_209')\n"
                                   + "and len(isnull(cut.T_EMAIL_1,'')) <> 0 \n"
                                              + ") ex5103\n"
                                    + "On ct.CIF_ID = ex5103.T_MNEMONIC\n"
                                  + "Inner Join FILE_TABLE_{0} ft\n"
                                  + "on ct.SESSION_KEY = ft.SESSION_KEY\n"
                                 + "Where ct.REPORT_TYPE = '水單' And ct.CHANNEL_CODE<> 'MMAB2B' and left(ct.TXN_ID, 1) not in('M', 'N', 'L') And(len(ct.TRANS_STATE) = 0 or ct.TRANS_STATE ='LIVE')\n"
                                 + "And ct.TXN_DATE >= @START_TXN_DATE And ct.TXN_DATE <= @END_TXN_DATE And ft.FILE_TYPE = 11\n"
                                 + "And NOT EXISTS(Select* From BATCH_CIF_TABLE Where CIF_ID= ct.CIF_ID)\n"
                                 + "order by CIF_ID";

                Parameters = new List<IDataParameter>();
                Parameters.Add(new SqlParameter(string.Format("@{0}", "START_TXN_DATE"), START_TXN_DATE));
                Parameters.Add(new SqlParameter(string.Format("@{0}", "END_TXN_DATE"), END_TXN_DATE));

                strSql = string.Format(strSql, YM);

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

        }
        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        public class Insert
        {
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
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete
        /// </summary>
        public class Delete
        {

        }
        #endregion
    }
}