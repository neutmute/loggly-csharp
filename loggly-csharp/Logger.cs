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
            return LogSync(message, false);
        }

        public void Log(string message, params string[] tags)
        {
            Log(message, false);
        }

        public void Log(string message, Action<LogResponse> callback, params string[] tags)
        {
            Log(message, false, callback);
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
            });

            synchronizer.WaitOne();
            return response;
        }

        public void Log(string message, string category, params string[] tags)
        {
            Log(message, category, null, tags);
        }

        public void Log(string message, string category, IDictionary<string, object> data, params string[] tags)
        {
            var logEntry = new Dictionary<string, object>(data ?? new Dictionary<string, object>())
                        {
                           {"message", message}, {"category", category}
                        };
            var jsonLogEntry = JsonConvert.SerializeObject(logEntry);
            Log(jsonLogEntry, true);
        }

        public void LogInfo(string message, params string[] tags)
        {
            LogInfo(message, null, tags);
        }

        public void LogInfo(string message, IDictionary<string, object> data, params string[] tags)
        {
            Log(message, "info", data, tags);
        }

        public void LogVerbose(string message, params string[] tags)
        {
            LogVerbose(message, null, tags);
        }

        public void LogVerbose(string message, IDictionary<string, object> data, params string[] tags)
        {
            Log(message, "verbose", data, tags);
        }

        public void LogWarning(string message, params string[] tags)
        {
            LogWarning(message, null, tags);
        }

        public void LogWarning(string message, IDictionary<string, object> data, params string[] tags)
        {
            Log(message, "warning", data, tags);
        }

        public void LogError(string message, Exception ex, params string[] tags)
        {
            LogError(message, ex, null, tags);
        }

        public void LogError(string message, Exception ex, IDictionary<string, object> data, params string[] tags)
        {
            var exceptionData = new Dictionary<string, object>(data ?? new Dictionary<string, object>())
                             {
                                {"exception", ex.ToString()}
                             };
            Log(message, "error", exceptionData, tags);
        }

        public void Log(string message, bool json, params string[] tags)
        {
            Log(message, json, null, tags);
        }

        public void Log(string message, bool json, Action<LogResponse> callback, params string[] tags)
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