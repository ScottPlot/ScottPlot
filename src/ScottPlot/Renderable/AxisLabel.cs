using ScottPlot.Drawing;
using System;
using System.Drawing;

namespace ScottPlot.Renderable
{
    public class AxisLabel : IRenderable
    {
        /// <summary>
        /// Controls whether this axis occupies space and is displayed
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Edge of the data area this axis represents
        /// </summary>
        public Edge Edge;

        /// <summary>
        /// Axis title
        /// </summary>
        public string Label = null;

        /// <summary>
        /// Font options for the axis title
        /// </summary>
        public Drawing.Font Font = new Drawing.Font() { Size = 16 };

        /// <summary>
        /// Set this field to display a bitmap instead of a text axis label
        /// </summary>
        public Bitmap ImageLabel = null;

        /// <summary>
        /// Padding (in pixels) around the image label
        /// </summary>
        public float ImagePadding = 5;

        /// <summary>
        /// Amount of padding (in pixels) to surround the contents of this axis
        /// </summary>
        public float PixelSizePadding;

        /// <summary>
        /// Distance to offset this axis to account for multiple axes
        /// </summary>
        public float PixelOffset;

        /// <summary>
        /// Exact size (in pixels) of the contents of this this axis
        /// </summary>
        public float PixelSize;

        /// <summary>
        /// Return the size of the contents of this axis.
        /// Returned dimensions are screen-accurate (even if this axis is rotated).
        /// </summary>
        /// <returns></returns>
        public SizeF Measure()
        {
            if (ImageLabel != null)
            {
                bool ImageIsRotated = (Edge == Edge.Left || Edge == Edge.Right);
                float width = ImageIsRotated ? ImageLabel.Height : ImageLabel.Width;
                float height = ImageIsRotated ? ImageLabel.Width : ImageLabel.Height;
                return new SizeF(width + ImagePadding, height + ImagePadding);
            }
            else
            {
                return GDI.MeasureString(Label, Font);
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false || (string.IsNullOrWhiteSpace(Label) && ImageLabel == null))
                return;

            using var gfx = GDI.Graphics(bmp, lowQuality);
            (float x, float y, int rotation) = GetAxisCenter(dims);

            if (ImageLabel is null)
                RenderTextLabel(gfx, x, y, rotation);
            else
                RenderImageLabel(gfx, x, y, rotation);
        }

        private void RenderImageLabel(Graphics gfx, float x, float y, int rotation)
        {
            // TODO: use ImagePadding instead of fractional padding

            float xOffset = Edge switch
            {
                Edge.Left => 0,
                Edge.Right => -3 * ImageLabel.Width / 2,
                Edge.Bottom => -ImageLabel.Width / 2,
                Edge.Top => -ImageLabel.Width / 2,
                _ => throw new NotImplementedException()
            };

            float yOffset = Edge switch
            {
                Edge.Left => -ImageLabel.Height / 2,
                Edge.Right => -ImageLabel.Height / 2,
                Edge.Bottom => -3 * ImageLabel.Height / 2,
                Edge.Top => ImageLabel.Height / 4,
                _ => throw new NotImplementedException()
            };

            gfx.TranslateTransform(x, y);
            gfx.DrawImage(ImageLabel, xOffset, yOffset);
            gfx.ResetTransform();
        }

        private void RenderTextLabel(Graphics gfx, float x, float y, int rotation)
        {
            // TODO: should padding be inverted if "bottom or right"?
            float padding = (Edge == Edge.Bottom) ? -PixelSizePadding : PixelSizePadding;

            using var font = GDI.Font(Font);
            using var brush = GDI.Brush(Font.Color);
            using var sf = GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Lower);
            sf.LineAlignment = Edge switch
            {
                Edge.Left => StringAlignment.Near,
                Edge.Right => StringAlignment.Near,
                Edge.Bottom => StringAlignment.Far,
                Edge.Top => StringAlignment.Near,
                _ => throw new NotImplementedException()
            };

            gfx.TranslateTransform(x, y);
            gfx.RotateTransform(rotation);
            gfx.DrawString(Label, font, brush, 0, padding, sf);
            gfx.ResetTransform();
        }

        /// <summary>
        /// Return the point and rotation representing the center of the base of this axis
        /// </summary>
        private (float x, float y, int rotation) GetAxisCenter(PlotDimensions dims)
        {
            int rotation = Edge switch
            {
                Edge.Left => -90,
                Edge.Right => 90,
                Edge.Bottom => 0,
                Edge.Top => 0,
                _ => throw new NotImplementedException()
            };

            float x = Edge switch
            {
                Edge.Left => dims.DataOffsetX - PixelOffset - PixelSize,
                Edge.Right => dims.DataOffsetX + dims.DataWidth + PixelOffset + PixelSize,
                Edge.Bottom => dims.DataOffsetX + dims.DataWidth / 2,
                Edge.Top => dims.DataOffsetX + dims.DataWidth / 2,
                _ => throw new NotImplementedException()
            };

            float y = Edge switch
            {
                Edge.Left => dims.DataOffsetY + dims.DataHeight / 2,
                Edge.Right => dims.DataOffsetY + dims.DataHeight / 2,
                Edge.Bottom => dims.DataOffsetY + dims.DataHeight + PixelOffset + PixelSize,
                Edge.Top => dims.DataOffsetY - PixelOffset - PixelSize,
                _ => throw new NotImplementedException()
            };

            return (x, y, rotation);
        }
    }
}
