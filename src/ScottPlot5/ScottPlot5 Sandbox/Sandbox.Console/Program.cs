ScottPlot.Plot plot = new();
plot.Add.Signal(ScottPlot.Generate.Sin());
plot.Add.Signal(ScottPlot.Generate.Cos());
plot.SavePng("test.png", 400, 300);

plot.ScaleFactor = 2;
plot.SavePng("test2.png", 400, 300);
