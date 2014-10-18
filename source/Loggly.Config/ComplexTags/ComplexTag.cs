using System;

namespace Loggly
{
    public abstract class ComplexTag
    {
        public string Formatter { get; set; }

        public abstract string Value { get; }

        public string FormattedValue
        {
            get { return String.Format(Formatter, Value); }
        }

        protected ComplexTag()
        {
            Formatter = "{0}";
        }
    }
}