using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Loggly
{
    public class SearchResponse : IEnumerable<string>
    {
        [JsonProperty("rsid")]
        public RSID RSID { get; set; }

        public IEnumerator<string> GetEnumerator()
        {
            return new SearchResponseEnumerator(this.RSID.Id);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class SearchResponseEnumerator : IEnumerator<string>
    {
        public SearchResponseEnumerator(string rsid)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public string Current { get; private set; }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }

    public class RSID
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("date_from")]
        public int From { get; set; }
        [JsonProperty("date_to")]
        public int To { get; set; }
        [JsonProperty("elapsed_time")]
        public double ElapsedTime { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }


}
