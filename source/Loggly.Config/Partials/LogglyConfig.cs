using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly.Config
{
    public partial class LogglyAppConfig
    {
        public static bool HasAppCopnfig
        {
            get { return Instance != null; }
        }
    }
}
