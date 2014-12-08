using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Loggly
{
    /// <summary>
    /// Just a wrapped dictionary
    /// </summary>
    public class MessageData : Dictionary<string, object>, IMessageData
    {
        public List<string> KeyList
        {
            get { return Keys.ToList(); }
        }

        public static MessageData FromDictionary(IDictionary<object, object> source)
        {
            var messageData = new MessageData();
            var asDictionary = source.ToDictionary(k => k.Key != null ? k.Key.ToString() : string.Empty, v => v.Value);
            asDictionary.ToList().ForEach(x => messageData.Add(x.Key, x.Value));
            return messageData;
        }

        public void AddIfAbsent(string key, object value)
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
            if (valueArgs.Length == 0)
            {
                Add(key, valueFormat);
            }
            else
            {
                var value = string.Format(valueFormat, valueArgs);
                Add(key, value);
            }
        }

        public string ToString(string delimiter)
        {
            var sb = new StringBuilder();

            foreach (var key in KeyList)
            {
                sb.AppendFormat("{0}={1}{2}", key, this[key], delimiter);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString(Environment.NewLine);
        }
    }
}
