using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

//#pragma warning disable 1591

namespace ScottPlot
{
    /// <summary>
    /// Create a Figure (everything starts here)
    /// </summary>
    public class Figure
    {
        // all axis logic and drawing routines occur in the Draw class
        private Draw Draw = new Draw();

        /// <summary>
        /// Create a new ScottPlot figure.
        /// </summary>
        public Figure(int width=800, int height=600)
        {
            Resize(width, height);
        }

        /// <summary>
        /// Resize the ScottPlot Figure.
        /// </summary>
        public void Resize(int width, int height)
        {
            //todo: error checking
            //todo: make sure we don't re-init if the width/height didn't change
            Draw.Init(width, height);
        }

        /// <summary>
        /// Return the rendered bitmap (axis + data)
        /// </summary>
        /// <returns></returns>
        public Bitmap Render()
        {
            return Draw.Render();
        }

        /// <summary>
        /// write the current figure bitmap to a file
        /// </summary>
        public void SaveFig(string filename)
        {
            Draw.Render().Save(filename);
        }

        /// <summary>
        /// given some data, automatically adjust the axis to accomodate all of it.
        /// manually set axis before drawing anything (very important).
        /// </summary>
        public void AxisAuto(double[] Xs, double[] Ys, double marginX=0, double marginY=0.1)
        {
            //todo zoom in or out
            Draw.Axis.Auto(Xs, Ys);
            Draw.InitAxis(); // redraw axis ticks after changing the axis
        }

        public void AxisSet(double X1, double X2, double Y1, double Y2)
        {
            //todo error checking
            Draw.Axis.Set(X1, X2, Y1, Y2);
            Draw.InitAxis(); // redraw axis ticks after changing the axis
        }

        public void AxisSet(double[] arr)
        {
            AxisSet(arr[0],arr[1],arr[2],arr[3]);
        }

        public void AxisZoom(double fracX=1, double fracY=1)
        {
            Draw.Axis.Zoom(fracX, fracY);
            Draw.InitAxis(); // redraw axis ticks after changing the axis
        }

        public void AxisPan(double Xpx, double Ypx)
        {
            
        }

        public double[] AxisGet()
        {
            double[] ax = { Draw.Axis.X1, Draw.Axis.X2, Draw.Axis.Y1, Draw.Axis.Y2 };
            return ax;
        }

        // routines here relate to click-drag panning and zooming for GUI applications.
        private int mouseDownX;
        private int mouseDownY;
        private double[] mouseDownAxis;
        public void MouseDown(int mouseX, int mouseY)
        {
            mouseDownX = mouseX;
            mouseDownY = mouseY;
            mouseDownAxis = AxisGet();
        }
        public void MousePan(int mouseX, int mouseY)
        {
            int dXpx = mouseX - mouseDownX;
            int dYpx = mouseDownY - mouseY;
            double dX = dXpx * Draw.Axis.UnitsPerPxX;
            double dY = dYpx * Draw.Axis.UnitsPerPxY;
            AxisSet(mouseDownAxis[0] - dX, mouseDownAxis[1] - dX,
                    mouseDownAxis[2] - dY, mouseDownAxis[3] - dY);
        }
        public void MouseZoom(int mouseX, int mouseY)
        {
            double dX = mouseX - mouseDownX;
            double dY = mouseDownY - mouseY;

            double halfWidthX = (mouseDownAxis[1] - mouseDownAxis[0]) / 2;
            double halfWidthY = (mouseDownAxis[3] - mouseDownAxis[2]) / 2;
            double centerX = (mouseDownAxis[1] + mouseDownAxis[0]) / 2;
            double centerY = (mouseDownAxis[3] + mouseDownAxis[2]) / 2;
            
            if (dX > 0) halfWidthX -= halfWidthX * (dX / (dX + 100));
            if (dX < 0) halfWidthX -= halfWidthX * (dX / 500);
            if (dY > 0) halfWidthY -= halfWidthY * (dY / (dY + 100));
            if (dY < 0) halfWidthY -= halfWidthY * (dY / 500);

            AxisSet(centerX - halfWidthX, centerX + halfWidthX,
                    centerY - halfWidthY, centerY + halfWidthY);

        }

        /// <summary>
        /// add a line ((X,Y) pairs connected by lines) to the canvas
        /// </summary>
        public void PlotLine(double[] Xs, double[] Ys, string colorCode = "b", int lineWidth=1, bool highQuality = false)
        {
            if (highQuality) Draw.gfxData.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            else Draw.gfxData.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            // first check to ensure our axis has been initialized
            if (Draw.Axis.X1 == Draw.Axis.X2) AxisAuto(Xs, Ys);

            // set the pin design
            Pen pen = new Pen(Draw.ColorCode(colorCode), lineWidth);

            // plot by drawing every line (slow for large data)
            // Draw.gfxData.DrawLines(pen, Draw.Axis.PixelPoints(Xs, Ys));

            // plot by drawing lines of pixels (faster for large data)
            List<double> listYs = new List<double>(Ys);
            PlotLine2(listYs, Xs[1]-Xs[0],Xs[0]);
        }


        public void PlotLine2(List<double> Ys, double unitsPerPoint = 10, double offsetX = 0)
        {
            //todo: is there a faster way to make an array?
            double unitsPerPixel = Draw.Axis.UnitsPerPxX;
            double iPerPixel = unitsPerPixel / unitsPerPoint;
            double nDataPixels = unitsPerPoint * Ys.Count / unitsPerPixel;
            double offsetPixels = -(Draw.Axis.X1 - offsetX) / unitsPerPixel;
            List<Point> points = new List<Point>();
            for (int x = 0; x < Draw.Axis.dataWidth; x++)
            {
                int iLeft = (int)((iPerPixel * ((x + 0) - offsetPixels)));
                int iRight = (int)((iPerPixel * ((x + 1) - offsetPixels)));
                if ((iLeft < 0) || (iRight <= 0)) continue;
                if (iRight > Ys.Count) continue;
                if (iRight - iLeft == 0) continue;
                double colMin = Ys.GetRange(iLeft, iRight - iLeft).Min();
                double colMax = Ys.GetRange(iLeft, iRight - iLeft).Max();
                colMin = Draw.Axis.dataHeight - (colMin - Draw.Axis.Y1) * Draw.Axis.pxPerUnitY;
                colMax = Draw.Axis.dataHeight - (colMax - Draw.Axis.Y1) * Draw.Axis.pxPerUnitY;
                points.Add(new Point(x, (int)colMin));
                if ((int)colMin != (int)colMax) { points.Add(new Point(x, (int)colMax)); }
            }
            Ys.RemoveAt(0); // somehow this helps alignment
            try
            {
                Draw.gfxData.DrawLines(new Pen(Color.Red, 1), points.ToArray());
            }
            catch
            {
                System.Console.WriteLine("CRASH IN Draw.gfxData.DrawLines()");
            }
            
        }

        /// <summary>
        /// draw a grid on the current data image
        /// </summary>
        public void Grid()
        {
            Draw.Grid();
        }

        /// <summary>
        /// erase all graph marks but leave axis intact
        /// </summary>
        public void Clear()
        {
            Draw.InitData();
        }

        /// <summary>
        /// show information about the current figure
        /// </summary>
        public void Info()
        {
            Draw.Axis.Info();
        }

    }
}
