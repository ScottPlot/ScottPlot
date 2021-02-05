# ScottPlot.Plot API
Virtually all functionality in ScottPlot is achieved by calling methods of the Plot module.

This document was generated for `ScottPlot 4.1.5-beta`
### Methods to Manipulate the Plot
Method | Summary
---|---
[**Add**](#Add)|Add a plottable to the plot
[**AddAxis**](#AddAxis)|Create and return an additional axis
[**AxisAuto**](#AxisAuto)|Automatically adjust axis limits to fit the data
[**AxisAutoX**](#AxisAutoX)|Automatically adjust axis limits to fit the data
[**AxisAutoY**](#AxisAutoY)|Automatically adjust axis limits to fit the data (with a little extra margin)
[**AxisPan**](#AxisPan)|Pan the primary X and Y axis without affecting zoom
[**AxisScale**](#AxisScale)|Adjust axis limits to achieve a certain pixel scale (units per pixel)
[**AxisScaleLock**](#AxisScaleLock)|Lock X and Y axis scales (units per pixel) together to protect symmetry of circles and squares
[**AxisZoom**](#AxisZoom)|Zoom in or out. The amount of zoom is defined as a fraction of the current axis span.
[**Benchmark**](#Benchmark)|If enabled, the benchmark displays render information in the corner of the plot.
[**Clear**](#Clear)|Clear all plottables
[**Clear**](#Clear)|Remove all plottables of the given type
[**Copy**](#Copy)|Return a new Plot with all the same Plottables (and some of the styles) of this one.
[**Equals**](#Equals)|Returns true if the given plot is the exact same plot as this one
[**Frame**](#Frame)|Configure color and visibility of the frame that outlines the data area. Note that the axis lines of all 4 primary axes touch each other, giving the appearance of a rectangle framing the data area. This method allows the user to customize these lines as a group or individually.
[**Frameless**](#Frameless)|Give the plot a frameless appearance by setting the size of all axes to zero. This causes the data area to go right up to the edge of the plot.
[**GetAxisLimits**](#GetAxisLimits)|Returns the current limits for a given pair of axes.
[**GetCoordinate**](#GetCoordinate)|Return the coordinate (in coordinate space) for the given pixel
[**GetCoordinateX**](#GetCoordinateX)|Return the X position (in coordinate space) for the given pixel column
[**GetCoordinateY**](#GetCoordinateY)|Return the Y position (in coordinate space) for the given pixel row
[**GetGuid**](#GetGuid)|Every plot has a globally unique ID (GUID) that can help differentiate it from other plots
[**GetHashCode**](#GetHashCode)|Returns an integer unique to this instance (based on the GUID)
[**GetNextColor**](#GetNextColor)|Return a new color from the Pallette based on the number of plottables already in the plot. Use this to ensure every plottable gets a unique color.
[**GetPixel**](#GetPixel)|Return the pixel for the given point in coordinate space
[**GetPixelX**](#GetPixelX)|Return the horizontal pixel location given position in coordinate space
[**GetPixelY**](#GetPixelY)|Return the vertical pixel location given position in coordinate space
[**GetPlottables**](#GetPlottables)|Return a copy of the list of plottables
[**GetSettings**](#GetSettings)|The Settings module stores manages plot state and advanced configuration. Its class structure changes frequently, and users are highly advised not to interact with it directly. This method returns the settings module for advanced users and developers to interact with.
[**Grid**](#Grid)|Customize basic options for the primary X and Y axes. Call XAxis.Grid() and YAxis.Grid() to further customize grid settings.
[**Layout**](#Layout)|Set padding around the data area by defining the minimum size and padding for all axes
[**Legend**](#Legend)|Configure legend visibility and location. Optionally you can further customize the legend by interacting with the object it returns.
[**MatchLayout**](#MatchLayout)|Adjust this axis layout based on the layout of a source plot
[**Palette**](#Palette)|The palette defines the default colors given to plottables when they are added
[**Remove**](#Remove)|Remove a specific plottable
[**Render**](#Render)|Render the plot onto a new Bitmap (using the size given when the plot was created or resized)
[**Render**](#Render)|Render the plot onto a new Bitmap of the given dimensions
[**Render**](#Render)|Render the plot onto an existing bitmap
[**RenderLegend**](#RenderLegend)|Return a new Bitmap containing only the legend
[**RenderLock**](#RenderLock)|Wait for the current render to finish, then prevent future renders until RenderUnlock() is called. Locking rendering is required if you intend to modify plottables while rendering is occurring in another thread.
[**RenderUnlock**](#RenderUnlock)|Release the render lock, allowing renders to proceed.
[**Resize**](#Resize)|Update the default size for new renders
[**SaveFig**](#SaveFig)|Save the plot as an image
[**SetAxisLimits**](#SetAxisLimits)|Set limits for the a given pair of axes
[**SetAxisLimits**](#SetAxisLimits)|Set limits for a pair of axes
[**SetAxisLimitsX**](#SetAxisLimitsX)|Set limits for the primary X axis
[**SetAxisLimitsY**](#SetAxisLimitsY)|Set limits for the primary Y axis
[**SetCulture**](#SetCulture)|Set the culture to use for number-to-string converstion for tick labels of all axes.
[**SetCulture**](#SetCulture)|Set the culture to use for number-to-string converstion for tick labels of all axes. This overload allows you to manually define every format string, allowing extensive customization of number and date formatting.
[**SetViewLimits**](#SetViewLimits)|Set limits of the view for the primary axes. View limits define the boundaries of axis limits. You cannot zoom, pan, or set axis limits beyond view limits.
[**Style**](#Style)|Set colors of all plot components using pre-defined styles
[**Style**](#Style)|Set the color of specific plot components
[**Title**](#Title)|Set the label for the horizontal axis above the plot (XAxis2)
[**Validate**](#Validate)|Throw an exception if any plottable contains an invalid state.
[**XLabel**](#XLabel)|Set the label for the vertical axis to the right of the plot (XAxis)
[**XTicks**](#XTicks)|Manually define X axis tick labels using consecutive integer positions (0, 1, 2, etc.)
[**XTicks**](#XTicks)|Manually define X axis tick positions and labels
[**YLabel**](#YLabel)|Set the label for the vertical axis to the right of the plot (YAxis2)
[**YTicks**](#YTicks)|Manually define Y axis tick labels using consecutive integer positions (0, 1, 2, etc.)
[**YTicks**](#YTicks)|Manually define Y axis tick positions and labels
### Shortcuts for Adding Plottables
Method | Summary
---|---
[**AddAnnotation**](#AddAnnotation)|Display text in the data area at a pixel location (not a X/Y coordinates)
[**AddArrow**](#AddArrow)|Display an arrow pointing to a spot in coordinate space
[**AddBar**](#AddBar)|Add a bar plot for the given values. Bars will be placed at X positions 0, 1, 2, etc.
[**AddBar**](#AddBar)|Add a bar plot for the given values using defined bar positions
[**AddBar**](#AddBar)|Add a bar plot (values +/- errors) using defined positions
[**AddBarGroups**](#AddBarGroups)|Create a series of bar plots and customize the ticks and legend
[**AddCandlesticks**](#AddCandlesticks)|Add candlesticks to the chart from OHLC (open, high, low, close) data
[**AddColorbar**](#AddColorbar)|Add a colorbar to display a colormap beside the data area
[**AddColorbar**](#AddColorbar)|Add a colorbar initialized with settings from a heatmap
[**AddFill**](#AddFill)|Create a polygon to fill the area between Y values and a baseline.
[**AddFill**](#AddFill)|Create a polygon to fill the area between Y values of two curves.
[**AddFillAboveAndBelow**](#AddFillAboveAndBelow)|Create a polygon to fill the area between Y values and a baseline that uses two different colors for area above and area below the baseline.
[**AddFunction**](#AddFunction)|
[**AddHeatmap**](#AddHeatmap)|Add a heatmap to the plot
[**AddHorizontalLine**](#AddHorizontalLine)|Add a horizontal axis line at a specific Y position
[**AddHorizontalSpan**](#AddHorizontalSpan)|Add a horizontal span (shades the region between two X positions)
[**AddImage**](#AddImage)|Display an image at a specific coordinate
[**AddLine**](#AddLine)|Add a line (a scatter plot with two points) to the plot
[**AddLine**](#AddLine)|
[**AddOHLCs**](#AddOHLCs)|Add OHLC (open, high, low, close) data to the plot
[**AddPie**](#AddPie)|Add a pie chart to the plot
[**AddPoint**](#AddPoint)|Add a point (a scatter plot with a single marker)
[**AddPolygon**](#AddPolygon)|Add a polygon to the plot
[**AddPolygons**](#AddPolygons)|
[**AddPopulation**](#AddPopulation)|Add a population to the plot
[**AddPopulations**](#AddPopulations)|Add multiple populations to the plot as a single series
[**AddPopulations**](#AddPopulations)|Add multiple populations to the plot as a single series
[**AddRadar**](#AddRadar)|Add a radar plot
[**AddScaleBar**](#AddScaleBar)|Add an L-shaped scalebar to the corner of the plot
[**AddScatter**](#AddScatter)|Add a scatter plot from X/Y pairs. Lines and markers are shown by default. Scatter plots are slower than Signal plots.
[**AddScatterLines**](#AddScatterLines)|Add a scatter plot from X/Y pairs connected by lines (no markers). Scatter plots are slower than Signal plots.
[**AddScatterList**](#AddScatterList)|Scatter plot with Add() and Clear() methods for updating data
[**AddScatterPoints**](#AddScatterPoints)|Add a scatter plot from X/Y pairs using markers at points (no lines). Scatter plots are slower than Signal plots.
[**AddSignal**](#AddSignal)|Signal plots have evenly-spaced X points and render very fast.
[**AddSignalConst**](#AddSignalConst)|SignalConts plots have evenly-spaced X points and render faster than Signal plots but data in source arrays cannot be changed after it is loaded. Methods can be used to update all or portions of the data.
[**AddSignalXY**](#AddSignalXY)|Speed-optimized plot for Ys with unevenly-spaced ascending Xs
[**AddSignalXYConst**](#AddSignalXYConst)|Speed-optimized plot for Ys with unevenly-spaced ascending Xs. Faster than SignalXY but values cannot be modified after loading.
[**AddText**](#AddText)|Display text at specific X/Y coordinates
[**AddText**](#AddText)|Display text at specific X/Y coordinates
[**AddTooltip**](#AddTooltip)|Display a text bubble that points to an X/Y location on the plot
[**AddVectorField**](#AddVectorField)|Add a 2D vector field to the plot
[**AddVerticalLine**](#AddVerticalLine)|Add a vertical axis line at a specific Y position
[**AddVerticalSpan**](#AddVerticalSpan)|Add a horizontal span (shades the region between two X positions)

## Add()

**Summary:** Add a plottable to the plot

**Parameters:**
* `ScottPlot.Plottable.IPlottable` plottable

**Returns:**
* `Void`

## AddAnnotation()

**Summary:** Display text in the data area at a pixel location (not a X/Y coordinates)

**Parameters:**
* `string` label
* `double` x
* `double` y

**Returns:**
* `ScottPlot.Plottable.Annotation`

## AddArrow()

**Summary:** Display an arrow pointing to a spot in coordinate space

**Parameters:**
* `double` xTip
* `double` yTip
* `double` xBase
* `double` yBase
* `float` lineWidth
* `Drawing.Color?` color

**Returns:**
* `ScottPlot.Plottable.ScatterPlot`

## AddAxis()

**Summary:** Create and return an additional axis

**Parameters:**
* `ScottPlot.Renderable.Edge` edge
* `int` axisIndex
* `string` title
* `Drawing.Color?` color

**Returns:**
* `ScottPlot.Renderable.Axis`

## AddBar()

**Summary:** Add a bar plot for the given values. Bars will be placed at X positions 0, 1, 2, etc.

**Parameters:**
* `Double[]` values
* `Drawing.Color?` color

**Returns:**
* `ScottPlot.Plottable.BarPlot`

## AddBar()

**Summary:** Add a bar plot for the given values using defined bar positions

**Parameters:**
* `Double[]` values
* `Double[]` positions
* `Drawing.Color?` color

**Returns:**
* `ScottPlot.Plottable.BarPlot`

## AddBar()

**Summary:** Add a bar plot (values +/- errors) using defined positions

**Parameters:**
* `Double[]` values
* `Double[]` errors
* `Double[]` positions
* `Drawing.Color?` color

**Returns:**
* `ScottPlot.Plottable.BarPlot`

## AddBarGroups()

**Summary:** Create a series of bar plots and customize the ticks and legend

**Parameters:**
* `String[]` groupLabels
* `String[]` seriesLabels
* `Double[][]` ys
* `Double[][]` yErr

**Returns:**
* `ScottPlot.Plottable.BarPlot[]`

## AddCandlesticks()

**Summary:** Add candlesticks to the chart from OHLC (open, high, low, close) data

**Parameters:**
* `ScottPlot.OHLC[]` ohlcs

**Returns:**
* `ScottPlot.Plottable.FinancePlot`

## AddColorbar()

**Summary:** Add a colorbar to display a colormap beside the data area

**Parameters:**
* `ScottPlot.Drawing.Colormap` colormap
* `int` space

**Returns:**
* `ScottPlot.Plottable.Colorbar`

## AddColorbar()

**Summary:** Add a colorbar initialized with settings from a heatmap

**Parameters:**
* `ScottPlot.Plottable.Heatmap` heatmap
* `int` space

**Returns:**
* `ScottPlot.Plottable.Colorbar`

## AddFill()

**Summary:** Create a polygon to fill the area between Y values and a baseline.

**Parameters:**
* `Double[]` xs
* `Double[]` ys
* `double` baseline
* `Drawing.Color?` color

**Returns:**
* `ScottPlot.Plottable.Polygon`

## AddFill()

**Summary:** Create a polygon to fill the area between Y values of two curves.

**Parameters:**
* `Double[]` xs1
* `Double[]` ys1
* `Double[]` xs2
* `Double[]` ys2
* `Drawing.Color?` color

**Returns:**
* `ScottPlot.Plottable.Polygon`

## AddFillAboveAndBelow()

**Summary:** Create a polygon to fill the area between Y values and a baseline that uses two different colors for area above and area below the baseline.

**Parameters:**
* `Double[]` xs
* `Double[]` ys
* `double` baseline
* `Drawing.Color?` colorAbove
* `Drawing.Color?` colorBelow

**Returns:**
* `ValueTuple<T>`

## AddFunction()

> **WARNING:** This method does not have XML documentation

**Parameters:**
* `Double?` function
* `Drawing.Color?` color
* `double` lineWidth
* `ScottPlot.LineStyle` lineStyle

**Returns:**
* `ScottPlot.Plottable.FunctionPlot`

## AddHeatmap()

**Summary:** Add a heatmap to the plot

**Parameters:**
* `Double[,]` intensities
* `ScottPlot.Drawing.Colormap` colormap
* `Boolean` lockScales

**Returns:**
* `ScottPlot.Plottable.Heatmap`

## AddHorizontalLine()

**Summary:** Add a horizontal axis line at a specific Y position

**Parameters:**
* `double` y
* `Drawing.Color?` color
* `float` width
* `ScottPlot.LineStyle` style
* `string` label

**Returns:**
* `ScottPlot.Plottable.HLine`

## AddHorizontalSpan()

**Summary:** Add a horizontal span (shades the region between two X positions)

**Parameters:**
* `double` xMin
* `double` xMax
* `Drawing.Color?` color
* `string` label

**Returns:**
* `ScottPlot.Plottable.HSpan`

## AddImage()

**Summary:** Display an image at a specific coordinate

**Parameters:**
* `Drawing.Bitmap` bitmap
* `double` x
* `double` y

**Returns:**
* `ScottPlot.Plottable.Image`

## AddLine()

**Summary:** Add a line (a scatter plot with two points) to the plot

**Parameters:**
* `double` x1
* `double` y1
* `double` x2
* `double` y2
* `Drawing.Color?` color
* `float` lineWidth

**Returns:**
* `ScottPlot.Plottable.ScatterPlot`

## AddLine()

> **WARNING:** This method does not have XML documentation

**Parameters:**
* `double` slope
* `double` offset
* `ValueTuple<T>` xLimits
* `Drawing.Color?` color
* `float` lineWidth

**Returns:**
* `ScottPlot.Plottable.ScatterPlot`

## AddOHLCs()

**Summary:** Add OHLC (open, high, low, close) data to the plot

**Parameters:**
* `ScottPlot.OHLC[]` ohlcs

**Returns:**
* `ScottPlot.Plottable.FinancePlot`

## AddPie()

**Summary:** Add a pie chart to the plot

**Parameters:**
* `Double[]` values
* `Boolean` hideGridAndFrame

**Returns:**
* `ScottPlot.Plottable.PiePlot`

## AddPoint()

**Summary:** Add a point (a scatter plot with a single marker)

**Parameters:**
* `double` x
* `double` y
* `Drawing.Color?` color
* `float` size
* `ScottPlot.MarkerShape` shape

**Returns:**
* `ScottPlot.Plottable.ScatterPlot`

## AddPolygon()

**Summary:** Add a polygon to the plot

**Parameters:**
* `Double[]` xs
* `Double[]` ys
* `Drawing.Color?` fillColor
* `double` lineWidth
* `Drawing.Color?` lineColor

**Returns:**
* `ScottPlot.Plottable.Polygon`

## AddPolygons()

> **WARNING:** This method does not have XML documentation

**Parameters:**
* `Collections.Generic.List<T>` polys
* `Drawing.Color?` fillColor
* `double` lineWidth
* `Drawing.Color?` lineColor

**Returns:**
* `ScottPlot.Plottable.Polygons`

## AddPopulation()

**Summary:** Add a population to the plot

**Parameters:**
* `ScottPlot.Statistics.Population` population
* `string` label

**Returns:**
* `ScottPlot.Plottable.PopulationPlot`

## AddPopulations()

**Summary:** Add multiple populations to the plot as a single series

**Parameters:**
* `ScottPlot.Statistics.Population[]` populations
* `string` label

**Returns:**
* `ScottPlot.Plottable.PopulationPlot`

## AddPopulations()

**Summary:** Add multiple populations to the plot as a single series

**Parameters:**
* `ScottPlot.Statistics.PopulationMultiSeries` multiSeries

**Returns:**
* `ScottPlot.Plottable.PopulationPlot`

## AddRadar()

**Summary:** Add a radar plot

**Parameters:**
* `Double[,]` values
* `Boolean` independentAxes
* `Double[]` maxValues
* `Boolean` disableFrameAndGrid

**Returns:**
* `ScottPlot.Plottable.RadarPlot`

## AddScaleBar()

**Summary:** Add an L-shaped scalebar to the corner of the plot

**Parameters:**
* `double` width
* `double` height
* `string` xLabel
* `string` yLabel

**Returns:**
* `ScottPlot.Plottable.ScaleBar`

## AddScatter()

**Summary:** Add a scatter plot from X/Y pairs. Lines and markers are shown by default. Scatter plots are slower than Signal plots.

**Parameters:**
* `Double[]` xs
* `Double[]` ys
* `Drawing.Color?` color
* `float` lineWidth
* `float` markerSize
* `ScottPlot.MarkerShape` markerShape
* `ScottPlot.LineStyle` lineStyle
* `string` label

**Returns:**
* `ScottPlot.Plottable.ScatterPlot`

## AddScatterLines()

**Summary:** Add a scatter plot from X/Y pairs connected by lines (no markers). Scatter plots are slower than Signal plots.

**Parameters:**
* `Double[]` xs
* `Double[]` ys
* `Drawing.Color?` color
* `float` lineWidth
* `ScottPlot.LineStyle` lineStyle
* `string` label

**Returns:**
* `ScottPlot.Plottable.ScatterPlot`

## AddScatterList()

**Summary:** Scatter plot with Add() and Clear() methods for updating data

**Parameters:**
* `Drawing.Color?` color
* `float` lineWidth
* `float` markerSize
* `string` label
* `ScottPlot.MarkerShape` markerShape
* `ScottPlot.LineStyle` lineStyle

**Returns:**
* `ScottPlot.Plottable.ScatterPlotList`

## AddScatterPoints()

**Summary:** Add a scatter plot from X/Y pairs using markers at points (no lines). Scatter plots are slower than Signal plots.

**Parameters:**
* `Double[]` xs
* `Double[]` ys
* `Drawing.Color?` color
* `float` markerSize
* `ScottPlot.MarkerShape` markerShape
* `string` label

**Returns:**
* `ScottPlot.Plottable.ScatterPlot`

## AddSignal()

**Summary:** Signal plots have evenly-spaced X points and render very fast.

**Parameters:**
* `Double[]` ys
* `double` sampleRate
* `Drawing.Color?` color
* `string` label

**Returns:**
* `ScottPlot.Plottable.SignalPlot`

## AddSignalConst()

**Summary:** SignalConts plots have evenly-spaced X points and render faster than Signal plots but data in source arrays cannot be changed after it is loaded. Methods can be used to update all or portions of the data.

**Parameters:**
* `T[]` ys
* `double` sampleRate
* `Drawing.Color?` color
* `string` label

**Returns:**
* `SignalPlotConst<T>`

## AddSignalXY()

**Summary:** Speed-optimized plot for Ys with unevenly-spaced ascending Xs

**Parameters:**
* `Double[]` xs
* `Double[]` ys
* `Drawing.Color?` color
* `string` label

**Returns:**
* `ScottPlot.Plottable.SignalPlotXY`

## AddSignalXYConst()

**Summary:** Speed-optimized plot for Ys with unevenly-spaced ascending Xs. Faster than SignalXY but values cannot be modified after loading.

**Parameters:**
* `TX[]` xs
* `TY[]` ys
* `Drawing.Color?` color
* `string` label

**Returns:**
* `SignalPlotXYConst<T>`

## AddText()

**Summary:** Display text at specific X/Y coordinates

**Parameters:**
* `string` label
* `double` x
* `double` y
* `float` size
* `Drawing.Color?` color

**Returns:**
* `ScottPlot.Plottable.Text`

## AddText()

**Summary:** Display text at specific X/Y coordinates

**Parameters:**
* `string` label
* `double` x
* `double` y
* `ScottPlot.Drawing.Font` font

**Returns:**
* `ScottPlot.Plottable.Text`

## AddTooltip()

**Summary:** Display a text bubble that points to an X/Y location on the plot

**Parameters:**
* `string` label
* `double` x
* `double` y

**Returns:**
* `ScottPlot.Plottable.Tooltip`

## AddVectorField()

**Summary:** Add a 2D vector field to the plot

**Parameters:**
* `ScottPlot.Statistics.Vector2[,]` vectors
* `Double[]` xs
* `Double[]` ys
* `string` label
* `Drawing.Color?` color
* `ScottPlot.Drawing.Colormap` colormap
* `double` scaleFactor

**Returns:**
* `ScottPlot.Plottable.VectorField`

## AddVerticalLine()

**Summary:** Add a vertical axis line at a specific Y position

**Parameters:**
* `double` x
* `Drawing.Color?` color
* `float` width
* `ScottPlot.LineStyle` style
* `string` label

**Returns:**
* `ScottPlot.Plottable.VLine`

## AddVerticalSpan()

**Summary:** Add a horizontal span (shades the region between two X positions)

**Parameters:**
* `double` yMin
* `double` yMax
* `Drawing.Color?` color
* `string` label

**Returns:**
* `ScottPlot.Plottable.VSpan`

## AxisAuto()

**Summary:** Automatically adjust axis limits to fit the data

**Parameters:**
* `double` horizontalMargin
* `double` verticalMargin

**Returns:**
* `Void`

## AxisAutoX()

**Summary:** Automatically adjust axis limits to fit the data

**Parameters:**
* `double` margin

**Returns:**
* `Void`

## AxisAutoY()

**Summary:** Automatically adjust axis limits to fit the data (with a little extra margin)

**Parameters:**
* `double` margin

**Returns:**
* `Void`

## AxisPan()

**Summary:** Pan the primary X and Y axis without affecting zoom

**Parameters:**
* `double` dx
* `double` dy

**Returns:**
* `Void`

## AxisScale()

**Summary:** Adjust axis limits to achieve a certain pixel scale (units per pixel)

**Parameters:**
* `Double?` unitsPerPixelX
* `Double?` unitsPerPixelY

**Returns:**
* `Void`

## AxisScaleLock()

**Summary:** Lock X and Y axis scales (units per pixel) together to protect symmetry of circles and squares

**Parameters:**
* `Boolean` enable

**Returns:**
* `Void`

## AxisZoom()

**Summary:** Zoom in or out. The amount of zoom is defined as a fraction of the current axis span.

**Parameters:**
* `double` xFrac
* `double` yFrac
* `Double?` zoomToX
* `Double?` zoomToY
* `int` xAxisIndex
* `int` yAxisIndex

**Returns:**
* `Void`

## Benchmark()

**Summary:** If enabled, the benchmark displays render information in the corner of the plot.

**Parameters:**
* `Boolean?` enable

**Returns:**
* `Boolean`

## Clear()

**Summary:** Clear all plottables

**Parameters:**

**Returns:**
* `Void`

## Clear()

**Summary:** Remove all plottables of the given type

**Parameters:**
* `Type` plottableType

**Returns:**
* `Void`

## Copy()

**Summary:** Return a new Plot with all the same Plottables (and some of the styles) of this one.

**Parameters:**

**Returns:**
* `ScottPlot.Plot`

## Equals()

**Summary:** Returns true if the given plot is the exact same plot as this one

**Parameters:**
* `Object` obj

**Returns:**
* `Boolean`

## Frame()

**Summary:** Configure color and visibility of the frame that outlines the data area. Note that the axis lines of all 4 primary axes touch each other, giving the appearance of a rectangle framing the data area. This method allows the user to customize these lines as a group or individually.

**Parameters:**
* `Boolean?` visible
* `Drawing.Color?` color
* `Boolean?` left
* `Boolean?` right
* `Boolean?` bottom
* `Boolean?` top

**Returns:**
* `Void`

## Frameless()

**Summary:** Give the plot a frameless appearance by setting the size of all axes to zero. This causes the data area to go right up to the edge of the plot.

**Parameters:**

**Returns:**
* `Void`

## GetAxisLimits()

**Summary:** Returns the current limits for a given pair of axes.

**Parameters:**
* `int` xAxisIndex
* `int` yAxisIndex

**Returns:**
* `ScottPlot.AxisLimits`

## GetCoordinate()

**Summary:** Return the coordinate (in coordinate space) for the given pixel

**Parameters:**
* `float` xPixel
* `float` yPixel

**Returns:**
* `ValueTuple<T>`

## GetCoordinateX()

**Summary:** Return the X position (in coordinate space) for the given pixel column

**Parameters:**
* `float` xPixel

**Returns:**
* `double`

## GetCoordinateY()

**Summary:** Return the Y position (in coordinate space) for the given pixel row

**Parameters:**
* `float` yPixel

**Returns:**
* `double`

## GetGuid()

**Summary:** Every plot has a globally unique ID (GUID) that can help differentiate it from other plots

**Parameters:**

**Returns:**
* `string`

## GetHashCode()

**Summary:** Returns an integer unique to this instance (based on the GUID)

**Parameters:**

**Returns:**
* `int`

## GetNextColor()

**Summary:** Return a new color from the Pallette based on the number of plottables already in the plot. Use this to ensure every plottable gets a unique color.

**Parameters:**
* `double` alpha

**Returns:**
* `Drawing.Color`

## GetPixel()

**Summary:** Return the pixel for the given point in coordinate space

**Parameters:**
* `double` x
* `double` y

**Returns:**
* `ValueTuple<T>`

## GetPixelX()

**Summary:** Return the horizontal pixel location given position in coordinate space

**Parameters:**
* `double` x

**Returns:**
* `float`

## GetPixelY()

**Summary:** Return the vertical pixel location given position in coordinate space

**Parameters:**
* `double` y

**Returns:**
* `float`

## GetPlottables()

**Summary:** Return a copy of the list of plottables

**Parameters:**

**Returns:**
* `ScottPlot.Plottable.IPlottable[]`

## GetSettings()

**Summary:** The Settings module stores manages plot state and advanced configuration. Its class structure changes frequently, and users are highly advised not to interact with it directly. This method returns the settings module for advanced users and developers to interact with.

**Parameters:**
* `Boolean` showWarning

**Returns:**
* `ScottPlot.Settings`

## Grid()

**Summary:** Customize basic options for the primary X and Y axes. Call XAxis.Grid() and YAxis.Grid() to further customize grid settings.

**Parameters:**
* `Boolean?` enable
* `Drawing.Color?` color
* `ScottPlot.LineStyle?` lineStyle

**Returns:**
* `Void`

## Layout()

**Summary:** Set padding around the data area by defining the minimum size and padding for all axes

**Parameters:**
* `Single?` left
* `Single?` right
* `Single?` bottom
* `Single?` top
* `Single?` padding

**Returns:**
* `Void`

## Legend()

**Summary:** Configure legend visibility and location. Optionally you can further customize the legend by interacting with the object it returns.

**Parameters:**
* `Boolean` enable
* `ScottPlot.Alignment` location

**Returns:**
* `ScottPlot.Renderable.Legend`

## MatchLayout()

**Summary:** Adjust this axis layout based on the layout of a source plot

**Parameters:**
* `ScottPlot.Plot` sourcePlot
* `Boolean` horizontal
* `Boolean` vertical

**Returns:**
* `Void`

## Palette()

**Summary:** The palette defines the default colors given to plottables when they are added

**Parameters:**
* `ScottPlot.Drawing.Palette` palette

**Returns:**
* `ScottPlot.Drawing.Palette`

## Remove()

**Summary:** Remove a specific plottable

**Parameters:**
* `ScottPlot.Plottable.IPlottable` plottable

**Returns:**
* `Void`

## Render()

**Summary:** Render the plot onto a new Bitmap (using the size given when the plot was created or resized)

**Parameters:**
* `Boolean` lowQuality

**Returns:**
* `Drawing.Bitmap`

## Render()

**Summary:** Render the plot onto a new Bitmap of the given dimensions

**Parameters:**
* `int` width
* `int` height
* `Boolean` lowQuality

**Returns:**
* `Drawing.Bitmap`

## Render()

**Summary:** Render the plot onto an existing bitmap

**Parameters:**
* `Drawing.Bitmap` bmp
* `Boolean` lowQuality

**Returns:**
* `Drawing.Bitmap`

## RenderLegend()

**Summary:** Return a new Bitmap containing only the legend

**Parameters:**

**Returns:**
* `Drawing.Bitmap`

## RenderLock()

**Summary:** Wait for the current render to finish, then prevent future renders until RenderUnlock() is called. Locking rendering is required if you intend to modify plottables while rendering is occurring in another thread.

**Parameters:**

**Returns:**
* `Void`

## RenderUnlock()

**Summary:** Release the render lock, allowing renders to proceed.

**Parameters:**

**Returns:**
* `Void`

## Resize()

**Summary:** Update the default size for new renders

**Parameters:**
* `float` width
* `float` height

**Returns:**
* `Void`

## SaveFig()

**Summary:** Save the plot as an image

**Parameters:**
* `string` filePath

**Returns:**
* `string`

## SetAxisLimits()

**Summary:** Set limits for the a given pair of axes

**Parameters:**
* `Double?` xMin
* `Double?` xMax
* `Double?` yMin
* `Double?` yMax
* `int` xAxisIndex
* `int` yAxisIndex

**Returns:**
* `Void`

## SetAxisLimits()

**Summary:** Set limits for a pair of axes

**Parameters:**
* `ScottPlot.AxisLimits` limits
* `int` xAxisIndex
* `int` yAxisIndex

**Returns:**
* `Void`

## SetAxisLimitsX()

**Summary:** Set limits for the primary X axis

**Parameters:**
* `double` xMin
* `double` xMax

**Returns:**
* `Void`

## SetAxisLimitsY()

**Summary:** Set limits for the primary Y axis

**Parameters:**
* `double` yMin
* `double` yMax

**Returns:**
* `Void`

## SetCulture()

**Summary:** Set the culture to use for number-to-string converstion for tick labels of all axes.

**Parameters:**
* `Globalization.CultureInfo` culture

**Returns:**
* `Void`

## SetCulture()

**Summary:** Set the culture to use for number-to-string converstion for tick labels of all axes. This overload allows you to manually define every format string, allowing extensive customization of number and date formatting.

**Parameters:**
* `string` shortDatePattern
* `string` decimalSeparator
* `string` numberGroupSeparator
* `Int32?` decimalDigits
* `Int32?` numberNegativePattern
* `Int32[]` numberGroupSizes

**Returns:**
* `Void`

## SetViewLimits()

**Summary:** Set limits of the view for the primary axes. View limits define the boundaries of axis limits. You cannot zoom, pan, or set axis limits beyond view limits.

**Parameters:**
* `double` xMin
* `double` xMax
* `double` yMin
* `double` yMax

**Returns:**
* `Void`

## Style()

**Summary:** Set colors of all plot components using pre-defined styles

**Parameters:**
* `ScottPlot.Style` style

**Returns:**
* `Void`

## Style()

**Summary:** Set the color of specific plot components

**Parameters:**
* `Drawing.Color?` figureBackground
* `Drawing.Color?` dataBackground
* `Drawing.Color?` grid
* `Drawing.Color?` tick
* `Drawing.Color?` axisLabel
* `Drawing.Color?` titleLabel

**Returns:**
* `Void`

## Title()

**Summary:** Set the label for the horizontal axis above the plot (XAxis2)

**Parameters:**
* `string` label
* `Boolean` bold

**Returns:**
* `Void`

## Validate()

**Summary:** Throw an exception if any plottable contains an invalid state.

**Parameters:**
* `Boolean` deep

**Returns:**
* `Void`

## XLabel()

**Summary:** Set the label for the vertical axis to the right of the plot (XAxis)

**Parameters:**
* `string` label

**Returns:**
* `Void`

## XTicks()

**Summary:** Manually define X axis tick labels using consecutive integer positions (0, 1, 2, etc.)

**Parameters:**
* `String[]` labels

**Returns:**
* `Void`

## XTicks()

**Summary:** Manually define X axis tick positions and labels

**Parameters:**
* `Double[]` positions
* `String[]` labels

**Returns:**
* `Void`

## YLabel()

**Summary:** Set the label for the vertical axis to the right of the plot (YAxis2)

**Parameters:**
* `string` label

**Returns:**
* `Void`

## YTicks()

**Summary:** Manually define Y axis tick labels using consecutive integer positions (0, 1, 2, etc.)

**Parameters:**
* `String[]` labels

**Returns:**
* `Void`

## YTicks()

**Summary:** Manually define Y axis tick positions and labels

**Parameters:**
* `Double[]` positions
* `String[]` labels

**Returns:**
* `Void`
