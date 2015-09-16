using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly
{
    public class HostnameTag : ComplexTag
    {
        public override string InputValue
        {
            get { return Environment.MachineName; }
        }
    }
}
