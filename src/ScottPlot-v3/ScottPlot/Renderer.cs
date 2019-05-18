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
            if (gfxData == null)
                return;

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

        public static void PlaceDataOnFigure(Graphics gfxFigure, Settings settings, Bitmap bmpData)
        {
            if (gfxFigure == null || bmpData == null)
                return;

            gfxFigure.DrawImage(bmpData, settings.dataOrigin);
        }

        public static void Labels(Graphics gfxFigure, Settings settings)
        {
            bool drawDebugRectangles = false;

            SizeF titleSizeF = gfxFigure.MeasureString(settings.title, settings.titleFont);
            Size titleSize = new Size((int)titleSizeF.Width, (int)titleSizeF.Height);
            Point titlePoint = new Point(settings.figureSize.Width / 2, settings.axisPadding);
            titlePoint.X -= titleSize.Width / 2;
            gfxFigure.DrawString(settings.title, settings.titleFont, settings.titleFontBrush, titlePoint, settings.sfNorthWest);
            if (drawDebugRectangles)
                gfxFigure.DrawRectangle(Pens.Magenta, titlePoint.X, titlePoint.Y, titleSize.Width, titleSize.Height);

            SizeF xLabelSizF = gfxFigure.MeasureString(settings.axisLabelX, settings.axisLabelFont);
            Size xLabelSize = new Size((int)xLabelSizF.Width, (int)xLabelSizF.Height);
            Point xLabelPoint = new Point(settings.dataSize.Width / 2 + settings.dataOrigin.X, settings.figureSize.Height - settings.axisPadding);
            xLabelPoint.X -= xLabelSize.Width / 2;
            xLabelPoint.Y -= xLabelSize.Height;
            gfxFigure.DrawString(settings.axisLabelX, settings.axisLabelFont, settings.axisLabelBrush, xLabelPoint, settings.sfNorthWest);
            if (drawDebugRectangles)
                gfxFigure.DrawRectangle(Pens.Magenta, xLabelPoint.X, xLabelPoint.Y, xLabelSize.Width, xLabelSize.Height);

            gfxFigure.RotateTransform(-90);
            SizeF yLabelSizF = gfxFigure.MeasureString(settings.axisLabelY, settings.axisLabelFont);
            Size yLabelSize = new Size((int)yLabelSizF.Width, (int)yLabelSizF.Height);
            Point yLabelPoint = new Point(-settings.dataSize.Height / 2 - settings.dataOrigin.Y, settings.axisPadding);
            yLabelPoint.X -= yLabelSize.Width / 2;
            gfxFigure.DrawString(settings.axisLabelY, settings.axisLabelFont, settings.axisLabelBrush, yLabelPoint, settings.sfNorthWest);
            if (drawDebugRectangles)
                gfxFigure.DrawRectangle(Pens.Magenta, yLabelPoint.X, yLabelPoint.Y, yLabelSize.Width, yLabelSize.Height);
            gfxFigure.ResetTransform();
        }

        public static void Ticks(Graphics gfxFigure, Settings settings)
        {
            if (settings.dataSize.Width < 1 || settings.dataSize.Height < 1)
                return;

            gfxFigure.DrawRectangle(Pens.Black, settings.dataOrigin.X, settings.dataOrigin.Y, settings.dataSize.Width - 1, settings.dataSize.Height - 1);
            Ticks ticks = new Ticks(gfxFigure, settings);
            ticks.Render();
        }

        public static void AxisFrame(Graphics gfxFigure, Settings settings)
        {
            if (settings.dataSize.Width<1 || settings.dataSize.Height<1)
                return;

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

        public static void Benchmark(Graphics gfxFigure, Settings settings, int plottableCount, int pointCount, double renderTimeMs, double renderRateHz, bool fullRender)
        {
            string msg = "";
            msg += $"Full render of {plottableCount} objects ({pointCount} points)";
            msg += string.Format(" took {0:000.000} ms ({1:000.00 Hz})", renderTimeMs, renderRateHz);
            if (!fullRender)
                msg = msg.Replace("Full", "Data-only");

            SizeF textSizeF = gfxFigure.MeasureString(msg, settings.debugFont);
            Size textSize = new Size((int)textSizeF.Width, (int)textSizeF.Height);
            Point textLocation = new Point(settings.dataSize.Width + settings.dataOrigin.X, settings.dataSize.Height + settings.dataOrigin.Y);
            textLocation.X -= textSize.Width + 3;
            textLocation.Y -= textSize.Height + 3;
            Rectangle textRect = new Rectangle(textLocation, textSize);
            gfxFigure.FillRectangle(settings.debugBackgroundBrush, textRect);
            //gfxFigure.DrawRectangle(settings.debugBorderPen, textRect);
            gfxFigure.DrawString(msg, settings.debugFont, settings.debugFontBrush, textLocation);
        }
    }
}
