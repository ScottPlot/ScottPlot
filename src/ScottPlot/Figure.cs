using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

/* ScottPlot is a class library intended to make it easy to graph large datasets in high speed.
 * 
 * Although features like mouse click-and-drag to zoom and pan are included for easy interactive GUI
 * integration, ScottPlot can be run entirely within console applications as well.
 * 
 * KEY TERMS:
 *      Figure - a Figure object is mostly what the user will interact with. It contains a Frame and a Graph.
 *      Frame - the frame is everything behind the data (the axis labels, grid lines, tick marks, etc).
 *      Graph - the part of the frame which gets drawn on when graphs are plotted.
 *      Axis - information about a single dimension (X vs Y) including the current min/max and pixel scaling.
 *  
 * THEORY OF OPERATION / USE OVERVIEW:
 *      * Create a Figure (telling it the size of the image)
 *      * Resize() can change the size in the future
 *      * Set colors as desired
 *      * Set the axis labels and title as desired
 *      * Zoom() and Pan() can be used to adjust window
 *      * Adjust the axis limits to window the data you wish to show
 *      * RedrawFrame() and now you are ready to add data
 *          * ClearGraph() to start a new data plot (erasing the last one)
 *          * Plot() methods accumulate drawings on the plot
 *      * Access the assembled image at any time with Render()
 *
 */
namespace ScottPlot
{
    
    // contains the frame and the graph!
    public class Figure
    {
        private Point graphPos = new Point(0, 0);

        private Bitmap bmpFrame;
        private Graphics gfxFrame;
        
        private Bitmap bmpGraph { get; set; }
        public Graphics gfxGraph;
        public Axis xAxis = new Axis(-10, 10, 100, false);
        public Axis yAxis = new Axis(-10, 10, 100, true);

        public Color colorFigBg;
        public Color colorGraphBg;
        public Color colorAxis;
        public Color colorGridLines;

        

        // the user can set these
        const string font = "Arial";
        Font fontTicks = new Font(font, 9, FontStyle.Regular);
        Font fontTitle = new Font(font, 20, FontStyle.Bold);
        Font fontAxis = new Font(font, 12, FontStyle.Bold);

        public string labelY = "";
        public string labelX = "";
        public string labelTitle = "";

        private int padL = 50, padT = 47, padR = 50, padB = 47;

        private System.Diagnostics.Stopwatch stopwatch;
        public DataGen gen = new DataGen(); // for easy access

        // A figure object contains what's needed to draw scale bars and axis labels around a graph.
        // The graph itself is its own object which lives inside the figure.
        public Figure(int width, int height)
        {
            stopwatch = System.Diagnostics.Stopwatch.StartNew();
            stopwatch.Stop();
            stopwatch.Reset();

            styleWeb();
            Resize(width, height);

            // default to anti-aliasing on
            gfxGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfxFrame.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfxFrame.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            FrameRedraw();
            GraphClear();
        }
        
        /// <summary>
        /// Resize the entire figure (in pixels)
        /// </summary>
        public void Resize(int width, int height)
        {
            // sanity check (make sure the graph area is at least 1px by 1px
            if (width - padL - padR < 1) width = padL + padR + 1;
            if (height - padT - padB < 1) height = padT + padB + 1;

            // figure resized, so resize the frame bitmap
            bmpFrame = new Bitmap(width, height);
            gfxFrame = Graphics.FromImage(bmpFrame);
            
            // now re-calculate the graph size based on the padding
            FramePad(null, null, null, null);

            // now resize the graph bitmap
            bmpGraph = new Bitmap(bmpFrame.Width - padL - padR, bmpFrame.Height - padT - padB);
            gfxGraph = Graphics.FromImage(bmpGraph);

            // now resize axis to the new pad dimensions
            xAxis.Resize(bmpGraph.Width);
            yAxis.Resize(bmpGraph.Height);
        }
        
        /// <summary>
        /// Change the padding between the edge of the graph and edge of the figure
        /// </summary>
        public void FramePad(int? left, int? right, int? top, int? bottom)
        {
            if (left != null) padL = (int)left;
            if (right != null) padR = (int)right;
            if (top != null) padT = (int)top;
            if (bottom != null) padB = (int)bottom;
            graphPos = new Point(padL, padT);
        }

        /// <summary>
        /// Clear the frame and redraw it from scratch.
        /// </summary>
        public void FrameRedraw()
        {
            
            gfxFrame.Clear(colorFigBg);

            // prepare things useful for drawing
            Pen penAxis = new Pen(new SolidBrush(colorAxis));
            Pen penGrid = new Pen(colorGridLines) { DashPattern = new float[] { 4, 4 } };
            Brush brush = new SolidBrush(colorAxis);
            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            StringFormat sfRight = new StringFormat();
            sfRight.Alignment = StringAlignment.Far;
            int posB = bmpGraph.Height + padT;
            int posCx = bmpGraph.Width / 2 + padL;
            int posCy = bmpGraph.Height / 2 + padT;

            int tick_size_minor = 2;
            int tick_size_major = 5;

            // draw the data rectangle and ticks
            gfxFrame.DrawRectangle(penAxis, graphPos.X - 1, graphPos.Y - 1, bmpGraph.Width + 1, bmpGraph.Height + 1);
            gfxFrame.FillRectangle(new SolidBrush(colorGraphBg), graphPos.X, graphPos.Y, bmpGraph.Width, bmpGraph.Height);
            foreach (Axis.Tick tick in xAxis.ticksMajor)
                gfxFrame.DrawLine(penAxis, new Point(padL + tick.posPixel, posB + 1), new Point(padL + tick.posPixel, posB + 1 + tick_size_minor));
            foreach (Axis.Tick tick in yAxis.ticksMajor)
                gfxFrame.DrawLine(penAxis, new Point(padL - 1, padT + tick.posPixel), new Point(padL - 1 - tick_size_minor, padT + tick.posPixel));
            foreach (Axis.Tick tick in xAxis.ticksMinor)
            {
                gfxFrame.DrawLine(penGrid, new Point(padL + tick.posPixel, padT), new Point(padL + tick.posPixel, padT + bmpGraph.Height - 1));
                gfxFrame.DrawLine(penAxis, new Point(padL + tick.posPixel, posB + 1), new Point(padL + tick.posPixel, posB + 1 + tick_size_major));
                gfxFrame.DrawString(tick.label, fontTicks, brush, new Point(tick.posPixel + padL, posB + 7), sfCenter);
            }
            foreach (Axis.Tick tick in yAxis.ticksMinor)
            {
                gfxFrame.DrawLine(penGrid, new Point(padL, padT + tick.posPixel), new Point(padL + bmpGraph.Width, padT + tick.posPixel));
                gfxFrame.DrawLine(penAxis, new Point(padL - 1, padT + tick.posPixel), new Point(padL - 1 - tick_size_major, padT + tick.posPixel));
                gfxFrame.DrawString(tick.label, fontTicks, brush, new Point(padL-6, tick.posPixel + padT-7), sfRight);
            }

            // draw labels
            gfxFrame.DrawString(labelX, fontAxis, brush, new Point(posCx, posB+24), sfCenter);
            gfxFrame.DrawString(labelTitle, fontTitle, brush, new Point(bmpFrame.Width/2,8), sfCenter);
            gfxFrame.TranslateTransform(gfxFrame.VisibleClipBounds.Size.Width, 0);
            gfxFrame.RotateTransform(-90);
            gfxFrame.DrawString(labelY, fontAxis, brush, new Point(-posCy, -bmpFrame.Width+2), sfCenter);
            gfxFrame.ResetTransform();

            // now that the frame is re-drawn, reset the graph
            GraphClear();
        }

        /// <summary>
        /// Copy the empty graph area from the frame onto the graph object
        /// </summary>
        public void GraphClear()
        {
            gfxGraph.DrawImage(bmpFrame, new Point(-padL, -padT));
            pointCount = 0;
        }

        private long pointCount=0;
        private string benchmarkMessage
        {
            get
            {
                double ms = this.stopwatch.ElapsedTicks * 1000.0 / System.Diagnostics.Stopwatch.Frequency;
                double hz = 1.0 / ms * 1000.0;
                string msg = "";
                double imageSizeMB = bmpFrame.Width * bmpFrame.Height * 4.0 / 1024 / 1024;
                msg += string.Format("{0} x {1} ({2:0.00} MB) ", bmpFrame.Width, bmpFrame.Height, imageSizeMB);
                msg += string.Format("with {0:n0} data points rendered in ", pointCount);
                msg += string.Format("{0:0.00 ms} ({1:0.00} Hz)", ms, hz);
                return msg;
            }
        }

        /// <summary>
        /// Return a merged bitmap of the frame with the graph added into it
        /// </summary>
        public Bitmap Render()
        {
            Bitmap bmpMerged = new Bitmap(bmpFrame);
            Graphics gfx = Graphics.FromImage(bmpMerged);
            gfx.DrawImage(bmpGraph, graphPos);

            // draw stamp message
            if (this.stopwatch.ElapsedTicks>0)
            {
                Font fontStamp = new Font(font, 8, FontStyle.Regular);
                SolidBrush brushStamp = new SolidBrush(colorAxis);
                Point pointStamp = new Point(bmpFrame.Width - padR - 2, bmpFrame.Height - padB - 14);
                StringFormat sfRight = new StringFormat();
                sfRight.Alignment = StringAlignment.Far;
                gfx.DrawString(benchmarkMessage, fontStamp, brushStamp, pointStamp, sfRight);

            }

            return bmpMerged;
        }

        /// <summary>
        /// Saves the figure in the format based on its extension.
        /// </summary>
        public void Save(string filename)
        {
            string basename = System.IO.Path.GetFileNameWithoutExtension(filename);
            string extension = System.IO.Path.GetExtension(filename).ToLower();
            string fullPath = System.IO.Path.GetFullPath(filename);

            switch (extension)
            {
                case ".png":
                    Render().Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                    Console.WriteLine($"saved {fullPath}");
                    break;
                case ".jpg":
                    Render().Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Console.WriteLine("saved as JPG");
                    break;
                case ".bmp":
                    Render().Save(filename);
                    Console.WriteLine("saved as BMP");
                    break;
                default:
                    Console.WriteLine("format not supported!");
                    break;
            }
        }

        /// <summary>
        /// Manually define axis limits.
        /// </summary>
        public void AxisSet(double? x1, double? x2, double? y1, double? y2)
        {
            if (x1 != null) xAxis.min = (double)x1;
            if (x2 != null) xAxis.max = (double)x2;
            if (y1 != null) yAxis.min = (double)y1;
            if (y2 != null) yAxis.max = (double)y2;
            if (x1 != null || x2 != null) xAxis.RecalculateScale();
            if (y1 != null || y2 != null) yAxis.RecalculateScale();
            if (x1 != null || x2 != null || y1 != null || y2 != null) FrameRedraw();
        }
        
        /// <summary>
        /// Adjust axis edges to tightly fit the given data. Set zoom to .9 to zoom out slightly.
        /// </summary>
        public void AxisAuto(double[] Xs, double[] Ys, double? zoomX, double? zoomY)
        {
            if (Xs != null) AxisSet(Xs.Min(), Xs.Max(), null, null);
            if (Ys != null) AxisSet(null, (double?)null, Ys.Min(), Ys.Max());
            Zoom(zoomX, zoomY);
        }

        /// <summary>
        /// Zoom in on the center of Axis by a fraction. 
        /// A fraction of 2 means that the new width will be 1/2 as wide as the old width.
        /// A fraction of 0.1 means the new width will show 10 times more axis length.
        /// </summary>
        public void Zoom(double? xFrac, double? yFrac)
        {
            if (xFrac!=null) xAxis.Zoom((double)xFrac);
            if (yFrac!=null) yAxis.Zoom((double)yFrac);
            FrameRedraw();
        }

        public void PanUnits(double dX, double dY)
        {
            xAxis.Pan(dX);
            yAxis.Pan(dY);
            FrameRedraw();
        }

        public void PanPixels(int dX, int dY)
        {
            xAxis.Pan(xAxis.unitsPerPx * dX);
            yAxis.Pan(yAxis.unitsPerPx * dY);
            FrameRedraw();
        }

        public void styleWeb()
        {
            colorFigBg = Color.White;
            colorGraphBg = Color.FromArgb(255, 235, 235, 235);
            colorAxis = Color.Black;
            colorGridLines = Color.LightGray;
        }

        public void styleForm()
        {
            colorFigBg = SystemColors.Control;
            colorGraphBg = Color.White;
            colorAxis = Color.Black;
            colorGridLines = Color.LightGray;
        }












        /// <summary>
        /// Call this before graphing to start a stopwatch. 
        /// Render time will be displayed when the output graph is rendered.
        /// </summary>
        public void BenchmarkThis(bool enable=true)
        {
            if (enable)
            {
                stopwatch.Restart();
            }
            else
            {
                stopwatch.Stop();
                stopwatch.Reset();
            }
        }

        private Point[] PointsFromArrays(double[] Xs, double[] Ys)
        {
            int pointCount = Math.Min(Xs.Length, Ys.Length);
            Point[] points = new Point[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                points[i] = new Point(xAxis.GetPixel(Xs[i]), yAxis.GetPixel(Ys[i]));
            }
            return points;
        }
        
        
        public void PlotLines(double[] Xs, double[] Ys, float lineWidth = 1, Color? lineColor = null)
        {
            if (lineColor == null) lineColor = Color.Red;

            Point[] points = PointsFromArrays(Xs, Ys);
            Pen penLine = new Pen(new SolidBrush((Color)lineColor), lineWidth);

            // adjust the pen caps and joins to make it as smooth as possible
            penLine.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            penLine.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            penLine.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

            // todo: prevent infinite zooming overflow errors
            gfxGraph.DrawLines(penLine, points);
            this.pointCount += points.Length;
        }

        public void PlotLines(double X1, double X2, double Y1, double Y2, float lineWidth = 1, Color? lineColor = null)
        {
            PlotLines(new double[] { X1, X2 }, new double[] { Y1, Y2 }, lineWidth, lineColor);
        }

        public void PlotSignal(double[] values, double pointSpacing=1, double offsetX=0, double offsetY=0, float lineWidth=1, Color? lineColor=null)
        {
            if (lineColor == null) lineColor = Color.Red;
            if (values == null) return;

            int pointCount = values.Length;
            double lastPointX = offsetX + values.Length * pointSpacing;
            int dataMinPx = (int)((offsetX - xAxis.min) / xAxis.unitsPerPx);
            int dataMaxPx = (int)((lastPointX - xAxis.min) / xAxis.unitsPerPx);
            double binUnitsPerPx = xAxis.unitsPerPx / pointSpacing;
            double dataPointsPerPixel = xAxis.unitsPerPx / pointSpacing;

            List<Point> points = new List<Point>();
            List<double> Ys = new List<double>();

            for (int i = 0; i < values.Length; i++) Ys.Add(values[i]); // copy entire array into list (SLOW!!!)

            if (dataPointsPerPixel < 1)
            {
                // LOW DENSITY TRADITIONAL X/Y PLOTTING
                int iLeft = (int)(((xAxis.min - offsetX) / xAxis.unitsPerPx) * dataPointsPerPixel);
                int iRight = iLeft + (int)(dataPointsPerPixel * bmpGraph.Width);
                for (int i = Math.Max(0, iLeft - 2); i < Math.Min(iRight + 3, Ys.Count - 1); i++)
                {
                    int xPx = xAxis.GetPixel((double)i * pointSpacing + offsetX);
                    int yPx = yAxis.GetPixel(Ys[i]);
                    points.Add(new Point(xPx, yPx));
                }
            } else
            {
                // BINNING IS REQUIRED FOR HIGH DENSITY PLOTTING
                for (int xPixel = Math.Max(0, dataMinPx); xPixel < Math.Min(bmpGraph.Width, dataMaxPx); xPixel++)
                {
                    int iLeft = (int)(binUnitsPerPx * (xPixel - dataMinPx));
                    int iRight = (int)(iLeft + binUnitsPerPx);
                    iLeft = Math.Max(iLeft, 0);
                    iRight = Math.Min(Ys.Count - 1, iRight);
                    iRight = Math.Max(iRight, 0);
                    if (iLeft == iRight) continue;
                    double yPxMin = Ys.GetRange(iLeft, iRight - iLeft).Min() + offsetY;
                    double yPxMax = Ys.GetRange(iLeft, iRight - iLeft).Max() + offsetY;
                    points.Add(new Point(xPixel, yAxis.GetPixel(yPxMin)));
                    points.Add(new Point(xPixel, yAxis.GetPixel(yPxMax)));
                }
            }

            if (points.Count < 2) return;
            Pen penLine = new Pen(new SolidBrush((Color)lineColor), lineWidth);
            float markerSize = 3;
            SolidBrush markerBrush = new SolidBrush((Color)lineColor);
            System.Drawing.Drawing2D.SmoothingMode originalSmoothingMode = gfxGraph.SmoothingMode;
            gfxGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None; // no antialiasing


            // todo: prevent infinite zooming overflow errors
            try {
                gfxGraph.DrawLines(penLine, points.ToArray());

                if (dataPointsPerPixel < .5)
                {
                    foreach (Point pt in points)
                    {
                        gfxGraph.FillEllipse(markerBrush, pt.X - markerSize / 2, pt.Y - markerSize / 2, markerSize, markerSize);
                    }
                }
            }
            catch (Exception ex) {
                System.Console.WriteLine("Exception plotting");
            }
            

            gfxGraph.SmoothingMode = originalSmoothingMode;
            this.pointCount += values.Length;
        }

        public void PlotScatter(double[] Xs, double[] Ys, float markerSize = 3, Color? markerColor = null)
        {
            if (markerColor == null) markerColor = Color.Red;
            Point[] points = PointsFromArrays(Xs, Ys);
            for (int i=0; i<points.Length; i++)
            {
                gfxGraph.FillEllipse(new SolidBrush((Color)markerColor), 
                                     points[i].X - markerSize / 2, 
                                     points[i].Y - markerSize / 2, 
                                     markerSize, markerSize);
            }
            pointCount += points.Length;
        }

        public void PlotScatter(double X, double Y, float markerSize = 3, Color? markerColor = null)
        {
            PlotScatter(new double[] { X }, new double[] { Y }, markerSize, markerColor);
        }


        /* MOUSE STUFF */

        MouseAxis mousePan = null;
        MouseAxis mouseZoom = null;
        
        public void MousePanStart(int xPx, int yPx) { mousePan = new MouseAxis(xAxis, yAxis, xPx, yPx); }
        public void MousePanEnd() { mousePan = null; }
        public void MouseZoomStart(int xPx, int yPx) { mouseZoom = new MouseAxis(xAxis, yAxis, xPx, yPx); }
        public void MouseZoomEnd() { mouseZoom = null; }
        public bool MouseIsDragging() { return (mousePan != null || mouseZoom != null); }

        public void MouseMove(int xPx, int yPx)
        {
            if (mousePan != null)
            {
                mousePan.Pan(xPx, yPx);
                AxisSet(mousePan.x1, mousePan.x2, mousePan.y1, mousePan.y2);
            } else if (mouseZoom != null)
            {
                mouseZoom.Zoom(xPx, yPx);
                AxisSet(mouseZoom.x1, mouseZoom.x2, mouseZoom.y1, mouseZoom.y2);
            }
        }

        internal MouseAxis MouseAxis
        {
            get => default(MouseAxis);
            set
            {
            }
        }
    }
}
