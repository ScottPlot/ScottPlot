**[ScottPlot](https://scottplot.net) is a free and open-source plotting library for .NET** that makes it easy to interactively display large datasets.

[![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/ScottPlot.gif)](https://scottplot.net)

The [**ScottPlot Cookbook**](https://scottplot.net/cookbook/5.0/) demonstrates how to create line plots, bar charts, pie graphs, scatter plots, and more with just a few lines of code.

## Quickstart

```cs
double[] dataX = { 1, 2, 3, 4, 5 };
double[] dataY = { 1, 4, 9, 16, 25 };

ScottPlot.Plot myPlot = new();
myPlot.Add.Scatter(dataX, dataY);
myPlot.SavePng("quickstart.png", 400, 300);
```

![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/console-quickstart.png)

## Windows Forms Quickstart

Install the [`ScottPlot.WinForms` NuGet package](https://www.nuget.org/packages/ScottPlot.WinForms), drop a `FormsPlot` from the toolbox onto your form, then add the following to your start-up sequence:

```cs
double[] dataX = { 1, 2, 3, 4, 5 };
double[] dataY = { 1, 4, 9, 16, 25 };

formsPlot1.Plot.Add.Scatter(dataX, dataY);
formsPlot1.Refresh();
```

![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/winforms-quickstart.png)

## More Quickstarts

* [**Console Application Quickstart**](https://scottplot.net/quickstart/console/)
* [**Windows Forms Quickstart**](https://scottplot.net/quickstart/winforms/)
* [**WPF Quickstart**](https://scottplot.net/quickstart/wpf/)
* [**WinUI Quickstart**](https://scottplot.net/quickstart/winui/)
* [**Uno Platform Quickstart**](https://scottplot.net/quickstart/unoplatform/)
* [**Avalonia Quickstart**](https://scottplot.net/quickstart/avalonia/)
* [**Eto Quickstart**](https://scottplot.net/quickstart/eto/)
* [**Blazor Quickstart**](https://scottplot.net/quickstart/blazor/)
* [**Powershell Quickstart**](https://scottplot.net/quickstart/powershell/)
* [**Polyglot Notebook Quickstart**](https://scottplot.net/quickstart/notebook/)

## Interactive Demo

The [**ScottPlot Demo**](https://scottplot.net/demo/) allows you to run these examples interactively.

## ScottPlot Cookbook

The [**ScottPlot Cookbook**](https://scottplot.net/cookbook/5.0/) demonstrates how to create line plots, bar charts, pie graphs, scatter plots, and more with just a few lines of code.

[![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/cookbook.jpg)](https://scottplot.net/cookbook/5.0/)

# Versions

* **ScottPlot 5.0 is the newest version of ScottPlot.** ScottPlot 5 is actively developed and supports all operating systems. The API is similar but not identical to ScottPlot 4. See the [What's New in ScottPlot 5.0](https://scottplot.net/faq/version-5.0/) page for details.

* **ScottPlot 4.1 is a stable version of ScottPlot** which continues to be maintained but no longer receives major new features. ScottPlot 4 supported all operating systems through .NET 6, but after .NET 7 it can only be used in projects that target Windows.