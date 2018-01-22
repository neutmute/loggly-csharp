using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Loggly.Config;
using Loggly.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loggly
{
    internal class SearchTransport : ISearchTransport
    {
        private readonly ISearchConfiguration _config;
        private HttpClient _httpClient;

        public SearchTransport(ISearchConfiguration config)
        {
            _config = config;
            _httpClient=new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", config.Username, config.Password))));
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
            return Search<EntryJsonResponse>("apiv2/events", parameters);
        }

        public FieldResponse Search(FieldQuery query)
        {
            var parameters = query.ToParameters();
            return Search<FieldResponse>($"apiv2/fields/{query.FieldName}/", parameters);
        }

        private T Search<T>(string endPoint, IDictionary<string, object> parameters)
            where T : class
        {
            try
            {
                var searchPathAndQuery = GetUrl(endPoint, parameters);
                
                using (var response = _httpClient.GetAsync(searchPathAndQuery).Result)
                {
                    var isFieldResponseResultExpected = typeof(T) == typeof(FieldResponse);
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
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
                    LogglyException.Throw(responseBody);
                    return null;
                }

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
                    sb.Append(WebUtility.UrlEncode(kvp.Value.ToString()));
                    sb.Append("&");
                }
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

    }
}