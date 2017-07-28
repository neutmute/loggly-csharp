using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Loggly.Tests.Loggly.Transports.HttpTransports;
using Moq;
using NUnit.Framework;

namespace Loggly.Tests.Loggly.Events
{
    public class SerializationDataFixture : Fixture
    {
        /// <summary>
        /// Fix for this exception..
        /// 
        /// Newtonsoft.Json.JsonSerializationException: 'Self referencing loop detected for property 'Module' with type 'System.Reflection.RuntimeModule'. Path 'Exception.TargetSite.Module.Assembly.EntryPoint'.'
        /// </summary>
        [Test]
        public async Task SelfReferencingLoopShouldNotFailMessageSerialization()
        {
            var transportMock = new Mock<IMessageTransport>();
            transportMock.Setup(x => x.Send(It.IsAny<IEnumerable<LogglyMessage>>()))
                .Callback((IEnumerable < LogglyMessage > x)=>x.ToList())//making sure the messages are evaluated
                .Returns(() => Task.FromResult(new LogResponse { Code = ResponseCode.Success }));

            var logglyEvent = new LogglyEvent();
            logglyEvent.Data.AddIfAbsent("ReferenceLoopType", new TypeWithReferenceLoopParent());

            var client = new LogglyClient(transportMock.Object);

            var res = await client.Log(logglyEvent);

            Assert.AreEqual(ResponseCode.Success, res.Code);
        }
    }

    public class TypeWithReferenceLoopParent
    {
        public TypeWithReferenceLoopParent()
        {
            Child = new TypeWithReferenceLoopChild()
            {
                Parent = this
            };
        }
        public class TypeWithReferenceLoopChild
        {
            public TypeWithReferenceLoopParent Parent { get; set; }
        }
        public TypeWithReferenceLoopChild Child { get; set; }
    }
}
