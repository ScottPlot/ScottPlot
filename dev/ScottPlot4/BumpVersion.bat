@echo off
cd ../BumpVersion
dotnet run "../../src/ScottPlot4/ScottPlot"
dotnet run "../../src/ScottPlot4/ScottPlot.Avalonia"
dotnet run "../../src/ScottPlot4/ScottPlot.Eto"
dotnet run "../../src/ScottPlot4/ScottPlot.WinForms"
dotnet run "../../src/ScottPlot4/ScottPlot.WPF"
pause