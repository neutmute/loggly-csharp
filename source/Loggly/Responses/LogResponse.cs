using Newtonsoft.Json;

namespace Loggly
{
    public enum ResponseCode
    {
        Unknown = 0,
        Success,
        Error,
        AssumedSuccess,
        SendDisabled
    }
    public class LogResponse
    {
        public ResponseCode Code { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            if (Code == ResponseCode.Success && string.IsNullOrEmpty(Message))
                return "Success=Success, Code=";
            else
                return string.Format("Success={0}, Code={1}", Code, Message);
        }
    }
}