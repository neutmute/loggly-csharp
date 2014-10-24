using System;
using System.Collections.Generic;
using System.Text;
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
            return Search(new SearchQuery { Query = query, From = start, Until = until, Size = numberOfRows});
        }

        public SearchResponse Search(SearchQuery query)
        {
            return _transport.Search(query);
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
            return Search<TMessage>(new SearchQuery { Query = query, From = start, Until = until, Size = numberOfRows });
        }

        public SearchResponse<TMessage> Search<TMessage>(SearchQuery query)
        {
            return _transport.Search<TMessage>(query);
        }

        public FieldResponse Field(FieldQuery query)
        {
            return _transport.Search(query);
        }

    }
}