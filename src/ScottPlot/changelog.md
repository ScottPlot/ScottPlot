# ScottPlot Changelog

## ScottPlot 3.0.7
_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-07-27_

### Major Changes
* **New plot type `plt.PlotStep()`:** demonstrated in the [cookbook](https://github.com/swharden/ScottPlot/blob/master/doc/cookbook/README.md#step-plot).  An interactive example is in the demos folder.
* **New plot type `plt.PlotCandlestick()`:** demonstrated in the [cookbook](https://github.com/swharden/ScottPlot/blob/master/doc/cookbook/README.md#candlestick). An interactive example is in the demos folder.
* **New plot type `plt.PlotOHLC()`:** demonstrated in the [cookbook](https://github.com/swharden/ScottPlot/blob/master/doc/cookbook/README.md#ohlc).  An interactive example is in the demos folder.
* **`plt.MatchPadding()`:** copies the data frame layout from one ScottPlot onto another (useful for making plots of matching size). An interactive example is in the demos folder.
* **`plt.MatchAxis()`:** copies the axes from one ScottPlot onto another (useful for making plots match one or both axis). An interactive example is in the demos folder.
* **`plt.Legend()` improvements**
  * The `location` argument allows the user to place the legend at one of 9 different places on the plot. See the [cookbook example](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#legend).
  * The `shadowDirection` argument allows the user to control if a shadow is shown and at what angle.
* **Custom marker shapes** can be specified using the `markerShape` argument. See the [cookbook example](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#custom-marker-shapes).


## ScottPlot 3.0.6
_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-06-30_

### Major Changes
* **Bar plot:** The plot module now has a `Bar()` method that lets users create various types of bar plots, as seen in [cookbook#plot-bar-data](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#plot-bar-data)
* **Histogram:** The new `ScottPlot.Histogram` class has tools to create and analyze histogram data (including cumulative probability). Examples of this can be seen at [cookbook#histogram](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#histogram) and [cookbook#cph](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#cph)
* **Step plot:** Scatter plots can now render as step plots. Use this feature by setting the `stepDisplay` argument with `PlotScatter()` as seen in the [cookbook#step-plot](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#step-plot)
* **Manual grid spacing:** Users can now manually define the grid density by setting the `xSpacing` and `ySpacing` arguments in `Grid()` as seen in [cookbook#manual-grid-spacing](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#manual-grid-spacing)
* **Draggable axis lines:** Axis lines can be dragged with the mouse if the `draggable` argument is set to `true` in `PlotHLine()` and `PlotHLine()`. Draggable axis line limits can also be set by defining additional arguments. The [DraggableMarkers](https://github.com/swharden/ScottPlot/tree/master/demos/ScottPlotDraggableMarkers) program was created to demonstrate this feature.

### Minor Changes
* using the scrollwheel to zoom now zooms to the cursor position rather than the center of the plot area
* `ScottPlot.DataGen.RandomNormal()` was created to create arbitrary amounts of normally-distributed random data
* fixed bug causing axis line color to appear incorrectly in the legend
* `AxisAuto()` is now called automatically on the first render. This means users no longer have to call this function manually for most applications. This simplifies quickstart programs to just: instantiate plot, plot data, render (now 3 lines in total instead of 4).
* throw exceptions if scatter, bar, or signal data inputs are null (rather than failing later)

## ScottPlot 3.0.5
_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-06-23_
* fixes a bug (discussed in [issue 11](https://github.com/swharden/ScottPlot/issues/11)) to improve pan and zoom performance.

## ScottPlot 3.0.4
_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-06-23_

### New features
* **Bar graphs:** New `plotBar()` method allow creation of bar graphs. By customizing the `barWidth` and `xOffset` arguments you can push bars together to create grouped bar graphs. Error bars can also be added with the `yError` argument. Some [cookbook examples](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#plot-bar-data) demonstrate what this new function can do.
* **Scatter plots support X and Y error bars:** `plotScatter()` now has arguments to allow X and Y error bars (with adjustable error bar line width and cap size). A [cookbook example](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#plotting-with-errorbars) was added and a demo program was also created to demonstrate this feature.
* **Draggable axis lines:** `plotHLine()` and `plotVLine()` now have a `draggable` argument which lets those axis lines be dragged around with the mouse (when using the `ScottPlotUC` user control). Examples are in the demo folder ([ScottPlotDraggableMarkers](https://github.com/swharden/ScottPlot/tree/master/demos/ScottPlotDraggableMarkers)). This feature was initially requested in [issue 11](https://github.com/swharden/ScottPlot/issues/11).

### Minor changes
* fixed errors caused by resizing to 0px
* fixed a capitalization inconsistency in the `plotSignal` argument list
* `axisAuto()` now responds to axis lines added by `plotHLine()` and `plotVLine()` (previously they were ignored)
* fixed an [issue](https://github.com/swharden/ScottPlot/issues/23) that caused SplitContainer splitters to freeze

## ScottPlot 3.0.3
_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-05-28_
* NuGet installer automatically adds system.drawing

## ScottPlot 3.0.2
_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-05-27_
* Recompiled to support the .NET 4.5 framework

### ScottPlot 3.0.1
_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-05-27_
* First version of ScottPlot published on NuGet