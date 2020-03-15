@echo off

echo press ENTER 3 times to upload to NuGet...

pause
nuget update -self
pause
pause
nuget push ..\..\..\..\src\ScottPlot\bin\Release\*.nupkg -Source https://api.nuget.org/v3/index.json
nuget push ..\..\..\..\src\ScottPlot.WinForms\bin\Release\*.nupkg -Source https://api.nuget.org/v3/index.json
nuget push ..\..\..\..\src\ScottPlot.WPF\bin\Release\*.nupkg -Source https://api.nuget.org/v3/index.json

pause