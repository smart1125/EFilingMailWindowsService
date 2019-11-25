using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Beautify;
using Log;
using System.Globalization;
using System.Xml.Linq;

namespace eFilingMailServiceSetting
{
    public partial class Form1 : SkinForm
    {
        #region Log
        /// <summary>
        /// 
        /// </summary>
        private ILog _Log = null;
        /// <summary>
        /// 
        /// </summary>
        public ILog Log
        {
            get
            {
                if (this._Log == null)
                {
                    this._Log = new WindowLog("Setting");
                    this._Log.MaxSize = 2;
                    try
                    {
                        DateTime now_date_time = DateTime.Now;
                        string check_clear_path = Path.Combine(this._Log.DirectoryPath, "clear.flag");
                        if (!File.Exists(check_clear_path)) File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));
                        DateTime date_time = DateTime.Parse(File.ReadAllText(check_clear_path).Trim(), CultureInfo.InvariantCulture);
                        bool clear = DateTime.Compare(DateTime.Now, date_time.AddDays(14)) > 0;

                        if (clear)
                        {
                            this._Log.Delete(14);

                            this._Log.DeleteBackup(14);

                            File.WriteAllText(check_clear_path, now_date_time.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));

                            this._Log.WriteLog(Mode.LogMode.INFO, string.Format("ClearLog::{0}", now_date_time.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                    }
                    catch (System.Exception ex) { this._Log.WriteLog(Mode.LogMode.ERROR, string.Format("ClearLog.Exception::{0}", ex.ToString())); }
                }
                return this._Log;
            }
            set { this._Log = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public void WriteLog(string Message)
        {
            this.Log.WriteLog(Mode.LogMode.INFO, Message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="Message"></param>
        public void WriteLog(Mode.LogMode Mode, string Message)
        {
            this.Log.WriteLog(Mode, Message);
        }
        #endregion

        #region XmlDocSetting
        /// <summary>
        /// 
        /// </summary>
        public XDocument _XDocSetting = null;
        /// <summary>
        /// 
        /// </summary>
        public XDocument XDocSetting
        {
            get
            {
                if (this._XDocSetting == null)
                {
                    this._XDocSetting = XDocument.Load(ConfigPath.SettingPath);
                }
                return this._XDocSetting;
            }
            set { this._XDocSetting = value; }
        }
        #endregion

        #region INFO()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public DialogResult INFO(string Message)
        {
            return MessageBox.Show(Message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region ERROR()
        /// <summary>
        /// Show error message
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="WLog"></param>
        public void ERROR(string Message, bool WLog)
        {
            if (WLog)
            {
                WriteLog(Mode.LogMode.ERROR, Message);
            }
            MessageBox.Show(Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region Confirm()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public DialogResult Confirm(string Message, MessageBoxButtons Buttons)
        {
            return MessageBox.Show(Message, "Info", Buttons, MessageBoxIcon.Information);
        }
        #endregion

        #region WatchRealTimeTimer
        /// <summary>
        /// 
        /// </summary>
        private static bool WatchRealTimeFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private System.Timers.Timer _WatchRealTimeTimer = null;
        /// <summary>
        /// 
        /// </summary>
        private System.Timers.Timer WatchRealTimeTimer
        {
            get
            {
                if (_WatchRealTimeTimer == null) _WatchRealTimeTimer = new System.Timers.Timer();

                return _WatchRealTimeTimer;
            }
            set { _WatchRealTimeTimer = value; }
        }
        #endregion

        public Form1()
        {
            InitializeComponent();

            try
            {
                for (int i = 0; i <= 23; i++) this.comboBoxWorkingTimerH.Items.Add(new ComBox.ComboBoxItem() { Text = i.ToString().PadLeft(2, '0'), Value = i.ToString().PadLeft(2, '0') });

                for (int i = 0; i <= 59; i++) this.comboBoxWorkingTimerM.Items.Add(new ComBox.ComboBoxItem() { Text = i.ToString().PadLeft(2, '0'), Value = i.ToString().PadLeft(2, '0') });

                this.dateTimeWorkStart.MaxDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                this.dateTimeWorkStart.MinDate = DateTime.Parse(DateTime.Now.AddYears(-10).ToString("yyyy/MM/dd HH:mm:ss"));
                this.dateTimeWorkStart.Value = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd HH:mm:ss"));
                this.dateTimeWorkEnd.MaxDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                this.dateTimeWorkEnd.MinDate = DateTime.Parse(DateTime.Now.AddYears(-10).ToString("yyyy/MM/dd HH:mm:ss"));
                this.dateTimeWorkEnd.Value = this.dateTimeWorkEnd.MaxDate;

                this.txtConnectionString.Text = (string)this.XDocSetting.Root.Element("ConnectionString");

                this.txtFTP_IP.Text = (string)this.XDocSetting.Root.Element("FTP").Element("FTP_IP");
                this.txtFTP_Port.Text = (string)this.XDocSetting.Root.Element("FTP").Element("FTP_Port");
                this.txtFTP_User.Text = (string)this.XDocSetting.Root.Element("FTP").Element("FTP_User");
                this.txtFTP_PWD.Text = (string)this.XDocSetting.Root.Element("FTP").Element("FTP_PWD");
                this.txtFTP_Remote_Path.Text = (string)this.XDocSetting.Root.Element("FTP").Element("FTP_Remote_Path");

                this.chBoxBatchWorkingTime.Checked = ((string)this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Batch").FirstOrDefault().Element("switch")).ToUpper().Equals("Y");

                this.lblLastWorkingTimeValue.Text = (string)this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Batch").FirstOrDefault().Element("lastWorkingTime");

                string startTime = (string)this.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "Batch").FirstOrDefault().Element("start");

                this.comboBoxWorkingTimerH.SelectedComboBoxItemByValue(startTime == null ? "00" : startTime.Substring(0, 2));
                this.comboBoxWorkingTimerM.SelectedComboBoxItemByValue(startTime == null ? "00" : startTime.Substring(2, 2));

                this.lblWorkingDateSwitch.Text = string.Format("上一次執行範圍");

                this.lblWorkingDateSwitchValue.Text = string.Format("{0}~{1}", (string)this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Real").FirstOrDefault().Element("startDate"), (string)this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Real").FirstOrDefault().Element("endDate"));

                this.lblRealTimeStatusValue.Text = string.Empty;

                this.txtBatchTimerInterval.Text = (string)this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Batch").FirstOrDefault().Element("interval");

                this.txtRealTimeTimerInterval.Text = (string)this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Real").FirstOrDefault().Element("interval");

                this.txtFTPTimerInterval.Text = (string)this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "FTP").FirstOrDefault().Element("interval");
            }
            catch (System.Exception ex)
            {
                this.ERROR(ex.ToString(), true);
            } 
        }

        #region CheckInput()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            if (String.IsNullOrEmpty(this.txtConnectionString.Text))
            {
                this.INFO(string.Format("{0}未填", this.lblConnectionString.Text)); return false;
            }
            if (String.IsNullOrEmpty(this.txtFTP_IP.Text))
            {
                this.INFO(string.Format("{0}未填", this.lblFTP_IP.Text)); return false;
            }
            if (String.IsNullOrEmpty(this.txtFTP_Remote_Path.Text))
            {
                this.INFO(string.Format("{0}未填", this.lblFTP_Remote_Path.Text)); return false;
            }
            int interval = -1;

            if (String.IsNullOrEmpty(this.txtBatchTimerInterval.Text))
            {
                this.INFO(string.Format("{0} Timer interval 未填", this.lblBatchTimerInterval.Text)); return false;
            }
            else if (!int.TryParse(this.txtBatchTimerInterval.Text, out interval))
            {
                this.INFO(string.Format("{0} Timer interval 錯誤", this.lblBatchTimerInterval.Text)); return false;
            }
            else if (Convert.ToInt32(this.txtBatchTimerInterval.Text) < 10)
            {
                this.INFO(string.Format("{0} Timer interval 不符規定", this.lblBatchTimerInterval.Text)); return false;
            }
            if (String.IsNullOrEmpty(this.txtRealTimeTimerInterval.Text))
            {
                this.INFO(string.Format("{0} Timer interval 未填", this.lblRealTimeTimerInterval.Text)); return false;
            }
            else if (!int.TryParse(this.txtRealTimeTimerInterval.Text, out interval))
            {
                this.INFO(string.Format("{0} Timer interval 錯誤", this.lblRealTimeTimerInterval.Text)); return false;
            }
            else if (Convert.ToInt32(this.txtRealTimeTimerInterval.Text) < 10)
            {
                this.INFO(string.Format("{0} Timer interval 不符規定", this.lblRealTimeTimerInterval.Text)); return false;
            }
            if (String.IsNullOrEmpty(this.txtFTPTimerInterval.Text))
            {
                this.INFO(string.Format("{0} Timer interval 未填", this.lblFTPTimerInterval.Text)); return false;
            }
            else if (!int.TryParse(this.txtFTPTimerInterval.Text, out interval))
            {
                this.INFO(string.Format("{0} Timer interval 錯誤", this.lblFTPTimerInterval.Text)); return false;
            }
            else if (Convert.ToInt32(this.txtFTPTimerInterval.Text) < 10)
            {
                this.INFO(string.Format("{0} Timer interval 不符規定", this.lblFTPTimerInterval.Text)); return false;
            }
            return true;
        }
        #endregion

        #region EndTimer()
        /// <summary>
        /// 
        /// </summary>
        private void EndTimer()
        {
            this.WatchRealTimeTimer.Stop();
            this.WatchRealTimeTimer.Close();
            this.WatchRealTimeTimer.Dispose();
            this.WatchRealTimeTimer = null;

            Form1.WatchRealTimeFlag = false;
        }
        #endregion

        #region btnStartWorking_Click()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartWorking_Click(object sender, EventArgs e)
        {
            this.WriteLog(string.Format("準備開始執行即時作業設定"));
            try
            {
                if (!this.CheckInput() || this.Confirm(string.Format("是否確定開始即時作業？"), MessageBoxButtons.YesNo) == DialogResult.No) return;

                DateTime start_date = this.dateTimeWorkStart.Value.CompareTo(this.dateTimeWorkEnd.Value) > 0 ? this.dateTimeWorkEnd.Value : this.dateTimeWorkStart.Value;

                DateTime end_date = this.dateTimeWorkEnd.Value.CompareTo(this.dateTimeWorkStart.Value) < 0 ? this.dateTimeWorkStart.Value : this.dateTimeWorkEnd.Value;

                this.WriteLog(string.Format("start_date:{0},end_date:{0}", start_date.ToString("yyyy/MM/dd"), end_date.ToString("yyyy/MM/dd")));

                int days = new TimeSpan(DateTime.Parse(end_date.ToString("yyyy/MM/dd 00:00:00")).Ticks - DateTime.Parse(start_date.ToString("yyyy/MM/dd 00:00:00")).Ticks).Days;

                this.WriteLog(string.Format("Ticks.days:{0}", days));

                if (days > 7)
                {
                    this.ERROR(string.Format("時間範圍勿大於7天，目前設定範圍是{0}天", days), true); return;
                }
                this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Real").FirstOrDefault().SetElementValue("startDate", start_date.ToString("yyyy/MM/dd 00:00:00"));
                this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Real").FirstOrDefault().SetElementValue("endDate", end_date.ToString("yyyy/MM/dd 23:59:59"));
                this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Real").FirstOrDefault().SetElementValue("switch", "Y");

                this.XDocSetting.Save(ConfigPath.SettingPath);

                this.lblWorkingDateSwitch.Text = string.Format("本次執行範圍");

                this.lblWorkingDateSwitchValue.Text = string.Format("{0}~{1}", (string)this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Real").FirstOrDefault().Element("startDate"), (string)this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Real").FirstOrDefault().Element("endDate"));

                this.lblRealTimeStatusValue.Text = "排程中";

                this.lblRealTimeStatusValue.ForeColor = Color.Green;

                this.WatchRealTimeTimer.Interval = 1000 * 10;
                this.WatchRealTimeTimer.Elapsed += delegate(Object senderWT, System.Timers.ElapsedEventArgs eWT)
                {
                    if (!Form1.WatchRealTimeFlag)
                    {
                        Form1.WatchRealTimeFlag = true;

                        bool working_date_switch = ((string)this.XDocSetting.Root.Descendants("Timer").Where(o => (string)o.Element("name") == "Real").FirstOrDefault().Element("switch")).ToUpper().Equals("Y");

                        this.lblRealTimeStatusValue.Text = working_date_switch ? "執行中" : "執行完畢";

                        this.lblRealTimeStatusValue.ForeColor = working_date_switch ? Color.Blue : Color.Red;

                        if (!working_date_switch) this.EndTimer();
                    }
                };
                this.WatchRealTimeTimer.Start();

                this.INFO(string.Format("設定完成"));
            }
            catch (System.Exception ex)
            {
                this.ERROR(ex.ToString(), true);
            }
            finally
            {
                this.WriteLog(string.Format("執行即時作業設定結束"));
            }
        }
        #endregion

        #region btnSave_Click()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.WriteLog(string.Format("準備開始修改設定"));
            try
            {
                if (!this.CheckInput()) return;

                this.WriteLog(string.Format("原設定::\r\n{0}", this.XDocSetting.ToString()));

                this.XDocSetting.Root.SetElementValue("ConnectionString", this.txtConnectionString.Text.Trim());

                this.XDocSetting.Root.Element("FTP").SetElementValue("FTP_IP", this.txtFTP_IP.Text);
                this.XDocSetting.Root.Element("FTP").SetElementValue("FTP_Port", String.IsNullOrEmpty(this.txtFTP_Port.Text) ? "21" : this.txtFTP_Port.Text.Trim());
                this.XDocSetting.Root.Element("FTP").SetElementValue("FTP_User", this.txtFTP_User.Text.Trim());
                this.XDocSetting.Root.Element("FTP").SetElementValue("FTP_PWD", this.txtFTP_PWD.Text.Trim());
                this.XDocSetting.Root.Element("FTP").SetElementValue("FTP_Remote_Path", this.txtFTP_Remote_Path.Text.Trim());

                this.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "Batch").FirstOrDefault().SetElementValue("switch", this.chBoxBatchWorkingTime.Checked ? "Y" : "N");
                this.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "Batch").FirstOrDefault().SetElementValue("start", ((ComBox.ComboBoxItem)this.comboBoxWorkingTimerH.SelectedItem).Value.ToString() + ((ComBox.ComboBoxItem)this.comboBoxWorkingTimerM.SelectedItem).Value.ToString());


                this.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "Batch").FirstOrDefault().SetElementValue("interval", this.txtBatchTimerInterval.Text.Trim());
                this.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "Real").FirstOrDefault().SetElementValue("interval", this.txtRealTimeTimerInterval.Text.Trim());
                this.XDocSetting.Root.Descendants("Timer").Where(x => (string)x.Element("name") == "FTP").FirstOrDefault().SetElementValue("interval", this.txtFTPTimerInterval.Text.Trim());
                
                this.XDocSetting.Save(ConfigPath.SettingPath);

                this.INFO(string.Format("設定完成"));
            }
            catch (System.Exception ex)
            {
                this.ERROR(ex.ToString(), true);
            }
            finally
            {
                this.WriteLog(string.Format("修改設定結束"));
            }
        }
        #endregion
    }
}
