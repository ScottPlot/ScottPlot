namespace ScottPlot;

public interface ILegendLayout
{
    LegendLayout GetLayout(Legend legend, PixelSize maxSize);
}
