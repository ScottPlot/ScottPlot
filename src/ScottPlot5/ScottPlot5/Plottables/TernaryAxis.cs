namespace ScottPlot.Plottables;

/// <summary>
/// A ternary axis uses triangular coordinates to describe a ternary coordinate system
/// where points are represented by three components A, B, and C that sum to a constant (usually 1 or 100).
/// This class draws a ternary axis and has options to customize edges and points.
/// </summary>
public class TernaryAxis : IPlottable, IManagesAxisLimits
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    /// <summary>
    /// Edges of the ternary plot triangle
    /// </summary>
    public List<TernaryAxisEdge> Edges { get; } = new();

    /// <summary>
    /// Corner labels of the ternary plot triangle
    /// </summary>
    public List<TernaryAxisCornerLabel> CornerLabels { get; } = new();

    /// <summary>
    /// Points to plot on the ternary diagram
    /// </summary>
    public List<Coordinates> Points { get; } = new();

    /// <summary>
    /// Enable this to modify the axis limits at render time to achieve "square axes"
    /// where the units/px values are equal for horizontal and vertical axes, allowing
    /// the triangle to appear equilateral.
    /// </summary>
    public bool ManageAxisLimits { get; set; } = true;

    // Minor grid styling properties
    public Color MinorGridColor { get; set; } = Colors.Gray;
    public float MinorGridThickness { get; set; } = 1.0f;
    public float MinorGridAlpha { get; set; } = 0.5f;  // Transparency (0.0 to 1.0)

    // Background color property
    public Color BackgroundColor { get; set; } = Colors.LightYellow;
    public float BackgroundAlpha { get; set; } = 0.3f;

    /// <summary>
    /// Initialize the ternary axis with default edges, corner labels, and tick marks
    /// </summary>
    public TernaryAxis()
    {
        // Define the corners of the triangle
        Coordinates cornerA = new(0, 0);
        Coordinates cornerB = new(1, 0);
        Coordinates cornerC = new(0.5, Math.Sqrt(3) / 2);

        // Define the edges of the triangle
        Edges.Add(new TernaryAxisEdge(cornerA, cornerB));
        Edges.Add(new TernaryAxisEdge(cornerB, cornerC));
        Edges.Add(new TernaryAxisEdge(cornerC, cornerA));

        // Define the labels at each corner
        CornerLabels.Add(new TernaryAxisCornerLabel(cornerA, "A"));
        CornerLabels.Add(new TernaryAxisCornerLabel(cornerB, "B"));
        CornerLabels.Add(new TernaryAxisCornerLabel(cornerC, "C"));
    }

    /// <summary>
    /// Converts ternary coordinates (A, B, C) to Cartesian coordinates (X, Y)
    /// </summary>
    public Coordinates GetCoordinates(double A, double B, double C)
    {
        if (Math.Abs(A + B + C - 1) > 1e-6)
            throw new ArgumentException("Components A, B, and C must sum to 1");

        double x = 0.5 * (2 * B + C);
        double y = (Math.Sqrt(3) / 2) * C;
        return new Coordinates(x, y);
    }

    /// <summary>
    /// Adds a point to the ternary plot
    /// </summary>
    public void AddPoint(double A, double B, double C)
    {
        Coordinates point = GetCoordinates(A, B, C);
        Points.Add(point);
    }

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(0, 1, 0, Math.Sqrt(3) / 2);
    }

    public virtual void UpdateAxisLimits(Plot plot)
    {
        if (!ManageAxisLimits)
            return;

        foreach (IAxisRule rule in plot.Axes.Rules)
        {
            if (rule is AxisRules.SquareZoomOut)
                return;
        }

        AxisRules.SquareZoomOut squareRule = new(Axes.XAxis, Axes.YAxis);
        plot.Axes.Rules.Add(squareRule);
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        RenderEdges(rp, paint);
        RenderBackground(rp, paint); // Draw the background color first
        RenderCornerLabels(rp, paint);
        RenderTicks(rp, paint); // Render tick marks along edges
        RenderPoints(rp, paint);
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
        using (SKPath path = new SKPath())
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
            label.LabelStyle.Alignment = label.Alignment;
            label.LabelStyle.FontSize = label.TextSize;
            label.LabelStyle.ForeColor = label.TextColor;
            label.LabelStyle.Bold = label.IsBold;

            // Calculate label position with offsets
            Pixel labelPixel = Axes.GetPixel(label.Position).WithOffset(label.OffsetX, label.OffsetY);

            // Render using LabelStyle
            label.LabelStyle.Render(rp.Canvas, labelPixel, paint);
        }
    }

    private void RenderPoints(RenderPack rp, SKPaint paint)
    {
        // Set up paint properties for points
        paint.Color = SKColors.Black; // Set point color (change as needed)
        paint.IsAntialias = true;
        paint.Style = SKPaintStyle.Fill;

        foreach (var point in Points)
        {
            Pixel px = Axes.GetPixel(point);
            rp.Canvas.DrawCircle(px.X, px.Y, 5, paint); // Adjust point size as needed
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

    private SKColor ToSKColor(Color color, float alpha)
    {
        return new SKColor(color.R, color.G, color.B, (byte)(alpha * 255));
    }
    private SKColor ToSKColor(Color color)
    {
        return new SKColor(color.R, color.G, color.B, color.A); // Alpha is now directly part of `color`
    }

    private void RenderGridLines(RenderPack rp, SKPaint paint, int tickCount)
    {
        paint.Color = ToSKColor(MinorGridColor, MinorGridAlpha); // Use MinorGridColor and MinorGridAlpha for grid lines
        paint.StrokeWidth = MinorGridThickness;

        Coordinates cornerA = Edges[0].Start;
        Coordinates cornerB = Edges[1].Start;
        Coordinates cornerC = Edges[2].Start;

        for (int i = 1; i < tickCount; i++)
        {
            double fractionA = i / (double)tickCount;
            double fractionB = (tickCount - i) / (double)tickCount;

            Coordinates tickAC = new Coordinates(
                cornerA.X + fractionA * (cornerC.X - cornerA.X),
                cornerA.Y + fractionA * (cornerC.Y - cornerA.Y)
            );

            Coordinates tickAB = new Coordinates(
                cornerA.X + fractionA * (cornerB.X - cornerA.X),
                cornerA.Y + fractionA * (cornerB.Y - cornerA.Y)
            );

            Coordinates tickBC = new Coordinates(
                cornerB.X + fractionA * (cornerC.X - cornerB.X),
                cornerB.Y + fractionA * (cornerC.Y - cornerB.Y)
            );

            Coordinates tickCA_B = new Coordinates(
                cornerC.X + fractionB * (cornerA.X - cornerC.X),
                cornerC.Y + fractionB * (cornerA.Y - cornerC.Y)
            );

            Coordinates tickCB_A = new Coordinates(
                cornerC.X + fractionB * (cornerB.X - cornerC.X),
                cornerC.Y + fractionB * (cornerB.Y - cornerC.Y)
            );

            Coordinates tickCB_C = new Coordinates(
                cornerC.X + fractionA * (cornerB.X - cornerC.X),
                cornerC.Y + fractionA * (cornerB.Y - cornerC.Y)
            );

            rp.Canvas.DrawLine(Axes.GetPixel(tickAC).X, Axes.GetPixel(tickAC).Y, Axes.GetPixel(tickAB).X, Axes.GetPixel(tickAB).Y, paint);
            rp.Canvas.DrawLine(Axes.GetPixel(tickAC).X, Axes.GetPixel(tickAC).Y, Axes.GetPixel(tickBC).X, Axes.GetPixel(tickBC).Y, paint);
            rp.Canvas.DrawLine(Axes.GetPixel(tickCA_B).X, Axes.GetPixel(tickCA_B).Y, Axes.GetPixel(tickCB_A).X, Axes.GetPixel(tickCB_A).Y, paint);
            rp.Canvas.DrawLine(Axes.GetPixel(tickCB_C).X, Axes.GetPixel(tickCB_C).Y, Axes.GetPixel(tickAB).X, Axes.GetPixel(tickAB).Y, paint);
        }
    }

    private void RenderTicksOnEdge(RenderPack rp, SKPaint tickPaint, TernaryAxisEdge edge, int tickCount, EdgeType edgeType)
    {
        // Configure tick line paint separately
        tickPaint.Color = ToSKColor(edge.TickLineColor);
        tickPaint.StrokeWidth = edge.TickLineThickness;
        tickPaint.IsAntialias = true;

        // Create a separate paint for labels to avoid interference with tick lines
        using var labelPaint = new SKPaint
        {
            IsAntialias = true,
            TextSize = edge.TickLabelStyle.FontSize,
            Color = ToSKColor(edge.TickLabelStyle.ForeColor),
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


    public enum EdgeType
    {
        Bottom,
        Right,
        Left
    }
}
