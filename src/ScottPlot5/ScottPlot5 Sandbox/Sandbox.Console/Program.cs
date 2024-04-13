using ScottPlot;

Plot plt = new();
plt.Add.Signal(Generate.Sin());
plt.Add.Signal(Generate.Cos());

plt.SavePng("test.png", 600, 300).LaunchFile();
plt.SavePng("test.png", 600, 300).LaunchInBrowser();
