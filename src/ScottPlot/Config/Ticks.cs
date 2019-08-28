using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class Ticks
    {
        public Font tickFont = new Font("Segoe UI", 10);
        public TickCollection tickCollectionX;
        public TickCollection tickCollectionY;
        public int tickSize = 5;
        public Color tickColor = Color.Black;
        public bool displayTicksX = true;
        public bool displayTicksXminor = true;
        public bool tickDateTimeX = false;
        public bool displayTicksY = true;
        public bool displayTicksYminor = true;
        public bool tickDateTimeY = false;
        public bool useMultiplierNotation = true;
        public bool useOffsetNotation = true;
        public bool useExponentialNotation = true;
        public double tickSpacingX = 0;
        public double tickSpacingY = 0;
    }
}
