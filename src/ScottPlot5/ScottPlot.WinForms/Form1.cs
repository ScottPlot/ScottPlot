using System.Diagnostics;

namespace ScottPlot.WinForms;

public partial class Form1 : Form
{
    readonly Plot Plot = new();
    readonly Plottables.DebugPoint DebugPoint = new();
    CoordinateRect MouseDownLimits;
    Pixel MouseDownPixel;
    bool IsMouseDown;

    public Form1()
    {
        InitializeComponent();
        Plot.Add(DebugPoint);
        Plot.Add(new Plottables.DebugGrid());
        skglControl1.MouseMove += SkglControl1_MouseMove;
        skglControl1.MouseDown += SkglControl1_MouseDown;
        skglControl1.MouseUp += SkglControl1_MouseUp;
        skglControl1.DoubleClick += SkglControl1_DoubleClick;
        skglControl1.MouseWheel += SkglControl1_MouseWheel;
    }

    private void SkglControl1_MouseWheel(object? sender, MouseEventArgs e)
    {
        double fracX = e.Delta > 0 ? 1.15 : .85;
        double fracY = e.Delta > 0 ? 1.15 : .85;
        Plot.MouseZoom(fracX, fracY, new Pixel(e.X, e.Y));
        skglControl1.Invalidate();
    }

    private void SkglControl1_DoubleClick(object? sender, EventArgs e)
    {
        Plot.ShowDebugMessage = !Plot.ShowDebugMessage;
    }

    private void SkglControl1_MouseDown(object? sender, MouseEventArgs e)
    {
        IsMouseDown = true;
        MouseDownLimits = Plot.GetAxisLimits();
        MouseDownPixel = new(e.X, e.Y);
    }

    private void SkglControl1_MouseUp(object? sender, MouseEventArgs e)
    {
        IsMouseDown = false;

        if (e.Button == MouseButtons.Middle)
        {
            Plot.SetAxisLimits(-10, 10, -10, 10);
            skglControl1.Invalidate();
        }
    }

    private void SkglControl1_MouseMove(object? sender, MouseEventArgs e)
    {
        if (IsMouseDown)
        {
            if (e.Button == MouseButtons.Left)
            {
                Plot.MousePan(MouseDownLimits, MouseDownPixel, new(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Right)
            {
                Plot.MouseZoom(MouseDownLimits, MouseDownPixel, new(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Middle)
            {

            }

            skglControl1.Invalidate();
            return;
        }

        // make the position follow the mouse
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
