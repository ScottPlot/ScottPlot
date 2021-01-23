using ScottPlot.Drawing;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ScottPlot.Plottable
{
    public class CoordinatedHeatmap : Heatmap
    {
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
        public InterpolationMode Interpolation { get; set; } = InterpolationMode.NearestNeighbor;

        protected override void RenderHeatmap(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            {
                gfx.InterpolationMode = Interpolation;

                float drawFrom = dims.GetPixelX(XMin);
                float drawTo = dims.GetPixelY(YMax);
                float drawWidth = (float)(dims.PxPerUnitX * (XMax - XMin));
                float drawHeight = (float)(dims.PxPerUnitY * (YMax - YMin));

                if (BackgroundImage != null && !DisplayImageAbove)
                    gfx.DrawImage(BackgroundImage, drawFrom, drawTo, drawWidth, drawHeight);

                gfx.DrawImage(BmpHeatmap, drawFrom, drawTo, drawWidth, drawHeight);

                if (BackgroundImage != null && DisplayImageAbove)
                    gfx.DrawImage(BackgroundImage, drawFrom, drawTo, drawWidth, drawHeight);
            }
        }

        public override AxisLimits GetAxisLimits()
        {
            return new AxisLimits(XMin, XMax, YMin, YMax);
        }
    }
}
