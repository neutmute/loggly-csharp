using System.Collections.Generic;

namespace Loggly
{
    public interface IMessageData
    {
        List<string> KeyList { get;  }

        object this[string key]
        {
            get;
            set;
        }

        void AddIfAbsent(string key, object value);
        void Add(string key, object value);
        void Add(string key, string valueFormat, params object[] valueArgs);
    }
}