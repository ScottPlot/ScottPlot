@echo off

echo.
echo ### DELETING RELEASE FOLDERS ###
RMDIR ..\..\src\ScottPlot\bin\Release /S /Q
RMDIR ..\..\src\ScottPlot.WinForms\bin\Release /S /Q
RMDIR ..\..\src\ScottPlot.WPF\bin\Release /S /Q

echo.
echo ### CLEANING SOLUTION ###
dotnet clean ..\..\src\ScottPlotV4.sln --verbosity quiet --configuration Release

echo.
echo ### REBUILDING SOLUTION ###
dotnet build ..\..\src\ScottPlot\ScottPlot.csproj --verbosity quiet --configuration Release
dotnet build ..\..\src\ScottPlot.WinForms\ScottPlot.WinForms.NUGET.csproj --verbosity quiet --configuration Release
dotnet build ..\..\src\ScottPlot.WPF\ScottPlot.WPF.NUGET.csproj --verbosity quiet --configuration Release

pause