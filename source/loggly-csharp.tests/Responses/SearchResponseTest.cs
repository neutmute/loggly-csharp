using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loggly.Responses;
using NUnit.Framework;
using Newtonsoft.Json;

namespace Loggly.Tests.Responses
{
    public class SearchResponseTest
    {
        [Test]
        public void SerializeTest()
        {
            string json =
                "{\"rsid\": {\"status\": \"SCHEDULED\",\"date_from\": 1379706043000,\"elapsed_time\": 0.017975807189941406,\"date_to\": 1380570043000, \"id\": \"1910175565\"} }";

            SearchResponse actualResponse = JsonConvert.DeserializeObject<SearchResponse>(json);

            Assert.IsNotNull(actualResponse);
            Assert.IsNotNull(actualResponse.RSID);
            Assert.AreEqual("SCHEDULED", actualResponse.RSID.Status);
            Assert.AreEqual(1379706043000, actualResponse.RSID.From);
            Assert.AreEqual(0.017975807189941406, actualResponse.RSID.ElapsedTime);
            Assert.AreEqual(1380570043000, actualResponse.RSID.To);
            Assert.AreEqual("1910175565", actualResponse.RSID.Id);
        }
    }
}
