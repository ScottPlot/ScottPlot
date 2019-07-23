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
            if (settings.gfxFigure != null)
                settings.gfxFigure.Clear(settings.figureBackgroundColor);
        }

        public static void DataBackground(Settings settings)
        {
            if (settings.gfxData != null)
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

            if (settings.legendLocation == legendLocation.none)
                return;

            int padding = 3;
            int stubWidth = 20;
            int stubHeight = 3;

            Brush brushText = new SolidBrush(settings.legendFontColor);
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
                else
                {
                    legendItems.Add(new LegendItem("", i, settings.plottables[i].color));
                    float thisItemFontWidth = settings.gfxData.MeasureString(settings.plottables[i].label, settings.legendFont).Width;
                    if (thisItemFontWidth > legendFontMaxWidth)
                        legendFontMaxWidth = thisItemFontWidth;

                }
            }
            legendItems.Reverse();

            // figure out where the legend should be
            float frameWidth = padding * 2 + legendFontMaxWidth + padding + stubWidth;
            float frameHeight = padding * 2 + legendFontLineHeight * legendItems.Count();
            Size frameSize = new Size((int)frameWidth, (int)frameHeight);
            Point frameLocation = new Point((int)(settings.dataSize.Width - frameWidth - padding),
                 (int)(settings.dataSize.Height - frameHeight - padding));
            Point textLocation = new Point(settings.dataSize.Width, settings.dataSize.Height);
            switch (settings.legendLocation)
            {
                case (legendLocation.none):
                    break;
                case (legendLocation.lowerRight):
                    frameLocation.X = (int)(settings.dataSize.Width - frameWidth - padding);
                    frameLocation.Y = (int)(settings.dataSize.Height - frameHeight - padding);
                    textLocation.X = (int)(settings.dataSize.Width - (legendFontMaxWidth + padding));
                    textLocation.Y = settings.dataSize.Height - padding * 2;
                    break;
                case (legendLocation.upperLeft):
                    frameLocation.X = (int)(padding);
                    frameLocation.Y = (int)(padding);
                    textLocation.X = (int)(frameWidth - legendFontMaxWidth + padding);
                    textLocation.Y = (int)(frameHeight);
                    break;
                case (legendLocation.lowerLeft):
                    frameLocation.X = (int)(padding);
                    frameLocation.Y = (int)(settings.dataSize.Height - frameHeight - padding);
                    textLocation.X = (int)(frameWidth - legendFontMaxWidth + padding);
                    textLocation.Y = settings.dataSize.Height - padding * 2;
                    break;
                case (legendLocation.upperRight):
                    frameLocation.X = (int)(settings.dataSize.Width - frameWidth - padding);
                    frameLocation.Y = (int)(padding);
                    textLocation.X = (int)(settings.dataSize.Width - (legendFontMaxWidth + padding));
                    textLocation.Y = (int)(frameHeight);
                    break;
                case (legendLocation.upperCenter):
                    frameLocation.X = (int)((settings.dataSize.Width) / 2 - frameWidth / 2 - padding * 5);
                    frameLocation.Y = (int)(padding);
                    textLocation.X = (int)(settings.dataSize.Width / 2 - legendFontMaxWidth / 2 + padding / 2);
                    textLocation.Y = (int)(frameHeight);
                    break;
                case (legendLocation.lowerCenter):
                    frameLocation.X = (int)((settings.dataSize.Width) / 2 - frameWidth / 2 - padding * 5);
                    frameLocation.Y = (int)(settings.dataSize.Height - frameHeight - padding);
                    textLocation.X = (int)(settings.dataSize.Width / 2 - legendFontMaxWidth / 2 + padding / 2);
                    textLocation.Y = settings.dataSize.Height - padding * 2;
                    break;
                case (legendLocation.middleLeft):
                    frameLocation.X = (int)(padding);
                    frameLocation.Y = (int)(settings.dataSize.Height / 2 - frameHeight / 2 - padding);
                    textLocation.X = (int)(frameWidth - legendFontMaxWidth + padding);
                    textLocation.Y = (int)(settings.dataSize.Height / 2 + frameHeight / 2 - padding * 2);
                    break;
                case (legendLocation.middleRight):
                    frameLocation.X = (int)(settings.dataSize.Width - frameWidth - padding);
                    frameLocation.Y = (int)(settings.dataSize.Height / 2 - frameHeight / 2 - padding);
                    textLocation.X = (int)(settings.dataSize.Width - (legendFontMaxWidth + padding));
                    textLocation.Y = (int)(settings.dataSize.Height / 2 + frameHeight / 2 - padding * 2);
                    break;
                default:
                    throw new NotImplementedException($"legend location {settings.legendLocation} is not supported");
            }
            Rectangle frameRect = new Rectangle(frameLocation, frameSize);

            // figure out where the legend should be
            Point shadowLocation = new Point();
            switch (settings.legendShadowDirection)
            {
                case (shadowDirection.lowerRight):
                    shadowLocation.X = frameRect.X + 2;
                    shadowLocation.Y = frameRect.Y + 2;
                    break;
                case (shadowDirection.lowerLeft):
                    shadowLocation.X = frameRect.X - 2;
                    shadowLocation.Y = frameRect.Y + 2;
                    break;
                case (shadowDirection.upperRight):
                    shadowLocation.X = frameRect.X + 2;
                    shadowLocation.Y = frameRect.Y - 2;
                    break;
                case (shadowDirection.upperLeft):
                    shadowLocation.X = frameRect.X - 2;
                    shadowLocation.Y = frameRect.Y - 2;
                    break;
                default:
                    settings.legendShadowDirection = shadowDirection.none;
                    break;
            }
            Rectangle shadowRect = new Rectangle(shadowLocation, frameSize);

            // draw the legend background

            if (settings.legendLocation != legendLocation.none)
            {
                if (settings.legendShadowDirection != shadowDirection.none)
                    settings.gfxData.FillRectangle(new SolidBrush(settings.legendShadowColor), shadowRect);
                settings.gfxData.FillRectangle(new SolidBrush(settings.legendBackColor), frameRect);
                settings.gfxData.DrawRectangle(new Pen(settings.legendFrameColor), frameRect);
            }

            if (settings.legendLocation != legendLocation.none)
            {
                for (int i = 0; i < legendItems.Count; i++)
                {
                    textLocation.Y -= (int)(legendFontLineHeight);

                    settings.gfxData.DrawString(legendItems[i].label, settings.legendFont, brushText, textLocation);
                    settings.gfxData.DrawLine(new Pen(legendItems[i].color, stubHeight),

                    textLocation.X - padding, textLocation.Y + legendFontLineHeight / 2,
                    textLocation.X - padding - stubWidth, textLocation.Y + legendFontLineHeight / 2);
                }
            }
        }

        public static void DataPlaceOntoFigure(Settings settings)
        {
            if (settings.gfxFigure == null || settings.bmpData == null)
                return;

            settings.gfxFigure.DrawImage(settings.bmpData, settings.dataOrigin);
        }

        public static void FigureLabels(Settings settings, bool drawDebugRectangles = false)
        {
            if (settings.gfxFigure == null)
                return;

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
