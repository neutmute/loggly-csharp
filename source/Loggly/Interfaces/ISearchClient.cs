using System;
using System.Threading.Tasks;
using Loggly.Responses;

namespace Loggly
{
   public interface ISearchClient
   {
       Task<SearchResponse> Search(string query);
       Task<SearchResponse> Search(string query, DateTime start, DateTime until);
       Task<SearchResponse> Search(string query, DateTime start, DateTime until, int size);
       Task<SearchResponse> Search(SearchQuery query);
       Task<SearchResponse<TMessage>> Search<TMessage>(string query);
       Task<SearchResponse<TMessage>> Search<TMessage>(string query, DateTime start, DateTime until);
       Task<SearchResponse<TMessage>> Search<TMessage>(string query, DateTime start, DateTime until, int size);
       Task<SearchResponse<TMessage>> Search<TMessage>(SearchQuery query);
       Task<FieldResponse> Field(FieldQuery query);
   }
}