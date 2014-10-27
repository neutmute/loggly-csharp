using System;
using Loggly.Responses;

namespace Loggly
{
   public interface ISearchClient
   {
       SearchResponse Search(string query);
       SearchResponse Search(string query, DateTime start, DateTime until);
       SearchResponse Search(string query, DateTime start, DateTime until, int size);
       SearchResponse Search(SearchQuery query);
       SearchResponse<TMessage> Search<TMessage>(string query);
       SearchResponse<TMessage> Search<TMessage>(string query, DateTime start, DateTime until);
       SearchResponse<TMessage> Search<TMessage>(string query, DateTime start, DateTime until, int size);
       SearchResponse<TMessage> Search<TMessage>(SearchQuery query);
       FieldResponse Field(FieldQuery query);
   }
}