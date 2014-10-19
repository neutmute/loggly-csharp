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
using Loggly.Responses;

namespace Loggly.Example
{
    public partial class MainForm : Form
    {
        ILogglyClient _loggly = new LogglyClient();
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnPlainText_Click(object sender, EventArgs e)
        {
            _loggly.Log("Simple message at {0}", DateTime.Now);
        }

        private void btnPlainWithCallback_Click(object sender, EventArgs e)
        {
            Action<LogResponse> callback = lr =>
            {
                Debug.WriteLine(lr);
            };
            _loggly.Log(callback, "Simple message at {0} with callback", DateTime.Now);
        }

        private void btnSendJson_Click(object sender, EventArgs e)
        {
            _loggly.Log(new LogObject());
        }
    }
}
