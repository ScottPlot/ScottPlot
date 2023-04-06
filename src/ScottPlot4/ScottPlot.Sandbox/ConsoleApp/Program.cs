using ScottPlot;

var plt = new ScottPlot.Plot(600, 400);
plt.AddSignal(ScottPlot.DataGen.Sin(51));
plt.AddSignal(ScottPlot.DataGen.Cos(51));

plt.Launch.FormsPlot();
plt.Launch.WpfPlot();