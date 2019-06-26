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
        private static readonly Regex IllegalCharRegex = new Regex(@"[^A-z0-9\.\-_]|\^", RegexOptions.None);

        public static IEnumerable<string> ToLegalStrings(this List<ITag> tags)
        {
            foreach (var tag in tags)
            {
                yield return CoerceLegalTag(tag.Value);
            }
        }

        public static void Add(this List<ITag> tags, string value)
        {
            tags.Add(new SimpleTag{Value = value});
        }

        /// <summary>
        /// https://www.loggly.com/docs/tags/
        /// </summary>
        internal static string CoerceLegalTag(string tagValue)
        {
            tagValue = IllegalCharRegex.Replace(tagValue, "_");

            // needs to be an alpha numeric prefix
            if (tagValue.StartsWith(".") || tagValue.StartsWith("-") || tagValue.StartsWith("_"))
            {
                tagValue = "z" + tagValue;
            }

            if (tagValue.Length > 64)
            {
                tagValue = tagValue.Substring(0, 64);
            }

            return tagValue;
        }
    }
}
