# ScottPlot v3

ScottPlot v3 is currently under development.

## Improved API

While previous versions of ScottPlot worked, the API was a bit difficult to learn and work with. A primary goal of ScottPlot v3 is simplification by placing all the things the user is likely to interact with at the top-level of the ScottPlot Plot object.

A ScottPlot plot does not require a GUI to produce, as its output can take the form of a Bitmap object. This makes it suitable for console applications, or obscure and/or cross-platform applications (anything that can save or display a Bitmap). 

```cs
// create a ScottPlot of a defined pixel size
var sp = new ScottPlot.plot(600, 800);

// resize events can happen any time
sp.Resize(601, 801);

// plot different types of data
sp.PlotPoint(x, y);
sp.PlotScatter(xs, ys);
sp.PlotSignal(ys, sampleRate: 1000);
sp.PlotAxVline(x);
sp.PlotAxVlines(xs);
sp.PlotAxHline(y);
sp.PlotAxVlines(ys);
sp.PlotText("hello", x, y);
sp.Clear();

// style with optional named arguments
sp.PlotScatter(xs, ys, lineWidth: 2, lineColor: Color.Blue,
                markerSize: 5, markerColor: Color.Red);

// a Legend will be supported to plot labeled data
sp.PlotScatter(xs, ysWin, label: "windows");
sp.PlotScatter(xs, ysLnx, label: "linux");

// zoom around or set axes like this
sp.Zoom(1.5, 2);
sp.Pan(10, 20);
sp.Axis(-10, 10, -100, 100);
sp.Margin(.1, .2);

// commonly adjusted settings have top-level functions
sp.Padding(20, 30, 40, 50);
sp.Title("Awesome Data", 16);
sp.YLabel("vertical units");
sp.XLabel("Horizontal units");
sp.Grid(color: Color.Gray);
sp.Background(figure: Color.gray, data: Color.white);

// detailed settings can be adjusted manually
sp.settings.gridColor = Color.LightGray;
sp.settings.tickFontSize = 10;
sp.settings.dataFrameColor = Color.Black;

// get the plot as a Bitmap (for display in a Form or something)
pictureBox1.BackgroundImage = sp.GetBitmap();

// save the plot
sp.SaveFig("demo.png")
```

## Interactive ScottPlot

A ScottPlotUC (user control) is provided which provides an easy way to create a mouse-interactive (left-click-drag, right-click-zoom) ScottPlot for Windows Forms. The user control will be developed only after the core ScottPlot API is fully implemented and tests are written.