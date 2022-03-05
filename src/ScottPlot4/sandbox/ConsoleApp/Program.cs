var plt = new ScottPlot.Plot(600, 400);
plt.Palette = ScottPlot.Palette.ColorblindFriendly;

// generate random data
System.Random rand = new(12345);
var xs = ScottPlot.DataGen.RandomWalk(rand, 20);
var ys = ScottPlot.DataGen.RandomWalk(rand, 20);
plt.AddScatter(xs, ys, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 10, label: "original");

// interpolate the same data in different ways
(double[] bzX, double[] bzY) = ScottPlot.Statistics.Interpolation.Bezier.InterpolateXY(xs, ys, .005);
(double[] crX, double[] crY) = ScottPlot.Statistics.Interpolation.CatmullRom.InterpolateXY(xs, ys, 15);
(double[] chX, double[] chY) = ScottPlot.Statistics.Interpolation.Chaikin.InterpolateXY(xs, ys, 4);
(double[] cbX, double[] cbY) = ScottPlot.Statistics.Interpolation.Cubic.InterpolateXY(xs, ys, 200);

// display the different curves as line plots
plt.AddScatterLines(bzX, bzY, lineWidth: 2, label: $"Bezier");
plt.AddScatterLines(crX, crY, lineWidth: 2, label: $"Catmull-Rom");
plt.AddScatterLines(chX, chY, lineWidth: 2, label: $"Chaikin");
plt.AddScatterLines(cbX, cbY, lineWidth: 2, label: $"Cubic");

// style the plot
plt.Legend();
plt.Frameless();
plt.Grid(false);
plt.XAxis2.Label("Spline Interpolation", size: 28, bold: true);
System.Console.WriteLine(plt.SaveFig("demo.png"));
