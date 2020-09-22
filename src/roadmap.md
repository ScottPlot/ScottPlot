# ScottPlot Roadmap

_Updated September 22, 2020_

* ScottPlot 4 is the current stable version of ScottPlot.

* I seek to improve ScottPlot 4 by favoring code improvement over implementation of new features.

* ScottPlot 5 is experimental. ScottPlot 5 is being developed in parallel with maintenance of ScottPlot 4, and it new features (e.g., OpenGL support) but a moderately different API.

* Low-priority or complex features I wish to eventually support are collected in [#412](https://github.com/swharden/ScottPlot/issues/412). I revisit these items when ScottPlot 5 matures.

## History of ScottPlot

Changes for each released version can be viewed on the releases page:\
https://github.com/swharden/ScottPlot/releases

* **ScottPlot 5** will provide support for alternative rendering systems such as ImageSharp and SkiaSharp.

* **ScottPlot 4.1** improves stability, modularity, configurability, and improves error checking of plots and user controls. This version has many internal refactors and optimizations, but only small changes to the public API.

* **ScottPlot 4.0** (Nov, 2019) ScottPlot.Plot module became platform-agnostic using .NET Standard and System.Drawing.Common. Total recode, but same API. User controls became separate, platform-specific modules.

* **ScottPlot 3.0** (May, 2019) Total recode with new API. First version released on NuGet.

* **ScottPlot 2.0** (Jan, 2019) Clean recode with new API. First version to get its own GitHub project. 

* **ScottPlot 1.0** (June, 2017) ScottPlot began as [swhPlot.cs](https://github.com/swharden/Csharp-Data-Visualization/blob/master/projects/17-06-24_stretchy_line_plot/pixelDrawDrag2/swhPlot.cs), a 150 line class used to create a [stretchy line plot](https://github.com/swharden/Csharp-Data-Visualization/tree/master/projects/17-06-24_stretchy_line_plot) to demonstrate how to draw lines interactively with C#.