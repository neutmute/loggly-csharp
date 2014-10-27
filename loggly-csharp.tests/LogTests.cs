using System.Threading;
using NUnit.Framework;

namespace Loggly.Tests.LoggerTests
{
   public class LogTests : BaseFixture
   {
      [Test]
      public void SynchronouslyLogsAMessage()
      {
         Server.Stub(new ApiExpectation {Method = "POST", Url = "/inputs/ITS-OVER-9000", Request = "Vegeta!!!", Response = "{}"});
         new Logger("ITS-OVER-9000").LogSync("Vegeta!!!");
      }

      [Test]
      public void SynchronouslyReturnsTheResponse()
      {
         Server.Stub(new ApiExpectation {Response = "{eventstamp: 123495}"});
         Assert.AreEqual(123495, new Logger("ITS-OVER-9000").LogSync("Vegeta!!!").TimeStamp);
      }

      [Test]
      [Ignore("This test is hanging the test runner randomly, my guess is that the worker thread doesn't get to execute the callback and it gets into an unconsistent state")]
      public void ASynchronouslyLogsAMessageWithNullCallback()
      {
         Server.Stub(new ApiExpectation {Method = "POST", Url = "/inpust/ATREIDES", Request = "Aynch is even cooler", Response = "{}"});
         new Logger("ATREIDES").Log("Aynch is even cooler");
      }

      [Test]
      public void ASynchronouslyCallsbackWithResponse()
      {
         Server.Stub(new ApiExpectation {Response = "{eventstamp: 747193}"});
         new Logger("ATREIDES").Log("Leto II", r =>
         {
            Assert.AreEqual(747193, r.TimeStamp);
            Set();
         });

         WaitOne();
      }

      [Test]
      public void WriteTwoLogEntriesAsyncWithCallback()
      {
         Server.Stub(new ApiExpectation {Response = "{eventstamp: 747193}"});
         long counter = 0;
         new Logger("ATREIDES").Log("Leto II", r =>
         {
            Assert.AreEqual(747193, r.TimeStamp);
            if (Interlocked.Increment(ref counter) == 2) { Set(); }
               
         });

         new Logger("ATREIDES").Log("Leto III", r =>
         {
            Assert.AreEqual(747193, r.TimeStamp);
            if (Interlocked.Increment(ref counter) == 2) { Set(); }
         });

         WaitOne();
      }

      [Test]
      public void ReturnsSuccessResponse()
      {
         Server.Stub(new ApiExpectation {Response = "{eventstamp: 123495}"});
         Assert.True(new Logger("ITS-OVER-9000").LogSync("Vegeta!!!").Success);
      }

      [Test]
      public void ReturnsErrorResponse()
      {
         // Missing expectation causing the error
         Assert.False(new Logger("ITS-OVER-9000").LogSync("Vegeta!!!").Success);
      }

      [Test]
      public void ReturnsErrorResponseWhenNetworkIssues()
      {
         // There is no listener on that port
         LogglyConfiguration.Configure(c => c.ForceUrlTo("http://localhost:9949/"));
         Assert.False(new Logger("ITS-OVER-9000").LogSync("Vegeta!!!").Success);
      }
   }
}