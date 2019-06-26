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

        public abstract void Render(Settings settings);
        public abstract override string ToString();

        public void Validate()
        {
            if (pointCount == 0)
                throw new System.Exception("pointCount must be >0");
        }

        public abstract double[] GetLimits();
    }
}