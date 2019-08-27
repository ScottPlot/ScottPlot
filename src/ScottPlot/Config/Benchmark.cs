using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class Benchmark : TextLabel
    {
        public Stopwatch stopwatch = new Stopwatch();
        public double msec;
        public double hz;

        public Benchmark()
        {
            fontName = "Consolas";
            fontSize = 10;
            colorBackground = Color.FromArgb(150, Color.LightYellow);
            visible = false;
        }

    }
}
