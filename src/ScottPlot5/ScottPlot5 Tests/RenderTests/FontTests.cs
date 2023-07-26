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
        string chinese = "测试";
        string japanese = "試験";
        string korean = "테스트";
        string msg = $"Chinese: {chinese} " +
            $"Japanese: {japanese} " +
            $"Korean: {korean}";

        Plot plt1 = new();
        plt1.Title($"[{Fonts.Default}] {msg}");
        plt1.SaveTestImage(600, 400, "default");

        Fonts.Default = Fonts.Detect(chinese) ?? Fonts.System;
        Plot plt2 = new();
        plt2.Title($"[{Fonts.Default}] {msg}");
        plt2.SaveTestImage(600, 400, "chinese");

        Fonts.Default = Fonts.Detect(japanese) ?? Fonts.System;
        Plot plt3 = new();
        plt3.Title($"[{Fonts.Default}] {msg}");
        plt3.SaveTestImage(600, 400, "japanese");

        Fonts.Default = Fonts.Detect(korean) ?? Fonts.System;
        Plot plt4 = new();
        plt4.Title($"[{Fonts.Default}] {msg}");
        plt4.SaveTestImage(600, 400, "korean");
    }
}
