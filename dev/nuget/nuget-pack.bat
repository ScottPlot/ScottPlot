cd ..\..\src\ScottPlot 
..\..\dev\nuget\nuget.exe pack scottplot.nuspec -OutputDirectory ..\..\dev\nuget\DotNet
..\..\dev\nuget\nuget.exe pack ScottPlot-dotNetCore.nuspec -OutputDirectory ..\..\dev\nuget\DotNetCore
pause