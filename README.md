# ScottPlot

**ScottPlot is an open-source interactive graphing library for .NET written in C#.** It was written to simplify the task of interactively displaying data on a graph that you can left-click-drag to pan and right-click drag to zoom. The core of this project is a portable class library which allows a user to supply figure dimensions and scale information and plot data directly on a bitmap buffer relying on ScottPlot to handle unit-to-pixel conversions, drawing of axis labels, tick marks, grid lines, etc. Although ScottPlot was designed for interactive graphing of large datasets in a GUI environment, its core can generate graphs from within console applications. ScottPlot was loosely inspired by matplotlib for Python.

![](/doc/screenshots/resize-pan-zoom.gif)

## Demos
**Compiled (EXE) demos are available for download in the [/demos/](/demos) folder.** They are a good way to test-out what ScottPlot can do without having to download the source code or learn about the API. For non-interactive demos suitable for console applications, check out the [ScottPlot cookbook](/doc/cookbook/readme.md)

## Use ScottPlot in Console Applications
ScottPlot does not require a GUI to create graphs, as they can be easily saved as BMP, JPG, or PNG files.


![](/doc/screenshots/console.png)

## Use ScottPlot in Windows Forms
In this example, clicking button1 draws a graph and applies it to a picturebox. 

![](/doc/screenshots/picturebox.png)

Note that this method looks excellent, but graphs are not interactive. Creating interactive graphs requires making handlers to resize and redraw the graph for resize events, click-and-drag, etc. However, all this functionality is pre-packaged in ScottPlot user controls which are designed to respond to resize events, left-click-drag panning, and right-click-drag zooming.

## ScottPlot User Controls for Interactive Graphs
ScottPlot user controls simplify the task of creating interactive graphs. Different user controls are optimized for specific tasks. For example, the ucSignal user control is designed to take very large arrays of data (tens of millions of data points) and produce an interactive plot (which pans and zooms with the mouse) updating at extremely high speed. Adding a reference to ScottPlot in a Windows Forms project reveals these user controls which can then be added into your Form.

![](/doc/screenshots/ScottPlotUC.png)

## Additional Examples
* Extensive examples are provided in the **[ScottPlot cookbook](/doc/cookbook/readme.md)**

## Installing ScottPlot

* **Download:** Get the [latest ScottPlot (ZIP)](https://github.com/swharden/ScottPlot/archive/master.zip) from this page

* **Add Project:** Right-click your solution, _add_, _Existing Project_, and select [/src/ScottPlot/ScottPlot.csproj](/src/ScottPlot/ScottPlot.csproj)

* **Add Reference:** Right-click your project, _Add_, _Reference_, then under _Projects_ select _ScottPlot_

## License
ScottPlot uses the [MIT License](LICENSE), so use it in whatever you want!
