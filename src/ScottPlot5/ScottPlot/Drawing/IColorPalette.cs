namespace ScottPlot.Drawing
{
    public interface IColorPalette
    {
        Renderer.Color GetColor(int index);
        int Count();
    }
}
