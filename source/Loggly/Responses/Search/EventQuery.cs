using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly.Responses
{
    public class EventQuery
    {
        public string Rsid { get; set; }

        public int Page { get; set; }

        public virtual IDictionary<string, object> ToParameters()
        {
            return new Dictionary<string, object>
                {
                   { "rsid", Rsid },
                   { "page", Page }
                };
        }
    }
}
