namespace GraphicalTestRunner;

public class ImageComparisonDetails
{
    public string BeforePath { get; }
    public string AfterPath { get; }
    public string Filename { get; }
    public string Change { get; }
    public double Difference { get; }
    RasterTestImage ImageBefore { get; }
    RasterTestImage ImageAfter { get; }

    public ImageComparisonDetails(string before, string after, string filename)
    {
        BeforePath = Path.Join(before, filename);
        AfterPath = Path.Join(after, filename);
        Filename = filename;

        bool beforeExists = File.Exists(BeforePath);
        bool afterExists = File.Exists(AfterPath);

        if (!beforeExists && !afterExists)
        {
            Change = "neither";
            Difference = 100;
            return;
        }
        else if (!beforeExists && afterExists)
        {
            Change = "new";
            Difference = 100;
            return;
        }
        else if (beforeExists && !afterExists)
        {
            Change = "deleted";
            Difference = 100;
            return;
        }

        ImageBefore = new RasterTestImage(BeforePath);
        ImageAfter = new RasterTestImage(AfterPath);

        //Difference = PercentPixelsDifferent(ImageBefore, ImageAfter);
        Difference = AmountPixelsDifferent(ImageBefore, ImageAfter);
        Change = (Difference == 0) ? "identical" : "changed";
    }

    private static double PercentPixelsDifferent(RasterTestImage img1, RasterTestImage img2)
    {
        if (img1.Width != img2.Width || img1.Height != img2.Height)
            throw new InvalidOperationException();

        int count = 0;

        for (int y = 0; y < img1.Height; y++)
        {
            for (int x = 0; x < img1.Width; x++)
            {
                byte value1 = img1.GrayscaleBytes[y, x];
                byte value2 = img2.GrayscaleBytes[y, x];
                if (value1 != value2)
                {
                    count += 1;
                }
            }
        }

        return (double)count / (img1.Width * img1.Height) * 100;
    }

    private static double AmountPixelsDifferent(RasterTestImage img1, RasterTestImage img2)
    {
        if (img1.Width != img2.Width || img1.Height != img2.Height)
            throw new InvalidOperationException();

        double sum = 0;

        for (int y = 0; y < img1.Height; y++)
        {
            for (int x = 0; x < img1.Width; x++)
            {
                byte value1 = img1.GrayscaleBytes[y, x];
                byte value2 = img2.GrayscaleBytes[y, x];
                if (value1 != value2)
                {
                    sum += Math.Abs(value1 - value2);
                }
            }
        }

        return sum;
    }
}
