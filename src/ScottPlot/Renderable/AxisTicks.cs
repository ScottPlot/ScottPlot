using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    public class AxisTicks : IRenderable
    {
        public Edge Edge { get; set; } = Edge.Bottom;

        public bool ShowLabels { get; set; } = true;
        public string FontName = Config.Fonts.GetDefaultFontName();
        public float FontSize = 12;

        public readonly Config.TickCollection Ticks = new Config.TickCollection(false);

        public bool ShowMajorTicks = true;
        public Color MajorTickColor = Color.Black;
        public int MajorTickLength = 5;

        public bool ShowMinorTicks = true;
        public Color MinorTickColor = Color.Black;
        public int MinorTickLength = 2;

        public double FixedSpacing = 0;
        public Config.DateTimeUnitKind? FixedDateTimeSpacingUnit = null;

        public bool RulerMode = false;
        public double Rotation = 0;

        public bool SnapToNearestPixel = true;

        // TODO: add support for multiplier and offset notation (removed in version 4.0.40)
        // TODO: add support for scientific notation (removed in version 4.0.40)
        /*
        public bool useMultiplierNotation = false;
        public bool useOffsetNotation = false;
        public bool useExponentialNotation = true;
        */

        public void Render(Settings settings)
        {
            using (var font = new Font(FontName, FontSize, GraphicsUnit.Pixel))
            {
                throw new NotImplementedException();
            }
        }
    }
}
