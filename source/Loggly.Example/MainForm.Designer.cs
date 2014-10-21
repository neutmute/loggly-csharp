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
            this.btnPlainText = new System.Windows.Forms.Button();
            this.btnPlainWithCallback = new System.Windows.Forms.Button();
            this.btnSendJson = new System.Windows.Forms.Button();
            this.groupBoxSearch = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearchResult = new System.Windows.Forms.TextBox();
            this.btnSearchEvents = new System.Windows.Forms.Button();
            this.groupBoxSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPlainText
            // 
            this.btnPlainText.Location = new System.Drawing.Point(12, 21);
            this.btnPlainText.Name = "btnPlainText";
            this.btnPlainText.Size = new System.Drawing.Size(173, 23);
            this.btnPlainText.TabIndex = 0;
            this.btnPlainText.Text = "Plain text message";
            this.btnPlainText.UseVisualStyleBackColor = true;
            this.btnPlainText.Click += new System.EventHandler(this.btnPlainText_Click);
            // 
            // btnPlainWithCallback
            // 
            this.btnPlainWithCallback.Location = new System.Drawing.Point(12, 50);
            this.btnPlainWithCallback.Name = "btnPlainWithCallback";
            this.btnPlainWithCallback.Size = new System.Drawing.Size(173, 23);
            this.btnPlainWithCallback.TabIndex = 1;
            this.btnPlainWithCallback.Text = "Plain text message with callback";
            this.btnPlainWithCallback.UseVisualStyleBackColor = true;
            this.btnPlainWithCallback.Click += new System.EventHandler(this.btnPlainWithCallback_Click);
            // 
            // btnSendJson
            // 
            this.btnSendJson.Location = new System.Drawing.Point(12, 79);
            this.btnSendJson.Name = "btnSendJson";
            this.btnSendJson.Size = new System.Drawing.Size(114, 23);
            this.btnSendJson.TabIndex = 2;
            this.btnSendJson.Text = "Send JSON";
            this.btnSendJson.UseVisualStyleBackColor = true;
            this.btnSendJson.Click += new System.EventHandler(this.btnSendJson_Click);
            // 
            // groupBoxSearch
            // 
            this.groupBoxSearch.Controls.Add(this.btnSearchEvents);
            this.groupBoxSearch.Controls.Add(this.txtSearchResult);
            this.groupBoxSearch.Controls.Add(this.btnSearch);
            this.groupBoxSearch.Location = new System.Drawing.Point(305, 2);
            this.groupBoxSearch.Name = "groupBoxSearch";
            this.groupBoxSearch.Size = new System.Drawing.Size(350, 379);
            this.groupBoxSearch.TabIndex = 4;
            this.groupBoxSearch.TabStop = false;
            this.groupBoxSearch.Text = "Search";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(15, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(178, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearchResult
            // 
            this.txtSearchResult.Location = new System.Drawing.Point(15, 50);
            this.txtSearchResult.Multiline = true;
            this.txtSearchResult.Name = "txtSearchResult";
            this.txtSearchResult.Size = new System.Drawing.Size(178, 87);
            this.txtSearchResult.TabIndex = 5;
            // 
            // btnSearchEvents
            // 
            this.btnSearchEvents.Location = new System.Drawing.Point(15, 158);
            this.btnSearchEvents.Name = "btnSearchEvents";
            this.btnSearchEvents.Size = new System.Drawing.Size(178, 23);
            this.btnSearchEvents.TabIndex = 6;
            this.btnSearchEvents.Text = "Find Events";
            this.btnSearchEvents.UseVisualStyleBackColor = true;
            this.btnSearchEvents.Click += new System.EventHandler(this.btnSearchEvents_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 380);
            this.Controls.Add(this.groupBoxSearch);
            this.Controls.Add(this.btnSendJson);
            this.Controls.Add(this.btnPlainWithCallback);
            this.Controls.Add(this.btnPlainText);
            this.Name = "MainForm";
            this.Text = "Loggly Example Application";
            this.groupBoxSearch.ResumeLayout(false);
            this.groupBoxSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPlainText;
        private System.Windows.Forms.Button btnPlainWithCallback;
        private System.Windows.Forms.Button btnSendJson;
        private System.Windows.Forms.GroupBox groupBoxSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearchResult;
        private System.Windows.Forms.Button btnSearchEvents;
    }
}

