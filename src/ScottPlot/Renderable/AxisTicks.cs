/* The AxisTicks object contains:
 *   - A TickCollection responsible for calculating tick positions and labels
 *   - major tick label styling
 *   - major/minor tick mark styling
 *   - major/minor grid line styling
 */

using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Renderable
{
    public class AxisTicks : IRenderable
    {
        // the tick collection determines where ticks should go and what tick labels should say
        public readonly TickCollection TickCollection = new TickCollection();

        // tick label styling
        public bool TickLabelVisible = true;
        public float TickLabelRotation = 0;
        public Drawing.Font TickLabelFont = new Drawing.Font() { Size = 11 };
        public bool TicksExtendOutward = true;

        // major tick/grid styling
        public bool MajorTickVisible = true;
        public float MajorTickLength = 5;
        public Color MajorTickColor = Color.Black;
        public bool MajorGridVisible = false;
        public LineStyle MajorGridStyle = LineStyle.Solid;
        public Color MajorGridColor = ColorTranslator.FromHtml("#efefef");
        public float MajorGridWidth = 1;

        // minor tick/grid styling
        public bool MinorTickVisible = true;
        public float MinorTickLength = 2;
        public Color MinorTickColor = Color.Black;
        public bool MinorGridVisible = false;
        public LineStyle MinorGridStyle = LineStyle.Solid;
        public Color MinorGridColor = ColorTranslator.FromHtml("#efefef");
        public float MinorGridWidth = 1;

        // misc configuration
        public Edge Edge;
        public bool IsHorizontal => Edge == Edge.Top || Edge == Edge.Bottom;
        public bool IsVertical => Edge == Edge.Left || Edge == Edge.Right;

        public bool IsVisible { get; set; } = true;

        public bool RulerMode = false;
        public bool SnapPx = true;
        public float PixelOffset = 0;

        // TODO: store the TickCollection in the Axis module, not in the Ticks module.

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!IsVisible)
                return;

            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality, false);

            double[] visibleMajorTicks = TickCollection.GetVisibleMajorTicks(dims)
                .Select(t => t.Position)
                .ToArray();

            double[] visibleMinorTicks = TickCollection.GetVisibleMinorTicks(dims)
                .Select(t => t.Position)
                .ToArray();

            if (MajorGridVisible)
                AxisTicksRender.RenderGridLines(dims, gfx, visibleMajorTicks, MajorGridStyle, MajorGridColor, MajorGridWidth, Edge);

            if (MinorGridVisible)
                AxisTicksRender.RenderGridLines(dims, gfx, visibleMinorTicks, MinorGridStyle, MinorGridColor, MinorGridWidth, Edge);

            if (MinorTickVisible)
            {
                float tickLength = TicksExtendOutward ? MinorTickLength : -MinorTickLength;
                AxisTicksRender.RenderTickMarks(dims, gfx, visibleMinorTicks, tickLength, MinorTickColor, Edge, PixelOffset);
            }

            if (MajorTickVisible)
            {
                float tickLength = MajorTickLength;

                if (RulerMode)
                    tickLength *= 4;

                tickLength = TicksExtendOutward ? tickLength : -tickLength;

                AxisTicksRender.RenderTickMarks(dims, gfx, visibleMajorTicks, tickLength, MajorTickColor, Edge, PixelOffset);
            }

            if (TickLabelVisible)
                AxisTicksRender.RenderTickLabels(dims, gfx, TickCollection, TickLabelFont, Edge, TickLabelRotation, RulerMode, PixelOffset, MajorTickLength, MinorTickLength);
        }
    }
}
