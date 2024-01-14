SET DemoFolder=..\..\src\ScottPlot5\ScottPlot5 Demos\ScottPlot5 WinForms Demo

rmdir /q/s "%DemoFolder%\bin"

dotnet build "%DemoFolder%" --configuration Release

explorer "%DemoFolder%\bin"

pause