:: must be run in cookbook folder so images can be loaded

cd %~dp0\..\..\src\cookbook\ScottPlot.Cookbook

dotnet run ^
  --project ..\CookbookGenerator ^
  --cookbookFolder .\ ^
  --saveImages C:\Users\scott\Documents\temp\cookbook\images ^
  --saveSource C:\Users\scott\Documents\temp\cookbook\source
  
pause