using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Statistics
{
    public struct Box
    {
        public bool outline;
        public bool fill;
        public double min;
        public double max;
        public double width;
        public System.Drawing.Color lineColor;
        public float lineWidth;
        public System.Drawing.Color fillColor;
    }

    public struct Whisker
    {
        public bool visible;
        public double min;
        public double max;
        public double width;
        public System.Drawing.Color lineColor;
        public float lineWidth;
    }

    public struct Midline
    {
        public bool visible;
        public double position;
        public double width;
        public System.Drawing.Color lineColor;
        public float lineWidth;
        public MarkerShape markerShape;
        public float markerSize;
    }

    public enum DataPointAlignment { center, left, right }

    public struct DataPoints
    {
        public double[] values;
        public double spreadFraction;
        public double offsetFraction;
        public MarkerShape markerShape;
        public float markerSize;
        public DataPointAlignment align;
    }

    public class BoxAndWhisker
    {
        public Box box = new Box();
        public Whisker whisker = new Whisker();
        public Midline midline = new Midline();
        public double xPosition;
        public string label;
        public DataPoints dataPoints = new DataPoints();

        public BoxAndWhisker(double xPosition, double[] dataValues)
        {
            this.xPosition = xPosition;
            dataPoints.values = dataValues;
            dataPoints.spreadFraction = 0.75;
            dataPoints.offsetFraction = 1.75;
            dataPoints.markerSize = 5;
            dataPoints.markerShape = MarkerShape.openCircle;
            dataPoints.align = DataPointAlignment.center;
            ResetStyle();
        }

        private void ResetStyle()
        {
            box.outline = true;
            box.fill = true;
            box.lineColor = System.Drawing.Color.Black;
            box.lineWidth = 2;
            box.fillColor = System.Drawing.Color.White;
            box.width = .4;

            whisker.visible = true;
            whisker.lineColor = System.Drawing.Color.Black;
            whisker.lineWidth = 2;
            whisker.width = .4;

            midline.visible = true;
            midline.lineColor = System.Drawing.Color.Black;
            midline.lineWidth = 2;
            midline.markerShape = MarkerShape.filledCircle;
            midline.markerSize = 3;
            midline.width = .4;
        }
    }
}
