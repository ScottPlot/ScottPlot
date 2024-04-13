using ScottPlot.Drawing;
using System;
using System.Linq;
using System.Drawing;
using ScottPlot.SnapLogic;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// This plot type is essentially the same as an Axis line, but it contains an array of positions.
    /// All lines are styled the same, but they can be positioned (and dragged) independently.
    /// </summary>
    public abstract class AxisLineVector : IPlottable, IDraggable, IHasLine, IHasColor
    {
        /// <summary>
        /// Location of the line (Y position if horizontal line, X position if vertical line)
        /// </summary>
        protected double[] Positions;
        public int CurrentIndex { get; set; } = 0;

        /// <summary>
        /// Add this value to each datapoint value before plotting (axis units)
        /// </summary>
        protected int Offset { get; set; } = 0;

        public int PointCount => Positions.Length;

        /// <summary>
        /// If True, the position will be labeled on the axis using the PositionFormatter
        /// </summary>
        public bool PositionLabel { get; set; } = false;

        /// <summary>
        /// Font to use for position labels (labels drawn over the axis)
        /// </summary>
        //public Drawing.Font PositionLabelFont = new(){ Color = Color.White, Bold = true };
        public readonly ScottPlot.Drawing.Font PositionLabelFont = new() { Color = Color.White, Bold = true };

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

        // customization
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public Color Color { get; set; } = Color.Black;
        public Color LineColor { get => Color; set { Color = value; } }
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;
        public MarkerShape MarkerShape { get; set; } = MarkerShape.filledCircle;
        public double LineWidth { get; set; } = 1;
        public float ErrorLineWidth { get; set; } = 1;
        public float ErrorCapSize { get; set; } = 3;
        public float MarkerSize { get; set; } = 5;
        public bool StepDisplay { get; set; } = false;

        /// <summary>
        /// Indicates whether the line is horizontal (position in Y units) or vertical (position in X units)
        /// </summary>
        private readonly bool IsHorizontal;

        /// <summary>
        /// If true, AxisAuto() will ignore the position of this line when determining axis limits
        /// </summary>
        public bool IgnoreAxisAuto { get; set; } = false;

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

        public AxisLineVector(bool isHorizontal)
        {
            IsHorizontal = isHorizontal;
        }

        public AxisLimits GetAxisLimits()
        {
            if (IgnoreAxisAuto)
                return AxisLimits.NoLimits;

            if (IsHorizontal)
            {
                double xMin = double.IsNegativeInfinity(Min) ? double.NaN : Min;
                double xMax = double.IsPositiveInfinity(Max) ? double.NaN : Max;
                return new AxisLimits(xMin, xMax, Positions.Min(), Positions.Max());
            }
            else
            {
                double yMin = double.IsNegativeInfinity(Min) ? double.NaN : Min;
                double yMax = double.IsPositiveInfinity(Max) ? double.NaN : Max;
                return new AxisLimits(Positions.Min(), Positions.Max(), yMin, yMax);
            }
        }

        public void ValidateData(bool deep = false)
        {
            Validate.AssertHasElements("Positions", Positions);
            if (deep)
            {
                Validate.AssertAllReal("Positions", Positions);
            }
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
            var pen = GDI.Pen(Color, LineWidth, LineStyle, true);

            if (IsHorizontal)
            {
                for (int i = 0; i < PointCount; i++)
                {
                    float pixelY = dims.GetPixelY(Positions[i] + Offset);

                    double xMin = Math.Max(Min, dims.XMin);
                    double xMax = Math.Min(Max, dims.XMax);
                    float pixelX1 = dims.GetPixelX(xMin);
                    float pixelX2 = dims.GetPixelX(xMax);

                    gfx.DrawLine(pen, pixelX1, pixelY, pixelX2, pixelY);

                }
            }
            else
            {
                for (int i = 0; i < PointCount; i++)
                {
                    float pixelX = dims.GetPixelX(Positions[i] + Offset);

                    double yMin = Math.Max(Min, dims.YMin);
                    double yMax = Math.Min(Max, dims.YMax);
                    float pixelY1 = dims.GetPixelY(yMin);
                    float pixelY2 = dims.GetPixelY(yMax);

                    gfx.DrawLine(pen, pixelX, pixelY1, pixelX, pixelY2);

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
                for (int i = 0; i < PointCount; i++)
                {
                    double lineposition = Positions[i] + Offset;
                    if (lineposition > dims.YMax || lineposition < dims.YMin)
                    { }
                    else
                    {

                        float pixelY = dims.GetPixelY(lineposition);
                        string yLabel = PositionFormatter(lineposition);
                        SizeF yLabelSize = GDI.MeasureString(gfx, yLabel, PositionLabelFont);
                        float xPos = PositionLabelOppositeAxis ? dims.DataOffsetX + dims.DataWidth : dims.DataOffsetX - yLabelSize.Width;
                        float yPos = pixelY - yLabelSize.Height / 2;
                        RectangleF xLabelRect = new RectangleF(xPos, yPos, yLabelSize.Width, yLabelSize.Height);
                        gfx.FillRectangle(fillBrush, xLabelRect);
                        var sf = GDI.StringFormat(HorizontalAlignment.Left, VerticalAlignment.Middle);
                        gfx.DrawString(yLabel, fnt, fontBrush, xPos, pixelY, sf);
                    }
                }
            }
            else
            {
                for (int i = 0; i < PointCount; i++)
                {
                    double lineposition = Positions[i] + Offset;
                    if (lineposition > dims.XMax || lineposition < dims.XMin)
                    { }
                    else
                    {

                        float pixelX = dims.GetPixelX(lineposition);
                        string xLabel = PositionFormatter(lineposition);
                        SizeF xLabelSize = GDI.MeasureString(gfx, xLabel, PositionLabelFont);
                        float xPos = pixelX - xLabelSize.Width / 2;
                        float yPos = PositionLabelOppositeAxis ? dims.DataOffsetY - xLabelSize.Height : dims.DataOffsetY + dims.DataHeight;
                        RectangleF xLabelRect = new RectangleF(xPos, yPos, xLabelSize.Width, xLabelSize.Height);
                        gfx.FillRectangle(fillBrush, xLabelRect);
                        var sf = GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Upper);
                        gfx.DrawString(xLabel, fnt, fontBrush, pixelX, yPos, sf);
                    }
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
                Positions[CurrentIndex] = coordinateY;
            }
            else
            {
                if (coordinateX < DragLimitMin) coordinateX = DragLimitMin;
                if (coordinateX > DragLimitMax) coordinateX = DragLimitMax;
                Positions[CurrentIndex] = coordinateX;
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
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            if (IsHorizontal)
            {
                for (int i = 0; i < PointCount; i++)
                {
                    if (Math.Abs(Positions[i] - coordinateY) <= snapY)
                    {
                        CurrentIndex = i;
                        return true;
                    }
                };
            }
            else
            {
                for (int i = 0; i < PointCount; i++)
                {
                    if (Math.Abs(Positions[i] - coordinateX) <= snapX)
                    {
                        CurrentIndex = i;
                        return true;
                    }
                }
            }
            return false;
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
            return new LegendItem[] { singleItem };
        }
    }

    /// <summary>
    /// Vertical line at an X position
    /// </summary>
    public class VLineVector : AxisLineVector
    {
        /// <summary>
        /// X position to render the line
        /// </summary>
        public double[] Xs { get => Positions; set => Positions = value; }

        public int offset { get => Offset; set => Offset = value; }

        public override string ToString() => $"{PointCount} lines positions X={Xs}, with an offset of {Offset}";
        public VLineVector() : base(false) { }
    }

    /// <summary>
    /// Horizontal line at an Y position
    /// </summary>
    public class HLineVector : AxisLineVector
    {
        /// <summary>
        /// Y position to render the line
        /// </summary>
        public double[] Ys { get => Positions; set => Positions = value; }

        public int offset { get => Offset; set => Offset = value; }

        public override string ToString() => $"{PointCount} lines positions Y={Ys}, with an offset of {Offset}";
        public HLineVector() : base(true) { }
    }
}
