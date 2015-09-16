using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Loggly.Config
{
    public class TagConfiguration : ITagConfiguration
    {
        public List<ITag> Tags { get; private set; }
        
        public TagConfiguration()
        {
            Tags = new List<ITag>();
        }

       
    }
}