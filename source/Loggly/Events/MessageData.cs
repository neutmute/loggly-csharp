using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Loggly
{
    public interface IMessageData
    {
        void AddSafe(string key, object value);
        void Add(string key, object value);
        void Add(string key, string valueFormat, params object[] valueArgs);
    }

    /// <summary>
    /// Just a wrapped dictionary
    /// </summary>
    public class MessageData : Dictionary<string, object>, IMessageData
    {
        public static MessageData FromDictionary(IDictionary<object, object> source)
        {
            var messageData = new MessageData();
            var asDictionary = source.ToDictionary(k => k.Key != null ? k.Key.ToString() : string.Empty, v => v.Value);
            asDictionary.ToList().ForEach(x => messageData.Add(x.Key, x.Value));
            return messageData;
        }

        public void AddSafe(string key, object value)
        {
            if (!ContainsKey(key))
            {
                Add(key, value);
            }
        }

        public void Add(string key, string value)
        {
            base.Add(key, value);
        }

        public void Add(string key, string valueFormat, params object[] valueArgs)
        {
            var value = string.Format(valueFormat, valueArgs);
            Add(key, value);
        }
        
    }
}
