
#######################################################
# Deerso Domain Build Script
#######################################################
param(
    [ValidateSet('Debug', 'Release')]
    [string]
    $config = "Debug",

    [ValidateSet('Major', 'Minor', 'Patch')]
    [string]
    $increment = "Patch"
)

$solutionName = "Deerso.TestHelpers.sln"

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$rootFolder = (Get-Item $scriptPath).Parent.FullName
$srcFolder = "$rootFolder\src"

task -name Build -description "builds outdated source files" -action {
	Exec {
			msbuild $srcFolder/$solutionName /t:Build "/p:Configuration=$config" /v:minimal /nologo
	}
     if ($lastExitCode -ne 0) {
        throw "Error: Build Task failed"
    }
};

task -name Clean -description "deletes all build artifacts" -action {
	Exec {
		msbuild $srcFolder/$solutionName /t:Clean
	}
     if ($lastExitCode -ne 0) {
        throw "Error: Build Task failed"
    }
};

task -name BumpVersion -depends Build -description "bumps the version of the assemblies" -action {
	. $scriptPath/Bump-Version.ps1 -Increment $increment
}
task -name Rebuild -depends Clean, Build -description "builds all source files"

task -name Default -depends Build;