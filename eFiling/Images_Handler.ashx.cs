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
using Aspose.Pdf;

namespace eFiling
{
    /// <summary>
    /// Images_Handler 的摘要描述
    /// </summary>
    public class Images_Handler : IHttpHandler
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
            this.MyUtility.InitLog("Images");

            context.Response.Clear();

            string data = string.Empty, message = string.Empty;

            SysCode code = SysCode.A000;

            string session_key = string.Empty, file_id = string.Empty, file_full_path = string.Empty, year_and_month = string.Empty, user_id = string.Empty;

            int file_type = 0, file_seq = 0;

            string[] para_arry = null;

            List<IDataParameter> para = null;

            DataTable dt = null;

            string strSql = string.Empty;

            Aspose.Pdf.License license = new Aspose.Pdf.License();

            license.SetLicense("Aspose.Pdf.lic");
            try
            {
                #region 取得參數

                data = context.GetRequest("data").DecryptDES();

                if (String.IsNullOrEmpty(data)) throw new Utility.ProcessException(string.Format("參數為空值"), ref code, SysCode.E004);

                this.MyUtility.WriteLog(Mode.LogMode.DEBUG, context, string.Format("DATA.Parameter:{0}", data));

                #endregion

                para_arry = data.Split('|');

                if (!para_arry.Length.Equals(7)) throw new Utility.ProcessException(string.Format("參數不足{0}/7", para_arry.Length), ref code, SysCode.E004);

                file_id = para_arry[0];

                file_type = Convert.ToInt32(para_arry[1]);

                year_and_month = para_arry[2];

                session_key = para_arry[3];

                file_full_path = para_arry[4];

                file_seq = Convert.ToInt32(para_arry[5]);

                user_id = para_arry[6];

                bool exists = File.Exists(file_full_path);

                if (!exists && file_type < 10) throw new Utility.ProcessException(string.Format("調閱檔案不存在"), ref code, SysCode.E006);

                if (!exists && file_type.Equals(11))
                {
                    DataRow[] dr = null;

                    strSql = this.MyUtility.Select.FILE_TABLE_NOT_IMAGE(year_and_month, file_id, ref para);

                    #region SQL Debug

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, strSql);

                    this.MyUtility.WriteLog(Log.Mode.LogMode.DEBUG, context, para.ToLog());

                    #endregion

                    dt = this.MyUtility.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                    string pdf_file_path = string.Empty;

                    if (dt.Rows.Count > 0)
                    {
                        dr = dt.Select(string.Format("FILE_TYPE=1"));

                        if (dr != null && dr.Length > 0) pdf_file_path = dr[0]["FILE_ROOT"].ToString().Trim().FilePathCombine(dr[0]["FILE_PATH"].ToString().Trim());
                    }
                    exists = File.Exists(pdf_file_path);

                    this.MyUtility.WriteLog(context, string.Format("PDF.Path:{0},{1}", pdf_file_path, exists));

                    if (!exists) throw new Utility.ProcessException(string.Format("PDF不存在"), ref code, SysCode.E006);

                    #region PDF 切割
                    try
                    {
                        using (Document pdfDocument = new Document(pdf_file_path))
                        {
                            Aspose.Pdf.Devices.Resolution resolution = new Aspose.Pdf.Devices.Resolution(150);

                            Aspose.Pdf.Devices.JpegDevice jpegDeviceLarge = new Aspose.Pdf.Devices.JpegDevice(resolution);

                            if (file_seq <= 0 || file_seq > pdfDocument.Pages.Count)
                            {
                                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, string.Format("PDF.Pages.Count:{0},FILE_SEQ:{1}", pdfDocument.Pages.Count, file_seq));

                                throw new Utility.ProcessException(string.Format("查無影像檔及無法從PDF內拆檔"), ref code, SysCode.E005);
                            }
                            jpegDeviceLarge.Process(pdfDocument.Pages[file_seq], file_full_path);

                            if (!File.Exists(file_full_path)) throw new Utility.ProcessException(string.Format("頁次{0}拆檔失敗", file_seq), ref code, SysCode.E005);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                        throw new Utility.ProcessException(ex.Message, ref code, SysCode.E005);
                    }
                    #endregion
                }
                this.MyUtility.DBLog(context, code, "Images", session_key, user_id, data);

                string file_ext = Path.GetExtension(file_full_path).Remove(0, 1);

                if (file_type < 10) this.MyUtility.ResponseFile(context, file_full_path, file_ext);
                else this.MyUtility.DrawImage(context, file_full_path, file_ext);
            }
            catch (System.Exception ex)
            {
                message = ex.Message;

                this.MyUtility.WriteLog(Mode.LogMode.ERROR, context, ex.ToString());

                code = !code.Equals(SysCode.A000) ? code : SysCode.E999;

                this.MyUtility.DBLog(context, code, "Images", session_key, user_id, !String.IsNullOrEmpty(data) ? string.Format("{0}\r\n{1}", data, ex.Message) : ex.Message);

                this.MyUtility.DrawMessage(context, ex.Message);

                this.MyUtility.SendEMail(context, "Images", session_key, code);
            }
            finally
            {
                license = null;

                dt = null;

                para = null;

                this.MyUtility.CloseConn();

                GC.Collect(); GC.WaitForPendingFinalizers();
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