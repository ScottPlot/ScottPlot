@echo off

echo.
echo ### REBUILDING COOKBOOK GENERATOR ###
:: must first rebuild the cookbook generator so it learns of the new version number
cd ScottPlotDevTools
dotnet build ScottPlotDevTools.csproj --configuration Debug
cd bin\Debug\netcoreapp3.1

echo.
echo ### GENERATING COOKBOOK ###
ScottPlotDevTools.exe -makeCookbook

echo.
echo ### CREATING AND PACKAGING DEMO ###
ScottPlotDevTools.exe -makeDemo

pause