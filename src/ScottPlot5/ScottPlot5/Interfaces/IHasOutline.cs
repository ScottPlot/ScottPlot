namespace ScottPlot;

public interface IHasOutline
{
    public LineStyle OutlineStyle { get; }

    public float OutlineWidth { get; set; }
    public LinePattern OutlinePattern { get; set; }
    public Color OutlineColor { get; set; }
}
