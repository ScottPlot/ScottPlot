namespace ScottPlot.Renderable
{
    public interface IRenderable
    {
        PlotLayer Layer { get; }
        bool Visible { get; set; }
        bool AntiAlias { get; set; }
        void Render(System.Drawing.Bitmap bmp, PlotInfo info);
    }
}
