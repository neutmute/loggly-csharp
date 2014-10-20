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
        ILogglyClient _loggly = new LogglyClient();

        private SearchResponse _searchResponse;
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnPlainText_Click(object sender, EventArgs e)
        {
            _loggly.Log("Simple message at {0} using {1}", DateTime.Now, LogglyConfig.Instance.MessageTransport);
        }

        private void btnPlainWithCallback_Click(object sender, EventArgs e)
        {
            var options = new MessageOptions {Callback = lr => Debug.WriteLine(lr)};
            _loggly.Log(options, "Simple message at {0} with callback using {1}", DateTime.Now, LogglyConfig.Instance.MessageTransport);
        }

        private void btnSendJson_Click(object sender, EventArgs e)
        {
            _loggly.Log(new LogObject());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchClient = new LogglySearchClient();
            _searchResponse = searchClient.Search(new SearchQuery { Query = "mysimpletag" });
            txtSearchResult.Text = _searchResponse.ToString();
        }

        private void btnSearchEvents_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            foreach (EventMessage m in _searchResponse)
            {
                sb.Append(m);
            }
            txtSearchResult.Text = sb.ToString();
        }

    }
}
