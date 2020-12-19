# ScottPlot Roadmap

### Note from Scott (December 19, 2020)

Over the last few months I refactored a lot of the core ScottPlot library (major changes are described in [version-4.1.md](version-4.1.md)). Remaining tasks include:
* Create a FAQ section for the website
* Improve plottable data validation and error reporting
* Create a signal plot that supports growing data
* Refactor user controls to increase simplicity, consistency, and performance
  * Render in a separate thread (wso the GUI is not blocked)
  * Use a common control module in the ScottPlot project so configuration and rendering methods are shared across all user controls

## Versions

_Detailed feature lists are on the [releases](https://github.com/swharden/ScottPlot/releases) page_

* **ScottPlot 4.1** (Nov, 2020) Added support for multiple axes. Refactored all plottables and plot components. Rendering system now renders onto a single image (rather than separate figure and data images), and does not store images in memory between renders. Many namespaces and public fields were renamed to promote discoverability.

* **ScottPlot 4.0** (Nov, 2019) ScottPlot.Plot module became platform-agnostic using .NET Standard and System.Drawing.Common. Total recode, but same API. User controls became separate, platform-specific modules.

* **ScottPlot 3.0** (May, 2019) Total recode with new API. First version released on NuGet.

* **ScottPlot 2.0** (Jan, 2019) Clean recode with new API. First version to get its own GitHub project. 

* **ScottPlot 1.0** (June, 2017) ScottPlot began as [swhPlot.cs](https://github.com/swharden/Csharp-Data-Visualization/blob/master/projects/17-06-24_stretchy_line_plot/pixelDrawDrag2/swhPlot.cs), a 150 line class used to create a [stretchy line plot](https://github.com/swharden/Csharp-Data-Visualization/tree/master/projects/17-06-24_stretchy_line_plot) to demonstrate how to draw lines interactively with C#.
