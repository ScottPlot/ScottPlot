using ScottPlot.Drawing;
using System;
using System.Drawing;
using ScottPlot.SnapLogic;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Display a text label at an X/Y position in coordinate space
    /// </summary>
    public class Text : IPlottable, IHasPixelOffset, IDraggable, IHasColor
    {
        // data
        public double X;
        public double Y;
        public string Label;

        // customization
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public bool BackgroundFill = false;
        public Color BackgroundColor;
        public Drawing.Font Font = new Drawing.Font();
        public Color Color { get => Font.Color; set => Font.Color = value; }
        public string FontName { get => Font.Name; set => Font.Name = value; }
        public float FontSize { get => Font.Size; set => Font.Size = value; }
        public bool FontBold { get => Font.Bold; set => Font.Bold = value; }
        public Alignment Alignment { get => Font.Alignment; set => Font.Alignment = value; }
        public float Rotation { get => Font.Rotation; set => Font.Rotation = value; }
        public float BorderSize { get; set; } = 0;
        public Color BorderColor { get; set; } = Color.Black;
        public float PixelOffsetX { get; set; } = 0;
        public float PixelOffsetY { get; set; } = 0;
        RectangleF LastRenderRectangleCoordinates { get; set; }
        private double DeltaCX { get; set; } = 0;
        private double DeltaCY { get; set; } = 0;
        public LegendItem[] GetLegendItems() => LegendItem.None;
        public ISnap2D DragSnap { get; set; } = new NoSnap2D();

        public override string ToString() => $"PlottableText \"{Label}\" at ({X}, {Y})";
        public AxisLimits GetAxisLimits()
        {
            return new AxisLimits(X, X, Y, Y);
        }

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(X) || double.IsNaN(Y))
                throw new InvalidOperationException("X and Y cannot be NaN");

            if (double.IsInfinity(X) || double.IsInfinity(Y))
                throw new InvalidOperationException("X and Y cannot be Infinity");

            if (string.IsNullOrWhiteSpace(Label))
                throw new InvalidOperationException("text cannot be null or whitespace");
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (string.IsNullOrWhiteSpace(Label) || IsVisible == false)
                return;

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var font = GDI.Font(Font))
            using (var fontBrush = new SolidBrush(Font.Color))
            using (var frameBrush = new SolidBrush(BackgroundColor))
            using (var outlinePen = new Pen(BorderColor, BorderSize))
            using (var redPen = new Pen(Color.Red, BorderSize))
            {
                float pixelX = dims.GetPixelX(X) + PixelOffsetX;
                float pixelY = dims.GetPixelY(Y) - PixelOffsetY;
                SizeF stringSize = GDI.MeasureString(gfx, Label, font);

                gfx.TranslateTransform(pixelX, pixelY);
                gfx.RotateTransform(Font.Rotation);

                (float dX, float dY) = GDI.TranslateString(gfx, Label, Font);
                gfx.TranslateTransform(-dX, -dY);

                if (BackgroundFill)
                {
                    RectangleF stringRect = new(0, 0, stringSize.Width, stringSize.Height);
                    gfx.FillRectangle(frameBrush, stringRect);
                    if (BorderSize > 0)
                        gfx.DrawRectangle(outlinePen, stringRect.X, stringRect.Y, stringRect.Width, stringRect.Height);
                }

                gfx.DrawString(Label, font, fontBrush, new PointF(0, 0));

                GDI.ResetTransformPreservingScale(gfx, dims);

                double degangle = Font.Rotation * Math.PI / 180;
                float xA = pixelX - dX;
                float yA = pixelY - dY;
                float xC = xA + stringSize.Width * (float)Math.Cos(degangle) - stringSize.Height * (float)Math.Sin(degangle);
                float yC = yA + stringSize.Height * (float)Math.Cos(degangle) + stringSize.Width * (float)Math.Sin(degangle);

                PointF pointA = new(xA, yA);
                PointF pointC = new(xC, yC);

                LastRenderRectangleCoordinates = RectangleF.FromLTRB(
                    left: (float)dims.GetCoordinateX(pointA.X),
                    top: (float)dims.GetCoordinateY(pointC.Y),
                    right: (float)dims.GetCoordinateX(pointC.X),
                    bottom: (float)dims.GetCoordinateY(pointA.Y));
            }
        }

        /// <summary>
        /// Indicates whether this marker is draggable in user controls.
        /// </summary>
        public bool DragEnabled { get; set; } = false;

        /// <summary>
        /// Cursor to display while hovering over this marker if dragging is enabled.
        /// </summary>
        public Cursor DragCursor { get; set; } = Cursor.Hand;

        /// <summary>
        /// If dragging is enabled the marker cannot be dragged more negative than this position
        /// </summary>
        public double DragXLimitMin = double.NegativeInfinity;

        /// <summary>
        /// If dragging is enabled the marker cannot be dragged more positive than this position
        /// </summary>
        public double DragXLimitMax = double.PositiveInfinity;

        /// <summary>
        /// If dragging is enabled the marker cannot be dragged more negative than this position
        /// </summary>
        public double DragYLimitMin = double.NegativeInfinity;

        /// <summary>
        /// If dragging is enabled the marker cannot be dragged more positive than this position
        /// </summary>
        public double DragYLimitMax = double.PositiveInfinity;

        /// <summary>
        /// This event is invoked after the marker is dragged
        /// </summary>
        public event EventHandler Dragged = delegate { };

        /// <summary>
        /// Move the marker to a new coordinate in plot space.
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

            if (coordinateX < DragXLimitMin) coordinateX = DragXLimitMin;
            if (coordinateX > DragXLimitMax) coordinateX = DragXLimitMax;
            if (coordinateX < DragYLimitMin) coordinateY = DragYLimitMin;
            if (coordinateX > DragYLimitMax) coordinateY = DragYLimitMax;
            X = coordinateX + DeltaCX;
            Y = coordinateY + DeltaCY;
            Dragged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Return True if the marker is within a certain number of pixels (snap) to the mouse
        /// </summary>
        /// <param name="coordinateX">mouse position (coordinate space)</param>
        /// <param name="coordinateY">mouse position (coordinate space)</param>
        /// <param name="snapX">snap distance (pixels)</param>
        /// <param name="snapY">snap distance (pixels)</param>
        /// <returns></returns>
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            PointF pt = new((float)coordinateX, (float)coordinateY);

            if (IsPointInsideRectangle(pt, LastRenderRectangleCoordinates))
            {
                DeltaCX = X - coordinateX;
                DeltaCY = Y - coordinateY;
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsPointInsideRectangle(PointF pt, RectangleF rect)
        {
            // https://swharden.com/blog/2022-02-01-point-in-rectangle/

            double x1 = rect.Left;
            double x2 = rect.Right;
            double x3 = rect.Right;
            double x4 = rect.Left;

            double y1 = rect.Top;
            double y2 = rect.Top;
            double y3 = rect.Bottom;
            double y4 = rect.Bottom;

            double a1 = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            double a2 = Math.Sqrt((x2 - x3) * (x2 - x3) + (y2 - y3) * (y2 - y3));
            double a3 = Math.Sqrt((x3 - x4) * (x3 - x4) + (y3 - y4) * (y3 - y4));
            double a4 = Math.Sqrt((x4 - x1) * (x4 - x1) + (y4 - y1) * (y4 - y1));

            double b1 = Math.Sqrt((x1 - pt.X) * (x1 - pt.X) + (y1 - pt.Y) * (y1 - pt.Y));
            double b2 = Math.Sqrt((x2 - pt.X) * (x2 - pt.X) + (y2 - pt.Y) * (y2 - pt.Y));
            double b3 = Math.Sqrt((x3 - pt.X) * (x3 - pt.X) + (y3 - pt.Y) * (y3 - pt.Y));
            double b4 = Math.Sqrt((x4 - pt.X) * (x4 - pt.X) + (y4 - pt.Y) * (y4 - pt.Y));

            double u1 = (a1 + b1 + b2) / 2;
            double u2 = (a2 + b2 + b3) / 2;
            double u3 = (a3 + b3 + b4) / 2;
            double u4 = (a4 + b4 + b1) / 2;

            double A1 = Math.Sqrt(u1 * (u1 - a1) * (u1 - b1) * (u1 - b2));
            double A2 = Math.Sqrt(u2 * (u2 - a2) * (u2 - b2) * (u2 - b3));
            double A3 = Math.Sqrt(u3 * (u3 - a3) * (u3 - b3) * (u3 - b4));
            double A4 = Math.Sqrt(u4 * (u4 - a4) * (u4 - b4) * (u4 - b1));

            double difference = A1 + A2 + A3 + A4 - a1 * a2;
            return difference < .001 * (rect.Height + rect.Width);
        }
    }
}
