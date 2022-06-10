using System.Diagnostics;

namespace ScottPlot.WinForms;

public partial class Form1 : Form
{
    readonly Plot Plot = new();
    readonly Plottables.DebugPoint DebugPoint = new();

    public Form1()
    {
        InitializeComponent();
        Plot.Add(DebugPoint);
        Plot.Add(new Plottables.DebugGrid());
        skglControl1.MouseMove += SkglControl1_MouseMove;
    }

    private void SkglControl1_MouseMove(object? sender, MouseEventArgs e)
    {
        Pixel pixel = new(e.X, e.Y);
        Coordinate coord = Plot.GetCoordinate(pixel);
        DebugPoint.Position = coord;
        skglControl1.Invalidate();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        skglControl1.Invalidate();
    }

    private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
    {
        Plot.Render(e.Surface);
    }
}
