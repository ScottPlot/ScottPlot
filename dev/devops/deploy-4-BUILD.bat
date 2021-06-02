@echo off

echo.
echo ### DELETING BIN FOLDERS ###
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S ..\..\bin') DO echo DELETING %%G
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S ..\..\bin') DO RMDIR /S /Q "%%G"

echo.
echo ### DELETING OBJ FOLDERS ###
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S ..\..\obj') DO echo DELETING %%G
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S ..\..\obj') DO RMDIR /S /Q "%%G"

echo.
echo ### BUILD RELEASE PACKAGES ###
dotnet build ..\..\src\ScottPlot\ScottPlot.csproj --verbosity quiet --configuration Release
dotnet build ..\..\src\controls\ScottPlot.WinForms\ScottPlot.WinForms.NUGET.csproj --verbosity quiet --configuration Release
dotnet build ..\..\src\controls\ScottPlot.WPF\ScottPlot.WPF.NUGET.csproj --verbosity quiet --configuration Release
dotnet build ..\..\src\controls\ScottPlot.Avalonia\ScottPlot.Avalonia.NUGET.csproj --verbosity quiet --configuration Release

pause