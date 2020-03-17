@echo off

echo.
echo ### GENERATING COOKBOOK ###

cd ScottPlotDevTools\bin\Debug\netcoreapp3.1
ScottPlotDevTools.exe -makeCookbook

echo.
echo ### CREATING AND PACKAGING DEMO ###
ScottPlotDevTools.exe -makeDemo

pause