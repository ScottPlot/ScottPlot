using ScottPlot;

Plot plt = new();
plt.Add.Signal(Generate.Sin()).LineStyle.HandDrawn = true;
plt.Add.Signal(Generate.Cos()).LineStyle.HandDrawn = true;

foreach (var axis in plt.Axes.GetAxes())
{
    axis.FrameLineStyle.HandDrawn = true;
}

plt.Axes.DefaultGrid.XAxisStyle.MajorLineStyle.HandDrawn = true;
plt.Axes.DefaultGrid.XAxisStyle.MinorLineStyle.HandDrawn = true;
plt.Axes.DefaultGrid.YAxisStyle.MajorLineStyle.HandDrawn = true;
plt.Axes.DefaultGrid.YAxisStyle.MajorLineStyle.HandDrawn = true;


plt.SavePng("test.png", 600, 300).LaunchFile();
plt.SavePng("test.png", 600, 300).LaunchInBrowser();
plt.SavePng("test.png", 600, 300).ConsoleWritePath();
