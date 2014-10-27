using System;
using NUnit.Framework;

namespace Loggly.Tests
{
   public class FacetTests : BaseFixture
   {
      [Test]
      public void ProperlySerializesTimes()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/api/facets/date", QueryString = "?q=NewQuery&from=2001-10-20T05%3a35%3a22.000Z" });
         new Facet("mogade").GetDate(new FacetQuery() { Query = "NewQuery", From = new DateTime(2001, 10, 20, 5, 35, 22, DateTimeKind.Utc) });
      }
      [Test]
      public void FacetsByIp()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/api/facets/ip", QueryString = "?q=an+ip+query" });
         new Facet("mogade").GetIp("an ip query");
      }
      [Test]
      public void FacetsByDate()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/api/facets/date", QueryString = "?q=a+date+query" });
         new Facet("mogade").GetDate("a date query");
      }
      [Test]
      public void FacetsByInput()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/api/facets/input", QueryString = "?q=an+input+query" });
         new Facet("mogade").GetInput("an input query");
      }
      [Test]
      public void GetsTheResponse()
      {
         Server.Stub(new ApiExpectation { Response = "{numFound: 20, start: 2, gmt_offset: '-500', gap:'+1HOURS', data:{'2010-02-17 02:08:45.912-0800': 8999, '2010-02-17 03:08:45.912-0800': 9000, '2010-02-17 04:08:45.912-0800': 9001}}" });
         var r = new Facet("mogade").GetInput("anything");
         Assert.AreEqual(20, r.TotalRecords);
         Assert.AreEqual("+1HOURS", r.Gap);
         Assert.AreEqual(2, r.Start);
         Assert.AreEqual("-500", r.GmtOffset);
         Assert.AreEqual(3, r.Data.Count);
         Assert.AreEqual(8999, r.Data["2010-02-17 02:08:45.912-0800"]);
         Assert.AreEqual(9000, r.Data["2010-02-17 03:08:45.912-0800"]);
         Assert.AreEqual(9001, r.Data["2010-02-17 04:08:45.912-0800"]);         
      }
   }
}