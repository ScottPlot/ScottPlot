using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableScatter : Plottable, IExportable
    {
        public double[] xs;
        public double[] ys;
        public double lineWidth;
        public float markerSize;
        public bool stepDisplay;
        public Pen penLine;
        public Brush brush;

        public PlottableScatter(double[] xs, double[] ys, Color color, double lineWidth, double markerSize, string label,
            bool stepDisplay, MarkerShape markerShape, LineStyle lineStyle)
        {

            if ((xs == null) || (ys == null))
                throw new Exception("X and Y data cannot be null");

            if (xs.Length != ys.Length)
                throw new Exception("Xs and Ys must have same length");

            this.xs = xs;
            this.ys = ys;
            this.color = color;
            this.lineWidth = lineWidth;
            this.markerSize = (float)markerSize;
            this.label = label;
            this.stepDisplay = stepDisplay;
            this.markerShape = markerShape;
            this.lineStyle = lineStyle;

            pointCount = xs.Length;

            if (xs.Length != ys.Length)
                throw new ArgumentException("X and Y arrays must have the same length");

            penLine = new Pen(color, (float)lineWidth)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round,
                DashStyle = StyleTools.DashStyle(lineStyle),
                DashPattern = StyleTools.DashPattern(lineStyle)
            };

            brush = new SolidBrush(color);
        }

        public override string ToString()
        {
            return $"PlottableScatter with {pointCount} points";
        }

        public override Config.AxisLimits2D GetLimits()
        {
            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(new double[] { xs.Min(), xs.Max(), ys.Min(), ys.Max() });
        }

        PointF[] points;
        PointF[] pointsStep;
        public override void Render(Settings settings)
        {
            penLine.Color = color;
            penLine.Width = (float)lineWidth;

            if (points is null)
                points = new PointF[xs.Length];

            for (int i = 0; i < xs.Length; i++)
                points[i] = settings.GetPixel(xs[i], ys[i]);

            if (stepDisplay)
            {
                if (pointsStep is null)
                    pointsStep = new PointF[xs.Length * 2 - 1];
                for (int i = 0; i < points.Length; i++)
                    pointsStep[i * 2] = points[i];
                for (int i = 0; i < points.Length - 1; i++)
                    pointsStep[i * 2 + 1] = new PointF(points[i + 1].X, points[i].Y);
            }

            if (penLine.Width > 0 && points.Length > 1)
            {
                if (stepDisplay)
                    settings.gfxData.DrawLines(penLine, pointsStep);
                else
                    settings.gfxData.DrawLines(penLine, points);
            }

            if ((markerSize > 0) && (markerShape != MarkerShape.none))
                for (int i = 0; i < points.Length; i++)
                    MarkerTools.DrawMarker(settings.gfxData, points[i], markerShape, markerSize, color);

        }

        public void SaveCSV(string filePath, string delimiter = ", ", string separator = "\n")
        {
            System.IO.File.WriteAllText(filePath, GetCSV(delimiter, separator));
        }

        public string GetCSV(string delimiter = ", ", string separator = "\n")
        {
            StringBuilder csv = new StringBuilder();
            for (int i = 0; i < ys.Length; i++)
                csv.AppendFormat("{0}{1}{2}{3}", xs[i], delimiter, ys[i], separator);
            return csv.ToString();
        }
    }
}
