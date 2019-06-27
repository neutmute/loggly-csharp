using System.Collections.Generic;
using NUnit.Framework;

namespace Loggly.Tests.Loggly.Events
{
    public class LogglyEventFixture : Fixture
    {
        [Test]
        public void CtorWithMessageDataIsSet()
        {
            var messageData = MessageData.FromDictionary(new Dictionary<string, object>
            {
                {"key1", "value"},
                {"key2", "value"}
            });

            var logglyEvent = new LogglyEvent(messageData);

            Assert.That(logglyEvent.Data, Is.EqualTo(messageData));
        }
    }
}