using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class Misc
    {
        // background colors
        public Color figureBackgroundColor = Color.White;
        public Color dataBackgroundColor = Color.White;

        // string formats (position indicates where their origin is)
        public StringFormat sfEast = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far };
        public StringFormat sfNorth = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center };
        public StringFormat sfNorthWest = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Near };
        public StringFormat sfNorthEast = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Far };
        public StringFormat sfSouth = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center };
        public StringFormat sfSouthWest = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Near };
        public StringFormat sfWest = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };
    }
}
