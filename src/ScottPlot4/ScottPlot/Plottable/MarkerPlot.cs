using System.Drawing;

namespace ScottPlot.Plottable
{
    public class MarkerPlot : IPlottable, IHasMarker, IHasColor
    {
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        /// <summary>
        /// Horizontal position in coordinate space
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Vertical position in coordinate space
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Marker to draw at this point
        /// </summary>
        public MarkerShape MarkerShape { get; set; } = MarkerShape.filledCircle;

        /// <summary>
        /// Size of the marker in pixel units
        /// </summary>
        public float MarkerSize { get; set; } = 10;

        /// <summary>
        /// Thickness of the marker lines in pixel units
        /// </summary>
        public float MarkerLineWidth { get; set; } = 1;

        /// <summary>
        /// Color of the marker to display at this point
        /// </summary>
        public Color Color { get; set; }
        public Color MarkerColor { get => Color; set { Color = value; } }

        /// <summary>
        /// Text to appear in the legend (if populated)
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Text to appear on the graph at the point
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Font settings for rendering <see cref="Text"/>.
        /// Alignment and orientation relative to the marker can be configured here.
        /// </summary>
        public Drawing.Font TextFont = new();

        public AxisLimits GetAxisLimits()
        {
            return new AxisLimits(X, X, Y, Y);
        }

        public LegendItem[] GetLegendItems()
        {
            LegendItem singleItem = new(this)
            {
                label = Label,
                markerShape = MarkerShape,
                markerSize = MarkerSize,
                color = Color
            };

            return LegendItem.Single(singleItem);
        }

        public void ValidateData(bool deep = false)
        {
            Validate.AssertIsReal(nameof(X), X);
            Validate.AssertIsReal(nameof(Y), Y);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!IsVisible)
                return;

            PointF point = new(dims.GetPixelX(X), dims.GetPixelY(Y));

            using Graphics gfx = Drawing.GDI.Graphics(bmp, dims, lowQuality);
            MarkerTools.DrawMarker(gfx, point, MarkerShape, (float)MarkerSize, Color, MarkerLineWidth);

            if (!string.IsNullOrEmpty(Text))
            {
                SizeF stringSize = Drawing.GDI.MeasureString(gfx, Text, TextFont.Name, TextFont.Size, TextFont.Bold);
                gfx.TranslateTransform(point.X, point.Y);
                gfx.RotateTransform(TextFont.Rotation);

                (float dX, float dY) = Drawing.GDI.TranslateString(gfx, Text, TextFont);
                gfx.TranslateTransform(-dX, -dY);

                using var font = Drawing.GDI.Font(TextFont);
                using var fontBrush = new SolidBrush(TextFont.Color);
                gfx.DrawString(Text, font, fontBrush, new PointF(0, 0));

                Drawing.GDI.ResetTransformPreservingScale(gfx, dims);
            }
        }
    }
}
