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
        private readonly string _url = "logs-01.loggly.com/";
        private readonly string _customerToken;

        public Logger(string customerToken)
        {
            _customerToken = customerToken;
        }

        public string Url
        {
            get { return _url; }
        }

        public void Log(string message)
        {
            Log(message, null);
        }

        public void Log(string message, Action<LogResponse> callback)
        {
            var communicator = new Communicator(this);
            var callbackWrapper = callback == null
                ? (Action<Response>) null
                : r =>
                {
                    if (r.Success)
                    {
                        var res = JsonConvert.DeserializeObject<LogResponse>(r.Raw);
                        res.Success = true;
                        callback(res);
                    }
                    else
                    {
                        var res = new LogResponse {Success = false};
                        callback(res);
                    }
                };

            communicator.SendPayload(Communicator.POST, "inputs/", _customerToken, true, callbackWrapper);
        }

        public void Log<TMessage>(TMessage logObject)
        {
            Log(logObject, null);
        }

        public void Log<TMessage>(TMessage logObject, Action<LogResponse> callback)
        {
            var enumerableMessage = logObject as IEnumerable;
            if (enumerableMessage != null)
            {
                var sb = new StringBuilder();
                foreach (object messageLine in enumerableMessage)
                {
                    sb.AppendLine(ToString(messageLine));
                }

                Log(sb.ToString(), callback);
            }
            else
            {
                Log(ToString(logObject), callback);
            }
        }

        private static string ToString(object messageLine)
        {
            if (messageLine is string)
            {
                return messageLine as string;
            }

            var asJson = JsonConvert.SerializeObject(messageLine,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
            return asJson;
        }
    }
}