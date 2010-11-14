using System;
using System.Collections.Generic;
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

      public SearchResponse Search(string query, int startingAt, int numberOfRows)
      {
         return Search(new SearchQuery { Query = query, StartingAt = startingAt, NumberOfRows = numberOfRows });
      }

      public SearchResponse Search(string query, DateTime start, DateTime until, int startingAt, int numberOfRows)
      {
         return Search(new SearchQuery { Query = query, From = start, Until = until, StartingAt = startingAt, NumberOfRows = numberOfRows});
      }

      public SearchResponse Search(SearchQuery query)
      {
         var communicator = new Communicator(this);         
         return communicator.GetPayload<SearchResponse>(string.Concat("api/search"), query.ToParameters());
      }
   }
}