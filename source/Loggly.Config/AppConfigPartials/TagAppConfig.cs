using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly.Config
{
    internal partial class TagAppConfig
    {
        public List<ComplexTag> GetComplexTags()
        {
            var tags = new List<ComplexTag>();
            foreach (ComplexTagAppConfig complexTagConfig in LogglyAppConfig.Instance.Tags.Complex)
            {
                var assembly = complexTagConfig.Assembly;
                if (string.IsNullOrEmpty(assembly))
                {
                    // Support minimal config with a default when unspecified
                    assembly = "Loggly.Config";
                }
                var complexTag = (ComplexTag)Activator.CreateInstance(assembly, complexTagConfig.Type).Unwrap();
                complexTag.Formatter = complexTagConfig.Formatter;
                tags.Add(complexTag);
            }
            return tags;
        }
    }
}
