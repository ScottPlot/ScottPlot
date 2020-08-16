using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;

namespace ScottPlot.Renderables
{
    public class Benchmark : IRenderable
    {
        private readonly Stopwatch sw = Stopwatch.StartNew();
        public void Restart() => sw.Restart();
        public void Stop() => sw.Stop();

        public void Render(Bitmap bmp, Experimental.FigureInfo fig)
        {
            double elapsedSec = (double)sw.ElapsedTicks / Stopwatch.Frequency;
            string message = $"{elapsedSec * 1000:0.00} ms ({1 / elapsedSec:0.00} Hz)";

            using (Graphics gfx = Graphics.FromImage(bmp))
            using (Font font = new Font(FontFamily.GenericMonospace, 12))
            {
                gfx.DrawString(message, font, Brushes.Red, 20, 20);
            }
        }
    }
}
