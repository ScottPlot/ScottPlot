:: run tests to generate cookbook
dotnet test ..\..\src\ScottPlot4

:: copy to ScottPlot.NET repo
robocopy ..\..\src\ScottPlot4\ScottPlot.Cookbook\CookbookOutput ..\..\..\ScottPlot.NET\content\cookbook\4.1 /E /NJH /NFL /NDL

pause