convert %~dp0\v5\scottplot-icon-rounded-128.png %~dp0\v5\scottplot-icon-rounded.ico
convert %~dp0\v5\scottplot-icon-rounded-border-128.png %~dp0\v5\scottplot-icon-rounded-border.ico
convert %~dp0\v5\scottplot-icon-square-128.png %~dp0\v5\scottplot-icon-square.ico

pause

copy /b/v/y %~dp0\v5\scottplot-icon-rounded.ico %~dp0\..\..\src\ScottPlot\icon.ico
copy /b/v/y %~dp0\v5\scottplot-icon-rounded-128.png %~dp0\..\..\src\ScottPlot\icon.png

copy /b/v/y %~dp0\v5\scottplot-icon-rounded.ico %~dp0\..\..\src\controls\ScottPlot.WinForms\icon.ico
copy /b/v/y %~dp0\v5\scottplot-icon-rounded-128.png %~dp0\..\..\src\controls\ScottPlot.WinForms\icon.png

copy /b/v/y %~dp0\v5\scottplot-icon-rounded.ico %~dp0\..\..\src\controls\ScottPlot.WPF\icon.ico
copy /b/v/y %~dp0\v5\scottplot-icon-rounded-128.png %~dp0\..\..\src\controls\ScottPlot.WPF\icon.png

copy /b/v/y %~dp0\v5\scottplot-icon-rounded.ico %~dp0\..\..\src\controls\ScottPlot.Avalonia\icon.ico
copy /b/v/y %~dp0\v5\scottplot-icon-rounded-128.png %~dp0\..\..\src\controls\ScottPlot.Avalonia\icon.png

copy /b/v/y %~dp0\v5\scottplot-icon-rounded.ico %~dp0\..\..\src\demo\ScottPlot.Demo.WPF\icon.ico
copy /b/v/y %~dp0\v5\scottplot-icon-rounded.ico %~dp0\..\..\src\demo\ScottPlot.Demo.WinForms\icon.ico
copy /b/v/y %~dp0\v5\scottplot-icon-rounded.ico %~dp0\..\..\src\demo\ScottPlot.Demo.Avalonia\icon.ico

pause