namespace ScottPlot.Arrows;

public class Single : IArrow
{
    public float Width { get; set; } = 5;
    public float HeadAxisLength { get; set; } = 15;
    public float HeadLength { get; set; } = 20;
    public float HeadWidth { get; set; } = 20;

    public float Scale { get; set; } = 1;

    public float MinimumLength { get; set; } = 0;
    public float MaximumLength { get; set; } = float.MaxValue;

    public void Render(RenderPack rp, Pixel pxBase, Pixel pxTip, ArrowStyle arrowStyle)
    {
        PixelLine arrowLine = new(pxBase, pxTip);
        float length = arrowLine.Length;

        rp.CanvasState.Save();
        rp.CanvasState.Translate(pxTip);
        rp.CanvasState.RotateDegrees(arrowLine.SlopeDegrees);

        Pixel[] pixels = [
            new(0, 0),
            new(HeadLength, HeadWidth / 2),
            new(HeadAxisLength, Width / 2),
            new(length, Width / 2),
            new(length, -Width / 2),
            new(HeadAxisLength, -Width / 2),
            new(HeadLength, -HeadWidth / 2),
            new(0, 0),
        ];

        using SKPaint paint = new();

        Drawing.DrawPath(rp.Canvas, paint, pixels, arrowStyle.FillStyle);
        Drawing.DrawPath(rp.Canvas, paint, pixels, arrowStyle.LineStyle);

        rp.CanvasState.Restore();
    }
}
