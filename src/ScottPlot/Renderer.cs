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
                catch (OverflowException)
                {
                    Debug.WriteLine($"OverflowException plotting: {pltThing}");
                }
            }
        }

        private class LegendItem
        {
            public string label;
            public int plottableIndex;
            public Color color;

            public LegendItem(string label, int plottableIndex, Color color)
            {
                this.label = label;
                this.plottableIndex = plottableIndex;
                this.color = color;
            }
        }

        public static void DataLegend(Settings settings)
        {
            int padding = 3;
            int stubWidth = 20;
            int stubHeight = 4;

            Brush brushText = new SolidBrush(settings.axisLabelColor);
            float legendFontLineHeight = settings.gfxData.MeasureString("TEST", settings.legendFont).Height;
            float legendFontMaxWidth = 0;

            // populate list of filled labels
            List<LegendItem> legendItems = new List<LegendItem>();
            for (int i = 0; i < settings.plottables.Count(); i++)
            {
                if (settings.plottables[i].label != null)
                {
                    legendItems.Add(new LegendItem(settings.plottables[i].label, i, settings.plottables[i].color));
                    float thisItemFontWidth = settings.gfxData.MeasureString(settings.plottables[i].label, settings.legendFont).Width;
                    if (thisItemFontWidth > legendFontMaxWidth)
                        legendFontMaxWidth = thisItemFontWidth;
                }
            }
            legendItems.Reverse();

            // draw the frame and border
            float frameWidth = padding * 2 + legendFontMaxWidth + padding + stubWidth;
            float frameHeight = padding * 2 + legendFontLineHeight * settings.plottables.Count();
            Size frameSize = new Size((int)frameWidth, (int)frameHeight);
            Point frameLoc = new Point((int)(settings.dataSize.Width - frameWidth - padding),
                (int)(settings.dataSize.Height - frameHeight - padding));
            Rectangle frameRect = new Rectangle(frameLoc, frameSize);
            settings.gfxData.FillRectangle(new SolidBrush(settings.dataBackgroundColor), frameRect);
            settings.gfxData.DrawRectangle(new Pen(settings.tickColor), frameRect);

            // draw the individual labels
            for (int i = 0; i < legendItems.Count; i++)
            {
                Point textLocation = new Point(settings.dataSize.Width, settings.dataSize.Height);
                textLocation.X -= (int)legendFontMaxWidth + padding * 2;
                textLocation.Y -= (int)(legendFontLineHeight * (i + 1)) + padding * 2;
                settings.gfxData.DrawString(legendItems[i].label, settings.legendFont, brushText, textLocation);
                settings.gfxData.DrawLine(new Pen(legendItems[i].color, stubHeight),
                    textLocation.X - padding, textLocation.Y + legendFontLineHeight / 2,
                    textLocation.X - padding - stubWidth, textLocation.Y + legendFontLineHeight / 2);
            }

            // draw colored lines  labels

            /*
            Size legendSize = new Size(0, 0);
            string[] legendLabels = new string[settings.plottables.Count()];
            Point[] legendLocations = new Point[settings.plottables.Count()];
            int textHalfHeight = (int)(textHeight / 2);

            for (int i = 0; i < settings.plottables.Count(); i++)
            {
                legendLabels[i] = settings.plottables[i].label;
                SizeF textSizeF = settings.gfxData.MeasureString(settings.plottables[i].label, legendFont);
                if (legendSize.Width < textSizeF.Width)
                    legendSize.Width = (int)textSizeF.Width;
                legendSize.Height += (int)textSizeF.Height + padding;
                Size textSize = new Size((int)textSizeF.Width, (int)textSizeF.Height);
                Point textLocation = new Point(settings.dataSize.Width, settings.dataSize.Height);
                textLocation.X -= textSize.Width + padding;
                textLocation.Y -= textSize.Height + padding;
                textLocation.Y -= (int)(padding + textSize.Height) * i;
                legendLocations[i] = textLocation;
            }


            Point textLocation2 = new Point(settings.dataSize.Width, settings.dataSize.Height);
            textLocation2.X -= legendSize.Width + padding;
            textLocation2.Y -= legendSize.Height + padding;

            Rectangle legendRect = new Rectangle(textLocation2, legendSize);
            legendRect.Width += stubWidth + padding;
            legendRect.X -= stubWidth + padding;

            Brush brushBackground = new SolidBrush(settings.dataBackgroundColor);
            Pen penBorder = new Pen(settings.tickColor);

            settings.gfxData.FillRectangle(brushBackground, legendRect);
            settings.gfxData.DrawRectangle(penBorder, legendRect);
            for (int i = 0; i < settings.plottables.Count(); i++)
            {
                settings.gfxData.DrawString(legendLabels[i], legendFont, brushText, legendLocations[i]);
                if (settings.plottables[i] is PlottableScatter)
                {
                    Pen pen = new Pen(((PlottableScatter)settings.plottables[i]).color, stubSize);
                    settings.gfxData.DrawLine(pen,
                        legendLocations[i].X,
                        legendLocations[i].Y + textHalfHeight,
                        legendLocations[i].X - stubWidth,
                        legendLocations[i].Y + textHalfHeight);
                }
            }
            */
        }

        public static void DataPlaceOntoFigure(Settings settings)
        {
            if (settings.gfxFigure == null || settings.bmpData == null)
                return;

            settings.gfxFigure.DrawImage(settings.bmpData, settings.dataOrigin);
        }

        public static void FigureLabels(Settings settings, bool drawDebugRectangles = false)
        {
            int dataCenterX = settings.dataSize.Width / 2 + settings.dataOrigin.X;
            int dataCenterY = settings.dataSize.Height / 2 + settings.dataOrigin.Y;

            // title
            SizeF titleSizeF = settings.gfxFigure.MeasureString(settings.title, settings.titleFont);
            Size titleSize = new Size((int)titleSizeF.Width, (int)titleSizeF.Height);
            Point titlePoint = new Point(dataCenterX - titleSize.Width / 2, settings.axisPadding);
            settings.gfxFigure.DrawString(settings.title, settings.titleFont, new SolidBrush(settings.titleColor), titlePoint, settings.sfNorthWest);
            if (drawDebugRectangles)
                settings.gfxFigure.DrawRectangle(Pens.Magenta, titlePoint.X, titlePoint.Y, titleSize.Width, titleSize.Height);

            // horizontal axis label
            SizeF xLabelSizF = settings.gfxFigure.MeasureString(settings.axisLabelX, settings.axisLabelFont);
            Size xLabelSize = new Size((int)xLabelSizF.Width, (int)xLabelSizF.Height);
            Point xLabelPoint = new Point(dataCenterX - xLabelSize.Width / 2, settings.figureSize.Height - settings.axisPadding - xLabelSize.Height);
            settings.gfxFigure.DrawString(settings.axisLabelX, settings.axisLabelFont, new SolidBrush(settings.axisLabelColor), xLabelPoint, settings.sfNorthWest);
            if (drawDebugRectangles)
                settings.gfxFigure.DrawRectangle(Pens.Magenta, xLabelPoint.X, xLabelPoint.Y, xLabelSize.Width, xLabelSize.Height);

            // vertical axis label
            SizeF yLabelSizF = settings.gfxFigure.MeasureString(settings.axisLabelY, settings.axisLabelFont);
            Size yLabelSize = new Size((int)yLabelSizF.Width, (int)yLabelSizF.Height);
            Point yLabelPoint = new Point(-dataCenterY - yLabelSize.Width / 2, settings.axisPadding);
            settings.gfxFigure.RotateTransform(-90);
            settings.gfxFigure.DrawString(settings.axisLabelY, settings.axisLabelFont, new SolidBrush(settings.axisLabelColor), yLabelPoint, settings.sfNorthWest);
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
            if (settings.dataSize.Width < 1 || settings.dataSize.Height < 1)
                return;

            Point tl = new Point(settings.dataOrigin.X - 1, settings.dataOrigin.Y - 1);
            Point tr = new Point(settings.dataOrigin.X + settings.dataSize.Width, settings.dataOrigin.Y - 1);
            Point bl = new Point(settings.dataOrigin.X - 1, settings.dataOrigin.Y + settings.dataSize.Height);
            Point br = new Point(settings.dataOrigin.X + settings.dataSize.Width, settings.dataOrigin.Y + settings.dataSize.Height);

            if (settings.displayAxisFrames)
            {
                Pen axisFramePen = new Pen(settings.tickColor);
                if (settings.displayFrameByAxis[0])
                    settings.gfxFigure.DrawLine(axisFramePen, tl, bl);
                if (settings.displayFrameByAxis[1])
                    settings.gfxFigure.DrawLine(axisFramePen, tr, br);
                if (settings.displayFrameByAxis[2])
                    settings.gfxFigure.DrawLine(axisFramePen, bl, br);
                if (settings.displayFrameByAxis[3])
                    settings.gfxFigure.DrawLine(axisFramePen, tl, tr);
            }
        }

        public static void Benchmark(Settings settings)
        {
            if (settings.displayBenchmark)
            {
                int debugPadding = 3;
                SizeF textSizeF = settings.gfxFigure.MeasureString(settings.benchmarkMessage, settings.benchmarkFont);
                Size textSize = new Size((int)textSizeF.Width, (int)textSizeF.Height);
                Point textLocation = new Point(settings.dataSize.Width + settings.dataOrigin.X, settings.dataSize.Height + settings.dataOrigin.Y);
                textLocation.X -= textSize.Width + debugPadding;
                textLocation.Y -= textSize.Height + debugPadding;
                Rectangle textRect = new Rectangle(textLocation, textSize);
                settings.gfxFigure.FillRectangle(settings.benchmarkBackgroundBrush, textRect);
                settings.gfxFigure.DrawRectangle(settings.benchmarkBorderPen, textRect);
                settings.gfxFigure.DrawString(settings.benchmarkMessage, settings.benchmarkFont, settings.benchmarkFontBrush, textLocation);
            }
        }
    }
}
