# ScottPlot Quickstarts

The following code examples demonstrate how to get ScottPlot up and running with minimum complexity. The following resources may also be useful:
* Cookbook: [/cookbook](/cookbook)
* Demos: [/demos](/demos)
* API Documentation: [/doc](/doc)

## QuickStart: Console Application

```cs
// generate some data to plot
int pointCount = 100;
double[] dataXs = new double[pointCount];
double[] dataSin = new double[pointCount];
double[] dataCos = new double[pointCount];
for (int i = 0; i < pointCount; i++)
{
    dataXs[i] = i;
    dataSin[i] = Math.Sin(i * 2 * Math.PI / pointCount);
    dataCos[i] = Math.Cos(i * 2 * Math.PI / pointCount);
}

// plot the data
var plt = new ScottPlot.Plot(500, 300);
plt.PlotScatter(dataXs, dataSin);
plt.PlotScatter(dataXs, dataCos);
plt.Title("ScottPlot Quickstart (console)");
plt.XLabel("experiment time (ms)");
plt.YLabel("signal (mV)");
plt.SaveFig("console.png");
```

![](console.png)

## Quickstart: WinForms Application

1. Install ScottPlot using NuGet
2. Drag/Drop the FormsPlot (from the toolbox) onto your Form
3. Add the following code to your startup sequence:

```cs
// generate some data to plot
int pointCount = 100;
double[] dataXs = new double[pointCount];
double[] dataSin = new double[pointCount];
double[] dataCos = new double[pointCount];
for (int i = 0; i < pointCount; i++)
{
	dataXs[i] = i;
	dataSin[i] = Math.Sin(i * 2 * Math.PI / pointCount);
	dataCos[i] = Math.Cos(i * 2 * Math.PI / pointCount);
}

// plot the data
formsPlot1.plt.PlotScatter(dataXs, dataSin);
formsPlot1.plt.PlotScatter(dataXs, dataCos);
formsPlot1.plt.XLabel("experiment time (ms)");
formsPlot1.plt.YLabel("signal (mV)");
formsPlot1.plt.Title("ScottPlot Quickstart");
formsPlot1.Render();
```

![](winforms.png)

### WPF Application

Using the WPF user control is the same as the WinForms user control. Virtually all your interaction is with the `wpfPlot1.plt` object (use it just like the plot object in the [cookbook](/cookbook)), then optionally force a render with `wpfPlot1.render()`. The user control handles mouse interactivity (left-click-drag pan, right-click-drag zoom).

```xml
<ScottPlot:ScottPlotWPF Name="wpfPlot1" Margin="10"/>
<Button Content="Add Plot" Click="AddPlot"/>
<Button Content="Clear" Click="Clear"/>
```

```cs
public MainWindow()
{
    InitializeComponent();
    wpfPlot1.plt.Title("WPF Demo");
    wpfPlot1.plt.YLabel("signal level");
    wpfPlot1.plt.XLabel("horizontal units");
}

private void AddPlot(object sender, RoutedEventArgs e)
{
    Random rand = new Random();
    wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 1000));
    wpfPlot1.plt.AxisAuto();
    wpfPlot1.Render();
}

private void Clear(object sender, RoutedEventArgs e)
{
    wpfPlot1.plt.Clear();
    wpfPlot1.Render();
}
```

![](wpf.png)