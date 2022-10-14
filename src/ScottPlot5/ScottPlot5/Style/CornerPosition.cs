using ScottPlot.Axis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Style
{
    public enum Corner
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public class CornerPosition : IPosition
    {
        public Corner Corner { get; set; } = Corner.TopLeft;
        public int Margin { get; set; } = 5;

        public CornerPosition(Corner corner)
        {
            Corner = corner;
        }

        public Pixel GetPosition(PixelRect bounds, PixelSize size)
        {
            return Corner switch
            {
                Corner.TopLeft => new Pixel(bounds.Left, bounds.Top) + new Pixel(Margin, Margin),
                Corner.TopRight => new Pixel(bounds.Right - size.Width, bounds.Top) + new Pixel(-Margin, Margin),
                Corner.BottomLeft => new Pixel(bounds.Left, bounds.Bottom - size.Height) + new Pixel(Margin, -Margin),
                Corner.BottomRight => new Pixel(bounds.Right - size.Width, bounds.Bottom - size.Height) + new Pixel(-Margin, -Margin),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
