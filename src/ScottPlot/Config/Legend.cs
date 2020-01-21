using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class Legend
    {
        public Font font = new Font(Fonts.GetDefaultFontName(), 12);
        public Color colorText = Color.Black;
        public Color colorBackground = Color.White;
        public Color colorShadow = Color.FromArgb(75, 0, 0, 0);
        public Color colorFrame = Color.Black;
        public legendLocation location = legendLocation.none;
        public shadowDirection shadow = shadowDirection.none;
        public Rectangle rect = new Rectangle(0, 0, 1, 1);
        public bool antiAlias = true;
        public bool fixedLineWidth = true;
    }
}
