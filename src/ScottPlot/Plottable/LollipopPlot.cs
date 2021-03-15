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
    /// Lollipop plots display a series of "Lollipops" in place of bars. 
    /// Positions are defined by Xs.
    /// Heights are defined by Ys (relative to BaseValue and YOffsets).
    /// </summary>
    public class LollipopPlot : BarPlotBase
    {
        /// <summary>
        /// Name for this series of values that will appear in the legend
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Color of all lollipop components (the stick and the circle)
        /// </summary>
        public Color LollipopColor { get; set; }

        /// <summary>
        /// Size of the circle at the end of each lollipop
        /// </summary>
        public float LollipopRadius { get; set; } = 5;

        /// <summary>
        /// Create a lollipop plot from arrays of positions and sizes
        /// </summary>
        /// <param name="positions">position of each lollipop</param>
        /// <param name="values">height of each lollipop</param>
        public LollipopPlot(double[] positions, double[] values) : base()
        {
            if (positions is null || positions.Length == 0 || values is null || values.Length == 0)
                throw new InvalidOperationException("xs and ys must be arrays that contains elements");

            if (values.Length != positions.Length)
                throw new InvalidOperationException("xs and ys must have the same number of elements");

            YErrors = DataGen.Zeros(values.Length);
            YOffsets = DataGen.Zeros(values.Length);
            Ys = values;
            Xs = positions ?? DataGen.Consecutive(values.Length);
        }

        public override LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem()
            {
                label = Label,
                color = LollipopColor,
                lineWidth = 10,
                markerShape = MarkerShape.none,
                borderColor = BorderColor,
                borderWith = 1
            };
            return new LegendItem[] { singleItem };
        }

        protected override void RenderBarFromRect(RectangleF rect, bool negative, Graphics gfx)
        {
            float centerPx = HorizontalOrientation ? rect.Y + rect.Height / 2 : rect.X + rect.Width / 2;
            using var fillPen = new Pen(LollipopColor);
            using var fillBrush = GDI.Brush(LollipopColor);

            if (HorizontalOrientation)
            {
                gfx.FillEllipse(fillBrush, negative ? rect.X : rect.X + rect.Width, centerPx - LollipopRadius / 2, LollipopRadius, LollipopRadius);
                gfx.DrawLine(fillPen, rect.X, centerPx, rect.X + rect.Width, centerPx);
            }
            else
            {
                gfx.FillEllipse(fillBrush, centerPx - LollipopRadius / 2, !negative ? rect.Y : rect.Y + rect.Height, LollipopRadius, LollipopRadius);
                gfx.DrawLine(fillPen, centerPx, rect.Y, centerPx, rect.Y + rect.Height);
            }
        }
    }
}
