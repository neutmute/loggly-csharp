using System.Reflection;
using System.Security;

[assembly: AssemblyCompany("Karl Seguin")]
[assembly: AssemblyCopyright("Copyright © Karl Seguin 2010")]
[assembly: AssemblyVersion("3.5.0.0")]
[assembly: AllowPartiallyTrustedCallers]

#if DEBUG
    [assembly: AssemblyProduct("loggly-csharp")]
    [assembly: AssemblyConfiguration("Release")]
#else
    [assembly: AssemblyProduct("loggly-csharp")]
    [assembly: AssemblyConfiguration("Release")]
#endif

