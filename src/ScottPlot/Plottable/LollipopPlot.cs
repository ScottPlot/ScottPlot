using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    public class LollipopPlot : BarPlotBase
    {
        public string Label { get; set; }
        public Color LollipopColor { get; set; }
        public float LollipopRadius { get; set; } = 5;

        public LollipopPlot(double[] xs, double[] ys, double[] yErr, double[] yOffsets) : base()
        {
            if (ys is null || ys.Length == 0)
                throw new InvalidOperationException("ys must be an array that contains elements");

            Ys = ys;
            Xs = xs ?? DataGen.Consecutive(ys.Length);
            YErrors = yErr ?? DataGen.Zeros(ys.Length);
            YOffsets = yOffsets ?? DataGen.Zeros(ys.Length);
        }

        public override LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem()
            {
                label = Label,
                color = LollipopColor,
                lineWidth = 10,
                markerShape = MarkerShape.none,
                //hatchColor = FillColorHatch,
                //hatchStyle = HatchStyle,
                borderColor = BorderColor,
                borderWith = 1
            };
            return new LegendItem[] { singleItem };
        }

        protected override void RenderBarFromRect(RectangleF rect, bool negative, Graphics gfx)
        {
            float centerPx = HorizontalOrientation ? rect.Y + rect.Height / 2 : rect.X + rect.Width / 2;
            using (var fillPen = new Pen(LollipopColor))
            using (var fillBrush = GDI.Brush(LollipopColor))
            {

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
}
