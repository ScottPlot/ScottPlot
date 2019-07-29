# ScottPlot Demos

**[ScottPlot-Demos.zip](ScottPlot-Demos.zip)** contains several applications which demonstrate different ways ScottPlot can be used. The best demo to try-out first is _plot types.exe_.

Source code for all demos is in [/demos/src/](/demos/src/)

![](/demos/src/plot-types/ScottPlot-screenshot.png)

## QuickStart Demos

### Console Application

Full source code is in [/demos/src/](/demos/src/)

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

![](/demos/src/quickstart-console/output.png)

### WinForms Application

Full source code is in [/demos/src/](/demos/src/)

1. Install ScottPlot using NuGet
2. Drag/Drop the ScottPlotUC (from the toolbox) onto your Form
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
scottPlotUC1.plt.PlotScatter(dataXs, dataSin);
scottPlotUC1.plt.PlotScatter(dataXs, dataCos);
scottPlotUC1.plt.XLabel("experiment time (ms)");
scottPlotUC1.plt.YLabel("signal (mV)");
scottPlotUC1.plt.Title("ScottPlot Quickstart");
scottPlotUC1.Render();
```

![](/demos/src/quickstart-winforms/screenshot.png)

### WPF Application

Full source code is in [/demos/src/](/demos/src/)

_Note: A WPF user control which proves mouse interactivity does not exist at this time. This method creates a graph as a Bitmap then applied it to an Image object._

```xml
<Canvas Name="canvasPlot" Height="Auto" Width="Auto">
    <Image Name="imagePlot" Width="{Binding ActualWidth}" Height="{Binding ActualHeight}"/>
</Canvas>
```

```cs
private void Window_Loaded(object sender, RoutedEventArgs e)
{
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
    ScottPlot.Plot plt = new ScottPlot.Plot((int)canvasPlot.ActualWidth, 
                                            (int)canvasPlot.ActualHeight);
    plt.PlotScatter(dataXs, dataSin);
    plt.PlotScatter(dataXs, dataCos);
    plt.Title("ScottPlot WPF Demo");
    imagePlot.Source = bmpImageFromBmp(plt.GetBitmap());
}
```

```cs
public BitmapImage bmpImageFromBmp(System.Drawing.Bitmap bmp)
{
    System.IO.MemoryStream stream = new System.IO.MemoryStream();
    ((System.Drawing.Bitmap)bmp).Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
    BitmapImage bmpImage = new BitmapImage();
    bmpImage.BeginInit();
    stream.Seek(0, System.IO.SeekOrigin.Begin);
    bmpImage.StreamSource = stream;
    bmpImage.EndInit();
    return bmpImage;
}
```

![](/demos/src/quickstart-wpf/screenshot.png)