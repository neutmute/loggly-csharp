using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
            IEnumerable enumerableMessage = message as IEnumerable;
            if (enumerableMessage != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (object messageLine in enumerableMessage)
                    sb.AppendLine(JsonConvert.SerializeObject(
                        messageLine, 
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

                Log(sb.ToString(), true, callback, tags);
            }
            else
            {
                Log(JsonConvert.SerializeObject(
                    message, 
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), true, callback, tags);
            }
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
            bool isMultiLine = message.Contains("\n");

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
            communicator.SendPayload(Communicator.POST, string.Concat(isMultiLine ? "bulk/" : "inputs/", _customerToken), message, json, callbackWrapper, tags);
        }
    }
}