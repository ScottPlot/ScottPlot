
## Quickstart (NuGet)
1. Install [ScottPlot using NuGet](https://www.nuget.org/packages/ScottPlot/)
2. Drag/Drop the ScottPlotUC (from the toolbox) onto your Form
3. Add the following code to your startup sequence:

```cs
double[] xs = new double[] {1, 2, 3, 4, 5};
double[] ys = new double[] {1, 4, 9, 16, 25};
scottPlotUC1.plt.PlotScatter(xs, ys);
scottPlotUC1.plt.AxisAuto();
scottPlotUC1.Render();
```

![](/dev/nuget/quickstart.png)

* Review the [documentation](/doc/) pages to learn how to use additional ScottPlot features

## Quickstart (from source)
This quickstart demonstrates how to add an interactive ScottPlot to a Windows Forms Application using the ScottPlotUC (user control) from the source code folder. Note that ScottPlot also has a [Console Application quickstart](/doc/quickstart-console.md) and a [WPF Application quickstart](/doc/quickstart-WPF.md) for those interested in using ScottPlot in other application frameworks. 

* Create a Windows Forms Application
* Add an existing project reference to [ScottPlot.csproj](/src/ScottPlot/ScottPlot.csproj)
* Rebuild the solution
* Drag/Drop the ScottPlotUC (from the toolbox) onto your form
* Add the following code to your startup sequence:

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
scottPlotUC1.plt.PlotScatter(dataXs, dataSin);
scottPlotUC1.plt.PlotScatter(dataXs, dataCos);
scottPlotUC1.plt.XLabel("experiment time (ms)");
scottPlotUC1.plt.YLabel("signal (mV)");
scottPlotUC1.plt.Title("ScottPlot Quickstart");
scottPlotUC1.plt.AxisAuto();
scottPlotUC1.Render();
```

![](/demos/ScottPlotQuickstartForms/compiled/ScottPlotQuickstartForms.png)

* Review the [documentation](/doc/) pages to learn how to use additional ScottPlot features
* Full source code: [demos/ScottPlotQuickstartForms](/demos/ScottPlotQuickstartForms) 
* Compiled version: [ScottPlotQuickstartForms.zip](/demos/ScottPlotQuickstartForms/compiled/ScottPlotQuickstartForms.zip)
