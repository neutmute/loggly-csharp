using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using Loggly.Responses;

namespace Loggly.Tests
{
    
    /// <summary>
    /// WARNING: some of these tests take long, because it will test that the log is actually indexed
    /// </summary>
    public class IntegrationTests : BaseFixture
    {

        protected override bool NeedAServer
        {
            get
            {
                return false;
            }
        }

        [Test]
        public void SyncLogToPlainTextInput()
        {
            var textInput = "969cc439-9fc3-400e-b699-fe57480e4c80";
            LogglyConfiguration.Configure(c => c.AuthenticateWith("csharptests", "Passw0rd!"));

            var logger = new Logger(textInput);
            var randomString = GenerateRandomString(8);
            logger.LogSync(randomString);

            var signal = new AutoResetEvent(false);
            SearchResponse response = null;

            new Thread(() =>
            {
                var running = true;
                while (running)
                {
                    Thread.Sleep(3000);
                    response = new Searcher("csharptests").Search(randomString);

                    if (response.TotalRecords > 0)
                        running = false;
                }

                signal.Set();
            }).Start();

            signal.WaitOne(50 * 1000); // wait till loggly index the new event (if it didn't after 50 seconds it is broken)

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.TotalRecords);
            Assert.AreEqual(randomString, response.Results[0].Text);
        }

        [Test]
        public void LogInfoToJsonInputAsync()
        {
            var textInput = "e6d64ac2-c8e9-45e0-ac73-3298ff8cb96f";
            LogglyConfiguration.Configure(c => c.AuthenticateWith("csharptests", "Passw0rd!"));

            var logger = new Logger(textInput);
            var randomString = GenerateRandomString(8);
            logger.LogInfo(randomString, new Dictionary<string, object> { { "key1", "value1" }, { "key2", "value2" } } );

            var signal = new AutoResetEvent(false);
            SearchJsonResponse response = null;

            new Thread(() =>
            {
                var running = true;
                while (running)
                {
                    Thread.Sleep(3000);
                    response = new Searcher("csharptests").SearchJson("message", randomString);

                    if (response.TotalRecords > 0)
                        running = false;
                }

                signal.Set();
            }).Start();

            signal.WaitOne(50 * 1000); // wait till loggly index the new event (if it didn't after 50 seconds it is broken)

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.TotalRecords);
            Assert.AreEqual(randomString, response.Results[0].Json["message"]);
            Assert.AreEqual("info", response.Results[0].Json["category"]);
            Assert.AreEqual("value1", response.Results[0].Json["key1"]);
            Assert.AreEqual("value2", response.Results[0].Json["key2"]);
        }

        [Test]
        public void LogErrorToJsonInputAsync()
        {
            var textInput = "e6d64ac2-c8e9-45e0-ac73-3298ff8cb96f";
            LogglyConfiguration.Configure(c => c.AuthenticateWith("csharptests", "Passw0rd!"));

            var logger = new Logger(textInput);
            var randomString = GenerateRandomString(8);
            logger.LogError(randomString, new InvalidOperationException("oops" + randomString + " something went wrong"));

            var signal = new AutoResetEvent(false);
            SearchJsonResponse response = null;

            new Thread(() =>
            {
                var running = true;
                while (running)
                {
                    Thread.Sleep(3000);
                    response = new Searcher("csharptests").SearchJson("exception", "oops" + randomString);

                    if (response.TotalRecords > 0)
                        running = false;
                }

                signal.Set();
            }).Start();

            signal.WaitOne(50 * 1000); // wait till loggly index the new event (if it didn't after 50 seconds it is broken)

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.TotalRecords);
            Assert.AreEqual(randomString, response.Results[0].Json["message"]);
            Assert.AreEqual("error", response.Results[0].Json["category"]);
            Assert.AreEqual("System.InvalidOperationException: oops" + randomString + " something went wrong", response.Results[0].Json["exception"]);
        }

        [Test]
        public void LogVerboseToJsonInputAsync()
        {
            var textInput = "e6d64ac2-c8e9-45e0-ac73-3298ff8cb96f";
            LogglyConfiguration.Configure(c => c.AuthenticateWith("csharptests", "Passw0rd!"));

            var logger = new Logger(textInput);
            var randomString = GenerateRandomString(8);
            logger.LogVerbose(randomString);

            var signal = new AutoResetEvent(false);
            SearchJsonResponse response = null;

            new Thread(() =>
            {
                var running = true;
                while (running)
                {
                    Thread.Sleep(3000);
                    response = new Searcher("csharptests").SearchJson("message", randomString);

                    if (response.TotalRecords > 0)
                        running = false;
                }

                signal.Set();
            }).Start();

            signal.WaitOne(50 * 1000); // wait till loggly index the new event (if it didn't after 50 seconds it is broken)

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.TotalRecords);
            Assert.AreEqual(randomString, response.Results[0].Json["message"]);
            Assert.AreEqual("verbose", response.Results[0].Json["category"]);
        }


        [Test]
        public void LogWarningToJsonInputAsync()
        {
            var textInput = "e6d64ac2-c8e9-45e0-ac73-3298ff8cb96f";
            LogglyConfiguration.Configure(c => c.AuthenticateWith("csharptests", "Passw0rd!"));

            var logger = new Logger(textInput);
            var randomString = GenerateRandomString(8);
            logger.LogWarning(randomString);

            var signal = new AutoResetEvent(false);
            SearchJsonResponse response = null;

            new Thread(() =>
            {
                var running = true;
                while (running)
                {
                    Thread.Sleep(3000);
                    response = new Searcher("csharptests").SearchJson("message", randomString);

                    if (response.TotalRecords > 0)
                        running = false;
                }

                signal.Set();
            }).Start();

            signal.WaitOne(50 * 1000); // wait till loggly index the new event (if it didn't after 50 seconds it is broken)

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.TotalRecords);
            Assert.AreEqual(randomString, response.Results[0].Json["message"]);
            Assert.AreEqual("warning", response.Results[0].Json["category"]);
        }

        private string GenerateRandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
                
            return builder.ToString().ToLower();
        }
    }
}
