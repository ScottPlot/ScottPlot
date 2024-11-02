namespace ScottPlot.Plottables;

/// <summary>
/// This plot type places a triangular axis on the plot 
/// and has methods to convert between triangular and Cartesian coordinates.
/// </summary>
public class TriangularAxis : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    /// <summary>
    /// Fractional amount of padding between the edge of the triangle and the data area.
    /// Increase this value to make room for large tick and corner labels.
    /// </summary>
    public double PaddingFraction { get; set; } = 0.25;

    public AxisLimits GetAxisLimits() => new AxisLimits(0, 1, 0, Math.Sqrt(3) / 2)
        .WithZoom(1 - PaddingFraction, 1 - PaddingFraction);

    public TriangularAxisEdge[] Edges { get; }

    public TriangularAxisCornerLabel[] CornerLabels { get; }

    // Minor grid styling properties
    public Color MinorGridColor { get; set; } = Colors.Gray;
    public float MinorGridThickness { get; set; } = 1.0f;
    public float MinorGridAlpha { get; set; } = 0.1f;

    // Background color property
    public Color BackgroundColor { get; set; } = Colors.LightYellow;
    public float BackgroundAlpha { get; set; } = 0.3f;

    public Coordinates LeftCorner = new(0, 0);
    public Coordinates RightCorner = new(1, 0);
    public Coordinates TopCorner = new(0.5, Math.Sqrt(3) / 2);

    // TODO: replace this enum with three coordinate lines
    public enum EdgeType { Bottom, Right, Left, };

    public TriangularAxis()
    {
        Edges = [
            new TriangularAxisEdge(LeftCorner, RightCorner),
            new TriangularAxisEdge(RightCorner, TopCorner),
            new TriangularAxisEdge(TopCorner, LeftCorner),
        ];

        CornerLabels = [
            new TriangularAxisCornerLabel(LeftCorner, string.Empty),
            new TriangularAxisCornerLabel(RightCorner, string.Empty),
            new TriangularAxisCornerLabel(TopCorner, string.Empty),
        ];
    }

    /// <summary>
    /// Converts triangular coordinates (A, B, C) to Cartesian coordinates (X, Y)
    /// </summary>
    public Coordinates GetCoordinates(double bottomFraction, double leftFraction, double rightFraction)
    {
        double x = bottomFraction;
        double y = ((1 - leftFraction) + rightFraction) / 2 * TopCorner.Y;
        return new Coordinates(x, y);
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        RenderEdges(rp, paint);
        RenderBackground(rp, paint); // Draw the background color first
        RenderCornerLabels(rp, paint);
        RenderTicks(rp, paint); // Render tick marks along edges
    }

    private void RenderBackground(RenderPack rp, SKPaint paint)
    {
        // Define the coordinates for the triangle corners
        Pixel pixelA = Axes.GetPixel(Edges[0].Start);
        Pixel pixelB = Axes.GetPixel(Edges[1].Start);
        Pixel pixelC = Axes.GetPixel(Edges[2].Start);

        // Set paint properties for background color
        paint.Color = new SKColor(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, (byte)(BackgroundAlpha * 255));
        paint.IsAntialias = true;
        paint.Style = SKPaintStyle.Fill;

        // Draw the triangle
        using (SKPath path = new())
        {
            path.MoveTo(pixelA.X, pixelA.Y);
            path.LineTo(pixelB.X, pixelB.Y);
            path.LineTo(pixelC.X, pixelC.Y);
            path.Close();
            rp.Canvas.DrawPath(path, paint);
        }
    }

    private void RenderEdges(RenderPack rp, SKPaint paint)
    {
        foreach (var edge in Edges)
        {
            Drawing.DrawLine(rp.Canvas, paint, Axes.GetPixel(edge.Start), Axes.GetPixel(edge.End), edge.LineStyle);
        }
    }

    private void RenderCornerLabels(RenderPack rp, SKPaint paint)
    {
        foreach (var label in CornerLabels)
        {
            if (string.IsNullOrEmpty(label.LabelText))
                continue; // Skip if no text

            // Assign text settings from LabelStyle
            label.LabelStyle.Text = label.LabelText ?? string.Empty;
            label.LabelStyle.Alignment = Alignment.MiddleCenter;

            // Calculate label position with offsets
            Pixel labelPixel = Axes.GetPixel(label.Position).WithOffset(0, 0);

            // Render using LabelStyle
            label.LabelStyle.Render(rp.Canvas, labelPixel, paint);
        }
    }

    private void RenderTicks(RenderPack rp, SKPaint paint)
    {
        // Set up paint properties for the ticks
        paint.Color = SKColors.Black;
        paint.StrokeWidth = 2;
        paint.IsAntialias = true;

        // Render ticks on each edge
        RenderTicksOnEdge(rp, paint, Edges[0], 10, EdgeType.Bottom); // A-B edge
        RenderTicksOnEdge(rp, paint, Edges[1], 10, EdgeType.Right);  // B-C edge
        RenderTicksOnEdge(rp, paint, Edges[2], 10, EdgeType.Left);   // C-A edge
    }

    private void RenderGridLines(RenderPack rp, SKPaint paint, int tickCount)
    {
        paint.Color = MinorGridColor.WithAlpha(MinorGridAlpha).ToSKColor();
        paint.StrokeWidth = MinorGridThickness;

        Coordinates cornerA = Edges[0].Start;
        Coordinates cornerB = Edges[1].Start;
        Coordinates cornerC = Edges[2].Start;

        for (int i = 1; i < tickCount; i++)
        {
            double fractionA = i / (double)tickCount;
            double fractionB = (tickCount - i) / (double)tickCount;

            Coordinates tickAC = new(
                cornerA.X + fractionA * (cornerC.X - cornerA.X),
                cornerA.Y + fractionA * (cornerC.Y - cornerA.Y)
            );

            Coordinates tickAB = new(
                cornerA.X + fractionA * (cornerB.X - cornerA.X),
                cornerA.Y + fractionA * (cornerB.Y - cornerA.Y)
            );

            Coordinates tickBC = new(
                cornerB.X + fractionA * (cornerC.X - cornerB.X),
                cornerB.Y + fractionA * (cornerC.Y - cornerB.Y)
            );

            Coordinates tickCA_B = new(
                cornerC.X + fractionB * (cornerA.X - cornerC.X),
                cornerC.Y + fractionB * (cornerA.Y - cornerC.Y)
            );

            Coordinates tickCB_A = new(
                cornerC.X + fractionB * (cornerB.X - cornerC.X),
                cornerC.Y + fractionB * (cornerB.Y - cornerC.Y)
            );

            Coordinates tickCB_C = new(
                cornerC.X + fractionA * (cornerB.X - cornerC.X),
                cornerC.Y + fractionA * (cornerB.Y - cornerC.Y)
            );

            rp.Canvas.DrawLine(Axes.GetPixel(tickAC).X, Axes.GetPixel(tickAC).Y, Axes.GetPixel(tickAB).X, Axes.GetPixel(tickAB).Y, paint);
            rp.Canvas.DrawLine(Axes.GetPixel(tickAC).X, Axes.GetPixel(tickAC).Y, Axes.GetPixel(tickBC).X, Axes.GetPixel(tickBC).Y, paint);
            rp.Canvas.DrawLine(Axes.GetPixel(tickCA_B).X, Axes.GetPixel(tickCA_B).Y, Axes.GetPixel(tickCB_A).X, Axes.GetPixel(tickCB_A).Y, paint);
            rp.Canvas.DrawLine(Axes.GetPixel(tickCB_C).X, Axes.GetPixel(tickCB_C).Y, Axes.GetPixel(tickAB).X, Axes.GetPixel(tickAB).Y, paint);
        }
    }

    private void RenderTicksOnEdge(RenderPack rp, SKPaint tickPaint, TriangularAxisEdge edge, int tickCount, EdgeType edgeType)
    {
        // Configure tick line paint separately
        tickPaint.Color = edge.TickLineColor.ToSKColor();
        tickPaint.StrokeWidth = edge.TickLineThickness;
        tickPaint.IsAntialias = true;

        // Create a separate paint for labels to avoid interference with tick lines
        using SKPaint labelPaint = new()
        {
            IsAntialias = true,
            TextSize = edge.TickLabelStyle.FontSize,
            Color = edge.TickLabelStyle.ForeColor.ToSKColor(),
            Typeface = edge.TickLabelStyle.Bold ? SKTypeface.FromFamilyName(null, SKFontStyle.Bold) : SKTypeface.FromFamilyName(null, SKFontStyle.Normal),
        };

        for (int i = 0; i <= tickCount; i++)
        {
            double fraction = i / (double)tickCount;
            int labelValue = i * 10;

            double tickX = edge.Start.X + fraction * (edge.End.X - edge.Start.X);
            double tickY = edge.Start.Y + fraction * (edge.End.Y - edge.Start.Y);
            var tickPosition = new Coordinates(tickX, tickY);

            // Calculate perpendicular direction for tick positioning
            double dx = edge.End.Y - edge.Start.Y;
            double dy = edge.Start.X - edge.End.X;
            double length = Math.Sqrt(dx * dx + dy * dy);
            dx /= length;
            dy /= length;

            Pixel tickStart, tickEnd, labelPosition;

            // Determine tick start, end, and label positions based on edge type
            switch (edgeType)
            {
                case EdgeType.Bottom:
                    tickStart = Axes.GetPixel(tickPosition).WithOffset(0, 0);
                    tickEnd = Axes.GetPixel(tickPosition).WithOffset(0, 10);
                    labelPosition = Axes.GetPixel(tickPosition).WithOffset(0, 20);
                    edge.TickLabelStyle.Alignment = Alignment.MiddleCenter;
                    break;

                case EdgeType.Right:
                    tickStart = Axes.GetPixel(tickPosition).WithOffset((float)(0 * dx), (float)(0 * dy));
                    tickEnd = Axes.GetPixel(tickPosition).WithOffset((float)(10 * dx), (float)(0 * dy));
                    labelPosition = Axes.GetPixel(tickPosition).WithOffset((float)(12 * dx), (float)(12 * dy));
                    edge.TickLabelStyle.Alignment = Alignment.LowerLeft;
                    break;

                case EdgeType.Left:
                    tickStart = Axes.GetPixel(tickPosition).WithOffset((float)(0 * dx), (float)(0 * dy));
                    tickEnd = Axes.GetPixel(tickPosition).WithOffset((float)(10 * dx), (float)(0 * dy));
                    labelPosition = Axes.GetPixel(tickPosition).WithOffset((float)(12 * dx), (float)(12 * dy));
                    edge.TickLabelStyle.Alignment = Alignment.LowerRight;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(edgeType), "Invalid edge type specified");
            }

            // Draw tick line with tickPaint
            rp.Canvas.DrawLine(tickStart.X, tickStart.Y, tickEnd.X, tickEnd.Y, tickPaint);

            // Render tick label with separate labelPaint
            edge.TickLabelStyle.Text = labelValue.ToString();
            edge.TickLabelStyle.Render(rp.Canvas, labelPosition, labelPaint);
        }

        // Render any additional grid lines if required
        RenderGridLines(rp, tickPaint, tickCount);
    }
}
