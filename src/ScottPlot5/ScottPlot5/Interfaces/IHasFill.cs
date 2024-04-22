namespace ScottPlot;

public interface IHasFill
{
    FillStyle FillStyle { get; set; }
    Color FillColor { get; set; }
    Color FillHatchColor { get; set; }
    IHatch? FillHatch { get; set; }
}
