**ScottPlot is a free and open-source interactive plotting library for .NET** which makes it easy to interactively display data in a variety of formats. You can create interactive line plots, bar charts, scatter plots, etc., with just a few lines of code (see the [ScottPlot Cookbook](https://github.com/swharden/ScottPlot/tree/master/cookbook) for examples). 

[![](https://raw.githubusercontent.com/swharden/ScottPlot/master/demos/ScottPlot-screenshot.gif)](https://github.com/swharden/ScottPlot)

In graphical environments plots can be displayed interactively (left-click-drag to pan and right-click-drag to zoom) and in console applications plots can be created and saved as images. 

ScottPlot targets multiple frameworks (.NET Framework 4.5 and .NET Core 3.0), has user controls for WinForms and WPF, and is [available on NuGet](https://www.nuget.org/packages/ScottPlot/) with no dependencies.

## Quickstart

### Windows Forms Application

 1. Drag/Drop FormsPlot (from the toolbox) onto your form.
 2. Add this code to your startup sequence:

```cs
double[] xs = new double[] {1, 2, 3, 4, 5};
double[] ys = new double[] {1, 4, 9, 16, 25};
formsPlot1.plt.PlotScatter(xs, ys);
formsPlot1.Render();
```

### Console Application

```cs
double[] xs = new double[] { 1, 2, 3, 4, 5 };
double[] ys = new double[] { 1, 4, 9, 16, 25 };
var plt = new ScottPlot.Plot(600, 400);
plt.PlotScatter(xs, ys);
plt.SaveFig("demo.png");
```

### WPF Application

 1. Drag/Drop WpfPlot (from the toolbox) onto your form.
 2. Add this code to your startup sequence:

```xml
<ScottPlot:ScottPlotWPF Name="wpfPlot1" Margin="10"/>
<Button Content="Add Plot" Click="AddPlot"/>
<Button Content="Clear" Click="Clear"/>
```

```cs
double[] xs = new double[] {1, 2, 3, 4, 5};
double[] ys = new double[] {1, 4, 9, 16, 25};
wpfPlot1.plt.PlotScatter(xs, ys);
wpfPlot1.Render();
```

## Links
* [Cookbook](https://github.com/swharden/ScottPlot/blob/master/cookbook)
* [Documentation](https://github.com/swharden/ScottPlot/tree/master/doc)
* [ScottPlot on GitHub](https://github.com/swharden/ScottPlot)