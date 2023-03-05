using ScottPlot.Drawing;
using ScottPlot.SnapLogic;
using System;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlot.Plottable
{
    public abstract class AxisSpan : IPlottable, IDraggable, IDraggableSpan, IHasColor, IHasArea
    {
        // location and orientation
        protected double Position1;
        protected double Position2;
        private double Min => Math.Min(Position1, Position2);
        private double Max => Math.Max(Position1, Position2);
        readonly bool IsHorizontal;

        /// <summary>
        /// If true, AxisAuto() will ignore the position of this span when determining axis limits
        /// </summary>
        public bool IgnoreAxisAuto { get; set; } = false;

        // configuration
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public bool IsVisible { get; set; } = true;
        public Color Color { get; set; } = Color.FromArgb(128, Color.Magenta);
        public Color BorderColor { get; set; } = Color.Transparent;
        public float BorderLineWidth { get; set; } = 0;
        public LineStyle BorderLineStyle { get; set; } = LineStyle.None;
        public Color HatchColor { get; set; } = Color.Transparent;
        public HatchStyle HatchStyle { get; set; } = Drawing.HatchStyle.None;
        public string Label { get; set; } = null;

        // mouse interaction
        public bool DragEnabled { get; set; }
        public bool DragFixedSize { get; set; }
        public double DragLimitMin { get; set; } = double.NegativeInfinity;
        public double DragLimitMax { get; set; } = double.PositiveInfinity;
        public Cursor DragCursor => IsHorizontal ? Cursor.WE : Cursor.NS;

        /// <summary>
        /// This event is invoked after the line is dragged 
        /// </summary>
        public event EventHandler Dragged = delegate { };

        /// <summary>
        /// This event is invoked after the Edge1 is dragged 
        /// </summary>
        public event EventHandler<double> Edge1Dragged = delegate { };
        /// <summary>
        /// This event is invoked after the Edge2 is dragged 
        /// </summary>
        public event EventHandler<double> Edge2Dragged = delegate { };
        /// <summary>
        /// This event is invoked after the min edge is dragged 
        /// </summary>
        public event EventHandler<double> MinDragged = delegate { };
        /// <summary>
        /// This event is invoked after the max edge is dragged 
        /// </summary>
        public event EventHandler<double> MaxDragged = delegate { };

        /// <summary>
        /// This function applies snapping logic while dragging
        /// </summary>
        public ISnap2D DragSnap { get; set; } = new NoSnap2D();

        public AxisSpan(bool isHorizontal)
        {
            IsHorizontal = isHorizontal;
        }

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(Position1) || double.IsInfinity(Position1))
                throw new InvalidOperationException("position1 must be a valid number");

            if (double.IsNaN(Position2) || double.IsInfinity(Position2))
                throw new InvalidOperationException("position2 must be a valid number");
        }

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem(this)
            {
                label = Label,
                color = Color,
                borderWith = Math.Min(BorderLineWidth, 3),
                borderColor = BorderColor,
                borderLineStyle = BorderLineStyle,
                hatchColor = HatchColor,
                hatchStyle = HatchStyle,
            };
            return LegendItem.Single(singleItem);
        }

        public AxisLimits GetAxisLimits()
        {
            if (IgnoreAxisAuto)
                return AxisLimits.NoLimits;

            if (IsHorizontal)
                return AxisLimits.HorizontalLimitsOnly(Min, Max);
            else
                return AxisLimits.VerticalLimitsOnly(Min, Max);
        }

        private enum Edge { Edge1, Edge2, Neither };
        Edge edgeUnderMouse = Edge.Neither;

        /// <summary>
        /// Return True if either span edge is within a certain number of pixels (snap) to the mouse
        /// </summary>
        /// <param name="coordinateX">mouse position (coordinate space)</param>
        /// <param name="coordinateY">mouse position (coordinate space)</param>
        /// <param name="snapX">snap distance (pixels)</param>
        /// <param name="snapY">snap distance (pixels)</param>
        /// <returns></returns>
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            if (IsHorizontal)
            {
                if (Math.Abs(Position1 - coordinateX) <= snapX)
                    edgeUnderMouse = Edge.Edge1;
                else if (Math.Abs(Position2 - coordinateX) <= snapX)
                    edgeUnderMouse = Edge.Edge2;
                else
                    edgeUnderMouse = Edge.Neither;
            }
            else
            {
                if (Math.Abs(Position1 - coordinateY) <= snapY)
                    edgeUnderMouse = Edge.Edge1;
                else if (Math.Abs(Position2 - coordinateY) <= snapY)
                    edgeUnderMouse = Edge.Edge2;
                else
                    edgeUnderMouse = Edge.Neither;
            }

            return edgeUnderMouse != Edge.Neither;
        }

        /// <summary>
        /// Move the span to a new coordinate in plot space.
        /// </summary>
        /// <param name="coordinateX">new X position</param>
        /// <param name="coordinateY">new Y position</param>
        /// <param name="fixedSize">if True, both edges will be moved to maintain the size of the span</param>
        public void DragTo(double coordinateX, double coordinateY, bool fixedSize)
        {
            if (!DragEnabled)
                return;

            bool position1Changed = false;
            bool position2Changed = false;

            Coordinate original = new(coordinateX, coordinateY);
            Coordinate snapped = DragSnap.Snap(original);
            coordinateX = snapped.X;
            coordinateY = snapped.Y;

            if (IsHorizontal)
            {
                coordinateX = Math.Max(coordinateX, DragLimitMin);
                coordinateX = Math.Min(coordinateX, DragLimitMax);
            }
            else
            {
                coordinateY = Math.Max(coordinateY, DragLimitMin);
                coordinateY = Math.Min(coordinateY, DragLimitMax);
            }

            double sizeBeforeDrag = Position2 - Position1;
            if (edgeUnderMouse == Edge.Edge1)
            {
                Position1 = IsHorizontal ? coordinateX : coordinateY;
                position1Changed = true;
                if (DragFixedSize || fixedSize)
                {
                    Position2 = Position1 + sizeBeforeDrag;
                    position2Changed = true;
                }
            }
            else if (edgeUnderMouse == Edge.Edge2)
            {
                Position2 = IsHorizontal ? coordinateX : coordinateY;
                position2Changed = true;
                if (DragFixedSize || fixedSize)
                {
                    Position1 = Position2 - sizeBeforeDrag;
                    position1Changed = true;
                }
            }
            else
            {
                Debug.WriteLine("DragTo() called but no side selected. Call IsUnderMouse() to select a side.");
            }

            // ensure fixed-width spans stay entirely inside the allowable range
            double belowLimit = DragLimitMin - Position1;
            double aboveLimit = Position2 - DragLimitMax;
            if (belowLimit > 0)
            {
                Position1 += belowLimit;
                Position2 += belowLimit;
                position1Changed = true;
                position2Changed = true;
            }
            if (aboveLimit > 0)
            {
                Position1 -= aboveLimit;
                Position2 -= aboveLimit;
                position1Changed = true;
                position2Changed = true;
            }

            Dragged(this, EventArgs.Empty);
            if (position1Changed)
            {
                Edge1Dragged(this, Position1);
                if (Position1 <= Position2)
                    MinDragged(this, Position1);
                else
                    MaxDragged(this, Position1);
            }
            if (position2Changed)
            {
                Edge2Dragged(this, Position2);
                if (Position2 <= Position1)
                    MinDragged(this, Position2);
                else
                    MaxDragged(this, Position2);
            }
        }

        private RectangleF GetClippedRectangle(PlotDimensions dims)
        {
            // clip the rectangle to the size of the data area to avoid GDI rendering errors
            float ClippedPixelX(double x) => dims.GetPixelX(Math.Max(dims.XMin, Math.Min(x, dims.XMax)));
            float ClippedPixelY(double y) => dims.GetPixelY(Math.Max(dims.YMin, Math.Min(y, dims.YMax)));

            float left = IsHorizontal ? ClippedPixelX(Min) : dims.DataOffsetX;
            float right = IsHorizontal ? ClippedPixelX(Max) : dims.DataOffsetX + dims.DataWidth;
            float top = IsHorizontal ? dims.DataOffsetY : ClippedPixelY(Max);
            float bottom = IsHorizontal ? dims.DataOffsetY + dims.DataHeight : ClippedPixelY(Min);

            float width = right - left + 1;
            float height = bottom - top + 1;

            // expand slightly so anti-aliasing transparency is not observed at the edges of frameless plots
            if (IsHorizontal)
            {
                top -= 1;
                height += 2;
            }
            else
            {
                left -= 1;
                width += 2;
            }

            return new RectangleF(left, top, width, height);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using var gfx = GDI.Graphics(bmp, dims, lowQuality);
            using var brush = GDI.Brush(Color, HatchColor, HatchStyle);
            using var pen = GDI.Pen(BorderColor, BorderLineWidth, BorderLineStyle);
            RectangleF rect = GetClippedRectangle(dims);
            gfx.FillRectangle(brush, rect);
            if (BorderLineWidth > 0 && BorderColor != Color.Transparent && BorderLineStyle != LineStyle.None)
                gfx.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
