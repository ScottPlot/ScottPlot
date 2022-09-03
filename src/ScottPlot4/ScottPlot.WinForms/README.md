**ScottPlot is a free and open-source plotting library for .NET** that makes it easy to interactively display large datasets. This package provides a Windows Forms control for interactive manipulation of ScottPlot plots.

## Windows Forms Quickstart

Drop a `FormsPlot` from the toolbox onto your form and add the following to your start-up sequence:

```cs
double[] xs = new double[] {1, 2, 3, 4, 5};
double[] ys = new double[] {1, 4, 9, 16, 25};
formsPlot1.Plot.AddScatter(xs, ys);
```

![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/winforms-quickstart.png)

## ScottPlot Cookbook

The [**ScottPlot Cookbook**](https://scottplot.net/cookbook/4.1/) demonstrates how to create line plots, bar charts, pie graphs, scatter plots, and more with just a few lines of code.

[![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/cookbook.jpg)](https://scottplot.net/cookbook/4.1/)