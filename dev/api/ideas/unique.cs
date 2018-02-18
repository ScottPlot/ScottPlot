// I think I'll use this. There is duplication at the class level, but
// once a class is made it will not need to be modified.


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

            thingsToPlot.Add(new Signal(new double[] { 1, 2, 3, 4 }, 20e3));
            thingsToPlot.Add(new DataXY(new double[] { 1, 2, 3 }, new double[] { 1, 4, 9 }));
            thingsToPlot.Add(new AxHline(0));

            // a loop like this would be used to plot each element
            foreach (object element in thingsToPlot)
            {
                if (element is Signal signal)
                {
                    Console.WriteLine($"Signal with {signal.Ys.Length} points");
                }
                else if (element is DataXY pointsXY)
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

    /* EACH OBJECT IS A SINGLE THING THAT SCOTTPLOT KNOWS HOW TO PLOT. 
     * IT CONTAINS EVERYTHING IT NEEDS TO KNOW TO DRAW IT ON THE CANVAS. */

    enum LineStyle { solid, dashed, dotted, none };
    enum MarkerStyle { circleSolid, circleOutline, none };

    class Signal
    {
        // unique data
        public double[] Ys;

        // line stuff
        public double sampleRate;
        public Color lineColor;

        public Signal(
            double[] Ys,
            double sampleRate = 1,
            Color? lineColor = null
            )
        {
            this.Ys = Ys;
            this.sampleRate = sampleRate;
            if (lineColor == null) lineColor = Color.Red;
            this.lineColor = (Color)lineColor;
        }
    }

    class SignalFill
    {
        public double[] Ys1;
        public double[] Ys2;
        public double sampleRate;
        public Color fillColor;

        public SignalFill(
            double[] Ys1,
            double[] Ys2,
            double sampleRate = 1,
            Color? fillColor = null
            )
        {
            this.Ys1 = Ys1;
            this.Ys2 = Ys2;
            this.sampleRate = sampleRate;
            if (fillColor == null) fillColor = Color.Red;
            this.fillColor = (Color)fillColor;
        }
    }

    class DataXY
    {
        public double[] Ys;
        public double[] Xs;
        public float lineWidth;
        public Color lineColor;
        public LineStyle lineStyle;
        public float markerSize;
        public Color markerColor;
        public MarkerStyle markerStyle;

        public DataXY(
            double[] Ys,
            double[] Xs,
            float lineWidth = 1,
            Color? lineColor = null,
            LineStyle lineStyle = LineStyle.solid,
            float markerSize = 3,
            Color? markerColor = null,
            MarkerStyle markerStyle = MarkerStyle.circleSolid
            )
        {
            this.Xs = Xs;
            this.Ys = Ys;
            this.lineWidth = lineWidth;
            this.lineStyle = lineStyle;
            if (lineColor == null) lineColor = Color.Red;
            this.lineColor = (Color)lineColor;
            this.markerSize = markerSize;
            if (markerColor == null) markerColor = Color.Red;
            this.markerColor = (Color)markerColor;
            this.markerStyle = markerStyle;
        }
    }

    class AxHline
    {
        public double yPos;
        public float lineWidth;
        public Color lineColor;
        public LineStyle lineStyle;

        public AxHline(
            double yPos,
            float lineWidth = 1,
            Color? lineColor = null,
            LineStyle lineStyle = LineStyle.solid
            )
        {
            this.yPos = yPos;
            this.lineWidth = lineWidth;
            this.lineStyle = lineStyle;
            if (lineColor == null) lineColor = Color.Red;
            this.lineColor = (Color)lineColor;
        }
    }

    class AxVline
    {
        public double xPos;
        public float lineWidth;
        public Color lineColor;
        public LineStyle lineStyle;

        public AxVline(
            double xPos,
            float lineWidth = 1,
            Color? lineColor = null,
            LineStyle lineStyle = LineStyle.solid
            )
        {
            this.xPos = xPos;
            this.lineWidth = lineWidth;
            this.lineStyle = lineStyle;
            if (lineColor == null) lineColor = Color.Red;
            this.lineColor = (Color)lineColor;
        }
    }

    class AxHspan
    {
        double x1, x2;
        public Color fillColor;
        public AxHspan(double x1, double x2, Color? fillColor= null)
        {
            this.x1 = Math.Min(x1, x2);
            this.x2 = Math.Max(x1, x2);
            if (fillColor == null) fillColor = Color.Red;
            this.fillColor = (Color)fillColor;
        }
    }

    class AxVspan
    {
        double x1, x2;
        public Color fillColor;
        public AxVspan(double x1, double x2, Color? fillColor = null)
        {
            this.x1 = Math.Min(x1, x2);
            this.x2 = Math.Max(x1, x2);
            if (fillColor == null) fillColor = Color.Red;
            this.fillColor = (Color)fillColor;
        }
    }

}
