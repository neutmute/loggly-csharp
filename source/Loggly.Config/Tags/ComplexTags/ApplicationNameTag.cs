using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loggly.Config;

namespace Loggly
{
    public class ApplicationNameTag : ComplexTag
    {   
        public override string Value
        {
            get { return LogglyConfig.Instance.ApplicationName; }
        }
    }
}
