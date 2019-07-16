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
            }
            legendItems.Reverse();

            // draw the frame and border
            float frameWidth = padding * 2 + legendFontMaxWidth + padding + stubWidth;
            float frameHeight = padding * 2 + legendFontLineHeight * legendItems.Count();
            Size frameSize = new Size((int)frameWidth, (int)frameHeight);

            Point frameLoc = new Point((int)(settings.DataSize.Width - frameWidth - padding),
                 (int)(settings.DataSize.Height - frameHeight - padding));

            if (settings.legendAvailablePositions.Contains(settings.legendPosition))
            {
                if (settings.legendPosition is "BottomRight")
                {
                    frameLoc.X = (int)(settings.DataSize.Width - frameWidth - padding);
                    frameLoc.Y = (int)(settings.DataSize.Height - frameHeight - padding);
                }
                else if (settings.legendPosition is "TopLeft")
                {
                    frameLoc.X = (int)(padding);
                    frameLoc.Y = (int)(padding);
                }
                else if (settings.legendPosition is "BottomLeft")
                {
                    frameLoc.X = (int)(padding);
                    frameLoc.Y = (int)(settings.DataSize.Height - frameHeight - padding);
                }
                else if (settings.legendPosition is "TopRight")
                {
                    frameLoc.X = (int)(settings.DataSize.Width - frameWidth - padding);
                    frameLoc.Y = (int)(padding);
                }
            }


            Rectangle frameRect = new Rectangle(frameLoc, frameSize);
            if (settings.legendAvailablePositions.Contains(settings.legendPosition))
            {
                settings.gfxData.FillRectangle(new SolidBrush(settings.legendBackColor), frameRect);
                settings.gfxData.DrawRectangle(new Pen(settings.legendFrameColor), frameRect);
            }


            // draw the individual labels
            Point textLocation = new Point(settings.DataSize.Width, settings.DataSize.Height);
            if (settings.legendAvailablePositions.Contains(settings.legendPosition))
            {
                if (settings.legendPosition is "BottomRight")
                {
                    textLocation.X = (int)(settings.DataSize.Width-(legendFontMaxWidth + padding));
                    textLocation.Y = settings.DataSize.Height - padding*2;
                }
                else if (settings.legendPosition is "TopLeft")
                {
                    textLocation.X = (int)(frameWidth - legendFontMaxWidth + padding );
                    textLocation.Y = (int)(frameHeight);
                }
                else if (settings.legendPosition is "BottomLeft")
                {
                    textLocation.X = (int)(frameWidth - legendFontMaxWidth + padding);
                    textLocation.Y = settings.DataSize.Height - padding*2;
                }
                else if (settings.legendPosition is "TopRight")
                {
                    textLocation.X = (int)(settings.DataSize.Width - (legendFontMaxWidth + padding));
                    textLocation.Y = (int)(frameHeight );
                }
            }
            else
            {
                textLocation.X = (int)(settings.DataSize.Width - (legendFontMaxWidth + padding));
                textLocation.Y = settings.DataSize.Height - padding;
            }

            if (settings.legendAvailablePositions.Contains(settings.legendPosition))
            {

                for (int i = 0; i < legendItems.Count; i++)
                {
                    //textLocation.X -= (int)legendFontMaxWidth + padding * 2;
                    textLocation.Y -= (int)(legendFontLineHeight  );

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

            settings.gfxFigure.DrawImage(settings.bmpData, settings.DataOrigin);
        }

        public static void FigureLabels(Settings settings, bool drawDebugRectangles = false)
        {
            if (settings.gfxFigure == null)
                return;

            int dataCenterX = settings.DataSize.Width / 2 + settings.DataOrigin.X;
            int dataCenterY = settings.DataSize.Height / 2 + settings.DataOrigin.Y;

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
            Point xLabelPoint = new Point(dataCenterX - xLabelSize.Width / 2, settings.FigureSize.Height - settings.axisPadding - xLabelSize.Height);
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
            if (settings.DataSize.Width < 1 || settings.DataSize.Height < 1)
                return;

            Ticks ticks = new Ticks(settings);
            ticks.RenderTicks();
        }

        public static void FigureFrames(Settings settings)
        {
            if (settings.DataSize.Width < 1 || settings.DataSize.Height < 1)
                return;

            Point tl = new Point(settings.DataOrigin.X - 1, settings.DataOrigin.Y - 1);
            Point tr = new Point(settings.DataOrigin.X + settings.DataSize.Width, settings.DataOrigin.Y - 1);
            Point bl = new Point(settings.DataOrigin.X - 1, settings.DataOrigin.Y + settings.DataSize.Height);
            Point br = new Point(settings.DataOrigin.X + settings.DataSize.Width, settings.DataOrigin.Y + settings.DataSize.Height);

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
                Point textLocation = new Point(settings.DataSize.Width + settings.DataOrigin.X, settings.DataSize.Height + settings.DataOrigin.Y);
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
