**ScottPlot is a free and open-source plotting library for .NET** that makes it easy to interactively display large datasets.

[![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/ScottPlot.gif)](https://swharden.com/scottplot)

The [**ScottPlot Cookbook**](https://swharden.com/scottplot/cookbook) demonstrates how to create line plots, bar charts, pie graphs, scatter plots, and more with just a few lines of code.

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

* [**Console Application** Quickstart](https://swharden.com/scottplot/quickstart#console-quickstart)
* [**Windows Forms** Quickstart](https://swharden.com/scottplot/quickstart#windows-forms-quickstart)
* [**WPF** Quickstart](https://swharden.com/scottplot/quickstart#wpf-quickstart)
* [**Avalonia** Quickstart](https://swharden.com/scottplot/quickstart#avalonia-quickstart)

## Interactive Demo

The [**ScottPlot Demo**](https://swharden.com/scottplot/demo) allows you to run these examples interactively.

## ScottPlot Cookbook

The [**ScottPlot Cookbook**](https://swharden.com/scottplot/cookbook) demonstrates how to create line plots, bar charts, pie graphs, scatter plots, and more with just a few lines of code.

[![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/cookbook.jpg)](https://swharden.com/scottplot/cookbook)

# Interactive ScottPlot Controls

ScottPlot WinForms control: https://www.nuget.org/packages/ScottPlot.WinForms

ScottPlot WPF control: https://www.nuget.org/packages/ScottPlot.WPF

ScottPlot Avalonia control: https://www.nuget.org/packages/ScottPlot.Avalonia