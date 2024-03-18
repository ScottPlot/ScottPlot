namespace ScottPlot.Plottables;

public class Heatmap : IPlottable, IHasColorAxis
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
    public CoordinateRect? Extent
    {
        get { return _extent; }
        set
        {
            _extent = value;
            Update();
        }
    }

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
    private double CellWidth => ExtentOrDefault.Width / Intensities.GetLength(1);

    /// <summary>
    /// Height of a single cell from the heatmap (in coordinate units)
    /// </summary>
    private double CellHeight => ExtentOrDefault.Height / Intensities.GetLength(0);

    /// <summary>
    /// This object holds data values for the heatmap.
    /// After editing contents users must call <see cref="Update"/> before changes
    /// appear on the heatmap.
    /// </summary>
    public readonly double[,] Intensities;

    private byte[,] _alphaMap;
    public byte[,] AlphaMap
    {
        get { return _alphaMap; }
        set
        {
            if (value.GetLength(0) != Height) throw new Exception("AlphaMap height must match the height of the Intensity map.");
            if (value.GetLength(1) != Width) throw new Exception("AlphaMap width must match the width of the Intensity map.");
            _alphaMap = value;
            Update();
        }
    }
    private byte _globalAlpha;
    public byte GlobalAlpha
    {
        get { return _globalAlpha; }
        set
        {
            _globalAlpha = value;
            var UpdatedAlphaMap = new byte[Height, Width];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    UpdatedAlphaMap[i, j] = value;
                }
            }
            AlphaMap = UpdatedAlphaMap;
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

    public Heatmap(double[,] intensities)
    {
        Intensities = intensities;
        // Create the Alpha array with the same size as Intensities
        _alphaMap = new byte[Height, Width];
        GlobalAlpha = (byte)255;
    }

    public Heatmap(double[,] intensities, byte globalAlpha)
    {
        Intensities = intensities;
        // Create the Alpha array with the same size as Intensities
        _alphaMap = new byte[Height, Width];
        GlobalAlpha = globalAlpha;
    }


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

        uint transparentBlack = 0x00000000;

        for (int y = 0; y < Height; y++)
        {
            int rowOffset = FlipY ? (Height - 1 - y) * Width : y * Width;
            for (int x = 0; x < Width; x++)
            {
                int xIndex = FlipX ? (Width - 1 - x) : x;
                if (Double.IsNaN(Intensities[y, xIndex]))
                {
                    argb[rowOffset + x] = transparentBlack;
                    continue;
                }
                var colorWithoutAlpha = Colormap.GetColor(Intensities[y, xIndex], range).ARGB;
                var alpha = AlphaMap[y, xIndex];
                // Extract the RGB components
                byte Red = (byte)((colorWithoutAlpha >> 16) & 0xFF);
                byte Green = (byte)((colorWithoutAlpha >> 8) & 0xFF);
                byte Blue = (byte)(colorWithoutAlpha & 0xFF);

                // Calculate premultiplied RGB components
                byte premultipliedRed = (byte)((Red * alpha) / 255);
                byte premultipliedGreen = (byte)((Green * alpha) / 255);
                byte premultipliedBlue = (byte)((Blue * alpha) / 255);
                argb[rowOffset + x] = ((uint)alpha << 24) | ((uint)premultipliedRed << 16) | ((uint)premultipliedGreen << 8) | premultipliedBlue;
            }
        }

        return argb;
    }

    /// <summary>
    /// Regenerate the image using the present settings and data in <see cref="Intensities"/>
    /// </summary>
    public void Update()
    {
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

    public Range GetRange() => Range.GetRange(Intensities);

    public void Render(RenderPack rp)
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
