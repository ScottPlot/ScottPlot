namespace ScottPlot.ArrowShapes;

public class SingleLine : IArrowShape
{
    public void Render(RenderPack rp, Pixel pxBase, Pixel pxTip, ArrowStyle arrowStyle)
    {
        PixelLine arrowLine = new(pxBase, pxTip);
        float length = arrowLine.Length;

        PixelLine[] lines = [
            new PixelLine(0, 0, length, 0),
            new PixelLine(0, 0, arrowStyle.ArrowheadLength, arrowStyle.ArrowheadWidth/2),
            new PixelLine(0, 0, arrowStyle.ArrowheadLength, -arrowStyle.ArrowheadWidth/2),
        ];

        rp.CanvasState.Save();
        rp.CanvasState.Translate(pxTip);
        rp.CanvasState.RotateDegrees(arrowLine.SlopeDegrees);

        Drawing.DrawLines(rp.Canvas, rp.Paint, lines, arrowStyle.LineStyle);

        rp.CanvasState.Restore();
    }
}
