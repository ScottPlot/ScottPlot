namespace ScottPlot.ArrowShapes;

public class SingleLine : IArrowShape
{
    public void Render(RenderPack rp, PixelLine arrowLine, ArrowStyle arrowStyle)
    {
        float length = arrowLine.Length;

        PixelLine[] lines = [
            new PixelLine(0, 0, length, 0),
            new PixelLine(0, 0, arrowStyle.ArrowheadLength, arrowStyle.ArrowheadWidth / 2),
            new PixelLine(0, 0, arrowStyle.ArrowheadLength, -arrowStyle.ArrowheadWidth / 2),
        ];

        rp.CanvasState.Save();
        rp.CanvasState.Translate(arrowLine.Pixel2);
        rp.CanvasState.RotateDegrees(arrowLine.AngleDegrees + 90);

        // origin is the tip, base extends to the right
        Drawing.DrawLines(rp.Canvas, rp.Paint, lines, arrowStyle.LineStyle);

        rp.CanvasState.Restore();
    }
}
