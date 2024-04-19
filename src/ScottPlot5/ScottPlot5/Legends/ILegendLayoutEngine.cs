namespace ScottPlot.Legends;

public interface ILegendLayoutEngine
{
    LegendLayout GetLayout(Legend legend, PixelSize maxSize);
}
