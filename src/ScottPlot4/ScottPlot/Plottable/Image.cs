using System.ComponentModel;
using System.Drawing;
using ScottPlot.Drawing;
using System;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Display a Bitmap at X/Y coordinates in unit space
    /// </summary>
    public class Image : IPlottable
    {
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Position of the primary corner (based on Alignment)
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Position of the primary corner (based on Alignment)
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// If defined, the image will be stretched to be this wide in axis units.
        /// If null, the image will use screen/pixel units.
        /// </summary>
        public double? WidthInAxisUnits { get; set; } = null;

        /// <summary>
        /// If defined, the image will be stretched to be this height in axis units.
        /// If null, the image will use screen/pixel units.
        /// </summary>
        public double? HeightInAxisUnits { get; set; } = null;

        /// <summary>
        /// Multiply the size of the image (in pixel units) by this scale factor.
        /// The primary corner (based on Alignment) will remain anchored.
        /// </summary>
        public double Scale = 1.0;

        /// <summary>
        /// Rotate the image clockwise around its primary corner (defined by Alignment) by this number of degrees
        /// </summary>
        public double Rotation { get; set; }

        /// <summary>
        /// Image to display
        /// </summary>
        public System.Drawing.Image Bitmap { get; set; }

        /// <summary>
        /// Indicates which corner of the Bitmap is described by X and Y.
        /// This corner will be the axis of Rotation, and the center of Scale.
        /// </summary>
        public Alignment Alignment { get; set; }

        public Color BorderColor { get; set; }

        /// <summary>
        /// Line width of the border (in pixels)
        /// </summary>
        public float BorderSize { get; set; }

        public string Label { get; set; }

        public int XAxisIndex { get; set; } = 0;

        public int YAxisIndex { get; set; } = 0;

        public Coordinate[] ClippingPoints { get; set; } = Array.Empty<Coordinate>();

        public AxisLimits GetAxisLimits()
        {
            if (Bitmap is null)
                return AxisLimits.NoLimits;

            return new AxisLimits(
                xMin: X,
                xMax: X + WidthInAxisUnits ?? 0,
                yMin: Y - HeightInAxisUnits ?? 0,
                yMax: Y);
        }

        public LegendItem[] GetLegendItems() => LegendItem.None;

        public override string ToString()
        {
            string w = (WidthInAxisUnits is null) ? $"{Bitmap.Width} pixels" : $"{WidthInAxisUnits} axis units";
            string h = (HeightInAxisUnits is null) ? $"{Bitmap.Height} pixels" : $"{HeightInAxisUnits} axis units";
            return $"Image at ({X}, {Y}), Width = {w}, Height = {h}";
        }

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(X) || double.IsInfinity(X))
                throw new InvalidOperationException("x must be a real value");

            if (double.IsNaN(Y) || double.IsInfinity(Y))
                throw new InvalidOperationException("y must be a real value");

            if (WidthInAxisUnits is double axisWidth)
                if (double.IsNaN(axisWidth) || double.IsInfinity(axisWidth))
                    throw new InvalidOperationException("width must be a real value");

            if (HeightInAxisUnits is double axisHeight)
                if (double.IsNaN(axisHeight) || double.IsInfinity(axisHeight))
                    throw new InvalidOperationException("height must be a real value");

            if (double.IsNaN(Scale) || double.IsInfinity(Scale))
                throw new InvalidOperationException("scale must be a real value");

            if (double.IsNaN(Rotation) || double.IsInfinity(Rotation))
                throw new InvalidOperationException("rotation must be a real value");

            if (Bitmap is null)
                throw new InvalidOperationException("image cannot be null");
        }

        private PointF ImageLocationOffset(float width, float height)
        {
            return Alignment switch
            {
                Alignment.LowerCenter => new PointF(-width / 2, -height),
                Alignment.LowerLeft => new PointF(0, -height),
                Alignment.LowerRight => new PointF(-width, -height),
                Alignment.MiddleLeft => new PointF(0, -height / 2),
                Alignment.MiddleRight => new PointF(-width, -height / 2),
                Alignment.UpperCenter => new PointF(-width / 2, 0),
                Alignment.UpperLeft => new PointF(0, 0),
                Alignment.UpperRight => new PointF(-width, 0),
                Alignment.MiddleCenter => new PointF(-width / 2, -height / 2),
                _ => throw new InvalidEnumArgumentException(),
            };
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            PointF defaultPoint = new(dims.GetPixelX(X), dims.GetPixelY(Y));

            float width, height;

            if (WidthInAxisUnits is double axisWidth)
                width = dims.GetPixelX(X + axisWidth) - defaultPoint.X;
            else
                width = Bitmap.Width;

            if (HeightInAxisUnits is double axisHeight)
                height = dims.GetPixelY(Y - axisHeight) - defaultPoint.Y;
            else
                height = Bitmap.Height;

            width = (float)(width * Scale);
            height = (float)(height * Scale);

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var framePen = new Pen(BorderColor, BorderSize * 2))
            {
                GDI.ClipIntersection(gfx, dims, ClippingPoints);

                gfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                gfx.TranslateTransform(defaultPoint.X, defaultPoint.Y);
                gfx.RotateTransform((float)Rotation);

                RectangleF rect = new(ImageLocationOffset(width, height), new SizeF(width, height));

                if (BorderSize > 0)
                    gfx.DrawRectangle(framePen, Math.Min(rect.X, rect.Right), Math.Min(rect.Y, rect.Bottom), Math.Abs(rect.Width) - 1, Math.Abs(rect.Height) - 1);

                gfx.DrawImage(Bitmap, rect);
            }
        }
    }
}
