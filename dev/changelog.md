# ScottPlot Changelog

## ScottPlot 4.1.30
_In development / not yet on NuGet_
* Plot: Improve values returned by `GetDataLimits()` when axis lines and spans are in use (#1415, #1505, #1532) _Thanks @EFeru_

## ScottPlot 4.1.29
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2022-01-02_
* WinForms Control: Improve ClearType text rendering by no longer defaulting to a transparent control background color (#1496)

## ScottPlot 4.1.28
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2022-01-01_
* Eto Control: New ScottPlot control for the Eto GUI framework (#1425, #1438) _Thanks @rafntor_
* Radar Plot: `OutlineWidth` now allows customization of the line around radar plots (#1426, #1277) _Thanks @Rayffer_
* Ticks: Improved minor tick and minor grid line placement (#1420, #1421) _Thanks @bclehmann and @at2software_
* Palette: Added Amber and Nero palettes (#1411, #1412) _Thanks @gauravagrwal_
* Style: Hazel style (#1414) _Thanks @gauravagrwal_
* MarkerPlot: Improved data area clipping (#1423, #1459) _Thanks @PremekTill, @lucabat, and @AndXaf_
* MarkerPlot: Improved key in legend (#1459, #1454) _Thanks @PremekTill and @Logicman111_
* Style: Plottables that implement `IStylable` are now styled when `Plot.Style()` is called. Styles are now improved for `ScaleBar` and `Colorbar` plot types. (#1451, #1447) _Thanks @diluculo_
* Population plot: Population plots `DataFormat` now have a `DataFormat` member that displays individual data points on top of a bar graph representing their mean and variance (#1440) Thanks _@Syntaxrabbit_
* SignalXY: Fixed bug affecting filled plots with zero area (#1476, #1477) _Thanks @chenxuuu_
* Cookbook: Added example showing how to place markers colored according to a colormap displayed in a colorbar (#1461) _Thanks @obnews_
* Ticks: Added option to invert tick mark direction (#1489, #1475) _Thanks @wangyexiang_
* FormsPlot: Improved support for WinForms 6 (#1430, #1483) _Thanks @SuperDaveOsbourne_
* Axes: Fixed bug where `AxisAuto()` failed to adjust all axes in multi-axis plots (#1497) _Thanks @Niravk1997_
* Radial Gauge Plot: Fixed bug affecting rendering of extremely small gauge angles (#1492, #1474) _Thanks @arthurits_
* Text plot and arrow plot: Now have `PixelOffsetX` and `PixelOffsetY` to facilitate small adjustments at render time (#1392)
* Image: New `Scale` property allows customization of image size (#1406)
* Axis: `Plot.GetDataLimits()` returns the boundaries of all data from all visible plottables regardless of the current axis limits (#1415) _Thanks @EFeru_
* Rendering: Improved support for scaled plots when passing scale as a `Plot.Render()` argument (#1416) _Thanks @Andreas_
* Text: Improved support for rotated text and background fills using custom alignments (#1417, #1516) _Thanks @riquich and @AndXaf_
* Text: Added options for custom borders (#1417, #1516) _Thanks @AndXaf and @MachineFossil_
* Plot: New `RemoveAxis()` method allows users to remove axes placed by `AddAxis()` (#1458) _Thanks @gobikulandaisamy_
* Benchmark: `Plot.BenchmarkTimes()` now returns an array of recent frame render times (#1493, #1491) _Thanks @anose001_
* Ticks: Disabling log-scaled minor ticks now disables tick label integer rounding (#1419) _Thanks @at2software_
* Rendering: Improve appearance of text by defaulting to ClearType font rendering (#1496, #823) _Thanks @Elgot_

## ScottPlot 4.1.27
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2021-10-24_
* Colorbar: Exposed fields for additional tick line and tick label customization (#1360) _Thanks @Maoyao233_
* Plot: Improved `AxisAutoY()` margins (#1363) _Thanks @Maoyao233_
* Radar Plot: `LineWidth` may now be customized (#1277, #1369) _Thanks @bclehmann_
* Controls: Stretching due to display scaling can be disabled with `Configuration.DpiStretch` in WPF and Avalonia controls (#1352, #1364) _Thanks @ktheijs and @bclehmann_
* Axes: Improved support for log-distributed minor tick and grid lines (#1386, #1393) _Thanks @at2software_
* Axes: `GetTicks()` can be used to get the tick positions and labels from the previous render
* WPF Control: Improved responsiveness while dragging with the mouse to pan or zoom (#1387, #1388) _Thanks @jbuckmccready_
* Layout: `MatchLayout()` has improved alignment for plots containing colorbars (#1338, #1349, #1351) _Thanks @dhgigisoave_
* Axes: Added multi-axis support for `SetInnerViewLimits()` and `SetOuterViewLimits()` (#1357, #1361) _Thanks @saroldhand_
* Axes: Created simplified overloads for `AxisAuto()` and `Margins()` that lack multi-axis arguments (#1367) _Thanks @cdytoby_
* Signal Plot: `FillAbove()`, `FillBelow()`, and `FillAboveAndBelow()` methods have been added to simplify configuration and reduce run-time errors. Direct access to fill-related fields has been deprecated. (#1401)
* Plot: `AddFill()` now has an overload to fill between two Y curves with shared X values
* Palette: Made all `Palette` classes public (#1394) _Thanks @Terebi42_
* Colorbar: Added `AutomaticTicks()` to let the user further customize tick positions and labels (#1403, #1362) _Thanks @bclehmann_
* Heatmap: Improved support for automatic tick placement in colorbars (#1403, #1362)
* Heatmap: Added `XMin`, `XMax`, `YMin`, and `YMax` to help configure placement and edge alignment (#1405) _Thanks @bclehmann_
* Coordinated Heatmap: This plot type has been deprecated now that the special functionality it provided is present in the standard `Heatmap` (#1405)
* Marker: Created a new `Marker` class to simplify the marker API. Currently it is a pass-through for `MarkerShape` enumeration members.
* Plot: `AddMarker()` makes it easy to place a styled marker at an X/Y position on the plot. (#1391)
* Plottable: `AddPoint()` now returns a `MarkerPlot` rather than a `ScatterPlot` with a single point (#1407)
* Axis lines: Added `Min` and `Max` properties to terminate the line at a finite point (#1390, #1399) _Thanks @bclehmann_

## ScottPlot 4.1.26
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2021-10-12_
* SignalPlotYX: Improve support for step display (#1342) _Thanks @EFeru_
* Heatmap: Improve automatic axis limit detection (#1278) _Thanks @bclehmann_
* Plot: Added `Margins()` to set default margins to use when `AxisAuto()` is called without arguments (#1345)
* Heatmap: Deprecated `ShowAxisLabels` in favor of tight margins (see cookbook) (#1278) _Thanks @bclehmann_
* Histogram: Fixed bug affecting binning of values at the upper edge of the final bin (#1348, #1350) _Thanks @jw-suh_
* NuGet: Packages have improved debug experience with SourceLink and snupkg format symbols (#1285)

## ScottPlot 4.1.25
* Palette: `ScottPlot.Palette` has been created and cookbook recipes have been updated to use it. The module it replaces (`ScottPlot.Drawing.Palette`) will not be marked obsolete until ScottPlot 5. (#1299, #1304)
* Style: Refactored to use static classes instead of enumeration members (#1299, #1291)
* NuGet: Improved System.Drawing.Common dependencies in user control packages (#1311, #1310) _Thanks @Kritner_
* Avalonia Control: Now targets .NET 5 (#1306, #1309) _Thanks @bclehmann_
* Plot: Fixed bug causing `GetPixel()` to return incorrect values for some axes (#1329, #1330) _Thanks @riquich_
* New Palettes:
  * `ColorblindFriendly` modeled after [Wong 2011](https://www.nature.com/articles/nmeth.1618.pdf) (#1312) _Thanks @arthurits_
  * `Dark` (#1313) _Thanks @arthurits_
  * `DarkPastel` (#1314) _Thanks @arthurits_
  * `Redness` (#1322) _Thanks @wbalbo_
  * `SummerSplash (#1317)` _Thanks @KanishkKhurana_
  * `Tsitsulin` 25-color optimal qualitative palette ([described here](http://tsitsul.in/blog/coloropt)) by [Anton Tsitsulin](http://tsitsul.in) (#1318) _Thanks @arthurits and @xgfs_
* New Styles:
  * `Burgundy` (#1319) _Thanks @arthurits_
  * `Earth` (#1320) _Thanks @martinkleppe_
  * `Pink` (#1234) _Thanks @nanrod_

## ScottPlot 4.1.23
* NuGet: use deterministic builds, add source link support, and include compiler flags (#1285)

## ScottPlot 4.1.22
* Coxcomb Plots: Added support for image labels (#1265, #1275) _Thanks @Rayffer_
* Palette: Added overloads for `GetColor()` and `GetColors()` to support transparency
* Plot Viewer: fixed bug causing render warning to appear in WinForms and Avalonia plot viewers (#1265, #1238) _Thanks @bukkideme, @Nexus452, and @bclehmann_

## ScottPlot 4.1.21
* Legend: Throw an exception if `RenderLegend()` is called on a plot with no labeled plottables (#1257)
* Radar: Improved support for category labels. (#1261, #1262) _Thanks @Rayffer_
* Controls: Now have a `Refresh()` method as an alias of `Render()` for manually redrawing the plot and updating the image on the screen. Using `Render()` in user controls is more similar to similar plotting libraries and less likely to be confused with `Plot.Render()` in documentation and warning messages. (#1264, #1270, #1263, #1245, #1165)
* Controls: Decreased visibility of the render warning (introduced in ScottPlot 4.1.19) by allowing it only to appear when the debugger is attached (#1165, #1264)
* Radial Gaugue Plot: Fixed divide-by-zero bug affecting normalized gauges (#1272) _Thanks @arthurits_

## ScottPlot 4.1.20
* Ticks: Fixed bug where corner labels would not render when multiplier or offset notation is in use (#1252, #1253) _Thanks @DavidBergstromSWE_

## ScottPlot 4.1.19
* Controls: Fixed bug where render warning message is not hidden if `RenderRequest()` is called (#1165) _Thanks @gigios_

## ScottPlot 4.1.18
* Ticks: Improve placement when axis scale lock is enabled (#1229, #1197)
* Plot: `SetViewLimits()` replaced by `SetOuterViewLimits()` and `SetInnerViewLimits()` (#1197) _Thanks @noob765_
* Plot: `EqualScaleMode` (an enumeration accepted by `AxisScaleLock()`) now has `PreserveSmallest` and `PreserveLargest` members to indicate which axis to prioritize when adjusting zoom level. The new default is `PreserveSmallest` which prevents data from falling off the edge of the plot when resizing. (#1197) _Thanks @noob765_
* Axis: Improved alignment of 90º rotated ticks (#1194, #1201) _Thanks @gigios_
* Controls: Fix bug where middle-click-drag zoom rectangle would persist if combined with scroll wheel events (#1226) _Thanks @Elgot_
* Scatter Plot: Fixed bug affecting plots where `YError` is set but `XError` is not (#1237, #1238) _Thanks @simmdan_
* Palette: Added `Microcharts` colorset (#1235) _Thanks @arthurits_
* SignalPlotXY: Added support for `FillType` (#1232) _Thanks @ddrrrr_
* Arrow: New plot type for rendering arrows on plots. Arrowhead functionality of scatter plots has been deprecated. (#1241, #1240)
* Controls: Automatic rendering has been deprecated. Users must call Render() manually at least once. (#1165, #1117)
* Radial Gauge Plots: `AddRadialGauge()` now adds a radial gauge plot (a new circular plot type where values are represented as arcs spanning a curve). See cookbook for examples and documentation. (#1242) _Thanks @arthurits_

## ScottPlot 4.1.17
* Improved `RadarPlot.Update()` default arguments (#1097) _Thanks @arthurits_
* Radar Plot: Improved `Update()` default arguments (#1097) _Thanks @arthurits_
* Crosshair: Added `XLabelOnTop` and `YLabelOnRight` options to improve multi-axis support and label customization (#1147) _Thanks @rutkowskit_
* Signal Plot: Added `StepDisplay` option to render signal plots as step plots when zoomed in (#1092, #1128) _Thanks @EFeru_
* Testing: Improved error reporting on failed XML documentation tests (#1127) _Thanks @StendProg_
* Histogram: Marked `ScottPlot.Statistics.Histogram` obsolete in favor of static methods in `ScottPlot.Statistics.Common` designed to create histograms and probability function curves (#1051, #1166). See cookbook for usage examples. _Thanks @breakwinz and @bclehmann_
* WpfPlot: Improve memory management for dynamically created and destroyed WpfPlot controls by properly unloading the dispatcher timer (#1115, #1117) _Thanks @RamsayGit, @bclehmann, @StendProg, and @Orace_
* Mouse Processing: Improved bug that affected fast drag-dropping of draggable objects (#1076)
* Rendering: Fixed clipping bug that caused some plot types to be rendered above data area frames (#1084)
* Plot: Added `Width` and `Height` properties
* Plot: `GetImageBytes()` now returns bytes for a PNG file for easier storage in cloud applications (#1107)
* Axis: Added a `GetSettings()` method for developers, testers, and experimenters to gain access to experimental objects which are normally private for extreme customization
* Axis: Axis ticks now have a `Ticks()` overload which allows selective control over major tick lines and major tick labels separately (#1118) _Thanks @kegesch_
* Plot: `AxisAuto()` now has `xAxisIndex` and `yAxisIndex` arguments to selectively adjust axes to fit data on a specified index (#1123)
* Crosshair: Refactored to use two `AxisLine`s so custom formatters can now be used and lines can be independently styled (#1173, #1172, #1122, 1195) _Thanks @Maoyao233 and @EFeru_
* ClevelandDotPlot: Improve automatic axis limit detection (#1185) _Thanks @Nextra_
* ScatterPlotList: Improved legend formatting (#1190) _Thanks @Maoyao233_
* Plot: Added an optional argument to `Frameless()` to reverse its behavior and deprecated `Frame()` (#1112, #1192) _Thanks @arthurits_
* AxisLine: Added `PositionLabel` option for displaying position as text (using a user-customizable formatter function) on the axis (#1122, #1195, #1172, #1173) _Thanks @EFeru and @Maoyao233_
* Radar Plot: Fixed rendering artifact that occurred when axis maximum is zero (#1139) _Thanks @petersesztak and @bclehmann_
* Mouse Processing: Improved panning behavior when view limits (axis boundaries) are active (#1148, #1203) _Thanks @at2software_
* Signal Plot: Fixed bug causing render artifacts when using fill modes (#1163, #1205)
* Scatter Plot: Added support for `OffsetX` and `OffsetY` (#1164, #1213)
* Coxcomb: Added a new plot type for categorical data. See cookbook for examples. (#1188) _Thanks @bclehmann_
* Axes: Added `LockLimits()` to control pan/zoom manipulation so individual axes can be manipulated in multi-axis plots. See demo application for example. (#1179, #1210) _Thanks @kkaiser41_
* Vector Plot: Add additional options to customize arrowhead style and position. See cookbook for examples. (#1202) _Thanks @hhubschle_
* Finance Plot: Fixed bug affecting plots with no data points (#1200) _Thanks @Maoyao233_
* Ticks: Improve display of rotated ticks on secondary axes (#1201) _Thanks @gigios_

## ScottPlot 4.1.16
* Made it easier to use custom color palettes (see cookbook) (#1058, #1082) _Thanks @EFeru_
* Added a `IgnoreAxisAuto` field to axis lines and spans (#999) _Thanks @kirsan31_
* Heatmaps now have a `Smooth` field which uses bicubic interpolation to display smooth heatmaps (#1003) _Thanks @xichaoqiang_
* Radar plots now have an `Update()` method for updating data values without clearing the plot (#1086, #1091) _Thanks @arthurits_
* Controls now automatically render after the list of plottables is modified (previously it was after the number of plottables changed). This behavior can be disabled by setting a public field in the control's `Configuration` module. (#1087, #1088) _Thanks @bftrock_
* New `Crosshair` plot type draws lines to highlight a point on the plot and labels their coordinates in the axes (#999, #1093) _Thanks @kirsan31_
* Added support for a custom `Func<double, string>` to be used as custom tick label formatters (see cookbook) (#926, #1070) _Thanks @damiandixon and @ssalsinha_
* Added `Move`, `MoveFirst`, and `MoveLast` to the `Plot` module for added control over which plottables appear on top (#1090) _Thanks @EFeru_
* Fixed bug preventing expected behavior when calling `AxisAutoX` and `AxisAutoY` (#1089) _Thanks @EFeru__

## ScottPlot 4.1.15
* Hide design-time error message component at run time to reduce flicking when resizing (#1073, #1075) _Thanks @Superberti and @bclehmann_
* Added a modern `Plot.GetBitmap()` overload suitable for the new stateless rendering system (#913 #1063)
* Controls now have `PlottableDragged` and `PlottableDropped` event handlers (#1072) _Thanks @JS-BGResearch_

## ScottPlot 4.1.14
* Add support for custom linestyles in SignalXY plots (#1017, #1016) _Thanks @StendProg and @breakwinz_
* Improved Avalonia dependency versioning (#1018, #1041) _Thanks @bclehmann_
* Controls now properly process `MouseEnter` and `MouseLeave` events (#999) _Thanks @kirsan31 and @breakwinz_
* Controls now have a `RenderRequest()` method that uses a render queue to facilitate non-blocking render calls (#813, #1034) _Thanks @StendProg_
* Added Last() to finance plots to make it easier to access the final OHLC (#1038) _Thanks @CalderWhite_
* Controls that fail to render in design mode now display the error message in a textbox to prevent Visual Studio exceptions (#1048) _Thanks @bclehmann_

## ScottPlot 4.1.13-beta
* `Plot.Render()` and `Plot.SaveFig()` now have a `scale` argument to allow for the creation of high resolution scaled plots (#983, #982, #981) _Thanks @PeterDavidson_
* A `BubblePlot` has been added to allow display of circles with custom colors and sizes. See cookbook for examples. (#984, #973, #960) _Thanks @PeterDavidson_
* Avalonia 0.10.3 is now supported (#986) _Thanks @bclehmann_
* Default version of System.Drawing.Common has been changed from `5.0.0` to `4.6.1` to minimize errors associated with downgrading (#1004, #1005, #993, #924, #655) _Thanks @bukkideme_

## ScottPlot 4.1.12-beta
* Added "Open in New Window" option to right-click menu (#958, #969) _Thanks @ademkaya and @bclehmann_
* User control `Configuration` module now has customizable scroll wheel zoom fraction (#940, #937) _Thanks @PassionateDeveloper86 and @StendProg_
* Added options to `Plot.AxisScaleLock()` to let the user define scaling behavior when the plot is resized (#933, #857) _Thanks @ricecakebear and @StendProg_
* Improved XML documentation for `DataGen` module (#903, #902) _Thanks @bclehmann_
* Fixed bug where tick labels would not render for axes with a single tick (#945, #828, #725, #925) _Thanks @saklanmazozgur and @audun_
* Added option to manually refine tick density (#828) _Thanks @ChrisAtVault and @bclehmann_
* Improved tick density calculations for DateTime axes (#725) _Thanks @bclehmann_
* Fixed SignalXY rendering artifact affecting the right edge of the plot (#929, #931) _Thanks @damiandixon and @StendProg_
* Improved line style customization for signal plots (#929, #931) _Thanks @damiandixon and @StendProg_
* Fixed bug where negative bar plots would default to red fill color (#968, #946) _Thanks @pietcoussens_
* Fixed bug where custom vertical margin was not respected when `AxisAuto()` was called with a middle-click (#943) _Thanks Andreas_
* Added a minimum distance the mouse must travel while click-dragging for the action to be considered a drag instead of a click (#962)
* Improved Histogram documentation and simplified access to probability curves (#930, #932, #971) _Thanks @LB767, @breakwinz, and @bclehmann_

## ScottPlot 4.1.11-beta
* FormsPlot mouse events are now properly forwarded to the base control (#892, #919) _Thanks @grabul_
* Prevent right-click menu from deploying after right-click-drag (#891, #917)
* Add offset support to SignalXY (#894, #890) _Thanks @StendProg_
* Eliminate rendering artifacts in SignalXY plots (#893, #889) _Thanks @StendProg and @grabul_
* Optimize cookbook generation and test execution (#901) _Thanks @bclehmann_

## ScottPlot 4.1.10-beta
* Fixed a bug where applying the Seabourn style modified axis frame and minor tick distribution (#866) _Thanks @oszymczak_
* Improved XML documentation and error reporting for getting legend bitmaps (#860) _Thanks @mzemljak_
* Fixed rendering bug affecting finance plots with thin borders (#837) _Thanks @AlgoExecutor_
* Improved argument names and XML docs for SMA and Bollinger band calculation methods (#830) _Thanks @ticool_
* Improved GetPointNearest support for generic signal plots (#809, #882, #886) _Thanks @StendProg, @at2software, and @mrradd_
* Added support for custom slice label colors in pie charts (#883, #844) _Thanks @bclehmann, @StendProg, and @Timothy343_
* Improved support for transparent heatmaps using nullable double arrays (#849, #852) _Thanks @bclehmann_
* Deprecated bar plot `IsHorizontal` and `IsVertical` in favor of an `Orientation` enumeration
* Deprecated bar plot `xs` and `ys` in favor of `positions` and `values` which are better orientation-agnostic names
* Added Lollipop and Cleveland plots as new types of bar plots (#842, #817) _Thanks @bclehmann_
* Fixed a bug where `Plot.AddBarGroups()` returned an array of nulls (#839) _Thanks @rhys-wootton_
* Fixed a bug affecting manual tick labels (#829) _Thanks @ohru131_
* Implemented an optional render queue to allow asynchronous rendering in user controls (#813) _Thanks @StendProg_

## ScottPlot 4.1.9-beta
* Improved support for negative DateTimes when using DateTime axis mode (#806, #807) _Thanks @StendProg and @at2software_
* Improved axis limit detection when using tooltips (#805, #811) _Thanks @bclehmann and @ChrisAtVault_
* Added `WickColor` field to candlestick plots (#803) _Thanks @bclehmann_
* Improved rendering of candlesticks that open and close at the same price (#803, #800) _Thanks @bclehmann and @AlgoExecutor_
* Improved rendering of SignalXY plots near the edge of the plot (#795) _Thanks @StendProg_
* new `AddScatterStep()` helper method creates a scatter plot with the step style (#808) _Thanks @KlaskSkovby_
* Marked `MultiPlot` obsolete
* Refactored `Colormap` module to use classes instead of reflection (#767, #773) _Thanks @StendProg_
* Refactored `OHLC` fields and finance plots to store `DateTime` and `TimeSpan` instead of `double` (#795)

## ScottPlot 4.1.8-beta
* Improved validation and error reporting for large heatmaps (#772) _Thanks @Matthias-C_
* Removed noisy console output in `ScatterPlotList` (#780) _Thanks @Scr0nch_
* Improved rendering bug in signal plots (#783, #788) _Thanks @AlgoExecutor and @StendProg_
* Fix bug that hid grid lines in frameless plots (#779)
* Improved appearance of marker-only scatter plots in the legend (#790) _Thanks @AlgoExecutor_
* `AddPoint()` now has a `label` argument to match `AddScatter()` (#787) _Thanks @AlgoExecutor_

## ScottPlot 4.1.7-beta
* Added support for image axis labels (#759, #446, #716) _Thanks @bclehmann_
* Added `MinRenderIndex` and `MaxRenderIndex` support to Scatter plots (#737, #763) _Thanks @StendProg_
* Improved display of horizontal manual axis tick labels (#724, #762) _Thanks @inqb and @Saklut_
* Added support for listing and retrieving colormaps by their names (#767, #773) _Thanks @StendProg_
* Enabled mouse pan and zoom for plots with infinitely small width and height (#768, #733, #764) _Thanks @saklanmazozgur_
* A descriptive exception is now thrown when attempting to create heatmaps of unsupported dimensions (#722) _Thanks @Matthias-C_

## ScottPlot 4.1.6-beta
* Fixed single point render bug in Signal plots (#744, #745) _Thanks @at2software and @StendProg_
* Improved display scaling support for WPF control (#721, #720) _Thanks @bclehmann_
* User control `OnAxesChanged` events now send the control itself as the sender object (#743, #756) _Thanks @at2software_
* Fixed configuration bug related to Alt + middle-click-drag-zoom (#741) _Thanks @JS-BGResearch and @bclehmann_
* Fixed render bug related to ALT + middle-click-drag zoom box (#742) _Thanks @bclehmann_
* Fixed render bug for extremely small plots (#735)
* Added a coordinated heatmap plot type (#707) _Thanks @StendProg_
* Improved appearance of heatmap edges (#713) _Thanks @StendProg_
* Improved design-time rendering of Windows Forms control
* Added and expanded XML documentation for Plot and Plottable classes
* Created a new cookbook website generator that combines reflection with XML documentation (#727, #738, #756)
* ScottPlot is now a reserved prefix on NuGet

## ScottPlot 4.1.5-beta
* Helper methods were added for creating scatter plots with just lines (`AddScatterLines()`) or just markers (`AddScatterPoints()`).
* Scatter and Signal plots have `GetPointNearest()` which now has a `xyRatio` argument to support identifying points near the cursor in pixel space (#709, #722) _Thanks @oszymczak, @StendProg, @bclehmann_
* Improved display of manual tick labels (#724) _Thanks @bclehmann_

## ScottPlot 4.1.4-beta
* User controls have been extensively redesigned (#683)
  * All user controls are almost entirely logic-free and pass events to `ScottPlot.Control`, a shared common back-end module which handles mouse interaction and pixel/coordinate conversions.
  * Controls no longer have a `Configure()` method with numerous named arguments, but instead a `Configuration` field with XML-documented public fields to customize behavior.
  * Renders occur automatically when the number of plottables changes, meaning you do not have to manually call `Render()` when plotting data for the first time. This behavior can be disabled in the configuration.
  * Avalonia 0.10.0 is now supported and uses this new back-end (#656, #700) _Thanks @bclehmann_
  * Events are used to provide custom right-click menu actions.
  * The right-click plot settings window (that was only available from the WinForms control) has been removed.
* New methods were added to `ScottPlot.Statistics.Common` which efficiently find the Nth smallest number, quartiles, or other quantiles from arrays of numbers (#690) _Thanks @bclehmann_
* New tooltip plot type (#696) _Thanks @bclehmann_
* Fixed simple moving average (SMA) calculation (#703) _Thanks @Saklut_
* Improved multi-axis rendering (#706) _Thanks @bclehmann_
* Improved `SetSourceAsync()` for segmented trees (#705, #692) _Thanks @jl0pd and @StendProg_
* Improved layout for axes with rotated ticks (#706, #699) _Thanks @MisterRedactus and @bclehmann_
* ScottPlot now multi-targets more platforms and supports the latest C# language version on modern platforms but restricts the language to C# 7.3 for .NET Framework projects (#691, #711) _Thanks @jl0pd_
* Improved project file to install `System.ValueTuple` when targeting .NET Framework 4.6.1 (#88, #691)

## ScottPlot 4.1.3-beta
* Scott will make a document to summarize 4.0 → 4.1 changes as we get closer to a non-beta release
* Fixed rendering bug affecting axis spans when zoomed far in (#662) _Thanks @StendProg_
* Improved Gaussian blur performance (#667) _Thanks @bclehmann_
* Largely refactored heatmaps (#679, #680) _Thanks @bclehmann_
* New `Colorbar` plot type (#681)
* Improved SMA and Bollinger band generators (#647) _Thanks @Saklut_
* Improved tick label rounding (#657)
* Improved setting of tick label color (#672)
* Improved fill above and below for scatter plots (#676) _Thanks @MithrilMan_
* Additional customizations for radar charts (#634, #628, #635) _Thanks @bclehmann and @SommerEngineering_

## ScottPlot 4.1.0

In November, 2020 ScottPlot 4.0 branched into a permanent `stable` branch, and ScottPlot 4.1 began development as beta / pre-release in the main branch. ScottPlot 4.0 continues to be maintained, but modifications are aimed at small bugfixes rather than large refactoring or the addition of new features. ScottPlot 4.1 merged into the master branch in November, 2020 (#605). Improvements are focused at enhanced performance, improved thread safety, support for multiple axes, and options for data validation.

* **Most plotting methods are unchanged so many users will not experience any breaking changes.**
* **Axis Limits**
  * Axis limits are described by a `AxisLimits` struct (previously `double[]` was used)
  * Methods which modify axis limits do not return anything (previously they returned `double[]`)
  * To get the latest axis limits call `Plot.AxisLimits()` which returns a `AxisLimits` object
* **Multiple Axes**
  * Multiple axes are now supported! There is no change to the traditional workflow if this feature is not used.
  * Most axis methods accept a `xAxisIndex` and `yAxisIndex` arguments to specify which axes they will modify or return
  * Most plottable objects have `xAxisIndex` and `yAxisIndex` fields which specify which axes they will render on
  * You can enable a second Y and X axis by calling `YLabel2` and `XLabel2()`
  * You can obtain an axis by calling `GetXAxis(xAxisIndex)` or `GetYAxis(yAxisIndex)`, then modify its public fields to customize its behavior
  * The default axes (left and bottom) both use axis index `0`
  * The secondary axes (right and top) both use axis index `1`
  * You can create additional axes by calling `Plot.AddAxis()` and customize it by modifying fields of the `Axis` it returns.
* **Layout**
  * The layout is re-calculated on every render, so it automatically adjusts to accommodate axis labels and ticks.
  * To achieve extra space around the data area, call `Layout()` to supply a minimum size for each axis.
  * To achieve a frameless plot where the data area fills the full figure, call `LayoutFrameless()`
* **Some namespaces and class names have changed**
  * The `Plottable` base class has been replaced with an `IPlottable` interface
  * Plottables have been renamed and moved into a `Plottable` namespace (e.g., `PlottableScatter` is  now `Plottable.ScatterPlot`)
  * Several enums have been renamed
* **The Settings module has been greatly refactored**
  * It is still private, but you can request it with `Plot.GetSettings()`
  * Many of its objects implement `IRenderable`, so their customization options are stored at the same level as their render methods.
* **The Render system is now stateless**
  * `Bitmap` objects are never stored. The `Render()` method will create and return a new `Bitmap` when called, or will render onto an existing `Bitmap` if it is supplied as an argument. This allows controls to manage their own performance optimization by optionally re-using a `Bitmap` for multiple renders.
  * Drawing is achieved with `using` statements which respect all `IDisposable` drawing objects, improving thread safety and garbage collection performance.

## ScottPlot 4.0.46
* Improved ticks for small plots (#724) _Thanks @Saklut_
* Improved display of manual ticks (#724) _Thanks @bclehmann_

## ScottPlot 4.0.45
* Fixed a bug that affected very small plots with the benchmark enabled (#626) _Thanks @martin-brajer_
* Improved labels in bar graphs using a yOffset (#584) _Thanks Terbaco_
* Added `RenderLock()` and `RenderUnlock()` to the Plot module to facilitate multi-threaded plot modification (#609) _Thanks @ZTaiIT1025_

## ScottPlot 4.0.44
* Improved limits for fixed-size axis spans (#586) _Thanks @Ichibot200 and @StendProg_
* Mouse drag/drop events now send useful event arguments (#593) _Thanks @charlescao460 and @StendProg_
* Fixed a bug that affected plots with extremely small (<1E-10) axis spans (#607) _Thanks @RFIsoft_
* `Plot.SaveFig()` now returns the full path to the file it created (#608)
* Fixed `AxisAuto()` bug affecting signal plots using min/max render indexes with a custom sample rate (#621) _Thanks @LB767_
* Fixed a bug affecting histogram normalization (#624) _Thanks @LB767_
* WPF and Windows Forms user controls now also target .NET 5

## ScottPlot 4.0.43
* Improved appearance of semi-transparent legend items (#567)
* Improved tick labels for ticks smaller than 1E-5 (#568) _Thanks @ozgur640_
* Improved support for Avalonia 0.10 (#571) _Thanks @bclehmann and @apkrymov_
* Improved positions for base16 ticks (#582, #581) _Thanks @bclehmann_

## ScottPlot 4.0.42
* Improved DPI scaling support when using WinForms in .NET Core applications (#563) _Thanks @Ichibot200_
* Improved DPI scaling support for draggable axis lines and spans (#563) _Thanks @Ichibot200_

## ScottPlot 4.0.41
* Improved density of DateTime ticks (#564, #561) _Thanks @StendProg and @waynetheron_
* Improved display of DateTime tick labels containing multiple spaces (#539, #564) _Thanks @StendProg_

## ScottPlot 4.0.40
* Added user control for Avalonia (#496, #503) _Thanks @bclehmann_
* Holding shift while left-click-dragging the edge of a span moves it instead of resizing it (#509) _Thanks @Torgano_
* CSV export is now culture invariant for improved support on systems where commas are decimal separators (#512) _Thanks Daniel_
* Added fill support to scatter plots (#529) _Thanks @AlexFsmn_
* Fix bug that occurred when calling `GetLegendBitmap()` before the plot was rendered (#527) _Thanks @el-aasi_
* Improved DateTime tick placement and added support for milliseconds (#539) _Thanks @StendProg_
* Pie charts now have an optional hollow center to produce donut plots (#534) _Thanks @bclehmann and @AlexFsmn_
* Added electrocardiogram (ECG) simulator to the DataGen module (#540) _Thanks @AteCoder_
* Improved mouse scroll wheel responsiveness by delaying high quality render (#545, #543, #550) _Thanks @StendProg_
* `Plot.PlotBitmap()` allows Bitmaps to be placed at specific coordinates (#528) _Thanks @AlexFsmn_
* `DataGen.SampleImage()` returns a sample Bitmap that can be used for testing
* Bar graphs now have a hatchStyle property to customize fill pattern (#555) _Thanks @bclehmann_
* Support timecode tick labels (#537) _Thanks @vrdriver and @StendProg_

## ScottPlot 4.0.39
* Legend now reflects LineStyle of Signal and SignalXY plots (#488) _Thanks @bclehmann_
* Improved mouse wheel zoom-to-cursor and middle-click-drag rectangle zoom in the WPF control for systems that use display scaling (#490) _Thanks @nashilnik_
* The `Configure()` method of user controls now has a `lowQualityAlways` argument to let the user easily enable/disable anti-aliasing at the control level. Previously this was only configurable by reaching into the control's plot object and calling its `AntiAlias()` method. (#499) _Thanks @RachamimYaakobov_
* SignalXY now supports parallel processing (#500) _Thanks @StendProg_
* SignalXY now respects index-based render limits (#493, #500) _Thanks @StendProg and @envine_

## ScottPlot 4.0.38
* Improved `Plot.PlotFillAboveBelow()` rendering of data with a non-zero baseline (#477) _Thanks @el-aasi_
* Added `Plot.PlotWaterfall()` for easy creation of waterfall-style bar plots (#463, #476) _Thanks @bclehmann_
* Axis tick labels can be displayed using notations other than base 10 by supplying `Plot.Ticks()` with `base` and `prefix` arguments, allowing axes that display binary (e.g., `0b100110`) or hexadecimal (eg., `0x4B0D10`) tick labels (#469, #457) _Thanks @bclehmann_
* Added options to `PlotBar()` to facilitate customization of text displayed above bars when `showValue` is enabled (#483) _Thanks @WillemWever_
* Plot objects are colored based on a pre-defined set of colors. The default colorset (category10) is the same palette of colors used by matplotlib. A new `Colorset` module has been created to better define this behavior, and `Plot.Colorset()` makes it easy to plot data using alternative colorsets. (#481)
* Fixed a bug that caused instability when a population plot is zoomed-out so much that its fractional distribution curve is smaller than a single pixel (#480) _Thanks @HowardWhile_
* Added `Plot.Remove()` method to make it easier to specifically remove an individual plottable after it has been plotted. `Plot.Clear()` is similar, but designed to remove classes of plot types rather than a specific plot object. (#479) _Thanks @cstyx and @Resonanz_
* Signal plots can now be created with a defined `minRenderIndex` (in addition to the already-supported `maxRenderIndex`) to facilitate partial display of large arrays (#474) _Thanks @bclehmann_

## ScottPlot 4.0.37
* Fixed a long-running issue related to strong assembly versioning that caused the WPF control to fail to render in the Visual Studio designer in .NET Framework (but not .NET Core) projects (#473, #466, #356) _Thanks @bhairav-thakkar, @riquich, @Helitune-RobMcKay, and @iu2kxv_
* User controls now also target `net472` (while still supporting `net461` and `netcoreapp3.0`) to produce a build folder with just 3 DLLs (compared to over 100 when building with .NET Framework 4.6.1)

## ScottPlot 4.0.36
* `PlotSignal()` and `PlotSignalXY()` plots now have an optional `useParallel` argument (and public property on the objects they return) to allow the user to decide whether parallel or sequential calculations will be performed. (#454, #419, #245, #72) _Thanks @StendProg_
* Improved minor tick alignment to prevent rare single-pixel artifacts (#417)
* Improved horizontal axis tick label positions in ruler mode (#453)
* Added a `Statistics.Interpolation` module to generate smooth interpolated splines from a small number of input data points. See advanced statistics cookbook example for usage information. (#459) _Thanks Hans-Peter Moser_
* Improved automatic axis adjustment when adding bar plots with negative values (#461, #462) _Thanks @bclehmann_
* Created `Drawing.Colormaps` module which has over a dozen colormaps for easily converting a fractional value to a color for use in plotting or heatmap displays (#457, #458) _Thanks @bclehmann_
* Updated `Plot.Clear()` to accept any `Plottable` as an argument, and all `Plottable` objects of the same type will be cleared (#464) _Thanks @imka-code_

## ScottPlot 4.0.35
* Added `processEvents` argument to `formsPlot2.Render()` to provide a performance enhancement when linking axes of two `FormsPlot` controls together (by calling `Plot.MatchAxis()` from the control's `AxesChanged` event, as seen in the _Linked Axes_ demo application) (#451, #452) _Thanks @StendProg and @robokamran_
* New `Plot.PlotVectorField()` method for displaying vector fields (sometimes called quiver plots) (#438, #439, #440) _Thanks @bclehmann and @hhubschle_
* Included an experimental colormap module which is likely to evolve over subsequent releases (#420, #424, #442) _Thanks @bclehmann_
* `PlotScatterHighlight()` was created as a type of scatter plot designed specifically for applications where "show value on hover" functionality is desired. Examples are both in the cookbook and WinForms and WPF demo applications. (#415, #414) _Thanks @bclehmann and @StendProg_
* `PlotRadar()` is a new plot type for creating Radar plots (also called spider plots or star plots). See cookbook and demo application for examples. (#428, #430) _Thanks @bclehmann_
* `PlotPlolygons()` is a new performance-optimized variant of `PlotPolygon()` designed for displaying large numbers of complex shapes (#426) _Thanks @StendProg_
* The WinForms control's `Configure()` now has a `showCoordinatesTooltip` argument to continuously display the position at the tip of the cursor as a tooltip (#410) _Thanks @jcbeppler_
* User controls now use SHIFT (previously ALT) to lock the horizontal axis and ALT (previously SHIFT) while left-click-dragging for zoom-to-region. Holding CTRL+SHIFT while right-click-dragging now zooms evenly, without X/Y distortion. (#436) _Thanks @tomwimmenhove and @StendProg_
* Parallel processing is now enabled by default. Performance improvements will be most noticeable on Signal plots. (#419, #245, #72)
* `Plot.PlotBar()` now has an `autoAxis` argument (which defaults `true`) that automatically adjusts the axis limits so the base of the bar graphs touch the edge of the plot area. (#406)
* OSX-specific DLLs are now only retrieved by NuGet on OSX (#433, #211, #212)
* Pie charts can now be made with `plt.PlotPie()`. See cookbook and demo application for examples. (#421, #423) _Thanks @bclehmann_
* `ScottPlot.FormsPlotViewer(Plot)` no longer resets the new window's plot to the default style (#416)  _Thanks @StendProg_
* Controls now have a `recalculateLayoutOnMouseUp` option to prevent resetting of manually-defined data area padding

## ScottPlot 4.0.34
* Improve display of `PlotSignalXY()` by not rendering markers when zoomed very far out (#402) _Thanks @gobikulandaisamy_
* Optimized rendering of solid lines which have a user-definable `LineStyle` property. This modification improves grid line rendering and increases performance for most types of plots. (#401, #327) _Thanks @bukkideme and @Ichibot200_

## ScottPlot 4.0.33
* Force grid lines to always draw using anti-aliasing. This compensates for a bug in `System.Drawing` that may cause diagonal line artifacts to appear when the user controls were panned or zoomed. (#401, #327) _Thanks @bukkideme and @Ichibot200_

## ScottPlot 4.0.32
* User controls now have a `GetMouseCoordinates()` method which returns the DPI-aware position of the mouse in graph coordinates (#379, #380) _Thanks @bclehmann_
* Default grid color was lightened in the user controls to match the default style (#372)
* New `PlotSignalXY()` method for high-speed rendering of signal data that has unevenly-spaced X coordinates (#374, #375) _Thanks @StendProg and @LogDogg_
* Modify `Tools.Log10()` to return `0` instead of `NaN`, improving automatic axis limit detection (#376, #377) _Thanks @bclehmann_
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
* Created `Plot.PlotBarGroups()` for easier construction of grouped bar plots from 2D data (#367) _Thanks @bclehmann_
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
* Fixed a marker/line alignment issue that only affeced low-density Signal plots on Linux and MacOS (#340) _Thanks @SeidChr_
* WPF control now appears in Toolbox (#151) _Thanks @RalphLAtGitHub_
* Plot titles are now center-aligned with the data area, not the figure. This improves the look of small plots with titles. (#365) _Thanks @Resonanz_
* Fixed bug that ignored `Configure(enableRightClickMenu: false)` in WPF and WinForms user controls. (#365) _Thanks @thunderstatic_
* Updated `Configure(enableScrollWheelZoom: false)` to disable middle-click-drag zooming. (#365) _Thanks @eduhza_
* Added color mixing methods to ScottPlot.Drawing.GDI (#361)
* Middle-click-drag zooming now respects locked axes (#353) _Thanks @LogDogg_
* Improved user control zooming of high-precision DateTime axis data (#351) _Thanks @bukkideme_
* Plot.AxisBounds() now lets user set absolute bounds for drag and pan operations (#349) _Thanks @LogDogg_
* WPF control uses improved Bitmap conversion method (#350)
* Function plots have improved handling of functions with infinite values (#370) _Thanks @bclehmann_

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
* Interactive plot viewers were created to make it easy to interactively display data in a pop-up window without having to write any GUI code: `ScottPlot.WpfPlotViewer` for WPF and `ScottPlot.FormsPlotViewer` for Windows Forms
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
* Added `rSquared` property to linear regression fits (#290) _Thanks @bclehmann and @StendProg_
* Added `Tools.ConvertPolarCoordinates()` to make it easier to display polar data on ScottPlot's Cartesian axes (#298) _Thanks @bclehmann_
* Improved `Plot.Function()` (#243) _Thanks @bclehmann_
* Added overload for `Plot.SetCulture()` to let the user define number and date formatting rather than relying on pre-made cultures (#301, #236) _Thanks @SeidChr_

## ScottPlot 4.0.19
* Improved how markers are drawn in Signal and SignalConst plots at the transition area between zoomed out and zoomed in (#263) _Thanks @bukkideme and @StendProg_
* Improved support for zero lineSize and markerSize in Signal and SignalConst plots (#263, #264) _Thanks @bukkideme and @StendProg_
* Improved thread safety of interactive graphs (#245) _Thanks @StendProg_
* Added `CoordinateFromPixelX()` and `CoordinateFromPixelY()` to get _double precision_ coordinates from a pixel location. Previously only SizeF (float) precision was available. This improvement is especially useful when using DateTime axes. (#269) _Thanks Chris_
* Added `AxisScale()` to adjust axis limits to set a defined scale (units per pixel) for each axis.
* Added `AxisEqual()` to adjust axis limits to set the scale of both axes to be the same regardless of the size of each axis (#272) _Thanks @gberrante_
* `PlotHSpan()` and `PlotVSpan()` now return `PlottableHSpan` and `PlottableVSpan` objects (instead of a `PlottableAxSpan` with a `vertical` property)
* `PlotHLine()` and `PlotVLine()` now return `PlottableHLine` and `PlottableVLine` objects (instead of a `PlottableAxLine` with a `vertical` property)
* MultiPlot now has a `GetSubplot()` method which returns the Plot from a row and column index (#242) _Thanks @Resonanz and @StendProg_
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
* Improved support for MacOS and Linux (#211, #212, #216) _Thanks @hexxone and @StendProg_
* Fixed a bug affecting the `ySpacing` argument in `Plot.Grid()` (#221) _@Thanks teejay-87_
* Enabled `visible` argument in `Title()`, `XLabel()`, and `YLabel()` (#222) _Thanks @ckovamees_
* AxisSpan: Edges are now optionally draggable (#228) _Thanks @StendProg_
* AxisSpan: Can now be selectively removed with `Clear()` argument
* AxisSpan: Fixed bug caused by zooming far into an axis span (#226) _Thanks @StendProg_
* WinForms control: now supports draggable axis lines and axis spans
* WinForms control: Right-click menu now has "copy image" option (#220)
* WinForms control: Settings screen now has "copy CSV" button to export data (#220)
* WPF control: now supports draggable axis lines and axis spans
* WPF control: Configure() to set various WPF control options
* Improved axis handling, expansion, and auto-axis (#219, #230) _Thanks @StendProg_
* Added more options to `DataGen.Cos()`
* Tick labels can be hidden with `Ticks()` argument (#223) _Thanks @ckovamees_

## ScottPlot 4.0.14
* Improved `MatchAxis()` and `MatchLayout()` (#217) _Thanks @ckovamees and @StendProg_

## ScottPlot 4.0.13
* Improved support for Linux and MacOS _Thanks @hexxone_
* Improved font validation (#211, #212) _Thanks @hexxone and @StendProg_

## ScottPlot 4.0.11
* User controls now have a `cursor` property which can be set to allow custom cursors. (#187) _Thanks @gobikulandaisamy_
* User controls now have a `mouseCoordinates` property which make it easy to get the X/Y location of the cursor. (#187) _Thanks @gobikulandaisamy_

## ScottPlot 4.0.10
* Improved density colormap (#192, #194) _Thanks @StendProg_
* Added linear regression tools and cookbook example (#198) _Thanks @bclehmann_
* Added `maxRenderIndex` to Signal to allow partial plotting of large arrays intended to be used with live, incoming data (#202) _Thanks @StendProg and @plumforest_
* Made _Shift + Left-click-drag_ zoom into a rectangle light middle-click-drag (in WinForms and WPF controls) to add support for mice with no middle button (#90) _Thanks @JagDTalcyon_
* Throw an exception if `SaveFig()` is called before the image is properly sized (#192) _Thanks @karimshams and @StendProg_
* `Ticks()` now has arguments for `FontName` and `FontSize` (#204) _Thanks Clay_
* Fixed a bug that caused poor layout due to incorrect title label size estimation (#205) _Thanks Clay_
* `Grid()` now has arguments to selectively enable/disable horizontal and vertical grid lines (#206) _Thanks Clay_
* Added tool and cookbook example to make it easier to plot data on a log axis (#207) _Thanks @senged_
* Arrows can be plotted using `plt.PlotArrow()` (#201) _Thanks Clay_

## ScottPlot 4.0.9
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-12-03_
* Use local regional display settings when formatting the month tick of DateTime axes. (#108) _Thanks @FadyDev2_
* Debug symbols are now packaged in the NuGet file

## ScottPlot 4.0.7
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-12-01_\
* Added WinForms support for .NET Framework 4.7.2 and 4.8
* Fixed bug in WinForms control that only affected .NET Core 3.0 applications (#189, #138) _Thanks @petarpetrovt_

## ScottPlot 4.0.6
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-29_\
* fixed bug that affected the settings dialog window in the WinForms control. (#187) _Thanks @gobikulandaisamy_

## ScottPlot 4.0.5
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-27_
* improved spacing for non-uniformly distributed OHLC and candlestick plots. (#184) _Thanks @Luvnet-890_
* added `fixedLineWidth` to `Legend()` to allow the user to control whether legend lines are dynamically sized. (#185) _Thanks @ab-tools_
* legend now hides lines or markers of they're hidden in the plottable
* DateTime axes now use local display format (#108) _Thanks @FadyDev2_

## ScottPlot 4.0.4
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-24_
* `PlotText()` now supports a background frame (#181) _Thanks @Luvnet-890_
* OHLC objects can be created with a double or a DateTime (#182) _Thanks @Minu476_
* Improved `AxisAuto()` fixes bug for mixed 2d and axis line plots

## ScottPlot 4.0.3
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-23_
* Fixed bug when plotting single-point candlestick (#172) _Thanks @Minu476_
* Improved style editing of plotted objects (#173) _Thanks @Minu476_
* Fixed pan/zoom axis lock when holding CTRL or ALT (#90) _Thanks @FadyDev2_
* Simplified the look of the user controls in designer mode
* Improved WPF control mouse tracking when using DPI scaling
* Added support for manual tick positions and labels (#174) _Thanks @Minu476_
* Improved tick system when using DateTime units (#108) _Thanks @Padanian, @FadyDev2, and @Bhandejiya_
* Created `Tools.DateTimesToDoubles(DateTime[] array)` to easily convert an array of dates to doubles which can be plotted with ScottPlot, then displayed as time using `plt.Ticks(dateTimeX: true)`.
* Added an inverted sign flag to allow display of an axis with descending units. (#177) _Thanks Bart_

## ScottPlot 4.0.2
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-09_
* Multi-plot figures: Images with several plots can be created using `ScottPlot.MultiPlot()`
* `ScottPlot.DataGen` functions which require a `Random` can accept null (they will create a `Random` if null is given)
* `plt.MatchAxis()` and `plt.MatchLayout()` have been improved
* `plt.PlotText()` now supports rotated text using the `rotation` argument. (#160) _Thanks @gwilson9_
* `ScottPlot.WinForms` user control has new events and `formsPlot1.Configure()` arguments to make it easy to replace the default functionality for double-clicking and deploying the right-click menu (#166). _Thanks @FadyDev2_
* All plottables now have a `visible` property which makes it easy to toggle visibility on/off after they've been plotted. _Thanks @Nasser_

## ScottPlot 4.0.1
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-11-03_
* ScottPlot now targets .NET Standard 2.0 so in addition to .NET Framework projects it can now be used in .NET Core applications, ASP projects, Xamarin apps, etc.
* The WinForms control has its own package which targets both .NET Framework 4.6.1 and .NET Core 3.0 _Thanks @petarpetrovt_
* The WPF control has its own package targeting .NET Core 3.0 _Thanks @petarpetrovt_
* Better layout system and control of padding _Thanks @Ichibot200_
* Added ruler mode to `plt.Ticks()` _Thanks @Ichibot200_
* `plt.MatchLayout()` no longer throws exceptions
* Eliminated `MouseTracker` class (tracking is now in user controls)
* Use NUnit (not MSTest) for tests

## ScottPlot 3.1.6
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-10-20_
* Reduced designer mode checks to increase render speed _Thanks @StendProg_
* Fixed cursor bug that occurred when draggable axis lines were used _Thanks @Kamran_
* Fully deleted the outdated `ScottPlotUC`
* Fixed infinite zoom bug caused by calling AxisAuto() when plotting a single point (or perfectly straight horizontal or vertical line)
* Added `ToolboxItem` and `DesignTimeVisible` delegates to WpfPlot control to try to get it to appear in the toolbox (but it doesn't seem to be working)
* Improved figure padding when axes frames are disabled _Thanks @Ichibot200_
* Improved rendering of ticks at the edge of the plottable area _Thanks @Ichibot200_
* Added `AxesChanged` event to user control to make it easier to sync axes between multiple plots
* Disabled drawing of arrows on user control in designer mode

## ScottPlot 3.1.5
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-10-06_
* WPF user control improved support for display scaling _Thanks @morningkyle_
* Fixed bug that crashed on extreme zoom-outs _Thanks @morningkyle_
* WPF user control improvements (middle-click autoaxis, scrollwheel zoom)
* ScottPlot user control has a new look in designer mode. Exceptions in user controls in designer mode can crash Visual Studio, so this risk is greatly reduced by not attempting to render a ScottPlot _inside_ Visual Studio.

## ScottPlot 3.1.4
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-09-22_
* middle-click-drag zooms into a rectangle drawn with the mouse
* Fixed bug that caused user control to crash Visual Studio on some systems that used DPI scaling. (#125, #111) _Thanks @ab-tools and @bukkideme_
* Fixed poor rendering for extremely small plots
* Fixed bug when making a scatter plot with a single point (#126). _Thanks @bonzaiferroni_
* Added more options to right-click settings menu (grid options, legend options, axis labels, editable plot labels, etc.)
* Improved axis padding and image tightening
* Greatly refactored the settings module (no change in functionality)

## ScottPlot 3.1.3
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-08-25_
* FormsPlot: middle-click-drag zooms into a rectangle
* FormsPlot: CTRL+scroll to lock vertical axis
* FormsPlot: ALT+scroll to loch horizontal axis
* FormsPlot: Improved (and overridable) right-click menu
* Ticks: rudimentary support for date tick labels (`dateTimeX` and `dateTimeY`)
* Ticks: options to customize notation (`useExponentialNotation`, `useOffsetNotation`, and `useMultiplierNotation`)

## ScottPlot 3.1.0
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-08-19_
* `ScottPlotUC` was renamed to `FormsPlot`
* `ScottPlotWPF` was renamed to `WpfPlot`
* The right-click menu has improved. It responds faster and has improved controls to adjust plot settings.
* Plots can now be saved in BMP, PNG, JPG, and TIF format
* Holding `CTRL` while click-dragging locks the horizontal axis
* Holding `ALT` while click-dragging locks the vertical axis
* Minor ticks are now displayed (and can be turned on or off with `Ticks()`)
* Legend can be accessed for external display with `GetLegendBitmap()`
* anti-aliasing is turned off while click-dragging to increase responsiveness (#93) _Thanks @StendProg_
* `PlotSignalConst` now has support for generics and improved performance using single-precision floating-point math. _Thanks @StendProg_
* Legend draws more reliably (#104, #106) _Thanks @StendProg_
* `AxisAuto()` now has `expandOnly` arguments
* Axis lines with custom lineStyles display properly in the legend

## ScottPlot 3.0.9
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-08-12_
* New Plot Type: `PlotSignalConst` for extremely large arrays of data which are not expected to change after being plotted. Plots generated with this method can be much faster than `PlotSignal`. (#70) _Thanks @StendProg_
* Greatly improved axis tick labels. Axis tick labels are now less likely to overlap with axis labels, and it displays very large and very small numbers well using exponential notation. (#47, #68) _Thanks @Padanian_
* Parallel processing support for `SignalPlot` (#72) _Thanks @StendProg_
* Every `Plot` function now returns a `Plottable`. When creating things like scatter plots, text, and axis lines, the returned object can now be used to update the data, position, styling, or call plot-type-specific methods.
* Right-click menu now displays ScottPlot and .NET Framework version
* Improved rendering of extremely zoomed-out signals 
* Rendering speed increased now that `Format32bppPArgb` is the default PixelFormat (#83) _Thanks @StendProg_
* `DataGen.NoisySin()` was added
* Code was tested in .NET Core 3.0 preview and compiled without error. Therefore, the next release will likely be for .NET Core 3.0 (#85, #86) _Thanks @petarpetrovt_
* User controls now render graphs with anti-alias mode off (faster) while the mouse is being dragged. Upon release a high quality render is performed.

## ScottPlot 3.0.8
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-08-04_
* WPF User Control: A ScottPlotWPF user control was created to allow provide a simple mouse-interactive ScottPlot control to WPF applications. It is not as full-featured as the winforms control (it lacks a right-click menu and click-and-drag functions), but it is simple to review the code (<100 lines) and easy to use.
* New plot type: `plt.AxisSpan()` shades a region of the graph (semi-transparency is supported)
* Ticks: Vertical ticks no longer overlap with vertical axis label (#47) _Thanks @bukkideme_
* Ticks: When axis tick labels contain very large or very small numbers, scientific notation mode is engaged
* Ticks: Horizontal tick mark spacing increased to prevent overlapping
* Ticks: Vertical tick mark spacing increased to be consistent with horizontal tick spacing
* Plottable objects now have a `SaveCSV(filename)` method. Scatter and Signal plot data can be saved from the user control through the right-click menu.
* Added `lineStyle` arguments to Scatter plots
* Improved legend: ability to set location, ability to set shadow direction, markers and lines are now rendered in the legend
* Improved ability to use custom fonts
* Segoe UI is now the default font for all plot components

## ScottPlot 3.0.7
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-07-27_
* New plot type: `plt.PlotStep()`
* New plot type `plt.PlotCandlestick()`
* New plot type `plt.PlotOHLC()`
* `plt.MatchPadding()` copies the data frame layout from one ScottPlot onto another (useful for making plots of matching size)
* `plt.MatchAxis()` copies the axes from one ScottPlot onto another (useful for making plots match one or both axis)
* `plt.Legend()` improvements: The `location` argument allows the user to place the legend at one of 9 different places on the plot. The `shadowDirection` argument allows the user to control if a shadow is shown and at what angle.
* Custom marker shapes can be specified using the `markerShape` argument.

## ScottPlot 3.0.6
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-06-30_
* Bar plot: The plot module now has a `Bar()` method that lets users create various types of bar plots
* Histogram: The new `ScottPlot.Histogram` class has tools to create and analyze histogram data (including cumulative probability)
* Step plot: Scatter plots can now render as step plots. Use this feature by setting the `stepDisplay` argument with `PlotScatter()`
* Manual grid spacing: Users can now manually define the grid density by setting the `xSpacing` and `ySpacing` arguments in `Grid()`
* Draggable axis lines: Axis lines can be dragged with the mouse if the `draggable` argument is set to `true` in `PlotHLine()` and `PlotHLine()`. Draggable axis line limits can also be set by defining additional arguments.
* Using the scrollwheel to zoom now zooms to the cursor position rather than the center of the plot area
* `ScottPlot.DataGen.RandomNormal()` was created to create arbitrary amounts of normally-distributed random data
* Fixed bug causing axis line color to appear incorrectly in the legend
* `AxisAuto()` is now called automatically on the first render. This means users no longer have to call this function manually for most applications. This simplifies quickstart programs to just: instantiate plot, plot data, render (now 3 lines in total instead of 4).
* Throw exceptions if scatter, bar, or signal data inputs are null (rather than failing later)

## ScottPlot 3.0.5
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-06-23_
* Improved pan and zoom performance

## ScottPlot 3.0.4
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-06-23_
* Bar graphs: New `plotBar()` method allow creation of bar graphs. By customizing the `barWidth` and `xOffset` arguments you can push bars together to create grouped bar graphs. Error bars can also be added with the `yError` argument.
* Scatter plots support X and Y error bars: `plotScatter()` now has arguments to allow X and Y error bars with adjustable error bar line width and cap size.
* Draggable axis lines: `plotHLine()` and `plotVLine()` now have a `draggable` argument which lets those axis lines be dragged around with the mouse (#11) _Thanks @plumforest_
* Fixed errors caused by resizing to 0px
* Fixed a capitalization inconsistency in the `plotSignal` argument list
* `axisAuto()` now includes positions of axis lines (previously they were ignored)
* Fixed an that caused SplitContainer splitters to freeze (#23) _Thanks @bukkideme_

## ScottPlot 3.0.3
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-05-29_
* Update NuGet package to depend on System.Drawing.Common

## ScottPlot 3.0.2
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-05-28_
* Changed target from .NET Framework 4.5 to 4.7.2 (#15) _Thanks @plumforest_

#### ScottPlot 3.0.1
_Published on [NuGet](https://www.nuget.org/packages?q=scottplot) on 2019-05-28_
* First version of ScottPlot published on NuGet