using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScottPlot
{
    public class PlottableRadar : Plottable
    {
        private double[,] values;
        public string[] categoryNames;
        public string[] groupNames;
        public Color[] colors;
        private SolidBrush brush = new SolidBrush(Color.Black);
        private Pen pen = new Pen(Color.Black);
        private double max;

        private double[,] Normalize(double[,] input)
        {
            double[,] output = new double[input.GetLength(0), input.GetLength(1)];
            double max = input[0, 0];

            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    if (input[i, j] > max)
                    {
                        max = input[i, j];
                    }
                }
            }

            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    output[i, j] = input[i, j] / max; //This is not a true normalize, this creates a number on the interval [min/max, 1], not [0,1]. It looks better for this plot
                }
            }

            this.max = max;
            return output;
        }

        public PlottableRadar(double[,] values, string[] categoryNames, string[] groupNames, Color[] colors)
        {
            this.values = Normalize(values);
            this.categoryNames = categoryNames;
            this.groupNames = groupNames;
            this.colors = colors;
        }

        public override LegendItem[] GetLegendItems()
        {
            if (groupNames is null)
                return null;

            var items = new LegendItem[groupNames.Length];
            for (int i = 0; i < groupNames.Length; i++)
            {
                items[i] = new LegendItem(groupNames[i], colors[i], lineWidth: 10);
            }
            return items;
        }

        public override AxisLimits2D GetLimits()
        {
            return new AxisLimits2D(-1.5, 1.5, -1.5, 1.5);
        }

        public override int GetPointCount()
        {
            return values.Length;
        }

        public override void Render(Settings settings)
        {
            int numGroups = values.GetUpperBound(0) + 1;
            int numCategories = values.GetUpperBound(1) + 1;
            double sweepAngle = 2 * Math.PI / numCategories;
            double minScale = new double[] { settings.xAxisScale, settings.yAxisScale }.Min();
            PointF origin = settings.GetPixel(0, 0);

            double[] radii = new double[] { 0.25 * minScale, 0.5 * minScale, 1 * minScale };
            pen.Color = Color.Gray;
            for (int i = 0; i < radii.Length; i++)
            {
                settings.gfxData.DrawEllipse(pen, (int)(origin.X - radii[i]), (int)(origin.Y - radii[i]), (int)(radii[i] * 2), (int)(radii[i] * 2));
                brush.Color = Color.Black;
                StringFormat stringFormat = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far };
                settings.gfxData.DrawString($"{max * radii[i] / minScale:f1}", new Font(FontFamily.GenericSansSerif, 8), brush, origin.X, (float)(-radii[i] + origin.Y), stringFormat);
            }

            for (int i = 0; i < numCategories; i++)
            {
                PointF destination = new PointF((float)(1.1 * Math.Cos(sweepAngle * i - Math.PI / 2) * minScale + origin.X), (float)(1.1 * Math.Sin(sweepAngle * i - Math.PI / 2) * minScale + origin.Y));
                settings.gfxData.DrawLine(pen, origin, destination);

                if (categoryNames != null)
                {
                    PointF textDestination = new PointF((float)(1.3 * Math.Cos(sweepAngle * i - Math.PI / 2) * minScale + origin.X), (float)(1.3 * Math.Sin(sweepAngle * i - Math.PI / 2) * minScale + origin.Y));
                    StringAlignment alignment;
                    if (Math.Abs(textDestination.X - origin.X) < 0.1)
                    {
                        alignment = StringAlignment.Center;
                    }
                    else
                    {
                        alignment = settings.GetLocationX(textDestination.X) < 0 ? StringAlignment.Far : StringAlignment.Near;
                    }
                    StringFormat stringFormat = new StringFormat() { Alignment = alignment, LineAlignment = StringAlignment.Center };
                    settings.gfxData.DrawString(categoryNames[i], new Font(FontFamily.GenericSansSerif, 12), brush, textDestination, stringFormat);
                }
            }

            for (int i = 0; i < numGroups; i++)
            {
                PointF[] points = new PointF[numCategories];
                for (int j = 0; j < numCategories; j++)
                {
                    points[j] = new PointF((float)(values[i, j] * Math.Cos(sweepAngle * j - Math.PI / 2) * minScale + origin.X), (float)(values[i, j] * Math.Sin(sweepAngle * j - Math.PI / 2) * minScale + origin.Y));
                }

                brush.Color = Color.FromArgb(colors[i].ToArgb() - (0xD0 << 24)); //Drop the opacity
                pen.Color = colors[i];
                settings.gfxData.FillPolygon(brush, points);
                settings.gfxData.DrawPolygon(pen, points);
            }
        }

        public override string ToString()
        {
            return $"PlottableRadar with {GetPointCount()} points and {values.GetUpperBound(1) + 1} categories.";
        }
    }
}
