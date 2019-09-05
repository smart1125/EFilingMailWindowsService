using Aspose.Pdf;
using EFilingMailWindowsServiceTest.Class;
using Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFilingMailWindowsServiceTest
{
    public partial class Form1 : Form
    {
        private double MinTimerInterval = 1000 * 10;

        #region BATCH_CIFTimer_Interval
        /// <summary>
        /// 
        /// </summary>
        private double _BATCH_CaseIDTimer_Interval = 1000 * 10;
        /// <summary>
        /// 
        /// </summary>
        private double BATCH_CaseIDTimer_Interval
        {
            get
            {
                if (this._BATCH_CaseIDTimer_Interval < this.MinTimerInterval) this._BATCH_CaseIDTimer_Interval = this.MinTimerInterval;

                return this._BATCH_CaseIDTimer_Interval;
            }
            set
            {
                this._BATCH_CaseIDTimer_Interval = value;
            }
        }
        #endregion

        #region FTPTimer_Interval
        /// <summary>
        /// 
        /// </summary>
        private double _FTPTimer_Interval = 1000 * 10;
        /// <summary>
        /// 
        /// </summary>
        private double FTPTimer_Interval
        {
            get
            {
                if (this._FTPTimer_Interval < this.MinTimerInterval) this._FTPTimer_Interval = this.MinTimerInterval;

                return this._FTPTimer_Interval;
            }
            set
            {
                this._FTPTimer_Interval = value;
            }
        }
        #endregion

        #region RealTimeTimer_Interval
        /// <summary>
        /// 
        /// </summary>
        private double _RealTimeTimer_Interval = 1000 * 10;
        /// <summary>
        /// 
        /// </summary>
        private double RealTimeTimer_Interval
        {
            get
            {
                if (this._RealTimeTimer_Interval < this.MinTimerInterval) this._RealTimeTimer_Interval = this.MinTimerInterval;

                return this._RealTimeTimer_Interval;
            }
            set
            {
                this._RealTimeTimer_Interval = value;
            }
        }
        #endregion

        #region Log
        /// <summary>
        /// Log 介面
        /// </summary>
        private ILog _Log = null;
        /// <summary>
        /// Log 介面
        /// </summary>
        private void InitLog(string LogName)
        {
            if (this._Log == null)
            {
                this._Log = new WindowLog(LogName);
                this._Log.MaxSize = 4;

                try
                {
                    DateTime now_date_time = DateTime.Now;
                    string check_clear_path = Path.Combine(this._Log.DirectoryPath, "clear.flag");
                    if (!File.Exists(check_clear_path)) File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss"));
                    DateTime date_time = DateTime.Parse(File.ReadAllText(check_clear_path).Trim());
                    bool clear = DateTime.Compare(DateTime.Now, date_time.AddDays(14)) > 0;

                    if (clear)
                    {
                        this._Log.Delete(14);

                        this._Log.DeleteBackup(14);

                        File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss"));

                        this._Log.WriteLog(Mode.LogMode.INFO, string.Format("ClearLog::{0}", now_date_time.ToString("yyyy/MM/dd HH:mm:ss")));
                    }
                    this._Log.WriteLog(Mode.LogMode.INFO, string.Format("Start.{0}", LogName));
                }
                catch (System.Exception ex) { this._Log.WriteLog(Mode.LogMode.ERROR, string.Format("ClearLog.Exception::{0}", ex.ToString())); }
            }
        }
        /// <summary>
        /// 寫入成 Log
        /// </summary>
        /// <param name="Message">Log 訊息</param>
        private void WriteLog(string Message)
        {
            this._Log.WriteLog(Mode.LogMode.INFO, Message);
        }
        /// <summary>
        /// 寫入成 Log
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="Message">Log 訊息</param>
        private void WriteLog(Mode.LogMode Mode, string Message)
        {
            if (this._Log != null) this._Log.WriteLog(Mode, Message);
        }
        #endregion

        #region BATCH_CIFTimer
        /// <summary>
        /// 
        /// </summary>
        private static bool BATCH_CaseIDTimerFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private System.Timers.Timer _BATCH_CaseIDTimer = null;
        /// <summary>
        /// 
        /// </summary>
        private System.Timers.Timer BATCH_CaseIDTimer
        {
            get
            {
                if (_BATCH_CaseIDTimer == null) _BATCH_CaseIDTimer = new System.Timers.Timer();

                return _BATCH_CaseIDTimer;
            }
            set { _BATCH_CaseIDTimer = value; }
        }
        #endregion

        #region FTPTimer
        /// <summary>
        /// 
        /// </summary>
        private static bool FTPTimerFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private System.Timers.Timer _FTPTimer = null;
        /// <summary>
        /// 
        /// </summary>
        private System.Timers.Timer FTPTimer
        {
            get
            {
                if (_FTPTimer == null) _FTPTimer = new System.Timers.Timer();

                return _FTPTimer;
            }
            set { _FTPTimer = value; }
        }
        #endregion

        #region RealTimeTimer
        /// <summary>
        /// 
        /// </summary>
        private static bool RealTimeTimerFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private System.Timers.Timer _RealTimeTimer = null;
        /// <summary>
        /// 
        /// </summary>
        private System.Timers.Timer RealTimeTimer
        {
            get
            {
                if (_RealTimeTimer == null) _RealTimeTimer = new System.Timers.Timer();

                return _RealTimeTimer;
            }
            set { _RealTimeTimer = value; }
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.StartApplication();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Private Methods

        #region StartApplication()
        /// <summary>
        /// 
        /// </summary>
        private void StartApplication()
        {
            Utility utility = new Utility();

            this.BATCH_CaseIDTimer_Interval = (Double)utility.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "Batch").FirstOrDefault().Element("interval") * 1000;
            this.RealTimeTimer_Interval = (Double)utility.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "Real").FirstOrDefault().Element("interval") * 1000;
            this.FTPTimer_Interval = (Double)utility.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "FTP").FirstOrDefault().Element("interval") * 1000;

            utility = null;

            this.BATCH_CaseIDTimer.Interval = this.BATCH_CaseIDTimer_Interval;
            this.BATCH_CaseIDTimer.Elapsed += delegate (Object senderWT, System.Timers.ElapsedEventArgs eWT)
            {
                if (!Form1.BATCH_CaseIDTimerFlag) this.StartBATCH_CIF();
            };
            this.BATCH_CaseIDTimer.Start();

            this.StartBATCH_CIF();

            this.RealTimeTimer.Interval = this.RealTimeTimer_Interval;
            this.RealTimeTimer.Elapsed += delegate (Object senderWT, System.Timers.ElapsedEventArgs eWT)
            {
                if (!Form1.RealTimeTimerFlag) this.StartRealTime();
            };
            this.RealTimeTimer.Start();

            this.StartRealTime();

            this.FTPTimer.Interval = this.FTPTimer_Interval;
            this.FTPTimer.Elapsed += delegate (Object senderWT, System.Timers.ElapsedEventArgs eWT)
            {
                if (!Form1.FTPTimerFlag) this.StartFTP();
            };
            this.FTPTimer.Start();

            this.StartFTP();
        }
        #endregion

        #region EndApplication()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndApplication()
        {
            this.BATCH_CaseIDTimer.Stop();
            this.BATCH_CaseIDTimer.Close();
            this.BATCH_CaseIDTimer.Dispose();
            this.BATCH_CaseIDTimer = null;

            this.FTPTimer.Stop();
            this.FTPTimer.Close();
            this.FTPTimer.Dispose();
            this.FTPTimer = null;

            this.RealTimeTimer.Stop();
            this.RealTimeTimer.Close();
            this.RealTimeTimer.Dispose();
            this.RealTimeTimer = null;

            Form1.BATCH_CaseIDTimerFlag = Form1.FTPTimerFlag = Form1.RealTimeTimerFlag = false;
        }
        #endregion

        #region WriteLastWorkingTime()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyUtility"></param>
        /// <param name="MyLog"></param>
        private void WriteLastWorkingTime(Utility MyUtility)
        {
            if (MyUtility == null || MyUtility.XDocSetting == null) return;

            this.InitLog("WriteLastWorkingTime");

            this.WriteLog(Mode.LogMode.INFO, string.Format("WriteLastWorkingTime.Start"));

            MyUtility.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "Batch").FirstOrDefault().SetElementValue("lastWorkingTime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            int rety_count = 0;

            while (true)
            {
                try
                {
                    MyUtility.XDocSetting.Save(ConfigPath.SettingPath);

                    break;
                }
                catch (System.Exception ex)
                {
                    rety_count++;

                    this.WriteLog(Mode.LogMode.ERROR, string.Format("[WriteLastWorkingTime]嘗試存檔失敗第{0}次\r\n{1}", rety_count, ex.ToString()));

                    if (rety_count >= 10) throw new Exception(string.Format("[WriteLastWorkingTime]最後執行時間寫入失敗"));

                    Thread.Sleep(100);
                }
            }
        }
        #endregion

        #region WorkingDateSwitch()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyUtility"></param>
        /// <param name="MyLog"></param>
        private void WriteWorkingDateSwitch(Utility MyUtility, ILog MyLog)
        {
            if (MyUtility == null || MyUtility.XDocSetting == null) return;

            if (MyLog != null) MyLog.WriteLog(Mode.LogMode.INFO, string.Format("WriteWorkingDateSwitch.Start"));

            //MyUtility.XDocSetting.SetAttributesValue("./Changingtec/WorkingDate", "Switch", "N");

            int rety_count = 0;

            while (true)
            {
                try
                {
                    MyUtility.XDocSetting.Save(ConfigPath.SettingPath);

                    break;
                }
                catch (System.Exception ex)
                {
                    rety_count++;

                    if (MyLog != null) MyLog.WriteLog(Mode.LogMode.ERROR, string.Format("[WriteWorkingDateSwitch]嘗試存檔失敗第{0}次\r\n{1}", rety_count, ex.ToString()));

                    //todo 確認workDateSwitch是作什麼用的
                    if (rety_count >= 10) throw new Exception(string.Format("[WriteWorkingDateSwitch]最後執行時間寫入失敗"));

                    Thread.Sleep(100);
                }
            }
        }
        #endregion

        #region StartBATCH_CIF()
        /// <summary>
        /// 
        /// </summary>
        private void StartBATCH_CIF()
        {
            Form1.BATCH_CaseIDTimerFlag = true;

            Utility utility = null;

            string strSql = string.Empty;

            List<IDataParameter> para = null;

            DataTable dt = null;

            //todo 確認是否需要先複制再合檔？
            //string file_temp_dir_path = Path.Combine(ConfigPath.TempDirPath, Guid.NewGuid().ToString());

            try
            {
                utility = new Utility();

                this.InitLog("BATCH_CIF");

                if (utility.XDocSetting == null)
                {
                    this.WriteLog(Mode.LogMode.ERROR, string.Format("沒有設定檔"));

                    return;
                }
                //string last_working_time = (string)utility.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "Batch").FirstOrDefault().Element("lastWorkingTime");

                //bool check_working = String.IsNullOrEmpty(last_working_time);

                //if (!check_working)
                //{
                //    check_working = !DateTime.Parse(last_working_time).ToString("yyyyMMdd").Equals(DateTime.Now.AddSeconds(this.BATCH_CaseIDTimer_Interval / 1000).ToString("yyyyMMdd"));
                //}
                //if (check_working)
                //{
                //    string startTime = (string)utility.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "Batch").FirstOrDefault().Element("start");

                //    if (String.IsNullOrEmpty(startTime) || startTime.Length != 4) throw new Exception(string.Format("每日固定執行時間設定錯誤"));

                //    DateTime working_time = DateTime.Parse(string.Format("{0} {1}:{2}:00", DateTime.Now.ToString("yyyy/MM/dd"), startTime.Substring(0, 2), startTime.Substring(2, 2)));

                //    this.WriteLog(Mode.LogMode.INFO, string.Format("預計開始執行時間:{0}", working_time.ToString("yyyy/MM/dd HH:mm:ss")));

                //    TimeSpan ts = DateTime.Now - working_time;

                //    check_working = ts.TotalSeconds >= 0 && ts.TotalSeconds <= (this.BATCH_CaseIDTimer_Interval / 1000);

                //    this.WriteLog(Mode.LogMode.INFO, string.Format("比對時間結果:{0},{1}", check_working ? "準備開始" : "時間未到", ts.TotalSeconds.ToString()));
                //}
                //if (!check_working) return;

                DateTime start_date = DateTime.Now.AddDays(-1);

                this.WriteLog(Mode.LogMode.INFO, string.Format("準備執行 {0} 的作業", start_date.ToString("yyyy/MM/dd")));


                strSql = utility.Select.BATCH_CIF_INFO(start_date.ToString("yyyy/MM/dd 00:00:00"), start_date.ToString("yyyy/MM/dd 23:59:59"), start_date.ToString("yyyyMM"), ref para);

                #region SQL Debug

                this.WriteLog(Log.Mode.LogMode.DEBUG, strSql);

                this.WriteLog(Log.Mode.LogMode.DEBUG, para.ToLog());

                #endregion

                dt = utility.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                this.WriteLog(Mode.LogMode.INFO, string.Format("共有{0}筆資料", dt.Rows.Count));

                if (dt.Rows.Count.Equals(0))
                {
                    this.WriteLastWorkingTime(utility); return;
                }
                string message = string.Empty;

                var query = from batch in dt.AsEnumerable()
                            select new
                            {
                                CREATE_DATETIME = batch.Field<DateTime>("CREATE_DATETIME"),
                                SESSION_KEY = batch.Field<string>("SESSION_KEY"),
                                TXN_DATE = batch.Field<DateTime>("TXN_DATE"),
                                TXN_ACCOUNT = batch.Field<string>("TXN_ACCOUNT"),
                                BRANCH_ID = batch.Field<string>("BRANCH_ID"),
                                FILE_ID = batch.Field<string>("FILE_ID"),
                                FILE_CREATE_DATETIME = batch.Field<DateTime>("CREATE_DATETIME"),
                                FILE_SEQ = batch.Field<Int16>("FILE_SEQ"),
                                FILE_ROOT = batch.Field<string>("FILE_ROOT"),
                                FILE_PATH = batch.Field<string>("FILE_PATH"),
                                PDF = batch.Field<string>("PDF"),
                                FILE_TYPE = batch.Field<Int16>("FILE_TYPE"),
                                T_MNEMONIC = batch.Field<string>("T_MNEMONIC"),
                                T_EMAIL_SIGN_1 = batch.Field<string>("T_EMAIL_SIGN_1"),
                                T_EMAIL_1 = batch.Field<string>("T_EMAIL_1")
                            };

                var queryG = query.GroupBy(o => o.T_MNEMONIC);

                Aspose.Pdf.License licenseWordsTxt = new Aspose.Pdf.License();
                licenseWordsTxt.SetLicense("Aspose.Pdf.lic");

                //if (!Directory.Exists(file_temp_dir_path)) Directory.CreateDirectory(file_temp_dir_path);

                string txt_file_path = Path.Combine(ConfigPath.FtpDirPath, string.Format("{0}.txt", start_date.ToString("yyyyMMdd")));

                txt_file_path.DeleteSigleFile();

                StreamWriter streamWriter = new StreamWriter(txt_file_path);

                foreach (var item in queryG)
                {
                    this.WriteLog(Log.Mode.LogMode.INFO, string.Format("準備產生{0}", item.Key));

                    Document doc = new Document();

                    string pdf_file_path = Path.Combine(ConfigPath.FtpDirPath, string.Format("{0}_{1}_001.pdf", start_date.ToString("yyyyMMdd"), item.Key));

                    string content = string.Empty;
                    string cif_ID = string.Empty;

                    //if (!File.Exists(pdf_file_path))
                    {
                        long pdfFileSize = 0;
                        bool isPDFSizeTooLength = false;
                        int fileNameCount = 1;

                        foreach (var batch in item)
                        {
                            try
                            {
                                utility.DBLog(SysCode.A010, "Batch", batch.T_MNEMONIC, string.Format("開始產生PDF:{0}", Path.GetFileNameWithoutExtension(pdf_file_path)));

                                string file_root = batch.FILE_ROOT;

                                string file_path = batch.FILE_PATH;

                                string file_full_path = file_root.FilePathCombine(file_path);

                                bool exists = File.Exists(file_full_path);

                                if (!exists)
                                {
                                    this.WriteLog(Log.Mode.LogMode.ERROR, message = string.Format("SESSION_KEY:{0},CIF_ID:{1},FILE_ID:{2}::檔案不存在({3})", batch.SESSION_KEY, batch.T_MNEMONIC, batch.FILE_ID, file_full_path));
                                    continue;
                                }

                                switch (batch.T_MNEMONIC.Length)
                                {
                                    case 9:
                                    case 11:
                                        cif_ID = batch.T_MNEMONIC.Substring(0, batch.T_MNEMONIC.Length - 2);
                                        break;
                                    default:
                                        cif_ID = batch.T_MNEMONIC;
                                        break;
                                }
                                // Load the source image file to Stream object
                                FileStream fs = new FileStream(file_full_path, FileMode.Open, FileAccess.Read);

                                pdfFileSize += fs.Length;

                                byte[] tmpBytes = new byte[fs.Length];
                                fs.Read(tmpBytes, 0, int.Parse(fs.Length.ToString()));

                                MemoryStream mystream = new MemoryStream(tmpBytes);

                                // Instantiate BitMap object with loaded image stream
                                Bitmap b = new Bitmap(mystream);

                                // Add a page to pages collection of document
                                Page page = doc.Pages.Add();

                                // Set margins so image will fit, etc.
                                page.PageInfo.Margin.Bottom = 0;
                                page.PageInfo.Margin.Top = 0;
                                page.PageInfo.Margin.Left = 0;
                                page.PageInfo.Margin.Right = 0;

                                page.CropBox = new Aspose.Pdf.Rectangle(0, 0, b.Width, b.Height);
                                // Create an image object
                                Aspose.Pdf.Image image1 = new Aspose.Pdf.Image();
                                // Add the image into paragraphs collection of the section
                                page.Paragraphs.Add(image1);
                                // Set the image file stream
                                image1.ImageStream = mystream;

                                image1.ImageScale = 0.95F;

                                //大於9M的要先存檔
                                if (pdfFileSize > 9437184)
                                {
                                    pdf_file_path = Path.Combine(ConfigPath.FtpDirPath, string.Format("{0}_{1}_{2}.pdf", start_date.ToString("yyyyMMdd"), item.Key, fileNameCount.ToString().PadLeft(3, '0')));

                                    this.WriteLog(Log.Mode.LogMode.DEBUG, string.Format("pdfFileSize:{0}、pdf_file_path:{1}", pdfFileSize, pdf_file_path));

                                    content = Path.GetFileNameWithoutExtension(pdf_file_path) + ":" + cif_ID + ":" + batch.T_EMAIL_1 + ":" + batch.TXN_DATE.ToString("yyyyMMdd");

                                    pdf_file_path.DeleteSigleFile();

                                    doc.Encrypt("23113343", "23113343", 0, CryptoAlgorithm.AESx128);
                                    doc.Save(pdf_file_path);
                                    mystream.Close();
                                    doc.Pages.Clear();
                                    doc.Dispose();
                                    doc = null;

                                    doc = new Document();

                                    streamWriter.WriteLine(content);

                                    fileNameCount += 1;
                                    pdfFileSize = 0;
                                    isPDFSizeTooLength = true;
                                }
                                else {
                                    isPDFSizeTooLength = false;

                                    content = Path.GetFileNameWithoutExtension(pdf_file_path) + ":" + cif_ID + ":" + batch.T_EMAIL_1 + ":" + batch.TXN_DATE.ToString("yyyyMMdd");
                                }
                            }
                            catch (System.Exception ex)
                            {
                                this.WriteLog(Log.Mode.LogMode.ERROR, ex.ToString());

                                utility.DBLog(SysCode.E013, "Batch", batch.T_MNEMONIC, string.Format("{0}::{1}", Path.GetFileNameWithoutExtension(pdf_file_path), ex.Message));
                            }
                        }

                        if (!isPDFSizeTooLength)
                        {
                            pdf_file_path = Path.Combine(ConfigPath.FtpDirPath, string.Format("{0}_{1}_{2}.pdf", start_date.ToString("yyyyMMdd"), item.Key, fileNameCount.ToString().PadLeft(3, '0')));

                            pdf_file_path.DeleteSigleFile();

                            doc.Encrypt("23113343", "23113343", 0, CryptoAlgorithm.AESx128);
                            doc.Save(pdf_file_path);
                            doc.Dispose();

                            streamWriter.WriteLine(content);
                        }
                    }
                    //else this.WriteLog(Log.Mode.LogMode.INFO, string.Format("已存在於準備上傳，將略過 ({0})", pdf_file_path));
                }
                streamWriter.Close();

                this.WriteLastWorkingTime(utility);
            }
            catch (System.Exception ex)
            {
                this.WriteLog(Log.Mode.LogMode.ERROR, ex.ToString());

                this.WriteLog(Mode.LogMode.INFO, string.Format("將於{0}秒後重試", this.BATCH_CaseIDTimer.Interval / 1000));
            }
            finally
            {
                if (utility != null)
                {
                    utility.CloseConn();

                    utility = null;
                }
                dt = null;

                para = null;

                GC.Collect(); GC.WaitForPendingFinalizers();

                Form1.BATCH_CaseIDTimerFlag = false;
            }
        }
        #endregion

        #region StartFTP()
        /// <summary>
        /// 
        /// </summary>
        private void StartFTP()
        {
            Form1.FTPTimerFlag = true;

            Utility utility = null;

            DirectoryInfo dir_info = null;

            FileInfo[] files = null;
            try
            {
                utility = new Utility();

                this.InitLog("FTP");

                if (utility.XDocSetting == null)
                {
                    this.WriteLog(Mode.LogMode.INFO, string.Format("沒有設定檔")); return;
                }
                string ftp_ip = (string)utility.XDocSetting.Root.Element("FTP").Element("FTP_IP");

                int ftp_port = (int)utility.XDocSetting.Root.Element("FTP").Element("FTP_Port");

                string ftp_user = (string)utility.XDocSetting.Root.Element("FTP").Element("FTP_User");

                string ftp_pwd = (string)utility.XDocSetting.Root.Element("FTP").Element("FTP_PWD");

                string ftp_remote_path = (string)utility.XDocSetting.Root.Element("FTP").Element("FTP_Remote_Path");

                if (String.IsNullOrEmpty(ftp_ip)) throw new Exception(string.Format("FTP IP 尚未設定"));

                dir_info = new DirectoryInfo(ConfigPath.FtpDirPath);

                files = dir_info.GetFiles("*.*");

                if (files.Length.Equals(0)) return;

                this.WriteLog(Mode.LogMode.INFO, string.Format("共有{0}個檔案準備上傳", files.Length));

                Array.Sort(files, new FileSorterByCreationTime());

                for (int i = 0; i < files.Length; i++)
                {
                    #region FTP 上傳

                    FTP.FtpWebClient ftp_client = null;
                    try
                    {
                        this.WriteLog(Mode.LogMode.INFO, string.Format("準備 FTP 上傳 {0} ({1})", files[i].FullName, ftp_remote_path));

                        ftp_client = new FTP.FtpWebClient(ftp_ip, ftp_remote_path, ftp_user, ftp_pwd, ftp_port);

                        ftp_client.Files.Upload(files[i].FullName, files[i].Name);

                        this.WriteLog(Mode.LogMode.INFO, string.Format("FTP 上傳完成"));

                        ftp_client = null;

                        files[i].FullName.DeleteSigleFile();

                        this.WriteLog(Mode.LogMode.INFO, string.Format("刪除本地檔案:{0},{1}", files[i].FullName, File.Exists(files[i].FullName) ? "失敗" : "完成"));
                    }
                    catch (System.Exception ex)
                    {
                        this.WriteLog(Log.Mode.LogMode.ERROR, ex.ToString());
                    }
                    finally
                    {
                        ftp_client = null;
                    }
                    #endregion
                }
            }
            catch (System.Exception ex)
            {
                if (dir_info != null) dir_info = null;

                if (files != null) files = null;

                this.WriteLog(Log.Mode.LogMode.ERROR, ex.ToString());
            }
            finally
            {
                if (utility != null)
                {
                    utility.CloseConn();

                    utility = null;
                }

                if (dir_info != null) dir_info = null;

                if (files != null) files = null;

                GC.Collect(); GC.WaitForPendingFinalizers();

                Form1.FTPTimerFlag = false;
            }
        }
        #endregion

        #region StartRealTime()
        /// <summary>
        /// 
        /// </summary>
        private void StartRealTime()
        {
            //todo StartRealTime修正
            /*
            eFilingService.RealTimeTimerFlag = true;

            ILog log = null, log_alive = null, log_error = null;

            Utility u = null;

            string strSql = string.Empty;

            List<IDataParameter> para = null;

            DataTable dt = null;

            Dictionary<string, List<string>> file_list = new Dictionary<string, List<string>>();

            Dictionary<string, List<string>> pdf_empty_list = new Dictionary<string, List<string>>();

            string file_temp_dir_path = Path.Combine(ConfigPath.TempDirPath, Guid.NewGuid().ToString());
            try
            {
                if (!Directory.Exists(file_temp_dir_path)) Directory.CreateDirectory(file_temp_dir_path);

                u = new Utility();

                log = new WindowLog("RealTime");
                log.MaxSize = 4;

                log_error = new WindowLog("RealTime_ERROR");
                log_error.MaxSize = 4;

                log_alive = new WindowLog("RealTime_Alive");
                log_alive.MaxSize = 4;
                log_alive.WriteLog(Mode.LogMode.INFO, string.Format("I'm alive"));

                if (u.XDocSetting == null)
                {
                    log.WriteLog(Mode.LogMode.INFO, string.Format("沒有設定檔")); return;
                }
                bool real_time_switch = u.XDocSetting.GetAttributesValue("./Changingtec/WorkingDate", "Switch", "N").ToUpper().Equals("Y");

                if (!real_time_switch) return;

                List<string> ym_list = new List<string>();

                DateTime start_date = DateTime.Parse(u.XDocSetting.GetInnerText("./Changingtec/WorkingDate/StartDate", string.Empty));

                DateTime end_date = DateTime.Parse(u.XDocSetting.GetInnerText("./Changingtec/WorkingDate/EndDate", string.Empty));

                if (end_date.ToString("yyyyMM").Equals(start_date.ToString("yyyyMM")))
                {
                    ym_list.Add(start_date.ToString("yyyyMM"));
                }
                else
                {
                    DateTime start_date_temp = start_date;

                    DateTime end_date_temp = end_date;

                    while (!end_date_temp.ToString("yyyyMM").Equals(start_date_temp.ToString("yyyyMM")))
                    {
                        log.WriteLog(Mode.LogMode.INFO, string.Format("增加:{0}", end_date.ToString("yyyyMM")));

                        ym_list.Add(end_date_temp.ToString("yyyyMM"));

                        end_date_temp = end_date_temp.AddMonths(-1);
                    }
                }

                for (int i = 0; i < ym_list.Count; i++)
                {
                    string ym = ym_list[i];

                    strSql = u.Select.BATCH_CIF_TABLE(start_date.ToString("yyyy/MM/dd 00:00:00"), end_date.ToString("yyyy/MM/dd 23:59:59"), ym, ref para);

                    #region SQL Debug

                    log.WriteLog(Log.Mode.LogMode.DEBUG, strSql);

                    log.WriteLog(Log.Mode.LogMode.DEBUG, para.ToLog());

                    #endregion

                    dt = u.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                    log.WriteLog(Mode.LogMode.INFO, string.Format("{0}共有{1}筆資料", ym, dt.Rows.Count));

                    if (dt.Rows.Count.Equals(0)) continue;

                    string message = string.Empty;

                    for (int i2 = 0; i2 < dt.Rows.Count; i2++)
                    {
                        string session_key = dt.Rows[i2]["SESSION_KEY"].ToString().Trim();

                        string cif_id = dt.Rows[i2]["CIF_ID"].ToString().Trim();

                        string txn_date = DateTime.Parse(dt.Rows[i2]["TXN_DATE"].ToString().Trim()).ToString("yyyyMMdd");

                        string key = string.Format("{0}|{1}", cif_id, txn_date);

                        bool pdf = dt.Rows[i2]["PDF"].ToString().Trim().ToUpper().Equals("Y");

                        if (!pdf)
                        {
                            if (!pdf_empty_list.ContainsKey(key))
                            {
                                log.WriteLog(Log.Mode.LogMode.INFO, message = string.Format("SESSION_KEY:{0},CIF_ID:{1}::列入空單", session_key, key));

                                pdf_empty_list.Add(key, new List<string>());
                            }
                            continue;
                        }
                        bool check_file = dt.Rows[i]["FILE_ID"] != DBNull.Value && !String.IsNullOrEmpty(dt.Rows[i]["FILE_ID"].ToString());

                        if (!check_file)
                        {
                            log.WriteLog(Log.Mode.LogMode.INFO, message = string.Format("SESSION_KEY:{0},CIF_ID:{1}::不是空單，但沒有檔案", session_key, key));

                            if (!file_list.ContainsKey(key)) file_list.Add(key, new List<string>());
                        }
                        else
                        {
                            string file_id = dt.Rows[i]["FILE_ID"].ToString().Trim();

                            string file_type = dt.Rows[i]["FILE_TYPE"].ToString().Trim();

                            if (!file_type.Equals("11"))
                            {
                                log.WriteLog(Log.Mode.LogMode.INFO, message = string.Format("SESSION_KEY:{0},CIF_ID:{1},FILE_ID:{2}::不是圖檔，將略過該檔案", session_key, key, file_id));

                                continue;
                            }
                            string file_root = dt.Rows[i2]["FILE_ROOT"].ToString().Trim();

                            string file_path = dt.Rows[i2]["FILE_PATH"].ToString().Trim();

                            string file_full_path = file_root.FilePathCombine(file_path);

                            bool exists = File.Exists(file_full_path);

                            if (!exists)
                            {
                                log_error.WriteLog(Log.Mode.LogMode.ERROR, message = string.Format("SESSION_KEY:{0},CIF_ID:{1},FILE_ID:{2}::檔案不存在({3})", session_key, key, file_id, file_full_path));

                                log.WriteLog(Log.Mode.LogMode.ERROR, message);

                                continue;
                            }
                            if (!file_list.ContainsKey(cif_id))
                            {
                                log.WriteLog(Log.Mode.LogMode.INFO, message = string.Format("SESSION_KEY:{0},CIF_ID:{1}::列入正常產生PDF", session_key, key));

                                file_list.Add(cif_id, new List<string>());
                            }
                            string file_temp_path = Path.Combine(file_temp_dir_path, Path.GetFileName(file_full_path));

                            file_temp_path.DeleteSigleFile();

                            if (file_full_path.CopyFile(file_temp_path)) file_list[key].Add(file_temp_path);
                            else log_error.WriteLog(Log.Mode.LogMode.ERROR, message = string.Format("SESSION_KEY:{0},CIF_ID:{1},FILE_ID:{2}::檔案複製失敗({3})", session_key, key, file_id, file_full_path));
                        }
                    }
                    log.WriteLog(Log.Mode.LogMode.INFO, string.Format("共有{0}份準備產生PDF，共有{1}份準備產生PDF空單", file_list.Count, pdf_empty_list.Count));

                    int font_style = 1;

                    string font_path = "c:\\windows\\fonts\\msjh.ttc,0";

                    BaseFont ch_base_font = BaseFont.CreateFont(font_path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                    iTextSharp.text.Font font = new iTextSharp.text.Font(ch_base_font, 12, font_style);

                    #region 正常

                    foreach (string key in file_list.Keys)
                    {
                        List<string> l = file_list[key];

                        string cif_id = key.Split('|')[0];

                        string txn_date = key.Split('|')[1];

                        log.WriteLog(Log.Mode.LogMode.INFO, string.Format("準備產生{0}", cif_id));

                        string pdf_file_path = Path.Combine(ConfigPath.FtpDirPath, string.Format("{0}_BankSlip_{1}.pdf", txn_date, cif_id));

                        string pdf_file_temp_path = Path.Combine(ConfigPath.TempDirPath, string.Format("{0}_BankSlip_{1}.pdf", txn_date, cif_id));
                        try
                        {
                            pdf_file_path.DeleteSigleFile();

                            pdf_file_temp_path.DeleteSigleFile();

                            Document document = new Document(PageSize.A4);

                            using (FileStream fs = new FileStream(pdf_file_temp_path, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                                writer.Open();

                                document.Open();

                                log.WriteLog(Log.Mode.LogMode.INFO, string.Format("準備合併的檔案共有第{0}張", l.Count));

                                for (int i2 = 0; i2 < l.Count; i2++)
                                {
                                    log.WriteLog(Log.Mode.LogMode.INFO, string.Format("準備合併第{0}張", (i2 + 1)));

                                    document.NewPage();

                                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(l[i2]);

                                    image.ScaleToFit(document.PageSize.Width, document.PageSize.Height);

                                    image.SetAbsolutePosition(((document.PageSize.Width - image.ScaledWidth) / 2), ((document.PageSize.Height - image.ScaledHeight) / 2));

                                    document.Add(image);

                                    log.WriteLog(Log.Mode.LogMode.INFO, string.Format("合併第{0}張結束", (i2 + 1)));
                                }
                                document.Close();

                                writer.Close();

                                fs.Close();
                            }
                            pdf_file_path.DeleteSigleFile();

                            this.EncryptPDF(pdf_file_temp_path, pdf_file_path, true, "23113343", "23113343", PdfWriter.ALLOW_SCREENREADERS);

                            pdf_file_temp_path.DeleteSigleFile();
                        }
                        catch (System.Exception ex)
                        {
                            log.WriteLog(Log.Mode.LogMode.ERROR, ex.ToString());

                            log_error.WriteLog(Log.Mode.LogMode.ERROR, ex.ToString());
                        }
                        finally
                        {
                            pdf_file_temp_path.DeleteSigleFile();
                        }
                    }
                    #endregion

                    #region 空單

                    foreach (string key in pdf_empty_list.Keys)
                    {
                        List<string> l = file_list[key];

                        string cif_id = key.Split('|')[0];

                        string txn_date = key.Split('|')[1];

                        log.WriteLog(Log.Mode.LogMode.INFO, string.Format("準備產生{0}空單", cif_id));

                        string pdf_file_path = Path.Combine(ConfigPath.FtpDirPath, string.Format("{0}_BankSlip_{1}.pdf", txn_date, cif_id));

                        string pdf_file_temp_path = Path.Combine(ConfigPath.TempDirPath, string.Format("{0}_BankSlip_{1}.pdf", txn_date, cif_id));
                        try
                        {
                            if (!File.Exists(pdf_file_path))
                            {
                                pdf_file_temp_path.DeleteSigleFile();

                                Document document = new Document(PageSize.A4);

                                using (FileStream fs = new FileStream(pdf_file_temp_path, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    PdfWriter writer = PdfWriter.GetInstance(document, fs);

                                    writer.Open();

                                    document.Open();

                                    document.NewPage();
                                    document.Add(new Paragraph("本日無資料", font));

                                    document.Close();

                                    writer.Close();

                                    fs.Close();
                                }
                                pdf_file_path.DeleteSigleFile();

                                this.EncryptPDF(pdf_file_temp_path, pdf_file_path, true, "23113343", "23113343", PdfWriter.ALLOW_SCREENREADERS);

                                pdf_file_temp_path.DeleteSigleFile();
                            }
                            else log.WriteLog(Log.Mode.LogMode.INFO, string.Format("已存在於準備上傳，將略過 ({0})", pdf_file_path));
                        }
                        catch (System.Exception ex)
                        {
                            log.WriteLog(Log.Mode.LogMode.ERROR, ex.ToString());

                            log_error.WriteLog(Log.Mode.LogMode.ERROR, ex.ToString());
                        }
                        finally
                        {
                            pdf_file_temp_path.DeleteSigleFile();
                        }
                    }
                    #endregion
                }
                this.WriteWorkingDateSwitch(u, log);
            }
            catch (System.Exception ex)
            {
                if (log != null) log.WriteLog(Log.Mode.LogMode.ERROR, ex.ToString());

                if (log_error != null) log_error.WriteLog(Log.Mode.LogMode.ERROR, ex.ToString());

                if (log == null && log_error == null) eFilingService.SysLog.WriteLog(Log.Mode.LogMode.ERROR, ex.ToString());
            }
            finally
            {
                if (u != null)
                {
                    u.CloseConn();

                    u = null;
                }
                if (log != null) log = null;

                if (log_alive != null) log_alive = null;

                if (log_error != null) log_error = null;

                dt = null;

                para = null;

                file_temp_dir_path.DeleteDirectory();

                GC.Collect(); GC.WaitForPendingFinalizers();

                eFilingService.RealTimeTimerFlag = false;
            }

            */
        }
        #endregion

        internal IEnumerable<FileMaker> GetFileMaker(DateTime Start_Date)
        {
            string strSql = string.Empty;

            List<IDataParameter> para = null;

            DataTable dt = null;

            Utility utility = null;

            try
            {
                utility = new Utility();

                strSql = utility.Select.BATCH_CIF_INFO(Start_Date.ToString("yyyy/MM/dd 00:00:00"), Start_Date.ToString("yyyy/MM/dd 23:59:59"), Start_Date.ToString("yyyyMM"), ref para);

                #region SQL Debug

                this.WriteLog(Log.Mode.LogMode.DEBUG, strSql);

                this.WriteLog(Log.Mode.LogMode.DEBUG, para.ToLog());

                #endregion

                dt = utility.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, para);

                this.WriteLog(Mode.LogMode.INFO, string.Format("共有{0}筆資料", dt.Rows.Count));

                if (dt.Rows.Count.Equals(0))
                {
                    this.WriteLastWorkingTime(utility); yield break;
                }
                string message = string.Empty;

                var query = from batch in dt.AsEnumerable()
                            select new
                            {
                                CREATE_DATETIME = batch.Field<DateTime>("CREATE_DATETIME"),
                                SESSION_KE = batch.Field<string>("SESSION_KE"),
                                TXN_DATE = batch.Field<DateTime>("TXN_DATE"),
                                TXN_ACCOUNT = batch.Field<string>("TXN_ACCOUNT"),
                                BRANCH_ID = batch.Field<string>("BRANCH_ID"),
                                FILE_ID = batch.Field<string>("FILE_ID"),
                                FILE_CREATE_DATETIME = batch.Field<DateTime>("CREATE_DATETIME"),
                                FILE_SEQ = batch.Field<Int16>("FILE_SEQ"),
                                FILE_ROOT = batch.Field<string>("FILE_ROOT"),
                                FILE_PATH = batch.Field<string>("FILE_PATH"),
                                PDF = batch.Field<string>("PDF"),
                                FILE_TYPE = batch.Field<Int16>("FILE_TYPE"),
                                T_MNEMONIC = batch.Field<string>("T_MNEMONIC"),
                                T_EMAIL_SIGN_1 = batch.Field<string>("T_EMAIL_SIGN_1"),
                                T_EMAIL_1 = batch.Field<string>("T_EMAIL_1")
                            };



                foreach (var batch in query)
                {
                    FileMaker fileMaker = new FileMaker()
                    {
                        CREATE_DATETIME = batch.CREATE_DATETIME,
                        SESSION_KE = batch.SESSION_KE,
                        TXN_DATE = batch.TXN_DATE,
                        TXN_ACCOUNT = batch.TXN_ACCOUNT,
                        BRANCH_ID = batch.BRANCH_ID,
                        FILE_ID = batch.FILE_ID,
                        FILE_CREATE_DATETIME = batch.CREATE_DATETIME,
                        FILE_SEQ = batch.FILE_SEQ,
                        FILE_ROOT = batch.FILE_ROOT,
                        FILE_PATH = batch.FILE_PATH,
                        PDF = batch.PDF,
                        FILE_TYPE = batch.FILE_TYPE,
                        T_MNEMONIC = batch.T_MNEMONIC,
                        T_EMAIL_SIGN_1 = batch.T_EMAIL_SIGN_1,
                        T_EMAIL_1 = batch.T_EMAIL_1
                    };

                    yield return fileMaker;
                }
            }
            //catch (Exception ex)
            //{
            //    throw new Exception(string.Format("GetFileMakerError:{0}", ex.Message));
            //}
            finally
            {
                if (utility != null)
                {
                    utility.CloseConn();

                    utility = null;
                }

                dt = null;

                para = null;

                GC.Collect(); GC.WaitForPendingFinalizers();
            }
        }
        #endregion
    }
}
