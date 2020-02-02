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
                settings.gfxFigure.Clear(settings.misc.figureBackgroundColor);
        }

        public static void DataBackground(Settings settings)
        {
            if (settings.gfxData != null)
                settings.gfxData.Clear(settings.misc.dataBackgroundColor);
        }

        public static void DataGrid(Settings settings)
        {
            Pen pen = new Pen(settings.grid.color);

            if (settings.grid.enableVertical)
            {
                for (int i = 0; i < settings.ticks.x.tickPositionsMajor.Length; i++)
                {
                    double value = settings.ticks.x.tickPositionsMajor[i];
                    double unitsFromAxisEdge = value - settings.axes.x.min;
                    int xPx = (int)(unitsFromAxisEdge * settings.xAxisScale);
                    settings.gfxData.DrawLine(pen, xPx, 0, xPx, settings.dataSize.Height);
                }
            }

            if (settings.grid.enableHorizontal)
            {
                for (int i = 0; i < settings.ticks.y.tickPositionsMajor.Length; i++)
                {
                    double value = settings.ticks.y.tickPositionsMajor[i];
                    double unitsFromAxisEdge = value - settings.axes.y.min;
                    int yPx = settings.dataSize.Height - (int)(unitsFromAxisEdge * settings.yAxisScale);
                    settings.gfxData.DrawLine(pen, 0, yPx, settings.dataSize.Width, yPx);
                }
            }
        }

        public static void DataPlottables(Settings settings)
        {
            if (settings.gfxData == null)
                return;

            for (int i = 0; i < settings.plottables.Count; i++)
            {
                Plottable pltThing = settings.plottables[i];
                if (pltThing.visible)
                {
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
        }

        public static void CreateLegendBitmap(Settings settings)
        {
            LegendTools.DrawLegend(settings);
        }

        public static void PlaceLegendOntoFigure(Settings settings)
        {
            if (settings.gfxFigure == null || settings.gfxLegend == null)
                return;

            int plottablesShownInLegend = 0;
            foreach (var plottable in settings.plottables)
                if (plottable.visible && plottable.label != null)
                    plottablesShownInLegend += 1;

            if (plottablesShownInLegend > 0 && settings.legend.location != ScottPlot.legendLocation.none)
            {
                Point legendLocation = new Point(settings.dataOrigin.X + settings.legend.rect.Location.X,
                settings.dataOrigin.Y + settings.legend.rect.Location.Y);
                settings.gfxFigure.DrawImage(settings.bmpLegend, legendLocation);
            }
        }

        public static void PlaceDataOntoFigure(Settings settings)
        {
            if (settings.gfxFigure == null || settings.bmpData == null)
                return;

            settings.gfxFigure.DrawImage(settings.bmpData, settings.dataOrigin);
        }

        public static void FigureLabels(Settings settings)
        {
            if (settings.gfxFigure == null)
                return;

            int dataCenterX = settings.dataSize.Width / 2 + settings.dataOrigin.X;
            int dataCenterY = settings.dataSize.Height / 2 + settings.dataOrigin.Y;

            if (settings.title.visible)
            {
                settings.gfxFigure.DrawString(
                        settings.title.text,
                        settings.title.font,
                        new SolidBrush(settings.title.color),
                        settings.layout.title.Center,
                        settings.misc.sfCenterCenter
                    );
            }

            if (settings.xLabel.visible)
            {
                settings.gfxFigure.DrawString(
                        settings.xLabel.text,
                        settings.xLabel.font,
                        new SolidBrush(settings.xLabel.color),
                        settings.layout.xLabel.Center,
                        settings.misc.sfCenterCenter
                    );
            }

            if (settings.yLabel.visible)
            {
                Point originalLocation = settings.layout.yLabel.Center;
                Point rotatedLocation = new Point(-originalLocation.Y, settings.layout.yLabel.Width - originalLocation.X);
                settings.gfxFigure.RotateTransform(-90);
                settings.gfxFigure.DrawString(
                        settings.yLabel.text,
                        settings.yLabel.font,
                        new SolidBrush(settings.yLabel.color),
                        rotatedLocation,
                        settings.misc.sfCenterCenter
                    );
                settings.gfxFigure.ResetTransform();
            }
        }

        public static void FigureTicks(Settings settings)
        {
            if (settings.dataSize.Width < 1 || settings.dataSize.Height < 1)
                return;

            settings.ticks.x.Recalculate(settings);
            settings.ticks.y.Recalculate(settings);

            RenderTicksOnLeft(settings);
            RenderTicksOnBottom(settings);
            RenderTickMultipliers(settings);
        }

        public static void FigureFrames(Settings settings)
        {
            if (settings.dataSize.Width < 1 || settings.dataSize.Height < 1)
                return;

            Point tl = new Point(settings.dataOrigin.X - 1, settings.dataOrigin.Y - 1);
            Point tr = new Point(settings.dataOrigin.X + settings.dataSize.Width, settings.dataOrigin.Y - 1);
            Point bl = new Point(settings.dataOrigin.X - 1, settings.dataOrigin.Y + settings.dataSize.Height);
            Point br = new Point(settings.dataOrigin.X + settings.dataSize.Width, settings.dataOrigin.Y + settings.dataSize.Height);

            if (settings.layout.displayAxisFrames)
            {
                Pen axisFramePen = new Pen(settings.ticks.color);
                if (settings.layout.displayFrameByAxis[0])
                    settings.gfxFigure.DrawLine(axisFramePen, tl, bl);
                if (settings.layout.displayFrameByAxis[1])
                    settings.gfxFigure.DrawLine(axisFramePen, tr, br);
                if (settings.layout.displayFrameByAxis[2])
                    settings.gfxFigure.DrawLine(axisFramePen, bl, br);
                if (settings.layout.displayFrameByAxis[3])
                    settings.gfxFigure.DrawLine(axisFramePen, tl, tr);
            }
        }

        public static void Benchmark(Settings settings)
        {
            if (settings.benchmark.visible)
            {
                int debugPadding = 3;
                PointF textLocation = new PointF(settings.dataSize.Width + settings.dataOrigin.X, settings.dataSize.Height + settings.dataOrigin.Y);
                textLocation.X -= settings.benchmark.width + debugPadding;
                textLocation.Y -= settings.benchmark.height + debugPadding;
                RectangleF textRect = new RectangleF(textLocation, settings.benchmark.size);
                settings.gfxFigure.FillRectangle(new SolidBrush(settings.benchmark.colorBackground), textRect);
                settings.gfxFigure.DrawRectangle(new Pen(settings.benchmark.colorBorder), Rectangle.Round(textRect));
                settings.gfxFigure.DrawString(settings.benchmark.text, settings.benchmark.font, new SolidBrush(settings.benchmark.color), textLocation);
            }
        }

        public static void RenderTicksOnLeft(Settings settings)
        {
            if (!settings.ticks.displayYmajor)
                return;

            Pen pen = new Pen(settings.ticks.color);
            Brush brush = new SolidBrush(settings.ticks.color);

            for (int i = 0; i < settings.ticks.y.tickPositionsMajor.Length; i++)
            {
                double value = settings.ticks.y.tickPositionsMajor[i];
                string text = settings.ticks.y.tickLabels[i];

                double unitsFromAxisEdge = value - settings.axes.y.min;
                int xPx = settings.dataOrigin.X - 1;
                int yPx = settings.layout.data.bottom - (int)(unitsFromAxisEdge * settings.yAxisScale);

                if (settings.ticks.rulerModeY)
                {
                    settings.gfxFigure.DrawLine(pen, xPx, yPx, xPx - settings.ticks.size - settings.ticks.font.Height, yPx);
                    if (settings.ticks.displayYlabels)
                        settings.gfxFigure.DrawString(text, settings.ticks.font, brush, xPx - settings.ticks.size, yPx, settings.misc.sfSouthEast);
                }
                else
                {
                    settings.gfxFigure.DrawLine(pen, xPx, yPx, xPx - settings.ticks.size, yPx);
                    if (settings.ticks.displayYlabels)
                        settings.gfxFigure.DrawString(text, settings.ticks.font, brush, xPx - settings.ticks.size, yPx, settings.misc.sfEast);
                }
            }

            if (settings.ticks.displayYminor && settings.ticks.y.tickPositionsMinor != null)
            {
                foreach (var value in settings.ticks.y.tickPositionsMinor)
                {
                    double unitsFromAxisEdge = value - settings.axes.y.min;
                    int xPx = settings.dataOrigin.X - 1;
                    int yPx = settings.layout.data.bottom - (int)(unitsFromAxisEdge * settings.yAxisScale);
                    settings.gfxFigure.DrawLine(pen, xPx, yPx, xPx - settings.ticks.size / 2, yPx);
                }
            }

        }

        public static void RenderTicksOnBottom(Settings settings)
        {
            if (!settings.ticks.displayXmajor)
                return;

            Pen pen = new Pen(settings.ticks.color);
            Brush brush = new SolidBrush(settings.ticks.color);

            for (int i = 0; i < settings.ticks.x.tickPositionsMajor.Length; i++)
            {
                double value = settings.ticks.x.tickPositionsMajor[i];
                string text = settings.ticks.x.tickLabels[i];

                double unitsFromAxisEdge = value - settings.axes.x.min;
                int xPx = (int)(unitsFromAxisEdge * settings.xAxisScale) + settings.layout.data.left;
                int yPx = settings.layout.data.bottom;

                if (settings.ticks.rulerModeX)
                {
                    settings.gfxFigure.DrawLine(pen, xPx, yPx, xPx, yPx + settings.ticks.size + settings.ticks.font.Height);
                    if (settings.ticks.displayXlabels)
                        settings.gfxFigure.DrawString(text, settings.ticks.font, brush, xPx, yPx + settings.ticks.size, settings.misc.sfNorthWest);
                }
                else
                {
                    settings.gfxFigure.DrawLine(pen, xPx, yPx, xPx, yPx + settings.ticks.size);
                    if (settings.ticks.displayXlabels)
                        settings.gfxFigure.DrawString(text, settings.ticks.font, brush, xPx, yPx + settings.ticks.size, settings.misc.sfNorth);
                }
            }

            if (settings.ticks.displayXminor && settings.ticks.x.tickPositionsMinor != null)
            {
                foreach (var value in settings.ticks.x.tickPositionsMinor)
                {
                    double unitsFromAxisEdge = value - settings.axes.x.min;
                    int xPx = (int)(unitsFromAxisEdge * settings.xAxisScale) + settings.layout.data.left;
                    int yPx = settings.layout.data.bottom;
                    settings.gfxFigure.DrawLine(pen, xPx, yPx, xPx, yPx + settings.ticks.size / 2);
                }
            }
        }

        private static void RenderTickMultipliers(Settings settings)
        {
            Brush brush = new SolidBrush(settings.ticks.color);

            if ((settings.ticks.x.cornerLabel != "") && settings.ticks.displayXmajor)
            {
                SizeF multiplierLabelXsize = settings.gfxFigure.MeasureString(settings.ticks.x.cornerLabel, settings.ticks.font);
                settings.gfxFigure.DrawString(settings.ticks.x.cornerLabel, settings.ticks.font, brush,
                    settings.dataOrigin.X + settings.dataSize.Width,
                    settings.dataOrigin.Y + settings.dataSize.Height + multiplierLabelXsize.Height,
                    settings.misc.sfNorthEast);
            }

            if ((settings.ticks.y.cornerLabel != "") && settings.ticks.displayYmajor)
            {
                settings.gfxFigure.DrawString(settings.ticks.y.cornerLabel, settings.ticks.font, brush,
                    settings.dataOrigin.X,
                    settings.dataOrigin.Y,
                    settings.misc.sfSouthWest);
            }
        }

        public static void MouseZoomRectangle(Settings settings)
        {
            if (settings.mouseMiddleRect != null)
            {
                Pen outline = new Pen(Color.FromArgb(100, 255, 0, 0));
                Brush fill = new SolidBrush(Color.FromArgb(50, 255, 0, 0));
                settings.gfxData.DrawRectangle(outline, (Rectangle)settings.mouseMiddleRect);
                settings.gfxData.FillRectangle(fill, (Rectangle)settings.mouseMiddleRect);
            }
        }
    }
}
