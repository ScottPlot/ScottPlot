# ScottPlot

**ScottPlot is an open-source interactive graphing library for .NET written in C#.** The core of this project is a portable class library which allows a user to supply figure dimensions and scale information and plot data directly on a bitmap buffer relying on ScottPlot to handle unit-to-pixel conversions, drawing of axis labels, tick marks, grid lines, etc. Although ScottPlot was designed for interactive graphing of large datasets in a GUI environment, its core can generate graphs from within console applications. ScottPlot was loosely inspired by matplotlib for Python.

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

## Additional Examples
* Extensive examples are provided in the **[ScottPlot cookbook](/doc/cookbook)**
* Advanced techniques like responsive resizing and animated data are discussed in [/doc/](/doc).

## Installing ScottPlot

**1.) Download** the [latest ScottPlot (ZIP)](https://github.com/swharden/ScottPlot/archive/master.zip) from this page.

**2.) Add the extracted ScottPlot as a project in your solution.** 

Right-click your solution, _add_, _Existing Project_, then select the [/src/ScottPlot](/src/ScottPlot) folder.

**3.) Add ScottPlot as a reference** to the project you want to use it in. 

Right-click the project, _Add_, _Reference_, then under _Projects_ select _ScottPlot_

**That's it!** You can now create a ScottPlot figure with `var fig = new ScottPlot.Figure(640,480)`. Glance over [the ScottPlot cookbook](/doc/cookbook/) to learn how to use all of ScottPlot's features.
