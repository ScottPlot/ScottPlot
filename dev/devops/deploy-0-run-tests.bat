@echo off

echo ### CLEANING SOLUTION ###
dotnet clean ..\..\src\ScottPlotV4.sln --verbosity quiet --configuration Release


echo ### RUNNING TESTS ###
dotnet test ..\..\src\ScottPlotV4.sln --configuration Release

pause