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
        public static void FigureClear(Settings settings)
        {
            settings.gfxFigure.Clear(settings.figureBackgroundColor);
        }

        public static void DataBackground(Settings settings)
        {
            settings.gfxData.Clear(settings.dataBackgroundColor);
        }

        public static void DataGrid(Settings settings)
        {
            Ticks ticks = new Ticks(settings);
            ticks.RenderGrid();
        }

        public static void DataPlottables(Settings settings)
        {
            if (settings.gfxData == null)
                return;

            for (int i = 0; i < settings.plottables.Count; i++)
            {
                Plottable pltThing = settings.plottables[i];
                try
                {
                    pltThing.Render(settings);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"EXCEPTION while rendering {pltThing}:\n{ex}");
                }
            }
        }

        public static void DataPlaceOntoFigure(Settings settings)
        {
            if (settings.gfxFigure == null || settings.bmpData == null)
                return;

            settings.gfxFigure.DrawImage(settings.bmpData, settings.dataOrigin);
        }

        public static void FigureLabels(Settings settings)
        {
            bool drawDebugRectangles = false;

            SizeF titleSizeF = settings.gfxFigure.MeasureString(settings.title, settings.titleFont);
            Size titleSize = new Size((int)titleSizeF.Width, (int)titleSizeF.Height);
            Point titlePoint = new Point(settings.figureSize.Width / 2, settings.axisPadding);
            titlePoint.X -= titleSize.Width / 2;
            settings.gfxFigure.DrawString(settings.title, settings.titleFont, settings.titleFontBrush, titlePoint, settings.sfNorthWest);
            if (drawDebugRectangles)
                settings.gfxFigure.DrawRectangle(Pens.Magenta, titlePoint.X, titlePoint.Y, titleSize.Width, titleSize.Height);

            SizeF xLabelSizF = settings.gfxFigure.MeasureString(settings.axisLabelX, settings.axisLabelFont);
            Size xLabelSize = new Size((int)xLabelSizF.Width, (int)xLabelSizF.Height);
            Point xLabelPoint = new Point(settings.dataSize.Width / 2 + settings.dataOrigin.X, settings.figureSize.Height - settings.axisPadding);
            xLabelPoint.X -= xLabelSize.Width / 2;
            xLabelPoint.Y -= xLabelSize.Height;
            settings.gfxFigure.DrawString(settings.axisLabelX, settings.axisLabelFont, settings.axisLabelBrush, xLabelPoint, settings.sfNorthWest);
            if (drawDebugRectangles)
                settings.gfxFigure.DrawRectangle(Pens.Magenta, xLabelPoint.X, xLabelPoint.Y, xLabelSize.Width, xLabelSize.Height);

            
            SizeF yLabelSizF = settings.gfxFigure.MeasureString(settings.axisLabelY, settings.axisLabelFont);
            Size yLabelSize = new Size((int)yLabelSizF.Width, (int)yLabelSizF.Height);
            Point yLabelPoint = new Point(-settings.dataSize.Height / 2 - settings.dataOrigin.Y, settings.axisPadding);
            yLabelPoint.X -= yLabelSize.Width / 2;
            settings.gfxFigure.RotateTransform(-90);
            settings.gfxFigure.DrawString(settings.axisLabelY, settings.axisLabelFont, settings.axisLabelBrush, yLabelPoint, settings.sfNorthWest);
            if (drawDebugRectangles)
                settings.gfxFigure.DrawRectangle(Pens.Magenta, yLabelPoint.X, yLabelPoint.Y, yLabelSize.Width, yLabelSize.Height);
            settings.gfxFigure.ResetTransform();
        }

        public static void FigureTicks(Settings settings)
        {
            if (settings.dataSize.Width < 1 || settings.dataSize.Height < 1)
                return;

            Ticks ticks = new Ticks(settings);
            ticks.RenderTicks();
        }

        public static void FigureFrames(Settings settings)
        {
            if (settings.dataSize.Width<1 || settings.dataSize.Height<1)
                return;

            Point tl = new Point(settings.dataOrigin.X - 1, settings.dataOrigin.Y - 1);
            Point tr = new Point(settings.dataOrigin.X + settings.dataSize.Width, settings.dataOrigin.Y - 1);
            Point bl = new Point(settings.dataOrigin.X - 1, settings.dataOrigin.Y + settings.dataSize.Height);
            Point br = new Point(settings.dataOrigin.X + settings.dataSize.Width, settings.dataOrigin.Y + settings.dataSize.Height);

            if (settings.axisFrame[0])
                settings.gfxFigure.DrawLine(settings.axisFramePen, tl, bl);
            if (settings.axisFrame[1])
                settings.gfxFigure.DrawLine(settings.axisFramePen, tr, br);
            if (settings.axisFrame[2])
                settings.gfxFigure.DrawLine(settings.axisFramePen, bl, br);
            if (settings.axisFrame[3])
                settings.gfxFigure.DrawLine(settings.axisFramePen, tl, tr);
        }

        public static void Benchmark(Settings settings)
        {
            if (settings.displayBenchmark)
            {
                int debugPadding = 3;
                SizeF textSizeF = settings.gfxFigure.MeasureString(settings.benchmarkMessage, settings.debugFont);
                Size textSize = new Size((int)textSizeF.Width, (int)textSizeF.Height);
                Point textLocation = new Point(settings.dataSize.Width + settings.dataOrigin.X, settings.dataSize.Height + settings.dataOrigin.Y);
                textLocation.X -= textSize.Width + debugPadding;
                textLocation.Y -= textSize.Height + debugPadding;
                Rectangle textRect = new Rectangle(textLocation, textSize);
                settings.gfxFigure.FillRectangle(settings.debugBackgroundBrush, textRect);
                settings.gfxFigure.DrawRectangle(settings.debugBorderPen, textRect);
                settings.gfxFigure.DrawString(settings.benchmarkMessage, settings.debugFont, settings.debugFontBrush, textLocation);
            }
        }
    }
}
