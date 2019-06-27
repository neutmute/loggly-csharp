using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly
{
    /// <summary>
    /// A dictionary of fields that can be passed to Loggly.
    /// </summary>
    public class MessageData : Dictionary<string, object>, IMessageData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageData" /> class that is empty.
        /// </summary>
        public MessageData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageData" /> class that is empty, and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="MessageData" /> can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is less than 0.</exception>
        public MessageData(int capacity)
            : base(capacity)
        {
        }

        public List<string> KeyList
        {
            get { return Keys.ToList(); }
        }

        /// <summary>
        /// Populates this instance of the <see cref="MessageData" /> class with content copied from <paramref name="source" />.
        /// </summary>
        /// <param name="source">The <see cref="IDictionary{TKey,TValue}"/> whose elements are copied to this <see cref="MessageData"/> object</param>.
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="ArgumentException">An element with the same key already exists in <paramref name="source" />.</exception>
        public void AddFromDictionary(IDictionary<object, object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            foreach (var pair in source)
            {
                this.Add(pair.Key.ToString(), pair.Value);
            }
        }

        /// <summary>
        /// Populates this instance of the <see cref="MessageData" /> class with content copied from <paramref name="source" />.
        /// </summary>
        /// <param name="source">The <see cref="IDictionary{TKey,TValue}"/> whose elements are copied to this <see cref="MessageData"/> object</param>.
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="ArgumentException">An element with the same key already exists in <paramref name="source" />.</exception>
        public void AddFromDictionary(IDictionary<string, object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            foreach (var pair in source)
            {
                this.Add(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageData" /> class with content copied from <paramref name="source" />.
        /// </summary>
        /// <param name="source">The <see cref="IDictionary{TKey,TValue}"/> whose elements are copied to the new <see cref="MessageData"/> object</param>.
        /// <returns>The new <see cref="MessageData"/> object</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static MessageData FromDictionary(IDictionary<object, object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var messageData = new MessageData(source.Count);
            messageData.AddFromDictionary(source);
            return messageData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageData" /> class with content copied from <paramref name="source" />.
        /// </summary>
        /// <param name="source">The <see cref="IDictionary{TKey,TValue}"/> whose elements are copied to the new <see cref="MessageData"/> object</param>.
        /// <returns>The new <see cref="MessageData"/> object</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static MessageData FromDictionary(IDictionary<string, object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var messageData = new MessageData(source.Count);
            messageData.AddFromDictionary(source);
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
