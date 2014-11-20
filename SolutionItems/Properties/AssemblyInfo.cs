using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: AssemblyVersion("4.5.0.2")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: AssemblyProduct("loggly-csharp")]
[assembly: InternalsVisibleTo("Loggly.Tests")]
#if DEBUG
    [assembly: AssemblyConfiguration("Debug")]
    [assembly: AssemblyInformationalVersion("4.5.0.2-alpha-v1")]    // trigger pre release package
#else
    [assembly: AssemblyConfiguration("Release")]
#endif

