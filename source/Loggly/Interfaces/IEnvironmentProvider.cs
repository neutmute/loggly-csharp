using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly
{
    public interface IEnvironmentProvider
    {
        int ProcessId { get; }

        string MachineName { get; }
    }
}
