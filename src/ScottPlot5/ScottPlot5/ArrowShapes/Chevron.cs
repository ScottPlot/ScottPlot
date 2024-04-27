namespace ScottPlot.ArrowShapes;

public class Chevron : IArrowShape
{
    public void Render(RenderPack rp, PixelLine arrowLine, ArrowStyle arrowStyle)
    {
        float length = arrowLine.Length;

        rp.CanvasState.Save();
        rp.CanvasState.Translate(arrowLine.Pixel2);
        rp.CanvasState.RotateDegrees(arrowLine.AngleDegrees + 90);

        // origin is the tip, base extends to the right
        Pixel[] pixels = [
            new(0, 0),
            new(arrowStyle.ArrowheadLength, arrowStyle.ArrowheadWidth / 2),
            new(length, arrowStyle.ArrowheadWidth / 2),
            new(length - arrowStyle.ArrowheadLength, 0),
            new(length, -arrowStyle.ArrowheadWidth / 2),
            new(arrowStyle.ArrowheadLength, -arrowStyle.ArrowheadWidth / 2),
            new(0, 0),
        ];

        Drawing.DrawPath(rp.Canvas, rp.Paint, pixels, arrowStyle.FillStyle);
        Drawing.DrawPath(rp.Canvas, rp.Paint, pixels, arrowStyle.LineStyle);

        rp.CanvasState.Restore();
    }
}
