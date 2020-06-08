using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ScottPlot.Config;
using ScottPlot.Drawing;

namespace ScottPlot
{
    public class PlottableHLine : Plottable, IDraggable
    {
        public double position;
        public LineStyle lineStyle;
        public Color color;
        public string label;

        private Pen pen;

        public PlottableHLine(double position, Color color, double lineWidth, string label,
            bool draggable, double dragLimitLower, double dragLimitUpper, LineStyle lineStyle)
        {
            this.position = position;
            this.color = color;
            this.label = label;
            this.lineStyle = lineStyle;

            pen = GDI.Pen(color, (float)lineWidth, lineStyle, true);

            DragEnabled = draggable;

            SetLimits(x1: double.NegativeInfinity, x2: double.PositiveInfinity, y1: dragLimitLower, y2: dragLimitUpper);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableHLine{label} at Y={position}";
        }

        public override AxisLimits2D GetLimits()
        {
            return new AxisLimits2D(double.NaN, double.NaN, position, position);
        }

        public override void Render(Settings settings)
        {
            PointF pt1, pt2;

            pt1 = settings.GetPixel(settings.axes.x.min, position);
            pt2 = settings.GetPixel(settings.axes.x.max, position);
            pt2.X -= (float).01; // fixes a System.Drawing bug

            settings.gfxData.DrawLine(pen, pt1, pt2);
        }

        public bool DragEnabled { get; set; }

        private double dragLimitY1 = double.NegativeInfinity;
        private double dragLimitY2 = double.PositiveInfinity;
        public void SetLimits(double? x1, double? x2, double? y1, double? y2)
        {
            if (y1 != null) dragLimitY1 = (double)y1;
            if (y2 != null) dragLimitY2 = (double)y2;
        }

        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            double distanceFromMouseY = Math.Abs(position - coordinateY);
            return (distanceFromMouseY <= snapY);
        }

        public void DragTo(double coordinateX, double coordinateY)
        {
            if (DragEnabled)
            {
                if (coordinateY < dragLimitY1) coordinateY = dragLimitY1;
                if (coordinateY > dragLimitY2) coordinateY = dragLimitY2;
                position = coordinateY;
            }
        }

        public override int GetPointCount()
        {
            return 1;
        }

        public Cursor DragCursor => Cursor.NS;

        public override LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new Config.LegendItem(label, color, lineStyle, pen.Width, MarkerShape.none);
            return new LegendItem[] { singleLegendItem };
        }
    }
}
