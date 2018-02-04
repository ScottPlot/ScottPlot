# ScottPlot

**ScottPlot is an open-source interactive graphing library for .NET written in C#.** It was written to simplify the task of interactively displaying data on a graph that you can left-click-drag to pan and right-click drag to zoom. The core of this project is a portable class library which allows a user to supply figure dimensions and scale information and plot data directly on a bitmap buffer relying on ScottPlot to handle unit-to-pixel conversions, drawing of axis labels, tick marks, grid lines, etc. Although ScottPlot was designed for interactive graphing of large datasets in a GUI environment, its core can generate graphs from within console applications. ScottPlot was loosely inspired by matplotlib for Python.

![](/doc/screenshots/resize-pan-zoom.gif)

## Use ScottPlot in Console Applications
ScottPlot does not require a GUI to create graphs, as they can be easily saved as BMP, JPG, or PNG files.

```C#
static void Main(string[] args)
{
  // create a new ScottPlot figure
  var fig = new ScottPlot.Figure(640, 480);
  fig.title = "Plotting Point Arrays";
  fig.yLabel = "Random Walk";
  fig.xLabel = "Sample Number";

  // generate data
  int pointCount = 123;
  double[] Xs = fig.gen.Sequence(pointCount);
  double[] Ys = fig.gen.RandomWalk(pointCount);
  fig.ResizeToData(Xs, Ys, .9, .9);

  // make the plot
  fig.PlotLines(Xs, Ys, 1, Color.Red);
  fig.PlotScatter(Xs, Ys, 5, Color.Blue);

  // save the file
  fig.Save("output.png");
}
```

![](/doc/screenshots/console.png)

## Use ScottPlot in Windows Forms
In this example, clicking button1 draws a graph and applies it to a picturebox.

```C#
private void button1_Click(object sender, EventArgs e)
{
  var fig = new ScottPlot.Figure(pictureBox1.Width, pictureBox1.Height);
  fig.styleForm(); // optimizes colors for forms
  fig.title = "Plotting Point Arrays";
  fig.yLabel = "Random Walk";
  fig.xLabel = "Sample Number";

  // generate data
  int pointCount = 123;
  double[] Xs = fig.gen.Sequence(pointCount);
  double[] Ys = fig.gen.RandomWalk(pointCount);
  fig.ResizeToData(Xs, Ys, .9, .9);

  // make the plot
  fig.PlotLines(Xs, Ys, 1, Color.Red);
  fig.PlotScatter(Xs, Ys, 5, Color.Blue);
  
  // place the graph onto a picturebox
  pictureBox1.Image = fig.Render();
}
```

![](/doc/screenshots/picturebox.png)


## ScottPlot User Controls
ScottPlot user controls simplify the task of creating interactive graphs. Different user controls are optimized for specific tasks. For example, the ucSignal user control is designed to take very large arrays of data (tens of millions of data points) and produce an interactive plot which updates at extremely high speed.

```C#
// After creating a new windows application, drag/drop the ucSignal user control onto it.
// This is the entire code required to produce an interactive graph of one million data points!
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        ucSignal1.showBenchmark = true;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        // create one million points of signal data
        int pointCount = 1_000_000;
        double[] Ys = new double[pointCount];
        for (int i = 0; i < pointCount; i++) Ys[i] = Math.Sin((double)i / 5000);
        ucSignal1.fig.title = string.Format("{0:n0} Data Points", pointCount);

        // load the data into the user control (and auto-fit axis limits to the data)
        ucSignal1.Ys = Ys; 
        ucSignal1.ResetAxis();
    }
}
```

<img src="/doc/screenshots/ucSignal2.png" width="604">

## Additional Examples
* Extensive examples are provided in the **[ScottPlot cookbook](/doc/cookbook)**

## Installing ScottPlot

* **Download:** Get the [latest ScottPlot (ZIP)](https://github.com/swharden/ScottPlot/archive/master.zip) from this page

* **Add Project:** Right-click your solution, _add_, _Existing Project_, and select [/src/ScottPlot/ScottPlot.csproj](/src/ScottPlot/ScottPlot.csproj)

* **Add Reference:** Right-click your project, _Add_, _Reference_, then under _Projects_ select _ScottPlot_

## License
ScottPlot uses the [MIT License](LICENSE), so use it in whatever you want!
