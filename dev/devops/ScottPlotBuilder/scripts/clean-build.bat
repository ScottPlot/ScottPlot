@echo off

echo.
echo ########## Building ScottPlot #################################################
echo.
del /F /Q ..\..\..\..\src\ScottPlot\bin\Release\*.*
dotnet clean ../../../../src/ScottPlot/ScottPlot.csproj --configuration Release || goto :error
dotnet build ../../../../src/ScottPlot/ScottPlot.csproj --configuration Release || goto :error

echo.
echo ########## Building ScottPlot.WinForms ########################################
echo.
del /F /Q ..\..\..\..\src\ScottPlot.WinForms\bin\Release\*.*
dotnet clean ../../../../src/ScottPlot.WinForms/ScottPlot.WinForms.csproj --configuration Release || goto :error
dotnet build ../../../../src/ScottPlot.WinForms/ScottPlot.WinForms.csproj --configuration Release || goto :error

echo.
echo ########## Building ScottPlot.WPF #############################################
echo.
del /F /Q ..\..\..\..\src\ScottPlot.WPF\bin\Release\*.*
dotnet clean ../../../../src/ScottPlot.WPF/ScottPlot.WPF.csproj --configuration Release || goto :error
dotnet build ../../../../src/ScottPlot.WPF/ScottPlot.WPF.csproj --configuration Release || goto :error

:SUCCESS
echo.
echo SUCCESS!
pause
exit /b %errorlevel%

:ERROR
echo !!!ERROR!!!
pause
exit /b %errorlevel%