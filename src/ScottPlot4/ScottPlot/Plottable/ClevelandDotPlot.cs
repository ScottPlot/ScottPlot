using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Cleveland Dot plots display a series of paired p[oints. 
    /// Positions are defined by Xs.
    /// Heights are defined by Ys1 and Ys2 (internally done with Ys and YOffsets).
    /// </summary>
    public class ClevelandDotPlot : BarPlotBase, IPlottable
    {
        /// <summary>
        /// Color for the line
        /// </summary>
        public Color StemColor { get; set; } = Color.Gray;

        /// <summary>
        /// Color for markers placed at <see cref="Ys1"/>
        /// </summary>
        public Color Point1Color { get => Color1; set => Color1 = value; }

        /// <summary>
        /// Color for markers placed at <see cref="Ys2"/>
        /// </summary>
        public Color Point2Color { get => Color2; set => Color2 = value; }

        /// <summary>
        /// Size of the markers at the ends of each line
        /// </summary>
        public float DotRadius { get; set; } = 5;

        /// <summary>
        /// Width of the stem (in pixels)
        /// </summary>
        public float LineWidth { get; set; } = 1;

        // TODO: don't expose these, instead put them behind an Update() method
        // that lets the user update one or both arrays. This can also perform length checking.
        public double[] Ys1
        {
            get => ValueOffsets;
            set
            {
                double[] diff = (Ys1 ?? DataGen.Zeros(value.Length)).Zip(value, (y, v) => y - v).ToArray();
                ValueOffsets = value;

                if (Ys2 != null)
                    Ys2 = (Ys2 ?? DataGen.Zeros(value.Length)).Zip(diff, (y, v) => y + v).ToArray();
            }
        }

        public double[] Ys2
        {
            get
            {
                if (Values == null)
                    return null;

                double[] offsets = Ys1 ?? DataGen.Zeros(Values.Length);
                return Values.Select((y, i) => y + offsets[i]).ToArray();
            }
            set
            {
                double[] offsets = Ys1 ?? DataGen.Zeros(value.Length);
                Values = value.Select((y, i) => y - offsets[i]).ToArray();
            }
        }

        /// <summary>
        /// Text to display in the legend associated with the series 1 data
        /// </summary>
        private string Label { get; set; } = null;

        /// <summary>
        /// Color for one of the markers
        /// </summary>
        private Color Color1 { get; set; } = Color.Green;

        /// <summary>
        /// Marker to use for the series 1 data
        /// </summary>
        private MarkerShape MarkerShape1 { get; set; } = MarkerShape.filledCircle;

        /// <summary>
        /// Text to display in the legend associated with the series 2 data
        /// </summary>
        private string Label2 { get; set; }

        /// <summary>
        /// Color for one of the markers
        /// </summary>
        private Color Color2 { get; set; } = Color.Red;

        /// <summary>
        /// Marker to use for the series 2 data
        /// </summary>
        private MarkerShape MarkerShape2 { get; set; } = MarkerShape.filledCircle;

        /// <summary>
        /// Allows customizing the first point (set by ys1)
        /// </summary>
        /// <param name="color">The color of the dot, null for no change.</param>
        /// <param name="markerShape">The shape of the dot, null for no change.</param>
        /// <param name="label">The label of the dot in the legend, null for no change</param>
        public void SetPoint1Style(Color? color = null, MarkerShape? markerShape = null, string label = null)
        {
            Label = label ?? Label;
            MarkerShape1 = markerShape ?? MarkerShape1;
            Color1 = color ?? Color1;
        }

        /// <summary>
        /// Allows customizing the second point (set by ys2)
        /// </summary>
        /// <param name="color">The color of the dot, null for no change.</param>
        /// <param name="markerShape">The shape of the dot, null for no change.</param>
        /// <param name="label">The label of the dot in the legend, null for no change</param>
        public void SetPoint2Style(Color? color = null, MarkerShape? markerShape = null, string label = null)
        {
            Label2 = label ?? Label2;
            MarkerShape2 = markerShape ?? MarkerShape2;
            Color2 = color ?? Color2;
        }

        public ClevelandDotPlot(double[] xs, double[] ys1, double[] ys2) : base()
        {
            Ys1 = ys1;
            Ys2 = ys2;
            Positions = xs;
            ValueErrors = DataGen.Zeros(ys1.Length);
        }

        public override AxisLimits GetAxisLimits()
        {
            double valueMin = double.PositiveInfinity;
            double valueMax = double.NegativeInfinity;
            double positionMin = double.PositiveInfinity;
            double positionMax = double.NegativeInfinity;

            for (int i = 0; i < Positions.Length; i++)
            {
                valueMin = new double[] { valueMin, Values[i] - ValueErrors[i] + ValueOffsets[i], ValueOffsets[i] }.Min();
                valueMax = new double[] { valueMax, Values[i] + ValueErrors[i] + ValueOffsets[i], ValueOffsets[i] }.Max();
                positionMin = Math.Min(positionMin, Positions[i]);
                positionMax = Math.Max(positionMax, Positions[i]);
            }

            valueMin = Math.Min(valueMin, ValueBase);
            valueMax = Math.Max(valueMax, ValueBase);

            if (ShowValuesAboveBars)
                valueMax += (valueMax - valueMin) * .1; // increase by 10% to accomodate label

            positionMin -= BarWidth / 2;
            positionMax += BarWidth / 2;

            positionMin += PositionOffset;
            positionMax += PositionOffset;

            return Orientation == Orientation.Vertical ?
                new AxisLimits(positionMin, positionMax, valueMin, valueMax) :
                new AxisLimits(valueMin, valueMax, positionMin, positionMax);
        }

        public LegendItem[] GetLegendItems()
        {
            var firstDot = new LegendItem(this)
            {
                label = Label,
                color = Color1,
                lineStyle = LineStyle.None,
                markerShape = MarkerShape1,
                markerSize = 5,
            };
            var secondDot = new LegendItem(this)
            {
                label = Label2,
                color = Color2,
                lineStyle = LineStyle.None,
                markerShape = MarkerShape2,
                markerSize = 5,
            };

            return new LegendItem[] { firstDot, secondDot };
        }

        public void ValidateData(bool deep = false) { }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            for (int barIndex = 0; barIndex < Values.Length; barIndex++)
            {
                if (Orientation == Orientation.Vertical)
                    RenderBarVertical(dims, gfx, Positions[barIndex] + PositionOffset, Values[barIndex], ValueErrors[barIndex], ValueOffsets[barIndex]);
                else
                    RenderBarHorizontal(dims, gfx, Positions[barIndex] + PositionOffset, Values[barIndex], ValueErrors[barIndex], ValueOffsets[barIndex]);
            }
        }

        private void RenderBarFromRect(RectangleF rect, bool negative, Graphics gfx)
        {
            float centerPx = Orientation == Orientation.Horizontal
                ? rect.Y + rect.Height / 2
                : rect.X + rect.Width / 2;

            using var stemPen = new Pen(StemColor, LineWidth);
            using var dot1Brush = GDI.Brush(Color1);
            using var dot2Brush = GDI.Brush(Color2);
            PointF[] points = new PointF[2];
            if (Orientation == Orientation.Horizontal)
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

        private void RenderBarVertical(PlotDimensions dims, Graphics gfx, double position, double value, double valueError, double yOffset)
        {
            // bar body
            float centerPx = dims.GetPixelX(position);
            double edge1 = position - BarWidth / 2;
            double value1 = Math.Min(ValueBase, value) + yOffset;
            double value2 = Math.Max(ValueBase, value) + yOffset;
            double valueSpan = value2 - value1;

            var rect = new RectangleF(
                x: dims.GetPixelX(edge1),
                y: dims.GetPixelY(value2),
                width: (float)(BarWidth * dims.PxPerUnitX),
                height: (float)(valueSpan * dims.PxPerUnitY));

            // errorbar
            double error1 = value > 0 ? value2 - Math.Abs(valueError) : value1 - Math.Abs(valueError);
            double error2 = value > 0 ? value2 + Math.Abs(valueError) : value1 + Math.Abs(valueError);
            float capPx1 = dims.GetPixelX(position - ErrorCapSize * BarWidth / 2);
            float capPx2 = dims.GetPixelX(position + ErrorCapSize * BarWidth / 2);
            float errorPx2 = dims.GetPixelY(error2);
            float errorPx1 = dims.GetPixelY(error1);

            RenderBarFromRect(rect, value < 0, gfx);

            if (ErrorLineWidth > 0 && valueError > 0)
            {
                using var errorPen = new Pen(ErrorColor, ErrorLineWidth);
                gfx.DrawLine(errorPen, centerPx, errorPx1, centerPx, errorPx2);
                gfx.DrawLine(errorPen, capPx1, errorPx1, capPx2, errorPx1);
                gfx.DrawLine(errorPen, capPx1, errorPx2, capPx2, errorPx2);
            }

            if (ShowValuesAboveBars)
                using (var valueTextFont = GDI.Font(Font))
                using (var valueTextBrush = GDI.Brush(Font.Color))
                using (var sf = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center })
                    gfx.DrawString(ValueFormatter(value), valueTextFont, valueTextBrush, centerPx, rect.Y, sf);
        }

        private void RenderBarHorizontal(PlotDimensions dims, Graphics gfx, double position, double value, double valueError, double yOffset)
        {
            // bar body
            float centerPx = dims.GetPixelY(position);
            double edge2 = position + BarWidth / 2;
            double value1 = Math.Min(ValueBase, value) + yOffset;
            double value2 = Math.Max(ValueBase, value) + yOffset;
            double valueSpan = value2 - value1;
            var rect = new RectangleF(
                x: dims.GetPixelX(value1),
                y: dims.GetPixelY(edge2),
                height: (float)(BarWidth * dims.PxPerUnitY),
                width: (float)(valueSpan * dims.PxPerUnitX));

            RenderBarFromRect(rect, value < 0, gfx);

            // errorbar
            double error1 = value > 0 ? value2 - Math.Abs(valueError) : value1 - Math.Abs(valueError);
            double error2 = value > 0 ? value2 + Math.Abs(valueError) : value1 + Math.Abs(valueError);
            float capPx1 = dims.GetPixelY(position - ErrorCapSize * BarWidth / 2);
            float capPx2 = dims.GetPixelY(position + ErrorCapSize * BarWidth / 2);
            float errorPx2 = dims.GetPixelX(error2);
            float errorPx1 = dims.GetPixelX(error1);

            if (ErrorLineWidth > 0 && valueError > 0)
            {
                using var errorPen = new Pen(ErrorColor, ErrorLineWidth);
                gfx.DrawLine(errorPen, errorPx1, centerPx, errorPx2, centerPx);
                gfx.DrawLine(errorPen, errorPx1, capPx2, errorPx1, capPx1);
                gfx.DrawLine(errorPen, errorPx2, capPx2, errorPx2, capPx1);
            }

            if (ShowValuesAboveBars)
                using (var valueTextFont = GDI.Font(Font))
                using (var valueTextBrush = GDI.Brush(Font.Color))
                using (var sf = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near })
                    gfx.DrawString(ValueFormatter(value), valueTextFont, valueTextBrush, rect.X + rect.Width, centerPx, sf);
        }
    }
}
