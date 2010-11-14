using NUnit.Framework;

namespace Loggly.Tests.LoggerTests
{
   public class LogTests : BaseFixture
   {
      [Test]
      public void SynchronouslyLogsAMessage()
      {
         Server.Stub(new ApiExpectation {Method = "POST", Url = "/inputs/ITS-OVER-9000", Request = "Vegeta!!!", Response = "{}"});
         new Logger("ITS-OVER-9000").Log("Vegeta!!!");
      }

      [Test]
      public void SynchronouslyReturnsTheResponse()
      {
         Server.Stub(new ApiExpectation {Response = "{eventstamp: 123495}"});
         Assert.AreEqual(123495, new Logger("ITS-OVER-9000").Log("Vegeta!!!").TimeStamp);
      }

      [Test]
      public void ASynchronouslyLogsAMessageWithNullCallback()
      {
         Server.Stub(new ApiExpectation {Method = "POST", Url = "/inpust/ATREIDES", Request = "Aynch is even cooler", Response = "{}"});
         new Logger("ATREIDES").LogAsync("Aynch is even cooler");
      }

      [Test]
      public void ASynchronouslyCallsbackWithResponse()
      {
         Server.Stub(new ApiExpectation {Response = "{eventstamp: 747193}"});
         new Logger("ATREIDES").LogAsync("Leto II", r =>
         {
            Assert.AreEqual(747193, r.TimeStamp);
            Set();
         });
         WaitOne();
      }
   }
}