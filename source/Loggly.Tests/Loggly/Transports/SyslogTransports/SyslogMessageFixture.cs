using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loggly.Transports.Syslog;
using Moq;
using NUnit.Framework;

namespace Loggly.Tests.Loggly.Transports.SyslogTransports
{
    public class SyslogMessageFixture : Fixture
    {
        [Test]
        public void MessageString()
        {
            var syslog = new SyslogMessage();

            var environmentProvider = new Mock<IEnvironmentProvider>();
            environmentProvider.Setup(p => p.MachineName).Returns("server1.com");
            environmentProvider.Setup(p => p.ProcessId).Returns(900);

            syslog.EnvironmentProvider = environmentProvider.Object;
            syslog.Timestamp = DateTimeOffset.Parse("2003-10-11T22:14:15.003Z");
            syslog.Text = "[8bf8cc10-4140-4c3e-a2b4-e6f5324f1aea@41058]";
            syslog.Facility = Facility.Auth;
            syslog.Level = Level.Critical;
            syslog.AppName = "sudo";
            syslog.MessageId = 31;

            var messageString = syslog.GetMessageAsString();
            Console.WriteLine(messageString);
            Assert.AreEqual("<34>1 2003-10-11T22:14:15.003000+00:00 server1.com sudo 900 31 [8bf8cc10-4140-4c3e-a2b4-e6f5324f1aea@41058]", messageString);
            //loggly example Assert.AreEqual("<34>1 2003-10-11T22:14:15.003Z server1.com sudo - - [8bf8cc10-4140-4c3e-a2b4-e6f5324f1aea@41058]", messageString);
        }
    }
}
