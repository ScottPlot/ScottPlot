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
        // drawing options
        public bool antiAliasData = true;
        public bool antiAliasFigure = true;

        // string formats (position indicates where their origin is)
        public StringFormat sfEast = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far };
        public StringFormat sfNorth = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center };
        public StringFormat sfNorthWest = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Near };
        public StringFormat sfNorthEast = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Far };
        public StringFormat sfSouth = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center };
        public StringFormat sfSouthWest = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Near };
        public StringFormat sfSouthEast = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Far };
        public StringFormat sfWest = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };
        public StringFormat sfCenterCenter = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
    }
}
