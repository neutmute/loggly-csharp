using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;
using Loggly.Responses;
using NUnit.Framework;

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

      [Test, Category("Integration")]
      public void SyncLogToPlainTextInput()
      {
         var randomString = GenerateRandomString(8);
         _logger.LogSync(randomString);
         var response = StartThread(randomString);
         Assert.IsNotNull(response);
         Assert.AreEqual(1, response.TotalRecords);
         //Assert.AreEqual(randomString, response.Results[0].Text);
      }

      [Test, Category("Integration")]
      public void LogInfoToJsonInputAsync()
      {
         var randomString = GenerateRandomString(8);
         _logger.LogInfo(randomString, new Dictionary<string, object> {{"key1", "value1"}, {"key2", "value2"}}, "tag1", "tag2");
         var response = StartJsonThread(randomString, "message");
         Assert.IsNotNull(response);
         Assert.AreEqual(1, response.TotalRecords);
         Assert.AreEqual(randomString, response.Results[0].Json["message"]);
         Assert.AreEqual("info", response.Results[0].Json["category"]);
         Assert.AreEqual("value1", response.Results[0].Json["key1"]);
         Assert.AreEqual("value2", response.Results[0].Json["key2"]);
      }

      [Test, Category("Integration")]
      public void LogErrorToJsonInputAsync()
      {
          // TODO: This test case needs to be expanded further. Looks like adding the word "oops" 
          // TODO: before randomString, makes the post and the search unreliable. 
         var randomString = GenerateRandomString(8);
         _logger.LogError(randomString, new InvalidOperationException(randomString + " something went wrong"));
         var response = StartJsonThread(randomString, "exception");
         Assert.IsNotNull(response);
         Assert.AreEqual(1, response.TotalRecords);
         Assert.AreEqual(randomString, response.Results[0].Json["message"]);
         Assert.AreEqual("error", response.Results[0].Json["category"]);
         Assert.AreEqual("System.InvalidOperationException: " + randomString + " something went wrong", response.Results[0].Json["exception"]);
      }

      [Test, Category("Integration")]
      public void LogVerboseToJsonInputAsync()
      {
         var randomString = GenerateRandomString(8);
         _logger.LogVerbose(randomString);

         var response = StartJsonThread(randomString, "message");
         Assert.IsNotNull(response);
         Assert.AreEqual(1, response.TotalRecords);
         Assert.AreEqual(randomString, response.Results[0].Json["message"]);
         Assert.AreEqual("verbose", response.Results[0].Json["category"]);
      }


      [Test, Category("Integration")]
      public void LogWarningToJsonInputAsync()
      {
         var randomString = GenerateRandomString(8);
         _logger.LogWarning(randomString);
         var response = StartJsonThread(randomString, "message");
         Assert.IsNotNull(response);
         Assert.AreEqual(1, response.TotalRecords);
         Assert.AreEqual(randomString, response.Results[0].Json["message"]);
         Assert.AreEqual("warning", response.Results[0].Json["category"]);
      }

      private static SearchJsonResponse StartThread(string randomString)
      {
         var signal = new AutoResetEvent(false);
         SearchJsonResponse response = null;
         new Thread(() =>
         {
            while (true)
            {
               Thread.Sleep(3000);
               response = new Searcher(ConfigurationManager.AppSettings["IntegrationAccount"]).Search(randomString);
               if (response.TotalRecords > 0) { break; }
            }
            signal.Set();
         }).Start();
         signal.WaitOne(_searchTimeout); // wait till loggly index the new event
         return response;
      }

      private static SearchJsonResponse StartJsonThread(string randomString, string property)
      {
          LogglyConfiguration.Configure(config => config.WithTimeout(180000));

         var signal = new AutoResetEvent(false);
         SearchJsonResponse response = null;
         new Thread(() =>
         {
            while (true)
            {
               Thread.Sleep(3000);
               response = new Searcher(ConfigurationManager.AppSettings["IntegrationAccount"]).SearchJson(property, randomString);
               if (response.TotalRecords > 0) { break; }
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
         var random = new Random();
         for (var i = 0; i < size; ++i)
         {
             var ch = chars[Convert.ToInt32(Math.Floor(24 * random.NextDouble()))];
            builder.Append(ch);
         }

         return builder.ToString().ToLower();
      }
   }
}