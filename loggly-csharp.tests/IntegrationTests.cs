using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using Loggly.Responses;
using NUnit.Framework;
using Newtonsoft.Json;

namespace Loggly.Tests
{
   /// <summary>
   /// WARNING: some of these tests take long, because it will test that the log is actually indexed
   /// </summary>
   public class IntegrationTests
   {
       // Bumping up the timeout to 5 minutes as it seems Loggly may take longer than 1 minute to get things indexed
       private const int _searchTimeout = 5*60*1000; // 5 minutes -> milliseconds
       private ILogger _logger;

      [SetUp]
      public void SetUp()
      {
         LogglyConfiguration.Configure(c => c.AuthenticateWith(ConfigurationManager.AppSettings["IntegrationUser"], ConfigurationManager.AppSettings["IntegrationPassword"]));
         _logger = new Logger(ConfigurationManager.AppSettings["IntegrationKey"]);
      }

      //[Test, Category("Integration")]
      //public void SyncLogToPlainTextInput()
      //{
      //   var randomString = GenerateRandomString(8);
      //   _logger.LogJson(new TestLogEntry() { Message = randomString }, "tag1", "tag2");
      //   var response = StartThread(randomString);
      //   Assert.IsNotNull(response);
      //   Assert.AreEqual(1, response.TotalEvents);
      //   Assert.AreEqual("{\"Message\":\"" + randomString + "\"}", response.ElementAt(0).Message);
      //   Assert.IsTrue(response.ElementAt(0).Tags.Any(tag => tag == "tag1"));
      //   Assert.IsTrue(response.ElementAt(0).Tags.Any(tag => tag == "tag2"));
      //}

    public class TestLogEntry
    {
        public string Message { get; set; }
    }

    [Test, Category("Integration")]
      public void AsyncLogToPlainTextInput()
      {
         var randomString = GenerateRandomString(8);
         _logger.LogJson(new TestLogEntry() { Message = randomString });
         var response = StartJsonThread(randomString, "Message");
         Assert.IsNotNull(response);
         Assert.AreEqual(1, response.TotalEvents);
         Assert.AreEqual(1, response.Count());
         Assert.AreEqual("{\"Message\":\"" + randomString + "\"}", response.ElementAt(0).Message);
         Assert.IsTrue(response.ElementAt(0).Tags.Any(tag => tag == "tag1"));
         Assert.IsTrue(response.ElementAt(0).Tags.Any(tag => tag == "tag2"));
      }

    //[Test, Category("Integration")]
    //public void AsyncLogToJsonBulkInput()
    //{
    //    var randomString1 = GenerateRandomString(8);
    //    var randomString2 = GenerateRandomString(8);
    //    var randomString3 = GenerateRandomString(8);

    //    _logger.LogJson(new[]
    //        {
    //            JsonConvert.SerializeObject(new TestLogEntry() { Message = randomString1 }),
    //            JsonConvert.SerializeObject(new TestLogEntry() { Message = randomString2 }),
    //            JsonConvert.SerializeObject(new TestLogEntry() { Message = randomString3 })
    //        }, "tag1", "tag2");

    //    var response1 = StartJsonThread(randomString1, "Message");
    //    Assert.IsNotNull(response1);
    //    Assert.AreEqual(1, response1.TotalEvents);
    //    var response2 = StartJsonThread(randomString2, "Message");
    //    Assert.IsNotNull(response2);
    //    Assert.AreEqual(1, response2.TotalEvents);
    //    var response3 = StartJsonThread(randomString3, "Message");
    //    Assert.IsNotNull(response3);
    //    Assert.AreEqual(1, response3.TotalEvents);
    //}

      private static SearchResponse StartThread(string randomString)
      {
         var signal = new AutoResetEvent(false);
         SearchResponse response = null;
         new Thread(() =>
         {
            while (true)
            {
               Thread.Sleep(3000);
               response = new Searcher(ConfigurationManager.AppSettings["IntegrationAccount"]).Search(randomString);
               if (response.TotalEvents > 0) { break; }
            }
            signal.Set();
         }).Start();
         signal.WaitOne(_searchTimeout); // wait till loggly index the new event
         return response;
      }

      private static SearchResponse StartJsonThread(string randomString, string property)
      {
          LogglyConfiguration.Configure(config => config.WithTimeout(180000));

         var signal = new AutoResetEvent(false);
         SearchResponse response = null;
         new Thread(() =>
         {
            while (true)
            {
               Thread.Sleep(3000);
               response = new Searcher(ConfigurationManager.AppSettings["IntegrationAccount"]).Search(
                   string.Format("json.{0}:{1}", property, randomString));
               if (response.TotalEvents > 0) { break; }
            }
            signal.Set();
         }).Start();
         signal.WaitOne(_searchTimeout); // wait till loggly index the new event
         return response;
      }

      private static string GenerateRandomString(int size)
      {
          string[] chars = new[]
              {
                  "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "o", "p", "q", "r", "s", "t", "u", "v",
                  "w", "x", "y", "z"
              };
         var builder = new StringBuilder();
          var random = new Random(Guid.NewGuid().GetHashCode());
         for (var i = 0; i < size; ++i)
         {
             var ch = chars[Convert.ToInt32(Math.Floor(24 * random.NextDouble()))];
            builder.Append(ch);
         }

         return builder.ToString().ToLower();
      }
   }
}