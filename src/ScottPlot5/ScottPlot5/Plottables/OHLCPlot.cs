using ScottPlot.Axis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottables
{
    public class OHLCPlot : IPlottable
    {
        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = Axis.Axes.Default;
        public readonly DataSources.IOHLCSource Data;
        
        public LineStyle GrowingStyle { get; } = new() { Color = Colors.LightGreen, Width = 2 };
        public LineStyle FallingStyle { get; } = new() { Color = Colors.LightCoral, Width = 2 };

        /// <summary>
        /// The number of pixels taken up by each wick
        /// </summary>
        public int Width { get; set; } = 10;

        public OHLCPlot(DataSources.IOHLCSource data)
        {
            Data = data;
        }

        public IEnumerable<LegendItem> LegendItems => Enumerable.Empty<LegendItem>();

        public AxisLimits GetAxisLimits() => Data.GetLimits();

        public void Render(SKSurface surface)
        {
            using SKPaint paint = new();
            using SKPath path = new();

            foreach ((double x, OHLC y) in Data.GetOHLCPoints())
            {
                LineStyle lineStyle = y.Close >= y.Open ? GrowingStyle : FallingStyle;
                lineStyle.ApplyToPaint(paint);

                float center = Axes.GetPixelX(x);
                float top = Axes.GetPixelY(y.High);
                float bottom = Axes.GetPixelY(y.Low);

                path.MoveTo(center, top);
                path.LineTo(center, bottom);

                float open = Axes.GetPixelY(y.Open);
                float close = Axes.GetPixelY(y.Close);

                path.MoveTo(center - Width / 2, open);
                path.LineTo(center, open);

                path.MoveTo(center, close);
                path.LineTo(center + Width / 2, close);

                surface.Canvas.DrawPath(path, paint);
                path.Reset();
            }
        }
    }
}
