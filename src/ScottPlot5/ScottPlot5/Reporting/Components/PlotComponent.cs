namespace ScottPlot.Reporting.Components;

public record PlotComponent(Plot Plot, int Width = 600, int Height = 400) : IComponent;

