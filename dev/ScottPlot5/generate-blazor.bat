:: Build blazor app
SET BLAZOR_PROJECT=..\..\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.Blazor.WebAssembly
del /f/q/s "%BLAZOR_PROJECT%\bin" > nul
dotnet publish "%BLAZOR_PROJECT%"

:: Execute the Cookbook test project to generate the JSON file
dotnet test "..\..\src\ScottPlot5\ScottPlot5 Cookbook"

:: Copy the cookbook recipes from the Cookbook output folder to the Blazor output folder
SET JSON_SOURCE=..\..\dev\www\cookbook\5.0\recipes.json
SET JSON_TARGET=..\..\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.Blazor.WebAssembly\bin\Release\net8.0\publish\wwwroot\sample-data
copy "%JSON_SOURCE%" "%JSON_TARGET%"

explorer "%JSON_TARGET%\..\"

pause