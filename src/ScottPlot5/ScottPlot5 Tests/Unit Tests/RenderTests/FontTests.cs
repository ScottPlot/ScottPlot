namespace ScottPlotTests.RenderTests;

internal class FontTests
{
    [Test]
    public void Test_Font_Default()
    {
        Plot plt = new();
        plt.Axes.Top.Label.Text = Fonts.Default;
        plt.XLabel("Horizontal Axis");
        plt.YLabel("Vertical Axis");
        plt.SaveTestImage();
    }

    [Test]
    public void Test_Font_SpecialCharacters()
    {
        List<(string, string)> samples =
        [
            ("English", "Test"),
            ("Chinese", "测试"),
            ("Japanese", "試験"),
            ("Korean", "테스트"),
            ("Chinese Mixed", "测试 123ABC"),
            ("Japanese Mixed", "試験 123ABC"),
            ("Korean Mixed", "테스트 123ABC"),
        ];

        Plot myPlot = new();
        myPlot.HideGrid();

        for (int i = 0; i < samples.Count; i++)
        {
            string language = samples[i].Item1;
            string text = samples[i].Item2;

            var txtSample = myPlot.Add.Text(text, 2.5, i);
            txtSample.LabelFontSize = 14;
            txtSample.LabelFontName = Fonts.Detect(text); // this works
            txtSample.LabelStyle.SetBestFont(); // this also works
            txtSample.LabelFontColor = Colors.Magenta;

            var txtLanguage = myPlot.Add.Text(language, 0, i);
            txtLanguage.LabelFontSize = 14;

            var txtFont = myPlot.Add.Text(txtSample.LabelFontName, 5, i);
            txtFont.LabelFontSize = 14;
            txtFont.LabelFontColor = Colors.Green;
        }

        myPlot.HideGrid();
        myPlot.Axes.SetLimits(-1, 10, -1, samples.Count);
        myPlot.SaveTestImage();
    }
}
