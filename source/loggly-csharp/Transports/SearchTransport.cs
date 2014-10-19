using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using Loggly.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loggly
{
    public class SearchTransport : HttpTransportBase, ISearchTransport
    {

        public SearchResponse Search(SearchQuery query)
        {
            var parameters = query.ToParameters();
            return Search<SearchResponse>("apiv2/search", parameters);
        }

        public SearchResponse<T> Search<T>(SearchQuery query)
        {
            var parameters = query.ToParameters();
            return Search<SearchResponse<T>>("apiv2/search", parameters);
        }
        
        public EntryJsonResponseBase Search(EventQuery query)
        {
            var parameters = query.ToParameters();
            return Search<EntryJsonResponseBase>("apiv2/events", parameters);
        }

        public FieldResponse Search(FieldQuery query)
        {

            var parameters = query.ToParameters();
            return Search<FieldResponse>("apiv2/fields", parameters);
        }


        private T Search<T>(string endPoint, IDictionary<string, object> parameters)
            where T : class
        {
            try
            {
                var searchPathAndQuery = BuildPathAndQuery(endPoint, parameters);
                var searchRequest = CreateRequest(searchPathAndQuery, HttpRequestType.Post, null, null);

                using (var response = searchRequest.GetResponse())
                {
                    T responseObject = typeof(T) == typeof(FieldResponse)
                        ? new FieldResponse(JObject.Parse(GetResponseBody(response)), parameters["fieldname"].ToString()) as T
                        : JsonConvert.DeserializeObject<T>(GetResponseBody(response));

                    if (responseObject is SearchResponseBase)
                    {
                        var searchResponse = responseObject as SearchResponseBase;
                        searchResponse.Transport = this;
                    }

                    return responseObject;
                }
            }
            catch (WebException ex)
            {
                LogglyException.Throw(ex, GetResponseBody(ex.Response));
                return null;
            }
            catch (Exception ex)
            {
                LogglyException.Throw(ex, ex.Message);
                return null;
            }
        }

        private static string BuildPathAndQuery(string endPoint, ICollection<KeyValuePair<string, object>> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return endPoint;
            }
            var sb = new StringBuilder(100);
            sb.Append(endPoint);
            sb.Append('?');
            foreach (var kvp in parameters)
            {
                if (kvp.Value == null)
                {
                    continue;
                }
                sb.Append(kvp.Key);
                sb.Append('=');
                sb.Append(HttpUtility.UrlEncode(kvp.Value.ToString()));
                sb.Append("&");
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }
    }
}