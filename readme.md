#![](https://raw.githubusercontent.com/neutmute/loggly-csharp/master/SolutionItems/loggly.png) .NET Client for Loggly  

A .NET client for loggly. Supporting Https, Syslog UDP and encrypted Syslog TCP transports.
Install via nuget with

	Install-Package loggly-csharp

**Note** Version 3.5 has completely broken compatibility with prior versions to bring major improvements.
Any existing code targeting versions < 3.0 will require modification.

## Configuration
Configuration is done via your app.config. The minimal amount config you require is to specify your customer token: 

	<configuration>
	  <configSections>
	    <section name="loggly" type="Loggly.Config.LogglyAppConfig, Loggly.Config, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null"/>
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

### ApplicationName
This is an optional attribute. If you leave this attribute out but have `NewRelic.AppName` in your app.config, then it will pick that value up automatically.
Render your application name as a tag by using the `HostnameTag` (keep reading).

### IsEnabled
Set it to false on your development machine so that no events are sent to loggly. 

### Transports ##
Three different transports may be specified with the `logTransport` attribute in the `transport` element.
The transport element is entirely optional and will default to the Https. The available transports are as follows:

#### Https
The default transport, posting to Loggly on port 443. Note that application and host source group filtering [are not supported by HTTP](https://community.loggly.com/customer/portal/questions/8416949--host-field-for-source-groups?b_id=50), so you may wish to consider a Syslog transport.

#### SyslogUdp
If you specify an `applicationName` in the config, the syslog UDP transport will populate the field so it may be filtered in a source group. Host is also automatically populated by  the client. Udp messages are sent in plain text.  

### SyslogSecure
Has the advantages of SyslogUdp as well as transmitting via the secure TLS TCP channel so that your logs are encrypted over the wire. Syslog supports JSON formatted messages just like Https.

### Tags 
Simple tags are string literals added to the app.config.

Complex tags are types inheriting from `ComplexTag`. They have the `formatter` attribute so you may specify your own `string.Format`.
The `Assembly` attribute is available as an optional parameter so you can roll your own tags too.

Loggly has certain restrictions around characters allowed in tags. This library replaces illegal characters automatically with an underscore.

If you don't need programatially driven tags, just write your simple tags. If your tags don't appear, check the [Loggly restrictions](https://www.loggly.com/docs/tags/) for tag formats. 

### Programmatic Configuration

If you prefer to set configuration programatically, specify the values via the static `LogglyConfig.Instance` class at application startup.

### Suppression
Sometimes you might emit something to a flat file log that doesn't make sense in loggly, such as a delimiting line of dashes: ---------

Add a property to your nLog event with the name `syslog-suppress` to filter these out so they don't transmit to loggly.

## Usage: LogglyClient
Send simple text messages with something like this.

	ILogglyClient _loggly = new LogglyClient();
	_loggly.Log("A simple text message at {0}", DateTime.Now);

Or log an entire object and let the client send it as structured JSON

	_loggly.Log(new MyAwesomeObjectToLog());

## Usage: SearchClient

See example project below in conjunction with the [loggly docs](https://www.loggly.com/docs/api-retrieving-data/)

## Loggly.Example Project
The solution has an example project with sample code to demonstrate the client.
Before starting, copy the example config into the user config, eg:

	C:\loggly-csharp>copy .\source\Loggly.Example\example.loggly.user.config .\source\Loggly.Example\loggly.user.config

And configure the file with your own customer token.

Of course, there is no need to have a config source in your real app, this is just a convenience for this public repository.


## Projects using this client
[nlog-targets-loggly](https://github.com/joefitzgerald/nlog-targets-loggly) An NLog target