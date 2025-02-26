:: Folder the cookbook will be generated in when the test project runs
SET CookbookSource=..\www\cookbook\5.0

:: Folder where the website repository is located
SET ShortcodeFolder=..\..\..\ScottPlot.NET\layouts\shortcodes
SET CookbookDest=%ShortcodeFolder%\content\cookbook\5.0

:: generate the demo app shortcode
dotnet run --project "..\..\src\ScottPlot5\ScottPlot5 Demos\ScottPlot5 WinForms Demo" "%ShortcodeFolder%\demos-winforms.html"
dotnet run --project "..\..\src\ScottPlot5\ScottPlot5 Demos\ScottPlot5 WPF Demo" "%ShortcodeFolder%\demos-wpf.html"

:: delete the old cookbook if it exists
rmdir /q/s "%CookbookSource%"

:: run the ScottPlot test project to generate cookbook images
dotnet test "..\..\src\ScottPlot5\ScottPlot5 Tests\Unit Tests"

:: run the Cookbook test project to generate HTML, Markdown, and JSON files
dotnet test "..\..\src\ScottPlot5\ScottPlot5 Cookbook"

:: copy all files (except HTML files) from the ScottPlot repo's cookbook output folder to the ScottPlot.NET repo's cookbook folder
del /f/q/s "%CookbookDest%" > nul
rmdir /q/s "%CookbookDest%"
robocopy "%CookbookSource%" "%CookbookDest%" /E /Z /R:5 /W:5 /TBD /NP /V
pause