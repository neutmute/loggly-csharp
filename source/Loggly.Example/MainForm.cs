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
            _loggly.Log("Simple message at {0} using {1}", DateTime.Now, LogglyConfig.Instance.Transport.LogTransport);
        }

        private void btnPlainWithCallback_Click(object sender, EventArgs e)
        {
            var options = new MessageOptions {Callback = lr => Debug.WriteLine(lr)};
            _loggly.Log(options, "Simple message at {0} with callback using {1}", DateTime.Now, LogglyConfig.Instance.Transport.LogTransport);
        }

        private void btnSendJson_Click(object sender, EventArgs e)
        {
            _loggly.Log(new LogObject());
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



    }
}
