if not exist Download mkdir Download
if not exist Download\package mkdir Download\package
if not exist Download\package\lib mkdir Download\package\lib
if not exist Download\package\lib\net4 mkdir Download\package\lib\net4

tools\ilmerge.exe /lib:loggly-csharp\bin\Release /internalize /ndebug /v2 /out:Download\Loggly.dll Loggly.dll Newtonsoft.Json.dll

copy LICENSE.txt Download

copy loggly-csharp\bin\Release\Loggly.dll Download\Package\lib\net4\

nuget.exe pack loggly.nuspec -b Download\Package -o Download