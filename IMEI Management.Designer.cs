namespace IMEIManagement
{
    partial class IMEI_Management
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
        this.cmbPortName = new System.Windows.Forms.ComboBox();
        this.cmbBaudRate = new System.Windows.Forms.ComboBox();
        this.lblComPort = new System.Windows.Forms.Label();
        this.lblBaudRate = new System.Windows.Forms.Label();
        this.label1 = new System.Windows.Forms.Label();
        this.cmbParity = new System.Windows.Forms.ComboBox();
        this.lblDataBits = new System.Windows.Forms.Label();
        this.cmbDataBits = new System.Windows.Forms.ComboBox();
        this.lblStopBits = new System.Windows.Forms.Label();
        this.cmbStopBits = new System.Windows.Forms.ComboBox();
        this.btnOpenPort = new System.Windows.Forms.Button();
        this.gbPortSettings = new System.Windows.Forms.GroupBox();
        this.tmrCheckComPorts = new System.Windows.Forms.Timer(this.components);
        this.toolTip = new System.Windows.Forms.ToolTip(this.components);
        this.btRefresh = new System.Windows.Forms.Button();
        this.btExcel = new System.Windows.Forms.Button();
        this.dgv_IMEI = new System.Windows.Forms.DataGridView();
        this.IMEI = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.Model = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.Buyer = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.Purpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.gbPortSettings.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgv_IMEI)).BeginInit();
        this.SuspendLayout();
        // 
        // cmbPortName
        // 
        this.cmbPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbPortName.FormattingEnabled = true;
        this.cmbPortName.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6"});
        this.cmbPortName.Location = new System.Drawing.Point(74, 21);
        this.cmbPortName.Name = "cmbPortName";
        this.cmbPortName.Size = new System.Drawing.Size(95, 21);
        this.cmbPortName.TabIndex = 1;
        // 
        // cmbBaudRate
        // 
        this.cmbBaudRate.FormattingEnabled = true;
        this.cmbBaudRate.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
        this.cmbBaudRate.Location = new System.Drawing.Point(74, 48);
        this.cmbBaudRate.Name = "cmbBaudRate";
        this.cmbBaudRate.Size = new System.Drawing.Size(95, 21);
        this.cmbBaudRate.TabIndex = 3;
        this.cmbBaudRate.Validating += new System.ComponentModel.CancelEventHandler(this.cmbBaudRate_Validating);
        // 
        // lblComPort
        // 
        this.lblComPort.AutoSize = true;
        this.lblComPort.Location = new System.Drawing.Point(7, 24);
        this.lblComPort.Name = "lblComPort";
        this.lblComPort.Size = new System.Drawing.Size(56, 13);
        this.lblComPort.TabIndex = 0;
        this.lblComPort.Text = "COM Port:";
        // 
        // lblBaudRate
        // 
        this.lblBaudRate.AutoSize = true;
        this.lblBaudRate.Location = new System.Drawing.Point(7, 51);
        this.lblBaudRate.Name = "lblBaudRate";
        this.lblBaudRate.Size = new System.Drawing.Size(61, 13);
        this.lblBaudRate.TabIndex = 2;
        this.lblBaudRate.Text = "Baud Rate:";
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(7, 80);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(36, 13);
        this.label1.TabIndex = 4;
        this.label1.Text = "Parity:";
        // 
        // cmbParity
        // 
        this.cmbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbParity.FormattingEnabled = true;
        this.cmbParity.Items.AddRange(new object[] {
            "None",
            "Even",
            "Odd"});
        this.cmbParity.Location = new System.Drawing.Point(74, 77);
        this.cmbParity.Name = "cmbParity";
        this.cmbParity.Size = new System.Drawing.Size(95, 21);
        this.cmbParity.TabIndex = 5;
        // 
        // lblDataBits
        // 
        this.lblDataBits.AutoSize = true;
        this.lblDataBits.Location = new System.Drawing.Point(7, 108);
        this.lblDataBits.Name = "lblDataBits";
        this.lblDataBits.Size = new System.Drawing.Size(53, 13);
        this.lblDataBits.TabIndex = 6;
        this.lblDataBits.Text = "Data Bits:";
        // 
        // cmbDataBits
        // 
        this.cmbDataBits.FormattingEnabled = true;
        this.cmbDataBits.Items.AddRange(new object[] {
            "7",
            "8",
            "9"});
        this.cmbDataBits.Location = new System.Drawing.Point(74, 105);
        this.cmbDataBits.Name = "cmbDataBits";
        this.cmbDataBits.Size = new System.Drawing.Size(95, 21);
        this.cmbDataBits.TabIndex = 7;
        this.cmbDataBits.Validating += new System.ComponentModel.CancelEventHandler(this.cmbDataBits_Validating);
        // 
        // lblStopBits
        // 
        this.lblStopBits.AutoSize = true;
        this.lblStopBits.Location = new System.Drawing.Point(7, 137);
        this.lblStopBits.Name = "lblStopBits";
        this.lblStopBits.Size = new System.Drawing.Size(52, 13);
        this.lblStopBits.TabIndex = 8;
        this.lblStopBits.Text = "Stop Bits:";
        // 
        // cmbStopBits
        // 
        this.cmbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbStopBits.FormattingEnabled = true;
        this.cmbStopBits.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
        this.cmbStopBits.Location = new System.Drawing.Point(74, 134);
        this.cmbStopBits.Name = "cmbStopBits";
        this.cmbStopBits.Size = new System.Drawing.Size(95, 21);
        this.cmbStopBits.TabIndex = 9;
        // 
        // btnOpenPort
        // 
        this.btnOpenPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnOpenPort.Location = new System.Drawing.Point(641, 190);
        this.btnOpenPort.Name = "btnOpenPort";
        this.btnOpenPort.Size = new System.Drawing.Size(60, 62);
        this.btnOpenPort.TabIndex = 6;
        this.btnOpenPort.Text = "Open Port";
        this.btnOpenPort.Click += new System.EventHandler(this.btnOpenPort_Click);
        // 
        // gbPortSettings
        // 
        this.gbPortSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.gbPortSettings.Controls.Add(this.cmbPortName);
        this.gbPortSettings.Controls.Add(this.cmbBaudRate);
        this.gbPortSettings.Controls.Add(this.cmbStopBits);
        this.gbPortSettings.Controls.Add(this.cmbParity);
        this.gbPortSettings.Controls.Add(this.cmbDataBits);
        this.gbPortSettings.Controls.Add(this.lblComPort);
        this.gbPortSettings.Controls.Add(this.lblStopBits);
        this.gbPortSettings.Controls.Add(this.lblBaudRate);
        this.gbPortSettings.Controls.Add(this.lblDataBits);
        this.gbPortSettings.Controls.Add(this.label1);
        this.gbPortSettings.Location = new System.Drawing.Point(567, 12);
        this.gbPortSettings.Name = "gbPortSettings";
        this.gbPortSettings.Size = new System.Drawing.Size(197, 172);
        this.gbPortSettings.TabIndex = 4;
        this.gbPortSettings.TabStop = false;
        this.gbPortSettings.Text = "COM Serial Port Settings";
        // 
        // tmrCheckComPorts
        // 
        this.tmrCheckComPorts.Enabled = true;
        this.tmrCheckComPorts.Interval = 500;
        // 
        // btRefresh
        // 
        this.btRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btRefresh.Location = new System.Drawing.Point(573, 190);
        this.btRefresh.Name = "btRefresh";
        this.btRefresh.Size = new System.Drawing.Size(62, 62);
        this.btRefresh.TabIndex = 6;
        this.btRefresh.Text = "Refresh Ports";
        this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
        // 
        // btExcel
        // 
        this.btExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btExcel.Image = global::IMEIManagement.Properties.Resources.excel;
        this.btExcel.Location = new System.Drawing.Point(707, 190);
        this.btExcel.Name = "btExcel";
        this.btExcel.Size = new System.Drawing.Size(60, 62);
        this.btExcel.TabIndex = 9;
        this.btExcel.Click += new System.EventHandler(this.btExcel_Click);
        // 
        // dgv_IMEI
        // 
        this.dgv_IMEI.BackgroundColor = System.Drawing.SystemColors.ControlLight;
        dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.dgv_IMEI.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
        this.dgv_IMEI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgv_IMEI.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IMEI,
            this.Model,
            this.Buyer,
            this.Purpose,
            this.Note});
        this.dgv_IMEI.Location = new System.Drawing.Point(12, 12);
        this.dgv_IMEI.Name = "dgv_IMEI";
        this.dgv_IMEI.Size = new System.Drawing.Size(549, 471);
        this.dgv_IMEI.TabIndex = 13;
        // 
        // IMEI
        // 
        this.IMEI.HeaderText = "IMEI Number";
        this.IMEI.Name = "IMEI";
        // 
        // Model
        // 
        this.Model.HeaderText = "Model";
        this.Model.Name = "Model";
        // 
        // Buyer
        // 
        this.Buyer.HeaderText = "Buyer";
        this.Buyer.Name = "Buyer";
        // 
        // Purpose
        // 
        this.Purpose.HeaderText = "Purpose";
        this.Purpose.Name = "Purpose";
        // 
        // Note
        // 
        this.Note.HeaderText = "Note";
        this.Note.Name = "Note";
        // 
        // IMEI_Management
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(776, 495);
        this.Controls.Add(this.dgv_IMEI);
        this.Controls.Add(this.btExcel);
        this.Controls.Add(this.gbPortSettings);
        this.Controls.Add(this.btRefresh);
        this.Controls.Add(this.btnOpenPort);
        this.MinimumSize = new System.Drawing.Size(505, 250);
        this.Name = "IMEI_Management";
        this.Text = "IMEI Management";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTerminal_FormClosing);
        this.Load += new System.EventHandler(this.frmTerminal_Load);
        this.Shown += new System.EventHandler(this.frmTerminal_Shown);
        this.gbPortSettings.ResumeLayout(false);
        this.gbPortSettings.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgv_IMEI)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ComboBox cmbPortName;
    private System.Windows.Forms.ComboBox cmbBaudRate;
    private System.Windows.Forms.Label lblComPort;
    private System.Windows.Forms.Label lblBaudRate;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cmbParity;
    private System.Windows.Forms.Label lblDataBits;
    private System.Windows.Forms.ComboBox cmbDataBits;
    private System.Windows.Forms.Label lblStopBits;
    private System.Windows.Forms.ComboBox cmbStopBits;
    private System.Windows.Forms.Button btnOpenPort;
    private System.Windows.Forms.GroupBox gbPortSettings;
		private System.Windows.Forms.Timer tmrCheckComPorts;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btRefresh;
        private System.Windows.Forms.Button btExcel;
        private System.Windows.Forms.DataGridView dgv_IMEI;
        private System.Windows.Forms.DataGridViewTextBoxColumn IMEI;
        private System.Windows.Forms.DataGridViewTextBoxColumn Model;
        private System.Windows.Forms.DataGridViewTextBoxColumn Buyer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Purpose;
        private System.Windows.Forms.DataGridViewTextBoxColumn Note;
  }
}

