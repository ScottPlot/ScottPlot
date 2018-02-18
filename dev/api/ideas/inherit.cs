// I don't like this because we'd have to pass tons of keywords around.
// I'd like to define things like colors and styles at instantiation

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<object> thingsToPlot = new List<object>();
            thingsToPlot.Add(new Signal(new double[] { 1, 2, 3, 4 }, 20e3));
            thingsToPlot.Add(new PointsXY(new double[] { 1, 2, 3 }, new double[] { 1, 4, 9 }));
            thingsToPlot.Add(new AxHline(0));

            foreach(object thing in thingsToPlot)
            {
                Console.WriteLine(thing);
            }

            Console.WriteLine("DONE");
        }
    }
    
    enum LineStyle { solid, dashed, dotted };
    enum MarkerShape { circleSolid, circleOutline };

    class PlottableThing
    {
        public Color lineColor;
        public float lineWidth;
        public LineStyle lineStyle;
        public Color markerColor;
        public float markerSize;
        public MarkerShape markerShape;
        public string label;

        public PlottableThing(
            Color? lineColor = null,
            float lineWidth = 1,
            LineStyle lineStyle = LineStyle.solid,
            Color? markerColor = null,
            float markerSize = 3,
            MarkerShape markerShape = MarkerShape.circleSolid,
            double alpha = 1,
            string label = null
            )
        {
            // choose default colors based on color sequence for theme
            if (lineColor == null) lineColor = Color.Red;
            markerColor = lineColor; // for now

            // adjust colors based on alpha

            this.lineColor = (Color)lineColor;
            this.lineWidth = lineWidth;
            this.lineStyle = lineStyle;
            this.markerColor = (Color)markerColor;
            this.markerSize = markerSize;
            this.markerShape = markerShape;
            this.label = label;
        }
    }

    class Signal: PlottableThing
    {
        public double[] Ys;
        public double sampleRate;
        public Signal(double[] Ys, double sampleRate)
        {
            this.Ys = Ys;
            this.sampleRate = sampleRate;
        }
    }

    class PointsXY : PlottableThing
    {
        public double[] Ys;
        public double[] Xs;

        public PointsXY(double[] Ys, double[] Xs)
        {
            this.Xs = Xs;
            this.Ys = Ys;
        }
    }

    class AxHline : PlottableThing
    {
        double yPos;
        public AxHline(double yPos)
        {
            this.yPos = yPos;
        }
    }
 
}
