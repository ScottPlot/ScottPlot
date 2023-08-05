namespace ScottPlot;

public interface ILegend
{
    bool IsVisible { get; set; }
    void Render(RenderPack rp);
}
