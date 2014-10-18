using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loggly.Config;
using NUnit;
using NUnit.Framework;

namespace Loggly.Tests.Config.ComplexTags
{
    [TestFixture]
    public class HostnameTagFixture 
    {
        [Test]
        public void FormattedValue()
        {
            var tag = new HostnameTag();
            tag.Formatter = "machine={0}";
            Assert.That(tag.FormattedValue.StartsWith("machine"));
            Assert.That(tag.FormattedValue.EndsWith(Environment.MachineName));
        }
    }
}
