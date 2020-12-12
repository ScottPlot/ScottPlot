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
        public bool RulerMode = false;
        public bool SnapPx = true;
        public float PixelOffset = 0;
        public bool IsVisible { get; set; } = true;

        // TODO: store the TickCollection in the Axis module, not in the Ticks module.

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, lowQuality))
            {
                if (MajorTickVisible)
                    AxisTicksRender.RenderTickMarks(dims, gfx, TickCollection.tickPositionsMajor, RulerMode ? MajorTickLength * 4 : MajorTickLength, MajorTickColor, Edge, PixelOffset);

                if (MajorTickVisible && TickLabelVisible)
                    AxisTicksRender.RenderTickLabels(dims, gfx, TickCollection, TickLabelFont, Edge, TickLabelRotation, RulerMode, PixelOffset, MajorTickLength, MinorTickLength);

                if (MinorTickVisible)
                    AxisTicksRender.RenderTickMarks(dims, gfx, TickCollection.tickPositionsMinor, MinorTickLength, MinorTickColor, Edge, PixelOffset);

                if (MajorGridVisible)
                    AxisTicksRender.RenderGridLines(dims, gfx, TickCollection.tickPositionsMajor, MajorGridStyle, MajorGridColor, MajorGridWidth, Edge);

                if (MinorGridVisible)
                    AxisTicksRender.RenderGridLines(dims, gfx, TickCollection.tickPositionsMinor, MinorGridStyle, MinorGridColor, MinorGridWidth, Edge);
            }
        }
    }
}
