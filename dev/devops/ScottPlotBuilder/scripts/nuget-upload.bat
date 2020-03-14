@echo off

echo press ENTER 3 times to upload to NuGet...

pause
pause
pause
nuget push ../../../../src/ScottPlot/ScottPlot.csproj -Source https://api.nuget.org/v3/index.json
nuget push ../../../../src/ScottPlot.WinForms/ScottPlot.WinForms.csproj -Source https://api.nuget.org/v3/index.json
nuget push ../../../../src/ScottPlot.WPF/ScottPlot.WPF.csproj -Source https://api.nuget.org/v3/index.json

pause