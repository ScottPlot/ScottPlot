using ScottPlot.Extensions;

namespace ScottPlot;

/// <summary>
/// Holds values for drawing a box-and-whisker symbol
/// </summary>
public class Box
{
    public double Position { get; set; }

    public double BoxMin { get; set; }
    public double BoxMax { get; set; }

    public double? BoxMiddle { get; set; }
    public double? WhiskerMin { get; set; }
    public double? WhiskerMax { get; set; }

    public double Width { get; set; } = 0.8;
    public double CapSize { get; set; } = 0.3;
    public FillStyle Fill { get; set; } = new();
    public LineStyle Stroke { get; set; } = new();
    public Orientation Orientation { get; set; } = Orientation.Vertical;

    public AxisLimits GetAxisLimits()
    {
        double xMin = Position - Width / 2;
        double xMax = Position + Width / 2;
        double yMin = WhiskerMin.HasValue ? Math.Min(BoxMin, WhiskerMin.Value) : BoxMin;
        double yMax = WhiskerMax.HasValue ? Math.Max(BoxMin, WhiskerMax.Value) : BoxMax;
        return new AxisLimits(xMin, xMax, yMin, yMax);
    }

    public void Render(RenderPack rp, SKPaint paint, IAxes axes)
    {
        // TODO: support horizontal boxes
        if (Orientation != Orientation.Vertical)
            throw new NotImplementedException();

        // body fill
        CoordinateRect bodyRect = new(Position - Width / 2, Position + Width / 2, BoxMin, BoxMax);
        PixelRect bodyRectPx = axes.GetPixelRect(bodyRect);
        Fill.ApplyToPaint(paint, bodyRectPx);
        Drawing.FillRectangle(rp.Canvas, bodyRectPx, paint);

        // body stroke
        Stroke.ApplyToPaint(paint);
        Drawing.DrawRectangle(rp.Canvas, bodyRectPx, paint);

        if (BoxMiddle.HasValue)
        {
            CoordinateLine lineMid = new(bodyRect.Left, BoxMiddle.Value, bodyRect.Right, BoxMiddle.Value);
            PixelLine lineMidPx = axes.GetPixelLine(lineMid);
            Drawing.DrawLine(rp.Canvas, paint, lineMidPx);
        }

        if (WhiskerMax.HasValue)
        {
            CoordinateLine lineMax = new(Position, BoxMax, Position, WhiskerMax.Value);
            PixelLine lineMaxPx = axes.GetPixelLine(lineMax);
            Drawing.DrawLine(rp.Canvas, paint, lineMaxPx);

            CoordinateLine lineMaxAcross = new(Position - CapSize / 2, WhiskerMax.Value, Position + CapSize / 2, WhiskerMax.Value);
            PixelLine lineMaxAcrossPx = axes.GetPixelLine(lineMaxAcross);
            Drawing.DrawLine(rp.Canvas, paint, lineMaxAcrossPx);
        }

        if (WhiskerMin.HasValue)
        {
            CoordinateLine lineMin = new(Position, BoxMin, Position, WhiskerMin.Value);
            PixelLine lineMinPx = axes.GetPixelLine(lineMin);
            Drawing.DrawLine(rp.Canvas, paint, lineMinPx);

            CoordinateLine lineMinAcross = new(Position - CapSize / 2, WhiskerMin.Value, Position + CapSize / 2, WhiskerMin.Value);
            PixelLine lineMinAcrossPx = axes.GetPixelLine(lineMinAcross);
            Drawing.DrawLine(rp.Canvas, paint, lineMinAcrossPx);
        }
    }
}
