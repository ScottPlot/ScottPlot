namespace ScottPlotTests;

internal class ImageDiffTests
{
    [Test]
    public void Test_Image_Diff()
    {
        Image img1 = new("TestImages/bag_frame1.png");
        Image img2 = new("TestImages/bag_frame2.png");
        ScottPlot.Testing.ImageDiff diff = new(img1, img2);

        diff.PercentOfDifferencePixels.Should().BeApproximately(17.94, .01);
        diff.NumberOfDifferentPixels.Should().Be(1601);
        diff.DifferenceImage?.SaveTestImage();
    }
}
