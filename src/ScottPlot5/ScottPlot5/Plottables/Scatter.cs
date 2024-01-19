/* Minimal case scatter plot for testing only.
 * Avoid temptation to use generics or generic math at this early stage of development!
 */

using System.Drawing;

namespace ScottPlot.Plottables;

public class Scatter : IPlottable
{
    public string? Label { get; set; }
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public LineStyle LineStyle { get; set; } = new(); // TODO: hide this
    public MarkerStyle MarkerStyle { get; set; } = MarkerStyle.Default; // TODO: hide this

    public IScatterSource Data { get; }

    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }


    /// <summary>
    /// If enabled, scatter plot points will be connected by square corners rather than straight diagnal lines
    /// </summary>
    public bool StepDisplay { get; set; } = false;
    /// <summary>
    /// Describes orientation of steps if <see cref="StepDisplay"/> is enabled.
    /// If true, lines will extend to the right before ascending or descending to the level of the following point.
    /// </summary>
    public bool StepDisplayRight { get; set; } = false;
    /// <summary>
    /// If enabled, points will be connected by smooth lines instead of straight diagnal lines.
    /// <see cref="SmoothTension"/> adjusts the smoothnes of the lines.
    /// </summary>
    public bool Smooth = false;
    /// <summary>
    /// Tension to use for smoothing when <see cref="Smooth"/> is enabled
    /// </summary>
    public double SmoothTension = 0.5;

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.Fill.Color = value;
            MarkerStyle.Outline.Color = value;
        }
    }

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public IEnumerable<LegendItem> LegendItems => EnumerableExtensions.One<LegendItem>(
        new LegendItem
        {
            Label = Label,
            Marker = MarkerStyle,
            Line = LineStyle,
        });

    public Scatter(IScatterSource data)
    {
        Data = data;
    }

    public void Render(RenderPack rp)
    {
        // TODO: can this be more effecient by moving this logic into the DataSource to avoid copying?
        Pixel[] pixels = Data.GetScatterPoints().Select(Axes.GetPixel).ToArray();

        if (!pixels.Any())
            return;

        using SKPaint paint = new();

        if (StepDisplay)
        {
            IEnumerable<Pixel> stepPixels = GetStepDisplayPixels(pixels, StepDisplayRight);
            Drawing.DrawLines(rp.Canvas, paint, stepPixels, LineStyle);
        }
        else if (Smooth)
        {
            // TODO: Implement SmoothTension
            Drawing.DrawLines(rp.Canvas, paint, pixels, LineStyle);
        }
        else
        {
            Drawing.DrawLines(rp.Canvas, paint, pixels, LineStyle);
        }
        Drawing.DrawMarkers(rp.Canvas, paint, pixels, MarkerStyle);
    }

    /// <summary>
    /// Convert scatter plot points (connected by diagnal lines) to step plot points (connected by right angles)
    /// by inserting an extra point between each of the original data points to result in L-shaped steps.
    /// </summary>
    /// <param name="points">Array of corner positions</param>
    /// <param name="right">Indicates that a line will extend to the right before rising or falling.</param>
    public static IEnumerable<Pixel> GetStepDisplayPixels(Pixel[] pixels, bool right)
    {
        Pixel[] pixelsStep = new Pixel[pixels.Count() * 2 - 1];

        int offsetX = right ? 1 : 0;
        int offsetY = right ? 0 : 1;

        for (int i = 0; i < pixels.Count() - 1; i++)
        {
            pixelsStep[i * 2] = pixels[i];
            pixelsStep[i * 2 + 1] = new Pixel(pixels[i + offsetX].X, pixels[i + offsetY].Y);
        }

        pixelsStep[pixelsStep.Length - 1] = pixels[pixels.Length - 1];

        return pixelsStep;
    }
}
