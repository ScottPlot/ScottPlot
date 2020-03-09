# ScottPlot

[![](https://img.shields.io/azure-devops/build/swharden/swharden/2?label=Build&logo=azure%20pipelines)](https://dev.azure.com/swharden/swharden/_build/latest?definitionId=2&branchName=master)
[![](https://img.shields.io/nuget/dt/ScottPlot?color=004880&label=NuGet%20Installs&logo=nuget)](https://www.nuget.org/packages/ScottPlot/)

**ScottPlot is a free and open-source graphing library for .NET** which makes it easy to display data in a variety of formats (line plots, bar charts, scatter plots, etc.) with just a few lines of code (see the **[ScottPlot Cookbook](http://swharden.com/scottplot/#cookbook)** for examples). User controls are available for WinForms and WPF to allow interactive display of data. 

![](dev/nuget/ScottPlot.gif)

Jump to the [demos](#Demos) section to see what ScottPlot can do on your system!

# Supported Platforms
ScottPlot is written in .NET Standard 2.0 so it [supports all modern .NET platforms](https://docs.microsoft.com/en-us/dotnet/standard/net-standard#net-implementation-support). The WPF control (WpfPlot) supports .NET Core and the Windows Forms control (FormsPlot) supports both .NET Framework and .NET Core.

Package | Supported Platforms | Purpose
---|---|---
[ScottPlot](https://www.nuget.org/packages/ScottPlot/) | .NET Standard 2.0 | Plot data and save or return a bitmap <br> _Supports Windows, Linux, and MacOS_
[ScottPlot.WinForms](https://www.nuget.org/packages/ScottPlot.WinForms/) | .NET Framework 4.6.1 <br> .NET Framework 4.7.2 <br> .NET Framework 4.8.0 <br>  .NET Core 3.0 | User control for mouse-interactive plots
[ScottPlot.WPF](https://www.nuget.org/packages/ScottPlot.WPF/) | .NET Core 3.0 | User control for mouse-interactive plots

_ScottPlot 3.1.6 is the last version to support .NET Framework 4.5_

## Quickstart

Solution files for quickstart examples are in [/dev/quickstart](/doc/quickstart)

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
formsPlot1.plt.PlotScatter(dataX, dataY);
formsPlot1.Render();
```

### Interactive Plot (WPF)
* Install the `ScottPlot.WPF` NuGet package
* Add the WpfPlot control by modifying your XAML file:
  * Add `xmlns:ScottPlot="clr-namespace:ScottPlot;assembly=ScottPlot.WPF"` to the top section
  * Add `<ScottPlot:WpfPlot Name="wpfPlot1" />` to the layout area
* Add code to the startup sequence of your CS file:
```cs
double[] dataX = new double[] {1, 2, 3, 4, 5};
double[] dataY = new double[] {1, 4, 9, 16, 25};
wpfPlot1.plt.PlotScatter(dataX, dataY);
wpfPlot1.Render();
```

## Cookbook
Review the **[ScottPlot Cookbook](http://swharden.com/scottplot/#cookbook)** to see what ScottPlot can do and learn how to use most of the ScottPlot features. Every in figure in the cookbook is displayed next to the code that was used to create it. 

<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/Quickstart_Quickstart_Scatter.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/PlotTypes_Bar_Quickstart.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/PlotTypes_Bar_MultipleBars.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/PlotTypes_Finance_CandleNoSkippedDays.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/PlotTypes_Scatter_CustomizeLines.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/PlotTypes_Scatter_RandomXY.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/PlotTypes_Signal_Density.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/Customize_Axis_LogAxis.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/Customize_PlotStyle_StyledLabels.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/Customize_Ticks_LocalizedGerman.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/Advanced_Multiplot_Quickstart.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/Examples_Stats_Histogram.png' width='200'></a>
<a href='http://swharden.com/scottplot/#cookbook'><img src='http://swharden.com/scottplot/cookbook/4.0.19/images/Examples_Stats_LinReg.png' width='200'></a>

## Demos

**Download for Windows (x64): [ScottPlotDemos.zip](/dev/demos/)**\
This click-to-run demo application was designed to make it easy to assess the capabilities of ScottPlot. Source code for the demo is in [/src](/src)

![](src/ScottPlot.Demo.WPF/screenshot.png)

In addition to interactively displaying all cookbook examples (which demonstrate how to use every plot type) the demo application also provides example code for advanced topics such as:
  * Mouse tracking
  * Plotting changing (or growing) data
  * Draggable axis lines
  * Display value under cursor


## About ScottPlot

**Author:** ScottPlot was created by [Scott Harden](http://www.SWHarden.com/) ([Harden Technologies, LLC](http://tech.swharden.com)) with many contributions from the user community. 

**Commission:** To inquire about the development special features or customized versions of this software for consumer applications, contact the author at [SWHarden@gmail.com](mailto:swharden@gmail.com).

**Version history:** the [ScottPlot changelog](/dev/changelog.md) has an excellent summary of the differences between major versions of ScottPlot.
