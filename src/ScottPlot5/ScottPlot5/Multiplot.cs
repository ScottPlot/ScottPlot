namespace ScottPlot;

public class Multiplot
{
    public List<Plot> Plots { get; } = [];
    public List<FractionRect> Rects { get; } = [];

    public Multiplot()
    {

    }

    public Multiplot(IEnumerable<Plot> plots)
    {

    }

    public void AddPlot(Plot plot) => Plots.Add(plot);

    public Plot AddPlot()
    {
        Plot plot = new();
        Plots.Add(plot);
        return plot;
    }

    // TODO: pass in layout as a class?

    public void LayoutRows()
    {
        Rects.Clear();
        for (int i = 0; i < Plots.Count; i++)
        {
            FractionRect rect = FractionRect.Row(i, Plots.Count);
            Rects.Add(rect);
        }
    }

    public void LayoutColumns()
    {
        Rects.Clear();
        for (int i = 0; i < Plots.Count; i++)
        {
            FractionRect rect = FractionRect.Column(i, Plots.Count);
            Rects.Add(rect);
        }
    }

    public void LayoutGrid(int cols, int rows)
    {
        Rects.Clear();
        if (rows * cols < Plots.Count)
        {
            throw new ArgumentException($"{nameof(rows)} x {nameof(cols)} must equal length of {nameof(Plots)}");
        }

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                FractionRect rect = FractionRect.GridCell(x, y, rows, cols);
                Rects.Add(rect);
            }
        }
    }

    public Image Render(int width, int height)
    {
        SKImageInfo imageInfo = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        SKSurface surface = SKSurface.Create(imageInfo);

        for (int i = 0; i < Plots.Count; i++)
        {
            Plot plot = Plots[i];
            PixelRect rect = Rects[i].GetPixelRect(width, height);
            plot.RenderManager.ClearCanvasBeforeEachRender = false;
            plot.Render(surface.Canvas, rect);
        }

        return new(surface);
    }

    public SavedImageInfo SavePng(string filename, int width = 800, int height = 600)
    {
        return Render(width, height).SavePng(filename);
    }
}
