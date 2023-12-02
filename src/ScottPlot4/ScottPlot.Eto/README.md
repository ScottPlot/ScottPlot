**ScottPlot is a free and open-source plotting library for .NET** that makes it easy to interactively display large datasets. This package provides an Eto control for interactive manipulation of ScottPlot plots. 

## Quickstart

```cs
double[] xs = new double[] { 1, 2, 3, 4, 5 };
double[] ys = new double[] { 1, 4, 9, 16, 25 };

var plotView = new ScottPlot.Eto.PlotView();
plotView.Plot.AddScatter(xs, ys);
plotView.Refresh();
this.Content = plotView;
```

| WPF                                                                                                   | GTK                                                                                                   | OSX                                                                                                   |
| ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| ![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/eto-quickstart-wpf.png) | ![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/eto-quickstart-gtk.png) | ![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/eto-quickstart-osx.png) |

For more details including MacOS and Linux instructions visit the ScottPlot Eto Quickstart web page: https://scottplot.net/quickstart/eto/