using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Loggly.Config
{
    public static class TagExtensions
    {
        public static string[] ToLegalStrings(this List<ITag> tags)
        {
            var renderedTags = new List<string>();
            tags.ForEach(st => renderedTags.Add(st.Value));

            CoerceLegalTags(renderedTags);

            return renderedTags.ToArray();
        }

        public static void Add(this List<ITag> tags, string value)
        {
            tags.Add(new SimpleTag{Value = value});
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
