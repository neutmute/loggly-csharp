using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loggly.Config;
using NUnit.Framework;

namespace Loggly.Tests.Loggly.Config
{
    public class TagConfigurationFixture : Fixture
    {
        /// <summary>
        /// https://www.loggly.com/docs/tags/
        /// </summary>
        [Test]
        public void Conform()
        {
            var tags = new List<string>();

            tags.Add("legalTag");
            tags.Add("_my_little.pony");
            tags.Add("-us");
            tags.Add("apache$");

            TagConfiguration.CoerceLegalTags(tags);

            Assert.AreEqual("legalTag", tags[0]);
            Assert.AreEqual("z_my_little.pony", tags[1]);
            Assert.AreEqual("z-us", tags[2]);
            Assert.AreEqual("apache_", tags[3]);

        }
    }
}
