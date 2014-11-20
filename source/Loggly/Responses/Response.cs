namespace Loggly.Responses
{
   public class Response
   {
      public bool Success { get; set; }
      public string Raw { get; set; }
      public ErrorMessage Error { get; set; }

       private Response()
       {
           
       }

       #region Factory

       public static Response CreateSuccess(string raw)
       {
           return new Response {Success = true, Raw = raw};
       }

       public static Response CreateError(ErrorMessage error)
       {
           return new Response {Success = false, Error = error};
       }

       #endregion

   }
}