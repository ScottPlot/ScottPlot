:: Build blazor app
SET BLAZOR_PROJECT=..\..\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.Blazor.WebAssembly
SET BLAZOR_WWWROOT=%BLAZOR_PROJECT%\bin\Release\net8.0\publish\wwwroot
del /f/q/s "%BLAZOR_WWWROOT%\bin" > nul
del /f/q/s "%BLAZOR_PROJECT%\bin" > nul
dotnet publish "%BLAZOR_PROJECT%"

:: Modify the html file
dotnet run --project ../BlazorTweaker/BlazorTweaker.csproj "%BLAZOR_WWWROOT%"

:: Execute the Cookbook test project to generate the JSON file
dotnet test "..\..\src\ScottPlot5\ScottPlot5 Cookbook"

:: Copy the cookbook recipes from the Cookbook output folder to the Blazor output folder
SET JSON_SOURCE=..\..\dev\www\cookbook\5.0\recipes.json
copy "%JSON_SOURCE%" "%BLAZOR_WWWROOT%\sample-data"

explorer "%BLAZOR_WWWROOT%"
pause