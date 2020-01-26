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

        public void Start()
        {
            stopwatch.Restart();
        }

        public void Stop()
        {
            stopwatch.Stop();
            msec = stopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            hz = 1000.0 / msec;
            text = string.Format("Benchmark took {0:0.000} ms ({1:0.00 Hz})", msec, hz);
        }

        public void UpdateMessage(int plottableCount, int pointCount)
        {
            text = "";
            text += string.Format("Full render of {0:n0} objects ({1:n0} points)", plottableCount, pointCount);
            text += string.Format(" took {0:0.000} ms ({1:0.00 Hz})", msec, hz);
            if (plottableCount == 1)
                text = text.Replace("objects", "object");
        }

        public override string ToString()
        {
            return text;
        }
    }
}
