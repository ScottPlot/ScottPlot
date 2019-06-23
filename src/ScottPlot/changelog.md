# ScottPlot Changelog

## ScottPlot 3.0.4
_In development / not yet published on NuGet_

### New features
* **Errorbars:** `plotScatter()` now has arguments to allow X and Y error bars (with adjustable error bar line width and cap size). A cookbook example was added and a demo program was also created to demonstrate this feature.
* **Draggable axis lines:** `plotHLine()` and `plotVLine()` now have a `draggable` argument which lets those axis lines be dragged around with the mouse (when using the `ScottPlotUC` user control). Examples are in the demo folder.

### Minor changes
* fixed errors caused by resizing to 0px
* fixed a capitalization inconsistency in the `plotSignal` argument list
* `axisAuto()` now responds to axis lines added by `plotHLine()` and `plotVLine()` (previously they were ignored).

## ScottPlot 3.0.3
_Published on NuGet on 2019-05-28_
* NuGet installer automatically adds system.drawing

## ScottPlot 3.0.2
_Published on NuGet on 2019-05-27_
* Recompiled to support the .NET 4.5 framework

### ScottPlot 3.0.1
_Published on NuGet on 2019-05-27_
* First version of ScottPlot published on NuGet