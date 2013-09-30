using System;
using Loggly.Responses;

namespace Loggly
{
   internal interface ISearcher
   {
       SearchJsonResponse Search(string query);
       SearchJsonResponse Search(string query, DateTime start, DateTime until);
       SearchJsonResponse Search(string query, int startingAt, int numberOfRows);
       SearchJsonResponse Search(string query, DateTime start, DateTime until, int startingAt, int numberOfRows);
       SearchJsonResponse Search(SearchQuery query);
   }
}