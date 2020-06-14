using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ScottPlot.Config;

namespace ScottPlot.plottables
{
    // This class is intended for experimentation with new tick-determination systems
    public class ExperimentalTicks : Plottable
    {
        public override AxisLimits2D GetLimits() => null;
        public override LegendItem[] GetLegendItems() => null;
        public override int GetPointCount() => 0;
        public override string ToString() => "experimental ticks";

        // public methods can be modified externally to tweak tick display settings
        public float lineWidth = 1;

        public override void Render(Settings settings)
        {
            // these are useful pixel locations
            float pixelX = settings.dataOrigin.X + settings.dataSize.Width + 5;
            float pixelYtop = settings.dataOrigin.Y;
            float pixelYbot = settings.dataOrigin.Y + settings.dataSize.Height;

            // these are useful coordinate values
            double coordinateYtop = settings.axes.y.max;
            double coordinateYbot = settings.axes.y.min;

            // Calculate where ticks should be.
            // This is may be easier with private static helper classes or structs.
            var ticks = new List<(double coordinate, bool isMajor)>();
            int coordinateY = (int)coordinateYbot;
            while (coordinateY <= coordinateYtop && ticks.Count < 50)
            {
                // This is a really silly way to make major and minor ticks.
                // Improving the tick system means improving this part of the code.
                bool isMajor = coordinateY % 5 == 0;
                ticks.Add((coordinateY, isMajor));
                coordinateY += 1;
            }

            using (var pen = new Pen(Color.Navy, lineWidth))
            using (var font = new Font(FontFamily.GenericMonospace, 10, FontStyle.Bold))
            {
                // draw vertical line
                settings.gfxFigure.DrawLine(pen, pixelX, pixelYbot, pixelX, pixelYtop);

                // draw and label individual ticks
                foreach (var tick in ticks)
                {
                    float tickWidth = tick.isMajor ? 6 : 3;
                    float tickPixelY = (float)settings.GetPixelY(tick.coordinate) + settings.dataOrigin.Y;

                    settings.gfxFigure.DrawLine(pen, pixelX, tickPixelY, pixelX + tickWidth, tickPixelY);

                    if (tick.isMajor)
                    {
                        settings.gfxFigure.DrawString($"{tick.coordinate:N2}",
                            font, Brushes.Navy, pixelX + tickWidth, tickPixelY, settings.misc.sfWest);
                    }
                }
            }
        }
    }
}
