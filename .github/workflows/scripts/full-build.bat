:: ScottPlot5
dotnet restore "src\ScottPlot4\ScottPlot5.sln"
dotnet build "src\ScottPlot4\ScottPlot5.sln" --configuration Release

:: ScottPlot4
dotnet restore "src\ScottPlot4\ScottPlot4.sln"
dotnet build "src\ScottPlot4\ScottPlot4.sln" --configuration Release