using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Loggly
{
    class EnvironmentProvider : IEnvironmentProvider
    {
        private readonly int _processId;
        private readonly string _machineName;

        public EnvironmentProvider()
        {
            _processId = Process.GetCurrentProcess().Id;
            _machineName = Environment.MachineName;
        }

        public int ProcessId => _processId;

        public string MachineName => _machineName;
    }
}
