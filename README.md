# ScottPlot

[![](https://img.shields.io/azure-devops/build/swharden/ScottPlot/15?label=Build&logo=azure%20pipelines)](https://dev.azure.com/swharden/ScottPlot/_build?definitionId=15)
[![](https://img.shields.io/azure-devops/tests/swharden/ScottPlot/15?label=Tests&logo=azure%20pipelines)](https://dev.azure.com/swharden/ScottPlot/_build?definitionId=15)
[![](https://img.shields.io/nuget/dt/ScottPlot?color=004880&label=Installs&logo=nuget)](https://www.nuget.org/packages/ScottPlot/)
[![](https://img.shields.io/nuget/v/scottplot?label=NuGet&logo=nuget)](https://www.nuget.org/packages/ScottPlot/)

**ScottPlot is a free and open-source plotting library for .NET** that makes it easy to interactively display large datasets. The [**ScottPlot Cookbook**](https://swharden.com/scottplot/cookbook) demonstrates how to create line plots, bar charts, pie graphs, scatter plots, and more with just a few lines of code.

* **[ScottPlot Cookbook](https://swharden.com/scottplot/cookbook)** 👈 _Learn how to use ScottPlot_

* **[ScottPlot Demo](https://swharden.com/scottplot/demo)** 👈 _See what ScottPlot can do_

* **Quickstart:** [WinForms](https://swharden.com/scottplot/quickstart#windows-forms-quickstart), [WPF](https://swharden.com/scottplot/quickstart#wpf-quickstart), [Avalonia](https://swharden.com/scottplot/quickstart#avalonia-quickstart), [Console](https://swharden.com/scottplot/quickstart#console-quickstart)

<div align='center'>

<a href='https://swharden.com/scottplot'><img src='dev/graphics/ScottPlot.gif'></a>

<a href='https://swharden.com/scottplot/cookbook'><img src='dev/graphics/cookbook.jpg'></a>

</div>

### Questions and Feedback

* **Ask questions** in [Discussions](https://github.com/swharden/ScottPlot/discussions/categories/q-a), [Issues](https://github.com/swharden/ScottPlot/issues), or [on StackOverflow]((https://stackoverflow.com/questions/ask?tags=scottplot))

* [**Create an issue**](https://github.com/swharden/ScottPlot/issues) for a feature suggestion or bug report

* If you enjoy ScottPlot **give us a star!** ⭐

### Major Versions

* See [**Releases**](https://github.com/swharden/ScottPlot/releases) for source code and notes for all versions

* **`ScottPlot 4.1`** is being actively-developed and is currently available as a pre-release package on NuGet. This version is faster than 4.0, supports multiple axes, and has a simpler API and a much better cookbook.

* **`ScottPlot 4.0`** is stable, available on NuGet, and has its own [branch](https://github.com/swharden/ScottPlot/branches) for continued bug fixes and refinements. However, new features are no longer being developed for this version.

### Plot in the Cloud

**This figure is an example of ScottPlot running in the cloud.** Once daily an [Azure Function](https://azure.microsoft.com/en-us/services/functions/) logs the number of stars this repository has and creates a graph of the star history using ScottPlot. Since the output is simply a static image accessible by a URL, it can be displayed in places where JavaScript is not permitted like in this readme file. Rendering plots server-side allows automatically-updating plots to be displayed in many interesting places. Source code can be found in the [StarGraph](https://github.com/swharden/StarGraph) repository.

<p align="center">
  <img src="https://swhardendev.z13.web.core.windows.net/packagestats/scottplot-stars.png">
</p>

### Developer Notes

* **Minimum Supported Platforms:** .NET Core 2.0 and .NET Framework 4.6.1 ([see details](https://swharden.com/scottplot/#supported-platforms))

* **The [ScottPlot Roadmap](dev/roadmap.md)** summarizes ScottPlot's origins and tracks development goals. If there is a pinned _Triaged Tasks and Features_ issue on the [issues page](https://github.com/ScottPlot/ScottPlot/issues), it likely indicates what I am currently working on.

* **Contributions are welcome!** See [contributing.md](CONTRIBUTING.md) to get started

### About ScottPlot

ScottPlot was created by [Scott W Harden](https://swharden.com/about/) (with many contributions from the open-source community) and is provided under the permissive [MIT license](LICENSE).
