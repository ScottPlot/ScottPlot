# ScottPlot

[![](https://img.shields.io/azure-devops/build/swharden/ScottPlot/15/master?logo=azure%20pipelines)](https://dev.azure.com/swharden/ScottPlot/_build?definitionId=15)
[![](https://img.shields.io/azure-devops/tests/swharden/ScottPlot/15/master?logo=azure%20pipelines)](https://dev.azure.com/swharden/ScottPlot/_build?definitionId=15)
[![](https://img.shields.io/nuget/dt/scottplot?color=004880&label=Downloads&logo=NuGet)](https://www.nuget.org/packages/ScottPlot/)
[![](https://img.shields.io/nuget/vpre/scottplot?color=%23004880&label=NuGet&logo=nuget)](https://www.nuget.org/packages/ScottPlot/)

**ScottPlot is a free and open-source plotting library for .NET** that makes it easy to interactively display large datasets. The [**ScottPlot Cookbook**](https://swharden.com/scottplot/cookbook) demonstrates how to create line plots, bar charts, pie graphs, scatter plots, and more with just a few lines of code.

* **[ScottPlot Cookbook](https://swharden.com/scottplot/cookbook)** 👈 _Learn how to use ScottPlot_

* **[ScottPlot Demo](https://swharden.com/scottplot/demo)** 👈 _See what ScottPlot can do_

* **Quickstart:** [WinForms](https://swharden.com/scottplot/quickstart#windows-forms-quickstart), [WPF](https://swharden.com/scottplot/quickstart#wpf-quickstart), [Avalonia](https://swharden.com/scottplot/quickstart#avalonia-quickstart), [Console](https://swharden.com/scottplot/quickstart#console-quickstart)

* [**What's New in ScottPlot 4.1?**](https://swharden.com/scottplot/faq/version-4.1/) _Released May, 2021_

<div align='center'>

<a href='https://swharden.com/scottplot'><img src='dev/graphics/ScottPlot.gif'></a>

<a href='https://swharden.com/scottplot/cookbook'><img src='dev/graphics/cookbook.jpg'></a>

</div>

### Questions and Feedback

* **Ask questions** in [Discussions](https://github.com/swharden/ScottPlot/discussions/categories/q-a), [Issues](https://github.com/swharden/ScottPlot/issues), or [on StackOverflow]((https://stackoverflow.com/questions/ask?tags=scottplot))

* [**Create an issue**](https://github.com/swharden/ScottPlot/issues) for a feature suggestion or bug report

* If you enjoy ScottPlot **give us a star!** ⭐

### Quickstart (Windows Forms)

Use NuGet to install [`ScottPlot.WinForms`](https://www.nuget.org/packages/ScottPlot.WinForms), drag/drop a `FormsPlot` onto your form, then add the following to your start-up sequence.

```cs
double[] dataX = new double[] {1, 2, 3, 4, 5};
double[] dataY = new double[] {1, 4, 9, 16, 25};
formsPlot1.Plot.AddScatter(dataX, dataY);
```

![](dev/graphics/winforms-quickstart.png)

### Plot in the Cloud

**This figure is an example of ScottPlot running in the cloud.** Once daily an [Azure Function](https://azure.microsoft.com/en-us/services/functions/) logs the number of stars this repository has and creates a graph of the star history using ScottPlot. Since the output is simply a static image accessible by a URL, it can be displayed in places where JavaScript is not permitted like in this readme file. Rendering plots server-side allows automatically-updating plots to be displayed in many interesting places.

```cs
var plt = new ScottPlot.Plot(700, 400);
plt.AddScatter(dates, stars);
plt.XAxis.DateTimeFormat(true);
plt.Title("ScottPlot Stars on GitHub");
plt.YLabel("Total Stars");
plt.SaveFig("quickstart.png");
```

<p align="center">
  <img src="https://swhardendev.z13.web.core.windows.net/packagestats/scottplot-stars.png">
</p>

### Developer Notes

* **Minimum Supported Platforms:** .NET Core 2.0 and .NET Framework 4.6.1 ([see details](https://swharden.com/scottplot/#supported-platforms))

* **Active development is focused on** topics listed in the pinned _Triaged Tasks and Features_ [issues](https://github.com/ScottPlot/ScottPlot/issues).

* **Contributions are welcome!** See [contributing.md](CONTRIBUTING.md) to get started

### About ScottPlot

ScottPlot was created by [Scott W Harden](https://swharden.com/about/) (with many contributions from the open-source community) and is provided under the permissive [MIT license](LICENSE).