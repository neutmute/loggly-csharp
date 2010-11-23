using System;
using Loggly.Responses;

namespace Loggly
{
   public class Facet : IFacet, IRequestContext
   {
      private const string _domain = ".loggly.com/";
      private readonly string _url;

      public Facet(string subdomain)
      {
         _url = string.Concat(subdomain, _domain);
      }

      public string Url
      {
         get { return _url; }
      }

      public FacetResponse GetDate(string query)
      {
         return GetDate(new FacetQuery { Query = query });
      }

      public FacetResponse GetDate(FacetQuery query)
      {
         return DoQuery("api/facets/date", query);
      }

      public FacetResponse GetIp(string query)
      {
         return GetIp(new FacetQuery { Query = query });
      }

      public FacetResponse GetIp(FacetQuery query)
      {
         return DoQuery("api/facets/ip", query);
      }

      public FacetResponse GetInput(string query)
      {
         return GetInput(new FacetQuery { Query = query });
      }

      public FacetResponse GetInput(FacetQuery query)
      {
         return DoQuery("api/facets/input", query);
      }

      private FacetResponse DoQuery(string endpoint, FacetQuery query)
      {
         var communicator = new Communicator(this);
         return communicator.GetPayload<FacetResponse>(endpoint, query.ToParameters());
      }
   }
}