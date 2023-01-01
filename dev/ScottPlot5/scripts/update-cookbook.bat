:: copy all files (except HTML files) from the ScottPlot repo's cookbook output folder to the ScottPlot.NET repo's cookbook folder
SET CookbookSource=C:\Users\scott\Documents\GitHub\ScottPlot\dev\www\cookbook\5.0
SET CookbookDest=C:\Users\scott\Documents\GitHub\ScottPlot.NET\content\cookbook\5.0
del /f/q/s "%CookbookDest%" > nul
rmdir /q/s "%CookbookDest%"
robocopy %CookbookSource% %CookbookDest% /E /Z /R:5 /W:5 /TBD /NP /V /XF *.html
pause