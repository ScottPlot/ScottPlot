namespace ScottPlot.Plottables.Interactive;

/// <summary>
/// An infinite line defined by two points that extends infinitely in both directions
/// </summary>
public class InteractiveTrendLine : IPlottable, IHasInteractiveHandles
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public Cursor Cursor { get; set; } = Cursor.Hand;

    private MutableCoordinateLine MutableLine { get; } = new();
    public CoordinateLine Line
    {
        get => MutableLine.CoordinateLine;
        set => MutableLine.Update(value);
    }

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => MutableLine.AxisLimits;

    public LineStyle LineStyle { get; } = new LineStyle(2);

    public MarkerStyle StartMarkerStyle { get; } = new(MarkerShape.FilledCircle, 8);
    public MarkerStyle EndMarkerStyle { get; } = new(MarkerShape.FilledCircle, 8);

    enum Node { Point1, Point2 };

    public InteractiveHandle? GetHandle(CoordinateRect rect)
    {
        // First check if the mouse is directly over one of the control points
        if (rect.Contains(MutableLine.Point1))
            return new InteractiveHandle(this, Cursor, (int)Node.Point1);

        if (rect.Contains(MutableLine.Point2))
            return new InteractiveHandle(this, Cursor, (int)Node.Point2);

        // Check if the infinite line passes through the rect. This allows the user to manipulate the line without
        //   having to see one of the 2 points on the screen - fairly necessary due to being an infinite line
        if (LineIntersectsRect(rect))
        {
            // Determine which control point is closer to the mouse and return that point
            Coordinates mousePos = rect.Center;
            double dist1 = mousePos.Distance(MutableLine.Point1);
            double dist2 = mousePos.Distance(MutableLine.Point2);

            return dist1 < dist2
                ? new InteractiveHandle(this, Cursor, (int)Node.Point1)
                : new InteractiveHandle(this, Cursor, (int)Node.Point2);
        }

        return null;
    }

    /// <summary>
    /// Check if the infinite line passes through or near the given rectangle
    /// </summary>
    private bool LineIntersectsRect(CoordinateRect rect)
    {
        CoordinateLine line = MutableLine.CoordinateLine;

        // Handle vertical line case
        var dx = line.X2 - line.X1;
        if (Math.Abs(dx) < 1e-10)
            return rect.ContainsX(line.X1);

        // Handle horizontal line case
        var dy = line.Y2 - line.Y1;
        if (Math.Abs(dy) < 1e-10)
            return rect.ContainsY(line.Y1);

        // General case: check if line intersects the rectangle
        // Calculate Y values at left and right edges of rect
        var slope = dy / dx;
        var yIntercept = line.Y1 - slope * line.X1;
        var yAtLeft = slope * rect.Left + yIntercept;
        var yAtRight = slope * rect.Right + yIntercept;

        // Check if either Y value is within the rect's Y range
        if (rect.ContainsY(yAtLeft) || rect.ContainsY(yAtRight))
            return true;

        // Also check if the line crosses through the rect vertically
        // (i.e., one Y is above and one is below)
        if ((yAtLeft < rect.Bottom && yAtRight > rect.Top) ||
            (yAtLeft > rect.Top && yAtRight < rect.Bottom))
            return true;

        return false;
    }

    public Color Color
    {
        set
        {
            LineStyle.Color = value;
            StartMarkerStyle.FillColor = value;
            EndMarkerStyle.FillColor = value;
        }
    }

    public virtual void PressHandle(InteractiveHandle handle) { }
    public virtual void ReleaseHandle(InteractiveHandle handle) { }
    public virtual void MoveHandle(InteractiveHandle handle, Coordinates point)
    {
        if (handle.Index == (int)Node.Point1)
            MutableLine.Point1 = point;

        if (handle.Index == (int)Node.Point2)
            MutableLine.Point2 = point;
    }

    public virtual void Render(RenderPack rp)
    {
        if (IsVisible == false)
            return;

        PixelLine pxLine = Axes.GetPixelLine(MutableLine.CoordinateLine);

        // Calculate the slope and extend the line to the edges of the data area
        PixelLine extendedLine = ExtendLineToEdges(pxLine, rp.DataRect);

        LineStyle.Render(rp.Canvas, extendedLine, rp.Paint);
        StartMarkerStyle.Render(rp.Canvas, pxLine.Pixel1, rp.Paint);
        EndMarkerStyle.Render(rp.Canvas, pxLine.Pixel2, rp.Paint);
    }

    private PixelLine ExtendLineToEdges(PixelLine line, PixelRect dataRect)
    {
        float dx = line.X2 - line.X1;
        float dy = line.Y2 - line.Y1;

        // Handle vertical line case
        if (Math.Abs(dx) < 0.001)
            return new PixelLine(line.X1, dataRect.Top, line.X1, dataRect.Bottom);

        // Handle horizontal line case
        if (Math.Abs(dy) < 0.001)
            return new PixelLine(dataRect.Left, line.Y1, dataRect.Right, line.Y1);

        // Calculate slope
        float slope = dy / dx;

        // Calculate y-intercept: y = mx + b -> b = y - mx
        float yIntercept = line.Y1 - slope * line.X1;

        // Calculate intersections with the data rect edges
        float leftY = slope * dataRect.Left + yIntercept;
        float rightY = slope * dataRect.Right + yIntercept;
        float topX = (dataRect.Top - yIntercept) / slope;
        float bottomX = (dataRect.Bottom - yIntercept) / slope;

        // Find which two edges the line intersects
        List<Pixel> intersections = new();

        if (leftY >= dataRect.Top && leftY <= dataRect.Bottom)
            intersections.Add(new Pixel(dataRect.Left, leftY));

        if (rightY >= dataRect.Top && rightY <= dataRect.Bottom)
            intersections.Add(new Pixel(dataRect.Right, rightY));

        if (topX >= dataRect.Left && topX <= dataRect.Right)
            intersections.Add(new Pixel(topX, dataRect.Top));

        if (bottomX >= dataRect.Left && bottomX <= dataRect.Right)
            intersections.Add(new Pixel(bottomX, dataRect.Bottom));

        // Return line between first two intersections
        if (intersections.Count >= 2)
            return new PixelLine(intersections[0], intersections[1]);

        // Fallback to original line if something went wrong
        return line;
    }
}
