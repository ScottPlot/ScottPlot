namespace ScottPlot;

public class CanvasState(SKCanvas canvas)
{
    private readonly SKCanvas Canvas = canvas;

    public int SaveLevels { get; set; } = 0;

    public void Save()
    {
        Canvas.Save();
        SaveLevels += 1;
    }

    public void Restore()
    {
        if (SaveLevels == 0)
        {
            throw new InvalidOperationException("Restore() was called more than Save()");
        }

        Canvas.Restore();
        SaveLevels -= 1;
    }

    public void RestoreAll()
    {
        while (SaveLevels > 0)
        {
            Restore();
        }
    }

    public void DisableClipping()
    {
        RestoreAll();
    }

    public void Clip(PixelRect rect)
    {
        Canvas.ClipRect(rect.ToSKRect());
    }

    // TODO: manage translation and rotation too
}
