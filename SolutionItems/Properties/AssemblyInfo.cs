using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: AssemblyCompany("Karl Seguin")]
[assembly: AssemblyCopyright("Copyright © Karl Seguin 2010")]
[assembly: AssemblyVersion("3.5.0.0")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: AssemblyProduct("loggly-csharp")]
[assembly: InternalsVisibleTo("Loggly.Tests")]
#if DEBUG
    [assembly: AssemblyConfiguration("Debug")]
    [assembly: AssemblyInformationalVersion("3.5.1-alpha-v2")]
#else
    [assembly: AssemblyConfiguration("Release")]
    [assembly: AssemblyInformationalVersion("3.5.0")]
#endif

