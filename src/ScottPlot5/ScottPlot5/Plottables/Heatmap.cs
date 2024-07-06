using ScottPlot.Colormaps;

namespace ScottPlot.Plottables;

[Flags]
internal enum EdgeDirection
{
    None = 0,
    Up = 0b1,
    Right = 0b10,
    Down = 0b100,
    Left = 0b1000,
    All = Up | Right | Down | Left,
}

public class Heatmap(double[,] intensities) : IPlottable, IHasColorAxis
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public bool IsoMap { get; set; } = false;
    private IColormap _colormap { get; set; } = new Colormaps.Viridis();
    public IColormap Colormap
    {
        get { return _colormap; }
        set
        {
            _colormap = value;
            Update();
        }
    }

    /// <summary>
    /// Indicates position of the data point relative to the rectangle used to represent it.
    /// An alignment of upper right means the rectangle will appear to the lower left of the point itself.
    /// </summary>
    private Alignment _cellAlignment { get; set; } = Alignment.MiddleCenter;
    public Alignment CellAlignment
    {
        get { return _cellAlignment; }
        set
        {
            _cellAlignment = value;
            Update();
        }
    }

    /// <summary>
    /// If defined, the this rectangle sets the axis boundaries of heatmap data.
    /// Note that the actual heatmap area is 1 cell larger than this rectangle.
    /// </summary>
    private CoordinateRect? _extent;
    public CoordinateRect? Extent // TODO: obsolete this
    {
        get { return _extent; }
        set
        {
            _extent = value;
            Update();
        }
    }

    /// <summary>
    /// If defined, the this rectangle sets the axis boundaries of heatmap data.
    /// Note that the actual heatmap area is 1 cell larger than this rectangle.
    /// </summary>
    public CoordinateRect? Position { get => Extent; set => Extent = value; }

    /// <summary>
    /// This variable controls whether row 0 of the 2D source array is the top or bottom of the heatmap.
    /// When set to false (default), row 0 is the top of the heatmap.
    /// When set to true, row 0 of the source data will be displayed at the bottom.
    /// </summary>
    private bool _flipRows = false;
    public bool FlipVertically
    {
        get { return _flipRows; }
        set
        {
            _flipRows = value;
            Update();
        }
    }

    /// <summary>
    /// This variable controls whether the first sample in each column of the 2D source array is the left or right of the heatmap.
    /// When set to false (default), sample 0 is the left of the heatmap.
    /// When set to true, sample 0 of the source data will be displayed at the right.
    /// </summary>
    private bool _flipColumns = false;
    public bool FlipHorizontally
    {
        get { return _flipColumns; }
        set
        {
            _flipColumns = value;
            Update();
        }
    }

    public bool FlipColumns { get => FlipHorizontally; set => FlipHorizontally = value; }
    public bool FlipRows { get => FlipVertically; set => FlipVertically = value; }

    /// <summary>
    /// If true, pixels in the final image will be interpolated to give the heatmap a smooth appearance.
    /// If false, the heatmap will appear as individual rectangles with sharp edges.
    /// </summary>
    private bool _smooth { get; set; } = false;
    public bool Smooth
    {
        get { return _smooth; }
        set
        {
            _smooth = value;
            Update();
        }
    }

    /// <summary>
    /// Actual extent of the heatmap bitmap after alignment has been applied
    /// </summary>
    private CoordinateRect AlignedExtent
    {
        get
        {
            double xOffset = Math.Abs(CellWidth) * CellAlignment.HorizontalFraction();
            double yOffset = Math.Abs(CellHeight) * CellAlignment.VerticalFraction();
            Coordinates cellOffset = new(-xOffset, -yOffset);
            return ExtentOrDefault.WithTranslation(cellOffset);
        }
    }

    /// <summary>
    /// Extent used at render time.
    /// Supplies the user-provided extent if available, 
    /// otherwise a heatmap centered at the origin with cell size 1.
    /// </summary>
    private CoordinateRect ExtentOrDefault
    {
        get
        {
            if (Extent.HasValue)
            {
                var extent = Extent.Value;
                //user will provide the extends to the data. The image will be one cell wider and taller so we need to add that on (it is being added on in teh default case).
                double cellwidth = extent.Width / (Intensities.GetLength(1) - 1);
                double cellheight = extent.Height / (Intensities.GetLength(0) - 1);
                if (extent.Left < extent.Right) extent.Right += cellwidth;
                if (extent.Left > extent.Right) extent.Left -= cellwidth; //cellwidth will be negative if extent is flipped
                if (extent.Bottom < extent.Top) extent.Top += cellheight;
                if (extent.Bottom > extent.Top) extent.Bottom -= cellheight; //cellheight will be negative if extent is inverted

                return extent;
            }

            return new CoordinateRect(
                left: 0,
                right: Intensities.GetLength(1),
                bottom: 0,
                top: Intensities.GetLength(0));
        }
    }

    /// <summary>
    /// Width of a single cell from the heatmap (in coordinate units)
    /// </summary>
    public double CellWidth
    {
        get
        {
            return ExtentOrDefault.Width / Intensities.GetLength(1);
        }
        set
        {
            double left = ExtentOrDefault.Left;
            double right = ExtentOrDefault.Left + value * Intensities.GetLength(1);
            double bottom = ExtentOrDefault.Bottom;
            double top = ExtentOrDefault.Top;
            Extent = new(left, right, bottom, top);
        }
    }

    /// <summary>
    /// Height of a single cell from the heatmap (in coordinate units)
    /// </summary>
    public double CellHeight
    {
        get
        {
            return ExtentOrDefault.Height / Intensities.GetLength(0);
        }
        set
        {
            double left = ExtentOrDefault.Left;
            double right = ExtentOrDefault.Right;
            double bottom = ExtentOrDefault.Bottom;
            double top = ExtentOrDefault.Bottom + value * Intensities.GetLength(0);
            Extent = new(left, right, bottom, top);
        }
    }

    /// <summary>
    /// This object holds data values for the heatmap.
    /// After editing contents users must call <see cref="Update"/> before changes
    /// appear on the heatmap.
    /// </summary>
    public readonly double[,] Intensities = intensities;

    /// <summary>
    /// Defines what color will be used to fill cells containing NaN.
    /// </summary>
    public Color NaNCellColor
    {
        get => _NaNCellColor;
        set
        {
            _NaNCellColor = value;
            Update();
        }
    }
    private Color _NaNCellColor = Colors.Transparent;

    private byte[,]? _alphaMap;

    /// <summary>
    /// If present, this array defines transparency for each cell in the heatmap.
    /// Values range from 0 (transparent) through 255 (opaque).
    /// </summary>
    public byte[,]? AlphaMap
    {
        get => _alphaMap;
        set
        {
            if (value?.GetLength(0) != Height)
                throw new Exception("AlphaMap height must match the height of the Intensity map.");

            if (value?.GetLength(1) != Width)
                throw new Exception("AlphaMap width must match the width of the Intensity map.");

            _alphaMap = value;

            Update();
        }
    }

    private double _Opacity = 1;

    /// <summary>
    /// Controls the opacity of the entire heatmap from 0 (transparent) to 1 (opaque)
    /// </summary>
    public double Opacity
    {
        get => _Opacity;
        set
        {
            _Opacity = NumericConversion.Clamp(value, 0, 1);
            Update();
        }
    }

    /// <summary>
    /// Height of the heatmap data (rows)
    /// </summary>
    int Height => Intensities.GetLength(0);

    /// <summary>
    /// Width of the heatmap data (columns)
    /// </summary>
    int Width => Intensities.GetLength(1);

    /// <summary>
    /// Generated and stored when <see cref="Update"/> is called
    /// </summary>
    private SKBitmap? Bitmap = null;
    private List<LinkedList<(int i, int j)>>? EdgePaths = null;

    ~Heatmap()
    {
        Bitmap?.Dispose();
    }

    /// <summary>
    /// Return heatmap as an array of ARGB values,
    /// scaled according to the heatmap setting,
    /// and in the order necessary to create a bitmap.
    /// </summary>
    private uint[] GetArgbValues()
    {
        // TODO: Allow setting number of buckets (and possibly their size, it's often useful to highlight a narrow range)
        var quantizedColormap = IsoMap ? new QuantizedColormap(Colormap, 3) : Colormap;

        Range range = GetRange();
        uint[] argb = new uint[Intensities.Length];

        // the XOR here disables flipping when the flip property and the extent is inverted.
        bool FlipY = FlipVertically ^ ExtentOrDefault.IsInvertedY;
        bool FlipX = FlipHorizontally ^ ExtentOrDefault.IsInvertedX;

        uint nanCellArgb = NaNCellColor.PremultipliedARGB;

        for (int y = 0; y < Height; y++)
        {
            int rowOffset = FlipY ? (Height - 1 - y) * Width : y * Width;
            for (int x = 0; x < Width; x++)
            {
                int xIndex = FlipX ? (Width - 1 - x) : x;

                // Make NaN cells transparent
                if (double.IsNaN(Intensities[y, xIndex]))
                {
                    argb[rowOffset + x] = nanCellArgb;
                    continue;
                }

                Color cellColor = quantizedColormap.GetColor(Intensities[y, xIndex], range);

                if (AlphaMap is not null)
                    cellColor = cellColor.WithAlpha(AlphaMap[y, xIndex]);

                if (Opacity != 1)
                    cellColor = cellColor.WithAlpha((byte)(cellColor.Alpha * Opacity));

                argb[rowOffset + x] = cellColor.PremultipliedARGB;
            }
        }

        return argb;
    }

    private EdgeDirection[,] GetEdgePoints(uint[] argbs)
    {
        var differsFrom = new EdgeDirection[Height, Width];

        if (!IsoMap)
            return differsFrom;

        // We assume here that each bitmap pixel has one and only one intensity value (i.e. the image is not scaled)
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                var self = argbs[i * Width + j];

                var leftIndex = i * Width + j - 1;
                var rightIndex = i * Width + j + 1;
                var upIndex = (i - 1) * Width + j;
                var downIndex = (i + 1) * Width + j;

                var left = leftIndex >= 0 && j > 0 ? argbs[leftIndex] : self;
                var right = rightIndex < argbs.Length && j + 1 < Width ? argbs[rightIndex] : self;
                var up = upIndex >= 0 ? argbs[upIndex] : self;
                var down = downIndex < argbs.Length ? argbs[downIndex] : self;

                // Because this is only intended to be used for the isomap we can do simplistic edge detection
                // If we ever extend it we likely need something like Canny detection

                var edgeDirection = EdgeDirection.None;
                if (self != left)
                    edgeDirection |= EdgeDirection.Left;

                if (self != right)
                    edgeDirection |= EdgeDirection.Right;

                if (self != up)
                    edgeDirection |= EdgeDirection.Up;

                if (self != down)
                    edgeDirection |= EdgeDirection.Down;


                differsFrom[i, j] = edgeDirection;
            }
        }

        var edgeDirections = new EdgeDirection[Height, Width];

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (differsFrom[i, j] == EdgeDirection.None)
                    continue;

                if ((differsFrom[i, j] & EdgeDirection.Left) != EdgeDirection.None)
                {
                    // i.e. the edge continues above this cell either in the same direction, or 90 degrees different
                    if (i > 0 && (differsFrom[i - 1, j] & (EdgeDirection.All ^ EdgeDirection.Down)) != EdgeDirection.None)
                        edgeDirections[i, j] |= EdgeDirection.Up;

                    if (i + 1 < Height && (differsFrom[i + 1, j] & (EdgeDirection.All ^ EdgeDirection.Up)) != EdgeDirection.None)
                        edgeDirections[i, j] |= EdgeDirection.Down;
                }

                //if (false && (differsFrom[i, j] & EdgeDirection.Right) != EdgeDirection.None)
                //{
                //    if (i > 0 && (differsFrom[i - 1, j] & EdgeDirection.Right) != EdgeDirection.None)
                //        edgeDirections[i, j] |= EdgeDirection.Up;

                //    if (i + 1 < Height && (differsFrom[i + 1, j] & EdgeDirection.Right) != EdgeDirection.None)
                //        edgeDirections[i, j] |= EdgeDirection.Down;
                //}

                if ((differsFrom[i, j] & EdgeDirection.Up) != EdgeDirection.None)
                {
                    if (j > 0 && (differsFrom[i, j - 1] & (EdgeDirection.All ^ EdgeDirection.Right)) != EdgeDirection.None)
                        edgeDirections[i, j] |= EdgeDirection.Left;

                    if (j + 1 < Width && (differsFrom[i, j + 1] & (EdgeDirection.All ^ EdgeDirection.Left)) != EdgeDirection.None)
                        edgeDirections[i, j] |= EdgeDirection.Right;
                }

                //if (false && (differsFrom[i, j] & EdgeDirection.Down) != EdgeDirection.None)
                //{
                //    if (j > 0 && (differsFrom[i, j - 1] & EdgeDirection.Down) != EdgeDirection.None)
                //        edgeDirections[i, j] |= EdgeDirection.Left;

                //    if (j + 1 < Width && (differsFrom[i, j + 1] & EdgeDirection.Down) != EdgeDirection.None)
                //        edgeDirections[i, j] |= EdgeDirection.Right;
                //}
            }
        }

        return edgeDirections;
    }

    // Prefers upper neighbour and makes its way clockwise
    private (int i, int j)? GetNeighbour(EdgeDirection[,] edges, (int i, int j) coords)
    {
        (int i, int j) = coords;
        var directions = EdgeDirection.None;
        try
        {
            directions = edges[i, j];
        }
        catch (Exception ex)
        {
            return null; // lol fix this shit
        }

        if ((directions & EdgeDirection.Up) != EdgeDirection.None)
            return (i - 1, j);

        if ((directions & EdgeDirection.Down) != EdgeDirection.None)
            return (i + 1, j);

        if ((directions & EdgeDirection.Left) != EdgeDirection.None)
            return (i, j - 1);

        if ((directions & EdgeDirection.Right) != EdgeDirection.None)
            return (i, j + 1);

        return null;
    }

    private void AddEdgesWithoutBackTracking(EdgeDirection[,] edges, LinkedList<(int i, int j)> path)
    {
        (int i, int j)? neighbour = null;
        while (path.Last is not null
            && (neighbour = GetNeighbour(edges, path.Last.Value)).HasValue) {

            var loop = path.Contains(neighbour.Value);

            path.AddLast(neighbour.Value);

            if (loop)
                break;
        }
    }

    private List<LinkedList<(int i, int j)>> GetEdgePaths(uint[] argbs)
    {
        var edges = GetEdgePoints(argbs);

        var paths = new List<LinkedList<(int i, int j)>>();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (edges[i, j] != EdgeDirection.None)
                {
                    if (paths.Find(ll => ll.Contains((i, j))) is not null)
                         continue;

                    var path = new LinkedList<(int i, int j)>([(i, j)]);
                    AddEdgesWithoutBackTracking(edges, path);

                    paths.Add(path);
                }
            }
        }

        return paths;
    }

    /// <summary>
    /// Regenerate the image using the present settings and data in <see cref="Intensities"/>
    /// </summary>
    public void Update()
    {
        DataRange = Range.GetRange(Intensities);
        uint[] argbs = GetArgbValues();
        Bitmap?.Dispose();
        Bitmap = Drawing.BitmapFromArgbs(argbs, Width, Height);

        if (IsoMap)
            EdgePaths = GetEdgePaths(argbs);
    }

    public AxisLimits GetAxisLimits()
    {
        return new(AlignedExtent);
    }

    /// <summary>
    /// Return the position in the array beneath the given point
    /// </summary>
    public (int x, int y) GetIndexes(Coordinates coordinates)
    {
        CoordinateRect rect = AlignedExtent;

        double distanceFromLeft = coordinates.X - rect.Left;
        int xIndex = (int)(distanceFromLeft / CellWidth);

        double distanceFromTop = rect.Top - coordinates.Y;
        int yIndex = (int)(distanceFromTop / CellHeight);

        return (xIndex, yIndex);
    }

    public Coordinates GetCoordinates(int x, int y)
    {
        CoordinateRect rect = AlignedExtent;

        double xCoord = rect.Left + x * CellWidth;
        double yCood = rect.Top - y * CellHeight;

        return new(xCoord, yCood);
    }

    /// <summary>
    /// Return the value of the cell beneath the given point.
    /// Returns NaN if the point is outside the heatmap area.
    /// </summary>
    public double GetValue(Coordinates coordinates)
    {
        CoordinateRect rect = AlignedExtent;

        if (!rect.Contains(coordinates))
            return double.NaN;

        (int xIndex, int yIndex) = GetIndexes(coordinates);

        return Intensities[yIndex, xIndex];
    }

    public IEnumerable<LegendItem> LegendItems => Enumerable.Empty<LegendItem>();

    public Range GetRange() => ManualRange ?? DataRange;

    /// <summary>
    /// Range of values spanned by the data the last time it was updated
    /// </summary>
    public Range DataRange { get; private set; }


    private Range? _ManualRange;

    /// <summary>
    /// If supplied, the colormap will span this range of values
    /// </summary>
    public Range? ManualRange
    {
        get => _ManualRange;
        set
        {
            _ManualRange = value;
            Update();
        }
    }

    public virtual void Render(RenderPack rp)
    {
        if (Bitmap is null)
            Update(); // automatically generate the bitmap on first render if it was not generated manually

        using SKPaint paint = new()
        {
            FilterQuality = Smooth ? SKFilterQuality.High : SKFilterQuality.None
        };

        SKRect rect = Axes.GetPixelRect(AlignedExtent).ToSKRect();

        rp.Canvas.DrawBitmap(Bitmap, rect, paint);

        var argbs = GetArgbValues();

        var edges = GetEdgePoints(argbs);
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (edges[i, j] != EdgeDirection.None)
                {
                    var pt = Axes.GetPixel(GetCoordinates(j, i)).ToSKPoint();
                    //rp.Canvas.DrawCircle(pt, 3, paint);

                }
            }
        }


        if (IsoMap && EdgePaths is not null)
        {
            LineStyle style = new LineStyle() { Width = 5 };
            style.ApplyToPaint(paint);

            foreach (var pathPoints in EdgePaths)
            {
                using SKPath path = new();
                foreach (var (i, j) in pathPoints)
                {
                    var pt = Axes.GetPixel(GetCoordinates(j, i)).ToSKPoint();
                    //System.Diagnostics.Debug.WriteLine($"({j}, {i}), {GetCoordinates(j, i)}");

                    if (path.PointCount == 0)
                        path.MoveTo(pt);
                    else
                        path.LineTo(pt);
                }

                rp.Canvas.DrawPath(path, paint);
            }
        }
    }
}
