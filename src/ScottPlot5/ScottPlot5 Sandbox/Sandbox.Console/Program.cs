using ScottPlot;

Plot myPlot = new();
myPlot.Add.Signal(Generate.Sin());
myPlot.Add.Signal(Generate.Cos());

myPlot.SavePng("test.png", 400, 300).LaunchFile();
