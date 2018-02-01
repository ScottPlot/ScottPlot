# ScottPlot Cookbook

This document was generated automatically from [Cookbook.cs](/src/Examples/console/ConsoleApp1/Cookbook.cs). Since it generates graphs using many different features, it doubles as both a test script and a cookbook to demonstrate various uses of the ScottPlot API.

## demo_001

```C#
/// <summary>
/// plot data from an Xs and Ys array (of doubles)
/// </summary>
public static void demo_001()
{
    // create a new ScottPlot figure
    Figure fig = new Figure(640, 480);
    fig.title = "Plotting Point Arrays";
    fig.yLabel = "Random Walk";
    fig.xLabel = "Sample Number";

    // generate data
    int pointCount = 123; // number of points which will be in this graph
    double[] Xs = gen.Sequence(pointCount); // create a series of Xs
    double[] Ys = gen.RandomWalk(pointCount);
    fig.ResizeToData(Xs, Ys, .9, .9);

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
/// <summary>
/// minimal-case example drawing a line and saving a file
/// </summary>
public static void demo_002()
{
    // create a new ScottPlot figure
    Figure fig = new Figure(640, 480);
    fig.title = "Direct Graphics Drawing";
    fig.yLabel = "Pure Awesomeness";
    fig.xLabel = "Relative Time (years)";
    fig.Axis(-15, 35, -10, 110); // x1, x2, y1, y2
    fig.BenchmarkThis();

    // draw a line directly on the Graphics object in AXIS units
    Point pt1 = new Point(fig.xAxis.UnitToPx(0), fig.yAxis.UnitToPx(13));
    Point pt2 = new Point(fig.xAxis.UnitToPx(32), fig.yAxis.UnitToPx(98));
    fig.gfxGraph.DrawLine(new Pen(new SolidBrush(Color.Blue), 5), pt1, pt2);

    // save the file
    fig.Save("output/demo_002.png");
}
```
![](demo_002.png)
