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

            // a sequence like this would be used to add data in the order it will be plotted
            thingsToPlot.Clear();
            thingsToPlot.Add(new Signal(new double[] { 1, 2, 3, 4 }, 20e3, lineWidth: 9));
            thingsToPlot.Add(new PointsXY(new double[] { 1, 2, 3 }, new double[] { 1, 4, 9 }));
            thingsToPlot.Add(new AxHline(0));

            // a loop like this would be used to plot each element
            foreach(object element in thingsToPlot)
            {
                if (element is Signal signal)
                {
                    Console.WriteLine($"Signal with {signal.Ys.Length} points and lineWidth of {signal.lineWidth}");
                }
                else if (element is PointsXY pointsXY)
                {
                    Console.WriteLine($"PointsXY with {pointsXY.Xs.Length} points and lineWidth of {pointsXY.lineWidth}");
                }
                else if (element is AxHline axHline)
                {
                    Console.WriteLine($"AxHline at {axHline.yPos} with lineWidth of {axHline.lineWidth}");
                }
                else
                {
                    Console.WriteLine($"I DONT KNO WHOW TO PLOT {element}");
                }
            }

            Console.WriteLine("DONE");
        }
    }
    
    enum LineStyle { solid, dashed, dotted };
    enum MarkerShape { circleSolid, circleOutline };

    class Style
    {
        public Color lineColor;
        public float lineWidth;
        public LineStyle lineStyle;
        public Color markerColor;
        public float markerSize;
        public MarkerShape markerShape;
        public string label;

        public Style(
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

            // TODO: adjust colors based on alpha

            this.lineColor = (Color)lineColor;
            this.lineWidth = lineWidth;
            this.lineStyle = lineStyle;
            this.markerColor = (Color)markerColor;
            this.markerSize = markerSize;
            this.markerShape = markerShape;
            this.label = label;
        }
    }

    class Signal : Style
    {
        public double[] Ys;
        public double sampleRate;
        public Style style = new Style();

        public Signal(double[] Ys, double sampleRate=1, float lineWidth = 1) : base(lineWidth: lineWidth)
        {
            this.Ys = Ys;
            this.sampleRate = sampleRate;
        }
    }

    class PointsXY : Style
    {
        public double[] Ys;
        public double[] Xs;
        public Style style = new Style();

        public PointsXY(double[] Ys, double[] Xs)
        {
            this.Xs = Xs;
            this.Ys = Ys;
        }
    }

    class AxHline : Style
    {
        public double yPos;
        public Style style = new Style();

        public AxHline(double yPos)
        {
            this.yPos = yPos;
        }
    }
 
}
