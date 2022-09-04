namespace ScottPlot;

public struct CoordinateSize
{
    public double Width;
    public double Height;
    public double Area => Width * Height;

    public CoordinateSize(double width, double height)
    {
        Width = width;
        Height = height;
    }
}
