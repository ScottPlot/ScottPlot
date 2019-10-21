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

#### MainWindow.xaml

```xaml
<Window x:Class="WpfApp4.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:WpfApp4"
        xmlns:ScottPlot="clr-namespace:ScottPlot;assembly=ScottPlot"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>
        <ScottPlot:WpfPlot Name="wpfPlot1"/>
    </Grid>
</Window>
```

#### MainWindow.xaml.cs

```cs
public MainWindow()
{
    InitializeComponent();
    wpfPlot1.plt.Title("WPF Demo");
    wpfPlot1.plt.YLabel("signal level");
    wpfPlot1.plt.XLabel("horizontal units");
    Random rand = new Random();
    wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 10_000));
    wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 10_000));
    wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 10_000));
    wpfPlot1.plt.AxisAuto();
    wpfPlot1.Render();
}
```

## Links
* [Cookbook](https://github.com/swharden/ScottPlot/blob/master/cookbook)
* [Documentation](https://github.com/swharden/ScottPlot/tree/master/doc)
* [ScottPlot on GitHub](https://github.com/swharden/ScottPlot)