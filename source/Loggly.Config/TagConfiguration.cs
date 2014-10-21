using System.Collections.Generic;

namespace Loggly.Config
{
    public class TagConfiguration : ITagConfiguration
    {
        public List<ISimpleTag> SimpleTags { get; private set; }

        public List<ComplexTag> ComplexTags { get; private set; }

        public TagConfiguration()
        {
            SimpleTags = new List<ISimpleTag>();
            ComplexTags = new List<ComplexTag>();
        }

        public List<string> GetRenderedTags()
        {
            var renderedTags = new List<string>();
            SimpleTags.ForEach(st => renderedTags.Add(st.Value));
            ComplexTags.ForEach(ct => renderedTags.Add(ct.FormattedValue));

            // conform tags to loggly rules. no spaces.
            for (int i = 0; i < renderedTags.Count; i++)
            {
                renderedTags[i] = renderedTags[i].Replace(" ", string.Empty);
            }

            return renderedTags;
        }
    }
}