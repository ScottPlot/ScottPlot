namespace GraphicalTestRunner;

public class ImageComparisonDetails
{
    public string BeforePath { get; }
    public string AfterPath { get; }
    public string Filename { get; }
    public string Change { get; }
    public double Difference { get; }

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

        // TODO: image comparison
        byte[] beforeBytes = File.ReadAllBytes(BeforePath);
        byte[] afterBytes = File.ReadAllBytes(AfterPath);
        float minLength = Math.Min(beforeBytes.Length, afterBytes.Length);

        int diffs = 0;
        for (int i = 0; i < minLength; i++)
        {
            if (beforeBytes[i] != afterBytes[i])
                diffs += 1;
        }

        Difference = (double)diffs / beforeBytes.Length * 100;
        Change = (diffs == 0) ? "identical" : "changed";
    }
}
