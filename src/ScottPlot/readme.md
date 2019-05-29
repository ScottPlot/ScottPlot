# ScottPlot
**ScottPlot is a free and open-source interactive graphing library for .NET written in C#.** 
In a GUI environment ScottPlot makes it easy to display data interactively (left-click-drag pan, right-click-drag zoom), and in non-GUI environments ScottPlot can be used to create graphs and save them as images. ScottPlot was designed to be fast enough to interactively display large datasets with millions of points at high framerates. ScottPlot is easy to integrate into .NET projects because it is [available on NuGet](https://www.nuget.org/packages/ScottPlot/) and has no dependencies outside the .NET framework libraries.

[![](https://raw.githubusercontent.com/swharden/ScottPlot/master/demos/ScottPlotDemo/compiled/ScottPlotDemo-small.gif)](https://github.com/swharden/ScottPlot)

## Links
* [Project page](https://github.com/swharden/ScottPlot)
* [Documentation](https://github.com/swharden/ScottPlot/tree/master/doc)
* [Cookbook](https://github.com/swharden/ScottPlot/tree/master/doc/cookbook)

## Quickstart: Windows Forms

 1. Drag/Drop ScottPlotUC (from the toolbox) onto your form
 2. Add this code to your startup sequence:

```cs
double[] xs = new double[] {1, 2, 3, 4, 5};
double[] ys = new double[] {1, 4, 9, 16, 25};
scottPlotUC1.plt.PlotScatter(xs, ys);
scottPlotUC1.plt.AxisAuto();
scottPlotUC1.Render();
```

## Quickstart: Console Application ###
	
```cs
double[] xs = new double[] { 1, 2, 3, 4, 5 };
double[] ys = new double[] { 1, 4, 9, 16, 25 };
var plt = new ScottPlot.Plot(600, 400);
plt.PlotScatter(xs, ys);
plt.AxisAuto();
plt.SaveFig("demo.png");
```