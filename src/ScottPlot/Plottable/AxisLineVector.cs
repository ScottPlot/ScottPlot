using ScottPlot;
using ScottPlot.Plottable;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace live_sandbox
{
    public abstract class AxisLineVector : IPlottable
    {
        /// <summary>
        /// Location of the line (Y position if horizontal line, X position if vertical line)
        /// </summary>
        protected double[] Positions;

        /// <summary>
        /// Add this value to each datapoint value before plotting (axis units)
        /// </summary>
        protected int Offset = 0;

        public int PointCount => Positions.Length;

        /// <summary>
        /// If True, the position will be labeled on the axis using the PositionFormatter
        /// </summary>
        public bool PositionLabel = false;

        /// <summary>
        /// Font to use for position labels (labels drawn over the axis)
        /// </summary>
        //public Drawing.Font PositionLabelFont = new(){ Color = Color.White, Bold = true };
        public ScottPlot.Drawing.Font PositionLabelFont = new ScottPlot.Drawing.Font() { Color = Color.White, Bold = true };

        /// <summary>
        /// Color to use behind the position labels
        /// </summary>
        public Color PositionLabelBackground = Color.Black;

        /// <summary>
        /// If true the position label will be drawn on the right or top of the data area.
        /// </summary>
        public bool PositionLabelOppositeAxis = false;

        /// <summary>
        /// This method generates the position label text for numeric (non-DateTime) axes.
        /// For DateTime axes assign your own format string that uses DateTime.FromOADate(position).
        /// </summary>
        public Func<double, string> PositionFormatter = position => position.ToString("F2");

        // customization
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public Color Color = Color.Black;
        public LineStyle LineStyle = LineStyle.Solid;
        public MarkerShape MarkerShape = MarkerShape.filledCircle;
        public double LineWidth = 1;
        public float ErrorLineWidth = 1;
        public float ErrorCapSize = 3;
        public float MarkerSize = 5;
        public bool StepDisplay = false;

        /// <summary>
        /// Indicates whether the line is horizontal (position in Y units) or vertical (position in X units)
        /// </summary>
        private readonly bool IsHorizontal;

        /// <summary>
        /// If true, AxisAuto() will ignore the position of this line when determining axis limits
        /// </summary>
        public bool IgnoreAxisAuto = false;

        /// <summary>
        /// Text that appears in the legend
        /// </summary>
        public string Label = string.Empty;

        /// <summary>
        /// The lower bound of the axis line.
        /// </summary>
        public double Min = double.NegativeInfinity;

        /// <summary>
        /// The upper bound of the axis line.
        /// </summary>
        public double Max = double.PositiveInfinity;

        public AxisLineVector(bool isHorizontal)
        {
            IsHorizontal = isHorizontal;
        }

        public AxisLimits GetAxisLimits()
        {
            if (IgnoreAxisAuto)
                return new AxisLimits(double.NaN, double.NaN, double.NaN, double.NaN);

            if (IsHorizontal)
                return new AxisLimits(double.NaN, double.NaN, Positions.Min(), Positions.Max());
            else
                return new AxisLimits(Positions.Min(), Positions.Max(), double.NaN, double.NaN);
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
                        SizeF yLabelSize = GDI.MeasureString(yLabel, PositionLabelFont);
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
                        SizeF xLabelSize = GDI.MeasureString(xLabel, PositionLabelFont);
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
