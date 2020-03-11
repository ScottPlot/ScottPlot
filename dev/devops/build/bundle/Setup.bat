@echo off
echo.
echo   This script installs Microsoft's .NET platform from the internet.
echo.
echo   Alternatively you could get it yourself: https://dotnet.microsoft.com/download
echo.
pause
powershell.exe -noLogo -ExecutionPolicy unrestricted -file dotnet-install.ps1
pause