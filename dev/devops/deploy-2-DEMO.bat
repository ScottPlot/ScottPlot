:: A .NET Framework 4.8 binary was chosen because at the time of writing
:: most computers don't have .NET 5 yet. Also, .NET 4.8 apps will launch a
:: dialog indicating how to download it if the system does not yet have it.

echo DONT USE THIS NOW - A CUSTOM APP IS REQUIRED TO MOVE SOURCE CODE FILES
pause
pause
pause

@echo off

echo.
echo ### DELETING OLD BINARY FOLDER ###
RMDIR /S /Q ..\..\src\demo\ScottPlot.Demo.WPF\bin

echo.
echo ### BUILDING DEMO RELEASE ###
dotnet build "..\..\src\demo\ScottPlot.Demo.WPF\WPF Demo.csproj" --configuration Release

echo.
echo ### CLEANING DEMO FOLDER ###
RMDIR /S /Q ..\..\src\demo\ScottPlot.Demo.WPF\bin\Release\net48\ref
del ..\..\src\demo\ScottPlot.Demo.WPF\bin\Release\net48\*.pdb
del ..\..\src\demo\ScottPlot.Demo.WPF\bin\Release\net48\*.json
del ..\..\src\demo\ScottPlot.Demo.WPF\bin\Release\net48\*.config
move ..\..\src\demo\ScottPlot.Demo.WPF\bin\Release\net48 "..\..\src\demo\ScottPlot.Demo.WPF\bin\Release\WPF Demo"
explorer ..\..\src\demo\ScottPlot.Demo.WPF\bin\Release

pause