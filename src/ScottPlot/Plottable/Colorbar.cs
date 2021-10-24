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
    public class Colorbar : IPlottable
    {
        public Renderable.Edge Edge = Renderable.Edge.Right;

        private Colormap Colormap;
        private Bitmap BmpScale;

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get => 0; set { } }
        public int YAxisIndex { get => 0; set { } }

        /// <summary>
        /// Width of the colorbar rectangle
        /// </summary>
        public int Width = 20;

        public readonly Drawing.Font TickLabelFont = new();
        public Color TickMarkColor = Color.Black;
        public float TickMarkLength = 3;
        public float TickMarkWidth = 1;

        private readonly List<Tick> ManualTicks = new();
        private bool AutomaticTickEnable = true;
        private bool AutomaticTickGreaterLesser = false;
        private int AutomaticTickMinimumSpacing = 40;
        private Func<double, string> AutomaticTickFormatter = position => $"{position:F2}";

        private double _Min = 0;
        public double Min
        {
            get => (Plottable is IHasColormap p) ? p.ColormapMin : _Min;
            set => _Min = value;
        }

        private double _Max = 1;
        public double Max
        {
            get => (Plottable is IHasColormap p) ? p.ColormapMax : _Max;
            set => _Max = value;
        }

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

        /// <summary>
        /// Configure ticks that are automatically generated in the absense of manually-added ticks
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="minimumSpacing">Minimum number of vertical pixels between tick positions</param>
        /// <param name="formatter">Optional custom string formatter to translate tick positions to labels</param>
        /// <param name="greaterLesser">Prefix the min and max labels with ≥ and ≤</param>
        public void AutomaticTicks(bool enable = true, int? minimumSpacing = null, Func<double, string> formatter = null, bool greaterLesser = false)
        {
            if (enable)
                ManualTicks.Clear();

            AutomaticTickEnable = enable;
            AutomaticTickMinimumSpacing = minimumSpacing ?? AutomaticTickMinimumSpacing;
            AutomaticTickFormatter = formatter ?? AutomaticTickFormatter;
            AutomaticTickGreaterLesser = greaterLesser;
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
        /// Manually define ticks (disabling automatic tick placement)
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

        public LegendItem[] GetLegendItems() => null;

        public AxisLimits GetAxisLimits() => new(double.NaN, double.NaN, double.NaN, double.NaN);

        public void ValidateData(bool deep = false) { }

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
            Colormap.Colorbar(Colormap, Width, 256, true);

        /// <summary>
        /// Return a Bitmap of just the color portion of the colorbar
        /// </summary>
        /// <param name="width">width of the Bitmap</param>
        /// <param name="height">height of the Bitmap</param>
        /// <param name="vertical">if true, colormap will be vertically oriented (tall and skinny)</param>
        /// <returns></returns>
        public Bitmap GetBitmap(int width, int height, bool vertical = true) =>
            Colormap.Colorbar(Colormap, width, height, vertical);

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (BmpScale is null)
                UpdateBitmap();

            RectangleF colorbarRect = RenderColorbar(dims, bmp);

            RenderTicks(dims, bmp, lowQuality, colorbarRect);
        }

        private List<Tick> CalculateTicks(float height, double tickSpacing)
        {
            List<Tick> ticks = new();
            int tickCount = (int)(height / tickSpacing);
            tickCount = Math.Max(tickCount, 1);
            double tickSpacingFraction = 1.0 / tickCount;
            double valueSpan = Max - Min;
            for (int i = 0; i <= tickCount; i++)
            {
                double colorbarFraction = tickSpacingFraction * i;
                double tickPosition = Min + colorbarFraction * valueSpan;

                string tickLabel = AutomaticTickFormatter(tickPosition);
                if (AutomaticTickGreaterLesser)
                {
                    if (i == 0)
                        tickLabel = "≤" + tickLabel;
                    else if (i == tickCount)
                        tickLabel = "≥" + tickLabel;
                }

                Tick tick = new(colorbarFraction, tickLabel, isMajor: true, isDateTime: false);
                ticks.Add(tick);
            }

            return ticks;
        }

        private RectangleF RenderColorbar(PlotDimensions dims, Bitmap bmp)
        {
            float scaleLeftPad = 10;

            PointF location = new(dims.DataOffsetX + dims.DataWidth + scaleLeftPad, dims.DataOffsetY);
            SizeF size = new(Width, dims.DataHeight);
            RectangleF rect = new(location, size);

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality: true, clipToDataArea: false))
            using (var pen = GDI.Pen(Color.Black))
            {
                gfx.DrawImage(BmpScale, location.X, location.Y, size.Width, size.Height + 1);
                gfx.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            }

            return rect;
        }

        private void RenderTicks(PlotDimensions dims, Bitmap bmp, bool lowQuality, RectangleF colorbarRect)
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
            List<Tick> ticks = useManualTicks ? ManualTicks : CalculateTicks(colorbarRect.Height, AutomaticTickMinimumSpacing);

            foreach (Tick tick in ticks)
            {
                float y = colorbarRect.Top + (float)((1 - tick.Position) * colorbarRect.Height);
                gfx.DrawLine(tickMarkPen, tickLeftPx, y, tickRightPx, y);
                gfx.DrawString(tick.Label, tickFont, tickLabelBrush, tickLabelPx, y, sf);
            }
        }
    }
}
