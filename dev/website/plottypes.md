
### Plot Types
_Several thumbnails should be shown for each plot type. Perhaps clicking on a thumbnail will reveal its source code?_

* **Annotation** - Annotations are text placed on the figure (typically in a corner) that does not move when the axis limits are adjusted. This is different than the text plot type which displays a string at a X/Y location in unit space.

* **Arrow** - Arrows have a start and end point in unit space. This plot type is actually a scatter plot with two points and a custom arrowhead.

* **Axis Line** (HLine and VLine) - Axis lines horizontal or vertical lines that extend across the axes. They can be draggable, allowing the mouse to move their position.

* **Axis Span** (HSpan and VSpan) - Axis spans shade a region of horizontal or vertical space. They can be draggable, allowing the mouse to adjust their width or position.

* **Bar Plot** - Simple bar plots can be made from arrays of positions and values. More advanced grouped bar plots can be made using the population plot type.

* **Error Bars** - Errorbars are a series of horizontal and/or vertical lines which represent a region of uncertainty.

* **Finance Plot** - Financial plots render candlestick or OHLC (open, high, low, close) symbols showing financial information for discrete time periods.

* **Function Plot** - A function plot uses a function to transform X values into Y values instead of displaying discrete X and Y data points. Function plots can be zoomed infinitely.

* **Heatmap** - Heatmaps display a 2D grid of values, translating value into color according to a colormap. Heatmaps allow 2D data values to be displayed as a raster image.

* **Image** - Image plots display bitmap images at specific coordinates in unit space. Images translate as axes are manipulated, but they do not change size.

* **Pie Plot** - Pie plots display categorical data as wedges of a circle. Pie plots have many options to customize the display of the wedges and labels. Pie graphs can also be called circle charts, those with an open center can also called donut charts.

* **Point** - A point is a scatter plot with a single value. To plot many points consider creating a scatter plot and set the line width to zero.

* **Polygon** - Polygons are collections of points which may be connected by lines and whose center may be filled. The Polygons plot type is a variant of the Polygon plot type optimized for speed in situations where many polygons need to be displayed.

* **Radar** - Radar plots display categorical data. Radar charts are also called spider charts, star charts, or web charts.

* **Scale Bar** - The scale bar is an L-shaped symbol in the corner of a plot to indicate scale when axis lines are hidden.

* **Scatter** - The scatter plot displays paired X and Y data. Lines may be optionally connected, and data points may optionally display marker symbols. The scatter plot is the most common plot type, but it is not recommended for large datasets. When data is evenly spaced, Signal plots offer greatly superior performance.

* **ScatterHighlight** - A variant of the scatter plot which contains extra logic to find the point nearest the mouse cursor and optionally highlight and/or retrieve it.

* **Signal** - The signal plot is a highly optimized plot type designed to display evenly-spaced data. Applications displaying more than a few dozen points will greatly benefit from using Signal plots over Scatter plots.

* **SignalConst** - This plot type is a variant of the Signal plot which offers even greater performance as long as the data is not mutated (hence const). It achieves its speed optimization by pre-calculating a binary tree to facilitate conversion between unit space and pixel space. However, calculation and storage of this tree means SignalConst takes some up-front time to initialize and occupies more memory compared to a basic Signal plot.

* **SignalXY** - This plot type is a variant of the Signal plot which can accommodate unevenly-spaced data points as long as they are ascending.

* **SignalXYConst** - This plot type is a variant of the SignalXY plot which achieves a speed improvement by assuming immutable data (hence const). The same pre-calculation and memory implications apply as with SignalConst.

* **Step** - The step plot is a type of scatter plot whose rendering style connects points by angled steps rather than diagonal lines.

* **Text** - Displays a string at a given point in unit space. This is different than an annotation which displays a string at a location on figure (in pixel space).

* **VectorField** - This plot type displays a square grid vector field using two 1D arrays of positional data and one 2D array of magnitude data.