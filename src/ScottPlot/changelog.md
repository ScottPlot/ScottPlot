# ScottPlot Changelog

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