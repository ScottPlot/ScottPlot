# ScottPlot Changelog

## ScottPlot 4.0.36 ⚠️ _in active development_
* `PlotSignal()` and `PlotSignalXY()` plots now have an optional `useParallel` argument (and public property on the objects they return) to allow the user to decide whether parallel or sequential calculations will be performed. (#454, #419, #245, #72) _Thanks @StendProg_

## ScottPlot 4.0.35
* Added `processEvents` argument to `formsPlot2.Render()` to provide a performance enhancement when linking axes of two `FormsPlot` controls together (by calling `Plot.MatchAxis()` from the control's `AxesChanged` event, as seen in the _Linked Axes_ demo application) (#451, #452) _Thanks @StendProg and @robokamran_
* New `Plot.PlotVectorField()` method for displaying vector fields (sometimes called quiver plots) (#438, #439, #440) _Thanks @Benny121221 and @hhubschle_
* Included an experimental colormap module which is likely to evolve over subsequent releases (#420, #424, #442) _Thanks @Benny121221_
* `PlotScatterHighlight()` was created as a type of scatter plot designed specifically for applications where "show value on hover" functionality is desired. Examples are both in the cookbook and WinForms and WPF demo applications. (#415, #414) _Thanks @Benny121221 and @StendProg_
* `PlotRadar()` is a new plot type for creating Radar plots (also called spider plots or star plots). See cookbook and demo application for examples. (#428, #430) _Thanks @Benny121221_
* `PlotPlolygons()` is a new performance-optimized variant of `PlotPolygon()` designed for displaying large numbers of complex shapes (#426) _Thanks @StendProg_
* The WinForms control's `Configure()` now has a `showCoordinatesTooltip` argument to continuously display the position at the tip of the cursor as a tooltip (#410) _Thanks @jcbeppler_
* User controls now use SHIFT (previously ALT) to lock the horizontal axis and ALT (previously SHIFT) while left-click-dragging for zoom-to-region. Holding CTRL+SHIFT while right-click-dragging now zooms evenly, without X/Y distortion. (#436) _Thanks @tomwimmenhove and @StendProg_
* Parallel processing is now enabled by default. Performance improvements will be most noticeable on Signal plots. (#419, #245, #72)
* `Plot.PlotBar()` now has an `autoAxis` argument (which defaults `true`) that automatically adjusts the axis limits so the base of the bar graphs touch the edge of the plot area. (#406)
* OSX-specific DLLs are now only retrieved by NuGet on OSX (#433, #211, #212)
* Pie charts can now be made with `plt.PlotPie()`. See cookbook and demo application for examples. (#421, #423) _Thanks @Benny121221_
* `ScottPlot.FormsPlotViewer(Plot)` no longer resets the new window's plot to the default style (#416)  _Thanks @StendProg_
* Controls now have a `recalculateLayoutOnMouseUp` option to prevent resetting of manually-defined data area padding

## ScottPlot 4.0.34
* Improve display of `PlotSignalXY()` by not rendering markers when zoomed very far out (#402) _Thanks @gobikulandaisamy_
* Optimized rendering of solid lines which have a user-definable `LineStyle` property. This modification improves grid line rendering and increases performance for most types of plots. (#401, #327) _Thanks @bukkideme and @citizen3942_

## ScottPlot 4.0.33
* Force grid lines to always draw using anti-aliasing. This compensates for a bug in `System.Drawing` that may cause diagonal line artifacts to appear when the user controls were panned or zoomed. (#401, #327) _Thanks @bukkideme and @citizen3942_

## ScottPlot 4.0.32
* User controls now have a `GetMouseCoordinates()` method which returns the DPI-aware position of the mouse in graph coordinates (#379, #380) _Thanks @Benny121221_
* Default grid color was lightened in the user controls to match the default style (#372)
* New `PlotSignalXY()` method for high-speed rendering of signal data that has unevenly-spaced X coordinates (#374, #375) _Thanks @StendProg and @LogDogg_
* Modify `Tools.Log10()` to return `0` instead of `NaN`, improving automatic axis limit detection (#376, #377) _Thanks @Benny121221_
* WpfPlotViewer and FormsPlotViewer launch in center of parent window (#378)
* Improve reliability of `Plot.AxisAutoX()` and `Plot.AxisAutoY()` (#382)
* The `Configure()` method of FormsPlot and WpfPlot controls now have `middleClickMarginX` and `middleClickMarginY` arguments which define horizontal and vertical auto-axis margin used for middle-clicking. Setting horizontal margin to 0 is typical when plotting signals. (#383)
* `Plot.Grid()` and `Plot.Ticks()` now have a `snapToNearestPixel` argument which controls whether these lines appear anti-aliased or not. For static images non-anti-aliased grid lines and tick marks look best, but for continuously-panning plots anti-aliased lines look better. The default behavior is to enable snapping to the nearest pixel, consistent with previous releases. (#384)
* Mouse events (MouseDown, MouseMove, etc.) are now properly forwarded to the FormsPlot control (#390) _Thanks @Minu476_
* Improved rendering of very small candlesticks and OHLCs in financial plots
* Labeled plottables now display their label in the ToString() output. This is useful when viewing plottables listed in the FormsPlot settings window #391 _Thanks @Minu476_
* Added a Statistics.Finance module with methods for creating Simple Moving Average (SMA) and Bollinger band technical indicators to Candlestick and OHLC charts. Examples are in the cookbook and demo program. (#397) _Thanks @Minu476_
* Scatter plots, filled plots, and polygon plots now support Xs and Ys which contain `double.NaN` #396
* Added support for line styles to Signal plots (#392) _Thanks @bukkideme_

## ScottPlot 4.0.31
* Created `Plot.PlotBarGroups()` for easier construction of grouped bar plots from 2D data (#367) _Thanks @Benny121221_
* Plot.PlotScaleBar() adds an L-shaped scalebar to the corner of the plot (#363)
* Default grid color lightened from #D3D3D3 (Color.LightGray) to #EFEFEF (#372)
* Improved error reporting for scatter plots (#369) _Thanks @JagDTalcyon_
* Improve pixel alignment by hiding grid lines and snapping tick marks that are 1px away from the lower left edge (#359)
* PlotText() ignores defaults to upperLeft alignment when rotation is used (#362)
* Improved minor tick positioning to prevent cases where minor ticks are 1px away from major ticks (#373)

## ScottPlot 4.0.30
* `Plot.PlotCandlestick()` and `Plot.PlotOHLC()`
  * now support `OHLC` objects with variable widths defined with a new `timeSpan` argument in the OHLC constructor. (#346) _Thanks @Minu476_
  * now support custom up/down colors including those with transparency (#346) _Thanks @Minu476_
  * have a new `sequential` argument to plot data based on array index rather than `OHLC.time`. This is a new, simpler way to display unevenly-spaced data (e.g., gaps over weekends) in a way that makes the gaps invisible. (#346) _Thanks @Minu476_
* Fixed a marker/line alignment issue that only affeced low-density Signal plots on Linux and MacOS (#340) _Thanks @SeisChr_
* WPF control now appears in Toolbox (#151) _Thanks @RalphLAtGitHub_
* Plot titles are now center-aligned with the data area, not the figure. This improves the look of small plots with titles. (#365) _Thanks @Resonanz_
* Fixed bug that ignored `Configure(enableRightClickMenu: false)` in WPF and WinForms user controls. (#365) _Thanks @thunderstatic_
* Updated `Configure(enableScrollWheelZoom: false)` to disable middle-click-drag zooming. (#365) _Thanks @eduhza_
* Added color mixing methods to ScottPlot.Drawing.GDI (#361)
* Middle-click-drag zooming now respects locked axes (#353) _Thanks @LogDogg_
* Improved user control zooming of high-precision DateTime axis data (#351) _Thanks @bukkideme_
* Plot.AxisBounds() now lets user set absolute bounds for drag and pan operations (#349) _Thanks @LogDogg_
* WPF control uses improved Bitmap conversion method (#350)
* Function plots have improved handling of functions with infinite values (#370) _Thanks @Benny121221_

## ScottPlot 4.0.29
* `Plot.PlotFill()` can be used to make scatter plots with shaded regions. Giving it a single pair of X/Y values (`xs, ys`) lets you shade beneath the curve to the `baseline` value (which defaults to 0). You can also give it a pair of X/Y values (`xs1, ys1, xs2, ys2`) and the area between the two curves will be shaded (the two curves do not need to be the same length). See cookbook for examples. (#255) _Thanks @ckovamees_ 
* `DataGen.Range()` now has `includeStop` argument to include the last value in the returned array.
* `Tools.Pad()` has been created to return a copy of a given array padded with data values on each side. (#255) _Thanks @ckovamees_
* [Seaborn](https://seaborn.pydata.org/) style can be activated using `Plot.Style(Style.Seaborn)` (#339)
* The `enableZooming` argument in `WpfPlot.Configure()` and `FormsPlot.Configure()` has been replaced by two arguments `enableRightClickZoom` and `enableScrollWheelZoom` (#338) _Thanks Zach_
* Improved rendering of legend items for polygons and filled plots (#341) _Thanks @SeidChr_
* Improved Linux rendering of legend items which use thick lines: axis spans, fills, polygons, etc. (#340) _Thanks @SeidChr_
* Addded `Plot.PlotFillAboveBelow()` to create a shaded line plot with different colors above/below the baseline. (#255) _Thanks @ckovamees_
* Improved rendering in Linux and MacOS by refactoring the font measurement system (#340) _Thanks @SeidChr_

## ScottPlot 4.0.28
* `Ticks()` now has arguments for numericStringFormat (X and Y) to make it easy to customize formatting of tick labels (percentage, currency, scientific notation, etc.) using standard [numeric format strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings). Example use is demonstrated in the cookbook. (#336) _Thanks @deiruch_
* The right-click menu can now be more easily customized by writing a custom menu to `FormsPlot.ContextMenuStrip` or `WpfPlot.ContextMenu`. Demonstrations of both are in the demo application. (#337) _Thanks @Antracik_

## ScottPlot 4.0.27
* `Plot.Polygon()` can now be used to plot polygons from X/Y points (#255) _Thanks @ckovamees_
* User controls now have an "open in new window" item in their right-click menu (#280)
* Plots now have offset notation and multiplier notation disabled by default. Layouts are automatically calculated before the first render, or manually after MouseUp events in the user controls. (#310)
* `Plot.Annotation()` allows for the placement of text on the figure using pixel coordinates (not unit coordinates on the data grid). This is useful for creating custom static labels or information messages. (#321) _Thanks @SeidChr_
* `FormsPlot.MouseDoubleClicked` event now passes a proper `MouseEventArgs` instead of `null` (#331) _Thanks @ismdiego_
* Added a right-click menu to `WpfPlot` with items (save image, copy image, open in new window, help, etc.) similar to `FormsPlot`

## ScottPlot 4.0.26
* The `ScottPlot.WPF` package (which provides the `WpfPlot` user control) now targets .NET Framework 4.7.2 (in addition to .NET Core 3.0), allowing it to be used in applications which target either platform. The ScottPlot demo application now targets .NET Framework 4.7.2 which should be easier to run on most Windows systems. (#333)
* The `ScottPlot.WinForms` package (which produves the `FormsPlot` control) now only targets .NET Framework 4.6.1 and .NET Core 3.0 platforms (previously it also had build targets for .NET Framework 4.7.2 and .NET Framework 4.8). It is important to note that no functionality was lost here. (#330, #333)

## ScottPlot 4.0.25
* `PlotBar()` now supports displaying values above each bar graph by setting the `showValues` argument.
* `PlotPopulations()` has extensive capabilities for plotting grouped population data using box plots, bar plots, box and whisper plots, scatter data with distribution curves, and more! See the cookbook for details. (#315)
* `Histogram` objects now have a `population` property.
* `PopulationStats` has been renamed to `Population` and has additional properties and methods useful for reporting population statistics.
* Improved grid rendering rare artifacts which appear as unwanted diagnal lines when anti-aliasing is disabled. (#327)

## ScottPlot 4.0.24
* `Plot.Clear()` has been improved to more effectively clear plottable objects. Various overloads are provided to selectively clear or preserve certain plot types. (#275) _Thanks @StendProg_
* `PlotBar()` has been lightly refactored. Argument order has been adjusted, and additional options have been added. Error cap width is now in fractional units instead of pixel units. Horizontal bar charts are now supported. (#277, #315) _Thanks @bonzaiferroni_

## ScottPlot 4.0.23
* Interactive plot viewers were created to make it easy to interactively display data in a pop-up window without having to write any GUI code. Examples have been added to the ScottPlot Demo application.
  * `ScottPlot.WpfPlotViewer(plt)` for WPF
  * `ScottPlot.FormsPlotViewer(plt)` for Windows Forms
  * These can even be called from console applications
* Fixed bug that affected the `ySpacing` argument of `Plot.Grid()`
* `Plot.Add()` makes it easy to add a custom `Plottable` to the plot
* `Plot.XLabels()` and `Plot.YLabels()` can now accept just a string array (x values are auto-populated as a consecutive series of numbers).
* Aliased `Plot.AxisAuto()` to `Plot.AutoAxis()` and `Plot.AutoScale()` to make this function easier to locate for users who may have experience with other plot libraries. (#309) _Thanks @Resonanz_
* Empty plots now render grid lines, ticks, and tick labels (#313)
* New plot type: Error bars. They allow the user to define error bar size in all 4 directions by calling `plt.PlotErrorBars()`. (#316) _Thanks @zrolfs_
* Improve how dashed lines appear in the legend
* Improved minor tick positions when using log scales with `logScaleX` and `logScaleY` arguments of `plt.Ticks()` method
* Fixed bug that caused the center of the coordinate field to shift when calling `Plot.AxisZoom()`
* Grid line thickness and style (dashed, dotted, etc) can be customized with new arguments in the `Plot.Grid()` method

## ScottPlot 4.0.22
* Added support for custom horizontal axis tick rotation (#300) _Thanks @SeidChr_
* Added support for fixed grid spacing when using DateTime axes (#299) _Thanks @SeidChr_
* Updated ScottPlot icon (removed small text, styled icon after emoji)
* Improved legend font size when using display scaling (#289)
* Scroll wheel zooming now zooms to cursor (instead of center) in WPF control. This feature works now even if display scaling is used. (#281)
* Added `Plot.EqualAxis` property to make it easy to lock axis scales together (#306) _Thanks @StendProg_

## ScottPlot 4.0.21

### Misc
* Created new cookbook and demo applications for WinForms and WPF (#271)
* The `FormsPlot.MouseMoved` event now has `MouseEventArgs` (instead of `EventArgs`). The purpose of this was to make it easy to access mouse pixel coordinates via `e.X` and `e.Y`, but this change may require modifications to applications which use the old event signature.
* WpfPlot now has an `AxisChanged` event (like FormsPlot)
* Fixed bug that caused `Plot.CoordinateFromPixelY()` to return incorrect value
* Fixed bug causing cursor to show arrows when hovered over some non-draggable objects
* Improved support for WinForms and WpfPlot transparency (#286) _Thanks @StendProg and @envine_
* Added `DataGen.Zeros()` and `DataGen.Ones()` to generate arrays filled with values using methods familiar to numpy users.
* Added `equalAxes` argument to `WpfPlot.Configure()` (#272)
* Fixed a bug affecting the `equalAxes` argument in `FormsPlot.Configure()` (#272)
* Made all `Plot.Axis` methods return axis limits as `double[]` (previously many of them returned `void`)
* Added overload for `Plot.PlotLine()` which accepts a slope, offset, and start and end X points to make it easy to plot a linear line with known formula. Using PlotFormula() will produce the same output, but this may be simpler to use for straight lines.
* Added `rSquared` property to linear regression fits (#290) _Thanks @Benny121221 and @StendProg_
* Added `Tools.ConvertPolarCoordinates()` to make it easier to display polar data on ScottPlot's Cartesian axes (#298) _Thanks @Benny121221_
* Improved `Plot.Function()` (#243) _Thanks @Benny121221_
* Added overload for `Plot.SetCulture()` to let the user define number and date formatting rather than relying on pre-made cultures (#301, #236) _Thanks @SeidChr_

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
