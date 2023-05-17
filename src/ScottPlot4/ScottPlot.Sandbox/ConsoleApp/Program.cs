using ScottPlot;

ScottPlot.Plot plt = new();
plt.BottomAxis.MinimumTickSpacing(0.5);
plt.BottomAxis.TickLabelFormat((d) => d % 1 == 0 ? d.ToString() : string.Empty);

plt.Launch.FormsPlot();
