using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loggly.Config;
using Loggly.Transports.Syslog;
using NUnit.Framework;

namespace Loggly.Tests.Loggly.Transports.SyslogTransports
{
    public class SyslogTransportBaseFixture : Fixture
    {
        [SetUp]
        public void Setup()
        {
            LogglyConfig.Instance.Tags.SimpleTags.Add(new SimpleTag {Value = "myTag"});
            LogglyConfig.Instance.CustomerToken = "MyLogglyToken";
        }

        [Test]
        public void SyslogContentWhenNoTags()
        {
            LogglyConfig.Instance.Tags.SimpleTags.Clear();

            var transport = new SyslogTcpTransport();
            var logglyMessage = new LogglyMessage();

            logglyMessage.Content = "myContent";

            var syslog = transport.ConstructSyslog(logglyMessage);

            Assert.AreEqual("[MyLogglyToken@41058] myContent", syslog.Text);
        }

        [Test]
        public void SyslogContentWithTags()
        {
            LogglyConfig.Instance.CustomerToken = "MyLogglyToken";

            var transport = new SyslogTcpTransport();
            var logglyMessage = new LogglyMessage();

            logglyMessage.Content = "myContent";

            var syslog = transport.ConstructSyslog(logglyMessage);

            Assert.AreEqual("[MyLogglyToken@41058 tag=\"myTag\"] myContent", syslog.Text);
        }
    }
}
