using Microsoft.Maui.Graphics;

namespace ScottPlot;

public class Plot
{
    public readonly PlotLayout Layout = new();
    public readonly PlotStyle Style = new();

    public Plot()
    {
    }

    public void Draw(ICanvas canvas, float width, float height)
    {
        Layout.Resize(width, height);

        if (Layout.HasFigureArea)
            DrawFigure(canvas);

        if (Layout.HasDataArea)
            DrawDataArea(canvas);
    }

    private void DrawFigure(ICanvas canvas)
    {
        canvas.FillColor = Style.FigureBackgroundColor;
        canvas.FillRectangle(Layout.FigureRect);
    }

    private void DrawDataArea(ICanvas canvas)
    {
        canvas.FillColor = Style.DataBackgroundColor;
        canvas.FillRectangle(Layout.DataRect);

        canvas.StrokeColor = Style.DataBorderColor;
        canvas.DrawRectangle(Layout.DataRect);
    }
}