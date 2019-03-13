$nuget="../tools/Nuget/nuget.exe"

$packages = Get-Project -All | Get-Package

$styleguide = $packages | Where-Object { $_.Id -eq "Empeek.Styleguide.Dotnet" }

if ( @($styleguide).Count -eq 0 ) {
    Write-Host "Message: The package Empeek.Styleguide.Dotnet is not installed in solution"
    exit 1;
}