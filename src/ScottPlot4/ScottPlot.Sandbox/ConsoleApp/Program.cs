using ScottPlot.WinForms; // required for the FormsPlot launcher

ScottPlot.Plot plt = new();
plt.AddSignal(ScottPlot.Generate.Sin());
plt.AddSignal(ScottPlot.Generate.Cos());

// save the plot using a temporary filename and launch it with the system default file handler
plt.Launch.ImageFile();

// open the plot in a mouse-interactive window using Windows Forms
plt.Launch.FormsPlot();

// open the plot in a web browser (displayed as a static image)
plt.Launch.ImageHTML();
