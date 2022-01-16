using Microsoft.Maui.Graphics;

namespace ScottPlot;

// TODO: dont store any state
public class PlotLayout
{
    public RectangleF FigureRect = new();

    public RectangleF DataRect = new();

    public bool HasDataArea => DataRect.Width > 0 && DataRect.Height > 0;

    public bool HasFigureArea => FigureRect.Width > 0 && FigureRect.Height > 0;

    public PlotLayout()
    {

    }

    public void Resize(float width, float height)
    {
        FigureRect.Width = width;
        FigureRect.Height = height;
        ResizeData();
    }

    private void ResizeData(float left = 40, float right = 10, float bottom = 20, float top = 10)
    {
        DataRect.X = left;
        DataRect.Width = FigureRect.Width - left - right;
        DataRect.Y = top;
        DataRect.Height = FigureRect.Height - top - bottom;
    }
}
