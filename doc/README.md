# ScottPlot Documentation

* [Quickstart Guides](https://github.com/swharden/ScottPlot#quickstart)
* [ScottPlot Cookbook](https://github.com/swharden/ScottPlot#cookbook)
* [Demo Applications](https://github.com/swharden/ScottPlot#demos)

## API Notes

> Warning, this isn't updated as often as it should be. It is possible there are small errors.

### The `Plot` object
The core of ScottPlot is the `ScottPlot.Plot` object. Virtually all use of ScottPlot involves creating a Plot and interacting with its top-level methods:

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
`plt.PlotSignalConst()` | Even faster, but cannot change data once plotted
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

## Tools
A few useful classes are provided with ScottPlot:
* `ScottPlot.DataGen` contains several methods which _generate_ data. This is a good way to create datasets to practice plotting.
* `ScottPlot.Histogram` contains methods for taking in a dataset, binning it, and providing an easy way to access the bin counts. Extra tools are included to create cumulative probability histogram (CPH) plots.