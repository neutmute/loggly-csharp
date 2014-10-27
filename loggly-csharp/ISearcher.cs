using System;
using Loggly.Responses;

namespace Loggly
{
   internal interface ISearcher
   {
      SearchResponse Search(string query);
      SearchResponse Search(string query, DateTime start, DateTime until);
      SearchResponse Search(string query, int startingAt, int numberOfRows);
      SearchResponse Search(string query, DateTime start, DateTime until, int startingAt, int numberOfRows);
      SearchResponse Search(SearchQuery query);
   }
}