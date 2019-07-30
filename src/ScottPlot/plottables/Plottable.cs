using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public abstract class Plottable
    {
        public int pointCount = 0;
        public string label = null;
        public Color color = Color.Black;
        public MarkerShape markerShape = MarkerShape.none;
        public abstract void Render(Settings settings);
        public abstract override string ToString();
        public abstract double[] GetLimits();
        public abstract void SaveCSV(string filePath);
    }
}