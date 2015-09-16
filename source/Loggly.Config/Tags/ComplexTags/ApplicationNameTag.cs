using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loggly.Config;

namespace Loggly
{
    public class ApplicationNameTag : ComplexTag
    {
        public override string InputValue
        {
            get { return LogglyConfig.Instance.ApplicationName; }
        }
    }
}
