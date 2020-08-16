using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderables
{
    public class Grid : IRenderable
    {
        public bool HorizontalLines;
        public Color Color = Color.LightGray;

        public void Render(Settings settings)
        {
            if (HorizontalLines)
                RenderHorizontalLines(settings);
            else
                RenderVerticalLines(settings);
        }

        private void RenderVerticalLines(Settings settings)
        {
            using (var gfx = Graphics.FromImage(settings.Bitmap))
            using (var pen = new Pen(Color, 1))
            {
                for (int i = 0; i < settings.ticks.x.tickPositionsMajor.Length; i++)
                {
                    double tickPosition = settings.ticks.x.tickPositionsMajor[i];
                    float pxX = (float)settings.PixelX(tickPosition);
                    gfx.DrawLine(pen, pxX, settings.DataT, pxX, settings.DataB);
                }
            }
        }

        private void RenderHorizontalLines(Settings settings)
        {
            using (var gfx = Graphics.FromImage(settings.Bitmap))
            using (var pen = new Pen(Color, 1))
            {
                for (int i = 0; i < settings.ticks.y.tickPositionsMajor.Length; i++)
                {
                    double tickPosition = settings.ticks.y.tickPositionsMajor[i];
                    float pxY = (float)settings.PixelY(tickPosition);
                    gfx.DrawLine(pen, settings.DataL, pxY, settings.DataR, pxY);
                }
            }
        }
    }
}
