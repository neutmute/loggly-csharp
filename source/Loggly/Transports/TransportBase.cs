using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loggly.Config;

namespace Loggly.Transports
{
    internal abstract class TransportBase
    {
        private static readonly ICollection<string> EmptyCollection = new string[0];

        /// <summary>
        /// Combines custom with global tags and makes sure they are loggly legal
        /// </summary>
        public ICollection<string> GetLegalTagUnion(List<ITag> customTags)
        {
            int capacity = LogglyConfig.Instance.TagConfig.Tags.Count + customTags.Count;
            if (capacity == 0)
                return EmptyCollection;

            var tagList = new List<string>(capacity);
            if (LogglyConfig.Instance.TagConfig.Tags.Count > 0)
                tagList.AddRange(LogglyConfig.Instance.TagConfig.Tags.ToLegalStrings());
            if (customTags.Count > 0)
                tagList.AddRange(customTags.ToLegalStrings());
            return tagList;
        }
    }
}
