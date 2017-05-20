using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Loggly.Config;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Loggly.Tests.Loggly.Transports.HttpTransports
{
    [Ignore("Tests buggy - fail in appveyor with data from different test")]
    [TestFixture]
    public class HttpMessageTransportFixture
    {
        private HttpMessageTransport _transport;
        private Uri _requestUri;
        private string _requestContent;
        private string _tags;

        [SetUp]
        public void Setup()
        {
            LogglyConfig.Instance.TagConfig.Tags.Clear();
            LogglyConfig.Instance.TagConfig.Tags.Add(new SimpleTag { Value = "myTag" });
            LogglyConfig.Instance.CustomerToken = "MyLogglyToken";
            LogglyConfig.Instance.Transport.EndpointHostname = "test";
            LogglyConfig.Instance.Transport.EndpointPort = 443;
            var handler = new Mock<FakeHttpMessageHandler> { CallBase = true };

            handler.Setup(x => x.Send(It.IsAny<HttpRequestMessage>()))
                .Callback(async (HttpRequestMessage x) =>
                    {
                        _requestUri = x.RequestUri;
                        _tags = x.Headers.GetValues("X-LOGGLY-TAG").FirstOrDefault();
                        _requestContent = await x.Content.ReadAsStringAsync();
                    }
                )
               .Returns((HttpRequestMessage request)=>
                    {
                        if ((request.Headers.GetValues("X-LOGGLY-TAG").FirstOrDefault() ?? "").Contains("triggerError"))
                        {
                            return new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.InternalServerError,
                                Content = new StringContent("An error occurred", Encoding.UTF8, "text/plain")
                            };
                        }
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent("{\"response\" : \"ok\"}", Encoding.UTF8, "application/json")
                        };
                    }
               );

            _transport = new HttpMessageTransport(handler.Object);
        }

        [Test]
        public async Task SendsSingleMessageToInputApi()
        {
            var message = new LogglyMessage { Content = "Test1", CustomTags = { new SimpleTag { Value = "TestTag" } } };

            var response = await _transport.Send(new[] { message });

            Assert.AreEqual("https://test/inputs/MyLogglyToken", _requestUri.ToString());

            Assert.AreEqual("myTag,TestTag", _tags);

            Assert.AreEqual("Test1", _requestContent);

            Assert.AreEqual(ResponseCode.Success, response.Code);
        }

        [Test]
        public async Task SendsMultipleMessageToBulkApi()
        {
            var messages = new[]
            {
                new LogglyMessage {Content = "Test1", CustomTags = {new SimpleTag {Value = "TestTag"}}},
                new LogglyMessage {Content = "Test2", CustomTags = {new SimpleTag {Value = "TestTag"},new SimpleTag {Value = "TestTagWhichWillBeIgnored"}}}
            };


            var response = await _transport.Send(messages);

            Assert.AreEqual("https://test/bulk/MyLogglyToken", _requestUri.ToString());

            Assert.AreEqual("myTag,TestTag", _tags);

            Assert.AreEqual("Test1\nTest2", _requestContent);

            Assert.AreEqual(ResponseCode.Success, response.Code);
        }

        [Test]
        public async Task ErrorResponseIsHandled()
        {
            var message = new LogglyMessage { Content = "Test1", CustomTags = { new SimpleTag { Value = "triggerError" } } };

            var response = await _transport.Send(new[] { message });

            Assert.AreEqual("https://test/inputs/MyLogglyToken", _requestUri.ToString());

            
            Assert.AreEqual(ResponseCode.Error, response.Code);
        }
    }
}
