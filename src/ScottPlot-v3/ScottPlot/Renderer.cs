using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ScottPlot
{
    public class Renderer
    {
        public static void Background(Graphics gfxFigure, Settings settings)
        {
            gfxFigure.Clear(settings.figureBackgroundColor);
        }

        public static void DataPlottables(Graphics gfxData, Settings settings, List<Plottable> plottables)
        {
            gfxData.Clear(settings.dataBackgroundColor);
            for (int i = 0; i < plottables.Count; i++)
            {
                Plottable pltThing = plottables[i];
                try
                {
                    pltThing.Render(settings, gfxData);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"EXCEPTION while rendering {pltThing}:\n{ex}");
                }
            }
        }

        public static void Labels(Graphics gfxFigure, Settings settings)
        {
            Point pointTitle = new Point(settings.figureSize.Width / 2, 10);
            gfxFigure.DrawString(settings.title, settings.titleFont, settings.titleFontBrush, pointTitle, settings.sfNorth);

            Point pointXlabel = new Point(settings.figureSize.Width / 2, settings.figureSize.Height - 10);
            gfxFigure.DrawString(settings.axisLabelX, settings.axisLabelFont, settings.axisLabelBrush, pointXlabel, settings.sfSouth);

            gfxFigure.RotateTransform(-90);
            Point pointYlabel = new Point(-settings.figureSize.Height / 2, 10);
            gfxFigure.DrawString(settings.axisLabelY, settings.axisLabelFont, settings.axisLabelBrush, pointYlabel, settings.sfNorth);
            gfxFigure.ResetTransform();
        }

        public static void Ticks(Graphics gfxFigure, Settings settings)
        {
            gfxFigure.DrawRectangle(Pens.Black, settings.dataOrigin.X, settings.dataOrigin.Y, settings.dataSize.Width - 1, settings.dataSize.Height - 1);
            global::ScottPlot.Ticks.DrawTicks(gfxFigure, settings);
        }

        public static void AxisFrame(Graphics gfxFigure, Settings settings)
        {
            Point tl = new Point(settings.dataOrigin.X - 1, settings.dataOrigin.Y - 1);
            Point tr = new Point(settings.dataOrigin.X + settings.dataSize.Width, settings.dataOrigin.Y - 1);
            Point bl = new Point(settings.dataOrigin.X - 1, settings.dataOrigin.Y + settings.dataSize.Height);
            Point br = new Point(settings.dataOrigin.X + settings.dataSize.Width, settings.dataOrigin.Y + settings.dataSize.Height);

            if (settings.axisFrame[0])
                gfxFigure.DrawLine(settings.axisFramePen, tl, bl);
            if (settings.axisFrame[1])
                gfxFigure.DrawLine(settings.axisFramePen, tr, br);
            if (settings.axisFrame[2])
                gfxFigure.DrawLine(settings.axisFramePen, bl, br);
            if (settings.axisFrame[3])
                gfxFigure.DrawLine(settings.axisFramePen, tl, tr);
        }

        public static void Benchmark(Graphics gfxFigure, Settings settings, string message)
        {
            SizeF textSizeF = gfxFigure.MeasureString(message, settings.debugFont);
            Size textSize = new Size((int)textSizeF.Width, (int)textSizeF.Height);
            Point textLocation = new Point(settings.figureSize.Width - textSize.Width, settings.figureSize.Height - textSize.Height);
            textLocation.X -= 5;
            textLocation.Y -= 5;
            Rectangle textRect = new Rectangle(textLocation, textSize);
            gfxFigure.FillRectangle(settings.debugBackgroundBrush, textRect);
            gfxFigure.DrawRectangle(settings.debugBorderPen, textRect);
            gfxFigure.DrawString(message, settings.debugFont, settings.debugFontBrush, textLocation);
        }
    }
}
