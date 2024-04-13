using ScottPlot.Drawing;
using ScottPlot.Renderable;
using System;
using System.Drawing;
using ScottPlot.SnapLogic;

namespace ScottPlot.Plottable
{
    public abstract class AxisLine : IDraggable, IPlottable, IHasLine, IHasColor
    {
        /// <summary>
        /// Location of the line (Y position if horizontal line, X position if vertical line)
        /// </summary>
        protected double Position { get; set; }

        /// <summary>
        /// If True, the position will be labeled on the axis using the PositionFormatter
        /// </summary>
        public bool PositionLabel { get; set; } = false;

        /// <summary>
        /// Font to use for position labels (labels drawn over the axis)
        /// </summary>
        public readonly Drawing.Font PositionLabelFont = new() { Color = Color.White, Bold = true };

        /// <summary>
        /// Color to use behind the position labels
        /// </summary>
        public Color PositionLabelBackground { get; set; } = Color.Black;

        public HorizontalAlignment PositionLabelAlignmentX { get; set; } = HorizontalAlignment.Center;

        public VerticalAlignment PositionLabelAlignmentY { get; set; } = VerticalAlignment.Middle;

        /// <summary>
        /// If true the position label will be drawn on the right or top of the data area.
        /// </summary>
        public bool PositionLabelOppositeAxis { get; set; } = false;

        /// <summary>
        /// If provided, the position label will be rendered on this axis
        /// </summary>
        public Axis PositionLabelAxis { get; set; } = null;

        /// <summary>
        /// This method generates the position label text for numeric (non-DateTime) axes.
        /// For DateTime axes assign your own format string that uses DateTime.FromOADate(position).
        /// </summary>
        public Func<double, string> PositionFormatter { get; set; } = position => position.ToString("F2");

        /// <summary>
        /// Position of the axis line in DateTime (OADate) units.
        /// </summary>
        public DateTime DateTime
        {
            get => DateTime.FromOADate(Position);
            set => Position = value.ToOADate();
        }

        /// <summary>
        /// Indicates whether the line is horizontal (position in Y units) or vertical (position in X units)
        /// </summary>
        private readonly bool IsHorizontal;

        /// <summary>
        /// If true, AxisAuto() will ignore the position of this line when determining axis limits
        /// </summary>
        public bool IgnoreAxisAuto { get; set; } = false;

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;
        public double LineWidth { get; set; } = 1;
        public Color Color { get; set; } = Color.Black;
        public Color LineColor { get => Color; set { Color = value; } }

        /// <summary>
        /// Text that appears in the legend
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Indicates whether this line is draggable in user controls.
        /// </summary>
        public bool DragEnabled { get; set; } = false;

        /// <summary>
        /// Cursor to display while hovering over this line if dragging is enabled.
        /// </summary>
        public Cursor DragCursor => IsHorizontal ? Cursor.NS : Cursor.WE;

        /// <summary>
        /// If dragging is enabled the line cannot be dragged more negative than this position
        /// </summary>
        public double DragLimitMin { get; set; } = double.NegativeInfinity;

        /// <summary>
        /// If dragging is enabled the line cannot be dragged more positive than this position
        /// </summary>
        public double DragLimitMax { get; set; } = double.PositiveInfinity;

        /// <summary>
        /// This event is invoked after the line is dragged
        /// </summary>
        public event EventHandler Dragged = delegate { };

        /// <summary>
        /// The lower bound of the axis line.
        /// </summary>
        public double Min { get; set; } = double.NegativeInfinity;

        /// <summary>
        /// The upper bound of the axis line.
        /// </summary>
        public double Max { get; set; } = double.PositiveInfinity;

        /// <summary>
        /// This function applies snapping logic while dragging
        /// </summary>
        public ISnap2D DragSnap { get; set; } = new NoSnap2D();

        public AxisLine(bool isHorizontal)
        {
            IsHorizontal = isHorizontal;
        }

        public AxisLimits GetAxisLimits()
        {
            if (IgnoreAxisAuto)
                return AxisLimits.NoLimits;

            if (IsHorizontal)
                return AxisLimits.VerticalLimitsOnly(Position, Position);
            else
                return AxisLimits.HorizontalLimitsOnly(Position, Position);
        }

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(Position) || double.IsInfinity(Position))
                throw new InvalidOperationException("position must be a valid number");

            if (PositionFormatter is null)
                throw new NullReferenceException(nameof(PositionFormatter));
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!IsVisible)
                return;

            if (LineWidth > 0)
                RenderLine(dims, bmp, lowQuality);

            if (PositionLabel)
                RenderPositionLabel(dims, bmp, lowQuality);
        }

        public void RenderLine(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            using var gfx = GDI.Graphics(bmp, dims, lowQuality);
            using var pen = GDI.Pen(Color, LineWidth, LineStyle, true);

            if (IsHorizontal)
            {
                float pixelY = dims.GetPixelY(Position);

                double xMin = Math.Max(Min, dims.XMin);
                double xMax = Math.Min(Max, dims.XMax);
                float pixelX1 = dims.GetPixelX(xMin);
                float pixelX2 = dims.GetPixelX(xMax);

                gfx.DrawLine(pen, pixelX1, pixelY, pixelX2, pixelY);
            }
            else
            {
                float pixelX = dims.GetPixelX(Position);

                double yMin = Math.Max(Min, dims.YMin);
                double yMax = Math.Min(Max, dims.YMax);
                float pixelY1 = dims.GetPixelY(yMin);
                float pixelY2 = dims.GetPixelY(yMax);

                gfx.DrawLine(pen, pixelX, pixelY1, pixelX, pixelY2);
            }
        }

        private void RenderPositionLabel(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            using var gfx = GDI.Graphics(bmp, dims, lowQuality, clipToDataArea: false);
            using var pen = GDI.Pen(Color, LineWidth, LineStyle, true);

            using var fnt = GDI.Font(PositionLabelFont);
            using var fillBrush = GDI.Brush(PositionLabelBackground);
            using var fontBrush = GDI.Brush(PositionLabelFont.Color);

            float axisOffset = PositionLabelAxis is not null ? PositionLabelAxis.PixelOffset : 0;

            if (IsHorizontal)
            {
                if (Position > dims.YMax || Position < dims.YMin)
                    return;

                if (PositionLabelOppositeAxis)
                {
                    GDI.DrawLabel(gfx,
                        text: PositionFormatter(Position),
                        x: dims.DataOffsetX + dims.DataWidth + axisOffset,
                        y: dims.GetPixelY(Position),
                        PositionLabelFont.Name, PositionLabelFont.Size, PositionLabelFont.Bold,
                        HorizontalAlignment.Left, PositionLabelAlignmentY,
                        PositionLabelFont.Color, PositionLabelBackground);
                }
                else
                {
                    GDI.DrawLabel(gfx,
                        text: PositionFormatter(Position),
                        x: dims.DataOffsetX - axisOffset,
                        y: dims.GetPixelY(Position),
                        PositionLabelFont.Name, PositionLabelFont.Size, PositionLabelFont.Bold,
                        HorizontalAlignment.Right, PositionLabelAlignmentY,
                        PositionLabelFont.Color, PositionLabelBackground);
                }

            }
            else
            {
                if (Position > dims.XMax || Position < dims.XMin)
                    return;

                if (PositionLabelOppositeAxis)
                {
                    GDI.DrawLabel(gfx,
                        text: PositionFormatter(Position),
                        x: dims.GetPixelX(Position),
                        y: dims.DataOffsetY - axisOffset,
                        PositionLabelFont.Name, PositionLabelFont.Size, PositionLabelFont.Bold,
                        PositionLabelAlignmentX, VerticalAlignment.Lower,
                        PositionLabelFont.Color, PositionLabelBackground);
                }
                else
                {
                    GDI.DrawLabel(gfx,
                        text: PositionFormatter(Position),
                        x: dims.GetPixelX(Position),
                        y: dims.DataOffsetY + dims.DataHeight + axisOffset,
                        PositionLabelFont.Name, PositionLabelFont.Size, PositionLabelFont.Bold,
                        PositionLabelAlignmentX, VerticalAlignment.Upper,
                        PositionLabelFont.Color, PositionLabelBackground);
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

            Coordinate original = new(coordinateX, coordinateY);
            Coordinate snapped = DragSnap.Snap(original);
            coordinateX = snapped.X;
            coordinateY = snapped.Y;

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
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            return IsHorizontal
                ? Math.Abs(Position - coordinateY) <= snapY
                : Math.Abs(Position - coordinateX) <= snapX;
        }


        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem(this)
            {
                label = Label,
                color = Color,
                lineStyle = LineStyle,
                lineWidth = LineWidth,
                markerShape = MarkerShape.none
            };
            return LegendItem.Single(singleItem);
        }
    }
}
