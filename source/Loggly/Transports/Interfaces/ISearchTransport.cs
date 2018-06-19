using Loggly.Config;
using Loggly.Responses;
using System.Threading.Tasks;

namespace Loggly
{
    public interface ISearchTransport
    {
        Task<SearchResponse> Search(SearchQuery query);
        Task<EntryJsonResponseBase> Search(EventQuery query);

        Task<SearchResponse<T>> Search<T>(SearchQuery query);

        Task<FieldResponse> Search(FieldQuery query);
    }
}