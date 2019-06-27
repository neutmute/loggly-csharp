using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Loggly.Tests.Loggly.Events
{
    public class MessageDataFixture : Fixture
    {
        [Test]
        public void CtorWithOutOfRangeCapacityThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new MessageData(-1));
        }

        [Test]
        public void AddFromNullObjectKeyedDictionaryThrowsException()
        {
            var messageData = new MessageData();

            Assert.Throws<ArgumentNullException>(() => messageData.AddFromDictionary((IDictionary<object, object>)null));
        }

        [Test]
        public void AddFromNullStringKeyedDictionaryThrowsException()
        {
            var messageData = new MessageData();

            Assert.Throws<ArgumentNullException>(() => messageData.AddFromDictionary((IDictionary<string, object>)null));
        }

        [Test]
        public void AddDuplicateKeysFromObjectKeyedDictionaryThrowsException()
        {
            var messageData = new MessageData();
            var source1 = new Dictionary<object, object>
            {
                {"key1", "value"},
                {"key2", "value"}
            };
            messageData.AddFromDictionary(source1);
            var source2 = new Dictionary<object, object>
            {
                {"key1", "value"}
            };

            Assert.Throws<ArgumentException>(() => messageData.AddFromDictionary(source2));
        }

        [Test]
        public void AddDuplicateKeysFromStringKeyedDictionaryThrowsException()
        {
            var messageData = new MessageData();
            var source1 = new Dictionary<string, object>
            {
                {"key1", "value"},
                {"key2", "value"}
            };
            messageData.AddFromDictionary(source1);
            var source2 = new Dictionary<string, object>
            {
                {"key1", "value"}
            };

            Assert.Throws<ArgumentException>(() => messageData.AddFromDictionary(source2));
        }

        [Test]
        public void AddFromStringKeyedDictionaryAddsPairs()
        {
            var messageData = new MessageData();
            var source = new Dictionary<string, object>
            {
                {"key1", "value"},
                {"key2", "value"}
            };

            messageData.AddFromDictionary(source);

            Assert.That(messageData, Is.EquivalentTo(source));
        }

        [Test]
        public void AddFromObjectKeyedDictionaryAddsPairs()
        {
            var messageData = new MessageData();
            var source = new Dictionary<object, object>
            {
                {"key1", "value"},
                {"key2", "value"}
            };

            messageData.AddFromDictionary(source);

            Assert.That(messageData, Is.EquivalentTo(source));
        }

        [Test]
        public void InitialiseFromStringKeyedDictionaryAddsPairs()
        {
            var source = new Dictionary<string, object>
            {
                {"key1", "value"},
                {"key2", "value"}
            };

            var messageData = MessageData.FromDictionary(source);

            Assert.That(messageData, Is.EquivalentTo(source));
        }

        [Test]
        public void InitialiseFromObjectKeyedDictionaryAddsPairs()
        {
            var source = new Dictionary<object, object>
            {
                {"key1", "value"},
                {"key2", "value"}
            };

            var messageData = MessageData.FromDictionary(source);

            Assert.That(messageData, Is.EquivalentTo(source));
        }
        
        /// <summary>
        /// Fix for this exception..
        /// 
        /// NLog.NLogRuntimeException NLog.NLogRuntimeException: Exception occurred in NLog ---> System.FormatException: Index (zero based) must be greater than or equal to zero and less than the size of the argument list.
        /// at System.Text.StringBuilder.AppendFormat(IFormatProvider provider, String format, Object[] args)
        /// at System.String.Format(IFormatProvider provider, String format, Object[] args)
        /// at Loggly.MessageData.Add(String key, String valueFormat, Object[] valueArgs)
        /// at NLog.Targets.Loggly.Write(LogEventInfo logEvent)
        /// at NLog.Targets.Target.Write(AsyncLogEventInfo logEvent)
        /// </summary>
        [Test]
        public void AddDoesntThrowWhenLooksLikeFormat()
        {
            var logglyEvent = new LogglyEvent();
            const string logMessage  = "a message that looks like it should have arg params {0}";
            logglyEvent.Data.Add("message", logMessage);
        }
    }
}
