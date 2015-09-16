using System;
using System.Collections.Generic;
using Loggly.Config;
using Loggly.Responses;

namespace Loggly
{
    public class EventOptions
    {
        /// <summary>
        /// Custom tags per event
        /// </summary>
        public List<ITag> Tags { get; set; }

        public EventOptions()
        {
            Tags = new List<ITag>();
        }
    }
}