using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly
{
    public class OperatingSystemPlatformTag : ComplexTag
    {
        public override string InputValue
        {
            get { return Environment.OSVersion.Platform.ToString(); }
        }
    }
}
