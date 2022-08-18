/* Minimal case signal plot for testing only
 * 
 * !! Avoid temptation to use generics or generic math at this early stage of development
 * 
 */

using SkiaSharp;

namespace ScottPlot.Plottables;

public class Signal : PlottableBase
{
    public readonly DataSource.ISignalSource Data;

    public Color Color = new(0, 0, 255);
    public float LineWidth = 1;

    public Signal(DataSource.ISignalSource data)
    {
        Data = data;
    }

    public override AxisLimits GetAxisLimits()
    {
        return Data.GetLimits();
    }

    public override void Render(SKSurface surface, PixelRect dataRect)
    {
        PixelRangeY[] verticalBars = new PixelRangeY[(int)dataRect.Width];

        double xUnitsPerPixel = XAxis!.Width / dataRect.Width;

        // for each vertical column of pixels in the data area
        for (int i = 0; i < verticalBars.Length; i++)
        {
            // determine how wide this column of pixels is in coordinate units
            float xPixel = i + dataRect.Left;
            double colX1 = XAxis!.GetCoordinate(xPixel, dataRect);
            double colX2 = colX1 + xUnitsPerPixel;
            CoordinateRange xRange = new(colX1, colX2);

            // determine how much vertical space the data of this pixel column occupies
            CoordinateRange yRange = Data.GetYRange(xRange);
            float yMin = YAxis!.GetPixel(yRange.Min, dataRect);
            float yMax = YAxis!.GetPixel(yRange.Max, dataRect);
            verticalBars[i] = new PixelRangeY(yMin, yMax);
        }

        surface.Canvas.ClipRect(dataRect.ToSKRect());

        SKPaint paint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            Color = Color.ToSKColor(),
            StrokeWidth = LineWidth,
        };

        for (int i = 0; i < verticalBars.Length; i++)
        {
            float x = dataRect.Left + i;
            surface.Canvas.DrawLine(x, verticalBars[i].Bottom, x, verticalBars[i].Top, paint);
        }
    }
}
