using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Loggly
{
    class EnvironmentProvider : IEnvironmentProvider
    {
        public int ProcessId
        {
            get
            {
                return Process.GetCurrentProcess().Id;
            }
        }

        public string MachineName
        {
            get
            {
                return Environment.MachineName;
            }
        }
    }
}
