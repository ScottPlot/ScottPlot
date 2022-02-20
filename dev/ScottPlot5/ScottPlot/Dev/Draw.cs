using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Graphics;

namespace ScottPlot.Dev
{
    public static class Draw
    {
        public static void RectangleTestPattern(ICanvas canvas, RectangleF rect, float alpha=.2f)
        {
            canvas.StrokeColor = Colors.Black.WithAlpha(alpha);
            canvas.StrokeSize = 1;
            canvas.DrawRectangle(rect);
            canvas.DrawLine(rect.Left, rect.Bottom, rect.Right, rect.Top);
            canvas.DrawLine(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
    }
}
