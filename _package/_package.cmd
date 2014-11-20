del /q *.nupkg
..\.nuget\nuget.exe pack ..\source\Loggly.Config\Loggly.Config.csproj -IncludeReferencedProjects -Prop Configuration=%1
..\.nuget\nuget.exe pack ..\source\Loggly\Loggly.csproj -IncludeReferencedProjects -Prop Configuration=%1