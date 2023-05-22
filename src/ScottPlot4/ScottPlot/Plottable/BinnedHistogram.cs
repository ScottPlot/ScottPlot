using ScottPlot.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace ScottPlot.Plottable;

public class BinnedHistogram : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;

    /// <summary>
    /// Number of items in each histogram bin.
    /// Add samples by calling <see cref="Add(double, double, int)"/>.
    /// It is not recommended to edit this externally, but it is possible.
    /// </summary>
    public readonly int[,] Counts;

    /// <summary>
    /// Location of the lower left of the heatmap
    /// </summary>
    public Coordinate Origin { get; set; } = new(0, 0);

    /// <summary>
    /// Size of each cell of the heatmap (in coordinate units)
    /// </summary>
    public CoordinateSize CellSize { get; set; } = new(1, 1);

    /// <summary>
    /// Number of rows in the binned histogram
    /// </summary>
    public int Rows => Counts.GetLength(0);

    /// <summary>
    /// Number of columns in the binned histogram
    /// </summary>
    public int Columns => Counts.GetLength(1);

    /// <summary>
    /// Size of the scaled histogram (coordinate units)
    /// </summary>
    public CoordinateSize Size => new(CellSize.Width * Columns, CellSize.Height * Rows);

    /// <summary>
    /// Region occupied by the scaled histogram (coordinate units)
    /// </summary>
    public CoordinateRect Rectangle => new(Origin, Size);

    public ScottPlot.Drawing.Colormap _Colormap = Colormap.Turbo;

    /// <summary>
    /// Colormap used to color the histogram according to its count density
    /// </summary>
    public ScottPlot.Drawing.Colormap Colormap
    {
        get => _Colormap;
        set
        {
            _Colormap = value;
            if (Colorbar is not null)
            {
                Colorbar.UpdateColormap(value);
            }
        }
    }

    /// <summary>
    /// If set, this colorbar's tick labels will be updated on every render.
    /// </summary>
    public Colorbar Colorbar = null;

    /// <summary>
    /// If enabled, bins with no counts will be transparent.
    /// </summary>
    public bool TransparentZero { get; set; } = true;

    public BinnedHistogram(int columns, int rows)
    {
        Counts = new int[rows, columns];
    }

    public AxisLimits GetAxisLimits() => AxisLimits.FromRect(Rectangle);

    public LegendItem[] GetLegendItems() => LegendItem.None;

    public void ValidateData(bool deep = false) { }

    /// <summary>
    /// Return the value of the cell with the largest count
    /// </summary>
    private int GetMaxCount()
    {
        int maxCount = 0;

        for (int y = 0; y < Counts.GetLength(0); y++)
            for (int x = 0; x < Counts.GetLength(1); x++)
                maxCount = Math.Max(maxCount, Counts[y, x]);

        return maxCount;
    }

    public void Clear()
    {
        for (int y = 0; y < Counts.GetLength(0); y++)
        {
            for (int x = 0; x < Counts.GetLength(1); x++)
            {
                Counts[y, x] = 0;
            }
        }
    }

    public void Add(Coordinate coordinate, int count = 1)
    {
        Add(coordinate.X, coordinate.Y);
    }

    public void AddRange(IEnumerable<Coordinate> coordinates)
    {
        foreach (Coordinate coordinate in coordinates)
            Add(coordinate);
    }

    public void Add(double x, double y, int count = 1)
    {
        // TODO: scale and shift
        int binIndexX = (int)x;
        int binIndexY = (int)y;

        // TODO: flag to control this behavior
        if (y < 0 || y >= Rows)
            return;
        if (x < 0 || x >= Columns)
            return;

        Counts[binIndexY, binIndexX] += count;
    }

    public Bitmap GetHeatmapBitmap()
    {
        int maxCount = GetMaxCount();

        if (Colorbar is not null)
        {
            Colorbar.MaxValue = maxCount;
            Colorbar.TickLabelFormatter = position => $"{position:N0}";
        }

        int bytesPerPixel = 4;
        int stride = (Columns % 4 == 0) ? Columns : Columns + 4 - Columns % 4;
        byte[] bmpBytes = new byte[stride * Rows * bytesPerPixel];

        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                int bmpY = Rows - y - 1;
                int offset = (bmpY * stride + x) * bytesPerPixel;

                byte value = (byte)(255 * Counts[y, x] / maxCount);
                (byte r, byte g, byte b) = Colormap.GetRGB(value);
                byte a = (TransparentZero && value == 0) ? (byte)0 : (byte)255;
                bmpBytes[offset + 0] = b;
                bmpBytes[offset + 1] = g;
                bmpBytes[offset + 2] = r;
                bmpBytes[offset + 3] = a;
            }
        }

        PixelFormat formatOutput = PixelFormat.Format32bppPArgb;
        Rectangle rect = new(0, 0, Columns, Rows);
        Bitmap bmp = new(stride, Rows, formatOutput);
        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, formatOutput);
        Marshal.Copy(bmpBytes, 0, bmpData.Scan0, bmpBytes.Length);
        bmp.UnlockBits(bmpData);

        return bmp;
    }

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        // Convert the heatmap position from coordinate to pixel units
        float left = dims.GetPixelX(Origin.X);
        float right = dims.GetPixelX(Origin.X + Size.Width);
        float top = dims.GetPixelY(Origin.Y + Size.Height);
        float bottom = dims.GetPixelY(Origin.Y);
        float width = right - left;
        float height = bottom - top;
        RectangleF rect = new(left, top, width, height);

        // Draw the heatmap in the coordinate area
        using Graphics gfx = Drawing.GDI.Graphics(bmp, dims);
        gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        Bitmap heatmapBmp = GetHeatmapBitmap();
        gfx.DrawImage(heatmapBmp, rect);
    }
}
