using System;

namespace ScottPlotTests
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class MeanPixel
    {
        public readonly double A, R, G, B;
        public double RGB { get => (R + G + B) / 3; }
        public readonly int pixels;

        public MeanPixel(ScottPlot.Plot plt, bool lowQuality = true)
        {
            var bmp = plt.Render(lowQuality);
            (A, R, G, B) = TestTools.MeanPixel(bmp);
            pixels = bmp.Width * bmp.Height;
        }

        public MeanPixel(System.Drawing.Bitmap bmp)
        {
            (A, R, G, B) = TestTools.MeanPixel(bmp);
            pixels = bmp.Width * bmp.Height;
        }

        public override string ToString()
        {
            string[] parts = { "R", "=", "G", "=", "B", "=", "R" };
            if (R < G) parts[1] = "<";
            if (R > G) parts[1] = ">";
            if (G < B) parts[3] = "<";
            if (G > B) parts[3] = ">";
            if (B < R) parts[5] = "<";
            if (B > R) parts[5] = ">";
            string comparisons = string.Join("", parts);
            string rgbVals = $"R={Math.Round(R, 3)}, G={Math.Round(G, 3)}, B={Math.Round(B, 3)}";
            return $"{pixels} pixels: {rgbVals} ({comparisons}) total={Math.Round(RGB, 3)}";
        }

        public override bool Equals(object obj)
        {
            if (obj is MeanPixel comparison)
            {
                return (R == comparison.R) && (G == comparison.G) && (B == comparison.B);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public bool IsEqualTo(System.Drawing.Bitmap bmp)
        {
            (_, double r, double g, double b) = TestTools.MeanPixel(bmp);
            return (R == r) && (G == g) && (B == b);
        }

        public bool IsDifferentThan(MeanPixel comparison) => RGB != comparison.RGB;

        public bool IsDarkerThan(MeanPixel comparison) => RGB < comparison.RGB;

        public bool IsLighterThan(MeanPixel comparison) => RGB > comparison.RGB;

        public bool IsEqualTo(MeanPixel comparison) => RGB == comparison.RGB;

        public bool IsGray() => (R == G) && (G == B) && (B == R);

        public bool IsNotGray() => !IsGray();

        public bool IsMoreBlueThan(MeanPixel comparison) => B > comparison.B;
    }
}
