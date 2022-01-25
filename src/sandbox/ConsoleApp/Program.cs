System.Random rand = new();
var xs = ScottPlot.DataGen.RandomWalk(rand, 20);
var ys = ScottPlot.DataGen.RandomWalk(rand, 20);

var plt = new ScottPlot.Plot();
plt.AddScatter(xs, ys);
plt.SaveFig("demo.png");
