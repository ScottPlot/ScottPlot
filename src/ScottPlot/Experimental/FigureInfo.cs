using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Experimental
{
    // this class is similar to settings for ScottPlot 4.0 but is created with every frame
    public class FigureInfo
    {
        public readonly int Width, Height;
        public readonly int DataL, DataT, DataR, DataB, DataWidth, DataHeight;
        public readonly double xMin, xMax, yMin, yMax;
        public readonly double xUnitsPerPx, xPxPerUnit, yUnitsPerPx, yPxPerUnit;
        public readonly double[] xMajorTicks, xMinorTicks, yMajorTicks, yMinorTicks;
        public readonly string[] xTickLabels, yTickLabels;
        public readonly string AxisLabelLeft, AxisLabelRight, AxisLabelBottom, AxisLabelTop;

        public enum AntiAliasMode { Always, Never, Custom };
        public readonly AntiAliasMode AntiAlias = AntiAliasMode.Custom;

        public double PixelX(double unitX)
        {
            double unitsFromDataLeft = unitX - xMin;
            double pxFromDataL = unitsFromDataLeft * xPxPerUnit;
            double pixelX = pxFromDataL + DataL;
            return pixelX;
        }

        public double PixelY(double unitY)
        {
            double unitsFromBottom = unitY - yMin;
            double pxFromDataB = unitsFromBottom * yPxPerUnit;
            return DataB - pxFromDataB;
        }

        public FigureInfo()
        {

        }

        public FigureInfo(Settings settings)
        {
            Width = settings.figureSize.Width;
            Height = settings.figureSize.Height;

            DataL = settings.dataOrigin.X;
            DataT = settings.dataOrigin.Y;
            DataR = DataL + settings.dataSize.Width - 1;
            DataB = DataT + settings.dataSize.Height - 1;
            DataWidth = settings.dataSize.Width;
            DataHeight = settings.dataSize.Height;

            xMin = settings.axes.x.min;
            xMax = settings.axes.x.max;
            yMin = settings.axes.y.min;
            yMax = settings.axes.y.max;

            xUnitsPerPx = (xMax - xMin) / DataWidth;
            xPxPerUnit = DataWidth / (xMax - xMin);
            yUnitsPerPx = (yMax - yMin) / DataHeight;
            yPxPerUnit = DataHeight / (yMax - yMin);

            xMajorTicks = settings.ticks.x.tickPositionsMajor;
            xMinorTicks = settings.ticks.x.tickPositionsMinor;
            xTickLabels = settings.ticks.x.tickLabels;

            yMajorTicks = settings.ticks.y.tickPositionsMajor;
            yMinorTicks = settings.ticks.y.tickPositionsMinor;
            yTickLabels = settings.ticks.y.tickLabels;
        }
    }
}
