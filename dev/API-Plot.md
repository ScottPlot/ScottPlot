# ScottPlot.Plot API
## Add()
_Add a plottable to the plot_
* Returns `Void`
* Parameters
  * `ScottPlot.Plottable.IPlottable` **`plottable`**
## AddAnnotation()
_Display text in the data area at a pixel location (not a X/Y coordinates)_
* Returns `ScottPlot.Plottable.Annotation`
* Parameters
  * `string` **`label`**
  * `double` **`x`**
  * `double` **`y`**
## AddArrow()
_Display an arrow pointing to a spot in coordinate space_
* Returns `ScottPlot.Plottable.ScatterPlot`
* Parameters
  * `double` **`xTip`**
  * `double` **`yTip`**
  * `double` **`xBase`**
  * `double` **`yBase`**
  * `float` **`lineWidth`**
  * `System.Drawing.Color?` **`color`**
## AddAxis()
_Create and return an additional axis_
* Returns `ScottPlot.Renderable.Axis`
* Parameters
  * `ScottPlot.Renderable.Edge` **`edge`**
  * `int` **`axisIndex`**
  * `string` **`title`**
  * `System.Drawing.Color?` **`color`**
## AddBar()
_Add a bar plot for the given values. Bars will be placed at X positions 0, 1, 2, etc._
* Returns `ScottPlot.Plottable.BarPlot`
* Parameters
  * `double[]` **`values`**
  * `System.Drawing.Color?` **`color`**
## AddBar()
_Add a bar plot for the given values using defined bar positions_
* Returns `ScottPlot.Plottable.BarPlot`
* Parameters
  * `double[]` **`values`**
  * `double[]` **`positions`**
  * `System.Drawing.Color?` **`color`**
## AddBar()
_Add a bar plot (values +/- errors) using defined positions_
* Returns `ScottPlot.Plottable.BarPlot`
* Parameters
  * `double[]` **`values`**
  * `double[]` **`errors`**
  * `double[]` **`positions`**
  * `System.Drawing.Color?` **`color`**
## AddBarGroups()
_Create a series of bar plots and customize the ticks and legend_
* Returns `ScottPlot.Plottable.BarPlot[]`
* Parameters
  * `string[]` **`groupLabels`**
  * `string[]` **`seriesLabels`**
  * `Double[][]` **`ys`**
  * `Double[][]` **`yErr`**
## AddCandlesticks()
_Add candlesticks to the chart from OHLC (open, high, low, close) data_
* Returns `ScottPlot.Plottable.FinancePlot`
* Parameters
  * `ScottPlot.OHLC[]` **`ohlcs`**
## AddColorbar()
_Add a colorbar to display a colormap beside the data area_
* Returns `ScottPlot.Plottable.Colorbar`
* Parameters
  * `ScottPlot.Drawing.Colormap` **`colormap`**
  * `int` **`space`**
## AddColorbar()
_Add a colorbar initialized with settings from a heatmap_
* Returns `ScottPlot.Plottable.Colorbar`
* Parameters
  * `ScottPlot.Plottable.Heatmap` **`heatmap`**
  * `int` **`space`**
## AddFill()
_Create a polygon to fill the area between Y values and a baseline._
* Returns `ScottPlot.Plottable.Polygon`
* Parameters
  * `double[]` **`xs`**
  * `double[]` **`ys`**
  * `double` **`baseline`**
  * `System.Drawing.Color?` **`color`**
## AddFill()
_Create a polygon to fill the area between Y values of two curves._
* Returns `ScottPlot.Plottable.Polygon`
* Parameters
  * `double[]` **`xs1`**
  * `double[]` **`ys1`**
  * `double[]` **`xs2`**
  * `double[]` **`ys2`**
  * `System.Drawing.Color?` **`color`**
## AddFillAboveAndBelow()
_Create a polygon to fill the area between Y values and a baseline
            that uses two different colors for area above and area below the baseline._
* Returns `ValueTuple`2[ScottPlot.Plottable.Polygon,ScottPlot.Plottable.Polygon]`
* Parameters
  * `double[]` **`xs`**
  * `double[]` **`ys`**
  * `double` **`baseline`**
  * `System.Drawing.Color?` **`colorAbove`**
  * `System.Drawing.Color?` **`colorBelow`**
## AddFunction()
_Add a line plot that uses a function (rather than X/Y points) to place the curve_
* Returns `ScottPlot.Plottable.FunctionPlot`
* Parameters
  * `Func`2[Double,Nullable`1[Double]]` **`function`**
  * `System.Drawing.Color?` **`color`**
  * `double` **`lineWidth`**
  * `ScottPlot.LineStyle` **`lineStyle`**
## AddHeatmap()
_Add a heatmap to the plot_
* Returns `ScottPlot.Plottable.Heatmap`
* Parameters
  * `Double[,]` **`intensities`**
  * `ScottPlot.Drawing.Colormap` **`colormap`**
  * `Boolean` **`lockScales`**
## AddHorizontalLine()
_Add a horizontal axis line at a specific Y position_
* Returns `ScottPlot.Plottable.HLine`
* Parameters
  * `double` **`y`**
  * `System.Drawing.Color?` **`color`**
  * `float` **`width`**
  * `ScottPlot.LineStyle` **`style`**
  * `string` **`label`**
## AddHorizontalSpan()
_Add a horizontal span (shades the region between two X positions)_
* Returns `ScottPlot.Plottable.HSpan`
* Parameters
  * `double` **`xMin`**
  * `double` **`xMax`**
  * `System.Drawing.Color?` **`color`**
  * `string` **`label`**
## AddImage()
_Display an image at a specific coordinate_
* Returns `ScottPlot.Plottable.Image`
* Parameters
  * `System.Drawing.Bitmap` **`bitmap`**
  * `double` **`x`**
  * `double` **`y`**
## AddLine()
_Add a line (a scatter plot with two points) to the plot_
* Returns `ScottPlot.Plottable.ScatterPlot`
* Parameters
  * `double` **`x1`**
  * `double` **`y1`**
  * `double` **`x2`**
  * `double` **`y2`**
  * `System.Drawing.Color?` **`color`**
  * `float` **`lineWidth`**
## AddLine()
_Add a line (a scatter plot with two points) to the plot_
* Returns `ScottPlot.Plottable.ScatterPlot`
* Parameters
  * `double` **`slope`**
  * `double` **`offset`**
  * `ValueTuple`2[Double,Double]` **`xLimits`**
  * `System.Drawing.Color?` **`color`**
  * `float` **`lineWidth`**
## AddOHLCs()
_Add OHLC (open, high, low, close) data to the plot_
* Returns `ScottPlot.Plottable.FinancePlot`
* Parameters
  * `ScottPlot.OHLC[]` **`ohlcs`**
## AddPie()
_Add a pie chart to the plot_
* Returns `ScottPlot.Plottable.PiePlot`
* Parameters
  * `double[]` **`values`**
  * `Boolean` **`hideGridAndFrame`**
## AddPoint()
_Add a point (a scatter plot with a single marker)_
* Returns `ScottPlot.Plottable.ScatterPlot`
* Parameters
  * `double` **`x`**
  * `double` **`y`**
  * `System.Drawing.Color?` **`color`**
  * `float` **`size`**
  * `ScottPlot.MarkerShape` **`shape`**
## AddPolygon()
_Add a polygon to the plot_
* Returns `ScottPlot.Plottable.Polygon`
* Parameters
  * `double[]` **`xs`**
  * `double[]` **`ys`**
  * `System.Drawing.Color?` **`fillColor`**
  * `double` **`lineWidth`**
  * `System.Drawing.Color?` **`lineColor`**
## AddPolygons()
_Add many polygons using an optimized rendering method_
* Returns `ScottPlot.Plottable.Polygons`
* Parameters
  * `Collections.Generic.List`1[Collections.Generic.List`1[ValueTuple`2[Double,Double]]]` **`polys`**
  * `System.Drawing.Color?` **`fillColor`**
  * `double` **`lineWidth`**
  * `System.Drawing.Color?` **`lineColor`**
## AddPopulation()
_Add a population to the plot_
* Returns `ScottPlot.Plottable.PopulationPlot`
* Parameters
  * `ScottPlot.Statistics.Population` **`population`**
  * `string` **`label`**
## AddPopulations()
_Add multiple populations to the plot as a single series_
* Returns `ScottPlot.Plottable.PopulationPlot`
* Parameters
  * `ScottPlot.Statistics.Population[]` **`populations`**
  * `string` **`label`**
## AddPopulations()
_Add multiple populations to the plot as a single series_
* Returns `ScottPlot.Plottable.PopulationPlot`
* Parameters
  * `ScottPlot.Statistics.PopulationMultiSeries` **`multiSeries`**
## AddRadar()
_Add a radar plot_
* Returns `ScottPlot.Plottable.RadarPlot`
* Parameters
  * `Double[,]` **`values`**
  * `Boolean` **`independentAxes`**
  * `double[]` **`maxValues`**
  * `Boolean` **`disableFrameAndGrid`**
## AddScaleBar()
_Add an L-shaped scalebar to the corner of the plot_
* Returns `ScottPlot.Plottable.ScaleBar`
* Parameters
  * `double` **`width`**
  * `double` **`height`**
  * `string` **`xLabel`**
  * `string` **`yLabel`**
## AddScatter()
_Add a scatter plot from X/Y pairs. 
            Lines and markers are shown by default.
            Scatter plots are slower than Signal plots._
* Returns `ScottPlot.Plottable.ScatterPlot`
* Parameters
  * `double[]` **`xs`**
  * `double[]` **`ys`**
  * `System.Drawing.Color?` **`color`**
  * `float` **`lineWidth`**
  * `float` **`markerSize`**
  * `ScottPlot.MarkerShape` **`markerShape`**
  * `ScottPlot.LineStyle` **`lineStyle`**
  * `string` **`label`**
## AddScatterLines()
_Add a scatter plot from X/Y pairs connected by lines (no markers).
            Scatter plots are slower than Signal plots._
* Returns `ScottPlot.Plottable.ScatterPlot`
* Parameters
  * `double[]` **`xs`**
  * `double[]` **`ys`**
  * `System.Drawing.Color?` **`color`**
  * `float` **`lineWidth`**
  * `ScottPlot.LineStyle` **`lineStyle`**
  * `string` **`label`**
## AddScatterList()
_Scatter plot with Add() and Clear() methods for updating data_
* Returns `ScottPlot.Plottable.ScatterPlotList`
* Parameters
  * `System.Drawing.Color?` **`color`**
  * `float` **`lineWidth`**
  * `float` **`markerSize`**
  * `string` **`label`**
  * `ScottPlot.MarkerShape` **`markerShape`**
  * `ScottPlot.LineStyle` **`lineStyle`**
## AddScatterPoints()
_Add a scatter plot from X/Y pairs using markers at points (no lines).
            Scatter plots are slower than Signal plots._
* Returns `ScottPlot.Plottable.ScatterPlot`
* Parameters
  * `double[]` **`xs`**
  * `double[]` **`ys`**
  * `System.Drawing.Color?` **`color`**
  * `float` **`markerSize`**
  * `ScottPlot.MarkerShape` **`markerShape`**
  * `string` **`label`**
## AddSignal()
_Signal plots have evenly-spaced X points and render very fast._
* Returns `ScottPlot.Plottable.SignalPlot`
* Parameters
  * `double[]` **`ys`**
  * `double` **`sampleRate`**
  * `System.Drawing.Color?` **`color`**
  * `string` **`label`**
## AddSignalConst()
_SignalConts plots have evenly-spaced X points and render faster than Signal plots
            but data in source arrays cannot be changed after it is loaded.
            Methods can be used to update all or portions of the data._
* Returns `ScottPlot.Plottable.SignalPlotConst`1[T]`
* Parameters
  * `T[]` **`ys`**
  * `double` **`sampleRate`**
  * `System.Drawing.Color?` **`color`**
  * `string` **`label`**
## AddSignalXY()
_Speed-optimized plot for Ys with unevenly-spaced ascending Xs_
* Returns `ScottPlot.Plottable.SignalPlotXY`
* Parameters
  * `double[]` **`xs`**
  * `double[]` **`ys`**
  * `System.Drawing.Color?` **`color`**
  * `string` **`label`**
## AddSignalXYConst()
_Speed-optimized plot for Ys with unevenly-spaced ascending Xs.
            Faster than SignalXY but values cannot be modified after loading._
* Returns `ScottPlot.Plottable.SignalPlotXYConst`2[TX,TY]`
* Parameters
  * `TX[]` **`xs`**
  * `TY[]` **`ys`**
  * `System.Drawing.Color?` **`color`**
  * `string` **`label`**
## AddText()
_Display text at specific X/Y coordinates_
* Returns `ScottPlot.Plottable.Text`
* Parameters
  * `string` **`label`**
  * `double` **`x`**
  * `double` **`y`**
  * `float` **`size`**
  * `System.Drawing.Color?` **`color`**
## AddText()
_Display text at specific X/Y coordinates_
* Returns `ScottPlot.Plottable.Text`
* Parameters
  * `string` **`label`**
  * `double` **`x`**
  * `double` **`y`**
  * `ScottPlot.Drawing.Font` **`font`**
## AddTooltip()
_Display a text bubble that points to an X/Y location on the plot_
* Returns `ScottPlot.Plottable.Tooltip`
* Parameters
  * `string` **`label`**
  * `double` **`x`**
  * `double` **`y`**
## AddVectorField()
_Add a 2D vector field to the plot_
* Returns `ScottPlot.Plottable.VectorField`
* Parameters
  * `ScottPlot.Statistics.Vector2[,]` **`vectors`**
  * `double[]` **`xs`**
  * `double[]` **`ys`**
  * `string` **`label`**
  * `System.Drawing.Color?` **`color`**
  * `ScottPlot.Drawing.Colormap` **`colormap`**
  * `double` **`scaleFactor`**
## AddVerticalLine()
_Add a vertical axis line at a specific Y position_
* Returns `ScottPlot.Plottable.VLine`
* Parameters
  * `double` **`x`**
  * `System.Drawing.Color?` **`color`**
  * `float` **`width`**
  * `ScottPlot.LineStyle` **`style`**
  * `string` **`label`**
## AddVerticalSpan()
_Add a horizontal span (shades the region between two X positions)_
* Returns `ScottPlot.Plottable.VSpan`
* Parameters
  * `double` **`yMin`**
  * `double` **`yMax`**
  * `System.Drawing.Color?` **`color`**
  * `string` **`label`**
## AxisAuto()
_Automatically adjust axis limits to fit the data (with a little extra margin)_
* Returns `Void`
* Parameters
  * `double` **`horizontalMargin`**
  * `double` **`verticalMargin`**
## AxisAutoX()
_Automatically adjust axis limits to fit the data (with a little extra margin)_
* Returns `Void`
* Parameters
  * `double` **`margin`**
## AxisAutoY()
_Automatically adjust axis limits to fit the data (with a little extra margin)_
* Returns `Void`
* Parameters
  * `double` **`margin`**
## AxisPan()
_Pan by a delta defined in coordinates_
* Returns `Void`
* Parameters
  * `double` **`dx`**
  * `double` **`dy`**
## AxisScale()
_Adjust axis limits to achieve a certain pixel scale (units per pixel)_
* Returns `Void`
* Parameters
  * `Double?` **`unitsPerPixelX`**
  * `Double?` **`unitsPerPixelY`**
## AxisScaleLock()
_Lock X and Y axis scales (units per pixel) together to protect symmetry of circles and squares_
* Returns `Void`
* Parameters
  * `Boolean` **`enable`**
## AxisZoom()
_Zoom by a fraction (zoom in if fraction > 1)_
* Returns `Void`
* Parameters
  * `double` **`xFrac`**
  * `double` **`yFrac`**
  * `Double?` **`zoomToX`**
  * `Double?` **`zoomToY`**
  * `int` **`xAxisIndex`**
  * `int` **`yAxisIndex`**
## Benchmark()
_Display render benchmark information on the plot_
* Returns `Void`
* Parameters
  * `Boolean` **`enable`**
## BenchmarkToggle()
***ERROR: XML DOCS NOT FOUND!***
* Returns `Void`
* Parameters
## Clear()
***ERROR: XML DOCS NOT FOUND!***
* Returns `Void`
* Parameters
## Clear()
_Remove all plottables of the given type_
* Returns `Void`
* Parameters
  * `Type` **`plottableType`**
## Colorset()
_Change the default color palette for new plottables_
* Returns `Void`
* Parameters
  * `ScottPlot.Drawing.Palette` **`colorset`**
## Copy()
***ERROR: XML DOCS NOT FOUND!***
* Returns `ScottPlot.Plot`
* Parameters
## Equals()
***ERROR: XML DOCS NOT FOUND!***
* Returns `Boolean`
* Parameters
  * `Object` **`obj`**
## Frame()
_Configure color and visibility of the frame that outlines the data area (lines along the edges of the primary axes)_
* Returns `Void`
* Parameters
  * `Boolean?` **`visible`**
  * `System.Drawing.Color?` **`color`**
  * `Boolean?` **`left`**
  * `Boolean?` **`right`**
  * `Boolean?` **`bottom`**
  * `Boolean?` **`top`**
## Frameless()
***ERROR: XML DOCS NOT FOUND!***
* Returns `Void`
* Parameters
## get_Version()
***ERROR: XML DOCS NOT FOUND!***
* Returns `String`
* Parameters
## get_XAxis()
***ERROR: XML DOCS NOT FOUND!***
* Returns `ScottPlot.Renderable.Axis`
* Parameters
## get_XAxis2()
***ERROR: XML DOCS NOT FOUND!***
* Returns `ScottPlot.Renderable.Axis`
* Parameters
## get_YAxis()
***ERROR: XML DOCS NOT FOUND!***
* Returns `ScottPlot.Renderable.Axis`
* Parameters
## get_YAxis2()
***ERROR: XML DOCS NOT FOUND!***
* Returns `ScottPlot.Renderable.Axis`
* Parameters
## GetAxisLimits()
_Get limits for the given axes_
* Returns `ScottPlot.AxisLimits`
* Parameters
  * `int` **`xAxisIndex`**
  * `int` **`yAxisIndex`**
## GetCoordinate()
_Retrun the coordinate (in plot space) for the given pixel_
* Returns `ValueTuple`2[Double,Double]`
* Parameters
  * `float` **`xPixel`**
  * `float` **`yPixel`**
## GetCoordinateX()
_Retrun the coordinate (in plot space) for the given pixel_
* Returns `Double`
* Parameters
  * `float` **`xPixel`**
## GetCoordinateY()
_Retrun the coordinate (in plot space) for the given pixel_
* Returns `Double`
* Parameters
  * `float` **`yPixel`**
## GetDraggables()
***ERROR: XML DOCS NOT FOUND!***
* Returns `ScottPlot.Plottable.IDraggable[]`
* Parameters
## GetDraggableUnderMouse()
_Return the draggable plottable under the mouse cursor (or null if there isn't one)_
* Returns `ScottPlot.Plottable.IDraggable`
* Parameters
  * `double` **`pixelX`**
  * `double` **`pixelY`**
  * `int` **`snapDistancePixels`**
## GetHashCode()
***ERROR: XML DOCS NOT FOUND!***
* Returns `Int32`
* Parameters
## GetLegendBitmap()
***ERROR: XML DOCS NOT FOUND!***
* Returns `System.Drawing.Bitmap`
* Parameters
## GetNextColor()
_Return a new color from the Pallette based on the number of plottables already in the plot.
            Use this to ensure every plottable gets a unique color._
* Returns `System.Drawing.Color`
* Parameters
  * `double` **`alpha`**
## GetPixel()
_Retrun the pixel location of the given coordinate (in plot space)_
* Returns `ValueTuple`2[Single,Single]`
* Parameters
  * `double` **`x`**
  * `double` **`y`**
## GetPixelX()
_Retrun the pixel location of the given coordinate (in plot space)_
* Returns `Single`
* Parameters
  * `double` **`x`**
## GetPixelY()
_Retrun the pixel location of the given coordinate (in plot space)_
* Returns `Single`
* Parameters
  * `double` **`y`**
## GetPlottables()
***ERROR: XML DOCS NOT FOUND!***
* Returns `ScottPlot.Plottable.IPlottable[]`
* Parameters
## GetSettings()
_Get access to the plot settings module (not exposed by default because its internal API changes frequently)_
* Returns `ScottPlot.Settings`
* Parameters
  * `Boolean` **`showWarning`**
## GetType()
***ERROR: XML DOCS NOT FOUND!***
* Returns `Type`
* Parameters
## Grid()
_Customize basic options for the primary X and Y axes. 
            Call XAxis and YAxis methods to further customize individual axes._
* Returns `Void`
* Parameters
  * `Boolean?` **`enable`**
  * `System.Drawing.Color?` **`color`**
  * `ScottPlot.LineStyle?` **`lineStyle`**
## Layout()
_Set padding around the data area by defining the minimum size and padding for all axes_
* Returns `Void`
* Parameters
  * `Single?` **`left`**
  * `Single?` **`right`**
  * `Single?` **`bottom`**
  * `Single?` **`top`**
  * `Single?` **`padding`**
## Legend()
_Set legend visibility and location. Save the returned object for additional customizations._
* Returns `ScottPlot.Renderable.Legend`
* Parameters
  * `Boolean` **`enable`**
  * `ScottPlot.Alignment` **`location`**
## MatchLayout()
_Adjust this axis layout based on the layout of a source plot_
* Returns `Void`
* Parameters
  * `ScottPlot.Plot` **`sourcePlot`**
  * `Boolean` **`horizontal`**
  * `Boolean` **`vertical`**
## Remove()
_Remove a specific plottable_
* Returns `Void`
* Parameters
  * `ScottPlot.Plottable.IPlottable` **`plottable`**
## Render()
_Render the plot onto a new Bitmap (using size defined by Plot.Width and Plot.Height)_
* Returns `System.Drawing.Bitmap`
* Parameters
  * `Boolean` **`lowQuality`**
## Render()
_Render the plot onto a new Bitmap of the given dimensions_
* Returns `System.Drawing.Bitmap`
* Parameters
  * `int` **`width`**
  * `int` **`height`**
  * `Boolean` **`lowQuality`**
## Render()
_Render the plot onto an existing bitmap_
* Returns `System.Drawing.Bitmap`
* Parameters
  * `System.Drawing.Bitmap` **`bmp`**
  * `Boolean` **`lowQuality`**
## RenderLock()
***ERROR: XML DOCS NOT FOUND!***
* Returns `Void`
* Parameters
## RenderUnlock()
***ERROR: XML DOCS NOT FOUND!***
* Returns `Void`
* Parameters
## SaveFig()
_Save the plot as an image file and return the full path of the new file_
* Returns `String`
* Parameters
  * `string` **`filePath`**
## SetAxisLimits()
_Set limits for the given axes_
* Returns `Void`
* Parameters
  * `Double?` **`xMin`**
  * `Double?` **`xMax`**
  * `Double?` **`yMin`**
  * `Double?` **`yMax`**
  * `int` **`xAxisIndex`**
  * `int` **`yAxisIndex`**
## SetAxisLimits()
_Set limits for the given axes_
* Returns `Void`
* Parameters
  * `ScottPlot.AxisLimits` **`limits`**
  * `int` **`xAxisIndex`**
  * `int` **`yAxisIndex`**
## SetAxisLimitsX()
_Set limits for the primary X axis_
* Returns `Void`
* Parameters
  * `double` **`xMin`**
  * `double` **`xMax`**
## SetAxisLimitsY()
_Set limits for the primary Y axis_
* Returns `Void`
* Parameters
  * `double` **`yMin`**
  * `double` **`yMax`**
## SetCulture()
_Set the culture to use for number-to-string converstion for tick labels of all axes_
* Returns `Void`
* Parameters
  * `Globalization.CultureInfo` **`culture`**
## SetCulture()
_Set the culture to use for number-to-string converstion for tick labels of all axes_
* Returns `Void`
* Parameters
  * `string` **`shortDatePattern`**
  * `string` **`decimalSeparator`**
  * `string` **`numberGroupSeparator`**
  * `Int32?` **`decimalDigits`**
  * `Int32?` **`numberNegativePattern`**
  * `Int32[]` **`numberGroupSizes`**
## SetSize()
_Set the default size for new renders_
* Returns `Void`
* Parameters
  * `float` **`width`**
  * `float` **`height`**
## SetViewLimits()
_Set limits of the view for the primary axes (you cannot zoom, pan, or set axis limits beyond these boundaries)_
* Returns `Void`
* Parameters
  * `double` **`xMin`**
  * `double` **`xMax`**
  * `double` **`yMin`**
  * `double` **`yMax`**
## Style()
_Set colors of all plot components using pre-defined styles_
* Returns `Void`
* Parameters
  * `ScottPlot.Style` **`style`**
## Style()
_Set the color of specific plot components_
* Returns `Void`
* Parameters
  * `System.Drawing.Color?` **`figureBackground`**
  * `System.Drawing.Color?` **`dataBackground`**
  * `System.Drawing.Color?` **`grid`**
  * `System.Drawing.Color?` **`tick`**
  * `System.Drawing.Color?` **`axisLabel`**
  * `System.Drawing.Color?` **`titleLabel`**
## Title()
_Set the label for the horizontal axis above the plot (XAxis2)._
* Returns `Void`
* Parameters
  * `string` **`label`**
  * `Boolean` **`bold`**
## ToString()
***ERROR: XML DOCS NOT FOUND!***
* Returns `String`
* Parameters
## Validate()
_Throw an exception if any plottable contains an invalid state. Deep validation is more thorough but slower._
* Returns `Void`
* Parameters
  * `Boolean` **`deep`**
## XLabel()
_Set the label for the vertical axis to the right of the plot (XAxis)._
* Returns `Void`
* Parameters
  * `string` **`label`**
## XTicks()
_Manually define X axis tick labels_
* Returns `Void`
* Parameters
  * `string[]` **`labels`**
## XTicks()
_Manually define X axis tick positions and labels_
* Returns `Void`
* Parameters
  * `double[]` **`positions`**
  * `string[]` **`labels`**
## YLabel()
_Set the label for the vertical axis to the right of the plot (YAxis2)._
* Returns `Void`
* Parameters
  * `string` **`label`**
## YTicks()
_Manually define Y axis tick labels_
* Returns `Void`
* Parameters
  * `string[]` **`labels`**
## YTicks()
_Manually define Y axis tick positions and labels_
* Returns `Void`
* Parameters
  * `double[]` **`positions`**
  * `string[]` **`labels`**
