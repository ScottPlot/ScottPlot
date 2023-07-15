namespace ScottPlotTests.RenderTests;

internal class FontTests
{
    [Test]
    public void Test_Font_Default()
    {
        ScottPlot.Plot plt = new();
        plt.TopAxis.Label.Text = Fonts.Default;
        plt.XLabel("Horizontal Axis");
        plt.YLabel("Vertical Axis");
        plt.SaveTestImage();
    }

    [Test]
    public void Test_Font_SpecialCharacters()
    {
        Fonts.Default = Fonts.Detect('频') ?? Fonts.System;

        ScottPlot.Plot plt = new();
        plt.TopAxis.Label.Text = "频率信号削减123";
        plt.XLabel("Horizontal Axis");
        plt.YLabel("Vertical Axis");
        plt.SaveTestImage();
    }
}
