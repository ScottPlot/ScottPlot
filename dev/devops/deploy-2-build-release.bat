@echo off

echo.
echo ### DELETING RELEASE FOLDERS ###
RMDIR ..\..\src\ScottPlot\bin\Release /S /Q
RMDIR ..\..\src\ScottPlot.WinForms\bin\Release /S /Q
RMDIR ..\..\src\ScottPlot.WPF\bin\Release /S /Q
RMDIR ..\..\src\ScottPlot.Avalonia\bin\Release /S /Q

echo.
echo ### BUILDING NUGET PACKAGES ###
dotnet build ..\..\src\ScottPlot\ScottPlot.csproj --verbosity quiet --configuration Release
dotnet build ..\..\src\ScottPlot.WinForms\ScottPlot.WinForms.NUGET.csproj --verbosity quiet --configuration Release
dotnet build ..\..\src\ScottPlot.WPF\ScottPlot.WPF.NUGET.csproj --verbosity quiet --configuration Release
dotnet build ..\..\src\ScottPlot.Avalonia\ScottPlot.Avalonia.NUGET.csproj --verbosity quiet --configuration Release
pause