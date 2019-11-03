# ScottPlot

[![](https://img.shields.io/azure-devops/build/swharden/swharden/2?label=Build&logo=azure%20pipelines)](https://dev.azure.com/swharden/swharden/_build/latest?definitionId=2&branchName=master)
[![](https://img.shields.io/azure-devops/tests/swharden/swharden/2?label=Tests&logo=azure%20pipelines)](https://dev.azure.com/swharden/swharden/_build/latest?definitionId=2&branchName=master)
[![](https://img.shields.io/nuget/dt/ScottPlot?color=004880&label=NuGet%20Installs&logo=nuget)](https://www.nuget.org/packages/ScottPlot/)

**ScottPlot is a free and open-source graphing library for .NET** which makes it easy to display data in a variety of formats (line plots, bar charts, scatter plots, etc.) with just a few lines of code (see the **[ScottPlot Cookbook](/cookbook)** for examples). User controls are available for WinForms and WPF to allow interactive display of data.

![](dev/nuget/ScottPlot.gif)

### NuGet Packages

NuGet Package | Target Framework(s) | Purpose
---|---|---
[ScottPlot](https://www.nuget.org/packages/ScottPlot/) | .NET Standard 2.0 | Plot data and save or return a bitmap
[ScottPlot.WinForms](https://www.nuget.org/packages/ScottPlot.WinForms/) | .NET Framework 4.6.1 <br> .NET Core 3.0 | User control for mouse-interactive plots
[ScottPlot.WPF](https://www.nuget.org/packages/ScottPlot.WPF/) | .NET Core 3.0 | User control for mouse-interactive plots

## Quickstart

Solution files for quickstart examples are in [/doc/quickstart](/doc/quickstart)

### Static Plot

* Install the `ScottPlot` NuGet package
* Add the following to your startup sequence:

```cs
double[] dataX = new double[] {1, 2, 3, 4, 5};
double[] dataY = new double[] {1, 4, 9, 16, 25};
var plt = new ScottPlot.Plot(600, 400);
plt.PlotScatter(dataX, dataY);
plt.SaveFig("quickstart.png");
```

_Static plots can be useful in GUI applications, such as plotting on a Picturebox:_

```cs
pictureBox1.Image = plt.GetBitmap();
```

### Interactive Plot (Windows Forms)
* Install the `ScottPlot.WinForms` NuGet package
* Drag/drop a FormsPlot user control onto the Form
* Add the following to the start-up sequence:
```cs
double[] dataX = new double[] {1, 2, 3, 4, 5};
double[] dataY = new double[] {1, 4, 9, 16, 25};
wpfPlot1.plt.PlotScatter(dataX, dataY);
formsPlot1.Render();
```

### Interactive Plot (WPF)
* Install the `ScottPlot.WPF` NuGet package
* Add `<ScottPlot:WpfPlot Name="wpfPlot1"/>` to your XAML file
* Add the following to the start-up sequence:
```cs
double[] dataX = new double[] {1, 2, 3, 4, 5};
double[] dataY = new double[] {1, 4, 9, 16, 25};
wpfPlot1.plt.PlotScatter(dataX, dataY);
wpfPlot1.Render();
```

## Cookbook
The **[ScottPlot Cookbook](/cookbook)** is the best way to both see what ScottPlot can do and learn how to use most of the ScottPlot features. Every in figure in the cookbook is displayed next to the code that was used to create it. Here are some samples:

<img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/02_Styling_Scatter_Plots.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/06b_Custom_LineStyles.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/22_Custom_Colors.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/25_Corner_Axis_Frame.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/30a_Signal.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/41_Axis_Spans.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/62_Plot_Bar_Data_Fancy.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/65_Histogram.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/66_CPH.png" width="200"><img src="https://raw.githubusercontent.com/swharden/ScottPlot/master/cookbook/images/67_Candlestick.png" width="200"> 

## Demos
* Download the demos as a click-to-run EXE: **[ScottPlotDemos.zip](/demos/ScottPlotDemos.zip)**
* Source code demos is in: [/demos](/demos)
* The demo project demonstrates advanced topics such as:
  * Animated plots
  * Plotting changing (or growing) data
  * Draggable axis lines
  * Show value on hover
  * Realtime display of audio data (PCM and FFT)

## About ScottPlot

ScottPlot was created by [Scott Harden](http://www.SWHarden.com/) ([Harden Technologies, LLC](http://tech.swharden.com)) with many contributions from the user community. To inquire about the development special features or customized versions of this software for consumer applications, contact the author at [SWHarden@gmail.com](mailto:swharden@gmail.com).
