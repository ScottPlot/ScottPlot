# ScottPlot Roadmap

The forward-looking roadmap is tracked in a pinned "triaged tasks and features" issue on the [**issues page**](https://github.com/ScottPlot/ScottPlot/issues).

## Versions

_Detailed notes for each version are in the [changelog](changelog.md) and [releases page](https://github.com/swharden/ScottPlot/releases)_

* **ScottPlot 4.1** (Nov 2020, released May 2021) Added support for multiple axes. Refactored all plottables and plot components. Rendering system now renders onto a single image (rather than separate figure and data images), and does not store images in memory between renders. Many namespaces and public fields were renamed to promote discoverability.

* **ScottPlot 4.0** (Nov, 2019) ScottPlot.Plot module became platform-agnostic using .NET Standard and System.Drawing.Common. Total recode, but same API. User controls became separate, platform-specific modules.

* **ScottPlot 3.0** (May, 2019) Total recode with new API. First version released on NuGet.

* **ScottPlot 2.0** (Jan, 2019) Clean recode with new API. First version to get its own GitHub project. 

* **ScottPlot 1.0** (June, 2017) ScottPlot began as [swhPlot.cs](https://github.com/swharden/Csharp-Data-Visualization/blob/master/projects/17-06-24_stretchy_line_plot/pixelDrawDrag2/swhPlot.cs), a 150 line class used to create a [stretchy line plot](https://github.com/swharden/Csharp-Data-Visualization/tree/master/projects/17-06-24_stretchy_line_plot) to demonstrate how to draw lines interactively with C#.
