# ScottPlot :chart_with_upwards_trend:

[![](https://img.shields.io/azure-devops/build/swharden/swharden/2?label=Build&logo=azure%20pipelines)](https://dev.azure.com/swharden/swharden/_build/latest?definitionId=2&branchName=master)
[![](https://img.shields.io/nuget/dt/ScottPlot?color=004880&label=NuGet%20Installs&logo=nuget)](https://www.nuget.org/packages/ScottPlot/)

**ScottPlot is a free and open-source graphing library for .NET** which makes it easy to display data in a variety of formats (line plots, bar charts, scatter plots, etc.) with just a few lines of code (see the **[ScottPlot Cookbook](http://swharden.com/scottplot/cookbook)** for examples). User controls are available for WinForms and WPF to allow interactive display of data. ScottPlot can be installed using NuGet.

<div align='center'>
<img src='dev/nuget/ScottPlot.gif'>
</div>

## New to ScottPlot?

The [ScottPlot website](http://swharden.com/scottplot/) contains documentation, tutorials, demos, and more...

* **[ScottPlot Cookbook](http://swharden.com/scottplot/cookbook) 👈 START HERE**
* [WPF Quickstart](http://swharden.com/scottplot/quickstart#wpf)
* [Windows Forms Quickstart](http://swharden.com/scottplot/quickstart#winforms)
* [Console Application Quickstart](http://swharden.com/scottplot/quickstart#console)


## Developer Resources

This repository is focused on software development tasks: source code tracking, implementation of new features, reporting and tracking bugs, etc.

### Supported Platforms

* **ScottPlot** targets **.NET Standard 2.0**\
_which supports .NET Framework 4.6.1, Xamarinin.iOS 10.14, Xamarinin.Mac 3.8
Xamarinin.Android 8.0, and Universal Windows Platform (UWP) 10.0.16299._
* **ScottPlot.WinForms** targets **.NET Framework 4.6.1** and **.NET Core 3.0**
* **ScottPlot.WPF** targets **.NET Core 3.0**

### Build ScottPlot from Source Code
ScottPlot is developed with the latest non-preview version of [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/).


To use the latest ScottPlot source code in your application, download this repository and add references in your project to `ScottPlot.csproj`, and one of `ScottPlot.WPF.csproj` or `ScottPlot.WinForms.csproj` (in the src folder). After adding the reference you may need to rebuild the solution before user controls appear in the toolbox.

> ⚠️ The latest source code may contain partially-implemented or experimental functionality. Users desiring stable source code are encouraged to download ScottPlot from the [releases](https://github.com/swharden/ScottPlot/releases) page.

### About ScottPlot

ScottPlot was created by [Scott Harden](http://www.SWHarden.com/) ([Harden Technologies, LLC](http://tech.swharden.com)) with many contributions from the open source user community.
