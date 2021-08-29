dotnet clean ..\..\src\ScottPlot.sln --configuration Release
dotnet test ..\..\src\ScottPlot.sln --configuration Release
explorer ..\..\src\tests\bin\Release\net5.0\website
pause