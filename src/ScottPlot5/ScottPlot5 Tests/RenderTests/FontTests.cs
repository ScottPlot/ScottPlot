namespace ScottPlotTests.RenderTests;

internal class FontTests
{
    [Test]
    public void Test_Font_SpecialCharacters()
    {
        Fonts.Default = Fonts.Detect('频');

        ScottPlot.Plot plt = new();
        plt.TopAxis.Label.Text = "频率信号削减123";
        Console.WriteLine(plt.TopAxis.Label.Font.Name);
        plt.SaveTestImage();
    }
}
