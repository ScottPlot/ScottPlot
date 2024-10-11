
namespace ScottPlot.Plottables;

public class ContourLines : IPlottable, IHasLine
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    private AxisLimits AxisLimits = AxisLimits.NoLimits;
    public AxisLimits GetAxisLimits() => AxisLimits;

    public CoordinatePath[]? Lines { get; private set; } = null;

    public LineStyle LineStyle { get; set; } = new() { Width = 1, Color = Colors.Black };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    /// <summary>
    /// Update contour lines from arbitrarily placed data points.
    /// </summary>
    public void Update(IEnumerable<Coordinates3d> coordinates)
    {
        Triangulation.TriangulatedIrregularNetwork tin = new(coordinates);
        Lines = tin.GetContourLines(.25).Where(x => x.Points.Length > 1).ToArray();
        UpdateAxisLimits();
    }

    /// <summary>
    /// Update contour lines from a rectangular grid of coordinates
    /// </summary>
    public void Update(Coordinates3d[,] coordinateGrid)
    {
        // TODO: use bilinear filtering directly on the rectangles that make up the heatmap.
        // The present implementation assumes an irregular network and is not best for grids.

        int count = coordinateGrid.GetLength(0) * coordinateGrid.GetLength(1);
        Coordinates3d[] coordinates = new Coordinates3d[count];

        int i = 0;
        for (int y = 0; y < coordinateGrid.GetLength(0); y++)
        {
            for (int x = 0; x < coordinateGrid.GetLength(1); x++)
            {
                coordinates[i++] = coordinateGrid[y, x];
            }
        }

        Triangulation.TriangulatedIrregularNetwork tin = new(coordinates);
        Lines = tin.GetContourLines(.25).Where(x => x.Points.Length > 1).ToArray();
        UpdateAxisLimits();
    }

    private void UpdateAxisLimits()
    {
        if (Lines is null)
        {
            AxisLimits = AxisLimits.NoLimits;
            return;
        }

        ExpandingAxisLimits limits = new();
        foreach (var line in Lines)
        {
            foreach (var point in line.Points)
            {
                limits.Expand(point);
            }
        }

        AxisLimits = limits.AxisLimits;
    }

    public void Render(RenderPack rp)
    {
        if (IsVisible == false || Lines is null || Lines.Length == 0)
            return;

        using SkiaSharp.SKPaint paint = new();

        foreach (PixelPath path in Axes.GetPixelPaths(Lines))
        {
            Drawing.DrawPath(rp.Canvas, paint, path, LineStyle);
        }
    }
}
