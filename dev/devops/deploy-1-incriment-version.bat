@echo off

echo.
echo ### INCRIMENTING VERSION ###
echo.
echo press ENTER to incriment version (modifying all .csproj files)...
pause

dotnet build ScottPlotDevTools\ScottPlotDevTools.csproj --configuration Debug
cd ScottPlotDevTools\bin\Debug\netcoreapp3.1
ScottPlotDevTools.exe -incrimentVersion
pause