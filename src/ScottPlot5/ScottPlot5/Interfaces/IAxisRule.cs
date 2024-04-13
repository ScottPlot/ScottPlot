namespace ScottPlot;

/// <summary>
/// These rules are applied just before each render
/// </summary>
public interface IAxisRule
{
    void Apply(RenderPack rp, bool beforeLayout);
}
