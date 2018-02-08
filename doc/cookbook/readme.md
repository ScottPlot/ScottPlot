# ScottPlot Cookbook

This document was generated automatically by a [program](/src/Examples/console/ConsoleApp1/Program.cs)  which pulls examples from [Cookbook.cs](/src/Examples/console/ConsoleApp1/Cookbook.cs). Since it generates graphs using many different features, it doubles as both a test script and a cookbook to demonstrate various uses of the ScottPlot API.

## demo_001

```C#
/// Create a plot of data from two arrays of doubles (Xs and Ys)

public static void demo_001()
{
    // create a new ScottPlot figure
    var fig = new ScottPlot.Figure(640, 480);
    fig.labelTitle = "Super Special Data";
    fig.labelY = "Random Walk";
    fig.labelX = "Sample Number";

    // generate data
    int pointCount = 123;
    double[] Xs = fig.gen.Sequence(pointCount);
    double[] Ys = fig.gen.RandomWalk(pointCount);

    // adjust the axis to fit the data (then zoom out slightly)
    fig.AxisAuto(Xs, Ys, .9, .9);

    // make the plot
    fig.BenchmarkThis();
    fig.PlotLines(Xs, Ys, 1, Color.Red);
    fig.PlotScatter(Xs, Ys, 5, Color.Blue);

    // save the file
    fig.Save("output/demo_001.png");
}
```
![](demo_001.png)


## demo_002

```C#
/// Zooming

public static void demo_002()
{
    var fig = new ScottPlot.Figure(640, 480);
    fig.labelTitle = "Super Special Data";
    fig.labelY = "Random Walk";
    fig.labelX = "Sample Number";

    double[] Xs = fig.gen.Sequence(123);
    double[] Ys = fig.gen.RandomWalk(123);

    fig.AxisAuto(Xs, Ys, null, null); // fit data precisely            
    fig.Zoom(2, .5);  // now zoom in horizontally and out vertically

    fig.PlotLines(Xs, Ys, 1, Color.Red);
    fig.PlotScatter(Xs, Ys, 5, Color.Blue);

    fig.Save("output/demo_002.png");
}
```
![](demo_002.png)


## demo_003

```C#
/// Changing colors

public static void demo_003()
{
    var fig = new ScottPlot.Figure(640, 480);
    fig.labelTitle = "Super Special Data";
    fig.labelY = "Random Walk";
    fig.labelX = "Sample Number";

    // go to town changing colors
    fig.colorAxis = Color.Yellow;
    fig.colorFigBg = Color.FromArgb(255,30,30,30);
    fig.colorGridLines = Color.FromArgb(255, 55, 55, 55);
    fig.colorGraphBg = Color.FromArgb(255, 40, 40, 40);

    double[] Xs = fig.gen.Sequence(123);
    double[] Ys = fig.gen.RandomWalk(123);
    fig.AxisAuto(Xs, Ys, .9, .9);

    fig.BenchmarkThis();
    fig.PlotLines(Xs, Ys, 1, Color.Gray);
    fig.PlotScatter(Xs, Ys, 5, Color.White);

    fig.Save("output/demo_003.png");
}
```
![](demo_003.png)


## demo_004

```C#
/// Overlapping plots of different sizes and colors

public static void demo_004()
{
    var fig = new ScottPlot.Figure(640, 480);
    fig.labelTitle = "Super Special Data";
    fig.labelY = "Random Walk";
    fig.labelX = "Sample Number";

    // use the same Xs every time
    double[] Xs = fig.gen.Sequence(123);

    // manually define axis
    fig.AxisSet(-5, 130, -10, 10); 

    fig.BenchmarkThis();
    fig.PlotLines(Xs, fig.gen.RandomWalk(123), 1, Color.Red);
    fig.PlotLines(Xs, fig.gen.RandomWalk(123), 2, Color.Orange);
    fig.PlotLines(Xs, fig.gen.RandomWalk(123), 3, Color.Yellow);
    fig.PlotLines(Xs, fig.gen.RandomWalk(123), 4, Color.Green);
    fig.PlotLines(Xs, fig.gen.RandomWalk(123), 5, Color.Blue);
    fig.PlotLines(Xs, fig.gen.RandomWalk(123), 6, Color.Indigo);
    fig.PlotLines(Xs, fig.gen.RandomWalk(123), 7, Color.Violet);

    fig.Save("output/demo_004.png");
}
```
![](demo_004.png)


## demo_005

```C#
/// Demonstrate transparency

public static void demo_005()
{
    var fig = new ScottPlot.Figure(640, 480);
    fig.labelTitle = "Super Special Data";
    fig.labelY = "Random Walk";
    fig.labelX = "Sample Number";

    // use the same Xs every time
    double[] Xs = fig.gen.Sequence(123);

    // manually define axis
    fig.AxisSet(-5, 130, -10, 10);

    // plot lines with different colors
    Color[] colors = new Color[] { Color.FromArgb(100, 255, 0, 0),  // red
                                   Color.FromArgb(100, 0, 150, 0),  // green
                                   Color.FromArgb(100, 0, 0, 255)}; // blue

    fig.BenchmarkThis();
    for (int i=0; i<colors.Length; i++) // for each color
    {
        for (int j=0; j<3; j++) // draw 3 lines
        {
            fig.PlotLines(Xs, fig.gen.RandomWalk(123), 5, colors[i]);
        }
    }

    fig.Save("output/demo_005.png");
}
```
![](demo_005.png)


## demo_006

```C#
/// Scatter plot features

public static void demo_006()
{
    // create a new ScottPlot figure
    var fig = new ScottPlot.Figure(640, 480);
    fig.labelTitle = "Super Special Data";
    fig.labelY = "Random Walk";
    fig.labelX = "Sample Number";

    // generate data
    int pointCount = 40;
    double[] Xs = fig.gen.Sequence(pointCount);

    // manually define axis
    fig.AxisSet(-3, 43, -2, 4);

    // make the plot
    fig.BenchmarkThis();
    fig.PlotScatter(Xs, fig.gen.RandomWalk(pointCount), 2, Color.Black);
    fig.PlotScatter(Xs, fig.gen.RandomWalk(pointCount), 5, Color.Red);
    fig.PlotScatter(Xs, fig.gen.RandomWalk(pointCount), 10, Color.Green);
    fig.PlotScatter(Xs, fig.gen.RandomWalk(pointCount), 20, Color.FromArgb(100,0,0,255));

    // save the file
    fig.Save("output/demo_006.png");
}
```
![](demo_006.png)


## demo_007

```C#
/// Draw a square with an X in it

public static void demo_007()
{
    // create a new ScottPlot figure
    var fig = new ScottPlot.Figure(640, 480);

    // zoom and pan axes before drawing on them
    fig.Zoom(.8, .8);

    // draw a blue X
    fig.PlotLines(-10, 10, -10, 10, 5, Color.Blue);
    fig.PlotLines(-10, 10, 10, -10, 5, Color.Blue);

    // draw a red rectangle
    double[] Xs = { -10, 10, 10, -10, -10 };
    double[] Ys = { -10, -10, 10, 10, -10 };
    fig.PlotLines(Xs, Ys, 5, Color.Red);

    // save the file
    fig.Save("output/demo_007.png");
}
```
![](demo_007.png)


## demo_008

```C#
/// Pan by a certain distance (in axis units)

public static void demo_008()
{
    // create a new ScottPlot figure
    var fig = new ScottPlot.Figure(640, 480);

    // zoom and pan axes before drawing on them
    fig.Zoom(.8, .8);
    fig.PanUnits(4, 4);

    // draw a blue X
    fig.PlotLines(-10, 10, -10, 10, 5, Color.Blue);
    fig.PlotLines(-10, 10, 10, -10, 5, Color.Blue);

    // draw a red rectangle
    double[] Xs = { -10, 10, 10, -10, -10 };
    double[] Ys = { -10, -10, 10, 10, -10 };
    fig.PlotLines(Xs, Ys, 5, Color.Red);

    // save the file
    fig.Save("output/demo_008.png");
}
```
![](demo_008.png)


## demo_009

```C#
/// Pan by a certain number of PIXELS (useful for mouse tracking)

public static void demo_009()
{
    // create a new ScottPlot figure
    var fig = new ScottPlot.Figure(640, 480);

    // zoom and pan axes before drawing on them
    fig.Zoom(.8, .8);
    fig.PanPixels(100, 100);

    // draw a blue X
    fig.PlotLines(-10, 10, -10, 10, 5, Color.Blue);
    fig.PlotLines(-10, 10, 10, -10, 5, Color.Blue);

    // draw a red rectangle
    double[] Xs = { -10, 10, 10, -10, -10 };
    double[] Ys = { -10, -10, 10, 10, -10 };
    fig.PlotLines(Xs, Ys, 5, Color.Red);

    // save the file
    fig.Save("output/demo_009.png");
}
```
![](demo_009.png)


## demo_010

```C#
/// Plot one million data points using PlotLines() - do not do this!!
/// For high density data (with a large number of data points) evenly spaced, use PlotSignal().
/// The purpose of this demonstration is to highlight how much faster PlotSignal() is over PlotLines()

public static void demo_010()
{
    // create a new ScottPlot figure
    var fig = new ScottPlot.Figure(640, 480);
    fig.labelTitle = "1 Million Points with PlotLines()";
    fig.labelY = "value";
    fig.labelX = "time (seconds)";

    // create ONE MILLION points
    double[] Xs = fig.gen.Sequence(1_000_000, 1.0 / 20e3); // 20 kHz
    double[] Ys = fig.gen.RandomWalk(1_000_000);
    fig.AxisAuto(Xs, Ys, null, .9);

    // using the SLOW METHOD
    fig.BenchmarkThis();
    fig.PlotLines(Xs, Ys, 1, Color.Red);

    // save the file
    fig.Save("output/demo_010.png");
}
```
![](demo_010.png)


## demo_011

```C#
/// Plot one million data points using PlotSignal()
/// This method is ideal for large amounts of evenly-spaced data.

public static void demo_011()
{
    // create a new ScottPlot figure
    var fig = new ScottPlot.Figure(640, 480);
    fig.labelTitle = "1 Million Points with PlotSignal()";
    fig.labelY = "value";
    fig.labelX = "time (seconds)";

    // create ONE MILLION points
    double[] Ys = fig.gen.RandomWalk(1_000_000);
    fig.AxisAuto(null, Ys, null, .9); // resize Y to data
    fig.AxisSet(0, Ys.Length / 20e3,null,null); // resize X manually

    // plot using the FAST METHOD
    fig.BenchmarkThis();
    fig.PlotSignal(Ys, 1.0 / 20e3);

    // save the file
    fig.Save("output/demo_011.png");
}
```
![](demo_011.png)


## demo_012

```C#
/// Stress-test PlotSignal() with 100 MILLION data points

public static void demo_012()
{
    // create a new ScottPlot figure
    var fig = new ScottPlot.Figure(640, 480);
    fig.labelTitle = "100 Million Point Stress-Test";
    fig.labelY = "value";
    fig.labelX = "time (seconds)";

    // create ONE MILLION points
    double[] Ys = fig.gen.RandomWalk(100_000_000);
    fig.AxisAuto(null, Ys, null, .9); // resize Y to data
    fig.AxisSet(0, Ys.Length / 20e3, null, null); // resize X manually

    // plot using the FAST METHOD
    fig.BenchmarkThis();
    fig.PlotSignal(Ys, 1.0 / 20e3);

    // save the file
    fig.Save("output/demo_012.png");
}
```
![](demo_012.png)


## demo_101

```C#
/// Draw by directly interacting with the graphics object and position/pixel conversion

public static void demo_101()
{
    // create a new ScottPlot figure
    var fig = new ScottPlot.Figure(640, 480);
    fig.labelTitle = "Direct Graphics Drawing";
    fig.labelY = "Pure Awesomeness";
    fig.labelX = "Relative Time (years)";
    fig.AxisSet(-15, 35, -10, 110); // x1, x2, y1, y2

    // draw a line directly on the Graphics object in AXIS units
    Point pt1 = new Point(fig.xAxis.GetPixel(0), fig.yAxis.GetPixel(13));
    Point pt2 = new Point(fig.xAxis.GetPixel(32), fig.yAxis.GetPixel(98));
    fig.gfxGraph.DrawLine(new Pen(new SolidBrush(Color.Blue), 5), pt1, pt2);

    // save the file
    fig.Save("output/demo_101.png");
}
```
![](demo_101.png)
