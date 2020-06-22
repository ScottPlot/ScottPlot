@echo off

:: this script requires nuget.exe to be in this folder
:: https://www.nuget.org/downloads
:: and have your API key stored on your systeminfo
:: nuget SetApiKey 123456789

echo.
echo ### UPDATING NUGET ###
nuget update -self

echo.
echo WARNING! This script will UPLOAD packages to nuget.org
echo.
echo press ENTER 3 times to proceed...
pause
pause
pause

echo.
echo ### REBUILDING SOLUTION ###
dotnet build ..\..\src\ScottPlotV4.sln --verbosity quiet --configuration Release

echo.
echo ### UPLOADING [ScottPlot] TO NUGET ###
nuget push ..\..\src\ScottPlot\bin\Release\*.nupkg -Source https://api.nuget.org/v3/index.json

echo.
echo ### UPLOADING [ScottPlot.WinForms] TO NUGET ###
nuget push ..\..\src\ScottPlot.WinForms\bin\Release\*.nupkg -Source https://api.nuget.org/v3/index.json

echo.
echo ### UPLOADING [ScottPlot.WPF] TO NUGET ###
nuget push ..\..\src\ScottPlot.WPF\bin\Release\*.nupkg -Source https://api.nuget.org/v3/index.json

echo.
pause