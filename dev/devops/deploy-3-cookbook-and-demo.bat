@echo off

echo.
echo ### REBUILDING COOKBOOK GENERATOR ###
:: must first rebuild the cookbook generator so it learns of the new version number
dotnet build ScottPlotDevTools\ScottPlotDevTools.csproj --configuration Debug

echo.
echo ### GENERATING COOKBOOK ###
cd ScottPlotDevTools\bin\Debug\netcoreapp3.1
ScottPlotDevTools.exe -makeCookbook

echo.
echo ### CREATING AND PACKAGING DEMO ###
ScottPlotDevTools.exe -makeDemo

pause