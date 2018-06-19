using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Loggly.Responses
{
    [JsonObject]
    public class SearchResponse : SearchResponseBase, IEnumerable<EventMessage>
    {

        public IEnumerator<EventMessage> GetEnumerator()
        {
            int page = 0;
            int returnedEntryCount = 0;
            var entryResonse = this.FirstEntryResponse ?? GetEntryJsonResponse(page).Result;

            while (true)
            {
                foreach (EventMessage eventMessage in (entryResonse as EntryJsonResponse).Events)
                {
                    returnedEntryCount++;
                    yield return eventMessage;
                }

                if (returnedEntryCount >= entryResonse.TotalEvents)
                    yield break;

                page++;
                entryResonse = GetEntryJsonResponse(page).Result;
            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override async Task<EntryJsonResponseBase> GetEntryJsonResponse(int page)
        {
            var eventQuery = new EventQuery {Rsid = this.Rsid.Id, Page = page};
            var entryResonse = await this.Transport.Search(eventQuery).ConfigureAwait(false);
            return entryResonse;
        }
    }
}
