using System;
using System.Net;
using Loggly.Responses;

namespace Loggly
{
    public enum MessageType
    {
        Plain,
        Json
    }
    public class LogglyMessage
    {
        public MessageType Type { get; set; }
        public string Content { get; set; }
    }


    public enum HttpRequestType
    {
        Get
        ,Post
    }

    class RequestState : ResponseState
    {
        public byte[] Payload { get; set; }
    }

    class ResponseState
    {
        public HttpWebRequest Request { get; set; }
        public Action<Response> Callback { get; set; }
    }
}