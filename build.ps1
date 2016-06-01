param(
    [string]$packageVersion = $null,
    [string]$configuration = "Release"
)

. ".\common.ps1"

$solutionName = "loggly"
$sourceUrl = "https://github.com/neutmute/loggly-csharp"

function init {
    # Initialization
    $global:rootFolder = Split-Path -parent $script:MyInvocation.MyCommand.Path
    $global:rootFolder = Join-Path $rootFolder .
    $global:packagesFolder = Join-Path $rootFolder packages
    $global:outputFolder = Join-Path $rootFolder _output
    $global:msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

    _WriteOut -ForegroundColor $ColorScheme.Banner "-= $solutionName Build =-"
    _WriteConfig "rootFolder" $rootFolder
}

function restorePackages{
    _WriteOut -ForegroundColor $ColorScheme.Banner "nuget, gitlink restore"
    
    New-Item -Force -ItemType directory -Path $packagesFolder
    _DownloadNuget $packagesFolder
    nuget restore
    nuget install gitlink -SolutionDir "$rootFolder" -ExcludeVersion
}

function nugetPack{
    _WriteOut -ForegroundColor $ColorScheme.Banner "Nuget pack"
    
    New-Item -Force -ItemType directory -Path $outputFolder

    if(!(Test-Path Env:\nuget )){
        $env:nuget = nuget
    }
    if(!(Test-Path Env:\PackageVersion )){
        $env:PackageVersion = "1.0.0.0"
    }

    nuget pack $rootFolder\Source\Loggly\Loggly.csproj -o $outputFolder -IncludeReferencedProjects -p Configuration=$configuration -Version $env:PackageVersion
    nuget pack $rootFolder\Source\Loggly.Config\Loggly.Config.csproj -IncludeReferencedProjects -o $outputFolder -p Configuration=$configuration -Version $env:PackageVersion
}

function nugetPublish{

    if(Test-Path Env:\nugetapikey ){
        _WriteOut -ForegroundColor $ColorScheme.Banner "Nuget publish..."
        &nuget push .\_output\* -ApiKey "$env:nugetapikey" -source https://www.nuget.org
    }
    else{
        _WriteOut -ForegroundColor Yellow "nugetapikey environment variable not detected. Skipping nuget publish"
    }
}

function buildSolution{

    _WriteOut -ForegroundColor $ColorScheme.Banner "Build Solution"
    & $msbuild "$rootFolder\$solutionName.sln" /p:Configuration=$configuration

    &"$rootFolder\packages\gitlink\lib\net45\GitLink.exe" $rootFolder -u $sourceUrl
}

function executeTests{

    Write-Host "Execute Tests"
    $nunitConsole = "$rootFolder\packages\NUnit.Runners.2.6.3\tools\nunit-console.exe"
    & $nunitConsole .\Source\Loggly.Tests\bin\$configuration\Loggly.Tests.dll

    checkExitCode
}

init

restorePackages

buildSolution

executeTests

nugetPack

nugetPublish

Write-Host "Build $env:PackageVersion complete"