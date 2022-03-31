:: delete old cookbooks
RMDIR /S /Q "C:\Users\scott\Documents\GitHub\ScottPlot.NET\content\cookbook\4.1\images"
RMDIR /S /Q "C:\Users\scott\Documents\GitHub\ScottPlot.NET\content\cookbook\4.1\source"

:: cookbook must be run in the cookbook folder so images can be located by recipes that use them
cd %~dp0\..\..\src\cookbook\ScottPlot.Cookbook
dotnet run ^
  --project CookbookGenerator ^
  --cookbookFolder C:\Users\scott\Documents\GitHub\ScottPlot\src\ScottPlot4\ScottPlot.Cookbook ^
  --saveImages C:\Users\scott\Documents\GitHub\ScottPlot.NET\content\cookbook\4.1\images ^
  --saveSource C:\Users\scott\Documents\GitHub\ScottPlot.NET\content\cookbook\4.1\source
  
pause