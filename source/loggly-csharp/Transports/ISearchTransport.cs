using Loggly.Responses;

namespace Loggly
{
    public interface ISearchTransport
    {
        SearchResponse Search(SearchQuery query);
        EntryJsonResponseBase Search(EventQuery query);

        SearchResponse<T> Search<T>(SearchQuery query);

        FieldResponse Search(FieldQuery query);
    }
}