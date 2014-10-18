using System;
using System.Linq;
using NUnit.Framework;

namespace Loggly.Tests
{
    public class SearchTests : BaseFixture
    {
        [Test]
        public void SendsASimpleSearchRequest()
        {
            string responseJson =
                "{\"rsid\": {\"status\": \"SCHEDULED\",\"date_from\": 1379706043000,\"elapsed_time\": 0.017975807189941406,\"date_to\": 1380570043000, \"id\": \"1910175565\"} }";

            Server.Stub(new ApiExpectation { Method = "GET", Url = "/apiv2/search", QueryString = "?q=abc+123", Response = responseJson });
            new Searcher("mogade").Search("abc 123");
        }

        [Test]
        public void ProperlySerializesTimes()
        {
            string responseJson =
    "{\"rsid\": {\"status\": \"SCHEDULED\",\"date_from\": 1379706043000,\"elapsed_time\": 0.017975807189941406,\"date_to\": 1380570043000, \"id\": \"1910175565\"} }";

            Server.Stub(new ApiExpectation { Method = "GET", Url = "/apiv2/search", QueryString = "?q=NewQuery&from=2001-10-20T05%3a35%3a22.000Z", Response = responseJson});
            new Searcher("mogade").Search(new SearchQuery { Query = "NewQuery", From = new DateTime(2001, 10, 20, 5, 35, 22, DateTimeKind.Utc) });
        }

        [Test]
        public void GetsTheResponse()
        {
            string responseJson =
                "{\"rsid\": {\"status\": \"SCHEDULED\",\"date_from\": 1379706043000,\"elapsed_time\": 0.017975807189941406,\"date_to\": 1380570043000, \"id\": \"1910175565\"} }";

            Server.Stub(new ApiExpectation { Response = responseJson });
            var r = new Searcher("mogade").Search("anything");
            Assert.IsNotNull(r);
            Assert.IsNotNull(r.RSID);
            Assert.AreEqual("SCHEDULED", r.RSID.Status);
            Assert.AreEqual(1379706043000, r.RSID.From);
            Assert.AreEqual(0.017975807189941406, r.RSID.ElapsedTime);
            Assert.AreEqual(1380570043000, r.RSID.To);
            Assert.AreEqual("1910175565", r.RSID.Id);
        }
    }
}