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

* User controls now use a central back-end so behavior of all controls is managed by a single class
* Configuration is no longer achieved by calling `formsPlot1.Configure()` but instead interacting with `formsPlot.Configuration`
* Controls have new event handlers to allow users to customize: 
  * Right-click action (defaults to a drop-down menu)
  * Drag/Drop actions
  * What happens when axes change (useful for linking multiple plots together)