SET RecipesJsonFile=..\..\dev\www\cookbook\5.0\recipes.json
del "%RecipesJsonFile%"

SET CookbookProjectFolder=..\..\src\ScottPlot5\ScottPlot5 Cookbook\ScottPlot Cookbook.csproj
dotnet test "%CookbookProjectFolder%"

SET DemoProjectFolder=..\..\src\ScottPlot5\ScottPlot5 Demos\ScottPlot5 WinForms Demo
rmdir /q/s "%DemoProjectFolder%\bin"
dotnet build "%DemoProjectFolder%" --configuration Release

copy "%RecipesJsonFile%" "%DemoProjectFolder%\bin\Release\recipes.json"

explorer "%DemoProjectFolder%\bin\Release"
pause