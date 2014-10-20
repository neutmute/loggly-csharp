using System.Collections.Generic;

namespace Loggly.Config
{
    public class TagConfiguration : ITagConfiguration
    {
        private string _renderedTagCsv;

        public List<ISimpleTag> SimpleTags { get; private set; }

        public List<ComplexTag> ComplexTags { get; private set; }

        public string RenderedTagCsv {
            get
            {
                if (_renderedTagCsv == null)
                {
                    _renderedTagCsv = string.Join(",", GetRenderedTags().ToArray());
                }
                return _renderedTagCsv;
            }
        }

        public TagConfiguration()
        {
            SimpleTags = new List<ISimpleTag>();
            ComplexTags = new List<ComplexTag>();
            _renderedTagCsv = null;
        }

        public List<string> GetRenderedTags()
        {
            var renderedTags = new List<string>();
            SimpleTags.ForEach(st => renderedTags.Add(st.Value));
            ComplexTags.ForEach(ct => renderedTags.Add(ct.FormattedValue));
            return renderedTags;
        }
    }
}