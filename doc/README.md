# ScottPlot Documentation

* **Quickstart guides** are available in the [/demos/](/demos/) folder.
* **[The ScottPlot Cookbook](/cookbook)** is one of the best ways to quickly learn about all the ScottPlot features.
* **Demo applications** are in the [/demos/](/demos/) folder.

## ScottPlot.Plot
The core of ScottPlot is the ScottPlot.Plot() class. Users only need to interact with its top-level methods. The following code creates a new plot 600px by 400px:

```cs
var plt = new ScottPlot.Plot(600, 400);
```

### Plotting Data

Method | Description
---|---
`plt.PlotText()` | Plot text at a specific location
`plt.PlotPoint()` | Plot a single point at a specific location
`plt.PlotScatter()` | Plot a scatter plot from X and Y points
`plt.PlotSignal()` | Plot evenly-spaced data (optimized for speed)
`plt.PlotBar()` | Plot X and Y points as a bar graph
`plt.PlotStep()` | Plot X and Y points as a step graph
`plt.PlotVLine()` | Plot a vertical line at the given X position
`plt.PlotHLine()` | Plot a horizontal line at the given Y position
`plt.Clear()` | Clears all plotted objects
`plt.GetPlottables()` | Get all plotted objects (useful for modifying their data after they've already been plotted)
`plt.GetTotalPoints()` | Total points across all plottable objects

### Axes

Method | Description
---|---
`plt.Axis()` | Manually set axis limits
`plt.AxisAuto()` | Automatically adjust axis limits to fit the data
`plt.AxisZoom()` | Zoom in or out by a given fraction
`plt.AxisPan()` | Pan by a given amount

### Labels

Method | Description
---|---
`plt.Title()` | Set the title
`plt.XLabel()` | Set the horizontal axis label
`plt.YLabel()` | Set the vertical axis label
`plt.Legend()` | Add a legend made from the labels given to plot objects

### Display Options

Method | Description
---|---
`plt.Ticks()` | Control axis tick visibility and styling
`plt.Grid()` | Control grid visibility and color
`plt.Frame()` | Control visibility and style of the data area frame
`plt.Benchmark()` | Control visibility of benchmark information on the data area
`plt.AntiAlias()` | Set anti-aliasing of the figure and the data
`plt.TightenLayout()` | Reduce space between data and labels
`plt.Style()` | Set colors of all of the plot elements

### Matching Functions

Method | Description
---|---
`plt.MatchAxis()` | use axis limits from another ScottPlot
`plt.MatchPadding()` | use frame padding from another ScottPlot

### Image Settings

Method | Description
---|---
`plt.Resize()` | Changes the pixel dimensions of image
`plt.GetBitmap()` | Returns the graph as a System.Drawing.Bitmap
`plt.SaveFig()` | Saves the graph as an image

## ScottPlotUC User Control
The ScottPlot user control (ScottPlotUC) contains a ScottPlot object pre-initialized (ScottPlotUC.plt) as well as extra bindings to track the mouse and resize the plot as the control changes shape. You can access this ScottPlot plot object and use all the methods listed above:

```cs
ScottPlotUC1.plt.PlotSignal(demoData);
```

The user control also comes with a Render() function which tells the ScottPlot object to render the graph as a bitmap and display it. This function is called automatically on click-and-drag mouse events (panning and zooming), but you need to call it manually after adding data or changing the graph in some way.

```cs
ScottPlotUC1.Render();
```

If you are plotting data which is continuously changing, call the Render() function every time the data changes. For rapidly-changing data, add a timer to your application and have it repeatedly call the Render() function.

## ScottPlot Tools
A few useful classes are provided with ScottPlot:
* `ScottPlot.DataGen` contains several methods which _generate_ data. This is a good way to create datasets to practice plotting.
* `ScottPlot.Histogram` contains methods for taking in a dataset, binning it, and providing an easy way to access the bin counts. Extra tools are included to create cumulative probability histogram (CPH) plots.