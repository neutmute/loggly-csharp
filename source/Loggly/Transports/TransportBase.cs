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
    }
}
