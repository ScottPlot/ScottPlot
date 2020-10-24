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
            throw new NotImplementedException();
        }

        public void Render(Settings settings)
        {
            throw new NotImplementedException();
        }
    }
}
