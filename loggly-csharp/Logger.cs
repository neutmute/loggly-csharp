using System;
using System.Collections.Generic;
using System.Threading;
using Loggly.Responses;
using Newtonsoft.Json;

namespace Loggly
{
    public class Logger : ILogger, IRequestContext
    {
        private string _url = "logs-01.loggly.com/";
        private readonly string _customerToken;

        public Logger(string customerToken, string alternativeUrl = null)
        {
            if (!string.IsNullOrEmpty(alternativeUrl))
                _url = alternativeUrl;

            _customerToken = customerToken;
        }

        public void Log(string message, params string[] tags)
        {
            Log(message, false, tags);
        }

        public void Log(string message, Action<LogResponse> callback, params string[] tags)
        {
            Log(message, false, callback, tags);
        }

        public void LogJson<TMessage>(TMessage message, params string[] tags)
        {
            LogJson(message, null, tags);
        }

        public void LogJson<TMessage>(TMessage message, Action<LogResponse> callback, params string[] tags)
        {
            Log(JsonConvert.SerializeObject(message), true, callback, tags);
        }

        public void LogJson(string message, params string[] tags)
        {
            Log(message, true, tags);
        }

        public void LogJson(string message, Action<LogResponse> callback, params string[] tags)
        {
            Log(message, true, callback, tags);
        }

        public string Url
        {
            get { return _url; }
        }

        private void Log(string message, bool json, params string[] tags)
        {
            Log(message, json, null, tags);
        }

        private void Log(string message, bool json, Action<LogResponse> callback, params string[] tags)
        {
            var communicator = new Communicator(this);
            var callbackWrapper = callback == null ? (Action<Response>)null : r =>
            {
                if (r.Success)
                {
                    var res = JsonConvert.DeserializeObject<LogResponse>(r.Raw);
                    res.Success = true;
                    callback(res);
                }
                else
                {
                    var res = new LogResponse { Success = false };
                    callback(res);
                }
            };
            communicator.SendPayload(Communicator.POST, string.Concat("inputs/", _customerToken), message, json, callbackWrapper, tags);
        }
    }
}