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
            this.groupBoxSync = new System.Windows.Forms.GroupBox();
            this.btnSendJson = new System.Windows.Forms.Button();
            this.btnPlainWithCallback = new System.Windows.Forms.Button();
            this.btnPlainText = new System.Windows.Forms.Button();
            this.groupBoxSearch.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBoxSend.SuspendLayout();
            this.groupBoxSync.SuspendLayout();
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
            this.groupBoxAsync.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxAsync.Location = new System.Drawing.Point(3, 138);
            this.groupBoxAsync.Name = "groupBoxAsync";
            this.groupBoxAsync.Size = new System.Drawing.Size(194, 161);
            this.groupBoxAsync.TabIndex = 7;
            this.groupBoxAsync.TabStop = false;
            this.groupBoxAsync.Text = "Asynchronous";
            // 
            // groupBoxSync
            // 
            this.groupBoxSync.Controls.Add(this.btnSendJson);
            this.groupBoxSync.Controls.Add(this.btnPlainWithCallback);
            this.groupBoxSync.Controls.Add(this.btnPlainText);
            this.groupBoxSync.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSync.Location = new System.Drawing.Point(3, 16);
            this.groupBoxSync.Name = "groupBoxSync";
            this.groupBoxSync.Size = new System.Drawing.Size(194, 122);
            this.groupBoxSync.TabIndex = 6;
            this.groupBoxSync.TabStop = false;
            this.groupBoxSync.Text = "Synchronous";
            // 
            // btnSendJson
            // 
            this.btnSendJson.Location = new System.Drawing.Point(9, 77);
            this.btnSendJson.Name = "btnSendJson";
            this.btnSendJson.Size = new System.Drawing.Size(173, 23);
            this.btnSendJson.TabIndex = 8;
            this.btnSendJson.Text = "Object as JSON";
            this.btnSendJson.UseVisualStyleBackColor = true;
            this.btnSendJson.Click += new System.EventHandler(this.btnSendJson_Click);
            // 
            // btnPlainWithCallback
            // 
            this.btnPlainWithCallback.Location = new System.Drawing.Point(9, 48);
            this.btnPlainWithCallback.Name = "btnPlainWithCallback";
            this.btnPlainWithCallback.Size = new System.Drawing.Size(173, 23);
            this.btnPlainWithCallback.TabIndex = 7;
            this.btnPlainWithCallback.Text = "Plain text message with callback";
            this.btnPlainWithCallback.UseVisualStyleBackColor = true;
            this.btnPlainWithCallback.Click += new System.EventHandler(this.btnPlainWithCallback_Click);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 576);
            this.Controls.Add(this.groupBoxSend);
            this.Controls.Add(this.groupBoxSearch);
            this.Name = "MainForm";
            this.Text = "Loggly Example Application";
            this.groupBoxSearch.ResumeLayout(false);
            this.groupBoxSearch.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBoxSend.ResumeLayout(false);
            this.groupBoxSync.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnSendJson;
        private System.Windows.Forms.Button btnPlainWithCallback;
        private System.Windows.Forms.Button btnPlainText;
    }
}

