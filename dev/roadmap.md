# ScottPlot Roadmap

For the latest roadmap related to active development see the pinned topics on the [**issues page**](https://github.com/ScottPlot/ScottPlot/issues).

## Versions

_Detailed notes about changes and contributors for each version can be found at https://ScottPlot.NET/changelog/_

* **ScottPlot 5.0** (in development) Major changes include:
  * No calls to `System.Drawing` (which is no longer supported on Linux or MacOS after .NET 6)
  * Rendering using `Microsoft.Maui.Graphics` and Skia (optionally with OpenGL)
  * Significant shift toward statelessness (greatly simplifying GUI testing)
  * Improved thread safety
  * Improved tick system
  * Less reliance on `double[]` and improved ability to use generic arrays, Spans, and Lists
  * Modern language features (Nullable, Records, etc)
  * Continued backward compatibility with .NET Framework 4.6.2
  * ⚠️ **UPDATE (March, 2022):** Development is paused until the `Microsoft.Maui.Graphics` package matures and releases. See [#1647](https://github.com/ScottPlot/ScottPlot/pull/1647) for information.

* **ScottPlot 4.1** (Nov 2020, released May 2021) Added support for multiple axes. Refactored all plottables and plot components. Rendering system now renders onto a single image (rather than separate figure and data images), and does not store images in memory between renders. Many namespaces and public fields were renamed to promote discoverability.

* **ScottPlot 4.0** (Nov, 2019) ScottPlot.Plot module became platform-agnostic using .NET Standard and System.Drawing.Common. Total recode, but same API. User controls became separate, platform-specific modules.

* **ScottPlot 3.0** (May, 2019) Total recode with new API. First version released on NuGet.

* **ScottPlot 2.0** (Jan, 2019) Clean recode with new API. First version to get its own GitHub project. 

* **ScottPlot 1.0** (June, 2017) ScottPlot began as [swhPlot.cs](https://github.com/swharden/Csharp-Data-Visualization/blob/master/dev/old/17-06-24_stretchy_line_plot/pixelDrawDrag2/swhPlot.cs), a 150 line class used to create a [stretchy line plot](https://github.com/swharden/Csharp-Data-Visualization/tree/master/dev/old/17-06-24_stretchy_line_plot) to demonstrate how to draw lines interactively with C#.
