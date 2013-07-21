if not exist Download mkdir Download
if not exist Download\package mkdir Download\package
if not exist Download\package\lib mkdir Download\package\lib
if not exist Download\package\lib\net35 mkdir Download\package\lib\net35

copy loggly-csharp\bin\Release\Loggly.dll Download\Package\lib\net35\

nuget.exe pack loggly.nuspec -BasePath Download\Package -o Download