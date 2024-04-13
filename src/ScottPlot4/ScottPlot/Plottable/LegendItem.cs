using ScottPlot.Drawing;
using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// This class describes a single item that appears in the figure legend.
    /// </summary>
    public class LegendItem
    {
        public string label { get; set; }
        public Color color { get; set; }
        public Color hatchColor { get; set; }
        public Color borderColor { get; set; }
        public float borderWith { get; set; }
        public LineStyle borderLineStyle { get; set; }

        public LineStyle lineStyle { get; set; }

        private double _lineWidth { get; set; }
        public double lineWidth
        {
            get => (Parent is IHasLine parent) ? Math.Min(parent.LineWidth, 10) : _lineWidth;
            set { _lineWidth = value; }
        }
        public Color LineColor => Parent is IHasLine p ? p.LineColor : color;

        public MarkerShape markerShape { get; set; }
        private float _markerSize = 0;
        public float markerSize
        {
            get => (Parent is IHasMarker parent) ? parent.MarkerSize : _markerSize;
            set { _markerSize = value; }
        }

        public float markerLineWidth =>
            Parent is IHasMarker parent ? Math.Min(parent.MarkerLineWidth, 3) : (float)lineWidth;

        public Color MarkerColor =>
            Parent is IHasMarker parent ? parent.MarkerColor : color;

        public HatchStyle hatchStyle;
        public bool ShowAsRectangleInLegend
        {
            get
            {
                bool hasVeryLargeLineWidth = lineWidth >= 10;
                bool hasArea = (Parent is not null) && (Parent is IHasArea);
                return hasVeryLargeLineWidth || hasArea;
            }
        }

        public readonly IPlottable Parent;

        public LegendItem(IPlottable parent)
        {
            Parent = parent;
        }

        public static LegendItem[] Single(LegendItem item)
        {
            return new LegendItem[] { item };
        }

        public static LegendItem[] Single(IPlottable parent, string label, Color color)
        {
            return new LegendItem[] { new LegendItem(parent) { label = label, color = color } };
        }

        public static LegendItem[] None => Array.Empty<LegendItem>();

        public void Render(Graphics gfx, float x, float y,
            float labelWidth, float labelHeight, System.Drawing.Font labelFont,
            float symbolWidth, float symbolPad,
            Pen outlinePen, SolidBrush textBrush, Brush legendItemHideBrush)
        {
            // draw text
            gfx.DrawString(label, labelFont, textBrush, x + symbolWidth, y);

            // prepare values for drawing a line
            outlinePen.Color = color;
            outlinePen.Width = 1;
            float lineY = y + labelHeight / 2;
            float lineX1 = x + symbolPad;
            float lineX2 = lineX1 + symbolWidth - symbolPad * 2;

            if (ShowAsRectangleInLegend)
            {
                // prepare values for drawing a rectangle
                PointF rectOrigin = new PointF(lineX1, (float)(lineY - 5));
                SizeF rectSize = new SizeF(lineX2 - lineX1, 10);
                RectangleF rect = new RectangleF(rectOrigin, rectSize);

                // draw a rectangle
                using (var legendItemFillBrush = GDI.Brush(color, hatchColor, hatchStyle))
                using (var legendItemOutlinePen = GDI.Pen(borderColor, borderWith, borderLineStyle))
                {
                    gfx.FillRectangle(legendItemFillBrush, rect);
                    gfx.DrawRectangle(legendItemOutlinePen, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
            else
            {
                // draw a line
                if (lineWidth > 0 && lineStyle != LineStyle.None)
                {
                    using var linePen = GDI.Pen(LineColor, lineWidth, lineStyle, false);
                    gfx.DrawLine(linePen, lineX1, lineY, lineX2, lineY);
                }

                // and perhaps a marker in the middle of the line
                float lineXcenter = (lineX1 + lineX2) / 2;
                PointF markerPoint = new PointF(lineXcenter, lineY);
                if ((markerShape != MarkerShape.none) && (markerSize > 0))
                    MarkerTools.DrawMarker(gfx, markerPoint, markerShape, markerSize, MarkerColor, markerLineWidth);
            }

            // Typically invisible legend items don't make it in the list.
            // If they do, display them simulating semi-transparency by drawing a white box over the legend item
            if (!Parent.IsVisible)
            {
                PointF hideRectOrigin = new(lineX1, y);
                SizeF hideRectSize = new(symbolWidth + labelWidth + symbolPad, labelHeight);
                RectangleF hideRect = new(hideRectOrigin, hideRectSize);
                gfx.FillRectangle(legendItemHideBrush, hideRect);
            }
        }
    }
}
