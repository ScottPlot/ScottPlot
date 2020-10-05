# ScottPlot Roadmap

_Updated October 4, 2020_

**My high priority goals are (in order):**

* Refactor plottables ([#578](https://github.com/swharden/ScottPlot/issues/578)) to (1) use public properties (each with tests and XML docs) to customize styling and behavior instead of constructor arguments and (2) add shallow and deep data validation methods (related to diagnostic mode, [#553](https://github.com/swharden/ScottPlot/issues/553)).

* Roll version from `4.0` to `4.1.x-alpha`. This way NuGet only lists the package if the pre-release box is checked, making breaking changes less offensive.

* Transition plottables to fully implement `IPlottable` (interface) instead of `Plottable` (abstract class with all abstract methods).

* Improve plottables by enhancing how data is updated and validated by creating and implementing `IValidatable`.

* Refactor all plottables to create and destroy disposable System.Drawing objects inside the render loop rather than store them at the class level.

* Refactor plottables to use public properties instead of large constructors to customize configuration and styling.

* Stop passing `Settings` into the `Render` method of plottables. Use concepts from `/dev/ScottPlot5` to create and pass a small configuration object with only figure dimensions and axis limits.

**A large list of features** I want to eventually support are collected in [#412](https://github.com/swharden/ScottPlot/issues/412)

## Versions

_Detailed feature lists are on the [releases](https://github.com/swharden/ScottPlot/releases) page_

* **ScottPlot 4.3** (future) will abstract the renderer to provide support for alternative rendering systems such as ImageSharp and SkiaSharp with OpenGL acceleration.

* **ScottPlot 4.2** (future) will refactor the Plot module to improve modularity, layout configuration, and add add advanced functionality to axes (such as support for multiple Y plots).

* **ScottPlot 4.1** (Started Sep, 2020) seeks to improve stability, modularity, configurability, and improves error checking of plots and user controls. This version has many internal refactors and optimizations, but only small changes to the public API.

* **ScottPlot 4.0** (Started Nov, 2019) ScottPlot.Plot module became platform-agnostic using .NET Standard and System.Drawing.Common. Total recode, but same API. User controls became separate, platform-specific modules.

* **ScottPlot 3.0** (Started May, 2019) Total recode with new API. First version released on NuGet.

* **ScottPlot 2.0** (Started Jan, 2019) Clean recode with new API. First version to get its own GitHub project. 

* **ScottPlot 1.0** (Started June, 2017) ScottPlot began as [swhPlot.cs](https://github.com/swharden/Csharp-Data-Visualization/blob/master/projects/17-06-24_stretchy_line_plot/pixelDrawDrag2/swhPlot.cs), a 150 line class used to create a [stretchy line plot](https://github.com/swharden/Csharp-Data-Visualization/tree/master/projects/17-06-24_stretchy_line_plot) to demonstrate how to draw lines interactively with C#.
