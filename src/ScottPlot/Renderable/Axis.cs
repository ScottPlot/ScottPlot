using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    /// <summary>
    /// This class holds axis rendering details (label, ticks, tick labels) but no logic
    /// </summary>
    public class Axis : IRenderable
    {
        public Edge Edge { get; set; } = Edge.Bottom;
        public bool IsVisible { get; set; } = true;

        public string Title = null;
        public Drawing.Font TitleFont = new Drawing.Font();

        public Ticks MajorTicks = new Ticks() { MarkLength = 5, GridLines = true };
        public Ticks MinorTicks = new Ticks() { MarkLength = 3, GridLines = false };

        public Axis(double[] positions, string[] labels, double[] minorPositions)
        {
            MajorTicks.Positions = positions;
            MajorTicks.Labels = labels;
            MinorTicks.Positions = minorPositions;
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var testFill = GDI.Brush(Color.LightGray))
            {
                var rect = new RectangleF(
                    x: dims.DataOffsetX,
                    y: dims.DataOffsetY + dims.DataHeight,
                    width: dims.DataWidth,
                    height: dims.Height - (dims.DataHeight + dims.DataOffsetY));
                gfx.FillRectangle(testFill, rect);
                gfx.Clear(Color.Blue);
            }
        }
    }
}
