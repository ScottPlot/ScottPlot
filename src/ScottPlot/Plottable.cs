using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{

    public static class PlottableDefaultsDELETEME
    {
        public static Color LineColor = Color.Black;
        public static float LineWidth = 1;

        public static Color MarkerColor = Color.Black;
        public static float MarkerSize = 10;

        public static Color TextColor = Color.Black;
        public static float TextSize = 12;
        public static string TextFont = "Segoe UI";
    }

    public abstract class Plottable
    {
        public int pointCount = 0;
        public string label = null;

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