using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Loggly.Tests.Loggly.ExtensionMethods
{
    public class DateTimeExtensionsFixture : Fixture
    {
        /// <summary>
        /// https://www.loggly.com/docs/automated-parsing/#json
        /// </summary>
        [Test]
        public void ToJsonIso8601()
        {
            var date = new DateTime(2013, 10, 11, 22, 14, 15, 3);
            Assert.AreEqual("2013-10-11T22:14:15.003000Z", date.ToJsonIso8601());
        }

        /// <summary>
        /// https://www.loggly.com/docs/tags/
        /// </summary>
        [Test]
        public void ToSyslog()
        {
            var date = new DateTime(2013, 10, 11, 22, 14, 15, 3);

            // Make this work outside of my timezone
            var syslogForm = date.ToSyslog();
            Assert.AreEqual(32, syslogForm.Length);
            Assert.That(syslogForm.StartsWith("2013-10-11T22:14:15.003000+"));
            Assert.That(syslogForm.EndsWith(":00"));

            Console.WriteLine(DateTime.UtcNow.ToSyslog());
        }

        [Test]
        public void DateTimeOffsetToSyslog()
        {
            var date = new DateTimeOffset(2013, 10, 11, 22, 14, 15, 3, TimeSpan.FromHours(10));
            Assert.AreEqual("2013-10-11T22:14:15.003000+10:00", date.ToSyslog());
            Console.WriteLine(DateTime.UtcNow.ToSyslog());
        }
    }
}
