# ScottPlot Quickstart: Console Application

* Full source: [demos/ScottPlotQuickstartConsole](/demos/ScottPlotQuickstartConsole)

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
plt.AxisAuto();
plt.SaveFig("console.png");
```

![demos/ScottPlotQuickstartConsole](/demos/ScottPlotQuickstartConsole/ScottPlot-console-output.png)