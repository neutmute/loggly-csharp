using System.Collections.Generic;
using System.Text.RegularExpressions;

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

            CoerceLegalTags(renderedTags);

            return renderedTags;
        }

        /// <summary>
        /// https://www.loggly.com/docs/tags/
        /// </summary>
        internal static void CoerceLegalTags(List<string> tags)
        {
            const string illegalCharRegex = @"[^A-z0-9\.\-_]";
            var regex = new Regex(illegalCharRegex, RegexOptions.None);

            for (int i = 0; i < tags.Count; i++)
            {
                tags[i] = regex.Replace(tags[i], "_");

                // needs to be an alpha numeric prefix
                if (tags[i].StartsWith(".") || tags[i].StartsWith("-") || tags[i].StartsWith("_"))
                {
                    tags[i] = "z" + tags[i];
                }
            }
        }
    }
}