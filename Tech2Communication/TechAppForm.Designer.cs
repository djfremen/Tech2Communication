namespace Tech2Communication
{
    partial class TechAppForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnNext1 = new System.Windows.Forms.Button();
            this.cboStopBits = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboParity = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabSecurity = new System.Windows.Forms.TabPage();
            this.btnGetSeed = new System.Windows.Forms.Button();
            this.btnNext2 = new System.Windows.Forms.Button();
            this.cboAccessLevel = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabVIN = new System.Windows.Forms.TabPage();
            this.lblExtractedVIN = new System.Windows.Forms.Label();
            this.btnConfirmVIN = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtManualVIN = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabDiagnostics = new System.Windows.Forms.TabPage();
            this.btnReadDTCs = new System.Windows.Forms.Button();
            this.btnClearDTCs = new System.Windows.Forms.Button();
            this.btnReadData = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabSecurity.SuspendLayout();
            this.tabVIN.SuspendLayout();
            this.tabDiagnostics.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabSettings);
            this.tabControl.Controls.Add(this.tabSecurity);
            this.tabControl.Controls.Add(this.tabVIN);
            this.tabControl.Controls.Add(this.tabDiagnostics);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(584, 361);
            this.tabControl.TabIndex = 0;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.btnConnect);
            this.tabSettings.Controls.Add(this.btnNext1);
            this.tabSettings.Controls.Add(this.cboStopBits);
            this.tabSettings.Controls.Add(this.label4);
            this.tabSettings.Controls.Add(this.cboParity);
            this.tabSettings.Controls.Add(this.label3);
            this.tabSettings.Controls.Add(this.cboBaudRate);
            this.tabSettings.Controls.Add(this.label2);
            this.tabSettings.Controls.Add(this.cboPort);
            this.tabSettings.Controls.Add(this.label1);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(576, 335);
            this.tabSettings.TabIndex = 0;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(116, 199);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(121, 23);
            this.btnConnect.TabIndex = 9;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnNext1
            // 
            this.btnNext1.Enabled = false;
            this.btnNext1.Location = new System.Drawing.Point(463, 306);
            this.btnNext1.Name = "btnNext1";
            this.btnNext1.Size = new System.Drawing.Size(107, 23);
            this.btnNext1.TabIndex = 8;
            this.btnNext1.Text = "Next >>";
            this.btnNext1.UseVisualStyleBackColor = true;
            this.btnNext1.Click += new System.EventHandler(this.btnNext1_Click);
            // 
            // cboStopBits
            // 
            this.cboStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStopBits.FormattingEnabled = true;
            this.cboStopBits.Location = new System.Drawing.Point(116, 156);
            this.cboStopBits.Name = "cboStopBits";
            this.cboStopBits.Size = new System.Drawing.Size(121, 21);
            this.cboStopBits.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Stop Bits";
            // 
            // cboParity
            // 
            this.cboParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParity.FormattingEnabled = true;
            this.cboParity.Location = new System.Drawing.Point(116, 118);
            this.cboParity.Name = "cboParity";
            this.cboParity.Size = new System.Drawing.Size(121, 21);
            this.cboParity.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Parity";
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBaudRate.FormattingEnabled = true;
            this.cboBaudRate.Location = new System.Drawing.Point(116, 78);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(121, 21);
            this.cboBaudRate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baud Rate";
            // 
            // cboPort
            // 
            this.cboPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPort.FormattingEnabled = true;
            this.cboPort.Location = new System.Drawing.Point(116, 37);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(121, 21);
            this.cboPort.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port";
            // 
            // tabSecurity
            // 
            this.tabSecurity.Controls.Add(this.btnGetSeed);
            this.tabSecurity.Controls.Add(this.btnNext2);
            this.tabSecurity.Controls.Add(this.cboAccessLevel);
            this.tabSecurity.Controls.Add(this.label5);
            this.tabSecurity.Location = new System.Drawing.Point(4, 22);
            this.tabSecurity.Name = "tabSecurity";
            this.tabSecurity.Padding = new System.Windows.Forms.Padding(3);
            this.tabSecurity.Size = new System.Drawing.Size(576, 335);
            this.tabSecurity.TabIndex = 1;
            this.tabSecurity.Text = "Security Access";
            this.tabSecurity.UseVisualStyleBackColor = true;
            // 
            // btnGetSeed
            // 
            this.btnGetSeed.Location = new System.Drawing.Point(116, 92);
            this.btnGetSeed.Name = "btnGetSeed";
            this.btnGetSeed.Size = new System.Drawing.Size(121, 23);
            this.btnGetSeed.TabIndex = 10;
            this.btnGetSeed.Text = "Request Seed";
            this.btnGetSeed.UseVisualStyleBackColor = true;
            this.btnGetSeed.Click += new System.EventHandler(this.btnGetSeed_Click);
            // 
            // btnNext2
            // 
            this.btnNext2.Enabled = false;
            this.btnNext2.Location = new System.Drawing.Point(463, 306);
            this.btnNext2.Name = "btnNext2";
            this.btnNext2.Size = new System.Drawing.Size(107, 23);
            this.btnNext2.TabIndex = 9;
            this.btnNext2.Text = "Next >>";
            this.btnNext2.UseVisualStyleBackColor = true;
            this.btnNext2.Click += new System.EventHandler(this.btnNext2_Click);
            // 
            // cboAccessLevel
            // 
            this.cboAccessLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAccessLevel.FormattingEnabled = true;
            this.cboAccessLevel.Location = new System.Drawing.Point(116, 50);
            this.cboAccessLevel.Name = "cboAccessLevel";
            this.cboAccessLevel.Size = new System.Drawing.Size(121, 21);
            this.cboAccessLevel.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Access Level";
            // 
            // tabVIN
            // 
            this.tabVIN.Controls.Add(this.lblExtractedVIN);
            this.tabVIN.Controls.Add(this.btnConfirmVIN);
            this.tabVIN.Controls.Add(this.btnCancel);
            this.tabVIN.Controls.Add(this.txtManualVIN);
            this.tabVIN.Controls.Add(this.label7);
            this.tabVIN.Controls.Add(this.label6);
            this.tabVIN.Location = new System.Drawing.Point(4, 22);
            this.tabVIN.Name = "tabVIN";
            this.tabVIN.Size = new System.Drawing.Size(576, 335);
            this.tabVIN.TabIndex = 2;
            this.tabVIN.Text = "VIN Validation";
            this.tabVIN.UseVisualStyleBackColor = true;
            // 
            // lblExtractedVIN
            // 
            this.lblExtractedVIN.AutoSize = true;
            this.lblExtractedVIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractedVIN.Location = new System.Drawing.Point(112, 55);
            this.lblExtractedVIN.Name = "lblExtractedVIN";
            this.lblExtractedVIN.Size = new System.Drawing.Size(230, 20);
            this.lblExtractedVIN.TabIndex = 5;
            this.lblExtractedVIN.Text = "[VIN will appear here]";
            // 
            // btnConfirmVIN
            // 
            this.btnConfirmVIN.Location = new System.Drawing.Point(463, 306);
            this.btnConfirmVIN.Name = "btnConfirmVIN";
            this.btnConfirmVIN.Size = new System.Drawing.Size(107, 23);
            this.btnConfirmVIN.TabIndex = 4;
            this.btnConfirmVIN.Text = "Confirm && Continue";
            this.btnConfirmVIN.UseVisualStyleBackColor = true;
            this.btnConfirmVIN.Click += new System.EventHandler(this.btnConfirmVIN_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(350, 306);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtManualVIN
            // 
            this.txtManualVIN.Location = new System.Drawing.Point(176, 141);
            this.txtManualVIN.MaxLength = 17;
            this.txtManualVIN.Name = "txtManualVIN";
            this.txtManualVIN.Size = new System.Drawing.Size(211, 20);
            this.txtManualVIN.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Enter Vehicle VIN to Confirm:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Extracted VIN:";
            // 
            // tabDiagnostics
            // 
            this.tabDiagnostics.Controls.Add(this.btnReadDTCs);
            this.tabDiagnostics.Controls.Add(this.btnClearDTCs);
            this.tabDiagnostics.Controls.Add(this.btnReadData);
            this.tabDiagnostics.Controls.Add(this.txtLog);
            this.tabDiagnostics.Location = new System.Drawing.Point(4, 22);
            this.tabDiagnostics.Name = "tabDiagnostics";
            this.tabDiagnostics.Size = new System.Drawing.Size(576, 335);
            this.tabDiagnostics.TabIndex = 3;
            this.tabDiagnostics.Text = "Diagnostics";
            this.tabDiagnostics.UseVisualStyleBackColor = true;
            // 
            // btnReadDTCs
            // 
            this.btnReadDTCs.Location = new System.Drawing.Point(27, 19);
            this.btnReadDTCs.Name = "btnReadDTCs";
            this.btnReadDTCs.Size = new System.Drawing.Size(107, 23);
            this.btnReadDTCs.TabIndex = 0;
            this.btnReadDTCs.Text = "Read DTCs";
            this.btnReadDTCs.UseVisualStyleBackColor = true;
            this.btnReadDTCs.Click += new System.EventHandler(this.btnReadDTCs_Click);
            // 
            // btnClearDTCs
            // 
            this.btnClearDTCs.Location = new System.Drawing.Point(140, 19);
            this.btnClearDTCs.Name = "btnClearDTCs";
            this.btnClearDTCs.Size = new System.Drawing.Size(107, 23);
            this.btnClearDTCs.TabIndex = 1;
            this.btnClearDTCs.Text = "Clear DTCs";
            this.btnClearDTCs.UseVisualStyleBackColor = true;
            this.btnClearDTCs.Click += new System.EventHandler(this.btnClearDTCs_Click);
            // 
            // btnReadData
            // 
            this.btnReadData.Location = new System.Drawing.Point(253, 19);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(107, 23);
            this.btnReadData.TabIndex = 2;
            this.btnReadData.Text = "Read Data";
            this.btnReadData.UseVisualStyleBackColor = true;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(8, 57);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(560, 270);
            this.txtLog.TabIndex = 3;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 339);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(584, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // TechAppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "TechAppForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tech2 Communication Tool";
            this.tabControl.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.tabSecurity.ResumeLayout(false);
            this.tabSecurity.PerformLayout();
            this.tabVIN.ResumeLayout(false);
            this.tabVIN.PerformLayout();
            this.tabDiagnostics.ResumeLayout(false);
            this.tabDiagnostics.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tabSecurity;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnNext1;
        private System.Windows.Forms.ComboBox cboStopBits;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboParity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboBaudRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabVIN;
        private System.Windows.Forms.TabPage tabDiagnostics;
        private System.Windows.Forms.Button btnGetSeed;
        private System.Windows.Forms.Button btnNext2;
        private System.Windows.Forms.ComboBox cboAccessLevel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblExtractedVIN;
        private System.Windows.Forms.Button btnConfirmVIN;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtManualVIN;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnReadDTCs;
        private System.Windows.Forms.Button btnClearDTCs;
        private System.Windows.Forms.Button btnReadData;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}