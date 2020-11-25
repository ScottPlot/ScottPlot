# ScottPlot Overview

This page summarizes the primary workflow of ScottPlot.

## Plottable Objects

A ***Plottable*** is anything you can display on a plot. The scatter plot is the most commonly used plottable, and many plottables can be added to a plot.

In GUI environments you must call the control's `Render()` method to request a redraw on the screen any time a plot or its data is modified. In console applications (and in these examples), `SaveFig()` incites a render automatically.

### Create Plottables with Helper Methods
The simplest way to plot data is to use helper methods. Helper methods create a plottable, customize it with optional methods, and add it to the plot.

```cs
double[] xs = { 1, 2, 3, 4, 5 };
double[] ys = { 1, 4, 9, 16, 25 };

var plt = new ScottPlot.Plot(400, 300);
plt.PlotScatter(xs, ys, color: Color.Red, lineWidth: 2);
plt.SaveFig("example.png");
```

![](images/create-plottable-red.png)

### Plottables can be Modified After Creation

Helper methods return the plottable they create, allowing further customization using public fields and methods. 

In GUI environments, this is how plottable style and data can be modified after it has already been added to the plot.

```cs
var plt = new ScottPlot.Plot(400, 300);
var scatter = plt.PlotScatter(xs, ys);
scatter.color = Color.Blue;
scatter.lineWidth = 2;
plt.SaveFig("example.png");
```

![](images/create-plottable-blue.png)

### Create Plottables Manually

Creating a plottable manually gives the user maximum ability to customize it. It also allows users to create their own plot types and add them to the plot.

```cs
var plt = new ScottPlot.Plot(400, 300);
var scatter = new ScottPlot.Plottable.ScatterPlot(xs, ys);
scatter.color = Color.Green;
scatter.lineWidth = 2;
plt.SaveFig("example.png");
```

![](images/create-plottable-green.png)

## Update Data Values

If you create a plottable by passing-in a data array, you can modify values in that data array later and those changes will automatically appear on the plot when it is rendered.

```cs
double[] xs = { 1, 2, 3, 4, 5 };
double[] ys = { 1, 4, 9, 16, 25 };

var plt = new ScottPlot.Plot(400, 300);
var scatter = plt.PlotScatter(xs, ys);
plt.SaveFig("example.png");
```

![](images/modify-plottable-before.png)

```cs
ys[2] = 23; // modify values inside an array
plt.SaveFig("example2.png");
```

![](images/modify-plottable-after.png)

* In this example the array is not changed (just the values inside the array). Different plot types have different fields and methods to facilitate updating data.

* To adjust axis limits to fit the new data call `plt.AxisAuto()`

* In GUI environments, call the control's `Render()` method to request a redraw on the screen after updating data

* In multi-threaded environments, use the `RenderLock()` to ensure plottables are not modified simultaneously while they are being rendered.

## Favor Signal Plot over Scatter Plot

* **Scatter plots have paired X/Y data points.** Scatter plots are designed to display ***hundreds*** of points, but performance rapidly drops as the number of points increases, so scatter plots are not appropriate for large datasets.

* **Signal plots have Y data and a sample rate.** Signal plots are optimized for performance and can render datasets with ***millions*** of points at a high framerate.

* **SignalConst** plots uses an algorithm optimized for constant data values, allowing interactive rendering of ***hundreds of millions*** of data points at a high framerate.

## Working with Axes

* Customize labels, ticks, and grid of primary axes: `XAxis` and `YAxis`
* Fit axis limits to the plotted data with `Plot.AxisAuto()`
* Get axis limits manually with `Plot.AxisLimits()` ⚠️ _Only applies to ScottPlot 4.1_
* Set axis limits manually with `Plot.AxisSet()` ⚠️ _Only applies to ScottPlot 4.1_
* Customize secondary axes: `XAxis2`, `YAxis2`
* Add additional custom axes

## Live Display of Changing Data

There are 3 main ways to displaying changing data in a GUI

* **Change values in arrays:** Plot fixed-length arrays once, then modify the values in those arrays at any time. Call the control's `Render()` to request a redraw of the plot containing the latest data.

* **Clear and re-plot:** Every time the data changes `plt.Clear()` all the old plottables and add new ones to the plot. Use the `RenderLock()` if calling this in a thread outside the GUI thread.

* **Use a ScatterList plot type:** Unlike regular scatter plots which use fixed-length arrays, the ScatterList plot type is designed to have points added and removed. ⚠️ _Only applies to ScottPlot 4.1_

* **Replace data arrays:** Replacing data arrays in plottables is possible, but the exact method varies by type. Array replacement is required if the plottable displays fixed-length arrays but the number of data points must change (requiring a new array to be loaded into the plottable). Scatter plots have a `ReplaceData()` method to facilitate this. Use the `RenderLock()` if calling this in a thread outside the GUI thread. ⚠️ _Only applies to ScottPlot 4.1_