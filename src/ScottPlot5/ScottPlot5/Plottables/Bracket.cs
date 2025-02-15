
using System.Numerics;

namespace ScottPlot.Plottables;

public class Bracket : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public Coordinates Point1 { get; set; }

    public Coordinates Point2 { get; set; }

    /// <summary>
    /// Controls whether the tip of the bracket is counter-clockwise from the line formed by the bracket base.
    /// </summary>
    public bool LabelCounterClockwise { get; set; } = false;

    /// <summary>
    /// Size of the small lines (in pixels) placed the edges of the bracket and between the center of the bracket and the label
    /// </summary>
    public float EdgeLength = 5;

    /// <summary>
    /// Text displayed near the center of the bracket
    /// </summary>
    public string Text { get; set; } = string.Empty;

    public LineStyle LineStyle { get; set; } = new() { IsVisible = true, Width = 1 };
    public LabelStyle LabelStyle { get; set; } = new() { IsVisible = true, Alignment = Alignment.LowerCenter };

    public AxisLimits GetAxisLimits() => new([Point1, Point2]);

    public virtual void Render(RenderPack rp)
    {
        double x1 = Point1.X;
        double y1 = Point1.Y;
        double x2 = Point2.X;
        double y2 = Point2.Y;

        double pxPerUnitX = rp.DataRect.Width / Axes.XAxis.Width;
        double pxPerUnitY = rp.DataRect.Height / Axes.YAxis.Height;

        var v = new Vector2((float)(x2 - x1), (float)(y2 - y1));
        var vPixel = new Vector2((float)(v.X * pxPerUnitX), (float)(v.Y * pxPerUnitY));
        var vDirectionVector = Vector2.Normalize(vPixel);

        if (v.X < 0 || (v.X == 0 && v.Y < 0)) // To prevent switching the order of the points from changing label position
        {
            vDirectionVector = Vector2.Negate(vDirectionVector);
        }

        Vector2 normal = Vector2.Normalize(new(vPixel.Y, vPixel.X));
        Vector2 antiNormal = Vector2.Negate(normal);

        static double AngleBetweenVectors(Vector2 reference, Vector2 v)
        {
            reference = Vector2.Normalize(reference);
            v = Vector2.Normalize(v);
            return Math.Acos(Vector2.Dot(reference, v));
        }

        var clockwiseNormalVector = AngleBetweenVectors(vDirectionVector, normal) > 0 ? normal : antiNormal;
        var counterClockwiseNormalVector = normal == clockwiseNormalVector ? antiNormal : normal;

        var edgeVector = LabelCounterClockwise ? counterClockwiseNormalVector : clockwiseNormalVector;

        var globalTranslation = edgeVector * EdgeLength;
        rp.CanvasState.Translate(new(globalTranslation.X, globalTranslation.Y));

        var pxStart1 = Axes.GetPixel(new(x1, y1));
        var pxEnd1 = Axes.GetPixel(new(x2, y2));
        var bracketHeadTranslation = edgeVector * EdgeLength;
        var pxStart2 = pxStart1.WithOffset(bracketHeadTranslation.X, bracketHeadTranslation.Y);
        var pxEnd2 = pxEnd1.WithOffset(bracketHeadTranslation.X, bracketHeadTranslation.Y);
        PixelLine mainLine = new(pxStart1.X, pxStart1.Y, pxStart2.X, pxStart2.Y);
        PixelLine edgeLine1 = new(pxEnd1.X, pxEnd1.Y, pxEnd2.X, pxEnd2.Y);
        PixelLine edgeLine2 = new(pxStart2.X, pxStart2.Y, pxEnd2.X, pxEnd2.Y);

        using SKPaint paint = new();
        Drawing.DrawLine(rp.Canvas, paint, mainLine, LineStyle);
        Drawing.DrawLine(rp.Canvas, paint, edgeLine1, LineStyle);
        Drawing.DrawLine(rp.Canvas, paint, edgeLine2, LineStyle);

        if (string.IsNullOrWhiteSpace(Text))
            return;

        // draw the "stub" line between center of bracket and center of base of label
        var halfVector = new Vector2((float)x1, (float)y1) + 0.5f * v;
        Pixel stubPixel1 = Axes.GetPixel(new(halfVector.X, halfVector.Y)).WithOffset(bracketHeadTranslation.X, bracketHeadTranslation.Y);
        Pixel stubPixel2 = stubPixel1.WithOffset(bracketHeadTranslation.X, bracketHeadTranslation.Y);
        PixelLine stubLine = new(stubPixel1, stubPixel2);

        Drawing.DrawLine(rp.Canvas, paint, stubLine, LineStyle);

        // draw label text aligned with the end of the stub
        rp.CanvasState.Translate(stubPixel2);

        // rotate as needed to keep the label upright
        var angle = (float)(-Math.Atan2(v.Y * pxPerUnitY, v.X * pxPerUnitX) * 180 / Math.PI);
        if (angle < 0)
            angle += 360;
        bool flippedText = false;
        if (angle > 90 && angle < 270)
        {
            flippedText = true;
            angle -= 180;
        }
        rp.CanvasState.RotateDegrees(angle);

        // translate as needed to adjust for flipped text
        bool IsInverted = edgeVector == antiNormal;
        var labelHeight = LabelStyle.Measure(Text).Height;
        var translateY = labelHeight * ((IsInverted && !flippedText || !IsInverted && flippedText) ? 0 : 1);
        rp.CanvasState.Translate(new(0, translateY));

        LabelStyle.Render(rp.Canvas, Pixel.Zero, paint, Text);
    }
}
