using System;
using System.Collections.Generic;
using System.Text;
using Loggly.Responses;

namespace Loggly
{
    public class Searcher : ISearcher, IRequestContext
    {
        private const string _domain = ".loggly.com/";
        private readonly string _url;

        public Searcher(string subdomain)
        {
            _url = string.Concat(subdomain, _domain);
        }

        public string Url
        {
            get { return _url; }
        }

        public SearchResponse Search(string query)
        {
            return Search(new SearchQuery { Query = query });
        }

        public SearchResponse Search(string query, DateTime start, DateTime until)
        {
            return Search(new SearchQuery { Query = query, From = start, Until = until });
        }

        public SearchResponse Search(string query, DateTime start, DateTime until, int numberOfRows)
        {
            return Search(new SearchQuery { Query = query, From = start, Until = until, NumberOfRows = numberOfRows});
        }

        public SearchResponse Search(SearchQuery query)
        {
            var communicator = new Communicator(this);
            return communicator.GetPayload<SearchResponse>("apiv2/search", query.ToParameters());
        }

        public SearchResponse<TMessage> Search<TMessage>(string query)
        {
            return Search<TMessage>(new SearchQuery { Query = query });
        }

        public SearchResponse<TMessage> Search<TMessage>(string query, DateTime start, DateTime until)
        {
            return Search<TMessage>(new SearchQuery { Query = query, From = start, Until = until });
        }

        public SearchResponse<TMessage> Search<TMessage>(string query, DateTime start, DateTime until, int numberOfRows)
        {
            return Search<TMessage>(new SearchQuery { Query = query, From = start, Until = until, NumberOfRows = numberOfRows });
        }

        public SearchResponse<TMessage> Search<TMessage>(SearchQuery query)
        {
            var communicator = new Communicator(this);
            return communicator.GetPayload<SearchResponse<TMessage>>("apiv2/search", query.ToParameters());
        }

        public FieldResponse Field(FieldQuery query)
        {
            var communicator = new Communicator(this);
            return communicator.GetPayload<FieldResponse>("apiv2/fields/" + query.FieldName + "/", query.ToParameters());
        }

    }
}