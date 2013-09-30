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

        public LogResponse LogSync(string message, params string[] tags)
        {
            return LogSync(message, false, tags);
        }

        public LogResponse LogSync<TMessage>(TMessage message, params string[] tags)
        {
            return LogSync(JsonConvert.SerializeObject(message), true, tags);
        }

        public void Log(string message, params string[] tags)
        {
            Log(message, false);
        }

        public void Log(string message, Action<LogResponse> callback, params string[] tags)
        {
            Log(message, false, callback);
        }

        public void Log<TMessage>(TMessage message, params string[] tags)
        {
            Log(message, null, tags);
        }

        public void Log<TMessage>(TMessage message, Action<LogResponse> callback, params string[] tags)
        {
            Log(JsonConvert.SerializeObject(message), true, callback, tags);
        }

        public string Url
        {
            get { return _url; }
        }

        public LogResponse LogSync(string message, bool json, params string[] tags)
        {
            var synchronizer = new AutoResetEvent(false);

            LogResponse response = null;
            Log(message, json, r =>
            {
                response = r;
                synchronizer.Set();
            },
            tags);

            synchronizer.WaitOne();
            return response;
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