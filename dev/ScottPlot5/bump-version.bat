@echo off
cd ../BumpVersion
dotnet run "../../src/ScottPlot5/ScottPlot5"
dotnet run "../../src/ScottPlot5/ScottPlot5 Controls/ScottPlot.Avalonia"
dotnet run "../../src/ScottPlot5/ScottPlot5 Controls/ScottPlot.Blazor"
dotnet run "../../src/ScottPlot5/ScottPlot5 Controls/ScottPlot.Eto"
dotnet run "../../src/ScottPlot5/ScottPlot5 Controls/ScottPlot.OpenGL"
dotnet run "../../src/ScottPlot5/ScottPlot5 Controls/ScottPlot.WinForms"
dotnet run "../../src/ScottPlot5/ScottPlot5 Controls/ScottPlot.WinUI"
dotnet run "../../src/ScottPlot5/ScottPlot5 Controls/ScottPlot.WPF"
pause