# Loggly .NET Driver

This is a .NET driver for [loggly.com](http://loggly.com) VERSION 2. It is a fork from https://github.com/karlseguin/loggly-csharp which currently only provides support for version 1. Besides that this repository provides a cleaner API and better support for JSON logging.

## Logging Events

Create a new `Logger` with your customer token:

	var logger = new Logger("my-long-key-that-i-got-when-setting-up-customer-tokens");

Use either a synchronous or asynchronous `Log` method.


## Searching Events

First, setup the username/password you want to connect with:

	LogglyConfiguration.Configure(c => c.AuthenticateWith("username", "password"));

Next, create a searcher with your domain:

	var searcher = new Searcher("mydomain");

Finally, use the various `Search` methods.

Note that searching happens synchronously.


## Integration Tests

To run the integration tests, you'll need to place a `config.user` file in the test's debug folder (assuming you are running tests in debug). The file should look something like:

	<appSettings>
		<add key="IntegrationKey" value="YOUR KEY"></add>
		<add key="IntegrationAccount" value="YOUR ACCOUNT NAME (SUBDOMAIN)"></add>
		<add key="IntegrationUser" value="YOUR USERNAME"></add>
		<add key="IntegrationPassword" value="YOUR PASSWORD"></add>
	</appSettings>
