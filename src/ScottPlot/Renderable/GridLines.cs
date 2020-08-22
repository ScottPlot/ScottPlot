using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    public class GridLines : IRenderable
    {
        public bool Visible = true;
        public bool SnapToNearestPixel = true;

        public Orientation Orientation;
        public float LineWidth = 1;
        public Color Color = ColorTranslator.FromHtml("#efefef");
        public LineStyle LineStyle = LineStyle.Solid;

        public void Render(Settings settings)
        {
            if ((Visible == false) || (LineWidth == 0))
                return;

            using (Pen pen = Drawing.GDI.Pen(Color, LineWidth, LineStyle))
            {
                if (Orientation == Orientation.Horizontal)
                    RenderHorizontalLines(pen, settings);
                else
                    RenderVerticalLines(pen, settings);
            }
        }

        private void RenderHorizontalLines(Pen pen, Settings settings)
        {
            for (int i = 0; i < settings.ticks.y.tickPositionsMajor.Length; i++)
            {
                double value = settings.ticks.y.tickPositionsMajor[i];
                double unitsFromAxisEdge = value - settings.axes.y.min;
                double yPx = settings.dataSize.Height - unitsFromAxisEdge * settings.yAxisScale;

                if (SnapToNearestPixel)
                    yPx = (int)yPx;

                if ((yPx == 0) && settings.layout.displayFrameByAxis[2])
                    continue; // don't draw a grid line 1px away from frame

                PointF ptLeft = new PointF(0, (float)yPx);
                PointF ptRight = new PointF(settings.dataSize.Width, (float)yPx);
                settings.gfxData.DrawLine(pen, ptLeft, ptRight);
            }
        }

        private void RenderVerticalLines(Pen pen, Settings settings)
        {
            for (int i = 0; i < settings.ticks.x.tickPositionsMajor.Length; i++)
            {
                double value = settings.ticks.x.tickPositionsMajor[i];
                double unitsFromAxisEdge = value - settings.axes.x.min;
                double xPx = unitsFromAxisEdge * settings.xAxisScale;

                if (SnapToNearestPixel)
                    xPx = (int)xPx;

                if ((xPx == 0) && settings.layout.displayFrameByAxis[0])
                    continue; // don't draw a grid line 1px away from frame

                PointF ptTop = new PointF((float)xPx, 0);
                PointF ptBot = new PointF((float)xPx, settings.dataSize.Height);
                settings.gfxData.DrawLine(pen, ptTop, ptBot);
            }
        }
    }
}
