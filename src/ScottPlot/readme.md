# ScottPlot
ScottPlot is an open-source interactive graphing library for .NET written in C#. In a GUI environment ScottPlot makes it easy to display data interactively (left-click-drag pan, right-click-drag zoom). ScottPlot was designed to be fast enough to interactively display large datasets with millions of points (such as WAV files) at high framerates. In non-GUI environments ScottPlot can create graphs and save them as images.

![](https://raw.githubusercontent.com/swharden/ScottPlot/master/demos/ScottPlotDemo/compiled/ScottPlotDemo-small.gif)

## Links
* [Project page](https://github.com/swharden/ScottPlot)
* [Documentation](https://github.com/swharden/ScottPlot/tree/master/doc)
* [Cookbook](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook)

## Quickstart

 1. Drag/Drop ScottPlotUC (from the toolbox) onto your form
 2. Add this code to your startup sequence:

```cs
double[] xs = new double[] {1, 2, 3, 4, 5};
double[] ys = new double[] {1, 4, 9, 16, 25};
scottPlotUC1.plt.PlotScatter(xs, ys);
scottPlotUC1.plt.AxisAuto();
scottPlotUC1.Render();
```