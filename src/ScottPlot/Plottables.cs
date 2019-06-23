using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace ScottPlot
{
    public class PlottableBar : Plottable
    {
        public double[] xs;
        public double[] ys;
        public double[] yErr;
        float errorLineWidth;
        float errorCapSize;
        float barWidth;
        float baseline = 0;
        double xOffset;
        Pen pen;
        Brush brush;

        public PlottableBar(double[] xs, double[] ys, double barWidth, double xOffset, Color color, string label, double[] yErr, double errorLineWidth = 1, double errorCapSize = 3)
        {
            if (xs.Length != ys.Length)
                throw new ArgumentException("X positions must be the same length as Y values");
            if (yErr != null && yErr.Length != xs.Length)
                throw new ArgumentException("Errorbar values must be the same length as Y values");

            if (yErr != null)
                for (int i = 0; i < yErr.Length; i++)
                    if (yErr[i] < 0)
                        yErr[i] = -yErr[i];

            this.xs = xs;
            this.ys = ys;
            this.yErr = yErr;
            this.barWidth = (float)barWidth;
            this.errorLineWidth = (float)errorLineWidth;
            this.errorCapSize = (float)errorCapSize;
            this.color = color;
            this.label = label;
            this.xOffset = xOffset;
            pointCount = ys.Length;

            pen = new Pen(color, (float)errorLineWidth)
            {
                // this prevents sharp corners
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

            brush = new SolidBrush(color);
        }

        public override double[] GetLimits()
        {
            double[] limits = new double[4];
            limits[0] = xs.Min() - barWidth / 2;
            limits[1] = xs.Max() + barWidth / 2;
            limits[2] = ys.Min();
            limits[3] = ys.Max();

            if (baseline < limits[2])
                limits[2] = baseline;

            if (yErr != null)
            {
                for (int i = 0; i < yErr.Length; i++)
                {
                    if (ys[i] - yErr[i] < limits[2])
                        limits[2] = ys[i] - yErr[i];
                    if (ys[i] + yErr[1] > limits[3])
                        limits[3] = ys[i] + yErr[i];
                }
            }

            return limits;
        }

        public override void Render(Settings settings)
        {
            for (int i = 0; i < pointCount; i++)
            {
                PointF barTop;
                PointF barBot;

                if (ys[i] > baseline)
                {
                    barTop = settings.GetPixel(xs[i], ys[i]);
                    barBot = settings.GetPixel(xs[i], baseline);
                }
                else
                {
                    barBot = settings.GetPixel(xs[i], ys[i]);
                    barTop = settings.GetPixel(xs[i], baseline);
                }

                float barTopPx = barTop.Y;
                float barHeightPx = barTop.Y - barBot.Y;
                float barWidthPx = (float)(barWidth * settings.xAxisScale);
                float barLeftPx = barTop.X - barWidthPx / 2;
                float xOffsetPx = (float)(xOffset * settings.xAxisScale);
                barLeftPx += xOffsetPx;

                settings.gfxData.FillRectangle(brush, barLeftPx, barTopPx, barWidthPx, -barHeightPx);

                if (yErr != null)
                {
                    PointF peakCenter = settings.GetPixel(xs[i], ys[i]);
                    float x = peakCenter.X + xOffsetPx;
                    float y = peakCenter.Y;
                    float errorPx = (float)(yErr[i] * settings.yAxisScale);
                    settings.gfxData.DrawLine(pen, x, y - errorPx, x, y + errorPx);
                    settings.gfxData.DrawLine(pen, x - errorCapSize, y - errorPx, x + errorCapSize, y - errorPx);
                    settings.gfxData.DrawLine(pen, x - errorCapSize, y + errorPx, x + errorCapSize, y + errorPx);
                }
            }
        }

        public override string ToString()
        {
            return $"PlottableBar with {pointCount} points";
        }
    }

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
            List<Point> linePoints = new List<Point>(visibleIndex2 - visibleIndex1 + 2);
            if (visibleIndex2 > ys.Length - 2)
                visibleIndex2 = ys.Length - 2;
            if (visibleIndex1 < 0)
                visibleIndex1 = 0;
            for (int i = visibleIndex1; i <= visibleIndex2 + 1; i++)
                linePoints.Add(settings.GetPixel(samplePeriod * i + xOffset, ys[i] + yOffset));

            settings.gfxData.DrawLines(pen, linePoints.ToArray());
            foreach (Point point in linePoints)
                settings.gfxData.FillEllipse(brush, point.X - markerSize / 2, point.Y - markerSize / 2, markerSize, markerSize);
        }

        private void RenderHighDensity(Settings settings, double offsetPoints, double columnPointCount)
        {
            List<Point> linePoints = new List<Point>(settings.dataSize.Width * 2 + 1);
            for (int xPx = 0; xPx < settings.dataSize.Width; xPx++)
            {
                // determine data indexes for this pixel column
                int index1 = (int)(offsetPoints + columnPointCount * xPx);
                int index2 = (int)(offsetPoints + columnPointCount * (xPx + 1));

                // skip invalid data index values
                if ((index2 < 0) || (index1 > ys.Length - 1))
                    continue;
                if (index1 < 0)
                    index1 = 0;
                if (index2 > ys.Length - 1)
                    index2 = ys.Length - 1;

                // get the min and max value for this column
                double lowestValue = ys[index1];
                double highestValue = ys[index1];
                for (int i = index1; i < index2; i++)
                {
                    if (ys[i] < lowestValue)
                        lowestValue = ys[i];
                    if (ys[i] > highestValue)
                        highestValue = ys[i];
                }
                int yPxHigh = settings.GetPixel(0, lowestValue + yOffset).Y;
                int yPxLow = settings.GetPixel(0, highestValue + yOffset).Y;

                // adjust order of points to enhance anti-aliasing
                if ((linePoints.Count < 2) || (yPxLow < linePoints[linePoints.Count - 1].Y))
                {
                    linePoints.Add(new Point(xPx, yPxLow));
                    linePoints.Add(new Point(xPx, yPxHigh));
                }
                else
                {
                    linePoints.Add(new Point(xPx, yPxHigh));
                    linePoints.Add(new Point(xPx, yPxLow));
                }
            }

            if (linePoints.Count > 0)
                settings.gfxData.DrawLines(pen, linePoints.ToArray());
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
    }


    public class PlottableScatter : Plottable
    {
        public double[] xs;
        public double[] ys;
        public double[] errorX;
        public double[] errorY;
        public float errorLineWidth;
        public float errorCapSize;
        public float markerSize;
        public Pen penLine;
        public Pen penLineError;
        public Brush brush;

        public PlottableScatter(double[] xs, double[] ys, Color color, double lineWidth, double markerSize, string label,
            double[] errorX, double[] errorY, double errorLineWidth, double errorCapSize)
        {
            if (xs.Length != ys.Length)
                throw new Exception("Xs and Ys must have same length");

            if (errorY != null)
                for (int i = 0; i < errorY.Length; i++)
                    if (errorY[i] < 0)
                        errorY[i] = -errorY[i];

            if (errorX != null)
                for (int i = 0; i < errorX.Length; i++)
                    if (errorX[i] < 0)
                        errorX[i] = -errorX[i];

            this.xs = xs;
            this.ys = ys;
            this.color = color;
            this.markerSize = (float)markerSize;
            this.label = label;
            this.errorX = errorX;
            this.errorY = errorY;
            this.errorLineWidth = (float)errorLineWidth;
            this.errorCapSize = (float)errorCapSize;


            pointCount = xs.Length;

            if (xs.Length != ys.Length)
                throw new ArgumentException("X and Y arrays must have the same length");

            if ((errorX != null) && (xs.Length != errorX.Length))
                throw new ArgumentException("errorX must be the same length as the original data");

            if ((errorY != null) && (xs.Length != errorY.Length))
                throw new ArgumentException("errorY must be the same length as the original data");

            penLine = new Pen(color, (float)lineWidth)
            {
                // this prevents sharp corners
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

            penLineError = new Pen(color, (float)errorLineWidth)
            {
                // this prevents sharp corners
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

            brush = new SolidBrush(color);
        }

        public override string ToString()
        {
            return $"PlottableScatter with {pointCount} points";
        }

        public override double[] GetLimits()
        {
            double[] limits = new double[4];

            if (errorX == null)
            {
                limits[0] = xs.Min();
                limits[1] = xs.Max();
            }
            else
            {
                limits[0] = xs[0] - errorX[0];
                limits[1] = xs[0] + errorX[0];
                for (int i = 1; i < xs.Length; i++)
                {
                    if (xs[i] - errorX[i] < limits[0])
                        limits[0] = xs[i] - errorX[i];
                    if (xs[i] + errorX[i] > limits[0])
                        limits[1] = xs[i] + errorX[i];
                }
            }

            if (errorY == null)
            {
                limits[2] = ys.Min();
                limits[3] = ys.Max();
            }
            else
            {
                limits[2] = ys[0] - errorY[0];
                limits[3] = ys[0] + errorY[0];
                for (int i = 1; i < ys.Length; i++)
                {
                    if (ys[i] - errorY[i] < limits[2])
                        limits[2] = ys[i] - errorY[i];
                    if (ys[i] + errorY[i] > limits[3])
                        limits[3] = ys[i] + errorY[i];
                }
            }
            return limits;
        }

        public override void Render(Settings settings)
        {
            Point[] points = new Point[xs.Length];
            for (int i = 0; i < xs.Length; i++)
                points[i] = settings.GetPixel(xs[i], ys[i]);

            if (errorY != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    PointF errorBelow = settings.GetPixel(xs[i], ys[i] - errorY[i]);
                    PointF errorAbove = settings.GetPixel(xs[i], ys[i] + errorY[i]);
                    float xCenter = errorBelow.X;
                    float yTop = errorAbove.Y;
                    float yBot = errorBelow.Y;
                    settings.gfxData.DrawLine(penLineError, xCenter, yBot, xCenter, yTop);
                    settings.gfxData.DrawLine(penLineError, xCenter - errorCapSize, yBot, xCenter + errorCapSize, yBot);
                    settings.gfxData.DrawLine(penLineError, xCenter - errorCapSize, yTop, xCenter + errorCapSize, yTop);
                }
            }

            if (errorX != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    PointF errorLeft = settings.GetPixel(xs[i] - errorX[i], ys[i]);
                    PointF errorRight = settings.GetPixel(xs[i] + errorX[i], ys[i]);
                    float yCenter = errorLeft.Y;
                    float xLeft = errorLeft.X;
                    float xRight = errorRight.X;
                    settings.gfxData.DrawLine(penLineError, xLeft, yCenter, xRight, yCenter);
                    settings.gfxData.DrawLine(penLineError, xLeft, yCenter - errorCapSize, xLeft, yCenter + errorCapSize);
                    settings.gfxData.DrawLine(penLineError, xRight, yCenter - errorCapSize, xRight, yCenter + errorCapSize);
                }
            }

            if (penLine.Width > 0)
                settings.gfxData.DrawLines(penLine, points);

            if (markerSize > 0)
                for (int i = 0; i < points.Length; i++)
                    settings.gfxData.FillEllipse(brush, points[i].X - markerSize / 2, points[i].Y - markerSize / 2, markerSize, markerSize);

        }
    }

    public class PlottableText : Plottable
    {
        public double x;
        public double y;
        public string text;
        public Brush brush;
        public Font font;

        public PlottableText(string text, double x, double y, Color color, string fontName, double fontSize, bool bold, string label)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            this.label = label;
            brush = new SolidBrush(color);
            FontStyle fontStyle = (bold == true) ? FontStyle.Bold : FontStyle.Regular;
            font = new Font(fontName, (float)fontSize, fontStyle);

            pointCount = 1;
        }

        public override string ToString()
        {
            return $"PlottableText \"{text}\" at ({x}, {y})";
        }

        public override double[] GetLimits()
        {
            return new double[] { x, x, y, y };
        }

        public override void Render(Settings settings)
        {
            settings.gfxData.DrawString(text, font, brush, settings.GetPixel(x, y));
        }
    }

    public class PlottableAxLine : Plottable
    {
        public double position;
        public bool vertical;
        public string orientation;
        public Pen pen;
        public bool draggable;

        public PlottableAxLine(double position, bool vertical, Color color, double lineWidth, string label, bool draggable)
        {
            this.position = position;
            this.vertical = vertical;
            this.label = label;
            this.draggable = draggable;
            orientation = (vertical) ? "vertical" : "horizontal";
            pen = new Pen(color, (float)lineWidth);
            pointCount = 1;
        }

        public override string ToString()
        {
            return $"PlottableAxLine ({orientation}) at {position}";
        }

        public override double[] GetLimits()
        {
            return new double[] { 0, 0, 0, 0 };
        }

        public override void Render(Settings settings)
        {
            Point pt1, pt2;

            if (vertical)
            {
                pt1 = settings.GetPixel(position, settings.axis[2]);
                pt2 = settings.GetPixel(position, settings.axis[3]);
            }
            else
            {
                pt1 = settings.GetPixel(settings.axis[0], position);
                pt2 = settings.GetPixel(settings.axis[1], position);
            }

            settings.gfxData.DrawLine(pen, pt1, pt2);
        }
    }
}
