# ScottPlot Documentation

Traditionally the ScottPlot documentation has been the cookbook webpage and the demo application source code. I'd like to break this up into 4 online documentation sections:

* **ScottPlot Quickstart** - Create a new project, install ScottPlot, and plot data in less than a minute.

* **ScottPlot Overview** - A summary of the primary concepts of ScottPlot. The goal is to create the minimum number of topics and examples that new users can review to maximally understand how to use ScottPlot.

* **ScottPlot FAQ** - Answers to common questions, often containing links to cookbook examples or a reference to the demo program source code for extended reading.

* **ScottPlot Cookbook** - Examples of all plot types and advanced customization of plottables, axes, and figure styling.

## ScottPlot Overview
* Plottables
  * Create plottables with `Plot.PlotSomething()` shortcuts
  * Create plottables with `new` and `Plot.Add()`
  * Scatter plots are simple, Signal plots are performant
  * Customize plottables using public fields and methods
* Axes
  * Customize labels, ticks, and grid of primary axes: `XAxis` and `YAxis`
  * Set axis limits manually with `Plot.AxisSet()`
  * Fit axis limits to the plotted data with `Plot.AxisAuto()`
  * Customize secondary axes: `XAxis2`, `YAxis2`
  * Add additional custom axes
* User Controls
  * Call the user control `Render()` method after modifying the plot
  * Multiple methods for displaying data that changes
    * Clear the plot and add new plottables
    * Update values in existing arrays
    * Replace data with new arrays

## ScottPlot FAQ
* Working with DateTime data
* Adjust figure padding
* Low quality rendering (anti-aliasing off)
* Frameless plot
* Multiplot
* Data containing `double.NaN`
* Creating line plots with gaps
* Using additional axes
* GUI-specific
  * By default rendering is low quality while dragging
  * Modify plots from multiple threads
  * Customize right-click menu
  * Get position under mouse
  * Print dialogue

## ScottPlot Cookbook
### Plot Types
_Thumbnails will be shown for all plot types._

* **Annotation** (`Plottable.Annotation`) - Annotations are text placed on the figure that does not move when the axis limits are adjusted. Annotations are typically aligned to a corner of the data area.

* **Arrow** (`Plottable.Arrow`) - Arrows are lines with an arrowhead that have a start and end point in unit space.

* **Axis Line** (`Plottable.HLine` and `Plottable.VLine`) - Axis lines horizontal or vertical lines that extend across the axes. They can be draggable, allowing the mouse to move their position.

* **Axis Span** (`Plottable.HSpan` and `Plottable.HSpan`) - Axis spans shade a region of horizontal or vertical space. They can be draggable, allowing the mouse to adjust their width or position.

* **Bar Plot** (`Plottable.BarPlot`) - Simple bar plots can be made from arrays of positions and values. More advanced grouped bar plots can be made using the population plot type.

* **Error Bars** (`Plottable.ErrorBar`) - Errorbars are a series of horizontal and/or vertical lines which represent a region of uncertainty.

* **Fill** (`Plottable.Arrow`) - 

* **Finance** (`Plottable.Arrow`) - 

* **Function** (`Plottable.Arrow`) - 

* **Heatmap** (`Plottable.Arrow`) - 

* **Image** (`Plottable.Arrow`) - 

* **Pie** (`Plottable.Arrow`) - 

* **Point** (`Plottable.Arrow`) - 

* **Polygon (and Polygons)** (`Plottable.Arrow`) - 

* **Radar** (`Plottable.Arrow`) - 

* **ScaleBar** (`Plottable.Arrow`) - 

* **Scatter** (`Plottable.Arrow`) - 

* **ScatterHighlight** (`Plottable.Arrow`) - 

* **Signal** (`Plottable.Arrow`) - 

* **SignalConst** (`Plottable.Arrow`) - 

* **SignalXY** (`Plottable.Arrow`) - 

* **SignalXYConst** (`Plottable.Arrow`) - 

* **Step** (`Plottable.Arrow`) - 

* **Text** (`Plottable.Arrow`) - 

* **VectorField** (`Plottable.Arrow`) - 

## Advanced Figure Customization

* Styling
  * standard styles
  * Figure background
  * Data background
  * Anti-aliasing

* Layout
* Colorsets