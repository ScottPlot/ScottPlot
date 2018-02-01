# ScottPlot

***ScottPlot is an open-source interactive graphing library for .NET written in C#.*** The core of this project is a portable class library which allows a user to supply figure dimensions and scale information and plot data directly on a bitmap buffer relying on ScottPlot to handle unit-to-pixel conversions, drawing of axis labels, tick marks, grid lines, etc. Although ScottPlot was designed for interactive graphing of large datasets in a GUI environment, its core can generate graphs from within console applications. ScottPlot was loosely inspired by matplotlib for Python.

> **WARNING: THIS PROJECT IS NOT READY FOR WIDESPREAD USE!** ScottPlot is still in the very early stages of development. Its API has not yet solidified, so building projects around it is not yet recommended. At this time, this repository is intended to track the development of this projet (rather than distribute it).

## Windows Forms
In this example, clicking button1 draws a graph and applies it to a picturebox.

```C#
private void button1_Click(object sender, EventArgs e)
{
  Figure fig = new Figure(pictureBox1.Width, pictureBox1.Height);
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
  fig.BenchmarkThis();
  fig.PlotLines(Xs, Ys, 1, Color.Red);
  fig.PlotScatter(Xs, Ys, 5, Color.Blue);

  pictureBox1.Image = fig.Render();
}
```

![](/doc/screenshots/picturebox.png)


## Console Applications
ScottPlot does not require a GUI to create graphs, as they can be easily saved as BMP, JPG, or PNG files.

```C#
static void Main(string[] args)
{
  // create a new ScottPlot figure
  Figure fig = new Figure(640, 480);
  fig.title = "Plotting Point Arrays";
  fig.yLabel = "Random Walk";
  fig.xLabel = "Sample Number";

  // generate data
  int pointCount = 123;
  double[] Xs = fig.gen.Sequence(pointCount);
  double[] Ys = fig.gen.RandomWalk(pointCount);
  fig.ResizeToData(Xs, Ys, .9, .9);

  // make the plot
  fig.BenchmarkThis();
  fig.PlotLines(Xs, Ys, 1, Color.Red);
  fig.PlotScatter(Xs, Ys, 5, Color.Blue);

  // save the file
  fig.Save("output.png");
}
```

![](/doc/screenshots/console.png)


## Cookbook
For an extensive collection of current code examples see the **[ScottPlot cookbook](/doc/cookbook)*
