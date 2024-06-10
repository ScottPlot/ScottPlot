namespace ScottPlot.Testing;

/// <summary>
/// Compare two raster images to identify and quantify differences
/// </summary>
public class ImageDiff
{
    public double NumberOfDifferentPixels { get; }
    public double PercentOfDifferencePixels { get; }
    public double TotalDifference { get; }
    public double MaxDifference { get; }
    public Image? DifferenceImage { get; }

    public ImageDiff(Image img1, Image img2, bool saveDiffImage = true)
    {
        if (img1.Size != img2.Size)
            throw new InvalidOperationException("images must be the same size");

        byte[,] diffValues = new byte[img1.Height, img2.Width];
        byte[,] img1values = img1.GetArrayGrayscale();
        byte[,] img2values = img2.GetArrayGrayscale();

        NumberOfDifferentPixels = 0;
        TotalDifference = 0;
        MaxDifference = 0;
        for (int y = 0; y < img1.Height; y++)
        {
            for (int x = 0; x < img2.Width; x++)
            {
                int diffValue = Math.Abs(img1values[y, x] - img2values[y, x]);
                diffValues[y, x] = (byte)diffValue;
                NumberOfDifferentPixels += diffValue > 0 ? 1 : 0;
                TotalDifference += diffValue;
                if (diffValue > MaxDifference)
                {
                    MaxDifference = diffValue;
                }
            }
        }

        PercentOfDifferencePixels = (double)NumberOfDifferentPixels / (img1.Height * img2.Width) * 100;

        if (saveDiffImage)
            DifferenceImage = new(diffValues);
    }
}
