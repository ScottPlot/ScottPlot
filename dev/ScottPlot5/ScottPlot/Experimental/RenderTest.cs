using Microsoft.Maui.Graphics;

namespace ScottPlot.Experimental;

public static class RenderTest
{
    public static readonly Random Rand = new();

    public static void Lines(ICanvas canvas, float width, float height, int lines = 100)
    {
        canvas.FillColor = Colors.Navy;
        canvas.FillRectangle(0, 0, width, height);

        canvas.StrokeColor = Colors.White;
        for (int i = 0; i < lines; i++)
        {
            float x1 = (float)Rand.NextDouble() * width;
            float x2 = (float)Rand.NextDouble() * width;
            float y1 = (float)Rand.NextDouble() * height;
            float y2 = (float)Rand.NextDouble() * height;
            canvas.DrawLine(x1, y1, x2, y2);
        }
    }
}