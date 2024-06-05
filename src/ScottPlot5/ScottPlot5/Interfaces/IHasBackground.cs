namespace ScottPlot;

public interface IHasBackground
{
    FillStyle BackgroundFillStyle { get; }
    Color BackgroundColor { get; set; }
    Color BackgroundHatchColor { get; set; }
    IHatch? BackgroundHatch { get; set; }
}
