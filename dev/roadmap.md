# ScottPlot Roadmap

### Note from Scott (November, 2020)

On November 17th I merged [#605](https://github.com/swharden/ScottPlot/pull/605) and upgraded the master branch from ScottPlot `4.0` to ScottPot `4.1-beta`. There were very large internal changes associated with this upgrade (see [changelog.md](changelog.md) for details), but I am working to keep the public methods largely the same.

**⚠️ I am continuing to work on this daily, and will temporarily reduce effort on issues and pull requests until this large transition stabilizes.** Remaining high priority tasks include:
* Refactor layout code for improved simplicity
* Refine multi-axis code for improved simplicity
* Improve data validation for plottables
* Create new plottables specifically for growing data
* Refactor the cookbook generator to create a simpler multi-page HTML (not markdown) static website
* Refactor user controls for improved multi-threaded rendering by making `Render()` non-blocking

## Versions

_Detailed feature lists are on the [releases](https://github.com/swharden/ScottPlot/releases) page_

* **ScottPlot 4.1** (Nov, 2020) Added support for multiple axes. Refactored all plottables and plot components. Rendering system now renders onto a single image (rather than separate figure and data images), and does not store images in memory between renders.

* **ScottPlot 4.0** (Nov, 2019) ScottPlot.Plot module became platform-agnostic using .NET Standard and System.Drawing.Common. Total recode, but same API. User controls became separate, platform-specific modules.

* **ScottPlot 3.0** (May, 2019) Total recode with new API. First version released on NuGet.

* **ScottPlot 2.0** (Jan, 2019) Clean recode with new API. First version to get its own GitHub project. 

* **ScottPlot 1.0** (June, 2017) ScottPlot began as [swhPlot.cs](https://github.com/swharden/Csharp-Data-Visualization/blob/master/projects/17-06-24_stretchy_line_plot/pixelDrawDrag2/swhPlot.cs), a 150 line class used to create a [stretchy line plot](https://github.com/swharden/Csharp-Data-Visualization/tree/master/projects/17-06-24_stretchy_line_plot) to demonstrate how to draw lines interactively with C#.
