namespace ScottPlot.Plottables;

public class Heatmap(double[,] intensities) : IPlottable, IHasColorAxis
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
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

                Color cellColor = Colormap.GetColor(Intensities[y, xIndex], range);

                if (AlphaMap is not null)
                    cellColor = cellColor.WithAlpha(AlphaMap[y, xIndex]);

                if (Opacity != 1)
                    cellColor = cellColor.WithAlpha((byte)(cellColor.Alpha * Opacity));

                argb[rowOffset + x] = cellColor.PremultipliedARGB;
            }
        }

        return argb;
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
    }
}
