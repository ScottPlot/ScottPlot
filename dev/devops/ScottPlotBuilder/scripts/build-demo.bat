@echo off

echo.
echo ########## Building WPF Demo ######################################################
echo.
dotnet clean ../../../../src/ScottPlot.Demo.WPF/ScottPlot.Demo.WPF.csproj --configuration Release || goto :error
dotnet build ../../../../src/ScottPlot.Demo.WPF/ScottPlot.Demo.WPF.csproj --configuration Release || goto :error

echo.
echo ########## Building WinForms Demo #################################################
echo.
dotnet clean ../../../../src/ScottPlot.Demo.WinForms/ScottPlot.Demo.WinForms.csproj --configuration Release || goto :error
dotnet build ../../../../src/ScottPlot.Demo.WinForms/ScottPlot.Demo.WinForms.csproj --configuration Release || goto :error


:SUCCESS
echo.
echo SUCCESS!
pause
exit /b %errorlevel%

:ERROR
echo !!!ERROR!!!
pause
exit /b %errorlevel%