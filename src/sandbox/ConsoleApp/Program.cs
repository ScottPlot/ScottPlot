var xs = new double[] { 1, 2, 3, 4, 5 };
var ys = new double[] { 1, 4, 9, 16, 25 };

var plt = new ScottPlot.Plot();
plt.AddScatter(xs, ys);
plt.SaveFig("demo.png");
