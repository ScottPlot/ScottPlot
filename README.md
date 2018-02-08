# ScottPlot

**ScottPlot is an open-source interactive graphing library for .NET written in C#.** It was written to simplify the task of interactively displaying data on a graph that you can left-click-drag to pan and right-click drag to zoom. Although ScottPlot was designed for interactive graphing of large datasets in a GUI environment, its core can generate graphs from within console applications. ScottPlot was loosely inspired by matplotlib for Python.

![](/doc/screenshots/resize-pan-zoom.gif)

# Example Use
These minimal-case examples demonstrate how to get started graphing data with ScottPlot. Advanced features (transparency, colors, markers, etc) are described in the [documentation folder](/doc/).

## User Control for Interactive Graphing
The simplest way to use ScottPlot is to drag/drop the ScottPlotUC (user control) onto a Windows Form, then fill it with some data. It will automatically draw the graph and immedaitely be interactive to the mouse (left-click-drag to pan, right-click-drag to zoom).

```cs
scottPlotUC1.PlotXY(Xs, Ys); // Make a line graph from double arrays
scottPlotUC1.AxisAuto(); // Auto-scale the axis to fit the data
```

_The animated example above demonstrates this functionality_

## Render onto a Picturebox for Static Graphing
If the goal is just to add a graph to a Windows Form, it can simply be rendered onto a picturebox eliminating the need for a special user control. This can be accomplished by interacting with the ScottPlot.Figure class directly which yields customization options and speed performance beyond that of the user control. Extensive examples are in the [ScottPlot Cookbook](/doc/cookbook/).

```cs
var fig = new ScottPlot.Figure(pictureBox1.Width, pictureBox1.Height);
fig.AxisAuto(Xs, Ys);
fig.PlotLines(Xs, Ys);
pictureBox1.Image = fig.Render();
```

<img src="/doc/screenshots/picturebox.png" width="640">

## Save to File from a Console Application
The ScottPlot.Figure class is fully functional without a GUI! Anything you can do in a Windows Form you can do from a Console Application (saving to a file rather than rending on a picturebox). All examples in the [ScottPlot Cookbook](/doc/cookbook/) can be used in console applications.

```cs
var fig = new ScottPlot.Figure(pictureBox1.Width, pictureBox1.Height);
fig.AxisAuto(Xs, Ys);
fig.PlotLines(Xs, Ys);
fig.Save("demo.png");
```

<img src="/doc/screenshots/console.png" width="640">

## Large Dataset Optimization
ScottPlot was written to interactively display data with _tens of millions_ of data points. A good example of this is a one hour WAV file (3600 seconds at 48 kHz = 172.8 million points). Numerous optimizations provide this functionality for signals (a series of evenly-spaced data points). Rather than graph two double arrays (Xs and Ys), use ScottPlot's signal graphing methods. Additional options (like like X and Y offset) are demonstrated in the [ScottPlot cookbook](/doc/cookbook/readme.md)

```cs
ScottPlotUC1.PlotSignal(Ys); // optimized to plot millions of points in real time
```

<img src="/demos/2018-02-07.png" width="808">

## Compiled Demos
Compiled (EXE) demos are available for download in the [/demos/](/demos) folder. They are a good way to test-out what ScottPlot can do without having to download the source code or learn about the API. For non-interactive demos suitable for console applications, check out the [ScottPlot cookbook](/doc/cookbook/readme.md)

# Installing ScottPlot

* **Download:** Get the [latest ScottPlot (ZIP)](https://github.com/swharden/ScottPlot/archive/master.zip) from this page

* **Add Project:** Right-click your solution, _add_, _Existing Project_, and select [/src/ScottPlot/ScottPlot.csproj](/src/ScottPlot/ScottPlot.csproj)

* **Add Reference:** Right-click your project, _Add_, _Reference_, then under _Projects_ select _ScottPlot_

_Once installed, drag/drop the ScottPlotUC onto your Windows Form and give it data! It will respond to the mouse immediately._

![](/doc/screenshots/ScottPlotUC.png)

## Object Diagram
This is the latest automatically-generated class diagram. ScottPlot is still early in development and the API tends to restructure frequently, so I find this view helpful.
![](/src/ScottPlot.png)

## License
ScottPlot uses the [MIT License](LICENSE), so use it in whatever you want!
