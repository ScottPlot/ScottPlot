using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableSignal : Plottable
    {
        public double[] ys;
        public double sampleRate;
        public double samplePeriod;
        public float markerSize;
        public double xOffset;
        public double yOffset;
        public Pen pen;
        public Brush brush;

        public PlottableSignal(double[] ys, double sampleRate, double xOffset, double yOffset, Color color, double lineWidth, double markerSize, string label)
        {

            if (ys == null)
                throw new Exception("Y data cannot be null");

            this.ys = ys;
            this.sampleRate = sampleRate;
            this.samplePeriod = 1.0 / sampleRate;
            this.markerSize = (float)markerSize;
            this.xOffset = xOffset;
            this.label = label;
            this.color = color;
            this.yOffset = yOffset;
            pointCount = ys.Length;
            brush = new SolidBrush(color);
            pen = new Pen(color, (float)lineWidth)
            {
                // this prevents sharp corners
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };
        }

        public override string ToString()
        {
            return $"PlottableSignal with {pointCount} points";
        }

        public override double[] GetLimits()
        {
            double[] limits = new double[4];
            limits[0] = 0 + xOffset;
            limits[1] = samplePeriod * ys.Length + xOffset;
            limits[2] = ys.Min() + yOffset;
            limits[3] = ys.Max() + yOffset;
            return limits;
        }

        private void RenderLowDensity(Settings settings, int visibleIndex1, int visibleIndex2)
        {
            List<PointF> linePoints = new List<PointF>(visibleIndex2 - visibleIndex1 + 2);
            if (visibleIndex2 > ys.Length - 2)
                visibleIndex2 = ys.Length - 2;
            if (visibleIndex1 < 0)
                visibleIndex1 = 0;
            for (int i = visibleIndex1; i <= visibleIndex2 + 1; i++)
                linePoints.Add(settings.GetPixel(samplePeriod * i + xOffset, ys[i] + yOffset));

            if (linePoints.Count > 1)
            {
                settings.gfxData.DrawLines(pen, linePoints.ToArray());
                foreach (PointF point in linePoints)
                    settings.gfxData.FillEllipse(brush, point.X - markerSize / 2, point.Y - markerSize / 2, markerSize, markerSize);
            }
        }

        private void RenderHighDensity(Settings settings, double offsetPoints, double columnPointCount)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = Math.Min(settings.dataSize.Width, xPxEnd);
            if (xPxStart >= xPxEnd)
                return;
            List<PointF> linePoints = new List<PointF>((xPxEnd - xPxStart) * 2 + 1);
            for (int xPx = xPxStart; xPx < xPxEnd; xPx++)
            {
                // determine data indexes for this pixel column
                int index1 = (int)(offsetPoints + columnPointCount * xPx);
                int index2 = (int)(offsetPoints + columnPointCount * (xPx + 1));

                if (index1 < 0)
                    index1 = 0;
                if (index2 > ys.Length - 1)
                    index2 = ys.Length - 1;

                // get the min and max value for this column
                double lowestValue, highestValue;
                MinMaxRangeQuery(index1, index2, out lowestValue, out highestValue);
                float yPxHigh = settings.GetPixel(0, lowestValue + yOffset).Y;
                float yPxLow = settings.GetPixel(0, highestValue + yOffset).Y;

                // adjust order of points to enhance anti-aliasing
                if ((linePoints.Count < 2) || (yPxLow < linePoints[linePoints.Count - 1].Y))
                {
                    linePoints.Add(new PointF(xPx, yPxLow));
                    linePoints.Add(new PointF(xPx, yPxHigh));
                }
                else
                {
                    linePoints.Add(new PointF(xPx, yPxHigh));
                    linePoints.Add(new PointF(xPx, yPxLow));
                }
            }

            if (linePoints.Count > 0)
                settings.gfxData.DrawLines(pen, linePoints.ToArray());
        }

        protected virtual void MinMaxRangeQuery(int index1, int index2, out double lowestValue, out double highestValue)
        {
            lowestValue = ys[index1];
            highestValue = ys[index1];
            for (int i = index1; i < index2; i++)
            {
                if (ys[i] < lowestValue)
                    lowestValue = ys[i];
                if (ys[i] > highestValue)
                    highestValue = ys[i];
            }
        }

        public override void Render(Settings settings)
        {

            double dataSpanUnits = ys.Length * samplePeriod;
            double columnSpanUnits = settings.xAxisSpan / settings.dataSize.Width;
            double columnPointCount = (columnSpanUnits / dataSpanUnits) * ys.Length;
            double offsetUnits = settings.axis[0] - xOffset;
            double offsetPoints = offsetUnits / samplePeriod;
            int visibleIndex1 = (int)(offsetPoints);
            int visibleIndex2 = (int)(offsetPoints + columnPointCount * (settings.dataSize.Width + 1));
            int visiblePointCount = visibleIndex2 - visibleIndex1;

            if (visiblePointCount > settings.dataSize.Width)
                RenderHighDensity(settings, offsetPoints, columnPointCount);
            else
                RenderLowDensity(settings, visibleIndex1, visibleIndex2);
        }

        public override void SaveCSV(string filePath)
        {
            StringBuilder csv = new StringBuilder();
            for (int i = 0; i < ys.Length; i++)
                csv.AppendFormat("{0}, {1}\n", xOffset + i * samplePeriod, ys[i]);
            System.IO.File.WriteAllText(filePath, csv.ToString());
        }
    }
}
