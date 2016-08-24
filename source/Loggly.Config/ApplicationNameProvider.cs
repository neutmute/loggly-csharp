#if FEATURE_SYSTEM_CONFIGURATION
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Loggly.Config
{
    /// <summary>
    /// In order, resolves 
    /// * the loggly app name
    /// * new relic app name
    /// * assembly name [disabnled - might be unreliable]
    /// </summary>
    class ApplicationNameProvider
    {
        public string GetName()
        {
            var newRelicAppName = ConfigurationManager.AppSettings["NewRelic.AppName"];
            string name= null;
            if (LogglyAppConfig.HasAppCopnfig && !string.IsNullOrEmpty(LogglyAppConfig.Instance.ApplicationName))
            {
                name = LogglyAppConfig.Instance.ApplicationName;
            }
            else if (!string.IsNullOrEmpty(newRelicAppName))
            {
                // save some config transform work
                name = newRelicAppName;
            }
            //else
            //{
            //    name = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            //}
            return name;
        }
    }
}
#endif
