using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

#if DEBUG
    [assembly: AssemblyConfiguration("Debug")]
#else
    [assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AllowPartiallyTrustedCallers]
[assembly: AssemblyVersion("4.6.1.0")]
[assembly: AssemblyProduct("loggly-csharp")]
[assembly: InternalsVisibleTo("Loggly.Tests")]

