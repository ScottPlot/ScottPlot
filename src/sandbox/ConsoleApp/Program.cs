System.Random rand = new();
var xs = ScottPlot.DataGen.RandomWalk(rand, 20);
var ys = ScottPlot.DataGen.RandomWalk(rand, 20);

var plt = new ScottPlot.Plot();
plt.AddScatter(xs, ys);
var cha = ScottPlot.Statistics.Interpolation.Chaikin.InterpolateXY(xs, ys, 2);
plt.AddScatter(cha.xs, cha.ys);

plt.SaveFig("demo.png");
