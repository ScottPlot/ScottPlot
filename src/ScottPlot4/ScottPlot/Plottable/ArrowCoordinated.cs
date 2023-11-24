using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// An arrow with X/Y coordinates for the base and the tip
    /// </summary>
    public class ArrowCoordinated : IPlottable, IHasPixelOffset, IHasLine, IHasColor
    {
        /// <summary>
        /// Location of the arrow base in coordinate space
        /// </summary>
        public Coordinate Base = new(0, 0);

        /// <summary>
        /// Location of the arrow base in coordinate space
        /// </summary>
        public Coordinate Tip = new(0, 0);

        /// <summary>
        /// Color of the arrow and arrowhead
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        /// <summary>
        /// Color of the arrow and arrowhead
        /// </summary>
        public Color LineColor { get => Color; set { Color = value; } }

        /// <summary>
        /// Thickness of the arrow line
        /// </summary>
        public double LineWidth { get; set; } = 2;

        /// <summary>
        /// Style of the arrow line
        /// </summary>
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;

        /// <summary>
        /// Label to appear in the legend
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Width of the arrowhead (pixels)
        /// </summary>
        public double ArrowheadWidth { get; set; } = 3;

        /// <summary>
        /// Height of the arrowhead (pixels)
        /// </summary>
        public double ArrowheadLength { get; set; } = 3;

        /// <summary>
        /// The arrow will be lengthened to ensure it is at least this size on the screen
        /// </summary>
        public float MinimumLengthPixels { get; set; } = 0;

        /// <summary>
        /// Marker to be drawn at the base (if MarkerSize > 0)
        /// </summary>
        public MarkerShape MarkerShape { get; set; } = MarkerShape.filledCircle;

        /// <summary>
        /// Size of marker (in pixels) to draw at the base
        /// </summary>
        public float MarkerSize { get; set; } = 0;

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public float PixelOffsetX { get; set; } = 0;
        public float PixelOffsetY { get; set; } = 0;

        public ArrowCoordinated(Coordinate arrowBase, Coordinate arrowTip)
        {
            Base.X = arrowBase.X;
            Base.Y = arrowBase.Y;
            Tip.X = arrowTip.X;
            Tip.Y = arrowTip.Y;
        }

        public ArrowCoordinated(double xBase, double yBase, double xTip, double yTip)
        {
            Base.X = xBase;
            Base.Y = yBase;
            Tip.X = xTip;
            Tip.Y = yTip;
        }

        public AxisLimits GetAxisLimits()
        {
            double xMin = Math.Min(Base.X, Tip.X);
            double xMax = Math.Max(Base.X, Tip.X);
            double yMin = Math.Min(Base.Y, Tip.Y);
            double yMax = Math.Max(Base.Y, Tip.Y);
            return new AxisLimits(xMin, xMax, yMin, yMax);
        }

        public LegendItem[] GetLegendItems()
        {
            LegendItem item = new(this)
            {
                label = Label,
                lineWidth = LineWidth,
                color = Color,
            };

            return LegendItem.Single(item);
        }

        public void ValidateData(bool deep = false)
        {
            if (!Base.IsFinite() || !Tip.IsFinite())
                throw new InvalidOperationException("Base and Tip coordinates must be finite");
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            using Graphics gfx = Drawing.GDI.Graphics(bmp, dims, lowQuality);
            using Pen penLine = Drawing.GDI.Pen(Color, LineWidth, LineStyle, true);

            Pixel basePixel = dims.GetPixel(Base);
            Pixel tipPixel = dims.GetPixel(Tip);

            basePixel.Translate(PixelOffsetX, -PixelOffsetY);
            tipPixel.Translate(PixelOffsetX, -PixelOffsetY);

            float lengthPixels = basePixel.Distance(tipPixel);
            if (lengthPixels < MinimumLengthPixels)
            {
                float expandBy = MinimumLengthPixels / lengthPixels;
                float dX = tipPixel.X - basePixel.X;
                float dY = tipPixel.Y - basePixel.Y;
                basePixel.X = tipPixel.X - dX * expandBy;
                basePixel.Y = tipPixel.Y - dY * expandBy;
            }

            MarkerTools.DrawMarker(gfx, new(basePixel.X, basePixel.Y), MarkerShape, MarkerSize, Color);

            penLine.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap((float)ArrowheadWidth, (float)ArrowheadLength, true);
            penLine.StartCap = System.Drawing.Drawing2D.LineCap.Flat;
            gfx.DrawLine(penLine, basePixel.X, basePixel.Y, tipPixel.X, tipPixel.Y);
        }
    }
}
