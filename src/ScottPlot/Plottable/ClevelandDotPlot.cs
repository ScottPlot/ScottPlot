using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    public class ClevelandDotPlot : BarPlotBase
    {
        public Color StemColor = Color.Gray;
        public float DotRadius { get; set; } = 5;

        private string Label1;
        private Color Color1 = Color.Green;
        private MarkerShape MarkerShape1 = MarkerShape.filledCircle;

        private string Label2;
        private Color Color2 = Color.Red;
        private MarkerShape MarkerShape2 = MarkerShape.filledCircle;

        public void SetDot1Style(Color? color = null, MarkerShape? markerShape = null, string? label = null)
        {
            Label1 = label ?? Label1;
            MarkerShape1 = markerShape ?? MarkerShape1;
            Color1 = color ?? Color1;
        }

        public void SetDot2Style(Color? color = null, MarkerShape? markerShape = null, string? label = null)
        {
            Label2 = label ?? Label2;
            MarkerShape2 = markerShape ?? MarkerShape2;
            Color2 = color ?? Color2;
        }

        public ClevelandDotPlot(double[] xs, double[] ys1, double[] ys2) : base()
        {
            this.Ys = ys2.Select((y, i) => y - ys1[i]).ToArray();
            this.YOffsets = ys1;
            this.Xs = xs;
            this.YErrors = DataGen.Zeros(ys1.Length);
        }

        public override AxisLimits GetAxisLimits()
        {
            double valueMin = double.PositiveInfinity;
            double valueMax = double.NegativeInfinity;
            double positionMin = double.PositiveInfinity;
            double positionMax = double.NegativeInfinity;

            for (int i = 0; i < Xs.Length; i++)
            {
                valueMin = new double[] { valueMin, Ys[i] - YErrors[i] + YOffsets[i], YOffsets[i] }.Min();
                valueMax = new double[] { valueMin, Ys[i] + YErrors[i] + YOffsets[i], YOffsets[i] }.Max();
                positionMin = Math.Min(positionMin, Xs[i]);
                positionMax = Math.Max(positionMax, Xs[i]);
            }

            valueMin = Math.Min(valueMin, BaseValue);
            valueMax = Math.Max(valueMax, BaseValue);

            if (ShowValuesAboveBars)
                valueMax += (valueMax - valueMin) * .1; // increase by 10% to accomodate label

            positionMin -= BarWidth / 2;
            positionMax += BarWidth / 2;

            positionMin += XOffset;
            positionMax += XOffset;

            return VerticalOrientation ?
                new AxisLimits(positionMin, positionMax, valueMin, valueMax) :
                new AxisLimits(valueMin, valueMax, positionMin, positionMax);
        }

        public override LegendItem[] GetLegendItems()
        {
            var firstDot = new LegendItem()
            {
                label = Label1,
                color = Color1,
                lineStyle = LineStyle.None,
                markerShape = MarkerShape1,
                markerSize = 5,
            };
            var secondDot = new LegendItem()
            {
                label = Label2,
                color = Color2,
                lineStyle = LineStyle.None,
                markerShape = MarkerShape2,
                markerSize = 5,
            };

            return new LegendItem[] { firstDot, secondDot };
        }

        protected override void RenderBarFromRect(RectangleF rect, bool negative, Graphics gfx)
        {
            float centerPx = HorizontalOrientation ? rect.Y + rect.Height / 2 : rect.X + rect.Width / 2;
            using (var stemPen = new Pen(StemColor))
            {
                using (var dot1Brush = GDI.Brush(Color1))
                using (var dot2Brush = GDI.Brush(Color2))
                {
                    PointF[] points = new PointF[2];
                    if (HorizontalOrientation)
                    {
                        points[0] = new PointF(negative ? rect.X + rect.Width : rect.X, centerPx - DotRadius / 2);
                        points[1] = new PointF(negative ? rect.X : rect.X + rect.Width, centerPx - DotRadius / 2);
                    }
                    else
                    {
                        points[0] = new PointF(centerPx - DotRadius / 2, !negative ? rect.Y + rect.Height : rect.Y);
                        points[1] = new PointF(centerPx - DotRadius / 2, !negative ? rect.Y : rect.Y + rect.Height);
                    }

                    gfx.DrawLine(stemPen, points[0], points[1]);
                    MarkerTools.DrawMarker(gfx, points[1], MarkerShape2, DotRadius, Color2);
                    MarkerTools.DrawMarker(gfx, points[0], MarkerShape1, DotRadius, Color1); // First point should be drawn overtop the second.
                }

            }
        }
    }
}
