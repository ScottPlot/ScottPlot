namespace ScottPlot.ArrowShapes;

public class ArrowheadLine : IArrowShape
{
    public void Render(RenderPack rp, PixelLine arrowLine, ArrowStyle arrowStyle)
    {
        rp.CanvasState.Save();
        rp.CanvasState.Translate(arrowLine.Pixel2);
        rp.CanvasState.RotateDegrees(arrowLine.AngleDegrees + 90);

        // origin is the tip, base extends to the right
        PixelLine line1 = new(new(0, 0), new(arrowStyle.ArrowheadLength, arrowStyle.ArrowheadWidth / 2));
        PixelLine line2 = new(new(0, 0), new(arrowStyle.ArrowheadLength, -arrowStyle.ArrowheadWidth / 2));

        Drawing.DrawLine(rp.Canvas, rp.Paint, line1, arrowStyle.LineStyle);
        Drawing.DrawLine(rp.Canvas, rp.Paint, line2, arrowStyle.LineStyle);

        rp.CanvasState.Restore();
    }
}
