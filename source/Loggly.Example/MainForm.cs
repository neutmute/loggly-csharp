using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Loggly.Config;
using Loggly.Responses;

namespace Loggly.Example
{
    public partial class MainForm : Form
    {

        readonly LogglyExample _logglyExample = new LogglyExample();

        private SearchResponse _searchResponse;
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnPlainText_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            using (new WaitCursor(this))
            {
                _logglyExample.SendPlainMessageSynchronous();
            }
        }

        private void btnPlainAsync_Click(object sender, EventArgs e)
        {
            using (new WaitCursor(this))
            {
               _logglyExample.SendAsync();
            }
        }

        private void btnSendJson_Click(object sender, EventArgs e)
        {
            using (new WaitCursor(this))
            {
                #pragma warning disable 4014
                _logglyExample.SendCustomObjectAsync();
                #pragma warning restore 4014
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchClient = new LogglySearchClient();

            var query = new SearchQuery();
            query.Query = "*";
            query.From = DateTime.Now.AddDays(-2);
            query.Until = DateTime.Now;
            query.Size = 2;

            _searchResponse = searchClient.Search(query);
            txtSearchResult.Text = _searchResponse.ToString();
        }

        private void btnSearchEvents_Click(object sender, EventArgs e)
        {
            var i = 3;
            foreach (EventMessage m in _searchResponse)
            {
                txtSearchResult.Text += "\r\n\r\n" + m;
                if (i++ > 3)
                {
                    break;
                }
            }
        }

        private void groupBoxTransport_Enter(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            radTransportHttps.Checked = true;
        }

        private void btnForcedTransport_Click(object sender, EventArgs e)
        {
            var logTransport = LogTransport.Https;
            if (radTransportSyslogUdp.Checked)
            {
                logTransport = LogTransport.SyslogUdp;
            }
            if (radTransportSyslogSecure.Checked)
            {
                logTransport = LogTransport.SyslogSecure;
            }

            _logglyExample.SendWithSpecificTransport(logTransport);
        }
    }
}
