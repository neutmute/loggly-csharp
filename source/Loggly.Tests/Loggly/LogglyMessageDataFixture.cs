using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Loggly.Tests.Loggly
{
    public class LogglyMessageDataFixture : Fixture
    {
        [Test]
        public void AddSafeIgnoresDuplicates()
        {
            var data = new MessageData();
            data.AddSafe("myKey", "data1");
            data.AddSafe("myKey", "data2");
            Assert.That((string)data["myKey"] == "data1");
        }
    }
}
