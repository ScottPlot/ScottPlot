@echo off

echo.
echo ### REBUILDING DEV TOOLS ###
echo.
dotnet build ScottPlotDevTools\ScottPlotDevTools.csproj --configuration Debug
cd ScottPlotDevTools\bin\Debug\netcoreapp3.1

echo.
echo ### INCRIMENTING VERSION ###
echo.
ScottPlotDevTools.exe -incrimentVersion
pause