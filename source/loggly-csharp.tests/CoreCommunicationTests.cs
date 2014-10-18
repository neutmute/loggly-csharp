using System.Collections.Generic;
using System.Net;
using NUnit.Framework;

namespace Loggly.Tests
{
   public class CoreCommunicationTests : BaseFixture
   {
      [Test]
      public void ThrowsLogglyExceptionOnServerError()
      {
         Server.Stub(new ApiExpectation {Response = "no!", Status = 401});
         var ex = Assert.Throws<LogglyException>(() => new Communicator(new FakeContext()).GetPayload<object>("end", new Dictionary<string, object>(0)));
         Assert.AreEqual("no!", ex.Message);
         Assert.AreEqual(WebExceptionStatus.ProtocolError, ((WebException)ex.InnerException).Status);
      }
      [Test]
      public void ThrowsLogglyExceptionOnParseError()
      {
         Server.Stub(new ApiExpectation { Response = "{invalidJson"});
         var ex = Assert.Throws<LogglyException>(() => new Communicator(new FakeContext()).GetPayload<object>("end", new Dictionary<string, object>(0)));
         Assert.IsTrue(ex.Message.StartsWith("Unexpected end while parsing unquoted property name"));
      }
   }

   public class FakeContext : IRequestContext
   {
      public string Url
      {
         get { return "someUrl"; }
      }
   }
}