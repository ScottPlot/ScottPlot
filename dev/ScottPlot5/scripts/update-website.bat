:: copy all files (except HTML files) from the ScottPlot repo's cookbook output folder to the ScottPlot.NET repo's cookbook folder
SET CookbookSource=..\..\www\cookbook\5.0
SET CookbookDest=..\..\..\..\ScottPlot.NET\content\cookbook\5.0
del /f/q/s "%CookbookDest%" > nul
rmdir /q/s "%CookbookDest%"
robocopy %CookbookSource% %CookbookDest% /E /Z /R:5 /W:5 /TBD /NP /V /XF *.html
pause