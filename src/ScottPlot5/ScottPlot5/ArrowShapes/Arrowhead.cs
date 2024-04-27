namespace ScottPlot.ArrowShapes;

public class Arrowhead : IArrowShape
{
    public void Render(RenderPack rp, PixelLine arrowLine, ArrowStyle arrowStyle)
    {
        rp.CanvasState.Save();
        rp.CanvasState.Translate(arrowLine.Pixel2);
        rp.CanvasState.RotateDegrees(arrowLine.AngleDegrees + 90);

        // origin is the tip, base extends to the right
        Pixel[] pixels = [
            new(0, 0),
            new(arrowStyle.ArrowheadLength, arrowStyle.ArrowheadWidth / 2),
            new(arrowStyle.ArrowheadAxisLength, 0),
            new(arrowStyle.ArrowheadAxisLength, 0),
            new(arrowStyle.ArrowheadLength, -arrowStyle.ArrowheadWidth / 2),
            new(0, 0),
        ];

        Drawing.DrawPath(rp.Canvas, rp.Paint, pixels, arrowStyle.FillStyle);
        Drawing.DrawPath(rp.Canvas, rp.Paint, pixels, arrowStyle.LineStyle);

        rp.CanvasState.Restore();
    }
}
