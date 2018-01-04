using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace ScottPlot
{
    /// <summary>
    /// tools to work with data axes
    /// </summary>
    public class Axis
    {
        // THESE AXIS NUMBERS ARE ENTIRELY IN ARBITRARY SPACE
        // NO CODE INSIDE THIS BLOCK KNOWS ABOUT THE PIXEL SIZE OF THE IMAGE!

        // axis limits (units)
        public double X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;

        // size of the data area (pixels)
        public int dataWidth, dataHeight;

        // scale of data area
        public double pxPerUnitX, pxPerUnitY, UnitsPerPxX, UnitsPerPxY;

        /// <summary>
        /// display information about the axis, figure, pixel/unit mesh, etc.
        /// </summary>
        public void Info()
        {
            System.Console.WriteLine("Horizontal axis limits = ({0}, {1})", X1, X2);
            System.Console.WriteLine("Vertical axis limits = ({0}, {1})", Y1, Y2);
            System.Console.WriteLine("Figure size (width, height) = ({0}, {1})", dataWidth, dataHeight);
            System.Console.WriteLine("Pixels per Unit (horizontal, vertical) = ({0}, {1})", pxPerUnitX, pxPerUnitY);
            System.Console.WriteLine("Units per Pixel (horizontal, vertical) = ({0}, {1})", UnitsPerPxX, UnitsPerPxY);
        }

        /// <summary>
        /// prepare the axis class for a certain size of data
        /// </summary>
        public void Init(int dataWidth, int dataHeight)
        {
            this.dataWidth = dataWidth;
            this.dataHeight = dataHeight;
            CalculateMesh();
        }

        /// <summary>
        /// re-calculate pixel-to-unit and unit-to-pixel values from the given size and axis
        /// </summary>
        public void CalculateMesh()
        {
            pxPerUnitX = (double)dataWidth / (X2 - X1);
            pxPerUnitY = (double)dataHeight / (Y2 - Y1);
            UnitsPerPxX = (X2 - X1) / (double)dataWidth;
            UnitsPerPxY = (Y2 - Y1) / (double)dataHeight;
        }

        /// <summary>
        /// Given Xs and Ys, adjust the axis boundaries to fit the data (with optional padding)
        /// </summary>
        public void Auto(double[] Xs, double[] Ys, double padX = 0, double padY = 0, bool expandOnly = false)
        {
            double[] limitsX = PaddedLimit(Xs, padX);
            double[] limitsY = PaddedLimit(Ys, padY);
            X1 = limitsX[0];
            X2 = limitsX[1];
            Y1 = limitsY[0];
            Y2 = limitsY[1];
            //todo: expand only

            // axis changed so recalc our pixel conversions
            CalculateMesh();
        }

        public void Zoom(double fracX=1, double fracY=1)
        {
            double spanX = X2 - X1;
            double spanY = Y2 - Y1;
            double centerX = (X2 + X1) / 2;
            double centerY = (Y2 + Y1) / 2;
            double offsetX = (spanX * fracX) / 2;
            double offsetY = (spanY * fracY) / 2;
            Set(centerX - offsetX, centerX + offsetX, centerY - offsetY, centerY + offsetY);
        }

        public void Set(double X1, double X2, double Y1, double Y2)
        {
            // todo: error checking
            this.X1 = X1;
            this.X2 = X2;
            this.Y1 = Y1;
            this.Y2 = Y2;
            CalculateMesh();
        }

        /// <summary>
        /// Given an array, return [min, max] with optional fractional padding.
        /// This is intended to be used when calculating magins with Auto().
        /// </summary>
        private double[] PaddedLimit(double[] data, double padFraction = 0)
        {
            double[] limits = new double[2];
            limits[0] = data.Min();
            limits[1] = data.Max();
            if (limits[0] == limits[1]) return limits;
            double padding = (limits[1] - limits[0]) * padFraction;
            limits[0] = limits[0] - padding;
            limits[1] = limits[1] + padding;
            return limits;
        }

        /// <summary>
        /// return an array of good tick values for an axis given a range
        /// </summary>
        public double[] TickGen(double X1, double X2, int widthPx, int nTicks = 10)
        {
            List<double> values = new List<double>();
            List<int> pixels = new List<int>();
            List<string> labels = new List<string>();
            double dataSpan = X2 - X1;
            double unitsPerPx = dataSpan / widthPx;
            double PxPerUnit = widthPx / dataSpan;
            double tickSize = RoundNumberNear((dataSpan / nTicks) * 1.5);

            int lastTick = 123456789;
            for (int i = 0; i < widthPx; i++)
            {
                double thisPosition = i * unitsPerPx + X1;
                int thisTick = (int)(thisPosition / tickSize);
                if (thisTick != lastTick)
                {
                    lastTick = thisTick;
                    double thisPositionRounded = (double)((int)(thisPosition / tickSize) * tickSize);
                    if (thisPositionRounded > X1 && thisPositionRounded < X2)
                    {
                        values.Add(thisPositionRounded);
                        pixels.Add(i);
                        labels.Add(string.Format("{0}", thisPosition));
                    }
                }
            }
            //todo: keep it arrays the whole time
            return values.ToArray();
        }

        /// <summary>
        /// given an arbitrary number, return the nearerest round number
        /// (i.e., 1000, 500, 100, 50, 10, 5, 1, .5, .1, .05, .01)
        /// </summary>
        private double RoundNumberNear(double target)
        {
            target = Math.Abs(target);
            int lastDivision = 2;
            double round = 1000000000000;
            while (round > 0.00000000001)
            {
                if (round <= target) return round;
                round /= lastDivision;
                if (lastDivision == 2) lastDivision = 5;
                else lastDivision = 2;
            }
            return 0;
        }

        /// <summary>
        /// format a number for a tick label by limiting its precision
        /// </summary>
        public string TickString(double val, double valDistance)
        {
            if (valDistance < .01) return string.Format("{0:0.0000}", val);
            if (valDistance < .1) return string.Format("{0:0.000}", val);
            if (valDistance < 1) return string.Format("{0:0.00}", val);
            if (valDistance < 10) return string.Format("{0:0.0}", val);
            return string.Format("{0:0}", val);
        }

        /// <summary>
        /// given Xs and Ys, use the axis information to return a list of points to be plotted.
        /// </summary>
        public Point[] PixelPoints(double[] Xs, double[] Ys)
        {
            //todo: error checking
            Point[] points = new Point[Xs.Length];
            for (int i = 0; i < points.Length; i++)
            {
                int x = (int)((Xs[i]-X1) * pxPerUnitX);
                int y = dataHeight - (int)((Ys[i]-Y1) * pxPerUnitY);
                points[i] = new Point(x, y);
            }
            return points;
        }

    }
}