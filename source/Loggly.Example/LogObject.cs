using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loggly.Example
{
    public class LogObject : IMessageData
    {
        public DateTime LocalTime
        {
            get { return DateTime.Now; }
        }
        public string StackTrace
        {
            get { return Environment.StackTrace; }
        }


        public string UserName
        {
            get { return Environment.UserName; }
        }


        public string CurrentDirectory
        {
            get { return Environment.CurrentDirectory; }
        }

        /// <summary>
        /// Note this is a nested object
        /// </summary>
        public OperatingSystem OSVersion
        {
            get { return Environment.OSVersion; }
        }


        public void AddIfAbsent(string key, object value)
        {
            //noop
        }

        public void Add(string key, string valueFormat, params object[] valueArgs)
        {
            //noop
        }


        public void Add(string key, object value)
        {
            //noop
        }

        public List<string> KeyList
        {
            get { return new List<string>(); }
        }

        public object this[string key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
