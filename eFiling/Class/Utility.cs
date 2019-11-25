using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Log;
using System.Net;
using System.Text;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Security.Cryptography;
using System.Configuration;
using XBitmap;

namespace eFiling
{
    public class Utility
    {
        #region Exception
        /// <summary>
        /// 
        /// </summary>
        public class ProcessException : Exception
        {
            public ProcessException(string message, ref SysCode code, SysCode SysCodeMode) : base(message) { code = SysCodeMode; }
        }
        #endregion

        #region WebConfig
        /// <summary>
        /// 
        /// </summary>
        public class WebConfig
        {
            #region LOG_PATH
            /// <summary>
            /// 
            /// </summary>
            public static string LOG_PATH
            {
                get
                {
                    if (ConfigurationManager.AppSettings["LOG_PATH"] == null) throw new Exception("Web.Config Error : LOG_PATH");

                    return ConfigurationManager.AppSettings["LOG_PATH"].Trim();
                }
            }
            #endregion

            #region LOG_LEVEL
            /// <summary>
            /// 
            /// </summary>
            public static Int16 LOG_LEVEL
            {
                get
                {
                    if (ConfigurationManager.AppSettings["LOG_LEVEL"] == null) throw new Exception("Web.Config Error : LOG_LEVEL");

                    return Convert.ToInt16(ConfigurationManager.AppSettings["LOG_LEVEL"].Trim());
                }
            }
            #endregion

            //#region LOG_SIZE
            ///// <summary>
            ///// 
            ///// </summary>
            //public static Int16 LOG_SIZE
            //{
            //    get
            //    {
            //        if (ConfigurationManager.AppSettings["LOG_SIZE"] == null) throw new Exception("Web.Config Error : LOG_SIZE");

            //        return Convert.ToInt16(ConfigurationManager.AppSettings["LOG_SIZE"].Trim());
            //    }
            //}
            //#endregion
        }
        #endregion

        #region Log
        /// <summary>
        /// Log 介面
        /// </summary>
        private ILog MyLog = null;
        /// <summary>
        /// Log 介面
        /// </summary>
        public void InitLog(string LogName)
        {
            try
            {
                this.MyLog = new WebLog(LogName);
                //this.MyLog.MaxSize = Utility.WebConfig.LOG_SIZE;
                this.MyLog.DirectoryPath = Utility.WebConfig.LOG_PATH;
                this.MyLog.Level = Utility.WebConfig.LOG_LEVEL;

                DateTime now_date_time = DateTime.Now;
                string check_clear_path = Path.Combine(this.MyLog.DirectoryPath, "clear.flag");
                if (!File.Exists(check_clear_path)) File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss"));
                DateTime date_time = DateTime.Parse(File.ReadAllText(check_clear_path).Trim());
                bool clear = DateTime.Compare(DateTime.Now, date_time.AddDays(14)) > 0;

                if (clear)
                {
                    this.MyLog.Delete(14);

                    this.MyLog.DeleteBackup(14);

                    File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss"));

                    this.MyLog.WriteLog(Mode.LogMode.INFO, string.Format("ClearLog::{0}", now_date_time.ToString("yyyy/MM/dd HH:mm:ss")));
                }
                this.MyLog.WriteLog(Mode.LogMode.INFO, string.Format("[{0}]Start.{1}", this.GetClientIPv4(), LogName));
            }
            catch (System.Exception ex) { if (this.MyLog != null) this.MyLog.WriteLog(Mode.LogMode.ERROR, string.Format("ClearLog.Exception::{0}", ex.ToString())); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Message"></param>
        public void WriteLog(HttpContext Context, string Message)
        {
            if (this.MyLog != null) this.MyLog.WriteLog(Mode.LogMode.INFO, string.Format("[{0}] {1}", Context.GetClientIPv4(), Message));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="Context"></param>
        /// <param name="Message"></param>
        public void WriteLog(Mode.LogMode Mode, HttpContext Context, string Message)
        {
            if (this.MyLog != null) this.MyLog.WriteLog(Mode, string.Format("[{0}] {1}", Context.GetClientIPv4(), Message));
        }
        #endregion

        #region PDFUploadLog
        /// <summary>
        /// Log 介面
        /// </summary>
        private ILog PDFUploadLog = null;
        /// <summary>
        /// Log 介面
        /// </summary>
        public void InitPDFUploadLog(string LogName)
        {
            try
            {
                this.PDFUploadLog = new WebLog(LogName);
                this.PDFUploadLog.DirectoryPath = Path.Combine( Utility.WebConfig.LOG_PATH ,"PDFUploadFile");
                this.PDFUploadLog.Level = Utility.WebConfig.LOG_LEVEL;

                DateTime now_date_time = DateTime.Now;
                string check_clear_path = Path.Combine(this.PDFUploadLog.DirectoryPath, "clear.flag");
                if (!File.Exists(check_clear_path)) File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss"));
                DateTime date_time = DateTime.Parse(File.ReadAllText(check_clear_path).Trim());
                bool clear = DateTime.Compare(DateTime.Now, date_time.AddDays(14)) > 0;

                if (clear)
                {
                    this.PDFUploadLog.Delete(14);

                    this.PDFUploadLog.DeleteBackup(14);

                    File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss"));

                    this.PDFUploadLog.WriteLog(Mode.LogMode.INFO, string.Format("ClearLog::{0}", now_date_time.ToString("yyyy/MM/dd HH:mm:ss")));
                }
                this.PDFUploadLog.WriteLog(Mode.LogMode.INFO, string.Format("[{0}]Start.{1}", this.GetClientIPv4(), LogName));
            }
            catch (System.Exception ex) { if (this.PDFUploadLog != null) this.PDFUploadLog.WriteLog(Mode.LogMode.ERROR, string.Format("ClearLog.Exception::{0}", ex.ToString())); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Message"></param>
        public void WritePDFUploadLog(HttpContext Context, string Message)
        {
            this.PDFUploadLog.WriteLog(Mode.LogMode.INFO, string.Format("[{0}] {1}", Context.GetClientIPv4(), Message));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="Context"></param>
        /// <param name="Message"></param>
        public void WritePDFUploadLog(Mode.LogMode Mode, HttpContext Context, string Message)
        {
            this.PDFUploadLog.WriteLog(Mode, string.Format("[{0}] {1}", Context.GetClientIPv4(), Message));
        }
        #endregion

        #region MailLog
        /// <summary>
        /// Log 介面
        /// </summary>
        private ILog MyMailLog = null;
        /// <summary>
        /// Log 介面
        /// </summary>
        public void InitMailLog(string LogName)
        {
            try
            {
                this.MyMailLog = new WebLog(LogName);
                this.MyMailLog.DirectoryPath = Utility.WebConfig.LOG_PATH;
                this.MyMailLog.Level = Utility.WebConfig.LOG_LEVEL;

                DateTime now_date_time = DateTime.Now;
                string check_clear_path = Path.Combine(this.MyMailLog.DirectoryPath, "clear.flag");
                if (!File.Exists(check_clear_path)) File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss"));
                DateTime date_time = DateTime.Parse(File.ReadAllText(check_clear_path).Trim());
                bool clear = DateTime.Compare(DateTime.Now, date_time.AddDays(14)) > 0;

                if (clear)
                {
                    this.MyMailLog.Delete(14);

                    this.MyMailLog.DeleteBackup(14);

                    File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss"));

                    this.MyMailLog.WriteLog(Mode.LogMode.INFO, string.Format("ClearLog::{0}", now_date_time.ToString("yyyy/MM/dd HH:mm:ss")));
                }
                this.MyMailLog.WriteLog(Mode.LogMode.INFO, string.Format("[{0}]Start.{1}", this.GetClientIPv4(), LogName));
            }
            catch (System.Exception ex) { if (this.MyMailLog != null) this.MyMailLog.WriteLog(Mode.LogMode.ERROR, string.Format("ClearLog.Exception::{0}", ex.ToString())); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Message"></param>
        public void WriteMailLog(HttpContext Context, string Message)
        {
            this.MyMailLog.WriteLog(Mode.LogMode.INFO, string.Format("[{0}] {1}", Context.GetClientIPv4(), Message));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="Context"></param>
        /// <param name="Message"></param>
        public void WriteMailLog(Mode.LogMode Mode, HttpContext Context, string Message)
        {
            this.MyMailLog.WriteLog(Mode, string.Format("[{0}] {1}", Context.GetClientIPv4(), Message));
        }
        #endregion

        #region DBConn
        /// <summary>
        /// 資料庫物件介面
        /// </summary>
        public DBLib.IDBConn _DBConn = null;
        /// <summary>
        /// 資料庫物件介面
        /// </summary>
        public DBLib.IDBConn DBConn
        {
            get
            {
                if (this._DBConn == null)
                {
                    this._DBConn = new DBLib.DBConn(new DBLibUtility.Mode.FreeMode()
                    {
                        APMode = DBLibUtility.Mode.APMode.Web,
                        DBMode = DBLibUtility.Mode.DBMode.MSSQL,
                        ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString().DecryptDES()
                    });
                }
                return this._DBConn;
            }
            set { this._DBConn = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CloseConn()
        {
            if (this._DBConn != null)
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region DBConnTransac
        /// <summary>
        /// 資料庫物件介面
        /// </summary>
        public DBLib.IDBTransacConn _DBConnTransac = null;
        /// <summary>
        /// 資料庫物件介面
        /// </summary>
        public DBLib.IDBTransacConn DBConnTransac
        {
            get
            {
                if (this._DBConnTransac == null)
                {
                    this._DBConnTransac = new DBLib.DBConnTransac(new DBLibUtility.Mode.FreeMode()
                    {
                        APMode = DBLibUtility.Mode.APMode.Web,
                        DBMode = DBLibUtility.Mode.DBMode.MSSQL,
                        ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString().DecryptDES()
                    });
                }
                return this._DBConnTransac;
            }
            set { this._DBConnTransac = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Rollback()
        {
            try
            {
                if (this.DBConnTransac.GeneralSqlCmd.Transaction != null) this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();
            }
            catch { }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CloseConnTransac()
        {
            if (this._DBConnTransac != null)
            {
                this.DBConnTransac.Dispose(); this.DBConnTransac = null;
            }
        }
        #endregion

        #region SqlCommand
        /// <summary>
        /// Sql Command (Select)
        /// </summary>
        private SqlCommand.Select _Select = null;
        /// <summary>
        /// Sql Command (Select)
        /// </summary>
        public SqlCommand.Select Select
        {
            get
            {
                if (this._Select == null) this._Select = new SqlCommand.Select(); return this._Select;
            }
        }
        /// <summary>
        /// Sql Command (Update)
        /// </summary>
        private SqlCommand.Update _Update = null;
        /// <summary>
        /// Sql Command (Update)
        /// </summary>
        public SqlCommand.Update Update
        {
            get
            {
                if (this._Update == null) this._Update = new SqlCommand.Update(); return this._Update;
            }
        }
        /// <summary>
        /// Sql Command (Delete)
        /// </summary>
        private SqlCommand.Delete _Delete = null;
        /// <summary>
        /// Sql Command (Delete)
        /// </summary>
        public SqlCommand.Delete Delete
        {
            get
            {
                if (this._Delete == null) this._Delete = new SqlCommand.Delete(); return this._Delete;
            }
        }
        /// <summary>
        /// Sql Command (Insert)
        /// </summary>
        private SqlCommand.Insert _Insert = null;
        /// <summary>
        /// Sql Command (Insert)
        /// </summary>
        public SqlCommand.Insert Insert
        {
            get
            {
                if (this._Insert == null) this._Insert = new SqlCommand.Insert(); return this._Insert;
            }
        }
        #endregion

        #region DBLog()
        /// <summary>
        /// 
        /// </summary>
        public void DBLog(HttpContext Context, SysCode CODE, string SYSAP, string SESSION_KEY, string USERID, string COMMENTS)
        {
            string strSql = string.Empty;

            List<IDataParameter> para = null;

            DBLib.DBConn dbConn = null;
            try
            {
                dbConn = new DBLib.DBConn(new DBLibUtility.Mode.FreeMode()
                {
                    APMode = DBLibUtility.Mode.APMode.Web,
                    DBMode = DBLibUtility.Mode.DBMode.MSSQL,
                    ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString().DecryptDES()
                });
                strSql = this.Insert.SYSLOG(CODE, SYSAP, SESSION_KEY, this.GetClientIPv4(), USERID, COMMENTS, ref para);

                #region SQL Debug

                this.WriteLog(Log.Mode.LogMode.DEBUG, Context, strSql);

                this.WriteLog(Log.Mode.LogMode.DEBUG, Context, para.ToLog());

                #endregion

                int result = dbConn.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                this.WriteLog(Mode.LogMode.INFO, Context, string.Format("Result:{0}", result));
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                para = null;

                if (dbConn != null) dbConn.Dispose();

                dbConn = null;
            }
        }
        #endregion

        #region GetClientIPv4()
        /// <summary>
        /// 取得客戶端主機 IPv4 位址
        /// </summary>
        /// <returns></returns>
        private string GetClientIPv4()
        {
            try
            {
                string ipv4 = String.Empty;

                foreach (IPAddress ip in Dns.GetHostAddresses(this.GetClientIP()))
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        ipv4 = ip.ToString(); break;
                    }
                }
                if (ipv4 != String.Empty) return ipv4;

                foreach (IPAddress ip in Dns.GetHostEntry(this.GetClientIP()).AddressList)
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        ipv4 = ip.ToString(); break;
                    }

                }
                return ipv4;
            }
            catch
            {
                string ipAddressString = HttpContext.Current.Request.UserHostAddress;

                if (ipAddressString == null) return null;

                IPAddress ipAddress;

                IPAddress.TryParse(ipAddressString, out ipAddress);

                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    List<IPAddress> addressList = new List<IPAddress>(System.Net.Dns.GetHostEntry(ipAddress).AddressList);

                    ipAddress = addressList.Find(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                return ipAddress.ToString();
            }
        }
        #endregion

        #region GetClientIP()
        /// <summary>
        /// 取得客戶端主機位址
        /// </summary>
        private string GetClientIP()
        {
            if (null == HttpContext.Current.Request.ServerVariables["HTTP_VIA"])
            {
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }
        #endregion

        #region CreateFile()
        /// <summary>
        /// 
        /// </summary>
        public void CreateFile(string FilePath, string FileBody, ref SysCode Code)
        {
            try
            {
                FilePath.DeleteSigleFile();

                byte[] fileByte = Convert.FromBase64String(FileBody.Trim().Replace(" ", "+"));

                using (FileStream fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(fileByte, 0, fileByte.Length);
                    fileStream.Close();
                }
            }
            catch (System.Exception ex)
            {
                throw new Utility.ProcessException(ex.ToString(), ref Code, SysCode.E001);
            }
        }
        #endregion

        #region CheckAndCreateTable()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Code"></param>
        public void CheckAndCreateTable(HttpContext Context, string YearAndMonth, ref SysCode Code)
        {
            int t = -1;

            SysCode code = SysCode.A000;

            if (String.IsNullOrEmpty(YearAndMonth) || !YearAndMonth.Length.Equals(6) || !int.TryParse(YearAndMonth, out t)) throw new Utility.ProcessException(string.Format("YM Error"), ref code, SysCode.E004);

            string strSql = string.Empty;

            bool check = false;

            List<IDataParameter> para = null;

            DBLib.DBConn dbConn = null;
            try
            {
                dbConn = new DBLib.DBConn(new DBLibUtility.Mode.FreeMode()
                {
                    APMode = DBLibUtility.Mode.APMode.Web,
                    DBMode = DBLibUtility.Mode.DBMode.MSSQL,
                    ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString().DecryptDES()
                });

                #region CHECK_CASETABLE

                strSql = this.Select.CHECK_CASETABLE(YearAndMonth, ref para);

                #region SQL Debug

                this.WriteLog(Log.Mode.LogMode.DEBUG, Context, strSql);

                this.WriteLog(Log.Mode.LogMode.DEBUG, Context, para.ToLog());

                #endregion

                check = dbConn.GeneralSqlCmd.ExecuteScalar(strSql, para);

                this.WriteLog(Context, string.Format("Result:{0}", check));

                if (!check)
                {
                    strSql = string.Format("Execute createCaseTable '{0}'", YearAndMonth);

                    #region SQL Debug

                    this.WriteLog(Log.Mode.LogMode.DEBUG, Context, strSql);

                    this.WriteLog(Log.Mode.LogMode.DEBUG, Context, para.ToLog());

                    #endregion

                    dbConn.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                    strSql = this.Select.CHECK_CASETABLE(YearAndMonth, ref para);

                    #region SQL Debug

                    this.WriteLog(Log.Mode.LogMode.DEBUG, Context, strSql);

                    this.WriteLog(Log.Mode.LogMode.DEBUG, Context, para.ToLog());

                    #endregion

                    check = dbConn.GeneralSqlCmd.ExecuteScalar(strSql, para);

                    this.WriteLog(Context, string.Format("Result:{0}", check));

                    if (!check) throw new Exception(string.Format("新增CASE_TABLE_{0}失敗", check));
                }
                #endregion

                #region CHECK_FILETABLE

                strSql = this.Select.CHECK_FILETABLE(YearAndMonth, ref para);

                #region SQL Debug

                this.WriteLog(Log.Mode.LogMode.DEBUG, Context, strSql);

                this.WriteLog(Log.Mode.LogMode.DEBUG, Context, para.ToLog());

                #endregion

                check = dbConn.GeneralSqlCmd.ExecuteScalar(strSql, para);

                this.WriteLog(Context, string.Format("Result:{0}", check));

                if (!check)
                {
                    strSql = string.Format("Execute createFileTable '{0}'", YearAndMonth);

                    #region SQL Debug

                    this.WriteLog(Log.Mode.LogMode.DEBUG, Context, strSql);

                    this.WriteLog(Log.Mode.LogMode.DEBUG, Context, para.ToLog());

                    #endregion

                    dbConn.GeneralSqlCmd.ExecuteNonQuery(strSql, para);

                    strSql = this.Select.CHECK_FILETABLE(YearAndMonth, ref para);

                    #region SQL Debug

                    this.WriteLog(Log.Mode.LogMode.DEBUG, Context, strSql);

                    this.WriteLog(Log.Mode.LogMode.DEBUG, Context, para.ToLog());

                    #endregion

                    check = dbConn.GeneralSqlCmd.ExecuteScalar(strSql, para);

                    this.WriteLog(Context, string.Format("Result:{0}", check));

                    if (!check) throw new Exception(string.Format("新增FILE_TABLE_{0}失敗", check));
                }
                #endregion
            }
            catch (System.Exception ex)
            {
                throw new Utility.ProcessException(ex.ToString(), ref Code, SysCode.E002);
            }
            finally
            {
                para = null;

                if (dbConn != null) dbConn.Dispose();

                dbConn = null;
            }
        }
        #endregion

        #region DrawImage()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyPage"></param>
        /// <param name="FilePath"></param>
        /// <param name="FileExt"></param>
        /// <param name="Thumbnail"></param>
        /// <param name="DPI"></param>
        /// <param name="Mode"></param>
        public void DrawImage(HttpContext Context, string FilePath, string FileExt)
        {
            Bitmap bmp = null;

            string contentType = string.Format("image/{0}", FileExt);

            ImageFormat imageFormat = ImageFormat.Png;
            try
            {
                if (FileExt.ToLower().EndsWith("png"))
                {
                    imageFormat = ImageFormat.Png;

                    contentType = string.Format("image/png");
                }
                else if (FileExt.ToLower().EndsWith("jpg"))
                {
                    imageFormat = ImageFormat.Jpeg;

                    contentType = string.Format("image/jpg");
                }
                else if (FileExt.ToLower().EndsWith("tif") || FileExt.ToLower().EndsWith("tiff"))
                {
                    imageFormat = ImageFormat.Tiff;

                    contentType = string.Format("image/jpg");
                }
                bmp = bmp.CreateBitmap(FilePath);
            }
            catch (System.Exception ex) { throw new Exception(string.Format("DrawImage.Exception::\r\n{0}", ex)); }
            finally
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, imageFormat);
                    Context.Response.ClearContent();
                    Context.Response.ContentType = contentType;
                    Context.Response.BinaryWrite(ms.ToArray());
                    ms.Close();
                }
                bmp.DisposeBitmap();
            }
            GC.Collect(); GC.WaitForPendingFinalizers();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyPage"></param>
        /// <param name="Bmp"></param>
        /// <param name="FileExt"></param>
        public void DrawImage(HttpContext Context, ref Bitmap Bmp, string FileExt)
        {
            string contentType = string.Format("image/{0}", FileExt);

            ImageFormat imageFormat = ImageFormat.Png;
            try
            {
                if (FileExt.ToLower().Equals("png"))
                {
                    imageFormat = ImageFormat.Png;

                    contentType = string.Format("image/png");
                }
                else if (FileExt.ToLower().Equals("jpg"))
                {
                    imageFormat = ImageFormat.Jpeg;

                    contentType = string.Format("image/jpg");
                }
            }
            catch (System.Exception ex) { throw new Exception(string.Format("DrawImage.Exception::\r\n{0}", ex)); }
            finally
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Bmp.Save(ms, imageFormat);
                    Context.Response.ClearContent();
                    Context.Response.ContentType = contentType;
                    Context.Response.BinaryWrite(ms.ToArray());
                    ms.Close();
                }
                Bmp.DisposeBitmap();
            }
            GC.Collect(); GC.WaitForPendingFinalizers();
        }
        #endregion

        #region DrawMessage()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Message"></param>
        internal void DrawMessage(HttpContext Context, string Message)
        {
            Font word_font = new Font("微軟正黑體", 12.5f, FontStyle.Regular);

            int w = 1653, h = 2338;

            char[] delimiterChars = { ' ', '\t', '\n' };

            string[] words = Message.Split(delimiterChars);

            using (Bitmap bmp = new Bitmap(w, h))
            {
                bmp.SetResolution(200, 200);

                Graphics g = Graphics.FromImage(bmp);

                Point point = new Point(0, 0);

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(point, new Size(bmp.Width, bmp.Height)),
                    Color.FromArgb(247, 140, 20),
                    Color.White,
                    80f,
                    true))
                {
                    Point p1 = new Point(0, point.Y);
                    Point p2 = new Point(0, point.Y + h);
                    Point p3 = new Point(w, point.Y + h);
                    Point p4 = new Point(w, point.Y);

                    Point[] curvePoint = new Point[] { p1, p2, p3, p4 };

                    g.FillPolygon(brush, curvePoint);
                }
                float word_gap = 5, word_y = word_gap;

                for (int i = 0; i < words.Length; i++)
                {
                    using (SolidBrush word_brush = new SolidBrush(Color.Black))
                    {
                        g.DrawString(words[i], word_font, word_brush, word_gap, word_y);
                    }
                    SizeF word_size = g.MeasureString(words[i], word_font, (int)(w - (word_gap * 2)));

                    word_y += word_size.Height + word_gap;
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    Context.Response.ClearContent();
                    Context.Response.ContentType = string.Format("image/png");
                    Context.Response.BinaryWrite(ms.ToArray());
                    ms.Close();
                }
                bmp.Dispose();
            }
            GC.Collect(); GC.WaitForPendingFinalizers();
        }
        #endregion

        #region ResponseFile()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyPage"></param>
        /// <param name="FilePath"></param>
        /// <param name="FileExtension"></param>
        public void ResponseFile(HttpContext Context, string FilePath, string FileExtension)
        {
            string contentType = FileExtension.GetContentType();

            FileInfo file_info = new FileInfo(FilePath);
            long file_size = file_info.Length;
            string file_name = file_info.Name;
            file_info = null;
            Context.Response.Clear();
            Context.Response.ClearHeaders();
            Context.Response.Buffer = false;
            Context.Response.ContentType = contentType;
            Context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(file_name, System.Text.Encoding.UTF8));
            Context.Response.AppendHeader("Content-Length", file_size.ToString());
            Context.Response.WriteFile(FilePath);
        }
        #endregion

        #region SendEMail()
        /// <summary>
        /// 
        /// </summary>
        internal class MailInfo
        {
            public string SMTPServer { get; set; }
            public int Port { get; set; }
            public string Account { get; set; }
            public string Password { get; set; }
            public string From { get; set; }
            public string Address { get; set; }
            public string Subject { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="APIName"></param>
        /// <param name="SessionKey"></param>
        /// <param name="Code"></param>
        public void SendEMail(HttpContext Context, string APIName, string SessionKey, SysCode Code)
        {
            this.InitMailLog("SendEMail");

            this.WriteMailLog(Mode.LogMode.INFO, Context, string.Format("Code::{0}", Code));

            if (Code != SysCode.E002 && Code != SysCode.E003 && Code != SysCode.E005 && Code != SysCode.E999) return;

            MailInfo info = new MailInfo();

            DBLib.DBConn dbConn = null;

            DataTable dt = null;

            string strSql = string.Empty;
            try
            {
                dbConn = new DBLib.DBConn(new DBLibUtility.Mode.FreeMode()
                {
                    APMode = DBLibUtility.Mode.APMode.Web,
                    DBMode = DBLibUtility.Mode.DBMode.MSSQL,
                    ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString().DecryptDES()
                });
                strSql = this.Select.SMTP();

                #region SQL Debug

                this.WriteMailLog(Context, strSql);

                #endregion

                dt = dbConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                this.WriteMailLog(Mode.LogMode.INFO, Context, string.Format("SMTP::{0}", dt.Rows.Count));

                if (dt.Rows.Count.Equals(0)) return;

                info = new MailInfo()
                {
                    SMTPServer = dt.Select(string.Format("FUNCTION_CODE='{0}'", "SMTP_IP"))[0]["PARAMETER"].ToString(),
                    Port = Convert.ToInt32(dt.Select(string.Format("FUNCTION_CODE='{0}'", "SMTP_POST"))[0]["PARAMETER"].ToString()),
                    Account = dt.Select(string.Format("FUNCTION_CODE='{0}'", "SMTP_USER"))[0]["PARAMETER"].ToString(),
                    Password = dt.Select(string.Format("FUNCTION_CODE='{0}'", "SMTP_PWD"))[0]["PARAMETER"].ToString(),
                    From = dt.Select(string.Format("FUNCTION_CODE='{0}'", "SMTP_FROM"))[0]["PARAMETER"].ToString(),
                    Address = dt.Select(string.Format("FUNCTION_CODE='{0}'", "SMTP_ERROR_TO_ADDRESS"))[0]["PARAMETER"].ToString(),
                    Subject = "eFiling 系統發生錯誤"
                };
                string message = string.Format("案件代碼:{0}<br/>API名稱:{1}<br/>IP:{2}<br/>錯誤代碼:{3}<br/>", SessionKey, APIName, this.GetClientIPv4(), Code.ToString());

                this.SendEMail(Context, info, message, dt.Select(string.Format("FUNCTION_CODE='{0}'", "SMTP_SSL"))[0]["PARAMETER"].ToString().Trim().ToLower().Equals("true"));
            }
            catch (System.Exception ex) { this.WriteMailLog(Mode.LogMode.ERROR, Context, string.Format("SendEMail.Exception::{0}", ex.Message)); throw ex; }
            finally
            {
                dt = null;

                if (dbConn != null) dbConn.Dispose();

                dbConn = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Info"></param>
        /// <param name="Message"></param>
        /// <param name="EnableSsl"></param>
        private void SendEMail(HttpContext Context, MailInfo Info, string Message, bool EnableSsl)
        {
            XMail.SendMail sendMail = null;
            try
            {
                this.WriteMailLog(Mode.LogMode.INFO, Context, string.Format("SendEMail.Start::{0},{1},{2},{3}", Info.SMTPServer, Info.Account, Info.Password, Info.Port));

                if (String.IsNullOrEmpty(Info.Address)) return;

                sendMail = new XMail.SendMail(Info.SMTPServer, Info.Account, Info.Password, Convert.ToInt32(Info.Port));

                sendMail.From = Info.From;

                sendMail.Address = Info.Address.Split(';');

                sendMail.Subject = Info.Subject;

                sendMail.IsBodyHtml = true;

                sendMail.EnableSsl = EnableSsl;

                sendMail.Body = Message;

                if (!String.IsNullOrEmpty(Info.SMTPServer)) sendMail.Send();
            }
            catch (System.Exception ex)
            {
                this.WriteMailLog(Mode.LogMode.ERROR, Context, string.Format("SendEMail.Exception::{0},{1},{2},{3},{4}", Info.SMTPServer, Info.Account, Info.Password, Info.Port, ex.Message));
            }
            finally
            {
                sendMail = null;
            }
        }
        #endregion
    }
}