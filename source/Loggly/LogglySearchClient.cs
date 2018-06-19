using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Loggly.Config;
using Loggly.Responses;

namespace Loggly
{
    public class LogglySearchClient : ISearchClient
    {
        private ISearchTransport _transport;

        public LogglySearchClient(ISearchConfiguration config)
        {
            _transport = new SearchTransport(config);
        }

        public LogglySearchClient() : this(LogglyConfig.Instance.Search)
        {
        }


        public Task<SearchResponse> Search(string query)
        {
            return Search(new SearchQuery { Query = query });
        }

        public Task<SearchResponse> Search(string query, DateTime start, DateTime until)
        {
            return Search(new SearchQuery { Query = query, From = start, Until = until });
        }

        public Task<SearchResponse> Search(string query, DateTime start, DateTime until, int size)
        {
            return Search(new SearchQuery { Query = query, From = start, Until = until, Size = size});
        }

        public Task<SearchResponse> Search(SearchQuery query)
        {
            return _transport.Search(query);
        }

        public Task<SearchResponse<TMessage>> Search<TMessage>(string query)
        {
            return Search<TMessage>(new SearchQuery { Query = query });
        }

        public Task<SearchResponse<TMessage>> Search<TMessage>(string query, DateTime start, DateTime until)
        {
            return Search<TMessage>(new SearchQuery { Query = query, From = start, Until = until });
        }

        public Task<SearchResponse<TMessage>> Search<TMessage>(string query, DateTime start, DateTime until, int size)
        {
            return Search<TMessage>(new SearchQuery { Query = query, From = start, Until = until, Size = size });
        }

        public Task<SearchResponse<TMessage>> Search<TMessage>(SearchQuery query)
        {
            return _transport.Search<TMessage>(query);
        }

        public Task<FieldResponse> Field(FieldQuery query)
        {
            return _transport.Search(query);
        }

    }
}