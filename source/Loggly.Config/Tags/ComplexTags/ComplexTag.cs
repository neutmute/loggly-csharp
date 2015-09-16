using System;
using Loggly.Config;

namespace Loggly
{
    public abstract class ComplexTag : ITag
    {
        public string Formatter { get; set; }

        public abstract string InputValue { get; }

        public string Value
        {
            get { return String.Format(Formatter, InputValue); }
        }

        protected ComplexTag()
        {
            Formatter = "{0}";
        }
    }
}