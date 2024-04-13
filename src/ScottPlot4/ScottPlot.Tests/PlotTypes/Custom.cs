using NUnit.Framework;
using ScottPlot.Plottable;
using ScottPlotTests;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Tests.PlotTypes;

internal class Custom
{
    class RainbowPlot : IPlottable
    {
        // The constructor populates the data
        double[] Xs { get; }
        double[] Ys { get; }
        Color[] Colors { get; }

        public RainbowPlot(double[] xs, double[] ys)
        {
            Xs = xs;
            Ys = ys;
            Colors = new Color[Xs.Length];

            Color[] rainbowColors = { Color.Red, Color.Orange, Color.Yellow,
                Color.Green, Color.Blue, Color.Indigo, Color.Violet };

            for (int i = 0; i < Xs.Length; i++)
                Colors[i] = rainbowColors[i % rainbowColors.Length];
        }

        // Properties can be exposed to facilitate advanced customization
        public float Radius { get; set; } = 10;

        // The following are requireiements of IPlottable
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public LegendItem[] GetLegendItems() => Array.Empty<LegendItem>(); // nothing for simplicity
        public void ValidateData(bool deep = false) { } // optional
        public AxisLimits GetAxisLimits() => new(Xs.Min(), Xs.Max(), Ys.Min(), Ys.Max());

        // This method describes how to plot the data on the cart.
        public void Render(PlotDimensions dims, System.Drawing.Bitmap bmp, bool lowQuality = false)
        {
            // Use ScottPlot's GDI helper functions to create System.Drawing objects
            using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
            using var brush = (SolidBrush)ScottPlot.Drawing.GDI.Brush(Color.Black);

            // Render data by drawing on the Graphics object
            for (int i = 0; i < Xs.Length; i++)
            {
                // Use 'dims' methods to convert between axis coordinates and pixel positions
                float xPixel = dims.GetPixelX(Xs[i]);
                float yPixel = dims.GetPixelY(Ys[i]);

                brush.Color = Colors[i];
                gfx.FillEllipse(brush,
                    x: xPixel - Radius,
                    y: yPixel - Radius,
                    width: Radius * 2,
                    height: Radius * 2);
            }
        }
    }

    [Test]
    public void Test_Custom_PlotType()
    {
        // this code is used to create the FAQ page
        // https://github.com/ScottPlot/ScottPlot.NET/issues/10

        double[] xs = ScottPlot.Generate.Consecutive(51);
        double[] ys = ScottPlot.Generate.Sin(51);
        RainbowPlot myPlottable = new(xs, ys);

        ScottPlot.Plot plt = new(400, 300);
        plt.Add(myPlottable);
        TestTools.SaveFig(plt);
    }
}
