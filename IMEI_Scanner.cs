#region Namespace Inclusions

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IMEIScanner.Properties;

#endregion

namespace IMEIScanner
{

    #region Public Enumerations

    public enum DataMode
    {
        Text,
        Hex
    }

    #endregion

    public partial class IMEI_Scanner : Form
    {
        #region Local Variables

        private readonly NotifyIcon TrayIcon;
        private readonly ContextMenu TrayIconContextMenu = new ContextMenu();

        // The main control for communicating through the RS-232 port
        private readonly SerialPort comport = new SerialPort();

        private readonly Settings settings = Settings.Default;
        private bool hasChanged;

        #endregion

        #region Constructor

        public IMEI_Scanner()
        {
            settings.Reload();

            #region BalloonTip

            TrayIcon = new NotifyIcon();
            TrayIcon.Icon = Resources.IMEI_Scanner;
            TrayIcon.BalloonTipIcon = ToolTipIcon.Info;
            TrayIcon.BalloonTipText = "Welcome to IMEI Scanner PGM";
            TrayIcon.BalloonTipTitle = "IMEI scanner";
            TrayIcon.Text = "Scan IMEI number and click to copy content";
            TrayIcon.BalloonTipClicked += TrayIcon_BalloonTipClicked;
            TrayIcon.DoubleClick += TrayIcon_DoubleClick;
            TrayIcon.ContextMenu = TrayIconContextMenu;
            TrayIcon.Visible = true;
            TrayIcon.ShowBalloonTip(15);

            TrayIconContextMenu.MenuItems.Add("&IMEI Scanner", IMEI_Scanner_Click);
            TrayIconContextMenu.MenuItems.Add("&Exit", Exit_Click);

            #endregion

            InitializeComponent();

            InitializeControlValues();

            EnableControls();

            comport.DataReceived += port_DataReceived;
            hasChanged = false;
        }

        //void IMEI_Scanner_SizeChanged(object sender, EventArgs e)
        //{
        //    if (this.WindowState == FormWindowState.Minimized)
        //    {
        //        Hide();
        //    }
        //}

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            TrayIcon.ShowBalloonTip(15);
        }

        private void IMEI_Scanner_Click(object sender, EventArgs e)
        {
            foreach (
                Form form in Application.OpenForms.Cast<Form>().Where(form => form.GetType() == typeof (IMEI_Scanner)))
            {
                form.Show();
                form.BringToFront();
                return;
            }
            //if (this.WindowState == FormWindowState.Minimized)
            //{
            //    Show();
            //    WindowState = FormWindowState.Normal;
            //}
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TrayIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            Clipboard.SetText(TrayIcon.BalloonTipText);
        }

        #endregion

        private void SaveSettings()
        {
            settings.BaudRate = int.Parse(cmbBaudRate.Text);
            settings.DataBits = int.Parse(cmbDataBits.Text);
            settings.DataMode = DataMode.Text;
            settings.Parity = (Parity) Enum.Parse(typeof (Parity), cmbParity.Text);
            settings.StopBits = (StopBits) Enum.Parse(typeof (StopBits), cmbStopBits.Text);
            settings.PortName = "COM2";

            settings.Save();
        }

        private void InitializeControlValues()
        {
            cmbParity.Items.Clear();
            cmbParity.Items.AddRange(Enum.GetNames(typeof (Parity)));
            cmbStopBits.Items.Clear();
            cmbStopBits.Items.AddRange(Enum.GetNames(typeof (StopBits)));

            cmbParity.Text = "None";
            cmbStopBits.Text = "One";
            cmbDataBits.Text = "8";
            cmbBaudRate.Text = "9600";

            RefreshComPortList();

            if (cmbPortName.Items.Contains(settings.PortName)) cmbPortName.Text = settings.PortName;
            else if (cmbPortName.Items.Count > 0) cmbPortName.SelectedIndex = cmbPortName.Items.Count - 1;
            else
            {
                MessageBox.Show(this, "Không tìm thấy cổng COM.\nVui lòng kiểm tra lại.", "Không có cổng COM",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void EnableControls()
        {
            // Enable/disable controls based on whether the port is open or not
            gbPortSettings.Enabled = !comport.IsOpen;
            //chkDTR.Enabled = chkRTS.Enabled = comport.IsOpen;

            if (comport.IsOpen) btnOpenPort.Text = "&Close Port";
            else btnOpenPort.Text = "&Open Port";
        }

        private void SetData(string text)
        {
            dgv_IMEI.Invoke(new EventHandler(delegate { dgv_IMEI.Rows.Add(text, "", "", settings.Purpose); }));
            TrayIcon.BalloonTipText = text;
            TrayIcon.ShowBalloonTip(15);
        }

        private void RefreshComPortList()
        {
            // Determain if the list of com port names has changed since last checked
            var selected = RefreshComPortList(cmbPortName.Items.Cast<string>(), cmbPortName.SelectedItem as string,
                                              comport.IsOpen);

            // If there was an update, then update the control showing the user the list of port names
            if (!String.IsNullOrEmpty(selected))
            {
                cmbPortName.Items.Clear();
                cmbPortName.Items.AddRange(OrderedPortNames());
                cmbPortName.SelectedItem = selected;
            }
        }

        private string[] OrderedPortNames()
        {
            int num;
            return
                SerialPort.GetPortNames().OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out num) ? num : 0).
                    ToArray();
        }

        private string RefreshComPortList(IEnumerable<string> previousPortNames, string currentSelection, bool portOpen)
        {
            string selected = null;
            var ports = SerialPort.GetPortNames();
            var updated = previousPortNames.Except(ports).Count() > 0 || ports.Except(previousPortNames).Count() > 0;

            if (updated)
            {
                ports = OrderedPortNames();
                var newest = SerialPort.GetPortNames().Except(previousPortNames).OrderBy(a => a).LastOrDefault();
                if (portOpen)
                {
                    if (ports.Contains(currentSelection)) selected = currentSelection;
                    else if (!String.IsNullOrEmpty(newest)) selected = newest;
                    else selected = ports.LastOrDefault();
                }
                else
                {
                    if (!String.IsNullOrEmpty(newest)) selected = newest;
                    else if (ports.Contains(currentSelection)) selected = currentSelection;
                    else selected = ports.LastOrDefault();
                }
            }
            return selected;
        }

        private void frmTerminal_Load(object sender, EventArgs e)
        {
            btnOpenPort.PerformClick();
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            RefreshComPortList();
        }

        private void dgv_IMEI_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            hasChanged = true;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn xóa hết thông tin?", "Xác nhận",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                dgv_IMEI.Rows.Clear();
                hasChanged = false;
            }
            else
            {
                TrayIcon.BalloonTipText = "test";
                TrayIcon.ShowBalloonTip(10);
            }
        }

        private void btReport_Click(object sender, EventArgs e)
        {
            var currentRow = 6;
            var excel = new CreateExcelDoc();
            excel.CreateHeaders(2, 5, "IMEI WRITING DAILY REPORT", "C2", "G2", 5, "YELLOW", true, 18, "yes");
            excel.AddData(4, 7, "Date: " + DateTime.Today.ToShortDateString(), "G4", "G4", "dd/mm/yyyy");

            excel.CreateHeaders(currentRow, 1, "No.", "A6", "A6", 1, "WHITE", true, 5, "n");
            excel.CreateHeaders(currentRow, 2, "Date", "B6", "B6", 1, "WHITE", true, 15, "n");
            excel.CreateHeaders(currentRow, 3, "IMEI/Serial Number", "C6", "C6", 1, "WHITE", true, 20, "n");
            excel.CreateHeaders(currentRow, 4, "Model", "D6", "D6", 1, "WHITE", true, 15, "n");
            excel.CreateHeaders(currentRow, 5, "Buyer", "E6", "E6", 1, "WHITE", true, 15, "n");
            excel.CreateHeaders(currentRow, 6, "Purpose", "F6", "F6", 1, "WHITE", true, 25, "n");
            excel.CreateHeaders(currentRow, 7, "Note", "G6", "G6", 1, "WHITE", true, 15, "n");

            foreach (DataGridViewRow row in dgv_IMEI.Rows)
            {
                // Neu ko co so IMEI, chuyen sang dong tiep theo
                if (String.IsNullOrEmpty(row.Cells[0].FormattedValue.ToString()))
                    continue;

                currentRow++;
                excel.AddData(currentRow, 1, "" + (currentRow - 6), "A" + currentRow, "A" + currentRow, "");
                excel.AddData(currentRow, 2, "" + DateTime.Today.ToShortDateString(), "B" + currentRow, "B" + currentRow,
                              "");
                excel.AddData(currentRow, 3, "'" + row.Cells[0].FormattedValue.ToString().Trim(), "C" + currentRow,
                              "C" + currentRow, "");
                excel.AddData(currentRow, 4, row.Cells[1].FormattedValue.ToString().Trim().ToUpper(), "D" + currentRow,
                              "D" + currentRow, "");
                excel.AddData(currentRow, 5, row.Cells[2].FormattedValue.ToString().Trim().ToUpper(), "E" + currentRow,
                              "E" + currentRow,
                              "");
                if (String.IsNullOrEmpty(row.Cells[3].FormattedValue.ToString().Trim()))
                {
                    excel.AddData(currentRow, 6, settings.Purpose.Trim(), "F" + currentRow, "F" + currentRow, "");
                }
                else
                {
                    excel.AddData(currentRow, 6, row.Cells[3].FormattedValue.ToString().Trim(), "F" + currentRow,
                                  "F" + currentRow, "");
                }
                excel.AddData(currentRow, 7, row.Cells[4].FormattedValue.ToString().Trim(), "G" + currentRow,
                              "G" + currentRow,
                              "");
            }
            hasChanged = false;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgv_IMEI.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            Clipboard.SetDataObject(dgv_IMEI.SelectedCells);
            if (dgv_IMEI.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    // Add the selection to the clipboard.
                    Clipboard.SetDataObject(
                        dgv_IMEI.GetClipboardContent());
                }
                catch (ExternalException)
                {
                    MessageBox.Show("Không thể copy vào clipboard", "Lỗi copy");
                }
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in dgv_IMEI.SelectedCells)
            {
                cell.Value = Clipboard.GetText();
            }
        }

        private void dgv_IMEI_KeyDown(object sender, KeyEventArgs e)
        {
            if (!dgv_IMEI.CurrentCell.IsInEditMode)
            {
                if (e.KeyData == (Keys.Control | Keys.C))
                {
                    copyToolStripMenuItem.PerformClick();
                }
                else if (e.KeyData == (Keys.Control | Keys.V))
                {
                    pasteToolStripMenuItem.PerformClick();
                }
                else if (e.KeyData == (Keys.Control | Keys.A))
                {
                    selectAllToolStripMenuItem.PerformClick();
                }
                else if (e.KeyData == (Keys.Control | Keys.X))
                {
                    cutToolStripMenuItem.PerformClick();
                }
                else if (e.KeyData == Keys.Delete)
                {
                    foreach (DataGridViewCell cell in dgv_IMEI.SelectedCells)
                    {
                        cell.Value = "";
                    }
                }
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(dgv_IMEI.SelectedCells);
            if (dgv_IMEI.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    // Add the selection to the clipboard.
                    Clipboard.SetDataObject(
                        dgv_IMEI.GetClipboardContent());
                    foreach (DataGridViewCell cell in dgv_IMEI.SelectedCells)
                    {
                        cell.Value = "";
                    }
                }
                catch (ExternalException)
                {
                    MessageBox.Show("Không thể copy vào clipboard", "Lỗi copy");
                }
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgv_IMEI.SelectAll();
        }

        private void dgv_IMEI_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        private void dgv_IMEI_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex != -1 && e.RowIndex != -1)
                {
                    if (!dgv_IMEI.SelectedCells.Contains(dgv_IMEI.Rows[e.RowIndex].Cells[e.ColumnIndex]))
                    {
                        dgv_IMEI.ClearSelection();
                        dgv_IMEI.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                    }
                }
            }
        }

        #region Event Handlers

        private void frmTerminal_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
            if (hasChanged)
            {
                MessageBox.Show("Bạn cần phải xuất ra excel trước khi thoát", "Chú ý", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void cmbBaudRate_Validating(object sender, CancelEventArgs e)
        {
            int x;
            e.Cancel = !int.TryParse(cmbBaudRate.Text, out x);
        }

        private void cmbDataBits_Validating(object sender, CancelEventArgs e)
        {
            int x;
            e.Cancel = !int.TryParse(cmbDataBits.Text, out x);
        }

        private void btnOpenPort_Click(object sender, EventArgs e)
        {
            try
            {
                var error = false;

                // If the port is open, close it.
                if (comport.IsOpen) comport.Close();
                else
                {
                    // Set the port's settings
                    comport.BaudRate = int.Parse(cmbBaudRate.Text);
                    comport.DataBits = int.Parse(cmbDataBits.Text);
                    comport.StopBits = (StopBits) Enum.Parse(typeof (StopBits), cmbStopBits.Text);
                    comport.Parity = (Parity) Enum.Parse(typeof (Parity), cmbParity.Text);
                    comport.PortName = cmbPortName.Text;

                    try
                    {
                        // Open the port
                        comport.Open();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        error = true;
                    }
                    catch (IOException)
                    {
                        error = true;
                    }
                    catch (ArgumentException)
                    {
                        error = true;
                    }

                    if (error)
                        MessageBox.Show(this, "Không thể mở cổng " + comport.PortName + ", vui lòng kiểm tra lại",
                                        "Lỗi cổng COM", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                // Change the state of the form's controls
                EnableControls();
            }
            catch (Exception ex)
            {
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!comport.IsOpen) return;
            var data = "";

            while (!data.Contains("\n"))
            {
                data += comport.ReadExisting();
            }
            SetData(data);
        }

        #endregion
    }
}