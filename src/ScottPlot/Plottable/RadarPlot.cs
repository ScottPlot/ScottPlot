using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Drawing;

namespace ScottPlot.Plottable
{
    public class RadarPlot : IPlottable
    {
        private readonly double[,] normalized;
        private readonly double normalizedMax;
        public string[] categoryNames;
        public string[] groupNames;
        public Color[] fillColors;
        public Color[] lineColors;
        public Color webColor;
        public bool IsVisible { get; set; } = true;
        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;

        public RadarPlot(double[,] values, Color[] lineColors, Color[] fillColors)
        {
            this.lineColors = lineColors;
            this.fillColors = fillColors;

            normalized = new double[values.GetLength(0), values.GetLength(1)];
            Array.Copy(values, 0, normalized, 0, values.Length);
            normalizedMax = NormalizeInPlace(normalized);
        }

        public override string ToString() =>
            $"PlottableRadar with {PointCount} points and {normalized.GetUpperBound(1) + 1} categories.";

        public void ValidateData(bool deep = false)
        {
            if (groupNames != null && groupNames.Length != normalized.GetLength(0))
                throw new InvalidOperationException("group names must match size of values");

            if (categoryNames != null && categoryNames.Length != normalized.GetLength(1))
                throw new InvalidOperationException("category names must match size of values");
        }

        /// <summary>
        /// Normalize a 2D array by dividing all values by the maximum value.
        /// </summary>
        /// <returns>maximum value in the array before normalization</returns>
        private double NormalizeInPlace(double[,] input)
        {
            double max = input[0, 0];

            for (int i = 0; i < input.GetLength(0); i++)
                for (int j = 0; j < input.GetLength(1); j++)
                    max = Math.Max(max, input[i, j]);

            for (int i = 0; i < input.GetLength(0); i++)
                for (int j = 0; j < input.GetLength(1); j++)
                    input[i, j] /= max;

            return max;
        }

        public LegendItem[] GetLegendItems()
        {
            if (groupNames is null)
                return null;

            List<LegendItem> legendItems = new List<LegendItem>();
            for (int i = 0; i < groupNames.Length; i++)
            {
                var item = new LegendItem()
                {
                    label = groupNames[i],
                    color = fillColors[i],
                    lineWidth = 10,
                    markerShape = MarkerShape.none
                };
                legendItems.Add(item);
            }

            return legendItems.ToArray();
        }

        public AxisLimits GetAxisLimits() => new AxisLimits(-1.5, 1.5, -1.5, 1.5);

        public int PointCount { get => normalized.Length; }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            int numGroups = normalized.GetUpperBound(0) + 1;
            int numCategories = normalized.GetUpperBound(1) + 1;
            double sweepAngle = 2 * Math.PI / numCategories;
            double minScale = new double[] { dims.PxPerUnitX, dims.PxPerUnitX }.Min();
            PointF origin = new PointF(dims.GetPixelX(0), dims.GetPixelY(0));
            double[] radii = new double[] { 0.25 * minScale, 0.5 * minScale, 1 * minScale };

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen pen = GDI.Pen(webColor))
            using (Brush brush = GDI.Brush(Color.Black))
            using (StringFormat sf = new StringFormat() { LineAlignment = StringAlignment.Center })
            using (var font = GDI.Font())
            {
                for (int i = 0; i < radii.Length; i++)
                {
                    gfx.DrawEllipse(pen, (int)(origin.X - radii[i]), (int)(origin.Y - radii[i]), (int)(radii[i] * 2), (int)(radii[i] * 2));
                    StringFormat stringFormat = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far };
                    gfx.DrawString($"{normalizedMax * radii[i] / minScale:f1}", GDI.Font(null, 8), brush, origin.X, (float)(-radii[i] + origin.Y), stringFormat);
                }

                for (int i = 0; i < numCategories; i++)
                {
                    PointF destination = new PointF((float)(1.1 * Math.Cos(sweepAngle * i - Math.PI / 2) * minScale + origin.X), (float)(1.1 * Math.Sin(sweepAngle * i - Math.PI / 2) * minScale + origin.Y));
                    gfx.DrawLine(pen, origin, destination);

                    if (categoryNames != null)
                    {
                        PointF textDestination = new PointF(
                            (float)(1.3 * Math.Cos(sweepAngle * i - Math.PI / 2) * minScale + origin.X),
                            (float)(1.3 * Math.Sin(sweepAngle * i - Math.PI / 2) * minScale + origin.Y));

                        if (Math.Abs(textDestination.X - origin.X) < 0.1)
                            sf.Alignment = StringAlignment.Center;
                        else
                            sf.Alignment = dims.GetCoordinateX(textDestination.X) < 0 ? StringAlignment.Far : StringAlignment.Near;
                        gfx.DrawString(categoryNames[i], font, brush, textDestination, sf);
                    }
                }

                for (int i = 0; i < numGroups; i++)
                {
                    PointF[] points = new PointF[numCategories];
                    for (int j = 0; j < numCategories; j++)
                        points[j] = new PointF(
                            (float)(normalized[i, j] * Math.Cos(sweepAngle * j - Math.PI / 2) * minScale + origin.X),
                            (float)(normalized[i, j] * Math.Sin(sweepAngle * j - Math.PI / 2) * minScale + origin.Y));

                    ((SolidBrush)brush).Color = fillColors[i];
                    pen.Color = lineColors[i];
                    gfx.FillPolygon(brush, points);
                    gfx.DrawPolygon(pen, points);
                }
            }
        }
    }
}
