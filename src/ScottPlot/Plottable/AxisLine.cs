using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Horizontal line at a Y position
    /// </summary>
    public class HLine : AxisLine
    {
        /// <summary>
        /// Y position to render the line
        /// </summary>
        public double Y { get => Position; set => Position = value; }
        public override string ToString() => $"Horizontal line at Y={Y}";
        public HLine() : base(true) { }
    }

    /// <summary>
    /// Vertical line at an X position
    /// </summary>
    public class VLine : AxisLine
    {
        /// <summary>
        /// X position to render the line
        /// </summary>
        public double X { get => Position; set => Position = value; }
        public override string ToString() => $"Vertical line at X={X}";
        public VLine() : base(false) { }
    }

    public abstract class AxisLine : IDraggable, IPlottable
    {
        // orientation and location
        protected double Position;
        private readonly bool IsHorizontal;

        // customizations
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public LineStyle LineStyle = LineStyle.Solid;
        public float LineWidth = 1;
        public Color Color = Color.Black;
        public string Label;

        // mouse interaction
        public bool DragEnabled { get; set; } = false;
        public Cursor DragCursor => IsHorizontal ? Cursor.NS : Cursor.WE;
        public double DragLimitMin = double.NegativeInfinity;
        public double DragLimitMax = double.PositiveInfinity;

        /// <summary>
        /// This event is invoked after the line is dragged 
        /// </summary>
        public event EventHandler Dragged = delegate { };

        public AxisLine(bool isHorizontal)
        {
            IsHorizontal = isHorizontal;
        }

        public AxisLimits GetAxisLimits() =>
            IsHorizontal ?
            new AxisLimits(double.NaN, double.NaN, Position, Position) :
            new AxisLimits(Position, Position, double.NaN, double.NaN);

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(Position) || double.IsInfinity(Position))
                throw new InvalidOperationException("position must be a valid number");
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var pen = GDI.Pen(Color, LineWidth, LineStyle, true))
            {
                if (IsHorizontal)
                {
                    float pixelX1 = dims.GetPixelX(dims.XMin);
                    float pixelX2 = dims.GetPixelX(dims.XMax);
                    float pixelY = dims.GetPixelY(Position);
                    gfx.DrawLine(pen, pixelX1, pixelY, pixelX2, pixelY);
                }
                else
                {
                    float pixelX = dims.GetPixelX(Position);
                    float pixelY1 = dims.GetPixelY(dims.YMin);
                    float pixelY2 = dims.GetPixelY(dims.YMax);
                    gfx.DrawLine(pen, pixelX, pixelY1, pixelX, pixelY2);
                }
            }
        }

        /// <summary>
        /// Move the line to a new coordinate in plot space.
        /// </summary>
        /// <param name="coordinateX">new X position</param>
        /// <param name="coordinateY">new Y position</param>
        /// <param name="fixedSize">This argument is ignored</param>
        public void DragTo(double coordinateX, double coordinateY, bool fixedSize)
        {
            if (!DragEnabled)
                return;

            if (IsHorizontal)
            {
                if (coordinateY < DragLimitMin) coordinateY = DragLimitMin;
                if (coordinateY > DragLimitMax) coordinateY = DragLimitMax;
                Position = coordinateY;
            }
            else
            {
                if (coordinateX < DragLimitMin) coordinateX = DragLimitMin;
                if (coordinateX > DragLimitMax) coordinateX = DragLimitMax;
                Position = coordinateX;
            }

            Dragged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Return True if the line is within a certain number of pixels (snap) to the mouse
        /// </summary>
        /// <param name="coordinateX">mouse position (coordinate space)</param>
        /// <param name="coordinateY">mouse position (coordinate space)</param>
        /// <param name="snapX">snap distance (pixels)</param>
        /// <param name="snapY">snap distance (pixels)</param>
        /// <returns></returns>
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY) =>
            IsHorizontal ?
            Math.Abs(Position - coordinateY) <= snapY :
            Math.Abs(Position - coordinateX) <= snapX;

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem()
            {
                label = Label,
                color = Color,
                lineStyle = LineStyle,
                lineWidth = LineWidth,
                markerShape = MarkerShape.none
            };
            return new LegendItem[] { singleItem };
        }
    }
}
