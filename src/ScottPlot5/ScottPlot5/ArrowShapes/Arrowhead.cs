namespace ScottPlot.ArrowShapes;

public class Arrowhead : IArrowShape
{
    public void Render(RenderPack rp, Pixel pxBase, Pixel pxTip, ArrowStyle arrowStyle)
    {
        PixelLine arrowLine = new(pxBase, pxTip);

        rp.CanvasState.Save();
        rp.CanvasState.Translate(pxTip);
        rp.CanvasState.RotateDegrees(arrowLine.SlopeDegrees);

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