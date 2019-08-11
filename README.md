# ScottPlot

**ScottPlot is a free and open-source interactive graphing library for .NET written in C#.** 
In a GUI environment ScottPlot makes it easy to display data interactively (left-click-drag pan, right-click-drag zoom), and in non-GUI environments ScottPlot can be used to create graphs and save them as images. ScottPlot is easy to use because it is available on NuGet and it has no dependencies outside the .NET framework libraries.

![](/demos/src/plot-types/ScottPlot-screenshot.gif)

## Quickstart
1. Install ScottPlot using [NuGet](https://www.nuget.org/packages/ScottPlot/)
2. Drag/Drop the ScottPlotUC (from the toolbox) onto your Form
3. Add the following code to your startup sequence

```cs
double[] xs = new double[] {1, 2, 3, 4, 5};
double[] ys = new double[] {1, 4, 9, 16, 25};
scottPlotUC1.plt.PlotScatter(xs, ys);
scottPlotUC1.Render();
```

You now have a mouse-interactive graph! Left-click-drag to pan, right-click-drag to zoom, and double-click to toggle benchmark display. Check out the [ScottPlot Cookbook](/cookbook) to see some of the ways graphs can be customized.

![](/dev/nuget/quickstart.png)

## Documentation
* **[ScottPlot Cookbook](/cookbook)** _🡐 start here_
* Project goals and API details are in **[/doc](/doc/)**
* The **[/demos](/demos)** folder contains:
  * Quickstart guides for WinForms, WPF, and Console Applications
  * Additional example programs
  * Click-to-run compiled demos

## Features

### Multiple Plot Types
ScottPlot can make scatter plots, box plots, step plots, and even financial plots (candlestick and OHLC plots). The signal plot is a type of scatter plot optimized for speed when plotting evenly-spaced data. Signal plots are capable of displaying _millions_ of data points at >100 Hz framerates allowing comfortable interaction using the mouse. The _plot types.exe_ application (in [/demos](/demos)) demonstrates this.

![](/demos/src/plot-types/ScottPlot-screenshot.png)

### Animated Plots
If you plot a double array with ScottPlot, later updating the original array automatically updates the data in ScottPlot. The _animated sine wave.exe_ application (in [/demos](/demos)) demonstrates this by plotting an array then uses a timer to update it continuously. Note that the graph is still mouse-interactive (panning and zooming) while continuously updating. 

![](/demos/src/animated-sin/screenshot.gif)

### Highspeed Data Visualization
ScottPlot can plot data quickly allowing for high framerates. The _audio monitor.exe_ demo (in [/demos](/demos)) uses two ScottPlot plots to display audio data in real time. The signal (PCM, top) and frequency (FFT, bottom) components are continuously updated at a high framerate and are both mouse-interactive.

![](/demos/src/audio-monitor/screenshot.gif)

## About ScottPlot

ScottPlot was created by [Scott Harden](http://www.SWHarden.com/) ([Harden Technologies, LLC](http://tech.swharden.com)) with many contributions from the user community. To inquire about the development special features or customized versions of this software for consumer applications, contact the author at [SWHarden@gmail.com](mailto:swharden@gmail.com).
