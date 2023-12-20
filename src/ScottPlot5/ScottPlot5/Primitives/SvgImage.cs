namespace ScottPlot;

public class SvgImage
{
    public readonly int Width;
    public readonly int Height;
    private readonly MemoryStream Stream;
    public readonly SKCanvas Canvas;

    public SvgImage(int width, int height)
    {
        Width = width;
        Height = height;
        SKRect rect = new(0, 0, width, height);
        Stream = new MemoryStream();
        Canvas = SKSvgCanvas.Create(rect, Stream);
    }

    public void Save(string filename)
    {
        FileStream fs = new(filename, FileMode.Create);
        Stream.WriteTo(fs);
        using StreamWriter sw = new(fs);
        sw.Write("</svg>");
    }
}
