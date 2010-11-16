using System;
using NUnit.Framework;

namespace Loggly.Tests
{
   public class SearchTests : BaseFixture
   {
      [Test]
      public void SendsASimpleSearchRequest()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/api/search", QueryString = "?q=abc+123" });
         new Searcher("mogade").Search("abc 123");
      }
      [Test]
      public void ProperlySerializesTimes()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/api/search", QueryString = "?q=NewQuery&from=2001-10-19T21%3a35%3a22.000Z" });
         new Searcher("mogade").Search(new SearchQuery { Query = "NewQuery", From = new DateTime(2001, 10, 20, 5, 35, 22) });
      }
      [Test]
      public void DoesntSerializeAllFields()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/api/search", QueryString = "?q=NewQuery" });
         new Searcher("mogade").Search(new SearchQuery { Query = "NewQuery", FieldsToSelect = Fields.All});         
      }
      [Test]
      public void SerializesSingleField()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/api/search", QueryString = "?q=NewQuery&fields=text" });
         new Searcher("mogade").Search(new SearchQuery { Query = "NewQuery", FieldsToSelect = Fields.Text });
      }
      [Test]
      public void SerializesMultipleField()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/api/search", QueryString = "?q=NewQuery&fields=ip%2cinputname%2ctimestamp" });
         new Searcher("mogade").Search(new SearchQuery { Query = "NewQuery", FieldsToSelect = Fields.Timestamp | Fields.Ip | Fields.InputName });
      }
      [Test]
      public void GetsTheResponse()
      {
         Server.Stub(new ApiExpectation { Response = "{numFound: 130, data:[{timestamp: '2010-02-17 02:08:45.912-0800', inputname: 'i1', ip: '444.555.666.777', text: 'log 1'}, {timestamp: '2011-02-17 02:08:45.912-0700', inputname: 'i2', ip: '444.555.666.778', text: 'log 2'}]}"});
         var r = new Searcher("mogade").Search("anything");
         Assert.AreEqual(130, r.TotalRecords);
         Assert.AreEqual(2, r.Results.Count);
         Assert.AreEqual(new DateTime(2010, 2, 17, 18, 8, 45, 912), r.Results[0].Timestamp);
         Assert.AreEqual("i1", r.Results[0].InputName);
         Assert.AreEqual("444.555.666.777", r.Results[0].IpAddress);
         Assert.AreEqual("log 1", r.Results[0].Text);
         Assert.AreEqual(new DateTime(2011, 2, 17, 17, 8, 45, 912), r.Results[1].Timestamp);
         Assert.AreEqual("i2", r.Results[1].InputName);
         Assert.AreEqual("444.555.666.778", r.Results[1].IpAddress);
         Assert.AreEqual("log 2", r.Results[1].Text);
      }
   }
}