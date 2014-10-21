using System.Collections.Generic;

namespace Loggly
{
    public class FieldQuery : SearchQuery
    {
        public string FieldName { get; set; }

        public override IDictionary<string, object> ToParameters()
        {
            IDictionary<string, object> parameters = base.ToParameters();
            parameters.Add("fieldname", this.FieldName);

            return parameters;
        }
    }
}