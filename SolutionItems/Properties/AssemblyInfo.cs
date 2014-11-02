using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: AssemblyVersion("4.5.0.0")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: AssemblyProduct("loggly-csharp")]
[assembly: InternalsVisibleTo("Loggly.Tests")]
#if DEBUG
    [assembly: AssemblyConfiguration("Debug")]
    [assembly: AssemblyInformationalVersion("4.5.0-alpha-v1")]
#else
    [assembly: AssemblyConfiguration("Release")]
    [assembly: AssemblyInformationalVersion("4.5.0")]
#endif

