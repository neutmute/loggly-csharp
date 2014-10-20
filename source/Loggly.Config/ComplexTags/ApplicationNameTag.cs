using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly.Config.ComplexTags
{
    public class ApplicationNameTag : ComplexTag
    {   
        public override string Value
        {
            get { return LogglyConfig.Instance.ApplicationName; }
        }
    }
}
