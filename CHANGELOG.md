## ScottPlot 5.0.36
_Not yet on NuGet..._
* Fonts: Made typeface caching thread-safe to improve support for multi-threaded environments (#3940) @Hawkwind250
* Ticks: Added a custom `LabelFormatter` to DateTime axes which use fixed intervals (#3936) @Fruchtzwerg94
* Fonts: Enabled sub-pixel text positioning for improved character placement (#3937) @bforlgreen
* Axes: Improved automatic axis limit expansion for extremely large numbers (#3930) @CodeDevAM
* Statistics: Added `ScottPlot.Statistics.Descriptive` methods `Median()` and `Percentile()`
* Population: Added a new Population plot type for displaying collections of values (#3944, #3676)

## ScottPlot 5.0.35
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-06-10_
* Legend: Added `Plot.ShowLegend()` overload that accepts an `Edge` for quickly adding a legend outside the data area (#3672, #3635)
* Radar Plot: New plot type (also called a spider charts or star charts) for representing multi-axis data as a 2D shape on a circular axis system (#3457, #3780) @bclehmann
* Coxcomb Plot: New plot type like a pie graph where the angle of slices is constant but the radii are not (#3457, #3780) @bclehmann
* Axes: Added `LabelFormatter` property to `DateTimeAutomatic` for custom formatting of DateTime tick labels (#3783) @loyvsc
* Rendering: Improve how backgrounds are drawn on on plots using a custom `ScaleFactor` (#3818) @MCF
* Plot: Added `Plot.Clear<T>()` as an alias for `Plot.Remove<T>()` to remove all plottables of the given type (#3820, #3804) @CoderPM2011
* Axes: Added `ScottPlot.AxisPanels.Experimental` namespace with examples in the demo app and cookbook (#3823) @EricEzaM
* Rendering: Added `Plot.RenderManager.RemoveAction<T>()` for easily removing specific actions from the render sequence
* SVG: Fixed issue where plots would have a black outline in some browsers (#3709) @sproott @KennyTK @aespitia
* Controls: Add "open in new window" to right-click menu for WinForms and WPF controls (#3730)
* Cookbook: Demonstrate how to achieve a frameless heatmap (#3828) @itsmygam3
* Cookbook: Demonstrate `Heatmap.CellAlignment` to achieve heatmaps that do not extend past their boundaries (#3806) @FengQingYangDad
* Signal: Improve support for datasets with no elements (#3797)
* Scatter: Improved line clipping when fill mode is enabled (#3792) @BendRocks @CoderPM2011
* Signal and Scatter: Added `MinRenderIndex` and `MaxRenderIndex` properties as shortcuts to those in the `Data` property (#3798)
* Scatter: Improve appearance when `FillY` is enabled and all data is on one side of `FillYValue` (#3791) @BendRocks
* Axes: Added `SetTicks()` shortcut for quickly switching to a manual tick generator pre-loaded with the given tick positions and labels (#3831) @Giviruk
* Legend: Clip the legend area so it does not flow outside the data area on extremely small plots (#3833) @drolevar
* Controls: Made axis locking methods `virtual` inside `InputBindings` to facilitate custom behavior (#3838) @JinjieZhao
* Fonts: Improved support for true-type font files and custom typefaces (#3841) @kebox7 @bclehmann
* Axis: Simplified strategy for achieving shared axis limits between multiple controls as seen in the demo application (#3873) @StendProg
* Controls: Improved `Plot.Interactions.Disable()` behavior so interactivity can be restored with `Plot.Interactions.Enable()` (#3879) @StendProg @KroMignon
* Controls: Improved mouse zoom behavior for plots with custom scale factors (#3887, #3886) @BrianAtZetica
* Text: Improve support for text objects containing null strings (#3892, #3861) @sdpenner
* Controls: Improve behavior of Alt + Left-Click-Drag zoom rectangle (#3896, #3845) @MCF
* Label: Improve support for text positioning when custom offsets are in use (#3898, #3865, #3836) @ValeraTychov, @bclehmann, @VibrationAnalystCN
* Avalonia: Enable `Focusable` to improve support for passing keyboard events (#3899) @bclehmann
* ImageMarker: New plot type for displaying an image at a point (#3904) @levipara
* SignalXY: Added `GetNearestX()` to the data source to help locate the point closest to the cursor's X position (#3807) @cataclism
* Scatter: Added `GetNearestX()` to the data source to help locate the point closest to the cursor's X position (#3807) @MatKinPro
* Controls: Disable middle-click-drag zooming on axes which have no data (#3810, #3897) @MCF
* DataLogger: Create `Add()` overloads which accept fixed-length arrays (#3555) @h25019871990
* SignalXY: Ensure the final point is always drawn in high density mode (#3812, #3918, #3921)
* Axes: Improved exception messages when calling `Zoom()` methods with invalid scale factors (#3813) @KennyTK
* WinForms: Exposed `SKControl` so users may bind to its events (#3819) @CD-SailingPerf
* Scatter: Added support for `Scale` and `Offset` properties (#3835) @bukkideme
* Axis Lines: Separated `LegendText` from `LabelText` so items may be configured separately
* Heatmap: Exposed `CellWidth` and `CellHeight` as an alternative sizing strategy to setting `Extent` (#3869) @alexisvrignaud
* ImageRect: New plot type that places an image inside a defined rectangle on the plot (#3870) @sdpenner
* Axis Rules: Improved behavior of snapping rules and improve smoothness of panning rules (#3919, #3547, #3701) @BrianAtZetica
* SignalXY: Improved appearance of rotated plots when low density mode is in use (#3921) @BrianAtZetica
* Heatmap: Added `ManualRange` so users can specify a range spanned by the colormap (#3922) @sdpenner
* Color: Fix infinite loop in the `Color.FromARGB()` overload that accepts an `int` argument (#3924) @r-j-s
* Heatmap: Added cookbook recipe demonstrating how to use custom tick formatter (#3844) @mawbydp
* DataLogger: Improved automatic axis management for loggers with empty datasets (#3880) @TenebrosFR
* SignalXY: Improved interpolation of edge points when step mode is enabled (#3894) @seishinkouki @StendProg
* SignalXY: Improve behavior of off-screen single-point signals (#3926) @githubkau
* SignalXY: Improved cookbook recipe demonstrating SignalXY plots with markers at each point

## ScottPlot 5.0.34
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-05-05_
* Axes: Added `AutoScale()` overloads that accept user-defined lists of plottables (#3776) @levipara
* SignalConst: Properly implement range search to achieve extreme performance improvements for large datasets (#3778) @StendProg @bclehmann @Cardroid
* Ticks: Added options for minor ticks when using DateTime axes (#3779, #3408) @EricEzaM
* Label: Improved support for measurement of labels with null strings (#3736) @Or8e4m4n

## ScottPlot 5.0.33
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-05-04_
* Markers: Reduced memory allocations and improved performance during rendering (#3767) @drolevar
* Axes: Prevent exceptions for conditions where tick generation produces no ticks (#3768) @drolevar @bclehmann
* Signal: Added an experimental signal source that uses caching of binned ranges to improve performance of large datasets (#3718) @Cardroid
* Label: Added `Measure()` overloads to facilitate label size evaluation without requiring `SKPaint` (#3761) @aespitia
* Signal: Fix rendering artifacts for `List<T>` data sources introduced in version 5.0.31 (#3765, #3747) @Limula-PMA
* Crosshair: Added options for a marker to be rendered at the intersection if `MarkerShape` is defined
* Label: Added `FontFile` and `SetTypeface()` to allow users to apply custom fonts (#3722) @kebox7
* SignalXY: Added `ConnectStyle` property to mimic scatter plots and allow for step display style (#3764) @kareem469

## ScottPlot 5.0.32
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-05-01_
* Image: Added support support conversion to/from pixel value arrays to facilitate differential image analysis and testing (#3748, #3727)
* Layout: Improve measurement of vertical axis tick labels (#3736) @ebarnard
* Annotation: Improved positioning of annotations containing many lines (#3749, #3700) @LerkLin
* Label: Significantly improved precision of single and multi-line text measurement and alignment (#3700)
* Axis Line: Set default line width to 1 which also improved default appearance of crosshair (#3752) @fdesordi
* Rendering: Copy the plottable list inside the render loop to facilitate plottable list modification mid-render (#3753) @ZSYMAX
* Controls: Exposed `ZoomRectangle.LineStyle` setter to support advanced customization of middle-click-drag zoom rectangle (#3754) @Graat
* Markers: Separate `LineColor`, `LineWidth`, etc. from `OutlineColor`, `OutlineWidth`, etc. to allow separate customization of line-based vs. fill-based marker shapes (#3755, #3716) @CD-SailingPerf
* Legend: Added `TightHorizontalWrapping` flag to allow items in horizontally oriented legends to wrap without aligning to columns (#3758) @MCF

## ScottPlot 5.0.31
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-04-27_
* Arrow: Refactored the arrow system to support multiple arrow types including user-provided ones (#3745, #3697)
* Color: Colors can be created from System.Drawing colors with `ScottPlot.Color.FromColor(System.Drawing.Color.Blue)` (#3745)
* Signal and SignalXY: Added `YScale` parameter to display data vertically scaled by the specified fraction (#3711, #3708) @feichti92
* Generate: Added `ConsecutiveHours()`, `ConsecutiveDays()`, `ConsecutiveWeekdays()`, to replace `Generate.DateTime` methods (#3721)

## ScottPlot 5.0.30
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-04-26_
* Bar: Set default line width to 1 so error bars are visible without requiring line customization (#3741) @Kareltje1980
* Controls: Added a `Interaction.ChangeOpposingAxesTogether` flag to enable mouse actions to one axis to be applied to all axes with the same orientation (#3729) @rubenslkirchner
* DataLogger: Remove requirement for new data points to contain ascending X values (#3737) @TenebrosFR
* RandomWalk2D: Created `ScottPlot.DataGenerators.RandomWalk2D` for easily generating 2D random data with randomly changing velocity
* Ticks: Improve tick distribution by using the `TickLabelStyle` font size to evaluate maximum tick label size (#3736) @ebarnard


## ScottPlot 5.0.29
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-04-26_
* Axes: Added a `Plot.Axes.ContinuouslyAutoscale` flag useful for plots containing continuously updating data (#3732) @rubenslkirchner
* DataStreamer: Improved axis limit management behavior (#3732) @rubenslkirchner
* Plot: Improved `CoordinateRect()` support for inverted axes (#3731) @Fokatu
* Grid: Improved behavior of `MajorLineWidth` property
* Cookbook: Demonstrate grid alignment with non-standard axes (#3714) @MichaelKuelshammer
* Demo: Improved strategy for axis limit copying in the shared axis demo (#3729) @rubenslkirchner

## ScottPlot 5.0.28
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-04-25_
* Marker: Refactored marker system to support improved styling and the ability to use custom markers (#3716, #3141)
* Interpolation: Improved control points for the first and last points of an interpolated cubic Bézier spline (#3717) @drolevar
* FillY: Improved default line style (#3726, #3723) @SebastianDirks @Fruchtzwerg94
* Plot: Added `MoveToFront()` and `MoveToBack()` to control the order plottables are rendered
* Scatter: Disable marker outline visibility by default (#3720)
* Markers: Disable rendering of lines when `LineWidth` is `0` (#3720)
* Scatter: Added support for filling above and below the curve (#3318, #3380) @xichaoqiang @Diddlik @slotinvo
* DataStreamer: Added `ContinuouslyAutoscale` flag to allow the vertical range to always tightly fit the data (#3561) @hazenjaqdx3 @zhhding @Xhichn
* Markers: Added `FillOutline` flag to make drawing lines on filled markers an opt-in feature

## ScottPlot 5.0.27
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-04-23_
* Signal: Corrected floating point error that caused points to be dropped in rare conditions (#3665) @mjazd
* DataStreamer: Added an optional argument to `ViewWipeRight()` that sets the fraction of oldest data to hide (#3668) @mloppnow
* Label: Refactored plottables to use consistently named properties. Properties such as `plottable.Label.FontColor` are now `plottable.Label.FontColor`, or `plottable.LabelStyle.FontColor`. Referencing obsolete property names yield build errors with messages that indicate names of the new properties to use. (#3658, #3666)
* Plottables: Styling objects `LabelStyle`, `LineStyle`, `MarkerStyle`, etc. are now readonly. Their contents may be set, and most plottables expose shortcuts to their properties. (#3658, #3666)
* Plot: Added `RenderInMemory()` (alias for `GetImage()`) so users can force a render as part of their startup process (#3674) Boris
* Ticks: improve appearance of rotated multiline tick labels (#3673) @aespitia
* Demo: Add an example window where the legend is displayed outside the plot control (#3672, #3635) @mikeKuester @Graat
* Demo: Fix mouse tracking logic to improve behavior of the multi-series mouseover demo (#3680, #3684) @jamaa @Graat
* Ticks: Refined tick label measurement for improved tick spacing (#3689)
* Legend: Added many additional customization options and support for multiple shapes (#3689)
* Legend: Text appearing in the legend for many plot types has been renamed from `Label` to `LegendText` (#3689)
* Rendering: Added `Plot.GetSvgXml()` so plots can create SVG images in memory without saving to disk (#3694) @aespitia
* Bar: Improved alignment of value labels on horizontal bar charts (#3698) @aespitia
* Legend: Created a `LegendPanel` to allow legends to be displayed outside the data area (#3672, #3635) @Graat @mikeKuester
* Axis: Prevent left axis from appearing if no plottables use it (#3637) @jpgarza93
* Label: Added `BorderRadius` to support backgrounds and outlines with rounded edges (#3659)
* Axis Rules: Changed behavior of axis rules to reduce reliance on previous renders (#3674, #1966, #3547)
* Blazor: Numerous improvements to the Blazor cookbook (#3667) @KroMignon
* Finance: Improve support for DateTime candlesticks before 1900 where OADate is negative (#3678)
* Label: Added ability to separately control background vs. text Anti-Aliasing (#3686)
* Ticks: Use system `CultureInfo` to generate numeric tick labels (#3688, #3687) @xantiva @mikeKuester
* Plottables: Made all `Render()` methods `virtual void` to facilitate advanced customization (#3693) @sdhongjun
* Function: Improve function plot performance by only calculating visible range (#3703) @Matthew-Chidlow

## ScottPlot 5.0.26
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-04-14_
* Function: Improved autoscaling behavior and respect for user-defined horizontal ranges (#3618) @Matthew-Chidlow  
* Function: Exposed `MinX` and `MaxX` to allow users to restrict display to a horizontal range (#3595, #3603) @Matthew-Chidlow @Dibyanshuaman
* Axis Lines: Added `ExcludeFromLegend` so text can be added to axis line labels without appearing in the legend (#3612) @MCF
* WPF: Added `GetPlotPixelPosition()` for getting mouse position relative to the figure (#3622) @KroMignon
* Scatter: Upgraded the default smooth behavior to use cubic spline interpolation and exposed `SmoothTension` (#3623, #3606, #3274, #3566, #3629) @drolevar
* Vector Field: Added a new plot type to display a collection of rooted vectors (#3625, #3626, #3632, #3630, #3631) @bclehmann
* AxisLine: Improve appearance in of the key displayed in the legend (#3627, #3613) @MCF
* Crosshair: Expose `VerticalLine` and `HorizontalLine` for to allow axis-specific customization (#3638) @Fruchtzwerg94 @heartacker
* AxisLine: Added properties for faster styling including an optional `TextAlignment` setting (#3640, #3624) @MCF
* Axes: Improved autoscaling support behavior for plots where nonstandard axes are in use (#3641, #3637) @KroMignon @jpgarza93
* WinUI: Added automatic display scaling detection and correction (#3642) @PZidlik
* Bar: Added a `CenterLabel` flag to cause value labels to be displayed centered within a bar (#3391) @tibormarchynzoom
* FormsPlot: Allow plots to persist through `Show()` and `Close()` events (#3643, #3589) @CodeBehemoth @bwedding @Kruno313
* Callout: New plot type that displays text with an arrow that points to a location on the plot (#3650, #3654) @NicolasLairNET
* Cookbook: Simplified function recipes to use static methods (#3656, #3655) @abdul-muyeed
* Demo: Created a WPF demo application to document WPF-specific topics like display scaling (#3585, #3622) @KroMignon @MagicFawkes
* Blazor: Fixed issue causing the `ScottPlot.Blazor` package to install the SkiaSharp WinForms control (#3621) @angelofb

## ScottPlot 4.1.74
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-04-14_
* Security: Use System.Drawing.Common version 4.7.2 to address [CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28)
* Package: Target supported versions of .NET Framework (4.6.2, 4.7.2, and 4.8) and .NET (6.0 and 8.0)

## ScottPlot 5.0.25
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-04-08_
* NuGet: Use snupkg format to include debug symbols (#3496)
* Scatter: Fixed indexing error affecting `GetNearest()` (#3616, #3461) @Matthew-Chidlow @SongPing @sunshuaize @mikeKuester
* Generate: Exposed a static `RandomWalker` instance for easily generating random walk datasets

## ScottPlot 5.0.24
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-04-07_
* Ticks: Modified `NumericFixedInterval` to add support for inverted axes (#3567) @Alexander-png
* Bar plot: Improved support for labels on horizontally oriented bar plots (#3546, #3520) @aespitia @yui1227
* Axis: Added new axis rules for edge locking, center locking, and tick snapping (#3547) @BrianAtZetica
* SignalXY: Added `GetNearest()` for locating the data point nearest the cursor (#3550) @endreew
* Demo: Added demonstration for draggable `SignalXY` plots which respond to the cursor (#3550) @endreew
* Legend: Do not display plottables where `IsVisible` is `false` (#3552, #3545, #3541) @KroMignon, @blahetal, @pkstrsk
* Annotation: Improve positioning so it is unaffected by typeface or font (#3558) @MCF
* Controls: Improve render artifacts on platforms that allow concurrent rendering and UI manipulation (#3559, #3557) @chjrom @Limula-PMA
* Controls: Improve behavior of interactions started outside the plot area (#3571, #3543) @bwedding @pkstrsk
* Label: Prevent rendering borders when line width is zero (#3572, #3538) @bwedding
* Scatter: Added support for `NaN` values to display gaps in the line (#3577, #3276) @drolevar @Hub3r
* DataLogger: Added support for `NaN` values to display gaps in the line (#3577) @drolevar
* Finance: OHLC plots now have a `Sequential` mode (like candlestick plots) for displaying data without gaps (#3590) @oktrue
* Plot: Added optional arguments to `GetCoordinateRect()` to support non-standard axes (#3591) @oktrue
* Axes: Added optional arguments to `Plot.Axes.AutoScale()` to add support for nonstandard axes (#3592)
* Axis Rules: Improved `Plot.Axes.SquareUnits()` to support inverted axes (#3592) @VisMotrix
* WinForms: Improve `FormsPlot` disposal so the control displays properly when re-launched (#3593, #3589) @bwedding @Kruno313
* Signal: Added support for inverted horizontal axes (#3594) @Excustic
* Axes: New helper methods `Plot.Axes.InvertX()`, `Plot.Axes.RectifyX()`, and similar for Y (#3594)
* Rendering: Improved performance for plot types with many lines (#3601) @drolevar
* Function Plot: Improve support for functions with limited X ranges (#3595, #3603) @Dibyanshuaman @Matthew-Chidlow
* Controls: All controls now include `Reset()` overloads for resetting or replacing the `Plot` (#3604, #3353) @aniketkumar7 @jon-rizzo
* Scatter: The `Smooth` property now allows points to be connected with smooth lines (#3606, #3274, #3566) @bjschwarz @ja1234567 @bwedding @CBrauer
* Layout: Added logic to reduce the size of axes which are visible but not used by any plottable (#3608)
* Colorbar: Improved positioning and support for adding multiple colorbars to plots (#3294, #3560, #3586) @NateEbling @mawbydp @hnMel
* Colorbar: Added a `Label` which users can customize to display an optional title (#3611) @mawbydp
* SignalXY: Added support for markers and marker styling (#3602, #3609) @Giviruk
* Scatter: Added support for `MinRenderIndex` and `MaxRenderIndex` to limit display to a portion of the data (#3614, #3308) @wellsw

## ScottPlot 5.0.23
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-03-24_
* Plot: Added `ShowLegend()` overload that does not override the current `Orientation` (#3450) @aespitia
* Grid: The standard grid can be accessed via `Plot.Grid` instead of `GetDefaultGrid()`
* Style: `Plot.Style.ColorLegend()` is deprecated. Access `Plot.Legend` properties directly as seen in the cookbook.
* Style: `Plot.Style.ColorAxes()` has moved to `Plot.Axes.Color()`
* Style: `Plot.Style.AxisFrame()` has moved to `Plot.Axes.Frame()`
* Style: `Plot.Style.SetBestFonts()` has moved to `Plot.Font.Automatic()`
* Grid: Added `Plot.Grid` with axis-specific styling options as seen in the cookbook (#3291, #3293) @bjschwarz @PaxITIS
* SignalXY: Fixed a bug where the final line segment was not drawn (#3495, #3423) @MareMare @mjazd
* SignalXY: Improved support for inverted vertical axes (#3495) @MareMare
* Controls: Ignore mouse wheel zooming if a zoom rectangle is being drawn (#3498) @BrianAtZetica
* Controls: Improve axis lock behavior when dragging the mouse on a control (#3498) @BrianAtZetica
* Heatmap: Added `Opacity` and `AlphaMap` properties to enhance transparency customization (#3499, #3349) @BrianAtZetica
* Heatmap: Intensity values that are `double.NaN` are now displayed as transparent cells (#3499, #3349) @BrianAtZetica
* Text: Added an `OffsetX` and `OffsetY` properties for adjusting text position in pixel units (#3506) @jamaa
* Demo: Added a demonstration window for highlight the point nearest the cursor across multiple scatter plots (#3507, #3503) @jamaa @RubensMigliore
* Polygon: Improved automatic axis limit detection of polygons (#3501) @drphobos
* Annotation: New plot type for adding text labels aligned to the data area which are always visible (#3510, #3356) @dlampa
* Ticks: Added `MinimumTickSpacing`, `TickDensity`, and `TargetTickCount` properties to the automatic tick generator (see Cookbook)
* Avalonia: Fixed transparent background issue introduced in the previous version (#3502, #3516) @chjrom @MrOldOwl @kebox7
* Rendering: Improved canvas state management to prevent duplicate restoration calls (#3527, #3523, #3528) @BrianAtZetica @chjrom
* Signal: Improved performance of large signal plots when zoomed in (#3530, #3229) @minjjKang 

## ScottPlot 4.1.73
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-03-24_
* Image: Improve automatic axis limit detection for images with manually defined positions (#3529, #3515) @bukkideme

## ScottPlot 5.0.22
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-03-16_
* Rendering: Added additional options for gradient fills (#3324) @KroMignon
* Plot: Improve `GetPixel()` behavior when a custom `ScaleFactor` is in use (#3327) @MCF
* Fonts: Improve behavior of cached typefaces (#3334, #3335) @Milkitic
* Legend: Added support for horizontal orientation (#3341, #3302, #3280) @KroMignon
* Controls: Created `AddSeparator()` to facilitate creation of custom context menus (#3342) @MCF
* Live Data: Improved indexing of the `Wipe` view to prevent race conditions when displaying live data (#3352) @drolevar
* Radial Gauge Plot: Added a new plot type for displaying categorical data as circular gauges (#3358) @arthurits
* Generate: Improved `RandomNormalSample()` behavior by fixing an off-by-one indexing error @DominicBeer
* Avalonia: Redraw plots using a non-blocking background thread to improve multi-axis behavior (#3373, #3359) @oktrue, @BendRocks, and @ykarpeev
* Bar plot: Added a `Label` property to allow a collection of bars to be displayed as a single item in the legend (#3375) @fhannan-ti
* WPF: Redraw plots using a non-blocking background background thread to improve multi-axis behavior (#3373, #3359, #3381) @drolevar
* Ellipse: Added `LineWidth`, `LineColor`, and `FillColor` shortcut properties
* Color: Added `Lighten()` and `Darken()` properties (#3387, #3390) @KroMignon
* Color: Modified `ToHSL()` to return improved Hue, Saturation and Luminosity values (#3390) @KroMignon
* SignalXY: Improve support for displaying data on inverted axes (#3396, #3400) @BrianAtZetica
* Axes: Improved support for ticks and labels on inverted axes (#3401, #3397) @BrianAtZetica
* Plot: Added `Remove()` overloads for Axes, Panels, and Grids (#3402, #3360) @Excustic, @redrabbit007, @csbebetter, @xichaoqiang
* Plot: `Plot.FigureBackground` is now `Plot.FigureBackground.Color` (and same with `DataBackground`)
* Plot: `Plot.FigureBackground.Image` and `Plot.DataBackground.Image` can be used to add a background image to plots (#3406, #3405) @unsigned-ru
* Axes: Updated the auto-scaler to ignore plottables with visibility disabled (#3407) @levipara
* Axes: Restrict pan, zoom, and autoscale to a single dimension if the cursor is over an axis panel (#3410) @drolevar
* Controls: Improved behavior of middle-click-drag zoom rectangle actions when CTRL or SHIFT is pressed
* DataLogger and DataStreamer: Improve support for multi-axis plots (#3411) @drolevar
* Controls: Prevent unnecessary zoom rectangle clearing (#3412) @drolevar
* Axes: Improve placement of decisecond and centisecond ticks on DateTime axes (#3413) @drolevar
* Label: Improved appearance of multiline labels with outlined borders or filled backgrounds (#3415, #3371) @NicolasLairNET
* Label: Added a `LineSpacing` property to allow manually defining line height in multi-line labels (#3415, #3371) @NicolasLairNET
* Heatmap: Improve vertical placement of scaled heatmaps (#3416, #3417) @BrianAtZetica
* Heatmap: Added `FlipHorizontally` and `FlipVertically` properties (#3418, #3419) @BrianAtZetica
* Heatmap: Fixed off-by-one render error when `Extent` is provided by the user (#3434, #3419) @BrianAtZetica
* Heatmap: Added support for the `Smooth` property do render anti-aliased images (#3419) @BrianAtZetica
* Ticks: Fix issue where manual ticks could be displayed outside the data area (#3425, #3427, #3395) @oktrue @wsomegapoint
* DataLogger and DataStreamer: prevent possible out-of-range exception when using the `Scroll` view mode (#3430, #3429) @KroMignon
* Color: Added `MixWith()` and related methods to facilitate color mixing and creation of color gradients (#3443, #3441) @KroMignon
* Pie: Added `DonutFraction` property to enable donut charts (#3447, #3438) @aespitia, @Prototipo-Erick-Santander
* Plot: `ScaleFactor` is now a `double` for simplified assignment (#3454, #3455) @MCF
* Marker: Improved default settings for outline-only markers (#3456, #3453) @KroMignon
* Scatter: New `Add.ScatterLine()` method creates a scatter plot with a line only and no markers (#3462, #3452) @MCF
* Scatter: New `Add.ScatterPoints()` method creates a scatter plot with markers only and no line (#3462, #3452) @MCF
* Ticks: Improve performance by reducing the number of string measurements (#3468) @drolevar
* Plot: `GetCoordinateRect()` now returns dimensions that respect `ScaleFactor` (#3471) @MCF
* Label: Added `Measure()` overloads to facilitate measuring arbitrary strings without modifying the label text (#3474, #3473, #3458) @aespitia @David-A-Blankenship
* Layout: Improved positioning of text for bottom tick labels with large font (#3436) @edwwsw
* Legend: Improve international font support when `Plot.Style.SetBestFonts()` is used (#3440) @edwwsw @yui1227
* Drawing: Do not draw rectangles if they have a line width of zero (#3384)
* Ticks: Do not render ticks on `Axes` where `TickLabelStyle.IsVisible` is `false`
* NuGet: Package now includes native Linux assets regardless of original build target (#3481, #3357)
* Bar: Added `Label` property to allow values to be displayed above bars (#3477) @DouglasWatt
* Axes: Added `Plot.Axes.SquareUnits()` helper method for adding an axis rule that enforces equal axis scales (#3451)
* Pie: Fixed issue were pie charts may have duplicate legends (#3445)
* Axes: Improved render event behavior and support for multi-axis plots (#3525) @BrianAtZetica

## ScottPlot 4.1.72
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-03-16_
* DataLogger: Improve support for multi-axis plots (#3411) @drolevar

## ScottPlot 5.0.21
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-01-28_
* RenderManager: Exposed `EnableRendering` to facilitate render locking in async environments (#3264, #3213, #3095) @kagerouttepaso
* Arrow: Added a new arrow plot type that can be used to mark a position in coordinate space (#3265) @hockerschwan
* Label: Improved measurement of text containing leading or tailing whitespace (#3223, #3268) @KroMignon and @lindpatr
* Axis Line: Added `LabelOppositeAxis` property and created dedicated cookbook page (#3275)Lyakabynka
* Plot: `AddRectangle()` now accepts more input types (#3263) @enumer
* Ticks: Created `IMinorTickGenerator` to allow users to inject their own logic for placing minor ticks
* Axes: Added support for log-scale tick labels and grid lines (#3143)
* Plot: Users can now `Add.Ellipse()` and `Add.Circle()` to place closed curves on plots (#3277, #3287) @hockerschwan
* Plot: Added `Plot.Remove()` overloads for removing all plottables of the given type (#3296, #3296) @DerekGooding
* Plot: Added `Plot.Remove()` overloads for removing plottables matching specific criteria (#3296, #3297) @KroMignon
* Plot: Added `Plot.GetPlottables()` overloads to facilitate iterating over plottables of a specific type
* Rendering: Added support for gradient fills (#3298, #3157, #3310) @KroMignon, @hockerschwan, and @faguetan
* Controls: Disabling interactions then re-enabling them restores original interactions (#3305, #3304) @Nils-Berghs
* Plot: Added `Plot.GetPixel()` overload for improved support on multi-axis plots (#3306) @MCF
* Axis Lines: Label background color may be distinct from line color (#3309) @PhoenixChenLu
* Axis Spans: New `Plot.Add.HorizontalSpan()` and `Plot.Add.VerticalSpan()` methods for shading axis ranges (#3307) @erikjl
* Interactivity: Added methods to simplify dragging axis lines and spans. See the demo application for details. (#3307) @erikjl
* Ticks: Improved tick density calculation to prevent overlapping tick labels for very large numbers (#3203)
* Axes: Deprecate `DateTimeTicks(Edge.Bottom)` in favor of `DateTimeTicksBottom()` which now returns the created axis.
* Cookbook: Demonstrate DateTime tick labels with custom string formatting (#3272, #3273) @sterenas and @stratdev3
* Demo: Added icon to main application and all windows launched within (#3281, #3273) @sterenas
* Controls: Do not list OpenGL controls in the toolbox. They can still be added programmatically, but they invite many problems and offer little performance improvements for most applications so their use is discouraged (#3282, #3262, #3271)
* WinForms: Disable design time visibility in .NET Framework projects to prevent Visual Studio error messages (#3300) @MaxFun
* Markers: Added `Plot.Add.Markers()` to display a collection of marker positions all using the same style (#3283)
* Axes: Added `Plot.Axes.Remove()` to allow users to remove additional axes they may have added (#3288)Felix
* Data Streamer and Data Logger: Renamed `IAxisManager` to `IAxisLimitManager` to disambiguate it from the `AxisManager` class (#3289)
* Pie: Added support for displaying slice label text above each slice (#3295) @sterenas
* Plot: `Save()` methods used to return the saved file path as a `string` but now they return a `SavedImageInfo` with a `Path` property and additional information (#3314)

## ScottPlot 5.0.20
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-01-21_
* Assembly: ScottPlot packages are now strongly named (#3235, #3241) @mlessmann
* Scatter Plot: Added a `ConnectStyle` to enable step display mode (#3242) @NoahSigl
* Polygon: `Plot.Add.Polygon()` now accepts generic type lists and arrays (#3244)howhowone_23
* Demo: Added a draggable points window to show how to drag points of a scatter plot (#3248)bologna
* Generate: Added `RandomNumber()` and `RandomNumbers()` overloads
* OHLC: Improved autoscaling behavior for empty datasets
* Generate: Added `RandomOHLCs()` overload that accepts a starting `DateTime` (#3254) @CBrauer
* Axes: Improved support for inverted axes (#3252) @fujiangang
* Finance: Improved performance of financial charts by not rendering symbols outside the data area (#3258)Lyakabynka
* SignalXY: Support vertical orientation (#3253) @manaruto
* Data logger and streamer: The property `Data` has been renamed to `DataSource` (#3260)
* SignalConst: The property `Data` has been renamed to `DataSource` (#3260)
* Axes: Added `AutoScaleExpand()` to zoom out to fit data only if necessary (#3259)
* Style: Added `Plot.Style.ColorLegend()` for quick customization of legend colors (#3247)
* Plot: Replacing palettes is now achieved by setting `Plot.Add.Palette` instead of `Plot.Palette`.
* Plot: Added `ShowLegend()` overload that accepts manually created legend items
* Scatter Plot: Added `LinePattern` property for customizing line style
* Pie: Improved default colors of pie charts created from discrete values

## ScottPlot 4.1.71
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-01-21_
* Assembly: All ScottPlot packages use the same strong name signing key (#3235, #3241) @mlessmann
* WPF Control: Routed events now pass the original source (#3243) @MarekJur

## ScottPlot 5.0.19
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-01-18_
* Plot: Improved render manager initialization (#3233) @VoteForPedro
* Projects: Sign all assemblies using strong names (#3235, #283) @mlessmann
* Axes: Improved automatic axis determination for plots containing non-real or infinite data limits (#3232, #3237)
* Bar Plots: Do not overwrite existing colors when adding `Bar` collections to the plot (#3231)
* Label: Clear cached typefaces when styles change (#3236) @kl7107 and @prime167

## ScottPlot 5.0.18
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-01-17_
* Axes: Improved default label rotation for DateTime axes (#3211, #3216) @CBrauer
* Fonts: Improved font detection for strings containing mixed-language characters (#3220, #3184, #2746) @kl7107 and @prime167
* Controls: Add a Reset function for context menus (#3224) @MCF
* Axes: Prevent exceptions when generating ticks for a DateTime axis with zero size (#3221) @devbotas
* SignalXY: Added `MinimumIndex` and `MaximumIndex` for partial array rendering (#3227)
* SignalXY: Added `OffsetX` and `OffsetY` for for applying a fixed offset in coordinate space (#3227)
* SignalConst: Improved display when signals are zoomed in enough to see individual points (#3228) @Marvenix

## ScottPlot 5.0.17
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-01-16_
* Rendering: Added a `RenderManager.EnableRendering` flag to skip render requests while performing dangerous actions in multi-threaded environments. Skipping renders compliments the `PreRenderLock` event which hangs renders. See the [async FAQ page](https://scottplot.net/faq/async/) for usage details. (#3213, #3095) @LumAsWell and @bclehmann
* WPF: Improved "Copy to Clipboard" functionality (#3214) @MCF
* Controls: Created `FormsPlotViewer` and `WpfPlotViewer` for launching interactive plots from console applications. See the [Interactive Plots in Console Applications](https://scottplot.net/faq/launch-console/) page for details. (#3212, #308) @chaojian-zhang
* DataLogger: Added `Add()` overloads which support X/Y pairs (#3210) @devbotas

## ScottPlot 5.0.16
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-01-15_
* Data Streamer: A new plot type for displaying live data as a fixed-width line trace (#3202, #3205)
* Data Logger: A new plot type for displaying live data as a growing scatter plot (#3202, #3205)
* Generate: Created a `Generate.RandomWalker` class for producing an infinite amount of streaming random data
* Ticks: Improved support for multi-line tick labels on vertical axes (#3208) @raburton
* Text: Exposed `FontName` and `LabelText` properties
* Internationalization: `Fonts.Detect()` now inspects all characters instead of just the first (#3184, #2746) @prime167
* Label: Added `SetBestFont()` to apply the installed font most likely able to display characters in the label (#3184, #2746) @prime167
* Style: Added `Plot.Style.SetBestFonts()` to apply the best font to all plot components (#3184, #2746) @prime167
* Controls: Removed `GetCoordinates()` from `IPlotControl`. Users can call Plot.GetCoordinates()` directly. (#3199)
* Ticks: Do not display manually defined grid lines, tick marks, or tick labels to appear outside the data area (#3207)
* Rendering: Created `IManagesAxisLimits` for `IPlottable` objects that manipulate axis limits at render time (#3207)
* NuGet: Improved package descriptions to better reflect that ScottPlot 5 is no longer in preview (#3207)

## ScottPlot 5.0.15
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-01-14_
* Ticks: Added additional styling options for axis tick labels (#3185) @barnettben
* Finance: Added `Sequential` property to display OHLC data without gaps (#2611, #3187) @robbyls, @mjpz, and @segeyros
* SignalConst: A high performance plot type for evenly-spaced unchanging data (#70, #3188) @StendProg
* Plot: Created `Plot.Add.Rectangle()` for placing a rectangular polygon on the plot
* Axis Rules: Improved `MaximumBoundary` and `MinimumBoundary` correction behavior (#3191) @Milyczekpolsl
* Bar Plot: Added support for horizontal bar graphs (#3192) @sghctoma

## ScottPlot 5.0.14-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-01-10_
* Rendering: Improved performance by limiting how often `AutoScale()` is called by the renderer (#3183) @Smonze

## ScottPlot 5.0.13-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-01-09_
* SignalXY: New high performance plot type for signal data with defined ascending X positions (#3163) @ChristianWeigand
* Scatter, Signal, and SignalXY: Improved support for generic data types
* Axis: Improve behavior of axis rules which reference axes from previous renders (#3179) @raburton
* Primitives: Separated `CoordinateRange` struct for passing ranges and `CoordinateRangeMutable` for mutating them (#3170)
* Function: Improved autoscaling behavior

## ScottPlot 5.0.12-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2024-01-07_
* Axes: Improved automatic axis scaling for plots containing 1D plottables (#3132)
* Coordinates: Added `AreReal` property to confirm `X` and `Y` are finite
* Crosshair: Added `X` and `Y` properties to compliment `Position`
* Axes: Removed `Plot.Axes` list to encourage interaction with `Plot.YAxes` and `Plot.XAxes` (#3133)
* Plot: Added `AddLeftAxis()`, `AddRightAxis()`, etc. to simplify multi-axis creation and management (#3133)
* Layout: Created `Plot.Layout.Frameless()` to hide axes and allow the data area to fill the figure
* Axes: Improve rotation for right axis labels
* Bar: Improve autoscaling for bar plots displaying error ranges
* Signal: Improved rendering of makers when plots are zoomed in (#3136)
* Signal: Exposed `Color`, `LineWidth`, and `MaximumMarkerSize` so users do not interact with `LineStyle` and `MarkerStyle` directly (#3136)
* Statistics: Created `Series` class for calculating statistics for time series data
* Scatter Plot: Added `LineWidth` and `MarkerSize` properties
* Finance: Created `SimpleMovingAverage` and `BollingerBands` in the `ScottPlot.Finance` namespace to facilitate calculation and display of technical indicators (#3137)
* Axes: Moved axis management logic from `Plot` into the `Plot.Axes` class. Notable changed method names include `Plot.Axes.SetLimits()`, `Plot.Axes.GetLimits()`, `Plot.Axes.AutoScale()`, and `Plot.Axes.Margins()` (#3140)
* Rendering: Improved anti-aliased drawing of solid shapes
* Axis: Added rules for zoom in/out boundaries, axis span limits, and square ratio locking (#3139, #3142)
* ErrorBar: Improved axis limit detection for data that does not start at zero (#3155) @wolfcomp
* DataSources: Created `SignalSourceUInt16` to demonstrate how to plot data with custom types (#3154) @angulion
* Signal Plot: Added support for generic type arrays and lists (#3154)
* Scatter Plot: Added support for generic types including Xs and Ys of different types (#3154)
* Scatter Plot: Added support for DateTime types (#3154)
* Style: Added support for more line patterns (#3161) @MCF
* Controls: Assigning `Interaction` can be used instead of `Replace()` for customizing mouse actions (#3150)
* Controls: Added `Menu` with `Add()` and `Clear()` methods to simplify context menu customization (#3150)
* Axes: Added rules for locking horizontal and/or vertical axes (#3160) @raburton
* Signal: Added `Data.MinimumIndex` and `Data.MaximumIndex` to allow for partial array rendering (#3158) @raburton
* Heatmap: Added `GetIndexes()` and `GetValue()` to get data from a coordinate (#3165) @skn41

## ScottPlot 5.0.11-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-12-29_
* Plot: `AutoScaler` can now be assigned a `FractionalAutoScaler` with custom properties (#3069, #3067) @arthurits
* Controls: add SVG to recommended save formats in the right-click menu (#3068)
* Bar: Refactored bar plots to simplify individual bar customization (#3070, #3066)
* Legend: Added `ManualItems` to allow building custom legend content
* Render: Prevent the pre-render auto-scaler from resetting manually defined axis limits (#3058)
* Cookbook: Rewrote reflection and source file parsing for simplified querying (#3081, #3080, #3079, #2962, #2755)
* Function: Added a new line plot type where Y position is a user defined function (#3094) @bclehmann
* Axes: Improved axis label alignment for secondary axes (#3030) @albyoo
* Statistics: Added generic overloads to `Statistics.Descriptive` class, renamed `StDev()` to `StandardDeviation()`, and added methods for calculating both sample and population statistics (#3071 and #3055) @arthurits
* Markers: Added a `None` marker (#3075, #3057) @Gray-lab
* Generate: Added methods for generating random marker shapes and colors
* Generate: `Random()` is deprecated in favor of `RandomSample()`
* Plot: Added `ShowLegend()` and `HideLegend()` helper methods which set `Plot.Legend.IsVisible`
* Marker Plot: `Plot.AddMarker()` can now be used to place a single marker on the plot (#3076, #2806) @Gray-lab
* Rendering: Fixed issue where disabling a plottable's visibility prevented rendering of subsequent plottables (#3097, #3089) @KroMignon
* SVG: Improved rendering of shadows by adding slight color to semitransparent black (#3098, #3063) @KroMignon
* Colormap: Added a `Reversed()` method for creating colormaps with reversed color order (#3100) @bukkideme
* Version: Added `ShouldBe()` method to assert the version of ScottPlot matches the expected one (#3093)
* Ticks: `TickGenerators.NumericManual` now has `AddMajor()` and `AddMinor()` to simplify manual tick placement (#3105, #2957)Lake
* Legend: Added `Plot.GetLegendImage()` and `Plot.GetLegendSvg()` for displaying legends outside plots (#3062, #2934) @KroMignon, @lichen95, and @bclehmann
* Plot: Added new `Line` plot type for creating straight lines between two points (#2915, #3109) @Gray-lab
* Controls: Added `IPlotInteraction` so users can inject their own `Interaction` (#3111, #3110) @albyoo
* Signal: Improved appearance of signal plots where `YOffset` is used (#2949) @minjjKang
* AxisLine: Improve rendering and simplify API by exposing common properties (#3060, #3056)
* Legend: `Alignment` has been renamed to `Location` (#3059)
* Box: Refactored box plot API to favor simplicity and user customization (#3072)
* Rendering: Added `RenderManager.RenderStarting` event to allow modification of plottable properties (#3077)GooBad
* Rendering: Added `RenderManager.PreRenderLock` event so developers of multi-threaded applications can ensure plottables are stable at render time (#3095) @bclehmann
* Statistics: Added descriptive statistics methods and improved support for 2D arrays (#3113, #3121) @arthurits
* Rendering: Improved appearance of shapes with custom hatches and outlines (#3099) @faguetan
* Text: Improved support for multiline labels (#3087) @raburton
* Layout: Improved tick and axis label alignment in fixed layout plots (#3104) @albyoo
* Layout: Created `Plot.Layout` class for holding `Frameless()` and related methods (#3106) @angulion

## ScottPlot 4.1.70
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-12-29_
* Population Plot: Improved performance for populations with curves that run off the screen (#3054) @Em3a-c and @cornford
* Performance: Improved performance of Bar and Finance plots by not drawing shapes outside the data area (#3053, #3078) @AndreyPalyutin
* Colormap: Added a `Reversed()` method for creating colormaps with reversed color order (#3100) @bukkideme
* Version: Added `ShouldBe()` method to assert the version of ScottPlot matches the expected one (#3093)
* Marker: Added support for `Marker.horizontalBar` to compliment `verticalBar` (#3101) @SerhiiMahera
* Axis: Span limits are respected when zooming with a window or scroll wheel (#3082) @ashe27
* Statistics: Added `Descriptive.StdErr()` for calculating standard error of the mean (#3112)

## ScottPlot 5.0.10-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-12-03_
* Signal: Improved support for datasets with repeating values (#2933, #2935) @StendProg
* Blazor: Added a Blazor control (#2959) @sulivanganter
* Layout: Expose `Matched` layout engine (#2881) @proplunger
* Plot: Added `DisableGrid()` and `EnableGrid()` helper methods (#2947)
* Render: Created `IRenderLast` plottables can implement to draw above axes (#2998, #2993)
* Controls: Added `Interaction.Disable()` and `Interaction.Enable()` methods for easy control of mouse interactivity
* Render: Improve axis frame and tick mark rendering for SVG export (#2944) @Crown0815
* Controls: Created OpenGL controls `FormsPlotGL` and `WpfPlotGL` distinct from `FormsPlot` and `WpfPlot` (#3008, #3007, #2950, #2395, #2565)
* Markers: Added numerous additional marker types (#2999, #3019) @Gray-lab
* Plot: Improved support for multiple axes and automatic scaling (#3027)
* RandomDataGenerator: Use a global Random number generator for improved randomness and thread safety (#2893, #3022) @KroMignon
* Scatter: Added `Data.GetNearest()` to simplify locating the point nearest the cursor (#3026, #3048) @JurasskPark and @CBrauer
* Plottable: Added a new `Text` plot type for displaying a label at a location in coordinate space (#2939)
* Plot: Benchmark is now a user-customizable plottable and `Plot.ShowBenchmark` is now `Plot.Benchmark.IsVisible` (#2961)
* Grid: Improve support for custom line styles (#2904) @minjjKang
* Pie: Improve appearance of slice labels in the legend (#2894, #2852) @zy1075984
* Legend: Replaced `List<ILegend>` with a simple `Legend` object with an `IsVisible` property (#2792)
* Avalonia: Improved sizing of plot controls inside containers (#2923) @JohnSmith20211124 and @Developer-Alexander

## ScottPlot 4.1.69
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-12-03_
* Axis: Added `IsReverse` property to let users invert the orientation of an axis (#2958) @HandsomeGoldenKnight
* Text: Exposed `LastRenderRectangleCoordinates` to improve mouse interactivity (#2994) @DaveMartel
* Arrow: Fixed bug in constructor overload (#2976, #3001) @Gray-lab
* Controls: Resizing will now invoke `OnAxesChanged` event (#3000, #3002) @dhgigisoave
* Plot: Added `LastRenderDimensions` for easy access to the latest figure dimensions (#3000, #3002) @dhgigisoave
* DataLogger and DataStreamer: Added support for custom line styles (#2972, #2972) @Gray-lab
* Population: Defining `BoxAlphaOverride` and `MarkerAlpha` allows for exact color representation (#2967, #3013) @Gray-lab and @Em3a-c
* RandomDataGenerator: Use a global Random number generator for improved randomness and thread safety (#2893, #3022) @KroMignon
* Controls: Improve `Bitmap` disposal as controls are unloaded (#3023, #2902) @KroMignon and @mocakturk
* ScatterPlotDraggable: Fixed bug affecting `IsUnderMouse()` after `Update()` is called (#2870, #2969, #3025) @KroMignon, @SasKayDE, and @onur-akaydin
* Bar: New `AddBar()` overload for creating a single highly customized bar graph bar (#3024, #3033) @melhashash
* FormsPlot: Fix bug affecting mouse interaction on plots with all items hidden (#2895) @LapinFou
* RadarPlot: Added customization options for axis label string formatting, manual tick positions, and transparency (#3041) @JbmOnGitHub
* Axis: Added `Axis.SetTicks()` to allow full customization of major and minor ticks (#2957) @FannyAtGitHub
* Plot: `GetImageHTML()` has been renamed to `GetImageHtml()` (#2974) @b4shful

## ScottPlot 5.0.9-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-10-03_
* PixelPadding: `TotalHorizontal` and `TotalVertical` renamed to `Horizontal` and `Vertical` (#2874, #2878) @viktoriussuwandi
* CoordinateRect: Added `Expanded()` method for creating a copy of the rectangle expanded to include a given point (#2871, #2890) @aespitia
* FillY: Added legend support (#2886, #2896) @msroest
* Plot: Created `Add.Line(x1, x2, y1, y2)` and related overloads for adding straight lines to plots (#2901, #2915)
* LinearRegression: Added `Statistics.Regression` (see cookbook) for fitting lines to collections of X/Y data points (#2901) @anewton
* Avalonia: Improve rendering in multi-control windows (#2920) @nightfog-git

## ScottPlot 4.1.68
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-10-03_
* Heatmap: Added a `UseParallel` option which can improve `Update()` performance for large datasets (#2897) @bukkideme

## ScottPlot 5.0.8-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-08-13_
* Rendering: Improved performance by caching typefaces (#2833, #2848) @KroMignon and @taya92413
* Avalonia: Improved performance, DPI awareness, and color rendering (#2818, #2859) @oktrue
* Rename: `XMin`, `XMax`, `YMin`, and `YMax` properties are now `Left`, `Right`, `Bottom`, `Top` for all coordinate primitives (#2840)
* Plot: Improve `AutoScale()` customization using `Margins()` to define whitespace area (#2857)
* Primitives: Improved equality checks (#2855)
* Controls: Added a `RenderQueue` to allow cross-control render requests that would otherwise cause render artifacts or infinite loops
* Controls: Created `SharedAxisManager` and `SharedLayoutManager` to facilitate pairing controls together
* Multiplot: Added methods for creating creating static multi-plot figures (#2868, #2869)

## ScottPlot 4.1.67
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-08-13_
* DataLogger: Improved appearance of legend items (#2829, #2850) @KroMignon and @p4pravin
* Radial Gauge Plot: Improved layout for plots with a large number of gauges (#2722) @tinuskotze
* DataLogger: Added support for markers (#2862) @KroMignon
* AxisLimits: Added `WithPan()` overloads to facilitate panning in interactive applications (#2863) @LapinFou
* Rectangle: Plots now have an `AddRectangle()` for placing rectangular shapes on plots (#2866) @dpieve

## ScottPlot 5.0.7-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-08-06_
* Axis: Fixed issue where axes with zero span would cause renders to fail (#2714) @ahmad-qamar
* Avalonia: Improve support for cross-platform and non-desktop applications (#2748) @PremekTill
* Scatter Plot: Improve support for empty datasets (#2740)
* Scatter Plot: Improve support for user-defined line widths (#2739, #2750) @dayo05
* Fonts: New static class to facilitate selecting fonts that support international characters (#2746) @heartacker
* Axis: Exposed `TickFont` to allow tick label size and style customization (#2747) @heartacker
* Plot: Added `Title()`, `XLabel()`, and `YLabel()` helper methods
* Fonts: Favor the system default font to achieve better support international characters (#2746) @heartacker
* Plot: Added `ScaleFactor` property to manage scaling of all plot components (#2747) @heartacker
* WinForms: Automatically adjust plot scaling to match display scaling (#2747) @heartacker
* Plot: Added a `RenderManager` which has a `List<RenderAction>` the user can modify to customize the render sequence (#2767)
* Plot: Refactored rendering system for all plottables, axes, etc. so canvases (not surfaces) are passed (#2767)
* WPF: Improved support for display scaling (#2760, #2766) @DmitryKotenev
* Plot: Added support for SVG export (#2704, #717)
* Legend: Respect `IsVisible` property (#2805)
* Ticks: Added `NumericManual` tick generator for manually-defined tick positions and labels
* Plot: `Title()`, `XLabel()`, and `YLabel()` have optional arguments for `size`
* Plot: Added `Plot.Style.SetFont()` to apply the given font to all titles, axis labels, and tick labels
* Plot: Added `Plot.Style.SetFontFromText()` to apply system font that best supports the language of the provided text (#2746) @heartacker
* RandomDataGenerator: Improved XML docs and added methods for returning single numbers (#2774, #2787) @Silent0Wings
* Pixel: Added constructor overload that accepts `double` values (#2780) @Silent0Wings
* Primitives: Refactored and added XML docs to `Pixel`, `PixelSize`, and `PixelRect` (#2784)
* Color: Added `WithAlpha()` method that accepts a fraction (#2794, #2776) @mjpz
* Coordinates: Added `Distance()` method for calculating distance between two points in axis space (#2791, #2798) @able-j
* CoordinateRect: Added a `Center` property that returns a `Coordinates` value in axis space (#2789, #2812) @tijin-abe-thomas
* CoordinateRect: Added a `Contains()` method to evaluate whether given `Coordinates` are inside the rectangle (#2790, #2813) @tijin-abe-thomas
* Crosshair: New plot type that draws a cross centered at a given position in X/Y space
* Avalonia: Support Avalonia version 11.0.1 (#2822) @oktrue
* Controls: Now have `GetCoordinates()` with built-in logic for display scaling compensation (#2760)
* Rendering: Improve multi-platform color support (#2818) @KroMignon and @oktrue
* Random Data Generation: Added an optional `slope` argument to `RandomWalk()` (#2763, #2826) @JasonC0x0D
* Browser: Improved support and documentation for running ScottPlot in the browser with Avalonia and WinUI (#2830) @oktrue
* Android: Improved support and documentation for running ScottPlot in Avalonia Android projects (#2830) @oktrue
* Legend: Now hidden by default with opt-in visibility by calling `Plot.Legend()` (#2764)
* Style: `Plot.Axes` has been renamed to `Plot.AxisStyler` to better communicate its purpose (#2778)
* Primitives: Created `ExpandingAxisLimits` helper class for creating `AxisLimits` inside plottables (#2799)
* Plot: Added `Pan()` and `Zoom()` methods that do not require passing state like `MousePan()` and `MouseZoom()` do (#2800)
* Plot: Added `Plot.RenderManager.RenderFinished` event that provides a `RenderDetails` indicating whether axes or layout changed (#2801)
* FormsPlot: Added `RefreshQueue()` to allow facilitate event-driven refreshing of multiple controls in single-thread applications (#2801, #2802)
* Plot: Added `MatchAxisLimits()` to simplify applying limits from one plot to another in multi-control applications (#2802)
* Layout: Added `MatchLayout()` and `FixedLayout()` as an alternative to the default `AutomaticLayout()` engine (#2802)

## ScottPlot 4.1.66
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-08-06_
* DataLogger: Improved support for single-point datasets (#2733) @KroMignon
* Plot: Added optional arguments to `AddDataLogger()` and `AddDataStreamer()` for customizing style (#2733) @KroMignon
* Version: Build information can now be accessed from the static `ScottPlot.Version` class
* Avalonia: Removed dependency on `Avalonia.Desktop` package (#2752, #2748) @Fruchtzwerg94
* Cookbook: Remove "experimental" designator from ScatterPlotList (#2782) @prime167
* Heatmap: Added `Rotation` and `CenterOfRotation` properties (#2814, #2815) @bukkideme
* WPF: Improved the `PlottableDragged` event (#2820) @tadmccorkle
* Avalonia: Support Avalonia version 11.0.1 (#2822) @oktrue
* Heatmap: Improved XML docs (#2738, #2827) @JasonC0x0D

## ScottPlot 5.0.6-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-07-09_
* Legend: Improve support for custom positioning (#2584, #2638) @heartacker
* OpenGL: Use CPU to render on devices without hardware acceleration (#2651) @StendProg
* Polygon: New plot type for displaying closed shapes with arbitrary X/Y corners (#2696) @Tilation
* FillY: New plot type for displaying a shaded area between two sets of Y points that share the same X points (#2696) @Tilation
* Avalonia: Added support for Avalonia 11 (#2720, #2184, #2664, #2507, #2321, #2184, #2183, #2725) @Fruchtzwerg94, @Xerxes004, and @bclehmann

## ScottPlot 4.1.65
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-07-09_
* Axis: Improved log-scaled axis minor tick density default value and customization (#2646) @hellfo
* Image: Added option to disable anti-aliasing for scaled images (#2649) @mYcheng-95
* Binned Histogram: New plot type that represents binned 2D histogram data as a heatmap (#2453)
* DataLogger: New type of scatter plot designed for infinitely growing X/Y datasets (#2377, #2641)
* DataStreamer: New type of signal plot for displaying live data as it is shifted in (#2377, #2641)
* WPF: Multi-target Framework 4.6.1 changed to 4.6.2 (#2685)
* Axis: Added option to customize tick line width (#2643, #2654) @Guillaume-Deville
* Horizontal Span: Fixed `ToString()` message @RachamimYaakobov
* Signal Plot: Added `ScaleY` property to compliment `OffsetY` (#2642, #2656) @Guillaume-Deville
* Colorbar: Automatically adjust label position to prevent overlap with tick labels (#2684) @bukkideme
* Launcher: Made `Plot.Launch` methods available without requiring using statements (#2627, #2657) @Guillaume-Deville
* Population plot: Added `BoxBorderColor` and `ErrorStDevBarColor` properties to customize appearance (#2708) @johndoh
* Arrow: Made tip and base positions mutable (#2673) @MyZQL
* ScatterPlotList: Add `GetXs()` and `GetYs()` to let users retrieve copies of data points (#2694, #2711) @bukkideme and @Marc-Frank
* FormsPlotViewer: New constructor for synchronized plots with bidirectional updates (#2653, #2710, #2722) @bukkideme
* LineStyle: Default patterns (and a new custom pattern) can be customized by assigning `ScottPlot.LineStylePatterns` (#2690, #2692) @mocakturk, @Marc-Frank, and @bukkideme
* Radar Plots: Improve vertical spacing for all aspect ratios (#2702) @pjt33
* Avalonia: Added support for Avalonia 11 (#2720, #2184, #2664, #2507, #2321, #2184, #2183, #2725) @Fruchtzwerg94, @Xerxes004, and @bclehmann
* Colorbar: Added a `ResizeLayout()` helper method for adjusting plot layouts to accommodate large tick labels (#2703)
* Scatter List: Improved support for data containing NaN values (#2707) @oldteacup
* Population Plot: Improved support for populations with no data (#2727, #2726) @marklam

## ScottPlot 5.0.5-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-05-17_
* Box Plot: New plot type for displaying multiple collections of population data (#2589) @bclehmann
* OpenGL Control: Prevent exceptions on keyboard input (#2609, #2616) @stendprog
* Platforms: Improved linux support by using SkiaSharp native assets without dependencies (#2607) @chrisxfire
* Color: Improved support for alpha values in constructor (#2625)Clay

## ScottPlot 4.1.64
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-05-17_
* Ellipse: Added `Rotation` property (#2588, #2595) @JohniMIEP and @bclehmann
* Controls: Prevent horizontal scroll wheel events from throwing exceptions (#2600, #2626, #2630) @bclehmann, @szescxz, and @Jordant190
* ScatterDataLogger: Experimental plot type for live incoming data (#2377, #2599)
* Ticks: Improved automatic layout sizing when manual ticks are used (#2603, #2605) @StefanBertels and @szescxz
* Ticks: Improved automatic layout sizing for short and empty tick labels (#2606) @szescxz
* Plot: Improved `AddVerticalLine()` XML docs (#2610) @wfs1900
* FinancePlot: `GetBollingerBands()` now accepts an optional standard deviation coefficient (#2594) @Minu476
* SignalPlot: Fixed bug where `Update()` did not change the final point (#2592) @Angeld10
* ScatterPlotDraggable: Expose IndexUnderMouse for access after drag events (#2682) @mocakturk

## ScottPlot 5.0.4-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-04-09_
* WpfPlot: Converted the `UserControl` to a `CustomControl` to facilitate inheritance and theming (#2565) @KroMignon
* Controls: Improved ALT + left-click-drag zoom rectangle behavior (#2566)

## ScottPlot 4.1.63
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-04-09_
* WpfPlot: Converted the `UserControl` to a `CustomControl` to facilitate inheritance and theming (#2509, #2526) @KroMignon
* Lollipop and Cleveland plots: Added `LineWidth` property (#2556) @benton-anderson
* Pie: Added `SliceLabelPosition` property to allow slice labels to be placed outside the pie (#2515, #2510, #2275) @nuelle16 and @cpa-level-it
* Axis: Made `Edge` and `AxisIndex` immutable to prevent accidental modification after construction (#2539, #2538) @cxjcqu
* Plot: Created `LeftAxis`, `RightAxis`, `BottomAxis`, and `TopAxis` which alias `YAxis`, `YAxis2`, `XAxis`, and `XAxis2` but are more expressive (#2568)
* Plot: `Launch` property has methods for launching the plot as a static image, refreshing web page, or interactive window (#2543, #2570)
* Heatmap: Improved support for semitransparent cells (#2313, #2277, #2285, #2461, #2484) @bukkideme
* Axis: Added `SetZoomInLimit()`, `SetZoomOutLimit()`, and `SetBoundary()` to control zoom and pan (#2250, #2291, #1997, #1873, #662) @dusko23, @Gholamalih, and @bclehmann
* Controls: Added `Configuration.RightClickDragZoomFromMouseDown` flag to enable right-click-drag zoom to scale relative to the cursor (#2296, #2573) @pavlexander
* Finance: Improved DateTime position of random stock price sample data (#2574)
* Axis: Improve tick spacing for extremely small plots (#2289) @Xerxes004
* Signal: Fixed bug causing `Update()` to throw an indexing error (#2578) @Angeld10
* Annotation: Position is no longer defined as `X` and `Y` but instead `Alignment`, `MarginX`, and `MarginY` (#2302) @EFeru
* Colorbar: Add `Label` property (#2341) @bukkideme

## ScottPlot 5.0.4-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-04-02_
* OpenGL: Enhanced customization options for OpenGL-accelerated scatter plots (#2446) @StendProg
* Data: Added axis limit caching functionality for improved performance of large scatter plots (#2460) @StendProg
* DataOperations: New static class with helper methods for working with 1D and 2D data (#2497) @bukkideme and @StendProg
* Financial: Created `IOHLC` to allow users to inject their own pricing logic (#2404) @mjpz
* Solution: Fixed configuration error caused by invalid GUIDs (#2525) @KroMignon
* Controls: Disabled context menu in non-interactive mode (#2475) @KroMignon
* Histogram: Improved constructor argument validation and support for small bins(#2490) @Margulieuxd and @bukkideme
* WpfPlot: Control now appears in the Visual Studio Toolbox (#2535, #1966)Valkyre
* Axis: Improved tick label format customization (#2500) @chhh

## ScottPlot 4.1.62
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-04-02_
* WinForms Control: `Reset()` makes new plots transparent (#2445) @Neopentane1
* Ellipse and Circle: New plot types demonstrated in the cookbook. (#2413, #2437) @bukkideme
* Heatmap: Added `FlipVertically` to invert vertical axis of heatmap data (#2444, #2450) @Neopentane1
* Histogram: Improved support for datasets with low variance (#2464, #2463) @Xerxes004
* Heatmap: Added `Opacity` property (#2461, #2484) @bukkideme
* DataOperations: New static class with helper methods for working with 1D and 2D data (#2497) @bukkideme and @StendProg
* Population: Added option for customizing horizontal errorbar alignment (#2502) @benton-anderson
* Financial: Created `IOHLC` to allow users to inject their own pricing logic (#2404) @mjpz
* OHLC: The `Volume` property and constructor overload initializing it have been deprecated (#2404)
* Axis: Expose tick, spine, and label configuration objects (#2512, #2513, #2353) @cxjcqu and @SaltyTears
* Signal: Improved `FillDisable()` behavior (#2436) @szescxz
* RadialGaugePlot: Improve alignment for plots with 1-3 gauges (#2448, #2128) @DavidWhataGIT, @johndoh, and daddydavid
* Pie: Added `LegendLabels` property so slices and legend items can have different labels (#2459) @vietanhbui
* Controls: Improved `GetCoordinate()` behavior for empty plots (#2468, #2540) @dusko23
* Histogram: Improved constructor argument validation and support for small bins(#2490) @Margulieuxd and @bukkideme
* Axis: Improved `Plot.AxisPanCenter()` support for multi-axis plots (#2483, #2544) @dusko23
* Bubble Plot: Added `RadiusIsPixels` flag which when `falst` sizes bubbles using radius units instead of pixels (#2492) @marcelpel
* Axis: Improved `Plot.MatchAxisLimits()` support for multi-axis plots (#2495) @Margulieuxd
* Plot: Improved `Plot.XLabel()` XML documentation (#2552) @JulianusIV

## ScottPlot 5.0.2-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-02-27_
* Signal Plot: Support X and Y offset (#2378) @minjjKang
* WebAssembly: New sandbox demonstrates interactive ScottPlot in a browser (#2380, #2374)rafntor
* OpenGL: Added experimental support for direct GPU rendering (#2383, #2397) @StendProg
* Finance Plots: Added OHLC and Candlestick plot types (#2386) @bclehmann
* Style: Improved `Plot.Style.Background()` color configuration (#2398) @Jonathanio123
* WPF: Added OpenGL support to the WPF control (#2395) @StendProg
* Palette: Refactored the palette system to allow ScottPlot 4 and 5 to share palette code (#2409)
* Plot: Added `GetImageHTML()` for improved rendering in interactive notebooks (#2385, #1772) @neilyoung2008

## ScottPlot 4.1.61
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-02-27_
* Axis: Throw exception immediately upon setting invalid axis limits (#2327) @mjpz
* Heatmap: Added support for transparent single-color heatmaps (#2336) @bukkideme
* Statistics: Improved median calculation method in population plots (#2363) @Syntaxrabbit
* AxisLineVector: Improved automatic axis limits when using limited axis lines (#2371) @ChrisAtVault
* Controls: `Configuration.AddLinkedControl()` simplifies axis sharing across multiple controls (#2402, #2372)
* Statistics: New `ScottPlot.Statistics.Histogram` class optimized for simplicity and live data (#2403, #2389) @bukkideme and @Xerxes004
* Statistics: Improved bin edge calculations for histograms with fixed bin size bins (#2299) @Xerxes004
* Palette: Refactored the palette system to allow ScottPlot 4 and 5 to share palette code (#2409)
* Heatmap: Added `GetBitmap()` to provide access to raw heatmap image data (#2396, #2410) @bukkideme
* Pie: Prevent invalid argument exceptions when drawing zero-size pie charts (#2415) @KC7465128305
* Colormap: Colormaps can be created from a set of colors (#2375, #2191, #2187) @dhgigisoave
* Function Plot: New optional `AxisLimits` allows users to define default axis limits (#2428, #2412) @bukkideme
* Population: Fixed bug causing argument exceptions for 1px high plots (#2429, #2384) @Sprenk
* Controls: Added `Configuration.AltLeftClickDragZoom` option to customize zooming behavior (#2391, #2392) @DevJins
* Error Bar: Added `Label` property which allows error bars to appear independently in the legend (#2432, #2388) @dongyi-cai-windsab
* Demo: Fixed bug preventing the cookbook from launching (#2443) @FannyAtGitHub

## ScottPlot 5.0.1-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-02-09_
* Namespace: DataSource → DataSources
* Error Bar: New plot type (#2346) @bclehmann
* Plot: Added `Style` object to group functions that perform complex styling tasks
* Controls: Added right-click context menus (#2350) @bclehmann
* Rendering: Added support for saving bitmap files (#2350) @bclehmann
* Axes: Added support for DateTime Axes (#2369) @bclehmann
* Rendering: Added support for line styles (#2373) @bclehmann
* WinUI3: Created a Uno WinUI3 control (#2374, #2039) @rafntor

## ScottPlot 5.0.0-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2023-01-01_
* ScottPlot 5: First version 5 release published to NuGet #2304

## ScottPlot 4.1.60
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-12-23_
* Pie Chart: Improved display when a single slice covers the entire pie (#2248, #2251) @bclehmann
* Plot: Added `AddFill()` arguments for `LineWidth` and `LineColor` (#2258) @Fruchtzwerg94
* Plot: Improved support for filled polygons with fewer than 3 points (#2258) @Fruchtzwerg94
* A new `IDraggableSpan` interface was added to trigger events when the edges of spans are dragged (#2268) @StendProg
* Palettes: Added new light-color palettes PastelWheel, LightSpectrum, and LightOcean (#2271, #2272, #2273) @arthurits
* Ticks: Improved tick calculations for very small plots (#2280, #2278) @Xerxes004
* Crosshair: HLine and VLine are no longer readonly (#2208) @arthurits
* Function Plot: Added support for filling above and below lines (#2239, #2238) @SGanard
* Signal Plot: Improved error messages for when `Update()` fails to replace data (#2263)
* Plot: `Clear()` now resets inner and outer view limits (#2264) @vietanhbui
* FormsPlot: Right-click help menu is now `TopMost` (#2282) @dusko23
* Signal Plot: Allow users to apply different colors to lines and markers (#2288) @Nuliax7
* Pie: Added `Size` option to allow customizing how large the pie chart is (#2317) @Rudde
* FormsPlot: Improved support for horizontal legends in the pop-out legend viewer (#2300) @rotger
* Axis: Added arguments to `AxisPan()` to improve multi-axis support (#2293)
* Axis: Added `AxisPanCenter()` to center the view on a coordinate (#2293) @dusko23
* Project: Use System.Drawing.Common version 4.7.2 to avoid CVE-2021-26701 (#2303, #1004, #1413) @gobikulandaisamy

## ScottPlot 4.1.59
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-11-06_
* Ticks: Improve datetime tick labels for systems with a 24-hour display format (#2132, #2135) @MareMare and @bclehmann
* Axis: `Plot.AddAxis()` now uses auto-incremented axis index unless one is explicitly defined (#2133) @bclehmann and Discord/Nick
* Axis: `Plot.GetAxesMatching()` was created to obtain a given vertical or horizontal axis (#2133) @bclehmann and Discord/Nick
* Axis: Corner label format can be customized for any axis by calling `CornerLabelFormat()` (#2134) @ShannonZ
* BarSeries: Improved rendering of negative values (#2147, #2152) @fe-c
* Function Plot: Added optional `XMin` and `XMax` fields which limit function rendering to a defined horizontal span (#2158, #2156, #2138) @bclehmann and @phil100vol
* FormsPlot: Plot viewer now has `RefreshLegendImage()` allowing the pop-out legend to be redrawn programmatically (#2157, #2153) @rosdyana
* Function Plot: Improved performance for functions which return null (#2158, #2156, #2138) @bclehmann
* BarSeries: improve support for negative and horizontal bar labels (#2148, #2159, #2152) @bclehmann
* Palette: Added `Normal` Palette based on [Anton Tsitsulin's Normal 6-color palette](http://tsitsul.in/blog/coloropt/) (#2161, #2010) @martinkleppe
* BarSeries: Added helper function to create a bar series from an array of values (#2161) @KonH
* SignalPlot: Add `Smooth` option (#2174, #2137) @rosdyana
* Signal Plot: Use correct marker when displaying in legend (#2172, #2173) @bclehmann
* Data Generation: Improved floating point precision of `RandomNormalValue` randomness (#2189, #2206) @arthurits and @bclehmann_
* Finance Plot: Improved SMA calculations for charts with unordered candlesticks (#2199, #2207) @zachesposito and @xenedia
* Avalonia Control: Fixed subscription to ContexMenu property changes (#2215) @DmitryZhelnin
* Legend: Support horizontal orientation and added cookbook example (#2216) @lucabat
* Data Generation: Added generic support for `Consecutive()`, `Random()`, and `RandomWalk()`
* SignalPlot: New `SignalPlotGeneric` type allows `AddSignal()` to support generic data types (#2217) @codecrafty

## ScottPlot 4.1.58
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-09-08_
* Radar: New `Smooth` field allows radar areas to be drawn with smooth lines (#2067, #2065) @theelderwand
* Ticks: Setting manual ticks will now throw an immediate `ArgumentException` if positions and labels have different lengths (#2063) @sergaent
* VectorFieldList: New plot type for plotting arbitrary coordinate/vector pairs which are not confined to a grid (#2064, #2079) @sjdemoor and @bclehmann
* HLine and VLine: Line (but not position label) is hidden if `LineWidth` is `0` (#2085) @A1145681
* Controls: The cursor now reverts to `Configuration.DefaultCursor` after moving off draggable objects (#2091) @kurupt44
* Snapping: SnapNearest classes now expose `SnapIndex()` (#2099) @BambOoxX
* Background: Added optional arguments to `Style()` lets users place a custom background image behind their plot (#2016) @apaaris
* Axis Line: Remove the ability to drag invisible lines (#2110) @A1145681
* Controls: Draggable objects can now only be dragged with the left mouse button (#2111, #2120) @A1145681
* Heatmap: Prevent rendering artifacts by throwing an exception if the 2D array is larger than 2^15 in either dimension (#2119, #2116) @dhgigisoave

## ScottPlot 4.1.57
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-08-18_
* Scatter: Improved `GetPointNearest()` when `OnNaN` is `Gap` or `Ignore` (#2048) @thopri
* Heatmap and Image: Added `Coordinate[] ClippingPoints` to give users the ability to clip to an arbitrary polygon (#2049, #2052) @xichaoqiang
* Image: Improved automatic axis limits measurement when `HeightInAxisUnits` is defined
* Plot: Reduced anti-aliasing artifacts at the edge of frameless plots (#2051)

## ScottPlot 4.1.56
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-08-16_
* Signal: Improved accuracy of `GetIndexForX()` (#2044) @CharlesMauldin
* Palette: Added help messages for users attempting to create custom palettes (#1966) @EFeru

## ScottPlot 4.1.55
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-08-14_
* Scatter: Data may now contain NaN if the `OnNaN` field is customized. `Throw` throws an exception of NaN is detected (default behavior), `Ignore` skips over NaN values (connecting adjacent points with a line), and `Gap` breaks the line so NaN values appear as gaps. (#2040, #2041)
* Plot: Added a `AddFillError()` helper method to create a shaded error polygon for displaying beneath a scatter plot (#2037)

## ScottPlot 4.1.53
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-08-11_
* Scatter and Signal Plot: `GetYDataRange()` now returns the range of Y values between a range of X positions, useful for setting automatic axis limits when plots are zoomed-in (#1946, #1942, #1929) @bclehmann
* WPF Control: Right-click copy now renders high quality image to the clipboard (#1952) @bclehmann
* Radar, Coxcomb, and Pie Chart: New options to customize hatch pattern and color. See cookbook for examples. (#1948, #1943) @bclehmann
* Signal Plot: Improve support for plots with a single point (#1951, #1949) @bclehmann and @Fruchtzwerg94
* Draggable Marker Plots: Improved drag behavior when drag limits are in use (#1970) @xmln17
* Signal Plot: Added support for plotting `byte` arrays (#1945)
* Axis Line: Added properties to customize alignment of position labels (#1972) @hamhub7
* Plot: MatchAxis no longer modifies limits of unintended axes (#1980) @PlayCreatively
* Plot: Improved error reporting for invalid axis limits (#1994) @Xerxes004
* Signal Plot: Improved `GetPointNearestX()` accuracy for plots with high zoom (#1987, #2019, #2020) @dhgigisoave
* Draggable: `IDraggable` now has functions to facilitate snapping (#2006, #2007, #2022) @Agorath
* Palette: `ScottPlot.Palette` has been refactored to replace `ScottPlot.Drawing.Palette` and `ScottPlot.Drawing.Colorset` (#2024)
* Palette: Palettes now implement `IEnumerable` and colors can be retrieved using `foreach` (#2028)
* Render: Improved thread safety of the render lock system (#2030) @anprevost
* Scatter: Exposed `SmoothTension` to customize behavior when `Smooth` is enabled (#1878) @Michael99

## ScottPlot 4.1.52
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-07-09_
* WinForms control: Fixed a bug introduced by the previous version which resulted in flickering while using the mouse to pan or zoom (#1938, #1913) @AbeniMatteo
* Plot: Added obsolete `GetLegendBitmap()` with message indicating `RenderLegend()` is to be used instead (#1937, #1936) @johnfoll
* Signal Plot: Improved performance using platform-specific fast paths for common data types to minimize allocations (#1927) @AbeniMatteo, @StendProg, and @bclehmann

## ScottPlot 4.1.51
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-06-30_
* WinForms Control: Fixed a bug that caused frequent mouse events to overflow the stack (#1906, #1913) @AbeniMatteo
* Performance: Improve string measurement performance using cached fonts (#1915) @AbeniMatteo
* Layout: Improve axis alignment when `ManualDataArea()` is used (#1901, #1907, #1911) @dhgigisoave
* Cookbook: Improve error message if recipes.json is not found (#1917) @AbeniMatteo

## ScottPlot 4.1.50
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-06-26_
* BarSeries: Lists passed into new BarSeries are preserved and can be modified after instantiation. Added a `Count` property. Added a `AddBarSeries()` overload that permits creating an empty BarSeries. (#1902)
* Markers: Improved performance for plot types that render multiple markers (#1910) @AbeniMatteo
* Plot: New `ManualDataArea()` function allows users to define pixel-perfect layouts (#1907, #1901) @dhgigisoave

## ScottPlot 4.1.49
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-06-21_
* BarSeries: A new type of bar plot which allows each bar to be individually customized and offers mouse collision detection (#1891, #1749) @jhm-ciberman
* SignalXY: When step mode is activated markers are now only drawn at original data points (#1896) @grabul
* SignalConst: Fixed indexing error affecting the Update() overload that accepted generic arrays (#1895, #1893) @strontiumpku
* Scatter and Signal: When `StepDisplay` is enabled, the new `StepDisplayRight` property can toggle step orientation (#1894, #1811) @dhgigisoave
* SignalXY: Markers now shown in legend when the plot is zoomed-in enough that they become visible on the plot itself

## ScottPlot 4.1.48
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-06-09_
* Plottable: Collapsed `IHasAxisLimits`, `IHasDataValidation`, and `IHasLegendItems` back into `IPlottable`, reverting a change introduced by the previous version. The intent of the original change was to promote interface segregation (e.g., colorbar has no axis limits). However, the purpose of this reversion is to maintain consistent behavior for users who implemented their own plottables implementing `IPlottable` and may not be aware of these new interfaces. (#1868, #1881)

## ScottPlot 4.1.47
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-06-07_
* Scatter Plot: New `Smooth` property allows data points to be connected by smooth lines (#1852, #1853) @liuhongran626
* Axis: Improved corner notation for multi-axis plots (#1875) @nassaleh
* Plottable: Optional segregated interfaces `IHasAxisLimits`, `IHasDataValidation`, and `IHasLegendItems` were broken-out of `IPlottable`. Note that this change was reverted in the subsequent release. (#1868, #1881)

## ScottPlot 4.1.46
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-06-05_
* Image: `AddImage()` has optional arguments to define rotation, scale, and anchor alignment. The `Image` plot type has new public properties allowing images to be stretched so position and size can be defined using axis units (see Cookbook). `Rotation` now respects all anchor alignments. (#1847) @wtywtykk and @bclehmann
* Bracket: New plot type to highlight a range of data between two points in coordinate space (#1863) @bclehmann
* Heatmap: Added `FlipVertically` property to invert orientation of original data (#1866, #1864) @bclehmann and @vtozarks
* Axis: Improved placement of horizontal axis tick labels when multiple axes are in use (#1861, #1848) @bclehmann and @Shengcancheng
* Crosshair: Now included in automatic axis limit detection. Use its `IgnoreAxisAuto` property to disable this functionality. (#1855, #1857) @CarloToso and @bclehmann
* BarPlot: Improved automatic axis detection for bar plots containing negative values (#1855, #1857) @CarloToso and @bclehmann
* IHittable: new interface to facilitate mouse click and hover hit detection (#1845) @StendProg and @bclehmann
* Tooltip: Added logic to enable detection of mouse hover or click (#1843, #1844, #1845) @kkaiser41, @bclehmann, and @StendProg
* Controls: All user controls now have a `LeftClickedPlottable` event that fires when a plottable implementing `IHittable` was left-clicked
* FormsPlot: Set `Configuration.EnablePlotObjectEditor` to `true` to allow users to launch a plot object property editor from the right-click menu (#1842, #1831) @bradmartin333 and @BambOoxX
* BarPlot: Fixed bug where zooming extremely far in would cause large fills to disappear (#1849, #1850) @ChrisAtVault

## ScottPlot 4.1.45
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-05-05_
* Plottables: Fields converted to properties and setters paired with getters to facilitate binding (#1831) @bradmartin333

## ScottPlot 4.1.44
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-05-05_
* SignalXY: Permit duplicate X values and improve exception messages when invalid X data is loaded (#1832) @Niravk1997

## ScottPlot 4.1.43
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-05-02_
* Draggable Scatter Plot: Fixed a bug where horizontal drag limits were applied to the vertical axis (#1795) @m4se
* Plot: Improved support for user-defined ticks when inverted axis mode is enabled (#1826, #1814) @Xerxes004
* Heatmap: Added `GetCellIndexes()` to return the heatmap data position for a given coordinate (#1822, #1787) @tonpimenta
* Controls: Added `LeftClicked` event to customize left-click actions in GUI environments (#1822, #1787)

## ScottPlot 4.1.42
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-05-01_
* SignalXY: Fixed bug causing plots to disappear when displaying partial data containing duplicated X values. (#1803, #1806) @StendProg and @bernhardbreuss
* SignalXY: X data is no longer required to be ascending when it is first assigned, improving support for plots utilizing min/max render indexing (#1771, #1777) @bernhardbreuss
* Grid: Calling `Plot.Grid(onTop: true)` will cause grid lines to be drawn on top of plottables (#1780, #1779, #1773) @bclehmann and @KATAMANENI
* FormsPlot: Fixed a bug that caused the default right-click menu to throw an exception when certain types of plottables were present (#1791, #1794) @ShenxuanLi, @MareMare, and @StendProg
* Avalonia: Improved middle-click-drag zoom-rectangle behavior (#1807) @kivarsen
* Avalonia: Improved position of right-click menu (#1809) @kivarsen
* Avalonia: Added double-click support which displays benchmark information by default (#1810) @kivarsen
* Axis: Improved support for switching between custom tick label format strings and custom formatter functions (#1813) @schifazl
* Plot: `AutomaticTickPositions()` can now be used to undo action of `ManualTickPositions()` (#1814)
* Plot: `AutomaticTickPositions()` optionally accepts an array of ticks and labels that can be displayed in addition to the automatic ones (#1814) @Xerxes004
* Signal Plot: Improved low density display when `LineStyle` is `None` (#1797) @nassaleh
* FormsPlot: Detached legend now restores initial legend visibility state on close (#1804) @BambOoxX

## ScottPlot 4.1.41
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-04-09_
* Plot: Added `Plot.GetImageHTML()` to make it easy to display ScottPlot images in .NET Interactive / Jupyter notebooks (#1772) @StendProg and @Regenhardt

## ScottPlot 4.1.40
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-04-07_
* SignalPlotXY: Improved support for custom markers (#1763, #1764) @bclehmann and @ChrisCC6
* Legend: `Plot.Legend()` accepts a nullable `Location` so legends can be enabled/disabled without changing position (#1765) @envine
* FormsPlot: The right-click menu now shows "detach legend" even if all plottable items with legends are set to invisible (#1765) @envine
* AxisLine: Added a `PositionLabelAxis` field that can be used to define a specific axis to draw the position label on in multi-axis plots (#1766) @fuxinsen

## ScottPlot 4.1.39
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-04-01_
* SignalPlotXY: Fixed bug where `GetPointNearestX()` did not check proximity to the final point (#1757) @MareMare

## ScottPlot 4.1.38
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-03-31_
* Bar plot: Improved automatic axis limit detection for bars with negative offset (#1750) @painstgithub
* Axis labels: Added a `rotation` argument to `Axis.LabelStyle()` to support flipping label orientation (#1754, #1194) @zeticabrian

## ScottPlot 4.1.37
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-03-25_
* Controls: Improved multi-axis support for mouse tracking by giving `GetMouseCoordinates()` optional axis index arguments (#1743) @kv-gits

## ScottPlot 4.1.36
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-03-19_
* Axis: Allow grid line and tick mark pixel snapping to be disabled (#1721, #1722) @Xerxes004
* Axis: `ResetLayout()` sets padding to original values to reverse changes made by adding colorbars (#1732, #1736) @ccopsey

## ScottPlot 4.1.35
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-03-06_
* Eto.Forms: Improved handling of events (#1719, #1718) @rafntor and @VPKSoft

## ScottPlot 4.1.34
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-03-03_
* Bubble plot: Added methods to get the point nearest the cursor (#1657, #1652, #1705) @BambOoxX, @Maoyao233, and @adgriff2
* Markers: Improved alignment of markers and lines on Linux and MacOS by half a pixel (#1660, #340)
* Plottable: Added `IsHighlighted` properties to make some plot types bold (#1660) @BambOoxX
* Plottable: Segregated existing functionality interfaces for `IHasLine`, `IHasMarker`, and `IHilightable` (#1660) @BambOoxX
* Plot: `AxisAuto()` now throws an exception of margins are defined outside the allowable range (#450, #1682) @xichaoqiang
* Plot: Added `PlotFillRightLeft` method for adding horizontal filled scatter plots (#450) @xichaoqiang
* Markers: All shapes are now drawn discretely instead of relying on text rendering for improved performance and consistency (#1668, #1660) @BambOoxX
* Scatter Plot: Support distinct `LineColor` and `MarkerColor` colors (#1668)
* SignalXY: Fix bug affecting the edge of the plot when step mode is active (#1703, #1699) @PeppermintKing
* SignalXY: Improve appearance of filled regions when step mode is active (#1703, #1697) @PeppermintKing
* Axis Span: Added options to customize fill pattern and border (#1692) @BambOoxX
* Markers: Additional customization options such as `MarkerLineWidth` (#1690) @BambOoxX
* Legend Viewer: New functionality to customize line, marker, and highlight options have been added to the the right-click menu of the Windows Forms control (#1655, #1651) @BambOoxX

## ScottPlot 4.1.33
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-02-04_
* Spline Interpolation: Added new methods for data smoothing including Bézier interpolation (#1593, #1606)
* Detachable Legend: Added an option to detach the legend to the right-click menu in the Windows Forms control. Clicking items in the detached legend toggles their visibility on the plot (#1589, #1573, #1326) @BambOoxX
* Marker: Added an optional `Text` (and `TextFont`) for displaying a message that moves with a marker (#1599)
* Heatmap: Heatmaps with custom X and Y sizing or positioning no longer call `AxisScaleLock()` automatically (#1145) @bclehmann
* Axis: GetCoordinateY() now returns more accurate coordinate (#1625, #1616) @BambOoxX
* Text: Now has `IsDraggable` field and improved mouseover detection that supports rotation (#1616, #1599) @BambOoxX and @Niravk1997
* Plot: `Frameless()` no longer results in an image with a 3 pixel transparent border (#1571, #1605) @sjlai1993
* Colorbar: `AddColorbar()` has new optional argument to enable placement on the left side of the plot (#1524) @Niravk1997
* Heatmap: Fixed bug affecting manually-scaled heatmaps (#1485) @ZPYin, @mYcheng-95, and @bclehmann
* Colorbar: Exposed `DataAreaPadding` to improve layout customization for multi-axis plots (#1637) @ccopsey

## ScottPlot 4.1.32
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-01-23_
* Interpolation: New cubic interpolation module with improved stability and simplified API (#1433) @allopatin
* Legend: `GetBitmap()` returns a transparent image instead of throwing an exception if there are no items in the legend (#1578) @BambOoxX
* Legend: Added `Count`, `HasItems`, and `GetItems()` so users can inspect legend contents to if/how they want to display it (#1578) @BambOoxX
* Plot: Exposed `GetDraggable()` to allow users to retrieve the plotted objects at specific pixel positions (#1578) @BambOoxX
* Axis Limits: Improved handling of axis limits for plots containing no data (#1581) @EFeru
* Repeating Axis Line: Improved display of text labels (#1586, #1557) @BambOoxX
* Axis: Improved multi-axis support for `GetPixel()` methods (#1584, #1587) @ChrisCC6 and @BambOoxX
* Error Bar: `Plot.AddErrorBars()` can now be used to place 1D or 2D error bars anywhere on the plot (#1466, #1588) @bclehmann
* Scatter Plot List: Added generic support to `ScatterPlotList<T>` as demonstrated in the cookbook (#1463, #1592) @tyrbentsen
* Draggable Scatter Plot: Created a new `ScatterPlotListDraggable` that supports dragging points and custom clamp logic as seen in the cookbook (#1422) @EFeru and @BambOoxX
* Axis: Users may now customize the number of minor ticks and grid lines when log scale is enabled (#1594, #1595, #1583) @hibus

## ScottPlot 4.1.31
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-01-17_
* MultiAxis: Improved support for draggable items placed on non-primary axes (#1556, #1545) @BambOoxX
* RepeatingAxisLine: New plot types `RepeatingVLine` and `RepeatingHLine` show a primary line and a user-defined number of harmonics. See cookbook for example and usage notes. (#1535, #1775) @BambOoxX
* Scatter: The new `ScatterPlotDraggable` plot type is for creating scatter plots with mouse-draggable points (#1560, #1422) @BambOoxX and @EFeru
* Controls: Improved middle-click-drag zoom rectangle support for plots with multiple axes (#1559, #1537) @BambOoxX
* Marker: New plot types `DraggableMarkerPlot` and `DraggableMarkerPlotInVector` give users options to add mouse-interactive markers to plots (#1558) @BambOoxX
* Bar Plot: New `ValueFormatter` option allows users to customize the text displayed above bars (#1542) @jankri
* Plot: `Title()` now has additional arguments for customizing text above the plot (#1564)Hendri

## ScottPlot 4.1.30
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-01-15_
* Plot: Improve values returned by `GetDataLimits()` when axis lines and spans are in use (#1415, #1505, #1532) @EFeru
* Rendering: Revert default text hinting from ClearType back to AntiAliased to improve text appearance on transparent backgrounds. Users may call `ScottPlot.Drawing.GDI.ClearType(true)` to opt-in to ClearType rendering which is superior for most situations. (#1553, #1550, #1528) @r84r, @wangyexiang, @Elgot, @EFeru, and @saklanmazozgur

## ScottPlot 4.1.29
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-01-02_
* WinForms Control: Improve ClearType text rendering by no longer defaulting to a transparent control background color (#1496)

## ScottPlot 4.1.28
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2022-01-01_
* Eto Control: New ScottPlot control for the Eto GUI framework (#1425, #1438) @rafntor
* Radar Plot: `OutlineWidth` now allows customization of the line around radar plots (#1426, #1277) @Rayffer
* Ticks: Improved minor tick and minor grid line placement (#1420, #1421) @bclehmann and @at2software
* Palette: Added Amber and Nero palettes (#1411, #1412) @gauravagrwal
* Style: Hazel style (#1414) @gauravagrwal
* MarkerPlot: Improved data area clipping (#1423, #1459) @PremekTill, @lucabat, and @AndXaf
* MarkerPlot: Improved key in legend (#1459, #1454) @PremekTill and @Logicman111
* Style: Plottables that implement `IStylable` are now styled when `Plot.Style()` is called. Styles are now improved for `ScaleBar` and `Colorbar` plot types. (#1451, #1447) @diluculo
* Population plot: Population plots `DataFormat` now have a `DataFormat` member that displays individual data points on top of a bar graph representing their mean and variance (#1440) @Syntaxrabbit
* SignalXY: Fixed bug affecting filled plots with zero area (#1476, #1477) @chenxuuu
* Cookbook: Added example showing how to place markers colored according to a colormap displayed in a colorbar (#1461) @obnews
* Ticks: Added option to invert tick mark direction (#1489, #1475) @wangyexiang
* FormsPlot: Improved support for WinForms 6 (#1430, #1483) @SuperDaveOsbourne
* Axes: Fixed bug where `AxisAuto()` failed to adjust all axes in multi-axis plots (#1497) @Niravk1997
* Radial Gauge Plot: Fixed bug affecting rendering of extremely small gauge angles (#1492, #1474) @arthurits
* Text plot and arrow plot: Now have `PixelOffsetX` and `PixelOffsetY` to facilitate small adjustments at render time (#1392)
* Image: New `Scale` property allows customization of image size (#1406)
* Axis: `Plot.GetDataLimits()` returns the boundaries of all data from all visible plottables regardless of the current axis limits (#1415) @EFeru
* Rendering: Improved support for scaled plots when passing scale as a `Plot.Render()` argument (#1416) @Andreas
* Text: Improved support for rotated text and background fills using custom alignments (#1417, #1516) @riquich and @AndXaf
* Text: Added options for custom borders (#1417, #1516) @AndXaf and @MachineFossil
* Plot: New `RemoveAxis()` method allows users to remove axes placed by `AddAxis()` (#1458) @gobikulandaisamy
* Benchmark: `Plot.BenchmarkTimes()` now returns an array of recent frame render times (#1493, #1491) @anose001
* Ticks: Disabling log-scaled minor ticks now disables tick label integer rounding (#1419) @at2software
* Rendering: Improve appearance of text by defaulting to ClearType font rendering (#1496, #823) @Elgot

## ScottPlot 4.1.27
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-10-24_
* Colorbar: Exposed fields for additional tick line and tick label customization (#1360) @Maoyao233
* Plot: Improved `AxisAutoY()` margins (#1363) @Maoyao233
* Radar Plot: `LineWidth` may now be customized (#1277, #1369) @bclehmann
* Controls: Stretching due to display scaling can be disabled with `Configuration.DpiStretch` in WPF and Avalonia controls (#1352, #1364) @ktheijs and @bclehmann
* Axes: Improved support for log-distributed minor tick and grid lines (#1386, #1393) @at2software
* Axes: `GetTicks()` can be used to get the tick positions and labels from the previous render
* WPF Control: Improved responsiveness while dragging with the mouse to pan or zoom (#1387, #1388) @jbuckmccready
* Layout: `MatchLayout()` has improved alignment for plots containing colorbars (#1338, #1349, #1351) @dhgigisoave
* Axes: Added multi-axis support for `SetInnerViewLimits()` and `SetOuterViewLimits()` (#1357, #1361) @saroldhand
* Axes: Created simplified overloads for `AxisAuto()` and `Margins()` that lack multi-axis arguments (#1367) @cdytoby
* Signal Plot: `FillAbove()`, `FillBelow()`, and `FillAboveAndBelow()` methods have been added to simplify configuration and reduce run-time errors. Direct access to fill-related fields has been deprecated. (#1401)
* Plot: `AddFill()` now has an overload to fill between two Y curves with shared X values
* Palette: Made all `Palette` classes public (#1394) @Terebi42
* Colorbar: Added `AutomaticTicks()` to let the user further customize tick positions and labels (#1403, #1362) @bclehmann
* Heatmap: Improved support for automatic tick placement in colorbars (#1403, #1362)
* Heatmap: Added `XMin`, `XMax`, `YMin`, and `YMax` to help configure placement and edge alignment (#1405) @bclehmann
* Coordinated Heatmap: This plot type has been deprecated now that the special functionality it provided is present in the standard `Heatmap` (#1405)
* Marker: Created a new `Marker` class to simplify the marker API. Currently it is a pass-through for `MarkerShape` enumeration members.
* Plot: `AddMarker()` makes it easy to place a styled marker at an X/Y position on the plot. (#1391)
* Plottable: `AddPoint()` now returns a `MarkerPlot` rather than a `ScatterPlot` with a single point (#1407)
* Axis lines: Added `Min` and `Max` properties to terminate the line at a finite point (#1390, #1399) @bclehmann

## ScottPlot 4.1.26
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-10-12_
* SignalPlotYX: Improve support for step display (#1342) @EFeru
* Heatmap: Improve automatic axis limit detection (#1278) @bclehmann
* Plot: Added `Margins()` to set default margins to use when `AxisAuto()` is called without arguments (#1345)
* Heatmap: Deprecated `ShowAxisLabels` in favor of tight margins (see cookbook) (#1278) @bclehmann
* Histogram: Fixed bug affecting binning of values at the upper edge of the final bin (#1348, #1350) @jw-suh
* NuGet: Packages have improved debug experience with SourceLink and snupkg format symbols (#1285)

## ScottPlot 4.1.25
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-10-06_
* Palette: `ScottPlot.Palette` has been created and cookbook recipes have been updated to use it. The module it replaces (`ScottPlot.Drawing.Palette`) will not be marked obsolete until ScottPlot 5. (#1299, #1304)
* Style: Refactored to use static classes instead of enumeration members (#1299, #1291)
* NuGet: Improved System.Drawing.Common dependencies in user control packages (#1311, #1310) @Kritner
* Avalonia Control: Now targets .NET 5 (#1306, #1309) @bclehmann
* Plot: Fixed bug causing `GetPixel()` to return incorrect values for some axes (#1329, #1330) @riquich
* New Palettes:
  * `ColorblindFriendly` modeled after [Wong 2011](https://www.nature.com/articles/nmeth.1618.pdf) (#1312) @arthurits
  * `Dark` (#1313) @arthurits
  * `DarkPastel` (#1314) @arthurits
  * `Redness` (#1322) @wbalbo
  * `SummerSplash (#1317)` @KanishkKhurana
  * `Tsitsulin` 25-color optimal qualitative palette ([described here](http://tsitsul.in/blog/coloropt)) by [Anton Tsitsulin](http://tsitsul.in) (#1318) @arthurits and @xgfs
* New Styles:
  * `Burgundy` (#1319) @arthurits
  * `Earth` (#1320) @martinkleppe
  * `Pink` (#1234) @nanrod

## ScottPlot 4.1.23
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-09-26_
* NuGet: use deterministic builds, add source link support, and include compiler flags (#1285)

## ScottPlot 4.1.22
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-09-26_
* Coxcomb Plots: Added support for image labels (#1265, #1275) @Rayffer
* Palette: Added overloads for `GetColor()` and `GetColors()` to support transparency
* Plot Viewer: fixed bug causing render warning to appear in WinForms and Avalonia plot viewers (#1265, #1238) @bukkideme, @Nexus452, and @bclehmann

## ScottPlot 4.1.21
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-09-18_
* Legend: Throw an exception if `RenderLegend()` is called on a plot with no labeled plottables (#1257)
* Radar: Improved support for category labels. (#1261, #1262) @Rayffer
* Controls: Now have a `Refresh()` method as an alias of `Render()` for manually redrawing the plot and updating the image on the screen. Using `Render()` in user controls is more similar to similar plotting libraries and less likely to be confused with `Plot.Render()` in documentation and warning messages. (#1264, #1270, #1263, #1245, #1165)
* Controls: Decreased visibility of the render warning (introduced in ScottPlot 4.1.19) by allowing it only to appear when the debugger is attached (#1165, #1264)
* Radial Gaugue Plot: Fixed divide-by-zero bug affecting normalized gauges (#1272) @arthurits

## ScottPlot 4.1.20
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-09-09_
* Ticks: Fixed bug where corner labels would not render when multiplier or offset notation is in use (#1252, #1253) @DavidBergstromSWE

## ScottPlot 4.1.19
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-09-08_
* Controls: Fixed bug where render warning message is not hidden if `RenderRequest()` is called (#1165) @gigios

## ScottPlot 4.1.18
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-09-08_
* Ticks: Improve placement when axis scale lock is enabled (#1229, #1197)
* Plot: `SetViewLimits()` replaced by `SetOuterViewLimits()` and `SetInnerViewLimits()` (#1197) @noob765
* Plot: `EqualScaleMode` (an enumeration accepted by `AxisScaleLock()`) now has `PreserveSmallest` and `PreserveLargest` members to indicate which axis to prioritize when adjusting zoom level. The new default is `PreserveSmallest` which prevents data from falling off the edge of the plot when resizing. (#1197) @noob765
* Axis: Improved alignment of 90º rotated ticks (#1194, #1201) @gigios
* Controls: Fix bug where middle-click-drag zoom rectangle would persist if combined with scroll wheel events (#1226) @Elgot
* Scatter Plot: Fixed bug affecting plots where `YError` is set but `XError` is not (#1237, #1238) @simmdan
* Palette: Added `Microcharts` colorset (#1235) @arthurits
* SignalPlotXY: Added support for `FillType` (#1232) @ddrrrr
* Arrow: New plot type for rendering arrows on plots. Arrowhead functionality of scatter plots has been deprecated. (#1241, #1240)
* Controls: Automatic rendering has been deprecated. Users must call Render() manually at least once. (#1165, #1117)
* Radial Gauge Plots: `AddRadialGauge()` now adds a radial gauge plot (a new circular plot type where values are represented as arcs spanning a curve). See cookbook for examples and documentation. (#1242) @arthurits

## ScottPlot 4.1.17
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-08-25_
* Improved `RadarPlot.Update()` default arguments (#1097) @arthurits
* Radar Plot: Improved `Update()` default arguments (#1097) @arthurits
* Crosshair: Added `XLabelOnTop` and `YLabelOnRight` options to improve multi-axis support and label customization (#1147) @rutkowskit
* Signal Plot: Added `StepDisplay` option to render signal plots as step plots when zoomed in (#1092, #1128) @EFeru
* Testing: Improved error reporting on failed XML documentation tests (#1127) @StendProg
* Histogram: Marked `ScottPlot.Statistics.Histogram` obsolete in favor of static methods in `ScottPlot.Statistics.Common` designed to create histograms and probability function curves (#1051, #1166). See cookbook for usage examples. @breakwinz and @bclehmann
* WpfPlot: Improve memory management for dynamically created and destroyed WpfPlot controls by properly unloading the dispatcher timer (#1115, #1117) @RamsayGit, @bclehmann, @StendProg, and @Orace
* Mouse Processing: Improved bug that affected fast drag-dropping of draggable objects (#1076)
* Rendering: Fixed clipping bug that caused some plot types to be rendered above data area frames (#1084)
* Plot: Added `Width` and `Height` properties
* Plot: `GetImageBytes()` now returns bytes for a PNG file for easier storage in cloud applications (#1107)
* Axis: Added a `GetSettings()` method for developers, testers, and experimenters to gain access to experimental objects which are normally private for extreme customization
* Axis: Axis ticks now have a `Ticks()` overload which allows selective control over major tick lines and major tick labels separately (#1118) @kegesch
* Plot: `AxisAuto()` now has `xAxisIndex` and `yAxisIndex` arguments to selectively adjust axes to fit data on a specified index (#1123)
* Crosshair: Refactored to use two `AxisLine`s so custom formatters can now be used and lines can be independently styled (#1173, #1172, #1122, 1195) @Maoyao233 and @EFeru
* ClevelandDotPlot: Improve automatic axis limit detection (#1185) @Nextra
* ScatterPlotList: Improved legend formatting (#1190) @Maoyao233
* Plot: Added an optional argument to `Frameless()` to reverse its behavior and deprecated `Frame()` (#1112, #1192) @arthurits
* AxisLine: Added `PositionLabel` option for displaying position as text (using a user-customizable formatter function) on the axis (#1122, #1195, #1172, #1173) @EFeru and @Maoyao233
* Radar Plot: Fixed rendering artifact that occurred when axis maximum is zero (#1139) @petersesztak and @bclehmann
* Mouse Processing: Improved panning behavior when view limits (axis boundaries) are active (#1148, #1203) @at2software
* Signal Plot: Fixed bug causing render artifacts when using fill modes (#1163, #1205)
* Scatter Plot: Added support for `OffsetX` and `OffsetY` (#1164, #1213)
* Coxcomb: Added a new plot type for categorical data. See cookbook for examples. (#1188) @bclehmann
* Axes: Added `LockLimits()` to control pan/zoom manipulation so individual axes can be manipulated in multi-axis plots. See demo application for example. (#1179, #1210) @kkaiser41
* Vector Plot: Add additional options to customize arrowhead style and position. See cookbook for examples. (#1202) @hhubschle
* Finance Plot: Fixed bug affecting plots with no data points (#1200) @Maoyao233
* Ticks: Improve display of rotated ticks on secondary axes (#1201) @gigios

## ScottPlot 4.1.16
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-05-30_
* Made it easier to use custom color palettes (see cookbook) (#1058, #1082) @EFeru
* Added a `IgnoreAxisAuto` field to axis lines and spans (#999) @kirsan31
* Heatmaps now have a `Smooth` field which uses bicubic interpolation to display smooth heatmaps (#1003) @xichaoqiang
* Radar plots now have an `Update()` method for updating data values without clearing the plot (#1086, #1091) @arthurits
* Controls now automatically render after the list of plottables is modified (previously it was after the number of plottables changed). This behavior can be disabled by setting a public field in the control's `Configuration` module. (#1087, #1088) @bftrock
* New `Crosshair` plot type draws lines to highlight a point on the plot and labels their coordinates in the axes (#999, #1093) @kirsan31
* Added support for a custom `Func<double, string>` to be used as custom tick label formatters (see cookbook) (#926, #1070) @damiandixon and @ssalsinha
* Added `Move`, `MoveFirst`, and `MoveLast` to the `Plot` module for added control over which plottables appear on top (#1090) @EFeru
* Fixed bug preventing expected behavior when calling `AxisAutoX` and `AxisAutoY` (#1089) @EFeru_

## ScottPlot 4.1.15
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-05-27_
* Hide design-time error message component at run time to reduce flicking when resizing (#1073, #1075) @Superberti and @bclehmann
* Added a modern `Plot.GetBitmap()` overload suitable for the new stateless rendering system (#913 #1063)
* Controls now have `PlottableDragged` and `PlottableDropped` event handlers (#1072) @JS-BGResearch

## ScottPlot 4.1.14
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-05-23_
* Add support for custom linestyles in SignalXY plots (#1017, #1016) @StendProg and @breakwinz
* Improved Avalonia dependency versioning (#1018, #1041) @bclehmann
* Controls now properly process `MouseEnter` and `MouseLeave` events (#999) @kirsan31 and @breakwinz
* Controls now have a `RenderRequest()` method that uses a render queue to facilitate non-blocking render calls (#813, #1034) @StendProg
* Added Last() to finance plots to make it easier to access the final OHLC (#1038) @CalderWhite
* Controls that fail to render in design mode now display the error message in a textbox to prevent Visual Studio exceptions (#1048) @bclehmann

## ScottPlot 4.1.13-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-05-02_
* `Plot.Render()` and `Plot.SaveFig()` now have a `scale` argument to allow for the creation of high resolution scaled plots (#983, #982, #981) @PeterDavidson
* A `BubblePlot` has been added to allow display of circles with custom colors and sizes. See cookbook for examples. (#984, #973, #960) @PeterDavidson
* Avalonia 0.10.3 is now supported (#986) @bclehmann
* Default version of System.Drawing.Common has been changed from `5.0.0` to `4.6.1` to minimize errors associated with downgrading (#1004, #1005, #993, #924, #655) @bukkideme

## ScottPlot 4.1.12-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-04-12_
* Added "Open in New Window" option to right-click menu (#958, #969) @ademkaya and @bclehmann
* User control `Configuration` module now has customizable scroll wheel zoom fraction (#940, #937) @PassionateDeveloper86 and @StendProg
* Added options to `Plot.AxisScaleLock()` to let the user define scaling behavior when the plot is resized (#933, #857) @boingo100p and @StendProg
* Improved XML documentation for `DataGen` module (#903, #902) @bclehmann
* Fixed bug where tick labels would not render for axes with a single tick (#945, #828, #725, #925) @saklanmazozgur and @audun
* Added option to manually refine tick density (#828) @ChrisAtVault and @bclehmann
* Improved tick density calculations for DateTime axes (#725) @bclehmann
* Fixed SignalXY rendering artifact affecting the right edge of the plot (#929, #931) @damiandixon and @StendProg
* Improved line style customization for signal plots (#929, #931) @damiandixon and @StendProg
* Fixed bug where negative bar plots would default to red fill color (#968, #946) @pietcoussens
* Fixed bug where custom vertical margin was not respected when `AxisAuto()` was called with a middle-click (#943)Andreas
* Added a minimum distance the mouse must travel while click-dragging for the action to be considered a drag instead of a click (#962)
* Improved Histogram documentation and simplified access to probability curves (#930, #932, #971) @LB767, @breakwinz, and @bclehmann

## ScottPlot 4.1.11-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-03-30_
* FormsPlot mouse events are now properly forwarded to the base control (#892, #919) @grabul
* Prevent right-click menu from deploying after right-click-drag (#891, #917)
* Add offset support to SignalXY (#894, #890) @StendProg
* Eliminate rendering artifacts in SignalXY plots (#893, #889) @StendProg and @grabul
* Optimize cookbook generation and test execution (#901) @bclehmann

## ScottPlot 4.1.10-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-03-21_
* Fixed a bug where applying the Seabourn style modified axis frame and minor tick distribution (#866) @oszymczak
* Improved XML documentation and error reporting for getting legend bitmaps (#860) @mzemljak
* Fixed rendering bug affecting finance plots with thin borders (#837) @AlgoExecutor
* Improved argument names and XML docs for SMA and Bollinger band calculation methods (#830) @ticool
* Improved GetPointNearest support for generic signal plots (#809, #882, #886) @StendProg, @at2software, and @mrradd
* Added support for custom slice label colors in pie charts (#883, #844) @bclehmann, @StendProg, and @Timothy343
* Improved support for transparent heatmaps using nullable double arrays (#849, #852) @bclehmann
* Deprecated bar plot `IsHorizontal` and `IsVertical` in favor of an `Orientation` enumeration
* Deprecated bar plot `xs` and `ys` in favor of `positions` and `values` which are better orientation-agnostic names
* Added Lollipop and Cleveland plots as new types of bar plots (#842, #817) @bclehmann
* Fixed a bug where `Plot.AddBarGroups()` returned an array of nulls (#839) @rhys-wootton
* Fixed a bug affecting manual tick labels (#829) @ohru131
* Implemented an optional render queue to allow asynchronous rendering in user controls (#813) @StendProg

## ScottPlot 4.1.9-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-02-21_
* Improved support for negative DateTimes when using DateTime axis mode (#806, #807) @StendProg and @at2software
* Improved axis limit detection when using tooltips (#805, #811) @bclehmann and @ChrisAtVault
* Added `WickColor` field to candlestick plots (#803) @bclehmann
* Improved rendering of candlesticks that open and close at the same price (#803, #800) @bclehmann and @AlgoExecutor
* Improved rendering of SignalXY plots near the edge of the plot (#795) @StendProg
* new `AddScatterStep()` helper method creates a scatter plot with the step style (#808) @KlaskSkovby
* Marked `MultiPlot` obsolete
* Refactored `Colormap` module to use classes instead of reflection (#767, #773) @StendProg
* Refactored `OHLC` fields and finance plots to store `DateTime` and `TimeSpan` instead of `double` (#795)

## ScottPlot 4.1.8-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-02-16_
* Improved validation and error reporting for large heatmaps (#772) @Matthias-C
* Removed noisy console output in `ScatterPlotList` (#780) @Scr0nch
* Improved rendering bug in signal plots (#783, #788) @AlgoExecutor and @StendProg
* Fix bug that hid grid lines in frameless plots (#779)
* Improved appearance of marker-only scatter plots in the legend (#790) @AlgoExecutor
* `AddPoint()` now has a `label` argument to match `AddScatter()` (#787) @AlgoExecutor

## ScottPlot 4.1.7-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-02-14_
* Added support for image axis labels (#759, #446, #716) @bclehmann
* Added `MinRenderIndex` and `MaxRenderIndex` support to Scatter plots (#737, #763) @StendProg
* Improved display of horizontal manual axis tick labels (#724, #762) @inqb and @Saklut
* Added support for listing and retrieving colormaps by their names (#767, #773) @StendProg
* Enabled mouse pan and zoom for plots with infinitely small width and height (#768, #733, #764) @saklanmazozgur
* A descriptive exception is now thrown when attempting to create heatmaps of unsupported dimensions (#722) @Matthias-C

## ScottPlot 4.1.6-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-02-08_
* Fixed single point render bug in Signal plots (#744, #745) @at2software and @StendProg
* Improved display scaling support for WPF control (#721, #720) @bclehmann
* User control `OnAxesChanged` events now send the control itself as the sender object (#743, #756) @at2software
* Fixed configuration bug related to Alt + middle-click-drag-zoom (#741) @JS-BGResearch and @bclehmann
* Fixed render bug related to ALT + middle-click-drag zoom box (#742) @bclehmann
* Fixed render bug for extremely small plots (#735)
* Added a coordinated heatmap plot type (#707) @StendProg
* Improved appearance of heatmap edges (#713) @StendProg
* Improved design-time rendering of Windows Forms control
* Added and expanded XML documentation for Plot and Plottable classes
* Created a new cookbook website generator that combines reflection with XML documentation (#727, #738, #756)
* ScottPlot is now a reserved prefix on NuGet

## ScottPlot 4.1.5-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-02-01_
* Helper methods were added for creating scatter plots with just lines (`AddScatterLines()`) or just markers (`AddScatterPoints()`).
* Scatter and Signal plots have `GetPointNearest()` which now has a `xyRatio` argument to support identifying points near the cursor in pixel space (#709, #722) @oszymczak, @StendProg, @bclehmann
* Improved display of manual tick labels (#724) @bclehmann

## ScottPlot 4.1.4-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2021-01-25_
* User controls have been extensively redesigned (#683)
  * All user controls are almost entirely logic-free and pass events to `ScottPlot.Control`, a shared common back-end module which handles mouse interaction and pixel/coordinate conversions.
  * Controls no longer have a `Configure()` method with numerous named arguments, but instead a `Configuration` field with XML-documented public fields to customize behavior.
  * Renders occur automatically when the number of plottables changes, meaning you do not have to manually call `Render()` when plotting data for the first time. This behavior can be disabled in the configuration.
  * Avalonia 0.10.0 is now supported and uses this new back-end (#656, #700) @bclehmann
  * Events are used to provide custom right-click menu actions.
  * The right-click plot settings window (that was only available from the WinForms control) has been removed.
* New methods were added to `ScottPlot.Statistics.Common` which efficiently find the Nth smallest number, quartiles, or other quantiles from arrays of numbers (#690) @bclehmann
* New tooltip plot type (#696) @bclehmann
* Fixed simple moving average (SMA) calculation (#703) @Saklut
* Improved multi-axis rendering (#706) @bclehmann
* Improved `SetSourceAsync()` for segmented trees (#705, #692) @jl0pd and @StendProg
* Improved layout for axes with rotated ticks (#706, #699) @MisterRedactus and @bclehmann
* ScottPlot now multi-targets more platforms and supports the latest C# language version on modern platforms but restricts the language to C# 7.3 for .NET Framework projects (#691, #711) @jl0pd
* Improved project file to install `System.ValueTuple` when targeting .NET Framework 4.6.1 (#88, #691)

## ScottPlot 4.1.3-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-12-27_
* Scott will make a document to summarize 4.0 → 4.1 changes as we get closer to a non-beta release
* Fixed rendering bug affecting axis spans when zoomed far in (#662) @StendProg
* Improved Gaussian blur performance (#667) @bclehmann
* Largely refactored heatmaps (#679, #680) @bclehmann
* New `Colorbar` plot type (#681)
* Improved SMA and Bollinger band generators (#647) @Saklut
* Improved tick label rounding (#657)
* Improved setting of tick label color (#672)
* Improved fill above and below for scatter plots (#676) @MithrilMan
* Additional customizations for radar charts (#634, #628, #635) @bclehmann and @SommerEngineering

## ScottPlot 4.1.0-beta
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-11-29_
* In November, 2020 ScottPlot 4.0 branched into a permanent `stable` branch, and ScottPlot 4.1 began development as beta / pre-release in the main branch. ScottPlot 4.0 continues to be maintained, but modifications are aimed at small bugfixes rather than large refactoring or the addition of new features. ScottPlot 4.1 merged into the master branch in November, 2020 (#605). Improvements are focused at enhanced performance, improved thread safety, support for multiple axes, and options for data validation.
* Most plotting methods are unchanged so many users will not experience any breaking changes.
* Axis Limits: Axis limits are described by a `AxisLimits` struct (previously `double[]` was used)
* Axis Limits: Methods which modify axis limits do not return anything (previously they returned `double[]`)
* Axis Limits: To get the latest axis limits call `Plot.AxisLimits()` which returns a `AxisLimits` object
* Multiple Axes: Multiple axes are now supported! There is no change to the traditional workflow if this feature is not used.
* Multiple Axes: Most axis methods accept a `xAxisIndex` and `yAxisIndex` arguments to specify which axes they will modify or return
* Multiple Axes: Most plottable objects have `xAxisIndex` and `yAxisIndex` fields which specify which axes they will render on
* Multiple Axes: You can enable a second Y and X axis by calling `YLabel2` and `XLabel2()`
* Multiple Axes: You can obtain an axis by calling `GetXAxis(xAxisIndex)` or `GetYAxis(yAxisIndex)`, then modify its public fields to customize its behavior
* Multiple Axes: The default axes (left and bottom) both use axis index `0`
* Multiple Axes: The secondary axes (right and top) both use axis index `1`
* Multiple Axes: You can create additional axes by calling `Plot.AddAxis()` and customize it by modifying fields of the `Axis` it returns.
* Layout: The layout is re-calculated on every render, so it automatically adjusts to accommodate axis labels and ticks.
* Layout: To achieve extra space around the data area, call `Layout()` to supply a minimum size for each axis.
* Layout: To achieve a frameless plot where the data area fills the full figure, call `LayoutFrameless()`
* Naming: The `Plottable` base class has been replaced with an `IPlottable` interface
* Naming: Plottables have been renamed and moved into a `Plottable` namespace (e.g., `PlottableScatter` is  now `Plottable.ScatterPlot`)
* Naming: Several enums have been renamed
* Settings: It is still private, but you can request it with `Plot.GetSettings()`
* Settings: Many of its objects implement `IRenderable`, so their customization options are stored at the same level as their render methods.
* Rendering: `Bitmap` objects are never stored. The `Render()` method will create and return a new `Bitmap` when called, or will render onto an existing `Bitmap` if it is supplied as an argument. This allows controls to manage their own performance optimization by optionally re-using a `Bitmap` for multiple renders.
* Rendering: Drawing is achieved with `using` statements which respect all `IDisposable` drawing objects, improving thread safety and garbage collection performance.

## ScottPlot 4.0.46
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-12-11_
* Improved ticks for small plots (#724) @Saklut
* Improved display of manual ticks (#724) @bclehmann

## ScottPlot 4.0.45
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-11-25_
* Fixed a bug that affected very small plots with the benchmark enabled (#626) @martin-brajer
* Improved labels in bar graphs using a yOffset (#584)Terbaco
* Added `RenderLock()` and `RenderUnlock()` to the Plot module to facilitate multi-threaded plot modification (#609) @ZTaiIT1025

## ScottPlot 4.0.44
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-11-22_
* Improved limits for fixed-size axis spans (#586) @Ichibot200 and @StendProg
* Mouse drag/drop events now send useful event arguments (#593) @charlescao460 and @StendProg
* Fixed a bug that affected plots with extremely small (<1E-10) axis spans (#607) @RFIsoft
* `Plot.SaveFig()` now returns the full path to the file it created (#608)
* Fixed `AxisAuto()` bug affecting signal plots using min/max render indexes with a custom sample rate (#621) @LB767
* Fixed a bug affecting histogram normalization (#624) @LB767
* WPF and Windows Forms user controls now also target .NET 5
* Improved appearance of semi-transparent legend items (#567)
* Improved tick labels for ticks smaller than 1E-5 (#568) @ozgur640
* Improved support for Avalonia 0.10 (#571) @bclehmann and @apkrymov
* Improved positions for base16 ticks (#582, #581) @bclehmann

## ScottPlot 4.0.42
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-09-27_
* Improved DPI scaling support when using WinForms in .NET Core applications (#563) @Ichibot200
* Improved DPI scaling support for draggable axis lines and spans (#563) @Ichibot200

## ScottPlot 4.0.41
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-09-26_
* Improved density of DateTime ticks (#564, #561) @StendProg and @waynetheron
* Improved display of DateTime tick labels containing multiple spaces (#539, #564) @StendProg

## ScottPlot 4.0.40
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-09-20_
* Added user control for Avalonia (#496, #503) @bclehmann
* Holding shift while left-click-dragging the edge of a span moves it instead of resizing it (#509) @Torgano
* CSV export is now culture invariant for improved support on systems where commas are decimal separators (#512)Daniel
* Added fill support to scatter plots (#529) @AlexFsmn
* Fix bug that occurred when calling `GetLegendBitmap()` before the plot was rendered (#527) @el-aasi
* Improved DateTime tick placement and added support for milliseconds (#539) @StendProg
* Pie charts now have an optional hollow center to produce donut plots (#534) @bclehmann and @AlexFsmn
* Added electrocardiogram (ECG) simulator to the DataGen module (#540) @AteCoder
* Improved mouse scroll wheel responsiveness by delaying high quality render (#545, #543, #550) @StendProg
* `Plot.PlotBitmap()` allows Bitmaps to be placed at specific coordinates (#528) @AlexFsmn
* `DataGen.SampleImage()` returns a sample Bitmap that can be used for testing
* Bar graphs now have a hatchStyle property to customize fill pattern (#555) @bclehmann
* Support timecode tick labels (#537) @vrdriver and @StendProg

## ScottPlot 4.0.39
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-08-09_
* Legend now reflects LineStyle of Signal and SignalXY plots (#488) @bclehmann
* Improved mouse wheel zoom-to-cursor and middle-click-drag rectangle zoom in the WPF control for systems that use display scaling (#490) @nashilnik
* The `Configure()` method of user controls now has a `lowQualityAlways` argument to let the user easily enable/disable anti-aliasing at the control level. Previously this was only configurable by reaching into the control's plot object and calling its `AntiAlias()` method. (#499) @RachamimYaakobov
* SignalXY now supports parallel processing (#500) @StendProg
* SignalXY now respects index-based render limits (#493, #500) @StendProg and @envine

## ScottPlot 4.0.38
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-07-07_
* Improved `Plot.PlotFillAboveBelow()` rendering of data with a non-zero baseline (#477) @el-aasi
* Added `Plot.PlotWaterfall()` for easy creation of waterfall-style bar plots (#463, #476) @bclehmann
* Axis tick labels can be displayed using notations other than base 10 by supplying `Plot.Ticks()` with `base` and `prefix` arguments, allowing axes that display binary (e.g., `0b100110`) or hexadecimal (eg., `0x4B0D10`) tick labels (#469, #457) @bclehmann
* Added options to `PlotBar()` to facilitate customization of text displayed above bars when `showValue` is enabled (#483) @WillemWever
* Plot objects are colored based on a pre-defined set of colors. The default colorset (category10) is the same palette of colors used by matplotlib. A new `Colorset` module has been created to better define this behavior, and `Plot.Colorset()` makes it easy to plot data using alternative colorsets. (#481)
* Fixed a bug that caused instability when a population plot is zoomed-out so much that its fractional distribution curve is smaller than a single pixel (#480) @HowardWhile
* Added `Plot.Remove()` method to make it easier to specifically remove an individual plottable after it has been plotted. `Plot.Clear()` is similar, but designed to remove classes of plot types rather than a specific plot object. (#479) @cstyx and @Resonanz
* Signal plots can now be created with a defined `minRenderIndex` (in addition to the already-supported `maxRenderIndex`) to facilitate partial display of large arrays (#474) @bclehmann

## ScottPlot 4.0.37
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-06-22_
* Fixed a long-running issue related to strong assembly versioning that caused the WPF control to fail to render in the Visual Studio designer in .NET Framework (but not .NET Core) projects (#473, #466, #356) @bhairav-thakkar, @riquich, @Helitune-RobMcKay, and @iu2kxv
* User controls now also target `net472` (while still supporting `net461` and `netcoreapp3.0`) to produce a build folder with just 3 DLLs (compared to over 100 when building with .NET Framework 4.6.1)

## ScottPlot 4.0.36
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-06-22_
* `PlotSignal()` and `PlotSignalXY()` plots now have an optional `useParallel` argument (and public property on the objects they return) to allow the user to decide whether parallel or sequential calculations will be performed. (#454, #419, #245, #72) @StendProg
* Improved minor tick alignment to prevent rare single-pixel artifacts (#417)
* Improved horizontal axis tick label positions in ruler mode (#453)
* Added a `Statistics.Interpolation` module to generate smooth interpolated splines from a small number of input data points. See advanced statistics cookbook example for usage information. (#459)Hans-Peter Moser
* Improved automatic axis adjustment when adding bar plots with negative values (#461, #462) @bclehmann
* Created `Drawing.Colormaps` module which has over a dozen colormaps for easily converting a fractional value to a color for use in plotting or heatmap displays (#457, #458) @bclehmann
* Updated `Plot.Clear()` to accept any `Plottable` as an argument, and all `Plottable` objects of the same type will be cleared (#464) @imka-code

## ScottPlot 4.0.35
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-06-09_
* Added `processEvents` argument to `formsPlot2.Render()` to provide a performance enhancement when linking axes of two `FormsPlot` controls together (by calling `Plot.MatchAxis()` from the control's `AxesChanged` event, as seen in theLinked Axes demo application) (#451, #452) @StendProg and @robokamran
* New `Plot.PlotVectorField()` method for displaying vector fields (sometimes called quiver plots) (#438, #439, #440) @bclehmann and @hhubschle
* Included an experimental colormap module which is likely to evolve over subsequent releases (#420, #424, #442) @bclehmann
* `PlotScatterHighlight()` was created as a type of scatter plot designed specifically for applications where "show value on hover" functionality is desired. Examples are both in the cookbook and WinForms and WPF demo applications. (#415, #414) @bclehmann and @StendProg
* `PlotRadar()` is a new plot type for creating Radar plots (also called spider plots or star plots). See cookbook and demo application for examples. (#428, #430) @bclehmann
* `PlotPlolygons()` is a new performance-optimized variant of `PlotPolygon()` designed for displaying large numbers of complex shapes (#426) @StendProg
* The WinForms control's `Configure()` now has a `showCoordinatesTooltip` argument to continuously display the position at the tip of the cursor as a tooltip (#410) @jcbeppler
* User controls now use SHIFT (previously ALT) to lock the horizontal axis and ALT (previously SHIFT) while left-click-dragging for zoom-to-region. Holding CTRL+SHIFT while right-click-dragging now zooms evenly, without X/Y distortion. (#436) @tomwimmenhove and @StendProg
* Parallel processing is now enabled by default. Performance improvements will be most noticeable on Signal plots. (#419, #245, #72)
* `Plot.PlotBar()` now has an `autoAxis` argument (which defaults `true`) that automatically adjusts the axis limits so the base of the bar graphs touch the edge of the plot area. (#406)
* OSX-specific DLLs are now only retrieved by NuGet on OSX (#433, #211, #212)
* Pie charts can now be made with `plt.PlotPie()`. See cookbook and demo application for examples. (#421, #423) @bclehmann
* `ScottPlot.FormsPlotViewer(Plot)` no longer resets the new window's plot to the default style (#416)  @StendProg
* Controls now have a `recalculateLayoutOnMouseUp` option to prevent resetting of manually-defined data area padding

## ScottPlot 4.0.34
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-05-18_
* Improve display of `PlotSignalXY()` by not rendering markers when zoomed very far out (#402) @gobikulandaisamy
* Optimized rendering of solid lines which have a user-definable `LineStyle` property. This modification improves grid line rendering and increases performance for most types of plots. (#401, #327) @bukkideme and @Ichibot200

## ScottPlot 4.0.33
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-05-18_
* Force grid lines to always draw using anti-aliasing. This compensates for a bug in `System.Drawing` that may cause diagonal line artifacts to appear when the user controls were panned or zoomed. (#401, #327) @bukkideme and @Ichibot200

## ScottPlot 4.0.32
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-05-17_
* User controls now have a `GetMouseCoordinates()` method which returns the DPI-aware position of the mouse in graph coordinates (#379, #380) @bclehmann
* Default grid color was lightened in the user controls to match the default style (#372)
* New `PlotSignalXY()` method for high-speed rendering of signal data that has unevenly-spaced X coordinates (#374, #375) @StendProg and @LogDogg
* Modify `Tools.Log10()` to return `0` instead of `NaN`, improving automatic axis limit detection (#376, #377) @bclehmann
* WpfPlotViewer and FormsPlotViewer launch in center of parent window (#378)
* Improve reliability of `Plot.AxisAutoX()` and `Plot.AxisAutoY()` (#382)
* The `Configure()` method of FormsPlot and WpfPlot controls now have `middleClickMarginX` and `middleClickMarginY` arguments which define horizontal and vertical auto-axis margin used for middle-clicking. Setting horizontal margin to 0 is typical when plotting signals. (#383)
* `Plot.Grid()` and `Plot.Ticks()` now have a `snapToNearestPixel` argument which controls whether these lines appear anti-aliased or not. For static images non-anti-aliased grid lines and tick marks look best, but for continuously-panning plots anti-aliased lines look better. The default behavior is to enable snapping to the nearest pixel, consistent with previous releases. (#384)
* Mouse events (MouseDown, MouseMove, etc.) are now properly forwarded to the FormsPlot control (#390) @Minu476
* Improved rendering of very small candlesticks and OHLCs in financial plots
* Labeled plottables now display their label in the ToString() output. This is useful when viewing plottables listed in the FormsPlot settings window #391 @Minu476
* Added a Statistics.Finance module with methods for creating Simple Moving Average (SMA) and Bollinger band technical indicators to Candlestick and OHLC charts. Examples are in the cookbook and demo program. (#397) @Minu476
* Scatter plots, filled plots, and polygon plots now support Xs and Ys which contain `double.NaN` #396
* Added support for line styles to Signal plots (#392) @bukkideme

## ScottPlot 4.0.31
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-05-06_
* Created `Plot.PlotBarGroups()` for easier construction of grouped bar plots from 2D data (#367) @bclehmann
* Plot.PlotScaleBar() adds an L-shaped scalebar to the corner of the plot (#363)
* Default grid color lightened from #D3D3D3 (Color.LightGray) to #EFEFEF (#372)
* Improved error reporting for scatter plots (#369) @JagDTalcyon
* Improve pixel alignment by hiding grid lines and snapping tick marks that are 1px away from the lower left edge (#359)
* PlotText() ignores defaults to upperLeft alignment when rotation is used (#362)
* Improved minor tick positioning to prevent cases where minor ticks are 1px away from major ticks (#373)

## ScottPlot 4.0.30
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-05-04_
* `Plot.PlotCandlestick()` and `Plot.PlotOHLC()`
  * now support `OHLC` objects with variable widths defined with a new `timeSpan` argument in the OHLC constructor. (#346) @Minu476
  * now support custom up/down colors including those with transparency (#346) @Minu476
  * have a new `sequential` argument to plot data based on array index rather than `OHLC.time`. This is a new, simpler way to display unevenly-spaced data (e.g., gaps over weekends) in a way that makes the gaps invisible. (#346) @Minu476
* Fixed a marker/line alignment issue that only affeced low-density Signal plots on Linux and MacOS (#340) @SeidChr
* WPF control now appears in Toolbox (#151) @RalphLAtGitHub
* Plot titles are now center-aligned with the data area, not the figure. This improves the look of small plots with titles. (#365) @Resonanz
* Fixed bug that ignored `Configure(enableRightClickMenu: false)` in WPF and WinForms user controls. (#365) @thunderstatic
* Updated `Configure(enableScrollWheelZoom: false)` to disable middle-click-drag zooming. (#365) @eduhza
* Added color mixing methods to ScottPlot.Drawing.GDI (#361)
* Middle-click-drag zooming now respects locked axes (#353) @LogDogg
* Improved user control zooming of high-precision DateTime axis data (#351) @bukkideme
* Plot.AxisBounds() now lets user set absolute bounds for drag and pan operations (#349) @LogDogg
* WPF control uses improved Bitmap conversion method (#350)
* Function plots have improved handling of functions with infinite values (#370) @bclehmann

## ScottPlot 4.0.29
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-04-11_
* `Plot.PlotFill()` can be used to make scatter plots with shaded regions. Giving it a single pair of X/Y values (`xs, ys`) lets you shade beneath the curve to the `baseline` value (which defaults to 0). You can also give it a pair of X/Y values (`xs1, ys1, xs2, ys2`) and the area between the two curves will be shaded (the two curves do not need to be the same length). See cookbook for examples. (#255) @ckovamees 
* `DataGen.Range()` now has `includeStop` argument to include the last value in the returned array.
* `Tools.Pad()` has been created to return a copy of a given array padded with data values on each side. (#255) @ckovamees
* [Seaborn](https://seaborn.pydata.org/) style can be activated using `Plot.Style(Style.Seaborn)` (#339)
* The `enableZooming` argument in `WpfPlot.Configure()` and `FormsPlot.Configure()` has been replaced by two arguments `enableRightClickZoom` and `enableScrollWheelZoom` (#338)Zach
* Improved rendering of legend items for polygons and filled plots (#341) @SeidChr
* Improved Linux rendering of legend items which use thick lines: axis spans, fills, polygons, etc. (#340) @SeidChr
* Addded `Plot.PlotFillAboveBelow()` to create a shaded line plot with different colors above/below the baseline. (#255) @ckovamees
* Improved rendering in Linux and MacOS by refactoring the font measurement system (#340) @SeidChr

## ScottPlot 4.0.28
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-04-07_
* `Ticks()` now has arguments for numericStringFormat (X and Y) to make it easy to customize formatting of tick labels (percentage, currency, scientific notation, etc.) using standard [numeric format strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings). Example use is demonstrated in the cookbook. (#336) @deiruch
* The right-click menu can now be more easily customized by writing a custom menu to `FormsPlot.ContextMenuStrip` or `WpfPlot.ContextMenu`. Demonstrations of both are in the demo application. (#337) @Antracik

## ScottPlot 4.0.27
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-04-05_
* `Plot.Polygon()` can now be used to plot polygons from X/Y points (#255) @ckovamees
* User controls now have an "open in new window" item in their right-click menu (#280)
* Plots now have offset notation and multiplier notation disabled by default. Layouts are automatically calculated before the first render, or manually after MouseUp events in the user controls. (#310)
* `Plot.Annotation()` allows for the placement of text on the figure using pixel coordinates (not unit coordinates on the data grid). This is useful for creating custom static labels or information messages. (#321) @SeidChr
* `FormsPlot.MouseDoubleClicked` event now passes a proper `MouseEventArgs` instead of `null` (#331) @ismdiego
* Added a right-click menu to `WpfPlot` with items (save image, copy image, open in new window, help, etc.) similar to `FormsPlot`

## ScottPlot 4.0.26
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-04-05_
* The `ScottPlot.WPF` package (which provides the `WpfPlot` user control) now targets .NET Framework 4.7.2 (in addition to .NET Core 3.0), allowing it to be used in applications which target either platform. The ScottPlot demo application now targets .NET Framework 4.7.2 which should be easier to run on most Windows systems. (#333)
* The `ScottPlot.WinForms` package (which produves the `FormsPlot` control) now only targets .NET Framework 4.6.1 and .NET Core 3.0 platforms (previously it also had build targets for .NET Framework 4.7.2 and .NET Framework 4.8). It is important to note that no functionality was lost here. (#330, #333)

## ScottPlot 4.0.25
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-03-29_
* `PlotBar()` now supports displaying values above each bar graph by setting the `showValues` argument.
* `PlotPopulations()` has extensive capabilities for plotting grouped population data using box plots, bar plots, box and whisper plots, scatter data with distribution curves, and more! See the cookbook for details. (#315)
* `Histogram` objects now have a `population` property.
* `PopulationStats` has been renamed to `Population` and has additional properties and methods useful for reporting population statistics.
* Improved grid rendering rare artifacts which appear as unwanted diagnal lines when anti-aliasing is disabled. (#327)

## ScottPlot 4.0.24
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-03-27_
* `Plot.Clear()` has been improved to more effectively clear plottable objects. Various overloads are provided to selectively clear or preserve certain plot types. (#275) @StendProg
* `PlotBar()` has been lightly refactored. Argument order has been adjusted, and additional options have been added. Error cap width is now in fractional units instead of pixel units. Horizontal bar charts are now supported. (#277, #315) @bonzaiferroni

## ScottPlot 4.0.23
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-03-23_
* Interactive plot viewers were created to make it easy to interactively display data in a pop-up window without having to write any GUI code: `ScottPlot.WpfPlotViewer` for WPF and `ScottPlot.FormsPlotViewer` for Windows Forms
* Fixed bug that affected the `ySpacing` argument of `Plot.Grid()`
* `Plot.Add()` makes it easy to add a custom `Plottable` to the plot
* `Plot.XLabels()` and `Plot.YLabels()` can now accept just a string array (x values are auto-populated as a consecutive series of numbers).
* Aliased `Plot.AxisAuto()` to `Plot.AutoAxis()` and `Plot.AutoScale()` to make this function easier to locate for users who may have experience with other plot libraries. (#309) @Resonanz
* Empty plots now render grid lines, ticks, and tick labels (#313)
* New plot type: Error bars. They allow the user to define error bar size in all 4 directions by calling `plt.PlotErrorBars()`. (#316) @zrolfs
* Improve how dashed lines appear in the legend
* Improved minor tick positions when using log scales with `logScaleX` and `logScaleY` arguments of `plt.Ticks()` method
* Fixed bug that caused the center of the coordinate field to shift when calling `Plot.AxisZoom()`
* Grid line thickness and style (dashed, dotted, etc) can be customized with new arguments in the `Plot.Grid()` method

## ScottPlot 4.0.22
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-03-17_
* Added support for custom horizontal axis tick rotation (#300) @SeidChr
* Added support for fixed grid spacing when using DateTime axes (#299) @SeidChr
* Updated ScottPlot icon (removed small text, styled icon after emoji)
* Improved legend font size when using display scaling (#289)
* Scroll wheel zooming now zooms to cursor (instead of center) in WPF control. This feature works now even if display scaling is used. (#281)
* Added `Plot.EqualAxis` property to make it easy to lock axis scales together (#306) @StendProg

## ScottPlot 4.0.21
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-03-15_
* Created new cookbook and demo applications for WinForms and WPF (#271)
* The `FormsPlot.MouseMoved` event now has `MouseEventArgs` (instead of `EventArgs`). The purpose of this was to make it easy to access mouse pixel coordinates via `e.X` and `e.Y`, but this change may require modifications to applications which use the old event signature.
* WpfPlot now has an `AxisChanged` event (like FormsPlot)
* Fixed bug that caused `Plot.CoordinateFromPixelY()` to return incorrect value
* Fixed bug causing cursor to show arrows when hovered over some non-draggable objects
* Improved support for WinForms and WpfPlot transparency (#286) @StendProg and @envine
* Added `DataGen.Zeros()` and `DataGen.Ones()` to generate arrays filled with values using methods familiar to numpy users.
* Added `equalAxes` argument to `WpfPlot.Configure()` (#272)
* Fixed a bug affecting the `equalAxes` argument in `FormsPlot.Configure()` (#272)
* Made all `Plot.Axis` methods return axis limits as `double[]` (previously many of them returned `void`)
* Added overload for `Plot.PlotLine()` which accepts a slope, offset, and start and end X points to make it easy to plot a linear line with known formula. Using PlotFormula() will produce the same output, but this may be simpler to use for straight lines.
* Added `rSquared` property to linear regression fits (#290) @bclehmann and @StendProg
* Added `Tools.ConvertPolarCoordinates()` to make it easier to display polar data on ScottPlot's Cartesian axes (#298) @bclehmann
* Improved `Plot.Function()` (#243) @bclehmann
* Added overload for `Plot.SetCulture()` to let the user define number and date formatting rather than relying on pre-made cultures (#301, #236) @SeidChr

## ScottPlot 4.0.19
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-02-29_
* Improved how markers are drawn in Signal and SignalConst plots at the transition area between zoomed out and zoomed in (#263) @bukkideme and @StendProg
* Improved support for zero lineSize and markerSize in Signal and SignalConst plots (#263, #264) @bukkideme and @StendProg
* Improved thread safety of interactive graphs (#245) @StendProg
* Added `CoordinateFromPixelX()` and `CoordinateFromPixelY()` to getdouble precision coordinates from a pixel location. Previously only SizeF (float) precision was available. This improvement is especially useful when using DateTime axes. (#269)Chris
* Added `AxisScale()` to adjust axis limits to set a defined scale (units per pixel) for each axis.
* Added `AxisEqual()` to adjust axis limits to set the scale of both axes to be the same regardless of the size of each axis (#272) @gberrante
* `PlotHSpan()` and `PlotVSpan()` now return `PlottableHSpan` and `PlottableVSpan` objects (instead of a `PlottableAxSpan` with a `vertical` property)
* `PlotHLine()` and `PlotVLine()` now return `PlottableHLine` and `PlottableVLine` objects (instead of a `PlottableAxLine` with a `vertical` property)
* MultiPlot now has a `GetSubplot()` method which returns the Plot from a row and column index (#242) @Resonanz and @StendProg
* Created `DataGen.Range()` to make it easy to create double arrays with evenly spaced data (#259)
* Improved support for display scaling (#273) @zrolfs
* Improved event handling (#266, #238) @StendProg
* Improved legend positioning (#253) @StendProg

## ScottPlot 4.0.18
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-02-07_
* Added `Plot.SetCulture()` for improved local culture formatting of numerical and DateTime axis tick labels (#236) @teejay-87

## ScottPlot 4.0.17
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-02-06_
* Added `mouseCoordinates` property to WinForms and WPF controls (#235) @bukkideme
* Fixed rendering bug that affected horizontal lines when anti-aliasing was turned off (#232) @StendProg
* Improved responsiveness while dragging axis lines and axis spans (#228) @StendProg

## ScottPlot 4.0.16
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-02-02_
* Improved support for MacOS and Linux (#211, #212, #216) @hexxone and @StendProg
* Fixed a bug affecting the `ySpacing` argument in `Plot.Grid()` (#221) @teejay-87
* Enabled `visible` argument in `Title()`, `XLabel()`, and `YLabel()` (#222) @ckovamees
* AxisSpan: Edges are now optionally draggable (#228) @StendProg
* AxisSpan: Can now be selectively removed with `Clear()` argument
* AxisSpan: Fixed bug caused by zooming far into an axis span (#226) @StendProg
* WinForms control: now supports draggable axis lines and axis spans
* WinForms control: Right-click menu now has "copy image" option (#220)
* WinForms control: Settings screen now has "copy CSV" button to export data (#220)
* WPF control: now supports draggable axis lines and axis spans
* WPF control: Configure() to set various WPF control options
* Improved axis handling, expansion, and auto-axis (#219, #230) @StendProg
* Added more options to `DataGen.Cos()`
* Tick labels can be hidden with `Ticks()` argument (#223) @ckovamees

## ScottPlot 4.0.14
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-01-24_
* Improved `MatchAxis()` and `MatchLayout()` (#217) @ckovamees and @StendProg

## ScottPlot 4.0.13
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-01-21_
* Improved support for Linux and MacOS @hexxone
* Improved font validation (#211, #212) @hexxone and @StendProg

## ScottPlot 4.0.11
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-01-19_
* User controls now have a `cursor` property which can be set to allow custom cursors. (#187) @gobikulandaisamy
* User controls now have a `mouseCoordinates` property which make it easy to get the X/Y location of the cursor. (#187) @gobikulandaisamy

## ScottPlot 4.0.10
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2020-01-18_
* Improved density colormap (#192, #194) @StendProg
* Added linear regression tools and cookbook example (#198) @bclehmann
* Added `maxRenderIndex` to Signal to allow partial plotting of large arrays intended to be used with live, incoming data (#202) @StendProg and @plumforest
* MadeShift + Left-click-drag zoom into a rectangle light middle-click-drag (in WinForms and WPF controls) to add support for mice with no middle button (#90) @JagDTalcyon
* Throw an exception if `SaveFig()` is called before the image is properly sized (#192) @karimshams and @StendProg
* `Ticks()` now has arguments for `FontName` and `FontSize` (#204)Clay
* Fixed a bug that caused poor layout due to incorrect title label size estimation (#205)Clay
* `Grid()` now has arguments to selectively enable/disable horizontal and vertical grid lines (#206)Clay
* Added tool and cookbook example to make it easier to plot data on a log axis (#207) @senged
* Arrows can be plotted using `plt.PlotArrow()` (#201)Clay

## ScottPlot 4.0.9
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-12-03_
* Use local regional display settings when formatting the month tick of DateTime axes. (#108) @FadyDev2
* Debug symbols are now packaged in the NuGet file

## ScottPlot 4.0.7
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-12-01_\_
* Added WinForms support for .NET Framework 4.7.2 and 4.8
* Fixed bug in WinForms control that only affected .NET Core 3.0 applications (#189, #138) @petarpetrovt

## ScottPlot 4.0.6
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-11-29_\_
* fixed bug that affected the settings dialog window in the WinForms control. (#187) @gobikulandaisamy

## ScottPlot 4.0.5
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-11-27_
* improved spacing for non-uniformly distributed OHLC and candlestick plots. (#184) @Luvnet-890
* added `fixedLineWidth` to `Legend()` to allow the user to control whether legend lines are dynamically sized. (#185) @ab-tools
* legend now hides lines or markers of they're hidden in the plottable
* DateTime axes now use local display format (#108) @FadyDev2

## ScottPlot 4.0.4
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-11-24_
* `PlotText()` now supports a background frame (#181) @Luvnet-890
* OHLC objects can be created with a double or a DateTime (#182) @Minu476
* Improved `AxisAuto()` fixes bug for mixed 2d and axis line plots

## ScottPlot 4.0.3
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-11-23_
* Fixed bug when plotting single-point candlestick (#172) @Minu476
* Improved style editing of plotted objects (#173) @Minu476
* Fixed pan/zoom axis lock when holding CTRL or ALT (#90) @FadyDev2
* Simplified the look of the user controls in designer mode
* Improved WPF control mouse tracking when using DPI scaling
* Added support for manual tick positions and labels (#174) @Minu476
* Improved tick system when using DateTime units (#108) @Padanian, @FadyDev2, and @Bhandejiya
* Created `Tools.DateTimesToDoubles(DateTime[] array)` to easily convert an array of dates to doubles which can be plotted with ScottPlot, then displayed as time using `plt.Ticks(dateTimeX: true)`.
* Added an inverted sign flag to allow display of an axis with descending units. (#177)Bart

## ScottPlot 4.0.2
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-11-09_
* Multi-plot figures: Images with several plots can be created using `ScottPlot.MultiPlot()`
* `ScottPlot.DataGen` functions which require a `Random` can accept null (they will create a `Random` if null is given)
* `plt.MatchAxis()` and `plt.MatchLayout()` have been improved
* `plt.PlotText()` now supports rotated text using the `rotation` argument. (#160) @gwilson9
* `ScottPlot.WinForms` user control has new events and `formsPlot1.Configure()` arguments to make it easy to replace the default functionality for double-clicking and deploying the right-click menu (#166). @FadyDev2
* All plottables now have a `visible` property which makes it easy to toggle visibility on/off after they've been plotted. @Nasser

## ScottPlot 4.0.1
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-11-03_
* ScottPlot now targets .NET Standard 2.0 so in addition to .NET Framework projects it can now be used in .NET Core applications, ASP projects, Xamarin apps, etc.
* The WinForms control has its own package which targets both .NET Framework 4.6.1 and .NET Core 3.0 @petarpetrovt
* The WPF control has its own package targeting .NET Core 3.0 @petarpetrovt
* Better layout system and control of padding @Ichibot200
* Added ruler mode to `plt.Ticks()` @Ichibot200
* `plt.MatchLayout()` no longer throws exceptions
* Eliminated `MouseTracker` class (tracking is now in user controls)
* Use NUnit (not MSTest) for tests

## ScottPlot 3.1.6
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-10-20_
* Reduced designer mode checks to increase render speed @StendProg
* Fixed cursor bug that occurred when draggable axis lines were used @Kamran
* Fully deleted the outdated `ScottPlotUC`
* Fixed infinite zoom bug caused by calling AxisAuto() when plotting a single point (or perfectly straight horizontal or vertical line)
* Added `ToolboxItem` and `DesignTimeVisible` delegates to WpfPlot control to try to get it to appear in the toolbox (but it doesn't seem to be working)
* Improved figure padding when axes frames are disabled @Ichibot200
* Improved rendering of ticks at the edge of the plottable area @Ichibot200
* Added `AxesChanged` event to user control to make it easier to sync axes between multiple plots
* Disabled drawing of arrows on user control in designer mode

## ScottPlot 3.1.5
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-10-06_
* WPF user control improved support for display scaling @morningkyle
* Fixed bug that crashed on extreme zoom-outs @morningkyle
* WPF user control improvements (middle-click autoaxis, scrollwheel zoom)
* ScottPlot user control has a new look in designer mode. Exceptions in user controls in designer mode can crash Visual Studio, so this risk is greatly reduced by not attempting to render a ScottPlotinside Visual Studio.

## ScottPlot 3.1.4
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-09-22_
* middle-click-drag zooms into a rectangle drawn with the mouse
* Fixed bug that caused user control to crash Visual Studio on some systems that used DPI scaling. (#125, #111) @ab-tools and @bukkideme
* Fixed poor rendering for extremely small plots
* Fixed bug when making a scatter plot with a single point (#126). @bonzaiferroni
* Added more options to right-click settings menu (grid options, legend options, axis labels, editable plot labels, etc.)
* Improved axis padding and image tightening
* Greatly refactored the settings module (no change in functionality)

## ScottPlot 3.1.3
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-08-25_
* FormsPlot: middle-click-drag zooms into a rectangle
* FormsPlot: CTRL+scroll to lock vertical axis
* FormsPlot: ALT+scroll to loch horizontal axis
* FormsPlot: Improved (and overridable) right-click menu
* Ticks: rudimentary support for date tick labels (`dateTimeX` and `dateTimeY`)
* Ticks: options to customize notation (`useExponentialNotation`, `useOffsetNotation`, and `useMultiplierNotation`)

## ScottPlot 3.1.0
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-08-19_
* `ScottPlotUC` was renamed to `FormsPlot`
* `ScottPlotWPF` was renamed to `WpfPlot`
* The right-click menu has improved. It responds faster and has improved controls to adjust plot settings.
* Plots can now be saved in BMP, PNG, JPG, and TIF format
* Holding `CTRL` while click-dragging locks the horizontal axis
* Holding `ALT` while click-dragging locks the vertical axis
* Minor ticks are now displayed (and can be turned on or off with `Ticks()`)
* Legend can be accessed for external display with `GetLegendBitmap()`
* anti-aliasing is turned off while click-dragging to increase responsiveness (#93) @StendProg
* `PlotSignalConst` now has support for generics and improved performance using single-precision floating-point math. @StendProg
* Legend draws more reliably (#104, #106) @StendProg
* `AxisAuto()` now has `expandOnly` arguments
* Axis lines with custom lineStyles display properly in the legend

## ScottPlot 3.0.9
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-08-12_
* New Plot Type: `PlotSignalConst` for extremely large arrays of data which are not expected to change after being plotted. Plots generated with this method can be much faster than `PlotSignal`. (#70) @StendProg
* Greatly improved axis tick labels. Axis tick labels are now less likely to overlap with axis labels, and it displays very large and very small numbers well using exponential notation. (#47, #68) @Padanian
* Parallel processing support for `SignalPlot` (#72) @StendProg
* Every `Plot` function now returns a `Plottable`. When creating things like scatter plots, text, and axis lines, the returned object can now be used to update the data, position, styling, or call plot-type-specific methods.
* Right-click menu now displays ScottPlot and .NET Framework version
* Improved rendering of extremely zoomed-out signals 
* Rendering speed increased now that `Format32bppPArgb` is the default PixelFormat (#83) @StendProg
* `DataGen.NoisySin()` was added
* Code was tested in .NET Core 3.0 preview and compiled without error. Therefore, the next release will likely be for .NET Core 3.0 (#85, #86) @petarpetrovt
* User controls now render graphs with anti-alias mode off (faster) while the mouse is being dragged. Upon release a high quality render is performed.

## ScottPlot 3.0.8
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-08-04_
* WPF User Control: A ScottPlotWPF user control was created to allow provide a simple mouse-interactive ScottPlot control to WPF applications. It is not as full-featured as the winforms control (it lacks a right-click menu and click-and-drag functions), but it is simple to review the code (<100 lines) and easy to use.
* New plot type: `plt.AxisSpan()` shades a region of the graph (semi-transparency is supported)
* Ticks: Vertical ticks no longer overlap with vertical axis label (#47) @bukkideme
* Ticks: When axis tick labels contain very large or very small numbers, scientific notation mode is engaged
* Ticks: Horizontal tick mark spacing increased to prevent overlapping
* Ticks: Vertical tick mark spacing increased to be consistent with horizontal tick spacing
* Plottable objects now have a `SaveCSV(filename)` method. Scatter and Signal plot data can be saved from the user control through the right-click menu.
* Added `lineStyle` arguments to Scatter plots
* Improved legend: ability to set location, ability to set shadow direction, markers and lines are now rendered in the legend
* Improved ability to use custom fonts
* Segoe UI is now the default font for all plot components

## ScottPlot 3.0.7
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-07-27_
* New plot type: `plt.PlotStep()`
* New plot type `plt.PlotCandlestick()`
* New plot type `plt.PlotOHLC()`
* `plt.MatchPadding()` copies the data frame layout from one ScottPlot onto another (useful for making plots of matching size)
* `plt.MatchAxis()` copies the axes from one ScottPlot onto another (useful for making plots match one or both axis)
* `plt.Legend()` improvements: The `location` argument allows the user to place the legend at one of 9 different places on the plot. The `shadowDirection` argument allows the user to control if a shadow is shown and at what angle.
* Custom marker shapes can be specified using the `markerShape` argument.

## ScottPlot 3.0.6
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-06-30_
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
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-06-23_
* Improved pan and zoom performance

## ScottPlot 3.0.4
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-06-23_
* Bar graphs: New `plotBar()` method allow creation of bar graphs. By customizing the `barWidth` and `xOffset` arguments you can push bars together to create grouped bar graphs. Error bars can also be added with the `yError` argument.
* Scatter plots support X and Y error bars: `plotScatter()` now has arguments to allow X and Y error bars with adjustable error bar line width and cap size.
* Draggable axis lines: `plotHLine()` and `plotVLine()` now have a `draggable` argument which lets those axis lines be dragged around with the mouse (#11) @plumforest
* Fixed errors caused by resizing to 0px
* Fixed a capitalization inconsistency in the `plotSignal` argument list
* `axisAuto()` now includes positions of axis lines (previously they were ignored)
* Fixed an that caused SplitContainer splitters to freeze (#23) @bukkideme

## ScottPlot 3.0.3
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-05-29_
* Update NuGet package to depend on System.Drawing.Common

## ScottPlot 3.0.2
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-05-28_
* Changed target from .NET Framework 4.5 to 4.7.2 (#15) @plumforest

## ScottPlot 3.0.1
_Published on [NuGet](https://www.nuget.org/profiles/ScottPlot) on 2019-05-28_
* First version of ScottPlot published on NuGet

