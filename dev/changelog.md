# ScottPlot Changelog

## ScottPlot 3.1.0

_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-08-19_

### Major Changes
* User controls were renamed
  * `ScottPlotUC` was renamed to `FormsPlot`
  * `ScottPlotWPF` was renamed to `WpfPlot`
* The right-click menu has improved. It responds faster and has improved controls to adjust plot settings.
* Plots can now be saved in BMP, PNG, JPG, and TIF format
* Holding `CTRL` while click-dragging locks the horizontal axis
* Holding `ALT` while click-dragging locks the vertical axis
* Minor ticks are now displayed (and can be turned on or off with `Ticks()`)
* Legend can be accessed for external display with `GetLegendBitmap()`

### Minor Changes
* anti-aliasing is turned off while click-dragging to increase responsiveness (#93, @StendProg)
* PlotSignalConst has several improvements (@StendProg)
  * It can can now accept a generic inputs
  * A demo has been added demonstrating highspeed interactive plotting of _one billion_ data points.
  * It is now slightly faster by default
  * It can use single-precision floating point calculations to further enhance performance
* Legend draws more reliably (#104, #106, @StendProg)
* `AxisAuto()` now has `expandOnly` arguments
* Axis lines with custom lineStyles display properly in the legend

## ScottPlot 3.0.9

_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-08-12_

### Major Changes
* **WPF User Control**: See [demos#wpf-application](https://github.com/swharden/ScottPlot/tree/master/demos#wpf-application) for a quickstart demo.
* **New Plot Type: `PlotSignalConst()`** for extremely large arrays of data which are not expected to change after being plotted. Plots generated with this method can be much faster than PlotSignal(). See [cookbook#signalconst](https://github.com/swharden/ScottPlot/tree/master/cookbook#signalconst) for example usage. _Special thanks to @StendProg for work on this feature._
* **Greatly improved axis tick labels.** Axis tick labels are now less likely to overlap with axis labels, and it displays very large and very small numbers well using exponential notation. For an example see [cookbook#axis-exponent-and-offset](https://github.com/swharden/ScottPlot/tree/master/cookbook#axis-exponent-and-offset). _Special thanks to @Padanian for work on this feature._
* **Parallel processing support for PlotSignal()**. When parallel processing is enabled PlotSignal() can now use it to render graphs faster. For details see [cookbook#signal-with-parallel-processing](https://github.com/swharden/ScottPlot/tree/master/cookbook#signal-with-parallel-processing). _Special thanks to @StendProg for work on this feature._
* **Every `Plot` function now returns a `Plottable`.** When creating things like scatter plots, text, and axis lines, the returned object can now be used to update the data, position, styling, or call plot-type-specific methods. For an example see [cookbook#modify-styles-after-plotting](https://github.com/swharden/ScottPlot/tree/master/cookbook#modify-styles-after-plotting).

### Minor Changes
* right-click menu now displays ScottPlot and .NET Framework version
* improved rendering of extremely zoomed-out signals 
* rendering speed increased now that Format32bppPArgb is the default PixelFormat (thanks @StendProg)
* DataGen.NoisySin() was added
* Code was tested in .NET Core 3.0 preview and compiled without error. Therefore, the next release will likely be for .NET Core 3.0 (Thanks @petarpetrovt)
* User controls now render graphs with anti-alias mode off (faster) while the mouse is being dragged. Upon release a high quality render is performed.

## ScottPlot 3.0.8

_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-08-04_

### Major Changes
* **WPF User Control:** A ScottPlotWPF user control was created to allow provide a simple mouse-interactive ScottPlot control to WPF applications. It is not as full-featured as the winforms control (it lacks a right-click menu and click-and-drag functions), but it is simple to review the code (<100 lines of [.xaml](https://github.com/swharden/ScottPlot/blob/master/src/ScottPlot/ScottPlotWPF.xaml) and [.xaml.cs](https://github.com/swharden/ScottPlot/blob/master/src/ScottPlot/ScottPlotWPF.xaml.cs)) and easy to use. See the [WPF Application Quickstart](https://github.com/swharden/ScottPlot/tree/master/demos#wpf-application) guide for details.
* **New plot type `plt.AxisSpan()`:** [demonstrated in the cookbook](https://github.com/swharden/ScottPlot/tree/master/cookbook#axis-spans) - shades a region of the graph (semi-transparency is supported).

### Minor Changes
* **improved tick marks**
  * Vertical ticks no longer overlap with vertical axis label (#47)
  * When axis tick labels contain very large or very small numbers, scientific notation mode is engaged (see [cookbook example](https://github.com/swharden/ScottPlot/tree/master/cookbook#very-large-numbers)).
  * Horizontal tick mark spacing increased to prevent overlapping
  * Vertical tick mark spacing increased to be consistent with horizontal tick spacing
* **CSV data export**
  * Plottable objects now have a `SaveCSV(filename)` method. See the [cookbook example](https://github.com/swharden/ScottPlot/tree/master/cookbook#save-scatter-data).
  * Scatter and Signal plot data can be saved from the user control through the right-click menu.
* Added `lineStyle` arguments to Scatter plots (see the [cookbook example](https://github.com/swharden/ScottPlot/tree/master/cookbook#custom-linestyles))
* Improved legend ([see cookbook example](https://github.com/swharden/ScottPlot/tree/master/cookbook#legend))
  * ability to set location
  * ability to set shadow direction
  * markers and lines rendered in legend
* Improved ability to use custom fonts ([see cookbook example](https://github.com/swharden/ScottPlot/tree/master/cookbook#custom-fonts))
* Segoe UI is now the default font for all plot components

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