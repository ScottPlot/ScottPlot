using ScottPlot.Drawing;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Display a text label at an X/Y position in coordinate space
    /// </summary>
    public class Text : IPlottable, IHasPixelOffset, IDraggable
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
        public Color Color { set => Font.Color = value; }
        public string FontName { set => Font.Name = value; }
        public float FontSize { set => Font.Size = value; }
        public bool FontBold { set => Font.Bold = value; }
        public Alignment Alignment { set => Font.Alignment = value; }
        public float Rotation { set => Font.Rotation = value; }
        public float BorderSize { get; set; } = 0;
        public Color BorderColor { get; set; } = Color.Black;
        public float PixelOffsetX { get; set; } = 0;
        public float PixelOffsetY { get; set; } = 0;
        RectangleF RectangleUnits;
        private double DeltaCX = 0;
        private double DeltaCY = 0;

        public override string ToString() => $"PlottableText \"{Label}\" at ({X}, {Y})";
        public AxisLimits GetAxisLimits() => new AxisLimits(X, X, Y, Y);
        public LegendItem[] GetLegendItems() => null;

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

                (float dX, float dY) = GDI.TranslateString(Label, Font);
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

                gfx.DrawEllipse(redPen, xA, yA, 2, 2);
                gfx.DrawEllipse(redPen, xC, yC, 2, 2);

                gfx.DrawString("A", font, fontBrush, pointA);
                gfx.DrawString("C", font, fontBrush, pointC);

                RectangleUnits = RectangleF.FromLTRB((float)dims.GetCoordinateX(xA), (float)dims.GetCoordinateY(yA), (float)dims.GetCoordinateX(xC), (float)dims.GetCoordinateY(yC));
            }
        }

        /// <summary>
        /// Indicates whether this marker is draggable in user controls.
        /// </summary>
        public bool DragEnabled { get; set; } = true;

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
            bool test = RectangleUnits.Contains((float)coordinateX, (float)coordinateY);
            if (test)
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
    }
}
