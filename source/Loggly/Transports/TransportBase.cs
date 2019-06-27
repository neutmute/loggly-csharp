using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loggly.Config;

namespace Loggly.Transports
{
    internal abstract class TransportBase
    {
        protected abstract string GetRenderedTags(List<ITag> customTags);

        /// <summary>
        /// Combines custom with global tags and makes sure they are loggly legal
        /// </summary>
        public ICollection<string> GetLegalTagUnion(List<ITag> customTags)
        {
            var tagList = new List<string>(LogglyConfig.Instance.TagConfig.Tags.Count + customTags.Count);
            tagList.AddRange(LogglyConfig.Instance.TagConfig.Tags.ToLegalStrings());
            tagList.AddRange(customTags.ToLegalStrings());
            return tagList;
        }
    }
}
