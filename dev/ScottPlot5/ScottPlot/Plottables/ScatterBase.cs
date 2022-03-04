using Microsoft.Maui.Graphics;

namespace ScottPlot.Plottables;

/// <summary>
/// This class imlpements most scatter plot styling and rendering logic.
/// Child classes implement this class and are only responsible for managing their data.
/// It is recommended that child classes make the objects that hold their data private.
/// </summary>
public abstract class ScatterBase : IPlottable
{
    public float LineWidth = 1;
    public Color LineColor = Colors.Black;

    public float MarkerSize = 3;
    public Color MarkerColor = Colors.Black;

    protected abstract PointF[] GetPoints(PlotConfig plotInfo);
    public abstract CoordinateRect GetDataLimits();

    protected abstract int Count { get; }

    public void Draw(ICanvas canvas, PlotConfig plotInfo)
    {
        canvas.ClipRectangle(plotInfo.DataRect.RectangleF);
        PointF[] points = GetPoints(plotInfo);

        canvas.StrokeSize = LineWidth;
        canvas.StrokeColor = LineColor;
        for (int i = 0; i < points.Length - 1; i++)
            canvas.DrawLine(points[i], points[i + 1]);

        if (MarkerSize > 0 && MarkerColor != Colors.Transparent)
        {
            canvas.FillColor = MarkerColor;
            for (int i = 0; i < points.Length; i++)
                canvas.FillCircle(points[i], MarkerSize);
        }
    }
}
