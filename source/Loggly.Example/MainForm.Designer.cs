namespace Loggly.Example
{
    partial class MainForm
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
            this.groupBoxSearch = new System.Windows.Forms.GroupBox();
            this.txtSearchResult = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearchEvents = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBoxSend = new System.Windows.Forms.GroupBox();
            this.groupBoxAsync = new System.Windows.Forms.GroupBox();
            this.btnSendJson = new System.Windows.Forms.Button();
            this.btnPlainAsync = new System.Windows.Forms.Button();
            this.groupBoxSync = new System.Windows.Forms.GroupBox();
            this.btnPlainText = new System.Windows.Forms.Button();
            this.groupBoxTransport = new System.Windows.Forms.GroupBox();
            this.radTransportHttps = new System.Windows.Forms.RadioButton();
            this.radTransportSyslogUdp = new System.Windows.Forms.RadioButton();
            this.radTransportSyslogSecure = new System.Windows.Forms.RadioButton();
            this.btnForcedTransport = new System.Windows.Forms.Button();
            this.groupBoxSearch.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBoxSend.SuspendLayout();
            this.groupBoxAsync.SuspendLayout();
            this.groupBoxSync.SuspendLayout();
            this.groupBoxTransport.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSearch
            // 
            this.groupBoxSearch.Controls.Add(this.txtSearchResult);
            this.groupBoxSearch.Controls.Add(this.panel1);
            this.groupBoxSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBoxSearch.Location = new System.Drawing.Point(206, 0);
            this.groupBoxSearch.Name = "groupBoxSearch";
            this.groupBoxSearch.Size = new System.Drawing.Size(553, 576);
            this.groupBoxSearch.TabIndex = 4;
            this.groupBoxSearch.TabStop = false;
            this.groupBoxSearch.Text = "Search";
            // 
            // txtSearchResult
            // 
            this.txtSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchResult.Location = new System.Drawing.Point(3, 52);
            this.txtSearchResult.Multiline = true;
            this.txtSearchResult.Name = "txtSearchResult";
            this.txtSearchResult.Size = new System.Drawing.Size(547, 521);
            this.txtSearchResult.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSearchEvents);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(547, 36);
            this.panel1.TabIndex = 7;
            // 
            // btnSearchEvents
            // 
            this.btnSearchEvents.Location = new System.Drawing.Point(155, 3);
            this.btnSearchEvents.Name = "btnSearchEvents";
            this.btnSearchEvents.Size = new System.Drawing.Size(186, 23);
            this.btnSearchEvents.TabIndex = 8;
            this.btnSearchEvents.Text = "Find Events";
            this.btnSearchEvents.UseVisualStyleBackColor = true;
            this.btnSearchEvents.Click += new System.EventHandler(this.btnSearchEvents_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(146, 23);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBoxSend
            // 
            this.groupBoxSend.Controls.Add(this.groupBoxTransport);
            this.groupBoxSend.Controls.Add(this.groupBoxAsync);
            this.groupBoxSend.Controls.Add(this.groupBoxSync);
            this.groupBoxSend.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBoxSend.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSend.Name = "groupBoxSend";
            this.groupBoxSend.Size = new System.Drawing.Size(200, 576);
            this.groupBoxSend.TabIndex = 5;
            this.groupBoxSend.TabStop = false;
            this.groupBoxSend.Text = "Send";
            // 
            // groupBoxAsync
            // 
            this.groupBoxAsync.Controls.Add(this.btnSendJson);
            this.groupBoxAsync.Controls.Add(this.btnPlainAsync);
            this.groupBoxAsync.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxAsync.Location = new System.Drawing.Point(3, 78);
            this.groupBoxAsync.Name = "groupBoxAsync";
            this.groupBoxAsync.Size = new System.Drawing.Size(194, 89);
            this.groupBoxAsync.TabIndex = 7;
            this.groupBoxAsync.TabStop = false;
            this.groupBoxAsync.Text = "Asynchronous";
            // 
            // btnSendJson
            // 
            this.btnSendJson.Location = new System.Drawing.Point(6, 48);
            this.btnSendJson.Name = "btnSendJson";
            this.btnSendJson.Size = new System.Drawing.Size(173, 23);
            this.btnSendJson.TabIndex = 10;
            this.btnSendJson.Text = "Object as JSON";
            this.btnSendJson.UseVisualStyleBackColor = true;
            this.btnSendJson.Click += new System.EventHandler(this.btnSendJson_Click);
            // 
            // btnPlainAsync
            // 
            this.btnPlainAsync.Location = new System.Drawing.Point(6, 19);
            this.btnPlainAsync.Name = "btnPlainAsync";
            this.btnPlainAsync.Size = new System.Drawing.Size(173, 23);
            this.btnPlainAsync.TabIndex = 9;
            this.btnPlainAsync.Text = "Plain text message";
            this.btnPlainAsync.UseVisualStyleBackColor = true;
            this.btnPlainAsync.Click += new System.EventHandler(this.btnPlainAsync_Click);
            // 
            // groupBoxSync
            // 
            this.groupBoxSync.Controls.Add(this.btnPlainText);
            this.groupBoxSync.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSync.Location = new System.Drawing.Point(3, 16);
            this.groupBoxSync.Name = "groupBoxSync";
            this.groupBoxSync.Size = new System.Drawing.Size(194, 62);
            this.groupBoxSync.TabIndex = 6;
            this.groupBoxSync.TabStop = false;
            this.groupBoxSync.Text = "Synchronous";
            // 
            // btnPlainText
            // 
            this.btnPlainText.Location = new System.Drawing.Point(9, 19);
            this.btnPlainText.Name = "btnPlainText";
            this.btnPlainText.Size = new System.Drawing.Size(173, 23);
            this.btnPlainText.TabIndex = 6;
            this.btnPlainText.Text = "Plain text message";
            this.btnPlainText.UseVisualStyleBackColor = true;
            this.btnPlainText.Click += new System.EventHandler(this.btnPlainText_Click);
            // 
            // groupBoxTransport
            // 
            this.groupBoxTransport.Controls.Add(this.btnForcedTransport);
            this.groupBoxTransport.Controls.Add(this.radTransportSyslogSecure);
            this.groupBoxTransport.Controls.Add(this.radTransportSyslogUdp);
            this.groupBoxTransport.Controls.Add(this.radTransportHttps);
            this.groupBoxTransport.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxTransport.Location = new System.Drawing.Point(3, 167);
            this.groupBoxTransport.Name = "groupBoxTransport";
            this.groupBoxTransport.Size = new System.Drawing.Size(194, 163);
            this.groupBoxTransport.TabIndex = 8;
            this.groupBoxTransport.TabStop = false;
            this.groupBoxTransport.Text = "Transport";
            this.groupBoxTransport.Enter += new System.EventHandler(this.groupBoxTransport_Enter);
            // 
            // radTransportHttps
            // 
            this.radTransportHttps.AutoSize = true;
            this.radTransportHttps.Location = new System.Drawing.Point(9, 42);
            this.radTransportHttps.Name = "radTransportHttps";
            this.radTransportHttps.Size = new System.Drawing.Size(50, 17);
            this.radTransportHttps.TabIndex = 1;
            this.radTransportHttps.TabStop = true;
            this.radTransportHttps.Text = "Https";
            this.radTransportHttps.UseVisualStyleBackColor = true;
            // 
            // radTransportSyslogUdp
            // 
            this.radTransportSyslogUdp.AutoSize = true;
            this.radTransportSyslogUdp.Location = new System.Drawing.Point(9, 65);
            this.radTransportSyslogUdp.Name = "radTransportSyslogUdp";
            this.radTransportSyslogUdp.Size = new System.Drawing.Size(82, 17);
            this.radTransportSyslogUdp.TabIndex = 2;
            this.radTransportSyslogUdp.TabStop = true;
            this.radTransportSyslogUdp.Text = "Syslog UDP";
            this.radTransportSyslogUdp.UseVisualStyleBackColor = true;
            // 
            // radTransportSyslogSecure
            // 
            this.radTransportSyslogSecure.AutoSize = true;
            this.radTransportSyslogSecure.Location = new System.Drawing.Point(9, 88);
            this.radTransportSyslogSecure.Name = "radTransportSyslogSecure";
            this.radTransportSyslogSecure.Size = new System.Drawing.Size(117, 17);
            this.radTransportSyslogSecure.TabIndex = 3;
            this.radTransportSyslogSecure.TabStop = true;
            this.radTransportSyslogSecure.Text = "Syslog Secure TCP";
            this.radTransportSyslogSecure.UseVisualStyleBackColor = true;
            // 
            // btnForcedTransport
            // 
            this.btnForcedTransport.Location = new System.Drawing.Point(9, 121);
            this.btnForcedTransport.Name = "btnForcedTransport";
            this.btnForcedTransport.Size = new System.Drawing.Size(170, 23);
            this.btnForcedTransport.TabIndex = 4;
            this.btnForcedTransport.Text = "Send message with selected transport";
            this.btnForcedTransport.UseVisualStyleBackColor = true;
            this.btnForcedTransport.Click += new System.EventHandler(this.btnForcedTransport_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 576);
            this.Controls.Add(this.groupBoxSend);
            this.Controls.Add(this.groupBoxSearch);
            this.Name = "MainForm";
            this.Text = "Loggly Example Application";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxSearch.ResumeLayout(false);
            this.groupBoxSearch.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBoxSend.ResumeLayout(false);
            this.groupBoxAsync.ResumeLayout(false);
            this.groupBoxSync.ResumeLayout(false);
            this.groupBoxTransport.ResumeLayout(false);
            this.groupBoxTransport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSearch;
        private System.Windows.Forms.TextBox txtSearchResult;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearchEvents;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBoxSend;
        private System.Windows.Forms.GroupBox groupBoxAsync;
        private System.Windows.Forms.GroupBox groupBoxSync;
        private System.Windows.Forms.Button btnPlainText;
        private System.Windows.Forms.Button btnSendJson;
        private System.Windows.Forms.Button btnPlainAsync;
        private System.Windows.Forms.GroupBox groupBoxTransport;
        private System.Windows.Forms.RadioButton radTransportSyslogSecure;
        private System.Windows.Forms.RadioButton radTransportSyslogUdp;
        private System.Windows.Forms.RadioButton radTransportHttps;
        private System.Windows.Forms.Button btnForcedTransport;
    }
}

