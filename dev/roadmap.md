# ScottPlot Roadmap

This page is a collection of ideas and plans for future releases of ScottPlot.

## Upcoming Features

### Minor

These modifications can be made without breaking existing code, so they are suitable for inclusion in the next minor version.

  * Add right-click menu to WpfPlot (like FormsPlot's)

### Major 

These modifications may break existing code, so these changes must be saved for inclusion in the next major version change.

* Namespace change: `ScottPlot.PlottableSignal` -> `ScottPlot.Plottable.Signal`

* Use `IRenderer` to abstract all calls to `System.Drawing`  and make it easier to implement an alternative drawing system (such as SkiaSharp with OpenGL). Investigatory code is on my [C# Data Visualization](https://swharden.com/CsharpDataVis/) website.

## Version History

* **ScottPlot 4.0** (Nov, 2019) ScottPlot.Plot module became platform-agnostic using .NET Standard and System.Drawing.Common. Total recode, but same API. User controls became separate, platform-specific modules.

* **ScottPlot 3.0** (May, 2019) Total recode with new API. First version released on NuGet.

* **ScottPlot 2.0** (Jan, 2019) Total recode with new API. First version to get its own GitHub project. 

* **ScottPlot 1.0** (June, 2017) ScottPlot began as [swhPlot.cs](https://github.com/swharden/Csharp-Data-Visualization/blob/master/projects/17-06-24_stretchy_line_plot/pixelDrawDrag2/swhPlot.cs), a 150 line class used to create a [stretchy line plot](https://github.com/swharden/Csharp-Data-Visualization/tree/master/projects/17-06-24_stretchy_line_plot) to demonstrate how to draw lines interactively with C#.
