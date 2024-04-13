using ScottPlot.Drawing;
using System;
using System.Drawing;
using ScottPlot.SnapLogic;

namespace ScottPlot.Plottable
{

    public abstract class RepeatingAxisLine : IDraggable, IPlottable, IHasColor
    {
        /// <summary>
        /// Location of the reference line (Y position if horizontal line, X position if vertical line)
        /// </summary>
        protected double Position;

        /// <summary>
        /// Total number of plotted lines
        /// </summary>
        protected int Count { get; set; } = 2;

        /// <summary>
        /// Offset about Position (in Y position if horizontal line, in X position if vertical line), this offset should be negative
        /// </summary>
        protected int Offset { get; set; } = 0;

        /// <summary>
        /// Shift between lines (in Y if horizontal line, in X if vertical line)
        /// </summary>
        protected double Shift { get; set; } = 1;

        /// <summary>
        /// If RelativePosition is true, then the Shift is interpreted as a ratio of Position, otherwise it is an absolute shift along the axis
        /// </summary>
        protected bool RelativePosition { get; set; } = true;

        /// <summary>
        /// If True, the position will be labeled on the axis using the PositionFormatter
        /// </summary>
        public bool PositionLabel { get; set; } = false;

        /// <summary>
        /// If True, the first line (positioned at the specified X or Y) will be thicker
        /// </summary>
        public bool HighlightReferenceLine { get; set; } = true;

        /// <summary>
        /// Font to use for position labels (labels drawn over the axis)
        /// </summary>
        //public Drawing.Font PositionLabelFont = new(){ Color = Color.White, Bold = true };
        public readonly Drawing.Font PositionLabelFont = new();

        /// <summary>
        /// Color to use behind the position labels
        /// </summary>
        public Color PositionLabelBackground { get; set; } = Color.Black;

        /// <summary>
        /// If true the position label will be drawn on the right or top of the data area.
        /// </summary>
        public bool PositionLabelOppositeAxis { get; set; } = false;

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
        public float LineWidth { get; set; } = 1;
        public Color Color { get; set; } = Color.Black;

        /// <summary>
        /// Text that appears in the legend
        /// </summary>
        public string Label { get; set; } = string.Empty;

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

        public RepeatingAxisLine(bool isHorizontal)
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

            RenderLine(dims, bmp, lowQuality);

            if (PositionLabel)
                RenderPositionLabel(dims, bmp, lowQuality);
        }

        public void RenderLine(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            var gfx = GDI.Graphics(bmp, dims, lowQuality);
            var pen = GDI.Pen(Color, 2 * LineWidth, LineStyle, true);
            var pen2 = GDI.Pen(Color, LineWidth, LineStyle, true);

            if (IsHorizontal)
            {
                for (int i = 0; i < Count; i++)
                {
                    float pixelY = dims.GetPixelY(ComputePosition(Position, Offset, Shift, i, RelativePosition));

                    double xMin = Math.Max(Min, dims.XMin);
                    double xMax = Math.Min(Max, dims.XMax);
                    float pixelX1 = dims.GetPixelX(xMin);
                    float pixelX2 = dims.GetPixelX(xMax);

                    if (HighlightReferenceLine && (i + Offset) == 0)
                    {
                        gfx.DrawLine(pen, pixelX1, pixelY, pixelX2, pixelY);
                    }
                    else
                    {
                        gfx.DrawLine(pen2, pixelX1, pixelY, pixelX2, pixelY);
                    }

                }
            }
            else
            {
                for (int i = 0; i < Count; i++)
                {
                    float pixelX = dims.GetPixelX(ComputePosition(Position, Offset, Shift, i, RelativePosition));

                    double yMin = Math.Max(Min, dims.YMin);
                    double yMax = Math.Min(Max, dims.YMax);
                    float pixelY1 = dims.GetPixelY(yMin);
                    float pixelY2 = dims.GetPixelY(yMax);

                    if (HighlightReferenceLine && (i + Offset) == 0)
                    {
                        gfx.DrawLine(pen, pixelX, pixelY1, pixelX, pixelY2);
                    }
                    else
                    {
                        gfx.DrawLine(pen2, pixelX, pixelY1, pixelX, pixelY2);
                    }
                }
            }
        }

        private void RenderPositionLabel(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            var gfx = GDI.Graphics(bmp, dims, lowQuality, clipToDataArea: false);
            var pen = GDI.Pen(Color, LineWidth, LineStyle, true);

            var fnt = GDI.Font(PositionLabelFont);
            var fillBrush = GDI.Brush(PositionLabelBackground);
            var fontBrush = GDI.Brush(PositionLabelFont.Color);

            if (IsHorizontal)
            {
                for (int i = 0; i < Count; i++)
                {
                    double lineposition = ComputePosition(Position, Offset, Shift, i, RelativePosition);
                    if (lineposition > dims.YMax || lineposition < dims.YMin)
                        continue;

                    float pixelY = dims.GetPixelY(lineposition);
                    string yLabel = PositionFormatter(lineposition);
                    SizeF yLabelSize = GDI.MeasureString(gfx, yLabel, PositionLabelFont);
                    float xPos = PositionLabelOppositeAxis ? dims.DataOffsetX + dims.DataWidth : dims.DataOffsetX - yLabelSize.Width;
                    float yPos = pixelY - yLabelSize.Height / 2;
                    RectangleF xLabelRect = new(xPos, yPos, yLabelSize.Width, yLabelSize.Height);
                    gfx.FillRectangle(fillBrush, xLabelRect);
                    var sf = GDI.StringFormat(HorizontalAlignment.Left, VerticalAlignment.Middle);
                    gfx.DrawString(yLabel, fnt, fontBrush, xPos, pixelY, sf);
                }
            }
            else
            {
                for (int i = 0; i < Count; i++)
                {
                    double lineposition = ComputePosition(Position, Offset, Shift, i, RelativePosition);
                    if (lineposition > dims.XMax || lineposition < dims.XMin)
                        continue;

                    float pixelX = dims.GetPixelX(lineposition);
                    string xLabel = PositionFormatter(lineposition);
                    SizeF xLabelSize = GDI.MeasureString(gfx, xLabel, PositionLabelFont);
                    float xPos = pixelX - xLabelSize.Width / 2;
                    float yPos = PositionLabelOppositeAxis ? dims.DataOffsetY - xLabelSize.Height : dims.DataOffsetY + dims.DataHeight;
                    RectangleF xLabelRect = new(xPos, yPos, xLabelSize.Width, xLabelSize.Height);
                    gfx.FillRectangle(fillBrush, xLabelRect);
                    var sf = GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Upper);
                    gfx.DrawString(xLabel, fnt, fontBrush, pixelX, yPos, sf);
                }
            }
        }

        /// <summary>
        /// Move the reference line to a new coordinate in plot space.
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
        /// Return True if the reference line is within a certain number of pixels (snap) to the mouse
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

        private double ComputePosition(double Position, int Offset, double Shift, int i, bool RelativePosition)
        {
            if (RelativePosition)
            {
                return Position * (1 + (i + Offset) * Shift);
            }
            else
            {
                return Position + (i + Offset) * Shift;
            }
        }
    }

    /// <summary>
    /// Repeating Vertical lines with refernce at an X position
    /// </summary>
    public class RepeatingVLine : RepeatingAxisLine
    {
        /// <summary>
        /// X position to render the line
        /// </summary>
        public double X { get => Position; set => Position = value; }
        public int count { get => Count; set => Count = value; }

        public int offset { get => Offset; set => Offset = value; }

        public double shift { get => Shift; set => Shift = value; }
        public bool relativeposition { get => RelativePosition; set => RelativePosition = value; }
        public override string ToString() => $"{Count} equispaced lines starting at X={X}, with an offset of {Offset} and " + (RelativePosition ? "a relative" : "an absolute") + $"shift of {Shift}";
        public RepeatingVLine() : base(false) { }
    }

    /// <summary>
    /// Repeating horizontHorizontal line at an Y position
    /// </summary>
    public class RepeatingHLine : RepeatingAxisLine
    {
        /// <summary>
        /// Y position to render the line
        /// </summary>
        public double Y { get => Position; set => Position = value; }
        public int count { get => Count; set => Count = value; }

        public int offset { get => Offset; set => Offset = value; }

        public double shift { get => Shift; set => Shift = value; }

        public bool relativeposition { get => RelativePosition; set => RelativePosition = value; }
        public override string ToString() => $"{Count} equispaced lines starting at Y={Y}, with an offset of {Offset} and " + (RelativePosition ? "a relative" : "an absolute") + $"shift of {Shift}";
        public RepeatingHLine() : base(true) { }
    }

}
