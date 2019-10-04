# ScottPlot

[![](https://img.shields.io/azure-devops/build/swharden/swharden/2?label=Build&logo=azure%20pipelines)](https://dev.azure.com/swharden/swharden/_build/latest?definitionId=2&branchName=master)
[![](https://img.shields.io/azure-devops/tests/swharden/swharden/2?label=Tests&logo=azure%20pipelines)](https://dev.azure.com/swharden/swharden/_build/latest?definitionId=2&branchName=master)
[![](https://img.shields.io/nuget/dt/ScottPlot?color=004880&label=NuGet%20Installs&logo=nuget)](https://www.nuget.org/packages/ScottPlot/)

**ScottPlot is a free and open-source interactive plotting library for .NET** which makes it easy to interactively display data in a variety of formats. You can create interactive line plots, bar charts, scatter plots, etc., with just a few lines of code (see the [ScottPlot Cookbook](/cookbook) for examples). 

![](/demos/src/plot-types/ScottPlot-screenshot.gif)

In graphical environments plots can be displayed interactively (left-click-drag to pan and right-click-drag to zoom) and in console applications plots can be created and saved as images. 

ScottPlot targets multiple frameworks (.NET Framework 4.5 and .NET Core 3.0), has user controls for WinForms and WPF, and is [available on NuGet](https://www.nuget.org/packages/ScottPlot/) with no dependencies.

## Quickstart
This quickstart is for Windows Forms Applications. Similar quickstarts for Console Applications and WPF Applications are in [/demos](/demos)

1. Install ScottPlot using [NuGet](https://www.nuget.org/packages/ScottPlot/)
2. Drag/Drop the FormsPlot (from the toolbox) onto your Form
3. Add the following code to your startup sequence

```cs
double[] xs = new double[] {1, 2, 3, 4, 5};
double[] ys = new double[] {1, 4, 9, 16, 25};
formsPlot1.plt.PlotScatter(xs, ys);
formsPlot1.Render();
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
ScottPlot can make scatter plots, box plots, step plots, and even financial plots (candlestick and OHLC plots). The signal plot is a type of scatter plot optimized for speed when plotting evenly-spaced data. Signal plots are capable of displaying _tens of millions_ of data points at >100 Hz framerates allowing comfortable interaction using the mouse. The _plot types.exe_ application (in [/demos](/demos)) demonstrates this.

![](/demos/src/plot-types/ScottPlot-screenshot.png)

### Animated Plots
If you plot a double array with ScottPlot, later updating the original array automatically updates the data in ScottPlot. The _animated sine wave.exe_ application (in [/demos](/demos)) demonstrates this by plotting an array then uses a timer to update it continuously. Note that the graph is still mouse-interactive (panning and zooming) while continuously updating. 

![](/demos/src/animated-sin/screenshot.gif)

### Highspeed Data Visualization
ScottPlot can plot data quickly allowing for high framerates. The _audio monitor.exe_ demo (in [/demos](/demos)) uses two ScottPlot plots to display audio data in real time. The signal (PCM, top) and frequency (FFT, bottom) components are continuously updated at a high framerate and are both mouse-interactive.

![](/demos/src/audio-monitor/screenshot.gif)

## Cookbook
The [ScottPlot Cookbook](/cookbook) is the best way to both see what ScottPlot can do and learn how to use most of the ScottPlot features. Every in [/cookbook](/cookbook) is shown next to the code used to create it. Here are some samples:

<img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/02_Styling_Scatter_Plots.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/06b_Custom_LineStyles.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/22_Custom_Colors.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/25_Corner_Axis_Frame.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/30a_Signal.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/41_Axis_Spans.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/62_Plot_Bar_Data_Fancy.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/65_Histogram.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/66_CPH.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/67_Candlestick.png" width="200"> 

## About ScottPlot

ScottPlot was created by [Scott Harden](http://www.SWHarden.com/) ([Harden Technologies, LLC](http://tech.swharden.com)) with many contributions from the user community. To inquire about the development special features or customized versions of this software for consumer applications, contact the author at [SWHarden@gmail.com](mailto:swharden@gmail.com).
