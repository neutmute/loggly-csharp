# .NET Client for Loggly  #

This is a .NET client for [loggly.com](http://loggly.com).

Install via nuget with

	Install-Package loggly-csharp

**Note** Version 3.5 has completely broken compatibility with prior versions to bring major improvements.
Any existing code will need fixing.

## Configuration ##
Configuration is done via app.config. The minimal amount config you require is to specify your customer token: 

	<configuration>
	  <configSections>
	    <section name="loggly" type="Loggly.Config.LogglyAppConfig, Loggly.Config, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null"/>
	  </configSections>
	  <loggly xmlns="Loggly" customerToken="your token" />
	</configuration>
 
When you get that working, take the training wheels off and go crazy:

	<loggly 
	  xmlns="Loggly" 
	  applicationName="Loggly.Example" 
	  customerToken="!!!!your token here!!!!" 
	  messageTransport="SyslogUdp"
	  throwExceptions="true">
	  <search account="your_loggly_account" username="your_loggly_username" password="myLittleP0ny!"/>  
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

Complex tags have the `formatter` attribute so you may specify your own `string.Format`.
The `Assembly` attribute is available as an optional parameter so you can roll your own tags too.

If you don't need programatially driven tags, just write your simple tags. If your tags don't appear, check the [Loggly restrictions](https://www.loggly.com/docs/tags/) for tag formats. 

As long as you keep the `xmlns` attribute, Visual Studio will provide auto completion.
If you prefer to set configuration programatically, specify the values via the static `LogglyConfig.Instance` at application startup.

## Transports ##
Three different transports may be specified with the `messageTransport` attribute:

### Http ###
The default transport is HTTP posting to Loggly on port 443. Note that the application and host attributes [are not supported by HTTP](https://community.loggly.com/customer/portal/questions/8416949--host-field-for-source-groups?b_id=50).

### SyslogUdp
If you specify an `applicationName` in the config, the syslog UDP transport will populate the field so it may be filtered in a source group. Host is also automatically populated by  the client. Udp messages are sent in plain text.  

### SyslogSecure
Has the advantages of SyslogUdp as well as transmitting via the secure TLS TCP channel so that your logs are encrypted over the wire.

## Loggly.Example
This project has sample code to demonstrate the client.
Before starting, copy the example config into the user config, eg:

	C:\loggly-csharp>copy .\source\Loggly.Example\example.loggly.user.config .\source\Loggly.Example\loggly.user.config

And configure the file with your own customer token.

Of course, there is no need to have a config source in your real app, this is just a convenience for this public repository.

## LogglyClient
Send simple text messages with something like this.

	ILogglyClient _loggly = new LogglyClient();
	_loggly.Log("A simple text message at {0}", DateTime.Now);

Or log an entire object and let the client send it as structured JSON

	_loggly.Log(new MyAwesomeObjectToLog());

## SearchClient

Currently broken but not far from working. 
Feel free to submit a Pull Request with a fix.

## Logging Adapters
See [nlog-targets-loggly](https://github.com/joefitzgerald/nlog-targets-loggly) for a ready made NLog target that uses this package