using System;

namespace Loggly.Responses
{
   public class Response
   {
      public bool Success { get; set; }
      public string Raw { get; set; }
      public ErrorMessage Error { get; set; }

      public static Response CreateSuccess(string raw)
      {
         return new Response { Success = true, Raw = raw };
      }
      public static Response CreateError(ErrorMessage error)
      {
         return new Response { Success = false, Error = error };
      }
   }

   public class ErrorMessage
   {
      public string Error { get; set; }
      public string Info { get; set; }
      public string Maintenance { get; set; }
      public Exception InnerException { get; set; }
   }
}