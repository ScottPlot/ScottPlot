namespace ScottPlot.WinForms;

/* This file demonstrates how a GUI app can interact with ScottPlot.
 * 
 * As the API stabalizes, much of this logic can be moved to a user control.
 * 
 * Eventually a user control abstraction should be implemented to make it
 * easy to create controls for different platforms.
 * 
 */

public partial class Form1 : Form
{
    readonly Plot Plot = new();

    /// <summary>
    /// Example crosshair plottable that this program will update to track the mouse
    /// </summary>
    readonly Plottables.DebugPoint DebugPoint = new();

    /// <summary>
    /// A snapshot of the axis limits when the mouse was pressed at the start of a click-drag action
    /// </summary>
    AxisLimits MouseDownLimits;

    /// <summary>
    /// Current position of the mouse used for hit detection and click-drag calculations
    /// </summary>
    Pixel MouseDownPixel;

    /// <summary>
    /// Set/cleared by SkglControl MouseDown/MouseUp events
    /// </summary>
    bool IsMouseDown;

    public Form1()
    {
        InitializeComponent();

        // wire-up mouse events programatically (not using the designer)
        skglControl1.MouseDown += SkglControl1_MouseDown;
        skglControl1.MouseUp += SkglControl1_MouseUp;
        skglControl1.MouseMove += SkglControl1_MouseMove;
        skglControl1.MouseWheel += SkglControl1_MouseWheel;
        skglControl1.DoubleClick += SkglControl1_DoubleClick;

        // plot some test objects
        Plot.Add(DebugPoint);
        Plot.AddScatter(Generate.Consecutive(51), Generate.Sin(51), Colors.Blue);
        Plot.AddScatter(Generate.Consecutive(51), Generate.Cos(51), Colors.Red);
    }

    private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
    {
        Plot.Render(e.Surface);
    }

    private void SkglControl1_MouseWheel(object? sender, MouseEventArgs e)
    {
        // zoom in/out to the pixel beneath the mouse
        double fracX = e.Delta > 0 ? 1.15 : .85;
        double fracY = e.Delta > 0 ? 1.15 : .85;
        Plot.MouseZoom(fracX, fracY, new Pixel(e.X, e.Y));
        skglControl1.Invalidate();
    }

    private void SkglControl1_DoubleClick(object? sender, EventArgs e)
    {
        // toggle the benchmark visibility
        Plot.Benchmark.IsVisible = !Plot.Benchmark.IsVisible;
    }

    private void SkglControl1_MouseDown(object? sender, MouseEventArgs e)
    {
        // possible start of a click-drag
        IsMouseDown = true;
        MouseDownLimits = Plot.GetAxisLimits();
        MouseDownPixel = new(e.X, e.Y);
    }

    private void SkglControl1_MouseUp(object? sender, MouseEventArgs e)
    {
        IsMouseDown = false;

        if (e.Button == MouseButtons.Middle)
        {
            // determine if the mouse was dragged enough to apply a zoom rectangle
            float travX = e.X - MouseDownPixel.X;
            float travY = e.Y - MouseDownPixel.Y;
            double pxDragged = Math.Sqrt(travX * travX + travY + travY);
            double pxThresholdForZoomRectangle = 5;

            if (pxDragged > pxThresholdForZoomRectangle)
            {
                // the middle mouse drag was large so apply the zoom rectangle
                Plot.MouseZoomRectangleClear(applyZoom: true);
            }
            else
            {
                // the middle mouse drag was small so assume it was middle-click axis reset
                Plot.MouseZoomRectangleClear(applyZoom: false);
                Plot.AutoScale();
            }
        }

        skglControl1.Invalidate();
    }

    private void SkglControl1_MouseMove(object? sender, MouseEventArgs e)
    {
        Pixel pixel = new(e.X, e.Y);

        if (IsMouseDown)
        {
            if (e.Button == MouseButtons.Left)
            {
                Plot.MousePan(MouseDownLimits, MouseDownPixel, pixel);
            }
            else if (e.Button == MouseButtons.Right)
            {
                Plot.MouseZoom(MouseDownLimits, MouseDownPixel, pixel);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                Plot.MouseZoomRectangle(MouseDownPixel, pixel);
            }

            skglControl1.Invalidate();
        }
        else
        {
            // THIS BLOCK IS DEMO CODE ONLY
            DebugPoint.Position = Plot.GetCoordinate(pixel);
            skglControl1.Invalidate();
        }
    }
}
