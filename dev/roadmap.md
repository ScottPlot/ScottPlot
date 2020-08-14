# ScottPlot Roadmap

### Latest Plan

> **⚠️ Message from Scott (August 2020):** I seek to stabilize ScottPlot 4.0 so I can focus all development effort into releasing ScottPlot 4.1 expediently. Users may post issues, but development of requested new features may be paused until 4.1 is released. **Due to active refactoring associated with the 4.0 → 4.1 transition, opening new PRs is discouraged until version 4.1 is released.** Progress is being tracked on [#505](https://github.com/swharden/ScottPlot/issues/505). Feel free to comment there or email me if you have questions in the interim.

### ScottPlot Versions

Changes for each released version can be viewed on the releases page:\
https://github.com/swharden/ScottPlot/releases

* **ScottPlot 4.2** will provide support for alternative rendering systems such as ImageSharp and SkiaSharp.

* **ScottPlot 4.1** (Aug-Sep 2020, see [#505](https://github.com/swharden/ScottPlot/issues/505)) improves stability, modularity, configurability, and improves error checking of plots and user controls. This version has many internal refactors and optimizations, but only small changes to the public API.

* **ScottPlot 4.0** (Nov, 2019) ScottPlot.Plot module became platform-agnostic using .NET Standard and System.Drawing.Common. Total recode, but same API. User controls became separate, platform-specific modules.

* **ScottPlot 3.0** (May, 2019) Total recode with new API. First version released on NuGet.

* **ScottPlot 2.0** (Jan, 2019) Clean recode with new API. First version to get its own GitHub project. 

* **ScottPlot 1.0** (June, 2017) ScottPlot began as [swhPlot.cs](https://github.com/swharden/Csharp-Data-Visualization/blob/master/projects/17-06-24_stretchy_line_plot/pixelDrawDrag2/swhPlot.cs), a 150 line class used to create a [stretchy line plot](https://github.com/swharden/Csharp-Data-Visualization/tree/master/projects/17-06-24_stretchy_line_plot) to demonstrate how to draw lines interactively with C#.