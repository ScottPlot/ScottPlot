using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// This variation of the Heatmap renders intensity data as a rectangle 
    /// sized to fit user-defined axis limits
    /// </summary>
    public class CoordinatedHeatmap : Heatmap, IDraggableModern
    {
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
        public InterpolationMode Interpolation { get; set; } = InterpolationMode.NearestNeighbor;
        public bool DragEnabled { get; set; } = true;

        public Cursor DragCursor => Cursor.Hand;

        public event EventHandler Dragged = delegate { };

        protected override void RenderHeatmap(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            {
                gfx.InterpolationMode = Interpolation;
                gfx.PixelOffsetMode = PixelOffsetMode.Half;

                int drawFromX = (int)Math.Round(dims.GetPixelX(XMin));
                int drawFromY = (int)Math.Round(dims.GetPixelY(YMax));
                int drawWidth = (int)Math.Round(dims.GetPixelX(XMax) - drawFromX);
                int drawHeight = (int)Math.Round(dims.GetPixelY(YMin) - drawFromY);
                Rectangle destRect = new Rectangle(drawFromX, drawFromY, drawWidth, drawHeight);
                ImageAttributes attr = new ImageAttributes();
                attr.SetWrapMode(WrapMode.TileFlipXY);

                if (BackgroundImage != null && !DisplayImageAbove)
                    gfx.DrawImage(BackgroundImage, destRect, 0, 0, BackgroundImage.Width, BackgroundImage.Height, GraphicsUnit.Pixel, attr);

                gfx.DrawImage(BmpHeatmap, destRect, 0, 0, BmpHeatmap.Width, BmpHeatmap.Height, GraphicsUnit.Pixel, attr);

                if (BackgroundImage != null && DisplayImageAbove)
                    gfx.DrawImage(BackgroundImage, destRect, 0, 0, BackgroundImage.Width, BackgroundImage.Height, GraphicsUnit.Pixel, attr);
            }
        }

        public override AxisLimits GetAxisLimits()
        {
            return new AxisLimits(XMin, XMax, YMin, YMax);
        }

        /// <summary>
        /// Move the CoordinatedHeatmap to a new coordinate in plot space.
        /// </summary>
        /// <param name="coordinateXFrom">Move signal from X coordinate</param>
        /// <param name="coordinateXTo">Move signal from Y coordinate</param>
        /// <param name="CoordinateYFrom">Move signal to X coordinate</param>
        /// <param name="coordinateYTo">Move Signal to Y coordinate</param>
        /// <param name="fixedSize">Unused flag</param>
        public void Drag(double coordinateXFrom, double coordinateXTo, double coordinateYFrom, double coordinateYTo, bool fixedSize)
        {
            var offsetX = coordinateXTo - coordinateXFrom;
            var offsetY = coordinateYTo - coordinateYFrom;
            XMin += offsetX;
            XMax += offsetX;
            YMin += offsetY;
            YMax += offsetY;
            Dragged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Return True if either CoordinatedHeatmap is within a certain number of pixels (snap) to the mouse
        /// </summary>
        /// <param name="coordinateX">mouse position X (coordinate space)</param>
        /// <param name="coordinateY">mouse position Y(coordinate space)</param>
        /// <param name="snapX">snap distance X axes (coordinate space)</param>
        /// <param name="snapY">snap distance X axes (coordinate space)</param>
        /// <returns>True if signal is within a mouse</returns>
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            return (coordinateX >= (XMin - snapX))
                && (coordinateX <= (XMax + snapX))
                && (coordinateY >= (YMin - snapY))
                && (coordinateY <= (YMax + snapY));
        }

        /// <summary>
        /// Never called 
        /// </summary>
        /// <param name="coordinateX"></param>
        /// <param name="coordinateY"></param>
        /// <param name="fixedSize"></param>
        public void DragTo(double coordinateX, double coordinateY, bool fixedSize)
        {
            throw new NotImplementedException();
        }
    }
}
