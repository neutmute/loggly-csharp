using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using Loggly.Config;
using Loggly.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loggly
{
    internal class SearchTransport : HttpTransportBase, ISearchTransport
    {
        private readonly ISearchConfiguration _config;
        public SearchTransport(ISearchConfiguration config)
        {
            _config = config;
        }
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
                var searchPathAndQuery = GetUrl(endPoint, parameters);
                var searchRequest = CreateRequest(searchPathAndQuery, HttpRequestType.Get);
                
                searchRequest.Credentials = new NetworkCredential(_config.Username, _config.Password);

                using (var response = searchRequest.GetResponse())
                {
                    var isFieldResponseResultExpected = typeof (T) == typeof (FieldResponse);
                    var responseBody = GetResponseBody(response);
                    T responseObject = isFieldResponseResultExpected
                        ? new FieldResponse(JObject.Parse(responseBody), parameters["fieldname"].ToString()) as T
                        : JsonConvert.DeserializeObject<T>(responseBody);

                    var responseAsSearchResponseBase = responseObject as SearchResponseBase;
                    if (responseAsSearchResponseBase != null)
                    {
                        responseAsSearchResponseBase.Transport = this;
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

        private string GetUrl(string endPoint, ICollection<KeyValuePair<string, object>> parameters)
        {
            var sb = new StringBuilder(100);
            sb.AppendFormat("https://{0}.loggly.com/", _config.Account);
            sb.Append(endPoint);

            if (parameters != null && parameters.Count > 0)
            {
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
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }
}