# ScottPlot Quickstart: WPF Application 

* Full source: [demos/ScottPlotQuickstartWPF](/demos/ScottPlotQuickstartWPF)
* A user control which proves mouse interactivity is not provided at this time.

```xaml
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
    plt.AxisAuto();
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

![demos/ScottPlotQuickstartWPF](/demos/ScottPlotQuickstartWPF/ScottPlot-WPF-demo.png)

