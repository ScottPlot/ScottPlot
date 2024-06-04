namespace ScottPlot;

public interface IHasMarker
{
    public MarkerStyle MarkerStyle { get; set; }
    public MarkerShape MarkerShape { get; set; }
    public float MarkerSize { get; set; }
    public Color MarkerFillColor { get; set; }
    public Color MarkerLineColor { get; set; }
    public float MarkerLineWidth { get; set; }
    public Color MarkerColor { get; set; }
}
