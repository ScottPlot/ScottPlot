@echo off

:: this script requires nuget.exe to be in this folder
:: https://www.nuget.org/downloads

echo.
echo ### UPDATING NUGET ###
nuget update -self

echo.
echo WARNING! This script will UPLOAD packages to nuget.org
echo.
press ENTER 3 times to proceed...
pause
pause
pause

echo.
echo ### UPLOADING PACKAGES TO NUGET ###
nuget push ..\..\src\ScottPlot\bin\Release\*.nupkg -Source https://api.nuget.org/v3/index.json
nuget push ..\..\src\ScottPlot.WinForms\bin\Release\*.nupkg -Source https://api.nuget.org/v3/index.json
nuget push ..\..\src\ScottPlot.WPF\bin\Release\*.nupkg -Source https://api.nuget.org/v3/index.json
pause