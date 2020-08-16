using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderables
{
    public class DataFrame : IRenderable
    {
        public Color color = Color.Black;
        public bool Left = true;
        public bool Right = true;
        public bool Top = true;
        public bool Bottom = true;

        public void Render(Settings settings)
        {
            using (var gfx = Graphics.FromImage(settings.Bitmap))
            using (var pen = new Pen(color, 1))
            {
                if (Left)
                    gfx.DrawLine(pen, settings.DataL, settings.DataT, settings.DataL, settings.DataB);
                if (Right)
                    gfx.DrawLine(pen, settings.DataR, settings.DataT, settings.DataR, settings.DataB);
                if (Top)
                    gfx.DrawLine(pen, settings.DataL, settings.DataT, settings.DataR, settings.DataT);
                if (Bottom)
                    gfx.DrawLine(pen, settings.DataL, settings.DataB, settings.DataR, settings.DataB);
            }
        }
    }
}
