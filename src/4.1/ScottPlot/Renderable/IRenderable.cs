namespace ScottPlot.Renderable
{
    public interface IRenderable
    {
        PlotLayer Layer { get; }
        void Render(System.Drawing.Bitmap bmp, PlotInfo info);
    }
}
