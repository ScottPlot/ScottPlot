namespace ScottPlot;

public class MultiPlot
{
    // TODO: refactor the private classes inside this class then expose them

    // TODO: refactor functionality so we could add plots simply then arrange them on a grid later

    private class SubPlot(Plot plot, SubPlotRect rect)
    {
        public Plot Plot { get; set; } = plot;
        public SubPlotRect Rect { get; set; } = rect;
        public PixelRect GetPixelRect(int width, int height) => Rect.GetPixelRect(width, height);
        public static SubPlot FullSize(Plot plot) => new SubPlot(plot, SubPlotRect.FullSize);
    }

    private class SubPlotRect
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public bool Fractional { get; set; }

        public static SubPlotRect FullSize => new()
        {
            Width = 1,
            Height = 1,
            Left = 0,
            Top = 0,
            Fractional = true,
        };

        public PixelRect GetPixelRect(int width, int height)
        {
            if (!Fractional)
            {
                Pixel topLeft = new(Left, Top);
                PixelSize size = new(Width, Height);
                return new PixelRect(topLeft, size);
            }
            else
            {
                Pixel topLeft = new(Left * width, Top * height);
                PixelSize size = new(Width * width, Height * height);
                return new PixelRect(topLeft, size);
            }
        }
    }

    private class SizedPlot(Plot plot, PixelSize size)
    {
        public Plot? Plot { get; } = plot;
        public MultiPlot? MultiPlot { get; } = null;
        public PixelSize Size { get; } = size;
    }

    private List<SubPlot> SubPlots { get; } = [];

    public Plot[] Plots => SubPlots.Select(x => x.Plot).ToArray();

    public MultiPlot()
    {
    }

    public static MultiPlot WithGrid(Plot[,] plots)
    {
        MultiPlot mp = new();

        int rows = plots.GetLength(0);
        int cols = plots.GetLength(1);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                mp.AddSubplot(plots[y, x], y, rows, x, cols);
            }
        }

        return mp;
    }

    public static MultiPlot WithSinglePlot(Plot plot)
    {
        MultiPlot mp = new();
        SubPlot sp = SubPlot.FullSize(plot);
        mp.SubPlots.Add(sp);
        return mp;
    }

    public void AddSubplot(Plot plot, int rowIndex, int totalRows, int columnIndex, int totalColumns)
    {
        if (totalRows < 1)
            throw new ArgumentException($"{nameof(totalRows)} must be at least 1");

        if (totalColumns < 1)
            throw new ArgumentException($"{nameof(totalColumns)} must be at least 1");

        double colWidth = 1.0 / totalColumns;
        double colHeight = 1.0 / totalRows;

        SubPlotRect rect = new()
        {
            Width = colWidth,
            Height = colHeight,
            Left = colWidth * columnIndex,
            Top = colHeight * rowIndex,
            Fractional = true,
        };

        SubPlots.Add(new(plot, rect));
    }

    public Image Render(int width, int height)
    {
        SKImageInfo imageInfo = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        SKSurface surface = SKSurface.Create(imageInfo);

        foreach (SubPlot subplot in SubPlots)
        {
            PixelRect rect = subplot.GetPixelRect(width, height);
            subplot.Plot.RenderManager.ClearCanvasBeforeEachRender = false;
            subplot.Plot.Render(surface.Canvas, rect);
        }

        return new(surface);
    }

    public SavedImageInfo SavePng(string filename, int width = 800, int height = 600)
    {
        return Render(width, height).SavePng(filename);
    }
}
