#if FEATURE_SYSTEM_ENVIRONMENT_OSVERSION
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly
{
    public class OperatingSystemVersionTag : ComplexTag
    {
        public override string InputValue
        {
            get { return Environment.OSVersion.VersionString; }
        }
    }
}
#endif
