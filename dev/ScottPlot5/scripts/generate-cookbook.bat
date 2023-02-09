:: this is where the cookbook will be created
SET CookbookSource=..\..\www\cookbook\5.0

:: delete the old cookbook if it exists
rmdir /q/s "%CookbookSource%"

:: run the tests to regenerate the cookbook
dotnet test ..\..\..\src\ScottPlot5\

:: copy all files (except HTML files) from the ScottPlot repo's cookbook output folder to the ScottPlot.NET repo's cookbook folder
SET CookbookDest=..\..\..\..\ScottPlot.NET\content\cookbook\5.0
del /f/q/s "%CookbookDest%" > nul
rmdir /q/s "%CookbookDest%"
robocopy %CookbookSource% %CookbookDest% /E /Z /R:5 /W:5 /TBD /NP /V /XF *.html
pause