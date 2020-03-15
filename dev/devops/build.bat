@echo off

cd ScottPlotDevTools\bin\Debug\netcoreapp3.1

echo press ENTER to incriment version and rebuild...
pause

ScottPlotDevTools.exe -incrimentVersion
ScottPlotDevTools.exe -makeCookbook
ScottPlotDevTools.exe -makeDemo
pause