#![](https://raw.githubusercontent.com/neutmute/loggly-csharp/master/SolutionItems/loggly.png) .NET Client for Loggly  

[![loggly-csharp MyGet Build Status](https://www.myget.org/BuildSource/Badge/loggly-csharp?identifier=096040c5-29c7-4254-9b71-611f780d43ff)](https://www.myget.org/) ![Version](https://img.shields.io/nuget/v/loggly-csharp.svg)

A .NET client for loggly. Supporting Https, Syslog UDP and encrypted Syslog TCP transports.
Install via nuget with

	Install-Package loggly-csharp

## Configuration via app.Config
Configuration may be done via your app.config. The minimal amount config you require is to specify your `customerToken`: 

	<configuration>
	  <configSections>
	    <section name="loggly" type="Loggly.Config.LogglyAppConfig, Loggly.Config"/>
	  </configSections>
	  <loggly xmlns="Loggly" customerToken="your token here" />
	</configuration>
 
When you get that working, take the training wheels off and go crazy:

	<loggly 
	  xmlns="Loggly" 
	  applicationName="MyAwesomeApp" 
	  customerToken="your token here" 
	  isEnabled="true"
	  throwExceptions="true">

  	  <transport logTransport="Https" endpointHostname="logs-01.loggly.com" endpointPort="443"/>

	  <search account="your_loggly_account" username="a_loggly_username" password="myLittleP0ny!"/>
  
	  <tags>
	    <simple>
	      <tag value="winforms"/>
	    </simple>
	    <complex>
	      <tag type="Loggly.HostnameTag" formatter="host-{0}"/>
	      <tag type="Loggly.ApplicationNameTag" formatter="application-{0}"/>
	      <tag type="Loggly.OperatingSystemVersionTag" formatter="os-{0}"/>
	      <tag type="Loggly.OperatingSystemPlatformTag" formatter="platform-{0}"/>
	    </complex>
	  </tags>
	</loggly>

## Configuration via Code

The following sample shows how to configure loggly via code - useful for Azure/Asp.NET core scenarios. Be sure to do this very early in the application life cycle in order to capture your early logs.

    var config = LogglyConfig.Instance;
    config.CustomerToken = "1ceb00da-cec7-45cb-8135-d3adbeef";
    config.ApplicationName = $"MyApp-{_environmentName}";

    config.Transport.EndpointHostname = "logs-01.loggly.com";
    config.Transport.EndpointPort = 443;
    config.Transport.LogTransport = LogTransport.Https;

    var ct = new ApplicationNameTag();
    ct.Formatter = "application-{0}";
    config.TagConfig.Tags.Add(ct);

## Proxy 
To set a global proxy for WebRequests used by HTTPS, you can do something like

    System.Net.WebRequest.DefaultWebProxy = new WebProxy("http://proxy.net:3128");

## Configuration Detail

### applicationName
This is an optional attribute. If you leave this attribute out but have `NewRelic.AppName` in your app.config, then it will pick that value up automatically.
Render your application name as a tag by using the `HostnameTag` (keep reading).

### isEnabled
Set it to false on your development machine so that no events are sent to loggly. 

### transport
Three different transports may be specified with the `logTransport` attribute in the `transport` element.
The transport element is entirely optional and will default to the Https. The available transports are as follows:

#### logTransport="Https"
The default transport, posting to Loggly on port 443. Note that application and host source group filtering [are not supported by HTTP](https://community.loggly.com/customer/portal/questions/8416949--host-field-for-source-groups?b_id=50), so you may wish to consider a Syslog transport.

#### logTransport="SyslogUdp"
If you specify an `applicationName` in the config, the syslog UDP transport will populate the field so it may be filtered in a source group. Host is also automatically populated by  the client. Udp messages are sent in plain text.  

#### logTransport="SyslogTcp"
Uses Syslog over TCP but is not encrypted. Note this needs to be on port 514 for loggly. Port 514 will be selected if not specified.   

#### logTransport="SyslogSecure"
Transmits using syslog messages via a secure TLS TCP channel so that your logs are encrypted over the wire. Syslog supports JSON formatted messages just like Https. Port 6514 is required for loggly and will be the default if not specified.

#### tags 
`simple` tags are string literals added to the app.config. What you see is what you get.

`complex` tags inherit from `ComplexTag`. They have the `formatter` attribute so you may specify your own `string.Format`.
The `Assembly` attribute is available as an optional parameter so you can roll your own tags too.

Loggly has certain restrictions around characters allowed in tags. This library automatically replaces illegal characters with an underscore.

## Usage: LogglyClient
Send simple text messages with something like this.

	ILogglyClient _loggly = new LogglyClient();
    var logEvent = new LogglyEvent();
    logEvent.Data.Add("message", "Simple message at {0}", DateTime.Now);
    _loggly.Log(logEvent);

Or log an entire object and let the client send it as structured JSON

	logEvent.Data.Add("context", new GlimmerWingPony());
    _loggly.Log(logEvent);

The `Log` method returns `Task<LogResponse>` so it is asynchronous by default but can be awaited. Only the Https transport would be worth awaiting as the Syslog transports are fire and forget. 

## Usage: SearchClient

See example project below in conjunction with the [loggly docs](https://www.loggly.com/docs/api-retrieving-data/)

## Loggly.Example Project
The solution has an example project with sample code to demonstrate the client.
Before starting, copy the example config into the user config, eg:

	C:\loggly-csharp>copy .\source\Loggly.Example\example.loggly.user.config .\source\Loggly.Example\loggly.user.config

And configure the file with your own customer token.

Of course, there is no need to have a config source in your real app, this is just a convenience for this public repository.

## Contributions
Contributions are welcome.

* Open an issue to discuss your proposed changes
* Branch your PR from the `develop` branch
* `.\build.ps1` is compatible with [myget](https://www.myget.org) and [appveyor](https://www.appveyor.com/) build services for dogfooding your modifications
* `.\build.ps1` / Visual Studio 2015 will require VS2015 command prompt / [.NET core tooling](https://www.microsoft.com/net/core#windows) to compile the solution

## Projects using this client
* [nlog-targets-loggly](https://github.com/joefitzgerald/nlog-targets-loggly) An NLog target
* [Serilog.Sinks.Loggly](https://github.com/serilog/serilog/tree/master/src/Serilog.Sinks.Loggly) Serilog sink
* [Loggly.CompositeC1](https://www.nuget.org/packages/Loggly.CompositeC1) TraceListener for Composite C1 CMS

## History
### v4.6
* .NET Standard / Core compatible

### v4.5.0.3
* Bug fixes for syslog transports

###v4.5 
* Targets framework 4.5
* Log method returns `Task<LogResponse>` for async/await compatibility

###v3.5
* New maintainer [neutmute](https://github.com/neutmute)
* Refactored API with new Config assembly
* Syslog UDP and TCP support added

###v2.0 and prior
* Maintained by [Karl Seguin](https://github.com/karlseguin) 
 
