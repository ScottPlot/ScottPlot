using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ScottPlot.Drawing;

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
            Pen pen = GDI.Pen(settings.grid.color, settings.grid.lineWidth, settings.grid.lineStyle);

            if (settings.grid.enableVertical)
            {
                for (int i = 0; i < settings.ticks.x.tickPositionsMajor.Length; i++)
                {
                    double value = settings.ticks.x.tickPositionsMajor[i];
                    double unitsFromAxisEdge = value - settings.axes.x.min;
                    double xPx = unitsFromAxisEdge * settings.xAxisScale;

                    if (settings.grid.snapToNearestPixel)
                        xPx = (int)xPx;

                    if ((xPx == 0) && settings.layout.displayFrameByAxis[0])
                        continue; // don't draw a grid line 1px away from frame

                    PointF ptTop = new PointF((float)xPx, 0);
                    PointF ptBot = new PointF((float)xPx, settings.dataSize.Height);
                    settings.gfxData.DrawLine(pen, ptTop, ptBot);
                }
            }

            if (settings.grid.enableHorizontal)
            {
                for (int i = 0; i < settings.ticks.y.tickPositionsMajor.Length; i++)
                {
                    double value = settings.ticks.y.tickPositionsMajor[i];
                    double unitsFromAxisEdge = value - settings.axes.y.min;
                    double yPx = settings.dataSize.Height - unitsFromAxisEdge * settings.yAxisScale;

                    if (settings.grid.snapToNearestPixel)
                        yPx = (int)yPx;

                    if ((yPx == 0) && settings.layout.displayFrameByAxis[2])
                        continue; // don't draw a grid line 1px away from frame

                    PointF ptLeft = new PointF(0, (float)yPx);
                    PointF ptRight = new PointF(settings.dataSize.Width, (float)yPx);
                    settings.gfxData.DrawLine(pen, ptLeft, ptRight);
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

            bool legendHasItems = LegendTools.GetLegendItems(settings).Length > 0;
            bool legendHasLocation = settings.legend.location != legendLocation.none;

            if (legendHasItems && legendHasLocation)
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

            // increase padding between ticks and labels for OSs with tighter fonts
            float tickLabelPadding = 0;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                tickLabelPadding = 2;

            for (int i = 0; i < settings.ticks.y.tickPositionsMajor.Length; i++)
            {
                double value = settings.ticks.y.tickPositionsMajor[i];
                string text = settings.ticks.y.tickLabels[i];

                double unitsFromAxisEdge = value - settings.axes.y.min;
                double xPx = settings.dataOrigin.X - 1;
                double yPx = settings.layout.data.bottom - unitsFromAxisEdge * settings.yAxisScale;
                if ((yPx == settings.layout.data.top) && settings.layout.displayFrameByAxis[0])
                    yPx -= 1; // snap ticks to the frame edge if they are 1px away
                if (settings.ticks.snapToNearestPixel)
                    yPx = (int)yPx;

                if (settings.ticks.rulerModeY)
                {
                    settings.gfxFigure.DrawLine(pen, (float)xPx, (float)yPx, (float)xPx - settings.ticks.size - settings.ticks.font.Height, (float)yPx);
                    if (settings.ticks.displayYlabels)
                        settings.gfxFigure.DrawString(text, settings.ticks.font, brush, (float)xPx - settings.ticks.size - tickLabelPadding, (float)yPx, settings.misc.sfSouthEast);
                }
                else
                {
                    settings.gfxFigure.DrawLine(pen, (float)xPx, (float)yPx, (float)xPx - settings.ticks.size, (float)yPx);
                    if (settings.ticks.displayYlabels)
                        settings.gfxFigure.DrawString(text, settings.ticks.font, brush, (float)xPx - settings.ticks.size - tickLabelPadding, (float)yPx, settings.misc.sfEast);
                }
            }

            if (settings.ticks.displayYminor && settings.ticks.y.tickPositionsMinor != null)
            {
                foreach (var value in settings.ticks.y.tickPositionsMinor)
                {
                    double unitsFromAxisEdge = value - settings.axes.y.min;
                    double xPx = settings.dataOrigin.X - 1;
                    double yPx = settings.layout.data.bottom - unitsFromAxisEdge * settings.yAxisScale;
                    if ((yPx == settings.layout.data.top) && settings.layout.displayFrameByAxis[0])
                        yPx -= 1; // snap ticks to the frame edge if they are 1px away
                    if (settings.ticks.snapToNearestPixel)
                        yPx = (int)yPx;
                    settings.gfxFigure.DrawLine(pen, (float)xPx, (float)yPx, (float)xPx - settings.ticks.size / 2, (float)yPx);
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
                double xPx = unitsFromAxisEdge * settings.xAxisScale + settings.layout.data.left;
                double yPx = settings.layout.data.bottom;
                if ((xPx == settings.layout.data.left) && settings.layout.displayFrameByAxis[2])
                    xPx -= 1; // snap ticks to the frame edge if they are 1px away
                if (settings.ticks.snapToNearestPixel)
                    xPx = (int)xPx;

                if (settings.ticks.rulerModeX)
                {
                    settings.gfxFigure.DrawLine(pen, (float)xPx, (float)yPx, (float)xPx, (float)yPx + settings.ticks.size + settings.ticks.font.Height);
                    if (settings.ticks.displayXlabels)
                        settings.gfxFigure.DrawString(text, settings.ticks.font, brush, (float)xPx, (float)yPx + settings.ticks.size / 2, settings.misc.sfNorthWest);
                }
                else
                {
                    settings.gfxFigure.DrawLine(pen, (float)xPx, (float)yPx, (float)xPx, (float)yPx + settings.ticks.size);
                    if (settings.ticks.displayXlabels)
                    {
                        if (settings.ticks.rotationX == 0)
                        {
                            settings.gfxFigure.DrawString(text, settings.ticks.font, brush, (float)xPx, (float)yPx + settings.ticks.size, settings.misc.sfNorth);
                        }
                        else
                        {
                            double horizontalOffset = settings.ticks.fontSize * .65;
                            settings.gfxFigure.TranslateTransform((float)xPx - (float)horizontalOffset, (float)yPx + settings.ticks.size);
                            settings.gfxFigure.RotateTransform(-(float)(Math.Abs(settings.ticks.rotationX)));
                            settings.gfxFigure.DrawString(text, settings.ticks.font, brush, new PointF(0, 0), settings.misc.sfNorthEast);
                            settings.gfxFigure.ResetTransform();
                        }
                    }
                }
            }

            if (settings.ticks.displayXminor && settings.ticks.x.tickPositionsMinor != null)
            {
                foreach (var value in settings.ticks.x.tickPositionsMinor)
                {
                    double unitsFromAxisEdge = value - settings.axes.x.min;
                    double xPx = unitsFromAxisEdge * settings.xAxisScale + settings.layout.data.left;
                    double yPx = settings.layout.data.bottom;
                    if ((xPx == settings.layout.data.left) && settings.layout.displayFrameByAxis[2])
                        xPx -= 1; // snap ticks to the frame edge if they are 1px away
                    if (settings.ticks.snapToNearestPixel)
                        xPx = (int)xPx;
                    settings.gfxFigure.DrawLine(pen, (float)xPx, (float)yPx, (float)xPx, (float)yPx + settings.ticks.size / 2);
                }
            }
        }

        private static void RenderTickMultipliers(Settings settings)
        {
            Brush brush = new SolidBrush(settings.ticks.color);

            if ((settings.ticks.x.cornerLabel != "") && settings.ticks.displayXmajor)
            {
                SizeF multiplierLabelXsize = Drawing.GDI.MeasureString(settings.gfxFigure, settings.ticks.x.cornerLabel, settings.ticks.font);
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
