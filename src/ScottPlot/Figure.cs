using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Code in this file serves these tasks:
 *     figure tasks - generating the frame, adding labels, saving, etc.
 *     plotting tasks - pixel/unit conversion, drawing of data onto data bitmap
 */

namespace ScottPlot
{
    public class Figure
    {

        private Bitmap bmpData, bmpFigure;
        private Graphics gfxData, gfxFigure;
        private Settings settings;
        private Data data;
        private int renderCount;

        public Figure(Settings settings, Data data)
        {
            this.settings = settings;
            this.data = data;
        }

        public void Save(string filePath, bool renderFirst = true)
        {
            if (renderFirst)
                Render();
            filePath = System.IO.Path.GetFullPath(filePath);
            string fileFolder = System.IO.Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(fileFolder))
                System.IO.Directory.CreateDirectory(fileFolder);
            string extension = System.IO.Path.GetExtension(filePath).ToLower();
            if (extension == ".jpg")
                bmpFigure.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            else if (extension == ".png")
                bmpFigure.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            else if (extension == ".bmp")
                bmpFigure.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
            else
                throw new Exception("file format unsupported");
            Console.WriteLine($"Wrote {filePath}");
        }

        public Bitmap GetBitmap(bool render = true)
        {
            if (render)
                Render();
            return bmpFigure;
        }

        public void Render()
        {
            renderCount += 1;

            // benchmark
            System.Diagnostics.Stopwatch renderStopwatch = System.Diagnostics.Stopwatch.StartNew();

            // re-initialize bitmap and graphics objects only if needed
            if (bmpFigure == null || bmpData == null || bmpFigure.Size != settings.Size || bmpData.Size != settings.dataPlotSize)
            {
                bmpFigure = new Bitmap(settings.width, settings.height);
                gfxFigure = Graphics.FromImage(bmpFigure);
                int dataWidthPx = Math.Max(1, settings.dataPlotWidth);
                int dataHeightPx = Math.Max(1, settings.dataPlotHeight);
                bmpData = new Bitmap(dataWidthPx, dataHeightPx);
                gfxData = Graphics.FromImage(bmpData);
            }

            // clear the old images
            gfxFigure.Clear(settings.figureBgColor);
            gfxData.Clear(settings.dataBgColor);

            // set up anti-aliasing based on settings
            gfxFigure.SmoothingMode = settings.figureSmoothingMode;
            gfxFigure.TextRenderingHint = settings.figureTextHint;
            gfxData.SmoothingMode = settings.dataSmoothingMode;
            gfxData.TextRenderingHint = settings.dataTextHint;

            // draw the grid behind the objects
            RenderDataGrid();

            // draw data objects onto the data bitmap then merge it into the figure
            foreach (Plottables.PlottableThing dataObject in data.plotObjects)
            {
                if (dataObject is Plottables.Scatter)
                    RenderDataScatter((Plottables.Scatter)dataObject);
                else if (dataObject is Plottables.Signal)
                    RenderDataSignal((Plottables.Signal)dataObject);
                else if (dataObject is Plottables.AxLine)
                    RenderDataAxLine((Plottables.AxLine)dataObject);
                else
                    Console.WriteLine("I don't know how to plot this: " + dataObject.ToString());
            }
            gfxFigure.DrawImage(bmpData, settings.dataPlotOrigin);

            // draw the axis ticks and tick labels
            RenderAxes();

            // add labels
            gfxFigure.DrawString(settings.title, settings.fontTitle, settings.brushLabels, settings.dataPlotCenterX, 10, settings.sfCenter);
            gfxFigure.DrawString(settings.axisLabelX, settings.fontAxis, settings.brushLabels, settings.dataPlotCenterX, settings.height - 25, settings.sfCenter);
            gfxFigure.TranslateTransform(gfxFigure.VisibleClipBounds.Size.Width, 0);
            gfxFigure.RotateTransform(-90);
            gfxFigure.DrawString(settings.axisLabelY, settings.fontAxis, settings.brushLabels, -settings.dataPlotCenterY, -settings.width + 5, settings.sfCenter);
            gfxFigure.ResetTransform();

            // see how long that took
            double renderTimeMs = renderStopwatch.ElapsedTicks * 1000.0 / System.Diagnostics.Stopwatch.Frequency;
            double renderRateHz = System.Diagnostics.Stopwatch.Frequency / renderStopwatch.ElapsedTicks;

            // determine the point count
            int dataPointCount = 0;
            foreach (Plottables.PlottableThing dataObject in data.plotObjects)
                dataPointCount += dataObject.pointCount;

            // add debug message
            if (settings.benchmarkShow)
            {
                string debugMessage = $"rendered {dataPointCount:n0} data points in {renderTimeMs:0.00} ms";
                gfxFigure.DrawString(debugMessage, settings.fontTicks, settings.benchmarkBrush, settings.dataPlotPosLeft, settings.dataPlotPosTop, settings.sfLeft);
            }
        }

        private void RenderDataGrid()
        {
            if (settings.gridEnabled == false)
                return;

            foreach (var tick in settings.axisY.ticksMajor)
            {
                Point pt1 = new Point(0, settings.dataPlotHeight - tick.pixel);
                Point pt2 = new Point(settings.dataPlotWidth, settings.dataPlotHeight - tick.pixel);
                gfxData.DrawLine(settings.gridPen, pt1, pt2);
            }

            foreach (var tick in settings.axisX.ticksMajor)
            {
                Point pt1 = new Point(tick.pixel, 0);
                Point pt2 = new Point(tick.pixel, settings.dataPlotHeight);
                gfxData.DrawLine(settings.gridPen, pt1, pt2);
            }
        }

        private void RenderAxes()
        {
            if (settings.drawAxes == false)
                return;

            int tickSizeMajor = 3;
            int tickSizeMinor = 1;
            int tickYoffsetVert = (int)(settings.fontTicks.Size * .8);
            int tickYoffsetHoriz = (int)(settings.fontTicks.Size * .3);

            foreach (var tick in settings.axisY.ticksMajor)
            {
                int pixelY = settings.dataPlotHeight - tick.pixel + settings.dataPlotPosTop;
                Point pt1 = new Point(settings.dataPlotPosLeft, pixelY);
                Point pt2 = new Point(settings.dataPlotPosLeft - tickSizeMajor - 1, pixelY);
                Point pt3 = new Point(pt2.X, pt2.Y - tickYoffsetVert);
                gfxFigure.DrawLine(settings.penAxisTicks, pt1, pt2);
                gfxFigure.DrawString(tick.label, settings.fontTicks, settings.brushAxisTickLabels, pt3, settings.sfRight);
            }

            foreach (var tick in settings.axisY.ticksMinor)
            {
                int pixelY = settings.dataPlotHeight - tick.pixel + settings.dataPlotPosTop;
                Point pt1 = new Point(settings.dataPlotPosLeft, pixelY);
                Point pt2 = new Point(settings.dataPlotPosLeft - tickSizeMinor - 1, pixelY);
                gfxFigure.DrawLine(settings.penAxisTicks, pt1, pt2);
            }

            foreach (var tick in settings.axisX.ticksMajor)
            {
                int pixelX = tick.pixel + settings.dataPlotPosLeft;
                Point pt1 = new Point(pixelX, settings.dataPlotPosBottom);
                Point pt2 = new Point(pixelX, settings.dataPlotPosBottom + 1 + tickSizeMajor);
                Point pt3 = new Point(pt2.X, pt2.Y + tickYoffsetHoriz);
                gfxFigure.DrawLine(settings.penAxisTicks, pt1, pt2);
                gfxFigure.DrawString(tick.label, settings.fontTicks, settings.brushAxisTickLabels, pt3, settings.sfCenter);
            }

            foreach (var tick in settings.axisX.ticksMinor)
            {
                int pixelX = tick.pixel + settings.dataPlotPosLeft;
                Point pt1 = new Point(pixelX, settings.dataPlotPosBottom);
                Point pt2 = new Point(pixelX, settings.dataPlotPosBottom + 1 + tickSizeMinor);
                gfxFigure.DrawLine(settings.penAxisTicks, pt1, pt2);
            }

            gfxFigure.DrawRectangle(settings.penAxisBorder, settings.dataPlotRectangle);

        }

        private class PixelColumn
        {
            public int min = 10;
            public int max = 20;
            public int iLeft = 0;
            public int iRight = 0;

            public PixelColumn(int min, int max, int iLeft, int iRight)
            {
                this.min = min;
                this.max = max;
                this.iLeft = iLeft;
                this.iRight = iRight;
            }
        }

        private double ArraySubMin(double[] data, int iLeft, int iRight)
        {
            double val = data[iLeft];
            for (int i = iLeft; i < iRight; i++)
                if (data[i] < val)
                    val = data[i];
            return val;
        }

        private double ArraySubMax(double[] data, int iLeft, int iRight)
        {
            double val = data[iLeft];
            for (int i = iLeft; i < iRight; i++)
                if (data[i] > val)
                    val = data[i];
            return val;
        }

        private PixelColumn[] SignalToPixelConverter(Plottables.Signal signal, Settings settings)
        {
            PixelColumn[] pxCols = new PixelColumn[settings.dataPlotWidth];

            // theoretical minimum pixel if X1 were drawn
            int dataMinPx = (int)(settings.axisX.pxPerUnit * (-settings.axisX.x1));

            // step column by column left to right
            for (int i = 0; i < settings.dataPlotWidth; i++)
            {
                // determine what index values of the signal correspond to this pixel column
                int iLeft = (int)(settings.axisX.unitsPerPx * signal.sampleRateHz * (i - dataMinPx));
                int iRight = (int)(iLeft + settings.axisX.unitsPerPx * signal.sampleRateHz);

                // ensure indexes are valid and skip columns without data
                iLeft = Math.Max(iLeft, 0);
                iRight = Math.Min(signal.ys.Length - 1, iRight);
                iRight = Math.Max(iRight, 0);
                if (iRight == 0)
                    continue;
                if (iRight < 0 || iLeft > iRight)
                    continue;

                // pull iLeft of this column to the same iRight as the last
                if (i > 0 && pxCols[i - 1] != null)
                    iLeft = pxCols[i - 1].iRight - 1;

                // determine vertical span of this range
                double valMin = ArraySubMin(signal.ys, iLeft, iRight);
                double valMax = ArraySubMax(signal.ys, iLeft, iRight);

                // TODO: make indexes perfect - they don't always match-up exactly
                //Console.WriteLine($"Index [{iLeft}:{iRight}] min/max = {valMin}/{valMax}");

                // convert this value to a pixel location on screen
                int pxMin = (int)((settings.axisY.x2 - valMin) * settings.axisY.pxPerUnit) + 1;
                int pxMax = (int)((settings.axisY.x2 - valMax) * settings.axisY.pxPerUnit) + 1;

                // populate the object
                pxCols[i] = new PixelColumn(pxMin, pxMax, iLeft, iRight);
            }

            return pxCols;
        }

        private void RenderDataSignal(Plottables.Signal signal)
        {

            // use the signal processing class to convert dense data to pixel column fills
            PixelColumn[] pxCols = SignalToPixelConverter(signal, settings);
            Pen linePen = new Pen(signal.style.lineColor);
            Brush brushPen = new SolidBrush(signal.style.lineColor);

            // disable anti-aliasing so art looks better
            var originalSmoothMode = gfxData.SmoothingMode;
            gfxData.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            // for zoomed-in data, only individual points will be plotted
            var points = new List<Point>();

            // plot one column at a time
            int skippedColumns = 0;
            for (int i = 0; i < pxCols.Length; i++)
            {
                if (pxCols[i] == null)
                {
                    // this column contains no data
                    skippedColumns += 1;
                }
                else if (pxCols[i].min == pxCols[i].max)
                {
                    // this column contains a single pixel of data
                    gfxData.FillRectangle(brushPen, i, pxCols[i].min, 1, 1);
                }
                else
                {
                    // this column contains a span of data
                    Point pt1 = new Point(i, pxCols[i].min);
                    Point pt2 = new Point(i, pxCols[i].max);
                    gfxData.DrawLine(linePen, pt1, pt2);
                }

                // if this is a new data point, remember it
                if (i > 0 && pxCols[i] != null && pxCols[i - 1] != null && pxCols[i].iLeft > pxCols[i - 1].iLeft)
                    points.Add(new Point(i, pxCols[i].min));
            }

            // draw markers at each individual point if zoomed-in6
            if ((points.Count() + skippedColumns) < settings.dataPlotWidth / 3)
            {
                int ptSize = 2;
                foreach (var point in points)
                    gfxData.FillEllipse(brushPen, point.X - ptSize, point.Y - ptSize, 2 * ptSize, 2 * ptSize);
                //gfxData.DrawLines(linePen, points.ToArray());
            }

            // revert to how things were
            gfxData.SmoothingMode = originalSmoothMode;
        }

        private void RenderDataScatter(Plottables.Scatter scatter)
        {
            // convert XY pairs to an array of points scaled to pixel units
            Point[] points = new Point[scatter.xs.Length];
            for (int i = 0; i < scatter.xs.Length; i++)
            {
                int pixelX = settings.axisX.UnitToPixel(scatter.xs[i]);
                int pixelY = settings.axisY.UnitToPixel(scatter.ys[i]);
                pixelY = settings.dataPlotHeight - pixelY;
                points[i] = new Point(pixelX, pixelY);
            }

            // draw lines
            if (points.Length > 1 && scatter.style.lineWidth > 0)
            {
                Pen linePen = new Pen(scatter.style.lineColor, scatter.style.lineWidth);
                linePen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                linePen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                linePen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                try
                {
                    gfxData.DrawLines(linePen, points);
                }
                catch
                {
                    Console.WriteLine("Crashed drawing lines");
                }
                
            }

            // draw markers
            int markerSize = scatter.style.markerSize;
            var markerPen = new Pen(scatter.style.markerColor);
            Brush markerBrush = new SolidBrush(scatter.style.markerColor);
            if (markerSize > 0)
            {
                try
                {
                    foreach (Point point in points)
                    {
                        Rectangle rect = new Rectangle(point.X - markerSize, point.Y - markerSize, markerSize * 2, markerSize * 2);
                        if (scatter.style.markerShape == Style.MarkerShape.circleFilled)
                            gfxData.FillEllipse(markerBrush, rect);
                        else if (scatter.style.markerShape == Style.MarkerShape.circleOpen)
                            gfxData.DrawEllipse(markerPen, rect);
                        else
                            Console.WriteLine("UNKNOWN SHAPE: {scatter.style.markerShape}");
                    }
                }
                catch
                {
                    Console.WriteLine($"Crashed drawing markers");
                }
            }
        }

        private void RenderDataAxLine(Plottables.AxLine axLine)
        {
            Pen linePen = new Pen(axLine.style.lineColor, axLine.style.lineWidth);
            if (axLine.vertical == true)
            {
                int x = settings.axisX.UnitToPixel(axLine.position);
                try
                {
                    gfxData.DrawLine(linePen, x, 0, x, settings.dataPlotHeight);
                }
                catch
                {
                    Console.WriteLine($"Crashed making a vertical line at: {x}");
                }
            }
            else
            {
                int y = settings.axisY.UnitToPixel(axLine.position);
                y = settings.dataPlotHeight - y;
                try
                {
                    gfxData.DrawLine(linePen, 0, y, settings.dataPlotWidth, y);
                }
                catch
                {
                    Console.WriteLine($"Crashed making a horizontal line at: {y}");
                }
            }
        }
    }
}