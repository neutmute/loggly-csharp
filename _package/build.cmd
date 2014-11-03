msbuild ..\loggly.sln /p:configuration=debug  /t:clean,build
msbuild ..\loggly.sln /p:configuration=release /t:clean,build