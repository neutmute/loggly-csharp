using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly.Transports
{
    internal abstract class TransportBase
    {
        private string _renderedTags;
        protected string RenderedTags
        {
            get
            {
                if (_renderedTags == null)
                {
                    _renderedTags = GetRenderedTags();
                }
                return _renderedTags;
            }
        }

        protected TransportBase()
        {
            _renderedTags = null;
        }

        protected abstract string GetRenderedTags();
    }
}
