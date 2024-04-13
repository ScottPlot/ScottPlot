using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using ScottPlot.Ticks;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A colorbar translates numeric intensity values to colors.
    /// The Colorbar plot type displays a Colorbar along an edge of the plot.
    /// </summary>
    public class Colorbar : IPlottable, IStylable
    {
        public Renderable.Edge Edge { get; set; } = Renderable.Edge.Right;

        private Colormap Colormap;
        private Bitmap BmpScale;

        public bool IsVisible { get; set; } = true;
        public bool LabelIsVisible { get; set; } = true;
        public int XAxisIndex { get => 0; set { } }
        public int YAxisIndex { get => 0; set { } }

        /// <summary>
        /// Width of the colored rectangle (pixels)
        /// </summary>
        public int Width { get; set; } = 20;

        public readonly Drawing.Font TickLabelFont = new();
        public Color TickMarkColor { get; set; } = Color.Black;
        public float TickMarkLength { get; set; } = 3;
        public float TickMarkWidth { get; set; } = 1;

        private readonly List<Tick> ManualTicks = new();
        private bool AutomaticTickEnable { get; set; } = true;
        private int AutomaticTickMinimumSpacing { get; set; } = 40;
        public Func<double, string> TickLabelFormatter { get; set; } = position => $"{position:F2}";

        /// <summary>
        /// Optional text to display rotated outside the colormap
        /// </summary>
        public string Label { get; set; } = string.Empty;
        public readonly Drawing.Font LabelFont = new() { Size = 16 };

        /// <summary>
        /// Distance (pixels) to offset the axis label from the edge of the colormap.
        /// This edge is typically large enough to accommodate tick labels.
        /// </summary>
        public float LabelMargin { get; set; } = -1;

        public float DataAreaPadding { get; set; } = 10;

        private double _MinValue { get; set; } = 0;
        public double MinValue
        {
            get => (Plottable is IHasColormap p) ? p.ColormapMin : _MinValue;
            set => _MinValue = value;
        }

        private double _MaxValue = 1;
        public double MaxValue
        {
            get => (Plottable is IHasColormap p) ? p.ColormapMax : _MaxValue;
            set => _MaxValue = value;
        }

        private bool _MinIsClipped = false;
        public bool MinIsClipped
        {
            get => (Plottable is IHasColormap p) ? p.ColormapMinIsClipped : _MinIsClipped;
            set => _MinIsClipped = value;
        }

        private bool _MaxIsClipped = false;
        public bool MaxIsClipped
        {
            get => (Plottable is IHasColormap p) ? p.ColormapMaxIsClipped : _MaxIsClipped;
            set => _MaxIsClipped = value;
        }

        private double _MinColor = 0;
        public double MinColor { get => _MinColor; set { _MinColor = value; UpdateBitmap(); } }

        private double _MaxColor = 1;
        public double MaxColor { get => _MaxColor; set { _MaxColor = value; UpdateBitmap(); } }

        /// <summary>
        /// Size (in pizels) of the colorbar, ticks, and label at the time of the last render
        /// </summary>
        private float LastRenderWidth = -1;

        /// <summary>
        /// If populated, this object holds the plottable containing the heatmap and value data this colorbar represents
        /// </summary>
        private IHasColormap Plottable;

        public Colorbar(Colormap colormap = null)
        {
            UpdateColormap(colormap ?? Colormap.Viridis);
        }

        public Colorbar(IHasColormap plottable)
        {
            Plottable = plottable;
            UpdateColormap(plottable.Colormap);
        }

        public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

        public LegendItem[] GetLegendItems() => LegendItem.None;

        public void ValidateData(bool deep = false) { }

        public void SetStyle(Color? tickMarkColor, Color? tickFontColor)
        {
            TickMarkColor = tickMarkColor ?? TickMarkColor;
            TickLabelFont.Color = tickFontColor ?? TickLabelFont.Color;
        }

        /// <summary>
        /// Configure ticks that are automatically generated in the absense of manually-added ticks
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="minimumSpacing">Minimum number of vertical pixels between tick positions</param>
        /// <param name="formatter">Optional custom string formatter to translate tick positions to labels</param>
        public void AutomaticTicks(bool enable = true, int? minimumSpacing = null, Func<double, string> formatter = null)
        {
            if (enable)
                ManualTicks.Clear();

            AutomaticTickEnable = enable;
            AutomaticTickMinimumSpacing = minimumSpacing ?? AutomaticTickMinimumSpacing;
            TickLabelFormatter = formatter ?? TickLabelFormatter;
        }

        /// <summary>
        /// Clear the list of manually-defined ticks.
        /// To enable automatic tick placement call 
        /// </summary>
        public void ClearTicks()
        {
            ManualTicks.Clear();
        }

        /// <summary>
        /// Add a tick to the list of manually-defined ticks (disabling automatic tick placement)
        /// </summary>
        /// <param name="fraction">from 0 (darkest) to 1 (brightest)</param>
        /// <param name="label">string displayed beside the tick</param>
        public void AddTick(double fraction, string label)
        {
            ManualTicks.Add(new(fraction, label, true, false));
        }

        /// <summary>
        /// Manually define ticks (disabling automatic tick placement)
        /// </summary>
        /// <param name="fractions">from 0 (darkest) to 1 (brightest)</param>
        /// <param name="labels">strings displayed beside the ticks</param>
        public void AddTicks(double[] fractions, string[] labels)
        {
            if (fractions.Length != labels.Length)
                throw new("fractions and labels must have the same length");

            for (int i = 0; i < fractions.Length; i++)
            {
                ManualTicks.Add(new(fractions[i], labels[i], true, false));
            }
        }

        /// <summary>
        /// Manually define ticks as a fraction from 0 to 1 (disabling automatic tick placement)
        /// </summary>
        /// <param name="fractions">from 0 (darkest) to 1 (brightest)</param>
        /// <param name="labels">strings displayed beside the ticks</param>
        public void SetTicks(double[] fractions, string[] labels)
        {
            if (fractions.Length != labels.Length)
                throw new("fractions and labels must have the same length");

            ClearTicks();
            AddTicks(fractions, labels);
        }

        /// <summary>
        /// Manually define ticks by value within a range (disabling automatic tick placement)
        /// </summary>
        /// <param name="values">position for each tick</param>
        /// <param name="labels">label for each tick</param>
        /// <param name="min">colorbar range minimum</param>
        /// <param name="max">colorbar range maximum</param>
        public void SetTicks(double[] values, string[] labels, double min, double max)
        {
            if (values.Length != labels.Length)
                throw new("fractions and labels must have the same length");

            double span = max - min;
            double[] fracs = values.Select(x => (x - min) / span).ToArray();
            SetTicks(fracs, labels);
        }

        /// <summary>
        /// Re-Render the colorbar using a new colormap
        /// </summary>
        public void UpdateColormap(Colormap newColormap)
        {
            Colormap = newColormap ?? Colormap.Viridis;
            UpdateBitmap();
        }

        private void UpdateBitmap()
        {
            BmpScale?.Dispose();
            BmpScale = GetBitmap();
        }

        /// <summary>
        /// Return a Bitmap of just the color portion of the colorbar.
        /// The width is defined by the Width field
        /// The height will be 256
        /// </summary>
        /// <returns></returns>
        public Bitmap GetBitmap() =>
            Colormap.Colorbar(Colormap, Width, 256, true, MinColor, MaxColor);

        /// <summary>
        /// Return a Bitmap of just the color portion of the colorbar
        /// </summary>
        /// <param name="width">width of the Bitmap</param>
        /// <param name="height">height of the Bitmap</param>
        /// <param name="vertical">if true, colormap will be vertically oriented (tall and skinny)</param>
        /// <returns></returns>
        public Bitmap GetBitmap(int width, int height, bool vertical = true) =>
            Colormap.Colorbar(Colormap, width, height, vertical, MinColor, MaxColor);

        /// <summary>
        /// Adjust the layout of the given plot based on size information from the previous render
        /// </summary>
        public void ResizeLayout(Plot plot)
        {
            if (LastRenderWidth < 0)
            {
                throw new InvalidOperationException("ResizeLayout() must be called after at least one render.");
            }

            var axis = Edge switch
            {
                Renderable.Edge.Left => plot.LeftAxis,
                Renderable.Edge.Right => plot.RightAxis,
                _ => throw new NotImplementedException(),
            };

            axis.SetSizeLimit(min: LastRenderWidth + 5);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (BmpScale is null)
                UpdateBitmap();

            RectangleF colorbarRect = RenderColorbar(dims, bmp);
            float ticksPartWidth = RenderTicks(dims, bmp, lowQuality, colorbarRect);
            float labelHeight = RenderLabel(dims, bmp, lowQuality, colorbarRect, ticksPartWidth);

            LastRenderWidth = labelHeight + ticksPartWidth + colorbarRect.Width;
        }

        /// <summary>
        /// Return a list of ticks evenly spaced between the min and max values.
        /// </summary>
        /// <param name="height">height of the vertical colorbar</param>
        /// <param name="tickSpacing">minimum pixel distance between adjacent ticks</param>
        /// <returns></returns>
        private List<Tick> GetEvenlySpacedTicks(float height, double tickSpacing)
        {
            List<Tick> ticks = new();
            int tickCount = (int)(height / tickSpacing);
            tickCount = Math.Max(tickCount, 1);
            double tickSpacingFraction = 1.0 / tickCount;
            double valueSpan = MaxValue - MinValue;
            for (int i = 0; i <= tickCount; i++)
            {
                double colorbarFraction = tickSpacingFraction * i;
                double tickPosition = MinValue + colorbarFraction * valueSpan;

                string tickLabel = TickLabelFormatter(tickPosition);
                if (MinIsClipped && i == 0)
                    tickLabel = "≤" + tickLabel;
                if (MaxIsClipped && i == tickCount)
                    tickLabel = "≥" + tickLabel;

                Tick tick = new(colorbarFraction, tickLabel, isMajor: true, isDateTime: false);
                ticks.Add(tick);
            }

            return ticks;
        }

        private RectangleF RenderColorbar(PlotDimensions dims, Bitmap bmp)
        {
            SizeF size = new(Width, dims.DataHeight);

            float locationY = dims.DataOffsetY;
            float locationX;
            if (Edge == Renderable.Edge.Right)
                locationX = dims.DataOffsetX + dims.DataWidth + DataAreaPadding;
            else if (Edge == Renderable.Edge.Left)
                locationX = DataAreaPadding;
            else
                throw new InvalidOperationException($"Unsupported {nameof(Edge)}: {Edge}");
            PointF location = new(locationX, locationY);

            RectangleF rect = new(location, size);

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality: true, clipToDataArea: false))
            using (var pen = GDI.Pen(TickMarkColor))
            {
                gfx.DrawImage(BmpScale, location.X, location.Y, size.Width, size.Height + 1);
                gfx.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            }

            return rect;
        }

        /// <summary>
        /// returns the width of the Ticks+string labels part
        /// </summary>
        /// <param name="dims"></param>
        /// <param name="bmp"></param>
        /// <param name="lowQuality"></param>
        /// <param name="colorbarRect"></param>
        /// <returns></returns>
        private float RenderTicks(PlotDimensions dims, Bitmap bmp, bool lowQuality, RectangleF colorbarRect)
        {
            float tickLeftPx = colorbarRect.Right;
            float tickRightPx = tickLeftPx + TickMarkLength;
            float tickLabelPx = tickRightPx + 2;

            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality, false);
            using var tickMarkPen = GDI.Pen(TickMarkColor, TickMarkWidth);
            using var tickLabelBrush = GDI.Brush(TickLabelFont.Color);
            using var tickFont = GDI.Font(TickLabelFont);
            using var sf = new StringFormat() { LineAlignment = StringAlignment.Center };

            bool useManualTicks = (ManualTicks.Count > 0 || AutomaticTickEnable == false);
            List<Tick> ticks = useManualTicks ? ManualTicks : GetEvenlySpacedTicks(colorbarRect.Height, AutomaticTickMinimumSpacing);

            foreach (Tick tick in ticks)
            {
                float y = colorbarRect.Top + (float)((1 - tick.Position) * colorbarRect.Height);
                gfx.DrawLine(tickMarkPen, tickLeftPx, y, tickRightPx, y);
                gfx.DrawString(tick.Label, tickFont, tickLabelBrush, tickLabelPx, y, sf);
            }

            Tick largestTick = ticks.OrderByDescending(x => x.Label.Length).FirstOrDefault();
            SizeF largestTickRect = gfx.MeasureString(largestTick.Label, tickFont);
            float mostRightPx = largestTickRect.Width + tickLabelPx;

            return mostRightPx - tickLeftPx;
        }

        private float RenderLabel(PlotDimensions dims, Bitmap bmp, bool lowQuality, RectangleF colorbarRect, float ticksWidth)
        {
            if (string.IsNullOrWhiteSpace(Label) || !LabelIsVisible)
                return 0;

            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality, false);
            using Brush brush = GDI.Brush(TickLabelFont.Color);
            using System.Drawing.Font font = GDI.Font(LabelFont);
            using StringFormat sf = new() { Alignment = StringAlignment.Center };

            if (LabelMargin < 0) LabelMargin = ticksWidth + 2;

            float x = colorbarRect.Right + LabelMargin;
            float y = (colorbarRect.Top + colorbarRect.Bottom) / 2;

            gfx.TranslateTransform(x, y);
            gfx.RotateTransform(-90);
            gfx.DrawString(Label, font, brush, 0, 0, sf);
            GDI.ResetTransformPreservingScale(gfx, dims);

            return GDI.MeasureString(gfx, Label, LabelFont).Height;
        }
    }
}
