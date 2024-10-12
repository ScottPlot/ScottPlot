namespace ScottPlot.Plottables;

public class ContourLines : IPlottable, IHasLine
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    private AxisLimits AxisLimits = AxisLimits.NoLimits;
    public AxisLimits GetAxisLimits() => AxisLimits;

    public List<ContourLine> Lines { get; } = [];

    public LineStyle LineStyle { get; set; } = new() { Width = 1, Color = Colors.Black };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public IColormap? Colormap { get; set; } = null;
    public double MinZ { get; private set; }
    public double MaxZ { get; private set; }

    /// <summary>
    /// If defined, contour lines will be drawn at this height and <see cref="ContourLineCount"/> will be ignored.
    /// </summary>
    public int ContourLineCount = 10;

    /// <summary>
    /// If defined, contour lines will be drawn at this height and <see cref="ContourLineCount"/> will be ignored.
    /// </summary>
    public double[]? ContourLineLevels = null;

    /// <summary>
    /// Update contour lines from arbitrarily placed data points.
    /// </summary>
    public void Update(IEnumerable<Coordinates3d> coordinates)
    {
        Triangulation.TriangulatedIrregularNetwork tin = new(coordinates);
        MinZ = tin.MinZ;
        MaxZ = tin.MaxZ;
        double[] zs = ContourLineLevels ?? tin.GetZsByCount(ContourLineCount);
        Lines.Clear();
        Lines.AddRange(tin.GetContourLines(zs));
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
        MinZ = tin.MinZ;
        MaxZ = tin.MaxZ;
        double[] zs = ContourLineLevels ?? tin.GetZsByCount(ContourLineCount);
        Lines.Clear();
        Lines.AddRange(tin.GetContourLines(zs));
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
            foreach (var point in line.Path.Points)
            {
                limits.Expand(point);
            }
        }

        AxisLimits = limits.AxisLimits;
    }

    public virtual void Render(RenderPack rp)
    {
        if (IsVisible == false || Lines is null || Lines.Count == 0)
            return;

        using SKPaint paint = new();

        for (int i = 0; i < Lines.Count; i++)
        {
            PixelPath path = Axes.GetPixelPath(Lines[i].Path);

            if (Colormap is not null)
            {
                double fraction = (Lines[i].Z - MinZ) / (MaxZ - MinZ);
                LineStyle.Color = Colormap.GetColor(fraction);
            }

            Drawing.DrawPath(rp.Canvas, paint, path, LineStyle);
        }
    }
}
