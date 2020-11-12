using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace ScottPlot.Renderable
{
    /// <summary>
    /// This class holds axis rendering details (label, ticks, tick labels) but no logic
    /// </summary>
    public class Axis : IRenderable
    {
        public readonly AxisDimensions Dims = new AxisDimensions();
        public int AxisIndex = 1;
        private Edge _Edge;
        public Edge Edge
        {
            get => _Edge;
            set
            {
                _Edge = value;
                Line.Edge = value;
                Title.Edge = value;
                Ticks.Edge = value;
                Ticks.TickCollection.verticalAxis = (value == Edge.Left || value == Edge.Right);
            }
        }
        public bool IsHorizontal => Edge == Edge.Top || Edge == Edge.Bottom;
        public bool IsVertical => Edge == Edge.Left || Edge == Edge.Right;
        public bool IsVisible { get; set; } = true;

        public float PixelSize = 123;
        public float PixelSizeMinimum = 5;
        public float PixelSizeMaximum = float.PositiveInfinity;
        public float PixelSizePadding = 3;

        public readonly AxisTitle Title = new AxisTitle();
        public readonly AxisTicks Ticks = new AxisTicks();
        public readonly AxisLine Line = new AxisLine();

        public void RecalculateTickPositions(PlotDimensions2D dims)
        {
            Ticks.TickCollection.Recalculate(dims, Ticks.MajorLabelFont);
        }

        public void Render(PlotDimensions2D dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            Title.PixelSizePadding = PixelSizePadding;

            using (var gfx = GDI.Graphics(bmp, lowQuality))
            {
                Ticks.Render(dims, bmp, lowQuality);
                Title.Render(dims, bmp, lowQuality);
                Line.Render(dims, bmp, lowQuality);
            }
        }

        // pass arguments from the Plot module to this level
        public void Configure(
            bool? showTitle = null,
            bool? showLabels = null,
            bool? showMajorTicks = null,
            bool? showMinorTicks = null,
            bool? showLine = null,
            Color? color = null,
            bool? useMultiplierNotation = null,
            bool? useOffsetNotation = null,
            bool? useExponentialNotation = null,
            bool? dateTime = null,
            bool? rulerMode = null,
            bool? invertSign = null,
            string fontName = null,
            float? fontSize = null,
            double? rotation = null,
            bool? logScale = null,
            string numericFormatString = null,
            bool? snapToNearestPixel = null,
            int? radix = null,
            string prefix = null,
            string dateTimeFormatString = null
            )
        {
            Title.IsVisible = showTitle ?? Title.IsVisible;
            Ticks.MajorLabelEnable = showLabels ?? Ticks.MajorLabelEnable;
            Ticks.MajorTickEnable = showMajorTicks ?? Ticks.MajorTickEnable;
            Ticks.MinorTickEnable = showMinorTicks ?? Ticks.MinorTickEnable;
            Line.IsVisible = showLine ?? Line.IsVisible;

            Ticks.Color = color ?? Ticks.Color;
            Title.Font.Color = color ?? Title.Font.Color;
            Line.Color = color ?? Line.Color;

            Ticks.TickCollection.useMultiplierNotation = useMultiplierNotation ?? Ticks.TickCollection.useMultiplierNotation;
            Ticks.TickCollection.useOffsetNotation = useOffsetNotation ?? Ticks.TickCollection.useOffsetNotation;
            Ticks.TickCollection.useExponentialNotation = useExponentialNotation ?? Ticks.TickCollection.useExponentialNotation;

            Ticks.TickCollection.dateFormat = dateTime ?? Ticks.TickCollection.dateFormat;
            Ticks.RulerMode = rulerMode ?? Ticks.RulerMode;
            Ticks.TickCollection.invertSign = invertSign ?? Ticks.TickCollection.invertSign;
            Ticks.MajorLabelFont.Name = fontName ?? Ticks.MajorLabelFont.Name;
            Ticks.MajorLabelFont.Size = fontSize ?? Ticks.MajorLabelFont.Size;
            Ticks.Rotation = (float)(rotation ?? Ticks.Rotation);
            Ticks.TickCollection.logScale = logScale ?? Ticks.TickCollection.logScale;
            Ticks.TickCollection.numericFormatString = numericFormatString ?? Ticks.TickCollection.numericFormatString;
            Ticks.SnapPx = snapToNearestPixel ?? Ticks.SnapPx;
            Ticks.TickCollection.radix = radix ?? Ticks.TickCollection.radix;
            Ticks.TickCollection.prefix = prefix ?? Ticks.TickCollection.prefix;
            Ticks.TickCollection.dateTimeFormatString = dateTimeFormatString ?? Ticks.TickCollection.dateTimeFormatString;
        }

        public void RecalculateAxisSize()
        {
            using (var tickFont = GDI.Font(Ticks.MajorLabelFont))
            using (var titleFont = GDI.Font(Title.Font))
            {
                PixelSize = 0;

                if (Title.IsVisible)
                    PixelSize += GDI.MeasureString(Title.Label, Title.Font).Height;

                if (Ticks.MajorLabelEnable)
                    PixelSize += IsHorizontal ? Ticks.TickCollection.maxLabelHeight : Ticks.TickCollection.maxLabelWidth * 1.2f;

                if (Ticks.MajorTickEnable)
                    PixelSize += Ticks.MajorTickLength;

                PixelSize = Math.Max(PixelSize, PixelSizeMinimum);
                PixelSize = Math.Min(PixelSize, PixelSizeMaximum);
                PixelSize += PixelSizePadding;
            }
        }
    }
}
