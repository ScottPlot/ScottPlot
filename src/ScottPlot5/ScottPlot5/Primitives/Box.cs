namespace ScottPlot;

/// <summary>
/// Holds values for drawing a box-and-whisker symbol
/// </summary>
public class Box : IHasFill, IHasLine
{
    public double Position { get; set; }

    public double BoxMin { get; set; }
    public double BoxMax { get; set; }

    public double? BoxMiddle { get; set; }
    public double? WhiskerMin { get; set; }
    public double? WhiskerMax { get; set; }

    public double Width { get; set; } = 0.8;

    public double WhiskerSizeFraction { get; set; } = 0.5;
    public double WhiskerSize => Width * WhiskerSizeFraction;

    public Orientation Orientation { get; set; } = Orientation.Vertical;
    public bool IsVisible { get; set; } = true;


    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }


    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    [Obsolete("use FillColor or FillStyle")]
    public FillStyle Fill { get => FillStyle; set => FillStyle = value; }

    [Obsolete("use LineWidth, LineColor, or LineStyle")]
    public LineStyle Stroke { get => LineStyle; set => LineStyle = value; }

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
        if (!IsVisible)
            return;

        // TODO: support horizontal boxes
        if (Orientation != Orientation.Vertical)
            throw new NotImplementedException();

        // body fill
        CoordinateRect bodyRect = new(Position - Width / 2, Position + Width / 2, BoxMin, BoxMax);
        PixelRect bodyRectPx = axes.GetPixelRect(bodyRect);
        Drawing.FillRectangle(rp.Canvas, bodyRectPx, paint, FillStyle);

        // body stroke
        Drawing.DrawRectangle(rp.Canvas, bodyRectPx, paint, LineStyle);

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

            CoordinateLine lineMaxAcross = new(Position - WhiskerSize / 2, WhiskerMax.Value, Position + WhiskerSize / 2, WhiskerMax.Value);
            PixelLine lineMaxAcrossPx = axes.GetPixelLine(lineMaxAcross);
            Drawing.DrawLine(rp.Canvas, paint, lineMaxAcrossPx);
        }

        if (WhiskerMin.HasValue)
        {
            CoordinateLine lineMin = new(Position, BoxMin, Position, WhiskerMin.Value);
            PixelLine lineMinPx = axes.GetPixelLine(lineMin);
            Drawing.DrawLine(rp.Canvas, paint, lineMinPx);

            CoordinateLine lineMinAcross = new(Position - WhiskerSize / 2, WhiskerMin.Value, Position + WhiskerSize / 2, WhiskerMin.Value);
            PixelLine lineMinAcrossPx = axes.GetPixelLine(lineMinAcross);
            Drawing.DrawLine(rp.Canvas, paint, lineMinAcrossPx);
        }
    }
}
