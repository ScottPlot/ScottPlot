using ScottPlot.Renderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Renderable
{
    public class ZoomRectangle : IRenderable
    {
        public PlotLayer Layer => PlotLayer.AboveData;

        public bool Visible { get; set; }
        private readonly Point Pt1 = new Point();
        private readonly Point Pt2 = new Point();

        public Color FillColor = Color.FromARGB(20, Colors.Red);
        public Color EdgeColor = Color.FromARGB(50, Colors.Red);

        public void MouseDown(float x, float y)
        {
            Pt1.X = x;
            Pt1.Y = y;
            Visible = true;
        }

        public (float xLeft, float xRight, float yTop, float yBottom) GetLimits()
        {
            float xLeft = Math.Min(Pt1.X, Pt2.X);
            float xRight = Math.Max(Pt1.X, Pt2.X);
            float yTop = Math.Min(Pt1.Y, Pt2.Y);
            float yBottom = Math.Max(Pt1.Y, Pt2.Y);
            return (xLeft, xRight, yTop, yBottom);
        }

        public void MouseUp()
        {
            Pt1.X = 0;
            Pt1.Y = 0;
            Pt2.X = 0;
            Pt2.Y = 0;
            Visible = false;
        }

        public void MouseMove(float x, float y)
        {
            Pt2.X = x;
            Pt2.Y = y;
        }

        public void Render(IRenderer renderer, Dimensions dims, bool lowQuality)
        {
            if (Visible is false)
                return;

            var (xLeft, xRight, yTop, yBottom) = GetLimits();
            Point pt = new Point(xLeft, yTop);
            Size sz = new Size(xRight - xLeft, yBottom - yTop);

            renderer.FillRectangle(pt, sz, FillColor);
            renderer.DrawRectangle(pt, sz, EdgeColor, 1);
        }
    }
}
