Write-Host "Running in CI Mode";
import-module Psake;
$psake.use_exit_on_error = $true;   
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
invoke-psake $scriptPath\build-Tasks.ps1 -parameters @{"config"="Release"} 
remove-module psake