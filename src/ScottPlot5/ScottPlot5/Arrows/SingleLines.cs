namespace ScottPlot.Arrows;

public class SingleLines : IArrow
{
    public float Width { get; set; } = 5;
    public float HeadAxisLength { get; set; } = 15;
    public float HeadLength { get; set; } = 20;
    public float HeadWidth { get; set; } = 20;

    public void Render(RenderPack rp, Pixel pxBase, Pixel pxTip, ArrowStyle arrowStyle)
    {
        PixelLine arrowLine = new(pxBase, pxTip);
        float length = arrowLine.Length;

        PixelLine[] lines = [
            new PixelLine(0, 0, length, 0),
            new PixelLine(0, 0, HeadLength, HeadWidth/2),
            new PixelLine(0, 0, HeadLength, -HeadWidth/2),
        ];

        rp.CanvasState.Save();
        rp.CanvasState.Translate(pxTip);
        rp.CanvasState.RotateDegrees(arrowLine.SlopeDegrees);

        Drawing.DrawLines(rp.Canvas, rp.Paint, lines, arrowStyle.LineStyle);

        rp.CanvasState.Restore();
    }
}
