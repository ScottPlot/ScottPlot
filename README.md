# ScottPlot

[![](https://img.shields.io/azure-devops/build/swharden/swharden/2?label=Build&logo=azure%20pipelines)](https://dev.azure.com/swharden/swharden/_build/latest?definitionId=2&branchName=master)
[![](https://img.shields.io/nuget/dt/ScottPlot?color=004880&label=Installs&logo=nuget)](https://www.nuget.org/packages/ScottPlot/)
[![Nuget](https://img.shields.io/nuget/v/scottplot?label=NuGet&logo=nuget)](https://www.nuget.org/packages/ScottPlot/)

**ScottPlot is a free and open-source graphing library for .NET** which makes it easy to display data in a variety of formats (line plots, bar charts, scatter plots, etc.) with just a few lines of code (see the **[ScottPlot Cookbook](http://swharden.com/scottplot/cookbook)** for examples). User controls are available for WinForms, WPF, and Avalonia to allow interactive display of data. ScottPlot can be installed using NuGet.

<div align='center'>
<img src='dev/nuget/ScottPlot.gif'>
</div>

## New to ScottPlot?

The [ScottPlot website](http://swharden.com/scottplot/) contains documentation, tutorials, demos, and more...

* **[ScottPlot Cookbook](http://swharden.com/scottplot/cookbook.md.html) 👈 START HERE**
* [WPF Quickstart](http://swharden.com/scottplot/quickstart.md.html#wpf-quickstart)
* [Windows Forms Quickstart](http://swharden.com/scottplot/quickstart.md.html#windows-forms-quickstart)
* [Console Application Quickstart](http://swharden.com/scottplot/quickstart.md.html#console-quickstart)
* [Avalonia Quickstart](https://github.com/swharden/ScottPlot/tree/master/src/ScottPlot.Demo.Avalonia)

## Developer Resources

This repository is focused on software development tasks: source code tracking, implementation of new features, reporting and tracking bugs, etc.

### Supported Platforms

* The [`ScottPlot`](https://www.nuget.org/packages/ScottPlot/) package supports:
  * .NET Core 2.0 and newer
  * .NET Framework 4.6.1 and newer
  * UWP 10.0.16299 and newer
* User control packages [`ScottPlot.WinForms`](https://www.nuget.org/packages/ScottPlot.WinForms), [`ScottPlot.WPF`](https://www.nuget.org/packages/ScottPlot.WPF), and [`ScottPlot.Avalonia`](https://www.nuget.org/packages/ScottPlot.Avalonia/):
  * .NET Core 3.0 and newer
  * .NET Framework 4.6.1 and newer

### Build ScottPlot from Source Code
ScottPlot is developed with the latest non-preview version of [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/).


To use the latest ScottPlot source code in your application, download this repository and add references in your project to `ScottPlot.csproj`, and one of `ScottPlot.WPF.csproj` or `ScottPlot.WinForms.csproj` (in the src folder). After adding the reference you may need to rebuild the solution before user controls appear in the toolbox.

> ⚠️ The latest source code may contain partially-implemented or experimental functionality. Users desiring stable source code are encouraged to download ScottPlot from the [releases](https://github.com/swharden/ScottPlot/releases) page.

### Contribute to ScottPlot

* **If you like ScottPlot, give it a star!** If you want to support this project but don't have a feature request or code modification to bring forward yet, you can still support us by following this project and giving it a star!

* **To Suggest a Feature:** Review the latest [ScottPlot Cookbook]() to ensure the feature doesn't already already exist, then [open an issue](https://github.com/swharden/ScottPlot/issues) and describe your idea.

* **Create a Pull Request:** Developers wishing to contribute directly to this project are encouraged to review [contributing.md](CONTRIBUTING.md)

### About ScottPlot

ScottPlot was created by [Scott Harden](http://www.SWHarden.com/) ([Harden Technologies, LLC](http://tech.swharden.com)) with many contributions from the open source user community. It is provided under the permissive [MIT license](LICENSE).
