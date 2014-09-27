#region Namespace Inclusions
using System;
using System.Linq;
using System.Data;
using System.Text;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using IMEIManagement.Properties;
using System.Threading;
using System.IO;
#endregion

namespace IMEIManagement
{
  #region Public Enumerations
  public enum DataMode { Text, Hex }
  #endregion

  public partial class IMEI_Management : Form
  {
    #region Local Variables

    private NotifyIcon TrayIcon;
    private ContextMenuStrip TrayIconContextMenu;
    private ToolStripMenuItem CloseMenuItem;

    // The main control for communicating through the RS-232 port
    private SerialPort comport = new SerialPort();

    // Various colors for logging info
    private Color[] LogMsgTypeColor = { Color.Blue, Color.Green, Color.Black, Color.Orange, Color.Red };
      
	private Settings settings = Settings.Default;
      
    #endregion

    #region Constructor
    public IMEI_Management()
    {
	  settings.Reload();
            #region BalloonTip
            TrayIcon = new NotifyIcon();

            TrayIcon.BalloonTipIcon = ToolTipIcon.Info;
            TrayIcon.BalloonTipText = "Hello";
            TrayIcon.BalloonTipTitle = "Barcode scanner";
            TrayIcon.Text = "Scan barcode and click to copy content";
            TrayIcon.BalloonTipClicked += TrayIcon_BalloonTipClicked;
            #endregion
      InitializeComponent();

      InitializeControlValues();

      EnableControls();

      comport.DataReceived +=new SerialDataReceivedEventHandler(port_DataReceived);
    }

    private void TrayIcon_BalloonTipClicked(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    #endregion

    private void SaveSettings()
    {
			settings.BaudRate = int.Parse(cmbBaudRate.Text);
			settings.DataBits = int.Parse(cmbDataBits.Text);
			settings.DataMode = DataMode.Text;
			settings.Parity = (Parity)Enum.Parse(typeof(Parity), cmbParity.Text);
			settings.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cmbStopBits.Text);
			settings.PortName = "COM2";

			settings.Save();
    }

    private void InitializeControlValues()
    {
      cmbParity.Items.Clear(); cmbParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
      cmbStopBits.Items.Clear(); cmbStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));

      cmbParity.Text = "None";
			cmbStopBits.Text = "One";
			cmbDataBits.Text = "8";
			cmbBaudRate.Text = "9600";

			RefreshComPortList();

			if (cmbPortName.Items.Contains(settings.PortName)) cmbPortName.Text = settings.PortName;
      else if (cmbPortName.Items.Count > 0) cmbPortName.SelectedIndex = cmbPortName.Items.Count - 1;
      else
      {
        MessageBox.Show(this, "Không tìm thấy cổng COM.\nVui lòng kiểm tra lại.", "Không có cổng COM", MessageBoxButtons.OK, MessageBoxIcon.Error);
        this.Close();
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
    
    #region Event Handlers
    private void frmTerminal_Shown(object sender, EventArgs e)
    {
   //   rtfTerminal.Text = "Application Started at "+ DateTime.Now + " \n";
    }
    private void frmTerminal_FormClosing(object sender, FormClosingEventArgs e)
    {
      SaveSettings();
    }

    private void cmbBaudRate_Validating(object sender, CancelEventArgs e)
    { int x; e.Cancel = !int.TryParse(cmbBaudRate.Text, out x); }

    private void cmbDataBits_Validating(object sender, CancelEventArgs e)
    { int x; e.Cancel = !int.TryParse(cmbDataBits.Text, out x); }

    private void btnOpenPort_Click(object sender, EventArgs e)
    {
        try
        {
            bool error = false;

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
        }catch(Exception ex){}
    }

    private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
	  if (!comport.IsOpen) return;
      String data = "";

      while (!data.Contains("\n"))
      {
        data += comport.ReadExisting();
      }
      SetData(data);
    }
    #endregion

    private void SetData(string text)
    {
        dgv_IMEI.Invoke(new EventHandler(delegate
        {
            dgv_IMEI.Rows.Add(text, "", "", "Test IMEI writing");
        }));

    }
	private void btnClear_Click(object sender, EventArgs e)
	{
	    dgv_IMEI.Rows.Add(1);
            //rtfTerminal.Clear();
   //         SetData("0988766554");
		}

	private void RefreshComPortList()
		{
			// Determain if the list of com port names has changed since last checked
			string selected = RefreshComPortList(cmbPortName.Items.Cast<string>(), cmbPortName.SelectedItem as string, comport.IsOpen);

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
			// Just a placeholder for a successful parsing of a string to an integer
			int num;

			// Order the serial port names in numberic order (if possible)
			return SerialPort.GetPortNames().OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out num) ? num : 0).ToArray(); 
		}
		
	private string RefreshComPortList(IEnumerable<string> PreviousPortNames, string CurrentSelection, bool PortOpen)
		{
			// Create a new return report to populate
			string selected = null;

			// Retrieve the list of ports currently mounted by the operating system (sorted by name)
			string[] ports = SerialPort.GetPortNames();

			// First determain if there was a change (any additions or removals)
			bool updated = PreviousPortNames.Except(ports).Count() > 0 || ports.Except(PreviousPortNames).Count() > 0;

			// If there was a change, then select an appropriate default port
			if (updated)
			{
				// Use the correctly ordered set of port names
				ports = OrderedPortNames();

				// Find newest port if one or more were added
				string newest = SerialPort.GetPortNames().Except(PreviousPortNames).OrderBy(a => a).LastOrDefault();

				// If the port was already open... (see logic notes and reasoning in Notes.txt)
				if (PortOpen)
				{
					if (ports.Contains(CurrentSelection)) selected = CurrentSelection;
					else if (!String.IsNullOrEmpty(newest)) selected = newest;
					else selected = ports.LastOrDefault();
				}
				else
				{
					if (!String.IsNullOrEmpty(newest)) selected = newest;
					else if (ports.Contains(CurrentSelection)) selected = CurrentSelection;
					else selected = ports.LastOrDefault();
				}
			}

			// If there was a change to the port list, return the recommended default selection
			return selected;
		}

    private void frmTerminal_Load(object sender, EventArgs e)
    {
        btnOpenPort_Click(sender, e);
    }

    private void btExcel_Click(object sender, EventArgs e)
    {
        int currentRow = 6;
        CreateExcelDoc excel = new CreateExcelDoc();
        excel.CreateHeaders(2, 5, "IMEI WRITING DAILY REPORT","D2","H2",5,"YELLOW",true,18,"yes");
        excel.AddData(4,8,"Date: "+DateTime.Today.ToShortDateString(),"H4","H4","dd/mm/yyyy");

        excel.CreateHeaders(currentRow, 4, "IMEI Number", "D6", "D6", 1, "WHITE", true, 15, "n");
        excel.CreateHeaders(currentRow, 5, "Model", "E6", "E6", 1, "WHITE", true, 15, "n");
        excel.CreateHeaders(currentRow, 6, "Buyer", "F6", "F6", 1, "WHITE", true, 15, "n");
        excel.CreateHeaders(currentRow, 7, "Purpose", "G6", "G6", 1, "WHITE", true, 30, "n");
        excel.CreateHeaders(currentRow, 8, "Note", "H6", "H6", 1, "WHITE", true, 15, "n");

        foreach (DataGridViewRow row in dgv_IMEI.Rows)
        {
            // Neu ko co so IMEI, chuyen sang dong tiep theo
            if (String.IsNullOrEmpty(row.Cells[0].FormattedValue.ToString()))
                continue;

            currentRow++;
            excel.AddData(currentRow, 4, row.Cells[0].FormattedValue.ToString(), "D" + currentRow, "D" + currentRow, "");
            excel.AddData(currentRow, 5, row.Cells[1].FormattedValue.ToString(), "E" + currentRow, "E" + currentRow, "");
            excel.AddData(currentRow, 6, row.Cells[2].FormattedValue.ToString(), "F" + currentRow, "F" + currentRow, "");
            if (String.IsNullOrEmpty(row.Cells[3].FormattedValue.ToString()))
            {
                excel.AddData(currentRow, 7, "Test IMEI writing", "G" + currentRow, "G" + currentRow, "");
            }
            else
            {
                excel.AddData(currentRow, 7, row.Cells[3].FormattedValue.ToString(), "G" + currentRow, "G" + currentRow, "");
            }
            excel.AddData(currentRow, 8, row.Cells[4].FormattedValue.ToString(), "H" + currentRow, "H" + currentRow, "");
        }
    }

    private void btRefresh_Click(object sender, EventArgs e)
    {
        RefreshComPortList();
    }

	}
}