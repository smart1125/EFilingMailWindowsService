namespace eFilingMailServiceSetting
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new Beautify.SkinTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblRealTimeStatusValue = new System.Windows.Forms.Label();
            this.lblRealTimeStatus = new System.Windows.Forms.Label();
            this.lblWorkingDateSwitchValue = new System.Windows.Forms.Label();
            this.lblWorkingDateSwitch = new System.Windows.Forms.Label();
            this.btnStartWorking = new Beautify.SkinButton();
            this.lblWorkEnd = new System.Windows.Forms.Label();
            this.lblWorkStart = new System.Windows.Forms.Label();
            this.dateTimeWorkEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimeWorkStart = new System.Windows.Forms.DateTimePicker();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnSave = new Beautify.SkinButton();
            this.groupBoxBatchWorkingTime = new System.Windows.Forms.GroupBox();
            this.chBoxBatchWorkingTime = new System.Windows.Forms.CheckBox();
            this.comboBoxWorkingTimerM = new System.Windows.Forms.ComboBox();
            this.comboBoxWorkingTimerH = new System.Windows.Forms.ComboBox();
            this.lblWorkingTimer = new System.Windows.Forms.Label();
            this.lblLastWorkingTimeValue = new System.Windows.Forms.Label();
            this.lblLastWorkingTime = new System.Windows.Forms.Label();
            this.groupBoxFTP = new System.Windows.Forms.GroupBox();
            this.txtFTP_Remote_Path = new System.Windows.Forms.TextBox();
            this.lblFTP_Remote_Path = new System.Windows.Forms.Label();
            this.txtFTP_User = new System.Windows.Forms.TextBox();
            this.lblFTP_User = new System.Windows.Forms.Label();
            this.txtFTP_PWD = new System.Windows.Forms.TextBox();
            this.lblFTP_PWD = new System.Windows.Forms.Label();
            this.txtFTP_Port = new System.Windows.Forms.TextBox();
            this.lblFTP_Port = new System.Windows.Forms.Label();
            this.txtFTP_IP = new System.Windows.Forms.TextBox();
            this.lblFTP_IP = new System.Windows.Forms.Label();
            this.groupBoxConnect = new System.Windows.Forms.GroupBox();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBoxTimerInterval = new System.Windows.Forms.GroupBox();
            this.txtFTPTimerInterval = new System.Windows.Forms.TextBox();
            this.lblFTPTimerInterval = new System.Windows.Forms.Label();
            this.txtRealTimeTimerInterval = new System.Windows.Forms.TextBox();
            this.lblRealTimeTimerInterval = new System.Windows.Forms.Label();
            this.txtBatchTimerInterval = new System.Windows.Forms.TextBox();
            this.lblBatchTimerInterval = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBoxBatchWorkingTime.SuspendLayout();
            this.groupBoxFTP.SuspendLayout();
            this.groupBoxConnect.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBoxTimerInterval.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(3, 24);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(703, 769);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblRealTimeStatusValue);
            this.tabPage1.Controls.Add(this.lblRealTimeStatus);
            this.tabPage1.Controls.Add(this.lblWorkingDateSwitchValue);
            this.tabPage1.Controls.Add(this.lblWorkingDateSwitch);
            this.tabPage1.Controls.Add(this.btnStartWorking);
            this.tabPage1.Controls.Add(this.lblWorkEnd);
            this.tabPage1.Controls.Add(this.lblWorkStart);
            this.tabPage1.Controls.Add(this.dateTimeWorkEnd);
            this.tabPage1.Controls.Add(this.dateTimeWorkStart);
            this.tabPage1.Location = new System.Drawing.Point(4, 30);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(695, 735);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "即時作業";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblRealTimeStatusValue
            // 
            this.lblRealTimeStatusValue.AutoSize = true;
            this.lblRealTimeStatusValue.BackColor = System.Drawing.Color.Transparent;
            this.lblRealTimeStatusValue.ForeColor = System.Drawing.Color.Blue;
            this.lblRealTimeStatusValue.Location = new System.Drawing.Point(212, 263);
            this.lblRealTimeStatusValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRealTimeStatusValue.Name = "lblRealTimeStatusValue";
            this.lblRealTimeStatusValue.Size = new System.Drawing.Size(132, 25);
            this.lblRealTimeStatusValue.TabIndex = 9;
            this.lblRealTimeStatusValue.Text = "本次執行狀態";
            // 
            // lblRealTimeStatus
            // 
            this.lblRealTimeStatus.AutoSize = true;
            this.lblRealTimeStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblRealTimeStatus.Location = new System.Drawing.Point(15, 263);
            this.lblRealTimeStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRealTimeStatus.Name = "lblRealTimeStatus";
            this.lblRealTimeStatus.Size = new System.Drawing.Size(132, 25);
            this.lblRealTimeStatus.TabIndex = 8;
            this.lblRealTimeStatus.Text = "本次執行狀態";
            // 
            // lblWorkingDateSwitchValue
            // 
            this.lblWorkingDateSwitchValue.AutoSize = true;
            this.lblWorkingDateSwitchValue.BackColor = System.Drawing.Color.Transparent;
            this.lblWorkingDateSwitchValue.ForeColor = System.Drawing.Color.Blue;
            this.lblWorkingDateSwitchValue.Location = new System.Drawing.Point(212, 202);
            this.lblWorkingDateSwitchValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkingDateSwitchValue.Name = "lblWorkingDateSwitchValue";
            this.lblWorkingDateSwitchValue.Size = new System.Drawing.Size(152, 25);
            this.lblWorkingDateSwitchValue.TabIndex = 7;
            this.lblWorkingDateSwitchValue.Text = "上一次執行時間";
            // 
            // lblWorkingDateSwitch
            // 
            this.lblWorkingDateSwitch.AutoSize = true;
            this.lblWorkingDateSwitch.BackColor = System.Drawing.Color.Transparent;
            this.lblWorkingDateSwitch.Location = new System.Drawing.Point(15, 202);
            this.lblWorkingDateSwitch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkingDateSwitch.Name = "lblWorkingDateSwitch";
            this.lblWorkingDateSwitch.Size = new System.Drawing.Size(192, 25);
            this.lblWorkingDateSwitch.TabIndex = 6;
            this.lblWorkingDateSwitch.Text = "已列入排程等待執行";
            // 
            // btnStartWorking
            // 
            this.btnStartWorking.DisableBaseColor = System.Drawing.SystemColors.ControlDark;
            this.btnStartWorking.DisableBorderColor = System.Drawing.SystemColors.ControlDark;
            this.btnStartWorking.DisableDropDownIconColor = System.Drawing.SystemColors.ControlDark;
            this.btnStartWorking.DisableFontColor = System.Drawing.SystemColors.ControlDark;
            this.btnStartWorking.ImageWidth = 18;
            this.btnStartWorking.Location = new System.Drawing.Point(291, 593);
            this.btnStartWorking.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStartWorking.Name = "btnStartWorking";
            this.btnStartWorking.RStyle = Beautify.SkinButton.RoundStyle.None;
            this.btnStartWorking.Size = new System.Drawing.Size(118, 60);
            this.btnStartWorking.TabIndex = 5;
            this.btnStartWorking.Text = "開始執行";
            this.btnStartWorking.UseVisualStyleBackColor = true;
            this.btnStartWorking.Click += new System.EventHandler(this.btnStartWorking_Click);
            // 
            // lblWorkEnd
            // 
            this.lblWorkEnd.AutoSize = true;
            this.lblWorkEnd.BackColor = System.Drawing.Color.Transparent;
            this.lblWorkEnd.Location = new System.Drawing.Point(164, 112);
            this.lblWorkEnd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkEnd.Name = "lblWorkEnd";
            this.lblWorkEnd.Size = new System.Drawing.Size(137, 25);
            this.lblWorkEnd.TabIndex = 4;
            this.lblWorkEnd.Text = "TXN_DATE 迄";
            // 
            // lblWorkStart
            // 
            this.lblWorkStart.AutoSize = true;
            this.lblWorkStart.BackColor = System.Drawing.Color.Transparent;
            this.lblWorkStart.Location = new System.Drawing.Point(164, 38);
            this.lblWorkStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkStart.Name = "lblWorkStart";
            this.lblWorkStart.Size = new System.Drawing.Size(137, 25);
            this.lblWorkStart.TabIndex = 3;
            this.lblWorkStart.Text = "TXN_DATE 起";
            // 
            // dateTimeWorkEnd
            // 
            this.dateTimeWorkEnd.Location = new System.Drawing.Point(300, 107);
            this.dateTimeWorkEnd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTimeWorkEnd.Name = "dateTimeWorkEnd";
            this.dateTimeWorkEnd.Size = new System.Drawing.Size(235, 33);
            this.dateTimeWorkEnd.TabIndex = 2;
            // 
            // dateTimeWorkStart
            // 
            this.dateTimeWorkStart.Location = new System.Drawing.Point(300, 32);
            this.dateTimeWorkStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTimeWorkStart.Name = "dateTimeWorkStart";
            this.dateTimeWorkStart.Size = new System.Drawing.Size(235, 33);
            this.dateTimeWorkStart.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnSave);
            this.tabPage2.Controls.Add(this.groupBoxBatchWorkingTime);
            this.tabPage2.Controls.Add(this.groupBoxFTP);
            this.tabPage2.Controls.Add(this.groupBoxConnect);
            this.tabPage2.Location = new System.Drawing.Point(4, 30);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(695, 735);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "批次作業";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.DisableBaseColor = System.Drawing.SystemColors.ControlDark;
            this.btnSave.DisableBorderColor = System.Drawing.SystemColors.ControlDark;
            this.btnSave.DisableDropDownIconColor = System.Drawing.SystemColors.ControlDark;
            this.btnSave.DisableFontColor = System.Drawing.SystemColors.ControlDark;
            this.btnSave.ImageWidth = 18;
            this.btnSave.Location = new System.Drawing.Point(291, 668);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.RStyle = Beautify.SkinButton.RoundStyle.None;
            this.btnSave.Size = new System.Drawing.Size(118, 60);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "儲存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBoxBatchWorkingTime
            // 
            this.groupBoxBatchWorkingTime.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxBatchWorkingTime.Controls.Add(this.chBoxBatchWorkingTime);
            this.groupBoxBatchWorkingTime.Controls.Add(this.comboBoxWorkingTimerM);
            this.groupBoxBatchWorkingTime.Controls.Add(this.comboBoxWorkingTimerH);
            this.groupBoxBatchWorkingTime.Controls.Add(this.lblWorkingTimer);
            this.groupBoxBatchWorkingTime.Controls.Add(this.lblLastWorkingTimeValue);
            this.groupBoxBatchWorkingTime.Controls.Add(this.lblLastWorkingTime);
            this.groupBoxBatchWorkingTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBatchWorkingTime.Location = new System.Drawing.Point(4, 351);
            this.groupBoxBatchWorkingTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxBatchWorkingTime.Name = "groupBoxBatchWorkingTime";
            this.groupBoxBatchWorkingTime.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxBatchWorkingTime.Size = new System.Drawing.Size(687, 168);
            this.groupBoxBatchWorkingTime.TabIndex = 2;
            this.groupBoxBatchWorkingTime.TabStop = false;
            this.groupBoxBatchWorkingTime.Text = "批次執行時間";
            // 
            // chBoxBatchWorkingTime
            // 
            this.chBoxBatchWorkingTime.AutoSize = true;
            this.chBoxBatchWorkingTime.Location = new System.Drawing.Point(466, 112);
            this.chBoxBatchWorkingTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chBoxBatchWorkingTime.Name = "chBoxBatchWorkingTime";
            this.chBoxBatchWorkingTime.Size = new System.Drawing.Size(218, 29);
            this.chBoxBatchWorkingTime.TabIndex = 5;
            this.chBoxBatchWorkingTime.Text = "今日是否已執行完畢";
            this.chBoxBatchWorkingTime.UseVisualStyleBackColor = true;
            // 
            // comboBoxWorkingTimerM
            // 
            this.comboBoxWorkingTimerM.FormattingEnabled = true;
            this.comboBoxWorkingTimerM.Location = new System.Drawing.Point(304, 105);
            this.comboBoxWorkingTimerM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxWorkingTimerM.Name = "comboBoxWorkingTimerM";
            this.comboBoxWorkingTimerM.Size = new System.Drawing.Size(86, 33);
            this.comboBoxWorkingTimerM.TabIndex = 4;
            // 
            // comboBoxWorkingTimerH
            // 
            this.comboBoxWorkingTimerH.FormattingEnabled = true;
            this.comboBoxWorkingTimerH.Location = new System.Drawing.Point(210, 105);
            this.comboBoxWorkingTimerH.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxWorkingTimerH.Name = "comboBoxWorkingTimerH";
            this.comboBoxWorkingTimerH.Size = new System.Drawing.Size(86, 33);
            this.comboBoxWorkingTimerH.TabIndex = 3;
            // 
            // lblWorkingTimer
            // 
            this.lblWorkingTimer.AutoSize = true;
            this.lblWorkingTimer.BackColor = System.Drawing.Color.Transparent;
            this.lblWorkingTimer.Location = new System.Drawing.Point(9, 110);
            this.lblWorkingTimer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkingTimer.Name = "lblWorkingTimer";
            this.lblWorkingTimer.Size = new System.Drawing.Size(172, 25);
            this.lblWorkingTimer.TabIndex = 2;
            this.lblWorkingTimer.Text = "每日固定執行時間";
            // 
            // lblLastWorkingTimeValue
            // 
            this.lblLastWorkingTimeValue.AutoSize = true;
            this.lblLastWorkingTimeValue.BackColor = System.Drawing.Color.Transparent;
            this.lblLastWorkingTimeValue.Location = new System.Drawing.Point(206, 53);
            this.lblLastWorkingTimeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLastWorkingTimeValue.Name = "lblLastWorkingTimeValue";
            this.lblLastWorkingTimeValue.Size = new System.Drawing.Size(192, 25);
            this.lblLastWorkingTimeValue.TabIndex = 1;
            this.lblLastWorkingTimeValue.Text = "上一次執行成功時間";
            // 
            // lblLastWorkingTime
            // 
            this.lblLastWorkingTime.AutoSize = true;
            this.lblLastWorkingTime.BackColor = System.Drawing.Color.Transparent;
            this.lblLastWorkingTime.Location = new System.Drawing.Point(9, 53);
            this.lblLastWorkingTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLastWorkingTime.Name = "lblLastWorkingTime";
            this.lblLastWorkingTime.Size = new System.Drawing.Size(192, 25);
            this.lblLastWorkingTime.TabIndex = 0;
            this.lblLastWorkingTime.Text = "上一次執行成功時間";
            // 
            // groupBoxFTP
            // 
            this.groupBoxFTP.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxFTP.Controls.Add(this.txtFTP_Remote_Path);
            this.groupBoxFTP.Controls.Add(this.lblFTP_Remote_Path);
            this.groupBoxFTP.Controls.Add(this.txtFTP_User);
            this.groupBoxFTP.Controls.Add(this.lblFTP_User);
            this.groupBoxFTP.Controls.Add(this.txtFTP_PWD);
            this.groupBoxFTP.Controls.Add(this.lblFTP_PWD);
            this.groupBoxFTP.Controls.Add(this.txtFTP_Port);
            this.groupBoxFTP.Controls.Add(this.lblFTP_Port);
            this.groupBoxFTP.Controls.Add(this.txtFTP_IP);
            this.groupBoxFTP.Controls.Add(this.lblFTP_IP);
            this.groupBoxFTP.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxFTP.Location = new System.Drawing.Point(4, 128);
            this.groupBoxFTP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxFTP.Name = "groupBoxFTP";
            this.groupBoxFTP.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxFTP.Size = new System.Drawing.Size(687, 223);
            this.groupBoxFTP.TabIndex = 1;
            this.groupBoxFTP.TabStop = false;
            this.groupBoxFTP.Text = "FTP";
            // 
            // txtFTP_Remote_Path
            // 
            this.txtFTP_Remote_Path.Location = new System.Drawing.Point(147, 158);
            this.txtFTP_Remote_Path.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFTP_Remote_Path.Name = "txtFTP_Remote_Path";
            this.txtFTP_Remote_Path.Size = new System.Drawing.Size(523, 33);
            this.txtFTP_Remote_Path.TabIndex = 8;
            // 
            // lblFTP_Remote_Path
            // 
            this.lblFTP_Remote_Path.AutoSize = true;
            this.lblFTP_Remote_Path.BackColor = System.Drawing.Color.Transparent;
            this.lblFTP_Remote_Path.Location = new System.Drawing.Point(9, 165);
            this.lblFTP_Remote_Path.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFTP_Remote_Path.Name = "lblFTP_Remote_Path";
            this.lblFTP_Remote_Path.Size = new System.Drawing.Size(133, 25);
            this.lblFTP_Remote_Path.TabIndex = 7;
            this.lblFTP_Remote_Path.Text = "Remote Path";
            // 
            // txtFTP_User
            // 
            this.txtFTP_User.Location = new System.Drawing.Point(148, 103);
            this.txtFTP_User.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFTP_User.Name = "txtFTP_User";
            this.txtFTP_User.Size = new System.Drawing.Size(190, 33);
            this.txtFTP_User.TabIndex = 5;
            // 
            // lblFTP_User
            // 
            this.lblFTP_User.AutoSize = true;
            this.lblFTP_User.BackColor = System.Drawing.Color.Transparent;
            this.lblFTP_User.Location = new System.Drawing.Point(9, 108);
            this.lblFTP_User.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFTP_User.Name = "lblFTP_User";
            this.lblFTP_User.Size = new System.Drawing.Size(54, 25);
            this.lblFTP_User.TabIndex = 4;
            this.lblFTP_User.Text = "User";
            // 
            // txtFTP_PWD
            // 
            this.txtFTP_PWD.Location = new System.Drawing.Point(478, 103);
            this.txtFTP_PWD.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFTP_PWD.Name = "txtFTP_PWD";
            this.txtFTP_PWD.PasswordChar = '*';
            this.txtFTP_PWD.Size = new System.Drawing.Size(190, 33);
            this.txtFTP_PWD.TabIndex = 6;
            // 
            // lblFTP_PWD
            // 
            this.lblFTP_PWD.AutoSize = true;
            this.lblFTP_PWD.BackColor = System.Drawing.Color.Transparent;
            this.lblFTP_PWD.Location = new System.Drawing.Point(376, 108);
            this.lblFTP_PWD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFTP_PWD.Name = "lblFTP_PWD";
            this.lblFTP_PWD.Size = new System.Drawing.Size(102, 25);
            this.lblFTP_PWD.TabIndex = 8;
            this.lblFTP_PWD.Text = "Password";
            // 
            // txtFTP_Port
            // 
            this.txtFTP_Port.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtFTP_Port.Location = new System.Drawing.Point(478, 47);
            this.txtFTP_Port.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFTP_Port.MaxLength = 4;
            this.txtFTP_Port.Name = "txtFTP_Port";
            this.txtFTP_Port.Size = new System.Drawing.Size(96, 33);
            this.txtFTP_Port.TabIndex = 3;
            // 
            // lblFTP_Port
            // 
            this.lblFTP_Port.AutoSize = true;
            this.lblFTP_Port.BackColor = System.Drawing.Color.Transparent;
            this.lblFTP_Port.Location = new System.Drawing.Point(376, 53);
            this.lblFTP_Port.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFTP_Port.Name = "lblFTP_Port";
            this.lblFTP_Port.Size = new System.Drawing.Size(51, 25);
            this.lblFTP_Port.TabIndex = 2;
            this.lblFTP_Port.Text = "Port";
            // 
            // txtFTP_IP
            // 
            this.txtFTP_IP.Location = new System.Drawing.Point(148, 47);
            this.txtFTP_IP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFTP_IP.Name = "txtFTP_IP";
            this.txtFTP_IP.Size = new System.Drawing.Size(192, 33);
            this.txtFTP_IP.TabIndex = 1;
            // 
            // lblFTP_IP
            // 
            this.lblFTP_IP.AutoSize = true;
            this.lblFTP_IP.BackColor = System.Drawing.Color.Transparent;
            this.lblFTP_IP.Location = new System.Drawing.Point(9, 53);
            this.lblFTP_IP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFTP_IP.Name = "lblFTP_IP";
            this.lblFTP_IP.Size = new System.Drawing.Size(30, 25);
            this.lblFTP_IP.TabIndex = 0;
            this.lblFTP_IP.Text = "IP";
            // 
            // groupBoxConnect
            // 
            this.groupBoxConnect.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxConnect.Controls.Add(this.txtConnectionString);
            this.groupBoxConnect.Controls.Add(this.lblConnectionString);
            this.groupBoxConnect.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxConnect.Location = new System.Drawing.Point(4, 5);
            this.groupBoxConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxConnect.Name = "groupBoxConnect";
            this.groupBoxConnect.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxConnect.Size = new System.Drawing.Size(687, 123);
            this.groupBoxConnect.TabIndex = 0;
            this.groupBoxConnect.TabStop = false;
            this.groupBoxConnect.Text = "連線設定";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(194, 47);
            this.txtConnectionString.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(476, 33);
            this.txtConnectionString.TabIndex = 1;
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.BackColor = System.Drawing.Color.Transparent;
            this.lblConnectionString.Location = new System.Drawing.Point(9, 53);
            this.lblConnectionString.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(180, 25);
            this.lblConnectionString.TabIndex = 0;
            this.lblConnectionString.Text = "Connection String";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBoxTimerInterval);
            this.tabPage3.Location = new System.Drawing.Point(4, 30);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Size = new System.Drawing.Size(695, 735);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "其他設定";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBoxTimerInterval
            // 
            this.groupBoxTimerInterval.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxTimerInterval.Controls.Add(this.txtFTPTimerInterval);
            this.groupBoxTimerInterval.Controls.Add(this.lblFTPTimerInterval);
            this.groupBoxTimerInterval.Controls.Add(this.txtRealTimeTimerInterval);
            this.groupBoxTimerInterval.Controls.Add(this.lblRealTimeTimerInterval);
            this.groupBoxTimerInterval.Controls.Add(this.txtBatchTimerInterval);
            this.groupBoxTimerInterval.Controls.Add(this.lblBatchTimerInterval);
            this.groupBoxTimerInterval.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxTimerInterval.Location = new System.Drawing.Point(4, 5);
            this.groupBoxTimerInterval.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxTimerInterval.Name = "groupBoxTimerInterval";
            this.groupBoxTimerInterval.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxTimerInterval.Size = new System.Drawing.Size(687, 177);
            this.groupBoxTimerInterval.TabIndex = 0;
            this.groupBoxTimerInterval.TabStop = false;
            this.groupBoxTimerInterval.Text = "Timer Interval (秒，請勿小於10秒)";
            // 
            // txtFTPTimerInterval
            // 
            this.txtFTPTimerInterval.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtFTPTimerInterval.Location = new System.Drawing.Point(148, 103);
            this.txtFTPTimerInterval.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFTPTimerInterval.Name = "txtFTPTimerInterval";
            this.txtFTPTimerInterval.Size = new System.Drawing.Size(190, 33);
            this.txtFTPTimerInterval.TabIndex = 5;
            // 
            // lblFTPTimerInterval
            // 
            this.lblFTPTimerInterval.AutoSize = true;
            this.lblFTPTimerInterval.BackColor = System.Drawing.Color.Transparent;
            this.lblFTPTimerInterval.Location = new System.Drawing.Point(9, 108);
            this.lblFTPTimerInterval.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFTPTimerInterval.Name = "lblFTPTimerInterval";
            this.lblFTPTimerInterval.Size = new System.Drawing.Size(46, 25);
            this.lblFTPTimerInterval.TabIndex = 4;
            this.lblFTPTimerInterval.Text = "FTP";
            // 
            // txtRealTimeTimerInterval
            // 
            this.txtRealTimeTimerInterval.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtRealTimeTimerInterval.Location = new System.Drawing.Point(478, 47);
            this.txtRealTimeTimerInterval.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRealTimeTimerInterval.MaxLength = 4;
            this.txtRealTimeTimerInterval.Name = "txtRealTimeTimerInterval";
            this.txtRealTimeTimerInterval.Size = new System.Drawing.Size(192, 33);
            this.txtRealTimeTimerInterval.TabIndex = 3;
            // 
            // lblRealTimeTimerInterval
            // 
            this.lblRealTimeTimerInterval.AutoSize = true;
            this.lblRealTimeTimerInterval.BackColor = System.Drawing.Color.Transparent;
            this.lblRealTimeTimerInterval.Location = new System.Drawing.Point(376, 53);
            this.lblRealTimeTimerInterval.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRealTimeTimerInterval.Name = "lblRealTimeTimerInterval";
            this.lblRealTimeTimerInterval.Size = new System.Drawing.Size(103, 25);
            this.lblRealTimeTimerInterval.TabIndex = 2;
            this.lblRealTimeTimerInterval.Text = "Real Time";
            // 
            // txtBatchTimerInterval
            // 
            this.txtBatchTimerInterval.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBatchTimerInterval.Location = new System.Drawing.Point(148, 47);
            this.txtBatchTimerInterval.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBatchTimerInterval.Name = "txtBatchTimerInterval";
            this.txtBatchTimerInterval.Size = new System.Drawing.Size(192, 33);
            this.txtBatchTimerInterval.TabIndex = 1;
            // 
            // lblBatchTimerInterval
            // 
            this.lblBatchTimerInterval.AutoSize = true;
            this.lblBatchTimerInterval.BackColor = System.Drawing.Color.Transparent;
            this.lblBatchTimerInterval.Location = new System.Drawing.Point(9, 53);
            this.lblBatchTimerInterval.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBatchTimerInterval.Name = "lblBatchTimerInterval";
            this.lblBatchTimerInterval.Size = new System.Drawing.Size(64, 25);
            this.lblBatchTimerInterval.TabIndex = 0;
            this.lblBatchTimerInterval.Text = "Batch";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 796);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(709, 796);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(709, 796);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批次水單設定";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBoxBatchWorkingTime.ResumeLayout(false);
            this.groupBoxBatchWorkingTime.PerformLayout();
            this.groupBoxFTP.ResumeLayout(false);
            this.groupBoxFTP.PerformLayout();
            this.groupBoxConnect.ResumeLayout(false);
            this.groupBoxConnect.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBoxTimerInterval.ResumeLayout(false);
            this.groupBoxTimerInterval.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Beautify.SkinTabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Beautify.SkinButton btnStartWorking;
        private System.Windows.Forms.Label lblWorkEnd;
        private System.Windows.Forms.Label lblWorkStart;
        private System.Windows.Forms.DateTimePicker dateTimeWorkEnd;
        private System.Windows.Forms.DateTimePicker dateTimeWorkStart;
        private System.Windows.Forms.GroupBox groupBoxConnect;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.GroupBox groupBoxFTP;
        private System.Windows.Forms.TextBox txtFTP_IP;
        private System.Windows.Forms.Label lblFTP_IP;
        private System.Windows.Forms.TextBox txtFTP_Port;
        private System.Windows.Forms.Label lblFTP_Port;
        private System.Windows.Forms.TextBox txtFTP_PWD;
        private System.Windows.Forms.Label lblFTP_PWD;
        private System.Windows.Forms.TextBox txtFTP_User;
        private System.Windows.Forms.Label lblFTP_User;
        private System.Windows.Forms.TextBox txtFTP_Remote_Path;
        private System.Windows.Forms.Label lblFTP_Remote_Path;
        private System.Windows.Forms.GroupBox groupBoxBatchWorkingTime;
        private System.Windows.Forms.Label lblLastWorkingTime;
        private System.Windows.Forms.Label lblWorkingTimer;
        private System.Windows.Forms.Label lblLastWorkingTimeValue;
        private Beautify.SkinButton btnSave;
        private System.Windows.Forms.ComboBox comboBoxWorkingTimerM;
        private System.Windows.Forms.ComboBox comboBoxWorkingTimerH;
        private System.Windows.Forms.CheckBox chBoxBatchWorkingTime;
        private System.Windows.Forms.Label lblWorkingDateSwitchValue;
        private System.Windows.Forms.Label lblWorkingDateSwitch;
        private System.Windows.Forms.Label lblRealTimeStatusValue;
        private System.Windows.Forms.Label lblRealTimeStatus;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBoxTimerInterval;
        private System.Windows.Forms.TextBox txtFTPTimerInterval;
        private System.Windows.Forms.Label lblFTPTimerInterval;
        private System.Windows.Forms.TextBox txtRealTimeTimerInterval;
        private System.Windows.Forms.Label lblRealTimeTimerInterval;
        private System.Windows.Forms.TextBox txtBatchTimerInterval;
        private System.Windows.Forms.Label lblBatchTimerInterval;
    }
}

