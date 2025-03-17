using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace Tech2Communication
{
    public partial class TechAppForm : Form
    {
        private SerialCommunication serialComm;
        private SecurityAccessManager securityManager;
        private System.Timers.Timer keepAliveTimer;
        private string extractedVIN = string.Empty;
        private bool securityAccessGranted = false;

        public TechAppForm()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Initialize port dropdown
            cboPort.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                cboPort.Items.Add(port);
            }
            if (cboPort.Items.Count > 0)
                cboPort.SelectedIndex = 0;

            // Initialize baud rate options
            cboBaudRate.Items.Clear();
            cboBaudRate.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200" });
            cboBaudRate.SelectedIndex = 0;

            // Initialize parity options
            cboParity.Items.Clear();
            foreach (string name in Enum.GetNames(typeof(Parity)))
            {
                cboParity.Items.Add(name);
            }
            cboParity.SelectedIndex = 0;

            // Initialize stop bits options
            cboStopBits.Items.Clear();
            foreach (string name in Enum.GetNames(typeof(StopBits)))
            {
                cboStopBits.Items.Add(name);
            }
            cboStopBits.SelectedIndex = 1; // One stop bit

            // Initialize access level dropdown
            cboAccessLevel.Items.Clear();
            cboAccessLevel.Items.Add("Level 01 (Basic)");
            cboAccessLevel.Items.Add("Level FB (Intermediate)");
            cboAccessLevel.Items.Add("Level FD (Highest)");
            cboAccessLevel.SelectedIndex = 2; // Default to highest level

            // Lock tabs initially
            tabControl.SelectedIndex = 0;
            tabControl.Enabled = true;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the selected port and baud rate
                string portName = cboPort.SelectedItem.ToString();
                int baudRate = int.Parse(cboBaudRate.SelectedItem.ToString());
                Parity parity = (Parity)Enum.Parse(typeof(Parity), cboParity.SelectedItem.ToString());
                StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), cboStopBits.SelectedItem.ToString());

                // Create the serial communication object
                serialComm = new SerialCommunication(portName, baudRate, parity, stopBits);

                // Try to open the port
                serialComm.Open();

                // Create the security manager
                securityManager = new SecurityAccessManager(serialComm);

                // Setup keep-alive timer
                SetupKeepAliveTimer();

                // Update UI
                btnConnect.Enabled = false;
                btnNext1.Enabled = true;
                UpdateStatus("Connected to " + portName);

                LogMessage("Connected to " + portName + " at " + baudRate + " baud");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to serial port: " + ex.Message,
                                "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Connection failed");
                LogMessage("Connection error: " + ex.Message);
            }
        }

        private void SetupKeepAliveTimer()
        {
            // Setup a timer for keep-alive messages (3 seconds)
            keepAliveTimer = new System.Timers.Timer(3000);
            keepAliveTimer.Elapsed += KeepAliveTimer_Elapsed;
            keepAliveTimer.AutoReset = true;
            keepAliveTimer.Start();
        }

        private void KeepAliveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (serialComm != null && serialComm.IsOpen)
            {
                try
                {
                    // Send keep-alive message on a separate thread
                    Invoke(new Action(() => {
                        securityManager.SendKeepAlive();
                    }));
                }
                catch (Exception ex)
                {
                    // Log but don't display errors for keep-alive
                    Console.WriteLine("Keep-alive error: " + ex.Message);
                }
            }
        }

        private void btnNext1_Click(object sender, EventArgs e)
        {
            // Move to the security access tab
            tabControl.SelectedIndex = 1;
        }

        private void btnGetSeed_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the selected access level
                AccessLevel level = AccessLevel.AccessLevelFD; // Default
                switch (cboAccessLevel.SelectedIndex)
                {
                    case 0:
                        level = AccessLevel.AccessLevel01;
                        break;
                    case 1:
                        level = AccessLevel.AccessLevelFB;
                        break;
                    case 2:
                        level = AccessLevel.AccessLevelFD;
                        break;
                }

                UpdateStatus("Requesting security access...");
                LogMessage("Requesting security access with level: " + level.ToString());

                // Request security access and extract VIN
                securityAccessGranted = securityManager.RequestSecurityAccess(level, out extractedVIN);

                if (securityAccessGranted)
                {
                    UpdateStatus("Security access granted");
                    LogMessage("Security access granted successfully");

                    // Update the VIN tab with the extracted VIN
                    lblExtractedVIN.Text = !string.IsNullOrEmpty(extractedVIN) ?
                        extractedVIN : "[VIN could not be extracted]";

                    // Enable the Next button
                    btnNext2.Enabled = true;
                }
                else
                {
                    UpdateStatus("Security access failed");
                    LogMessage("Failed to obtain security access");
                    MessageBox.Show("Failed to obtain security access. Please check your connection and try again.",
                                   "Security Access Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Error during security access");
                LogMessage("Security access error: " + ex.Message);
                MessageBox.Show("Error during security access: " + ex.Message,
                               "Security Access Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNext2_Click(object sender, EventArgs e)
        {
            // Move to the VIN validation tab
            tabControl.SelectedIndex = 2;
        }

        private void btnConfirmVIN_Click(object sender, EventArgs e)
        {
            // Validate VIN match if the user entered a VIN
            if (!string.IsNullOrEmpty(txtManualVIN.Text))
            {
                if (string.IsNullOrEmpty(extractedVIN))
                {
                    // If we couldn't extract a VIN, just accept the user's input
                    LogMessage("No VIN was extracted. Proceeding with user-provided VIN: " + txtManualVIN.Text);
                }
                else if (txtManualVIN.Text.Trim() != extractedVIN)
                {
                    // VINs don't match - warn the user
                    DialogResult result = MessageBox.Show(
                        "The VIN you entered does not match the VIN extracted from the vehicle.\n\n" +
                        "Extracted: " + extractedVIN + "\n" +
                        "Entered: " + txtManualVIN.Text + "\n\n" +
                        "This might indicate you're not connected to the intended vehicle.\n" +
                        "Do you still want to proceed?",
                        "VIN Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                    {
                        return;
                    }

                    LogMessage("VIN mismatch acknowledged by user");
                }
                else
                {
                    LogMessage("VIN match confirmed: " + extractedVIN);
                }
            }

            // Proceed to the diagnostics tab
            tabControl.SelectedIndex = 3;
            UpdateStatus("Ready for diagnostics");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Go back to the security access tab
            tabControl.SelectedIndex = 1;
        }

        private void btnReadDTCs_Click(object sender, EventArgs e)
        {
            if (!securityAccessGranted)
            {
                MessageBox.Show("Security access must be granted before reading DTCs",
                               "Security Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdateStatus("Reading DTCs...");
            LogMessage("Reading Diagnostic Trouble Codes...");

            // Implementation for reading DTCs would go here
            // This would use diagnostic service 0x19 (Read DTC Information)
        }

        private void btnClearDTCs_Click(object sender, EventArgs e)
        {
            if (!securityAccessGranted)
            {
                MessageBox.Show("Security access must be granted before clearing DTCs",
                               "Security Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to clear all Diagnostic Trouble Codes?",
                "Confirm Clear DTCs", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                UpdateStatus("Clearing DTCs...");
                LogMessage("Clearing Diagnostic Trouble Codes...");

                // Implementation for clearing DTCs would go here
                // This would use diagnostic service 0x14 (Clear DTC Information)
            }
        }

        private void btnReadData_Click(object sender, EventArgs e)
        {
            if (!securityAccessGranted)
            {
                MessageBox.Show("Security access must be granted before reading data",
                               "Security Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdateStatus("Reading data...");
            LogMessage("Reading live data parameters...");

            // Implementation for reading data would go here
            // This would use diagnostic service 0x22 (Read Data By Identifier)
        }

        private void UpdateStatus(string status)
        {
            // Thread-safe update of status bar
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), status);
                return;
            }

            lblStatus.Text = status;
            Application.DoEvents();
        }

        private void LogMessage(string message)
        {
            // Thread-safe update of log
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogMessage), message);
                return;
            }

            txtLog.AppendText(DateTime.Now.ToString("HH:mm:ss") + " - " + message + Environment.NewLine);
            txtLog.ScrollToCaret();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Clean up resources
            if (keepAliveTimer != null)
            {
                keepAliveTimer.Stop();
                keepAliveTimer.Dispose();
            }

            if (serialComm != null && serialComm.IsOpen)
            {
                serialComm.Close();
            }

            base.OnFormClosing(e);
        }
    }
}