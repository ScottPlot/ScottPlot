using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public class StarAxis
    {
        /// <summary>
        /// Values for every group (rows) and category (columns) normalized from 0 to 1.
        /// </summary>
        public double[,] Norm;

        /// <summary>
        /// Single value to normalize all values against for all groups/categories.
        /// </summary>
        public double NormMax;

        /// <summary>
        /// Individual values (one per category) to use for normalization.
        /// Length must be equal to the number of columns (categories) in the original data.
        /// </summary>
        public double[] NormMaxes;

        /// <summary>
        /// Labels for each category.
        /// Length must be equal to the number of columns (categories) in the original data.
        /// </summary>
        public string[] CategoryLabels;

        /// <summary>
        /// Controls rendering style of the concentric circles (ticks) of the web
        /// </summary>
        public RadarAxis AxisType { get; set; }

        /// <summary>
        /// Color of the axis lines and concentric circles representing ticks
        /// </summary>
        public Color WebColor { get; set; } = Color.Gray;

        /// <summary>
        /// Controls if values along each category axis are scaled independently or uniformly across all axes.
        /// </summary>
        public bool IndependentAxes;

        /// <summary>
        /// If true, each value will be written in text on the plot.
        /// </summary>
        public bool ShowAxisValues { get; set; } = true;

        public Graphics Graphics { get; set; }

        /// <summary>
        /// Font used for labeling values on the plot
        /// </summary>
        public Drawing.Font Font = new();
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            int numCategories = Norm.GetUpperBound(1) + 1;
            double sweepAngle = 2 * Math.PI / numCategories;
            double minScale = new double[] { dims.PxPerUnitX, dims.PxPerUnitX }.Min();
            PointF origin = new PointF(dims.GetPixelX(0), dims.GetPixelY(0));
            double[] radii = new double[] { 0.25 * minScale, 0.5 * minScale, 1 * minScale };

            using Pen pen = GDI.Pen(WebColor);
            using StringFormat sf = new StringFormat() { LineAlignment = StringAlignment.Center };
            using StringFormat sf2 = new StringFormat();
            using System.Drawing.Font font = GDI.Font(Font);
            using Brush fontBrush = GDI.Brush(Font.Color);

            for (int i = 0; i < radii.Length; i++)
            {
                double hypotenuse = (radii[i] / radii[radii.Length - 1]);

                if (AxisType == RadarAxis.Circle)
                {
                    Graphics.DrawEllipse(pen, (int)(origin.X - radii[i]), (int)(origin.Y - radii[i]), (int)(radii[i] * 2), (int)(radii[i] * 2));
                }
                else if (AxisType == RadarAxis.Polygon)
                {
                    PointF[] points = new PointF[numCategories];
                    for (int j = 0; j < numCategories; j++)
                    {
                        float x = (float)(hypotenuse * Math.Cos(sweepAngle * j - Math.PI / 2) * minScale + origin.X);
                        float y = (float)(hypotenuse * Math.Sin(sweepAngle * j - Math.PI / 2) * minScale + origin.Y);

                        points[j] = new PointF(x, y);
                    }
                    Graphics.DrawPolygon(pen, points);
                }
                if (ShowAxisValues)
                {
                    if (IndependentAxes)
                    {
                        for (int j = 0; j < numCategories; j++)
                        {
                            float x = (float)(hypotenuse * Math.Cos(sweepAngle * j - Math.PI / 2) * minScale + origin.X);
                            float y = (float)(hypotenuse * Math.Sin(sweepAngle * j - Math.PI / 2) * minScale + origin.Y);

                            sf2.Alignment = x < origin.X ? StringAlignment.Far : StringAlignment.Near;
                            sf2.LineAlignment = y < origin.Y ? StringAlignment.Far : StringAlignment.Near;

                            double val = NormMaxes[j] * radii[i] / minScale;
                            Graphics.DrawString($"{val:f1}", font, fontBrush, x, y, sf2);
                        }
                    }
                    else
                    {
                        double val = NormMax * radii[i] / minScale;
                        Graphics.DrawString($"{val:f1}", font, fontBrush, origin.X, (float)(-radii[i] + origin.Y), sf2);
                    }
                }
            }

            for (int i = 0; i < numCategories; i++)
            {
                PointF destination = new PointF((float)(1.1 * Math.Cos(sweepAngle * i - Math.PI / 2) * minScale + origin.X), (float)(1.1 * Math.Sin(sweepAngle * i - Math.PI / 2) * minScale + origin.Y));
                Graphics.DrawLine(pen, origin, destination);

                if (CategoryLabels != null)
                {
                    PointF textDestination = new PointF(
                        (float)(1.3 * Math.Cos(sweepAngle * i - Math.PI / 2) * minScale + origin.X),
                        (float)(1.3 * Math.Sin(sweepAngle * i - Math.PI / 2) * minScale + origin.Y));

                    if (Math.Abs(textDestination.X - origin.X) < 0.1)
                        sf.Alignment = StringAlignment.Center;
                    else
                        sf.Alignment = dims.GetCoordinateX(textDestination.X) < 0 ? StringAlignment.Far : StringAlignment.Near;
                    Graphics.DrawString(CategoryLabels[i], font, fontBrush, textDestination, sf);
                }
            }
        }
    }
}
