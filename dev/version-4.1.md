# ScottPlot 4.0 → 4.1 Notes

The largest changes between ScottPlot 4.0 and ScottPlot 4.1 are noted here

### Name Changes
* Plottables have been moved and renamed 
  * e.g., `ScottPlot.PlottableBar` is now `ScottPlot.Plottable.BarPlot`
* Plottable-creating helper methods have been renamed 
  * e.g., `Plot.PlotScatter()` is now `Plot.AddScatter()`
* `Axis()` has been replaced by `SetAxisLimits()` and `GetAxisLimits()`
* Plottable fields are now capitol
  * e.g., `fillColor` is now `FillColor`

### Modifying Data in Plottables
* Value types can be modified by editing public fields
  * e.g., `vline1.X = 123;`
* Values in arrays can be modified at any time
  * e.g., `scatterplot1.Xs[10] = 123;`
* Replacing arrays requires calling a helper method
  * e.g., `scatterplot1.Update(newXs, newYs);`

### Render System

The render system was largely refactored to improve multi-threaded support in user controls.

* Bitmaps are now never stored in the `Plot` module
* Bitmaps can be passed-in to the `Render()` method (for the plot to be rendered onto), or created when `Render()` is called and returned.

### User Controls

⚠️ Work on controls has not yet finished