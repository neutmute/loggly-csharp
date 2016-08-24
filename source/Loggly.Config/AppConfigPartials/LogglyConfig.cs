#if FEATURE_SYSTEM_CONFIGURATION
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly.Config
{
    internal partial class LogglyAppConfig
    {
        public static bool HasAppCopnfig
        {
            get { return Instance != null; }
        }

        public bool HasTagConfig
        {
            get { return Tags != null; }
        }

        public bool HasSearchConfig
        {
            get { return Search != null; }
        }
    }
}
#endif
