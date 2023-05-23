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
        public readonly TickGenerator TickCollection = new TickGenerator();

        // tick label styling
        public bool TickLabelVisible = true;
        public float TickLabelRotation = 0;
        public Drawing.Font TickLabelFont = new Drawing.Font() { Size = 11 };
        public bool TicksExtendOutward = true;

        // major tick/grid styling
        public bool MajorTickVisible = true;
        public float MajorTickLength = 5;
        public float MajorLineWidth = 1;
        public Color MajorTickColor = Color.Black;
        public bool MajorGridVisible = false;
        public LineStyle MajorGridStyle = LineStyle.Solid;
        public Color MajorGridColor = ColorTranslator.FromHtml("#efefef");
        public float MajorGridWidth = 1;

        // minor tick/grid styling
        public bool MinorTickVisible = true;
        public float MinorTickLength = 2;
        public float MinorLineWidth = 1;
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

        /// <summary>
        /// If true, grid lines will be drawn with anti-aliasing off to give the appearance of "snapping"
        /// to the nearest pixel and to avoid blurriness associated with drawing single-pixel anti-aliased lines.
        /// </summary>
        public bool SnapPx = true;

        public float PixelOffset = 0;

        // TODO: store the TickCollection in the Axis module, not in the Ticks module.
        // https://github.com/ScottPlot/ScottPlot/pull/1647

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!IsVisible)
                return;

            double[] majorTicks = TickCollection.GetVisibleMajorTicks(dims)
                .Select(t => t.Position)
                .ToArray();

            double[] minorTicks = TickCollection.GetVisibleMinorTicks(dims)
                .Select(t => t.Position)
                .ToArray();

            RenderTicksAndGridLines(dims, bmp, lowQuality || SnapPx, majorTicks, minorTicks);
            RenderTickLabels(dims, bmp, lowQuality);
        }

        private void RenderTicksAndGridLines(PlotDimensions dims, Bitmap bmp, bool lowQuality, double[] visibleMajorTicks, double[] visibleMinorTicks)
        {
            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality, false);

            if (MajorGridVisible)
                AxisTicksRender.RenderGridLines(dims, gfx, visibleMajorTicks, MajorGridStyle, MajorGridColor, MajorGridWidth, Edge);

            if (MinorGridVisible)
                AxisTicksRender.RenderGridLines(dims, gfx, visibleMinorTicks, MinorGridStyle, MinorGridColor, MinorGridWidth, Edge);

            if (MinorTickVisible)
            {
                float tickLength = TicksExtendOutward ? MinorTickLength : -MinorTickLength;
                AxisTicksRender.RenderTickMarks(dims, gfx, visibleMinorTicks, tickLength, MinorTickColor, Edge, PixelOffset, MinorLineWidth);
            }

            if (MajorTickVisible)
            {
                float tickLength = MajorTickLength;

                if (RulerMode)
                    tickLength *= 4;

                tickLength = TicksExtendOutward ? tickLength : -tickLength;

                AxisTicksRender.RenderTickMarks(dims, gfx, visibleMajorTicks, tickLength, MajorTickColor, Edge, PixelOffset, MajorLineWidth);
            }
        }

        private void RenderTickLabels(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality, false);

            if (TickLabelVisible)
                AxisTicksRender.RenderTickLabels(dims, gfx, TickCollection, TickLabelFont, Edge, TickLabelRotation, RulerMode, PixelOffset, MajorTickLength, MinorTickLength);
        }
    }
}
