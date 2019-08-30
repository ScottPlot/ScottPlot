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
        public Font font = new Font("Segoe UI", 10);
        public TickCollection x;
        public TickCollection y;
        public int size = 5;
        public Color color = Color.Black;

        public bool displayXmajor = true;
        public bool displayXminor = true;

        public double manualSpacingX = 0;
        public double manualSpacingY = 0;

        public bool timeFormatX = false;
        public bool timeFormatY = false;

        public bool displayYmajor = true;
        public bool displayYminor = true;

        public bool useMultiplierNotation = true;
        public bool useOffsetNotation = true;
        public bool useExponentialNotation = true;
    }
}
