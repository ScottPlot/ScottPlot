dotnet restore "src\ScottPlot4\ScottPlot5.sln"
dotnet build "src\ScottPlot4\ScottPlot5.sln" --configuration Release
dotnet restore "src\ScottPlot4\ScottPlot4.sln"
dotnet build "src\ScottPlot4\ScottPlot4.sln" --configuration Release