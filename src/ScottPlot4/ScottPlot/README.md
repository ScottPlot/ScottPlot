**ScottPlot is a free and open-source plotting library for .NET** that makes it easy to interactively display large datasets.

[![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/ScottPlot.gif)](https://scottplot.net)

The [**ScottPlot Cookbook**](https://scottplot.net/cookbook/4.1/) demonstrates how to create line plots, bar charts, pie graphs, scatter plots, and more with just a few lines of code.

## Quickstart

```cs
double[] xs = new double[] {1, 2, 3, 4, 5};
double[] ys = new double[] {1, 4, 9, 16, 25};

var plt = new ScottPlot.Plot(400, 300);
plt.AddScatter(xs, ys);
plt.SaveFig("console.png");
```

![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/console-quickstart.png)

## Windows Forms Quickstart

Drop a `FormsPlot` from the toolbox onto your form and add the following to your start-up sequence:

```cs
double[] xs = new double[] {1, 2, 3, 4, 5};
double[] ys = new double[] {1, 4, 9, 16, 25};
formsPlot1.Plot.AddScatter(xs, ys);
```

![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/winforms-quickstart.png)

## More Quickstarts

* [**Console Application** Quickstart](https://scottplot.net/quickstart/console/)
* [**Windows Forms** Quickstart](https://scottplot.net/quickstart/winforms/)
* [**WPF** Quickstart](https://scottplot.net/quickstart/wpf/)
* [**Avalonia** Quickstart](https://scottplot.net/quickstart/avalonia/)
* [**Eto** Quickstart](https://scottplot.net/quickstart/eto/)
* [**Powershell** Quickstart](https://scottplot.net/quickstart/powershell/)
* [**Interactive Notebook** Quickstart](https://scottplot.net/quickstart/notebook/)

## Interactive Demo

The [**ScottPlot Demo**](https://scottplot.net/demo/) allows you to run these examples interactively.

## ScottPlot Cookbook

The [**ScottPlot Cookbook**](https://scottplot.net/cookbook/4.1/) demonstrates how to create line plots, bar charts, pie graphs, scatter plots, and more with just a few lines of code.

[![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/cookbook.jpg)](https://scottplot.net/cookbook/4.1/)

## Supported Platforms

### .NET Versions
* .NET Standard 2.0
* .NET Framework 4.6.2 and newer
* .NET (Core) 6 and newer ([compatibility notes](https://scottplot.net/faq/dependencies/))

### Operating Systems

ScottPlot 4 is supported anywhere `System.Drawing.Common` is.

* Windows
* Linux ([extra setup may be required](https://scottplot.net/faq/dependencies/))
* MacOS ([extra setup may be required](https://scottplot.net/faq/dependencies/))

ScottPlot 5 ([in development](https://github.com/scottplot/scottplot)) uses SkiaSharp for improved cross-platform support for .NET 7 and later.

# Interactive ScottPlot Controls

ScottPlot WinForms control: https://www.nuget.org/packages/ScottPlot.WinForms

ScottPlot WPF control: https://www.nuget.org/packages/ScottPlot.WPF

ScottPlot Avalonia control: https://www.nuget.org/packages/ScottPlot.Avalonia