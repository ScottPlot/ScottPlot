﻿using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    public abstract class BarPlotBase
    {
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        /// <summary>
        /// Orientation of the bars.
        /// Default behavior is vertical so values are on the Y axis and positions are on the X axis.
        /// </summary>
        public Orientation Orientation = Orientation.Vertical;

        /// <summary>
        /// The position of each bar defines where the left edge of the bar should be.
        /// To center the bar at each position, adjust this value to be negative one-half of the BarWidth.
        /// </summary>
        public double PositionOffset { get; set; }

        /// <summary>
        /// Size of each bar (along the axis defined by Orientation) relative to ValueBase
        /// </summary>
        public double[] Values { get; set; }

        /// <summary>
        /// Location of the left edge of each bar.
        /// To center bars on these positions, adjust PositionOffset to be negative one-half of the BarWidth.
        /// </summary>
        public double[] Positions { get; set; }

        /// <summary>
        /// This array defines the base of each bar.
        /// Unless the user specifically defines it, this will be an array of zeros.
        /// </summary>
        public double[] ValueOffsets { get; set; }

        /// <summary>
        /// If populated, this array describes the height of errorbars for each bar
        /// </summary>
        public double[] ValueErrors { get; set; }

        /// <summary>
        /// If true, errorbars will be drawn according to the values in the YErrors array
        /// </summary>
        public bool ShowValuesAboveBars { get; set; }

        /// <summary>
        /// Bars are drawn from this level and extend according to the sizes defined in Values[]
        /// </summary>
        public double ValueBase { get; set; }

        /// <summary>
        /// Width of bars defined in axis units.
        /// If bars are evenly spaced, consider setting this to a fraction of the distance between the first two Positions.
        /// </summary>
        public double BarWidth = .8;

        /// <summary>
        /// Width of the errorbar caps defined in axis units.
        /// </summary>
        public double ErrorCapSize = .4;

        /// <summary>
        /// Thickness of the errorbar lines (pixel units)
        /// </summary>
        public float ErrorLineWidth = 1;

        /// <summary>
        /// Outline each bar with this color. 
        /// Set this to transparent to disable outlines.
        /// </summary>
        public Color BorderColor = Color.Black;

        /// <summary>
        /// Color of errorbar lines.
        /// </summary>
        public Color ErrorColor = Color.Black;

        /// <summary>
        /// Font settings for labels drawn above the bars
        /// </summary>
        public readonly Drawing.Font Font = new();

        public virtual AxisLimits GetAxisLimits()
        {
            double valueMin = double.PositiveInfinity;
            double valueMax = double.NegativeInfinity;
            double positionMin = double.PositiveInfinity;
            double positionMax = double.NegativeInfinity;

            for (int i = 0; i < Positions.Length; i++)
            {
                valueMin = Math.Min(valueMin, Values[i] - ValueErrors[i] + ValueOffsets[i]);
                valueMax = Math.Max(valueMax, Values[i] + ValueErrors[i] + ValueOffsets[i]);
                positionMin = Math.Min(positionMin, Positions[i]);
                positionMax = Math.Max(positionMax, Positions[i]);
            }

            valueMin = Math.Min(valueMin, ValueBase);
            valueMax = Math.Max(valueMax, ValueBase);

            if (ShowValuesAboveBars)
                valueMax += (valueMax - valueMin) * .1; // increase by 10% to accommodate label

            positionMin -= BarWidth / 2;
            positionMax += BarWidth / 2;

            positionMin += PositionOffset;
            positionMax += PositionOffset;

            return Orientation == Orientation.Vertical ?
                new AxisLimits(positionMin, positionMax, valueMin, valueMax) :
                new AxisLimits(valueMin, valueMax, positionMin, positionMax);
        }

        [Obsolete("Reference the 'Orientation' field instead of this field")]
        public bool VerticalOrientation
        {
            get => Orientation == Orientation.Vertical;
            set => Orientation = value ? Orientation.Vertical : Orientation.Horizontal;
        }

        [Obsolete("Reference the 'Orientation' field instead of this field")]
        public bool HorizontalOrientation
        {
            get => Orientation == Orientation.Horizontal;
            set => Orientation = value ? Orientation.Horizontal : Orientation.Vertical;
        }

        [Obsolete("Reference the 'Values' field instead of this field")]
        public double[] Ys
        {
            get => Values;
            set => Values = value;
        }

        [Obsolete("Reference the 'Positions' field instead of this field")]
        public double[] Xs
        {
            get => Positions;
            set => Positions = value;
        }

        [Obsolete("Reference the 'PositionOffset' field instead of this field", true)]
        public double XOffset
        {
            get => PositionOffset;
            set => PositionOffset = value;
        }
    }
}
