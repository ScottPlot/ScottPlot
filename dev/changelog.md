# ScottPlot Changelog

## Primary Version History

_ScottPlot uses [semantic](https://semver.org/) (major.minor.patch) versioning. Patches are typically non-breaking, but switching between major and minor versions may require modification of existing code._

* **ScottPlot 4.0** (Nov, 2019) ScottPlot.Plot module became platform-agnostic using .NET Standard and System.Drawing.Common. Total recode, but same API. User controls became separate, platform-specific modules.
* **ScottPlot 3.0** (May, 2019) Total recode with new API. First version released on NuGet.
* **ScottPlot 2.0** (Jan, 2019) Total recode with new API. First version to get its own GitHub project. 
* **ScottPlot 1.0** (June, 2017) ScottPlot began as [swhPlot.cs](https://github.com/swharden/Csharp-Data-Visualization/blob/master/projects/17-06-24_stretchy_line_plot/pixelDrawDrag2/swhPlot.cs), a 150 line class used to create a [stretchy line plot](https://github.com/swharden/Csharp-Data-Visualization/tree/master/projects/17-06-24_stretchy_line_plot) demonstrating how to draw lines interactively with C#.

## Coming Soon

* **This version (4.0.x)** - this list is a reminder of things to complete before proceeding to the next major version.
  * Improve support for display scaling (#273)
  * Create standalone user control launchers
* **ScottPlot 4.1**
  * Refactor plottable module (namespaces will change slightly)
* **ScottPlot 4.2** 
  * Remove rendering capabilities from ScottPlot.Plot so the dependency on System.Drawing can be eliminated
  * Create a GDI rendering module which uses System.Drawing 
  * Create a SkiaSharp rendering module and user control (supporting OpenGL hardware acceleration)

## ScottPlot 4.0.20 (IN DEVELOPMENT)

#### Work to do...
* `Plot.Function()` (#243) _Thanks @Benny121221_
* `Plot.BoxAndWhisker()`
* improved bar charts (#244, #260, #277) _Thanks @Benny121221 and @bonzaiferroni and @SoManyUsernamesTaken_
  * support for stacked box plots
  * support for horizontal box plots
* support for "shade below the curve" (#255) _Thanks @ckovamees_
* new demo/cookbook system and demo applications (#271)

### Misc
* Fixed bug that caused `Plot.CoordinateFromPixelY()` to return incorrect value

## ScottPlot 4.0.19

#### Plottable and Rendering Changes
* Improved how markers are drawn in Signal and SignalConst plots at the transition area between zoomed out and zoomed in (#263) _Thanks @bukkideme and @StendProg_
* Improved support for zero lineSize and markerSize in Signal and SignalConst plots (#263, #264) _Thanks @bukkideme and @StendProg_
* Improved thread safety of interactive graphs (#245) _Thanks @StendProg_

#### Changes to `ScottPlot.Plot` Module
* added `CoordinateFromPixelX()` and `CoordinateFromPixelY()` to get _double precision_ coordinates from a pixel location. Previously only SizeF (float) precision was available. This improvement is especially useful when using DateTime axes. (#269) _Thanks Chris_
* added `AxisScale()` to adjust axis limits to set a defined scale (units per pixel) for each axis.
* added `AxisEqual()` to adjust axis limits to set the scale of both axes to be the same regardless of the size of each axis. (#272) _Thanks @gberrante_
* `PlotHSpan()` and `PlotVSpan()` now return `PlottableHSpan` and `PlottableVSpan` objects (instead of a `PlottableAxSpan` with a `vertical` property)
* `PlotHLine()` and `PlotVLine()` now return `PlottableHLine` and `PlottableVLine` objects (instead of a `PlottableAxLine` with a `vertical` property)

#### Miscellaneous
* MultiPlot now has a `GetSubplot()` method which returns the Plot from a row and column index (#242). See cookbook for details. _Thanks @Resonanz and @StendProg_
* Created `DataGen.Range()` to make it easy to create double arrays with evenly spaced data (#259)
* Improved support for display scaling (#273) _Thanks @zrolfs_
* Improved event handling (#266, #238) _Thanks @StendProg_
* Improved legend positioning (#253) _Thanks @StendProg_

## ScottPlot 4.0.18
* Added `Plot.SetCulture()` for improved local culture formatting of numerical and DateTime axis tick labels (#236) _Thanks @teejay-87_

## ScottPlot 4.0.17
* Added `mouseCoordinates` property to WinForms and WPF controls (#235) _Thanks @bukkideme_
* Fixed rendering bug that affected horizontal lines when anti-aliasing was turned off (#232) _Thanks @StendProg_
* Improved responsiveness while dragging axis lines and axis spans (#228) _Thanks @StendProg_

## ScottPlot 4.0.16
* Improved support for MacOS and Linux (#211, #212, #216) _Thanks @Hexxonite and @StendProg_
* Fixed a few display bugs
  * Fixed a bug affecting the `ySpacing` argument in `Plot.Grid()` (#221) _@Thanks teejay-87_
  * Enabled `visible` argument in `Title()`, `XLabel()`, and `YLabel()` (#222) _Thanks @ckovamees_
* AxisSpan improvements
  * Edges are now optionally draggable (#228) _Thanks @StendProg_
  * Can now be selectively removed with `Clear()` argument
  * Fixed bug caused by zooming far into an axis span (#226) _Thanks @StendProg_
* WinForms Control
  * WinForms control now supports draggable axis lines and axis spans
  * Right-click menu now has "copy image" option (#220)
  * Settings screen now has "copy CSV" button to export data (#220)
* WPF Control
  * WPF control now supports draggable axis lines and axis spans
  * new WpfPlot.Configure() to set various WPF control options
* Misc improvements
  * Improved axis handling, expansion, and auto-axis (#219, #230) _Thanks @StendProg_
  * Added more options to `DataGen.Cos()`
  * Tick labels can be hidden with `Ticks()` argument (#223) _Thanks @ckovamees_

## ScottPlot 4.0.14
* Improved `MatchAxis()` and `MatchLayout()` (#217) _Thanks @ckovamees and @StendProg_

## ScottPlot 4.0.13
* Improved support for Linux and MacOS _Thanks @Hexxonite_
* Improved font validation (#211, #212) _Thanks @Hexxonite and @StendProg_

## ScottPlot 4.0.11
* User controls now have a `cursor` property which can be set to allow custom cursors. (#187) _Thanks @gobikulandaisamy_
* User controls now have a `mouseCoordinates` property which make it easy to get the X/Y location of the cursor. (#187) _Thanks @gobikulandaisamy_

## ScottPlot 4.0.10
* Improved density colormap (#192, #194) _Thanks @StendProg_
* Added linear regression tools and cookbook example (#198) _Thanks @Benny121221_
* Added `maxRenderIndex` to Signal to allow partial plotting of large arrays intended to be used with live, incoming data (#202) _Thanks @StendProg and @plumforest_
* Made _Shift + Left-click-drag_ zoom into a rectangle light middle-click-drag (in WinForms and WPF controls) to add support for mice with no middle button (#90) _Thanks @JagDTalcyon_
* Throw an exception if `SaveFig()` is called before the image is properly sized (#192) _Thanks @karimshams and @StendProg_
* Ticks() now has arguments for `FontName` and `FontSize` (#204) _Thanks Clay_
* Fixed a bug that caused poor layout due to incorrect title label size estimation (#205) _Thanks Clay_
* Grid() now has arguments to selectively enable/disable horizontal and vertical grid lines (#206) _Thanks Clay_
* Added tool and cookbook example to make it easier to plot data on a log axis (#207) _Thanks @senged_
* Arrows can be plotted using `plt.PlotArrow()` (#201) _Thanks Clay_

## ScottPlot 4.0.9
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-12-03_

* Use local regional display settings when formatting the month tick of DateTime axes (#108) _Thanks @FadyDev2_
* Debug symbols are now deployed NuGet

## ScottPlot 4.0.7
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-12-01_\
This release updated the ScottPlot.WinForms package only.

* Added WinForms support for .NET Framework 4.7.2 and 4.8
* Fixed bug in WinForms control that only affected .NET Core 3.0 applications (#189 and #138) _Thanks @petarpetrovt_

## ScottPlot 4.0.6
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-29_\
This release updated the ScottPlot.WinForms package only.

* fixed bug that affected the settings dialog window in the WinForms control (#187) _Thanks @gobikulandaisamy_

## ScottPlot 4.0.5
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-27_

#### Minor Changes
* improved spacing for non-uniformly distributed OHLC and candlestick plots (#184) _Thanks @satyat110_
* added `fixedLineWidth` to Legend() to allow the user to control whether legend lines are dynamically sized (#185) _Thanks @ab-tools_
* legend now hides lines or markers of they're hidden in the plottable
* DateTime axes now use local display format (#108) _Thanks @FadyDev2_

## ScottPlot 4.0.4
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-24_

* PlotText() now supports a background frame (#181) _Thanks @satyat110_
* OHLC objects can be created with a double or a DateTime (#182) _Thanks @Minu476_
* Improved AxisAuto() fixes bug for mixed 2d and axis line plots

## ScottPlot 4.0.3
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-23_

* fixed bug when plotting single-point candlestick (#172) _Thanks @Minu476_
* improved style editing of plotted objects (#173) _Thanks @Minu476_
* fixed pan/zoom axis lock when holding CTRL or ALT (#90) _Thanks @FadyDev2_
* simplified the look of the user controls in designer mode
* improved WPF control mouse tracking when using DPI scaling
* added support for manual tick positions and labels (#174) _Thanks @Minu476_
* improved tick system when using DateTime units (#108) _Thanks @Padanian, @FadyDev2, and @Bhandejiya_
* created `Tools.DateTimesToDoubles(DateTime[] array)` to easily convert an array of dates to doubles which can be plotted with ScottPlot, then displayed as time using `plt.Ticks(dateTimeX: true)`.
* added an inverted sign flag (#177) to allow display of an axis with descending units _Thanks Bart_

## ScottPlot 4.0.2
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-09_

* **Multi-plot figures:** Images with several plots can be created using `ScottPlot.MultiPlot()` as seen in the [Multiplot example](https://github.com/swharden/ScottPlot/tree/master/cookbook#multiplot) in the cookbook
* `ScottPlot.DataGen` functions which require a `Random` can accept null (they will create a `Random` if null is given)
* `plt.MatchAxis()` and `plt.MatchLayout()` have been improved
* `plt.PlotText()` now supports rotated text using the `rotation` argument (#160). See the [cookbook example](https://github.com/swharden/ScottPlot/tree/master/cookbook#plotting-text). _Thanks @gwilson9_
* `ScottPlot.WinForms` user control has new events and `formsPlot1.Configure()` arguments to make it easy to replace the default functionality for double-clicking and deploying the right-click menu (#166). _Thanks @FadyDev2_
* All plottables now have a `visible` property which makes it easy to toggle visibility on/off after they've been plotted. See the [cookbook example](https://github.com/swharden/ScottPlot/tree/master/cookbook#set-visibility). _Thanks Nasser_

## ScottPlot 4.0.1
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-03_

#### Major Changes
* **ScottPlot now targets .NET Standard 2.0** so in addition to .NET Framework projects it can now be used in .NET Core applications, ASP projects, Xamarin apps, etc.
* **The WinForms control has its own package** ([ScottPlot.WinForms](https://www.nuget.org/packages/ScottPlot.WinForms/)) which targets both .NET Framework 4.6.1 and  .NET Core 3.0. Thanks for your early efforts on this @petarpetrovt
* **The WPF control has its own package** ([ScottPlot.WPF](https://www.nuget.org/packages/ScottPlot.WPF/)) targeting .NET Core 3.0.

#### Minor Changes
* better layout system and control of padding (Thanks @citizen3942)
* added ruler mode to `plt.Ticks()` (Thanks @citizen3942)
* `plt.MatchLayout()` no longer throws exceptions
* eliminated `MouseTracker` class (tracking is now in user controls)
* Use NUnit (not MSTest) for tests

## ScottPlot 3.1.6
_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-10-20_

#### Minor Changes
* reduced designer mode checks to increase render speed (Thanks @StendProg)
* fixed cursor bug that occurred when draggable axis lines were used (Thanks Kamran)
* fully deleted the outdated `ScottPlotUC`
* fixed infinite zoom bug caused by calling AxisAuto() when plotting a single point (or perfectly straight horizontal or vertical line)
* added `ToolboxItem` and `DesignTimeVisible` delegates to WpfPlot control to try to get it to appear in the toolbox (but it doesn't seem to be working)
* improved figure padding when axes frames are disabled (Thanks @citizen3942)
* improved rendering of ticks at the edge of the plottable area (Thanks @citizen3942)
* added `AxesChanged` event to user control to make it easier to sync axes between multiple plots (see linked plots demo)
* disabled drawing of arrows on user control in designer mode

## ScottPlot 3.1.5
_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-10-06_

#### Minor Changes
* WPF user control improved support for display scaling (Thanks @morningkyle)
* Fixed bug that crashed on extreme zoom-outs (Thanks @morningkyle)
* WPF user control improvements (middle-click autoaxis, scrollwheel zoom)
* ScottPlot user control has a new look in designer mode. Exceptions in user controls in designer mode can crash Visual Studio, so this risk is greatly reduced by not attempting to render a ScottPlot _inside_ Visual Studio.

## ScottPlot 3.1.4
_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-09-22_

#### Major Changes
* middle-click-drag zooms into a rectangle drawn with the mouse

#### Minor Changes
* fixed bug that caused user control to crash Visual Studio on some systems that used DPI scaling (#125, #111). _Thanks @ab-tools and @bukkideme for your work on this._
* fixed poor rendering for extremely small plots
* fixed bug when making a scatter plot with a single point (#126). _Thanks @bonzaiferroni for your work on this._
* added more options to right-click settings menu (grid options, legend options, axis labels, editable plot labels, etc.)
* improved axis padding and image tightening
* greatly refactored the settings module (no change in functionality)

## ScottPlot 3.1.3

_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-08-25_

#### Minor Changes
* FormsPlot improvements
  * middle-click-drag zooms into a rectangle
  * CTRL+scroll to lock vertical axis
  * ALT+scroll to loch horizontal axis
  * Improved (and overridable) right-click menu
* Added additional options to `plt.Ticks()`
  * rudimentary support for date tick labels (`dateTimeX` and `dateTimeY`)
  * options to customize notation (`useExponentialNotation`, `useOffsetNotation`, and `useMultiplierNotation`)

## ScottPlot 3.1.0

_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-08-19_

#### Major Changes
* User controls were renamed
  * `ScottPlotUC` was renamed to `FormsPlot`
  * `ScottPlotWPF` was renamed to `WpfPlot`
* The right-click menu has improved. It responds faster and has improved controls to adjust plot settings.
* Plots can now be saved in BMP, PNG, JPG, and TIF format
* Holding `CTRL` while click-dragging locks the horizontal axis
* Holding `ALT` while click-dragging locks the vertical axis
* Minor ticks are now displayed (and can be turned on or off with `Ticks()`)
* Legend can be accessed for external display with `GetLegendBitmap()`

#### Minor Changes
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

#### Major Changes
* **WPF User Control**: See [demos#wpf-application](https://github.com/swharden/ScottPlot/tree/master/demos#wpf-application) for a quickstart demo.
* **New Plot Type: `PlotSignalConst()`** for extremely large arrays of data which are not expected to change after being plotted. Plots generated with this method can be much faster than PlotSignal(). See [cookbook#signalconst](https://github.com/swharden/ScottPlot/tree/master/cookbook#signalconst) for example usage. _Special thanks to @StendProg for work on this feature._
* **Greatly improved axis tick labels.** Axis tick labels are now less likely to overlap with axis labels, and it displays very large and very small numbers well using exponential notation. For an example see [cookbook#axis-exponent-and-offset](https://github.com/swharden/ScottPlot/tree/master/cookbook#axis-exponent-and-offset). _Special thanks to @Padanian for work on this feature._
* **Parallel processing support for PlotSignal()**. When parallel processing is enabled PlotSignal() can now use it to render graphs faster. For details see [cookbook#signal-with-parallel-processing](https://github.com/swharden/ScottPlot/tree/master/cookbook#signal-with-parallel-processing). _Special thanks to @StendProg for work on this feature._
* **Every `Plot` function now returns a `Plottable`.** When creating things like scatter plots, text, and axis lines, the returned object can now be used to update the data, position, styling, or call plot-type-specific methods. For an example see [cookbook#modify-styles-after-plotting](https://github.com/swharden/ScottPlot/tree/master/cookbook#modify-styles-after-plotting).

#### Minor Changes
* right-click menu now displays ScottPlot and .NET Framework version
* improved rendering of extremely zoomed-out signals 
* rendering speed increased now that Format32bppPArgb is the default PixelFormat (thanks @StendProg)
* DataGen.NoisySin() was added
* Code was tested in .NET Core 3.0 preview and compiled without error. Therefore, the next release will likely be for .NET Core 3.0 (Thanks @petarpetrovt)
* User controls now render graphs with anti-alias mode off (faster) while the mouse is being dragged. Upon release a high quality render is performed.

## ScottPlot 3.0.8

_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-08-04_

#### Major Changes
* **WPF User Control:** A ScottPlotWPF user control was created to allow provide a simple mouse-interactive ScottPlot control to WPF applications. It is not as full-featured as the winforms control (it lacks a right-click menu and click-and-drag functions), but it is simple to review the code (<100 lines of [.xaml](https://github.com/swharden/ScottPlot/blob/master/src/ScottPlot/ScottPlotWPF.xaml) and [.xaml.cs](https://github.com/swharden/ScottPlot/blob/master/src/ScottPlot/ScottPlotWPF.xaml.cs)) and easy to use. See the [WPF Application Quickstart](https://github.com/swharden/ScottPlot/tree/master/demos#wpf-application) guide for details.
* **New plot type `plt.AxisSpan()`:** [demonstrated in the cookbook](https://github.com/swharden/ScottPlot/tree/master/cookbook#axis-spans) - shades a region of the graph (semi-transparency is supported).

#### Minor Changes
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

#### Major Changes
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

#### Major Changes
* **Bar plot:** The plot module now has a `Bar()` method that lets users create various types of bar plots, as seen in [cookbook#plot-bar-data](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#plot-bar-data)
* **Histogram:** The new `ScottPlot.Histogram` class has tools to create and analyze histogram data (including cumulative probability). Examples of this can be seen at [cookbook#histogram](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#histogram) and [cookbook#cph](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#cph)
* **Step plot:** Scatter plots can now render as step plots. Use this feature by setting the `stepDisplay` argument with `PlotScatter()` as seen in the [cookbook#step-plot](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#step-plot)
* **Manual grid spacing:** Users can now manually define the grid density by setting the `xSpacing` and `ySpacing` arguments in `Grid()` as seen in [cookbook#manual-grid-spacing](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#manual-grid-spacing)
* **Draggable axis lines:** Axis lines can be dragged with the mouse if the `draggable` argument is set to `true` in `PlotHLine()` and `PlotHLine()`. Draggable axis line limits can also be set by defining additional arguments. The [DraggableMarkers](https://github.com/swharden/ScottPlot/tree/master/demos/ScottPlotDraggableMarkers) program was created to demonstrate this feature.

#### Minor Changes
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

#### New features
* **Bar graphs:** New `plotBar()` method allow creation of bar graphs. By customizing the `barWidth` and `xOffset` arguments you can push bars together to create grouped bar graphs. Error bars can also be added with the `yError` argument. Some [cookbook examples](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#plot-bar-data) demonstrate what this new function can do.
* **Scatter plots support X and Y error bars:** `plotScatter()` now has arguments to allow X and Y error bars (with adjustable error bar line width and cap size). A [cookbook example](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook#plotting-with-errorbars) was added and a demo program was also created to demonstrate this feature.
* **Draggable axis lines:** `plotHLine()` and `plotVLine()` now have a `draggable` argument which lets those axis lines be dragged around with the mouse (when using the `ScottPlotUC` user control). Examples are in the demo folder ([ScottPlotDraggableMarkers](https://github.com/swharden/ScottPlot/tree/master/demos/ScottPlotDraggableMarkers)). This feature was initially requested in [issue 11](https://github.com/swharden/ScottPlot/issues/11).

#### Minor changes
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

#### ScottPlot 3.0.1
_Published on [NuGet](https://www.nuget.org/packages/ScottPlot/) on 2019-05-27_
* First version of ScottPlot published on NuGet
