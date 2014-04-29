# Loggly .NET Driver

This is a .NET driver for [loggly.com](http://loggly.com).

## Logging Events

**Please note that as of the 2.0 release, async logging is the default. To log synchronously, use the corresponding `Sync` methods (`Log` vs `LogSync`)**

Create a new `Logger` with your input key:

	var logger = new Logger("my-long-key-that-i-got-when-setting-up-my-http-input");

OR
	
	var logger = new Logger("my-long-key-that-i-got-when-setting-up-my-http-input", "your-application-name");
	
	
Passing application name will help you to track your application easily in loggly dashboard if you use loggly-csharp library in multiple projects.

For JSON logging you can use `LogInfo`, `LogVerbose`, `LogWarning`, `LogError` methods that will create json objects with properties like category, message, exception (if applicable), extra data that you provide.

Use either a synchronous or asynchronous `Log` method.


## Searching Events on Loggly Dashboad

You can see logs on Loggly dashboard by using facet 

	userAgent:"loggly-csharp"

OR

	userAgent:"your-application-name"

## Searching Events using code

First, setup the username/password you want to connect with:

	LogglyConfiguration.Configure(c => c.AuthenticateWith("username", "password"));

Next, create a searcher with your domain:

	var searcher = new Searcher("mydomain");

Finally, use the various `Search` methods.

For JSON search you can use `SearchJson` methods.

Note that searching happens synchronously.


## Facets

First, setup the username/password you want to connect with:

	LogglyConfiguration.Configure(c => c.AuthenticateWith("username", "password"));

Next, create a facet with your domain:

	var facet = new Facet("mydomain");

Finally, use the various `GetDate`, `GetIp` and `GetInput* methods.

Getting facts is always synchronous


## Integration Tests

To run the integration tests, you'll need to place a `config.user` file in the test's debug folder (assuming you are running tests in debug). The file should look something like:

	<appSettings>
		<add key="IntegrationKey" value="YOUR KEY"></add>
		<add key="IntegrationUser" value="YOUR USERNAME"></add>
		<add key="IntegrationPassword" value="YOUR PASSWORD"></add>
	</appSettings>
