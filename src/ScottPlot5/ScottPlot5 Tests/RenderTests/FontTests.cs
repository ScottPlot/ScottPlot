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
        List<(string, string)> samples = new()
        {
            ( "English", "Test" ),
            ( "Chinese", "测试" ),
            ( "Japanese", "試験" ),
            ( "Korean", "테스트" ),
        };

        Plot myPlot = new();
        myPlot.HideGrid();

        for (int i = 0; i < samples.Count; i++)
        {
            string language = samples[i].Item1;
            string text = samples[i].Item2;

            var txtSample = myPlot.Add.Text(text, 1, i);
            txtSample.Size = 22;
            txtSample.FontName = Fonts.Detect(text); // this works
            txtSample.Label.SetBestFont(); // this also works
            txtSample.Color = Colors.Magenta;

            var txtLanguage = myPlot.Add.Text(language, 0, i);
            txtLanguage.Size = 22;

            var txtFont = myPlot.Add.Text(txtSample.FontName, 2, i);
            txtFont.Size = 22;
            txtFont.Color = Colors.Green;
        }

        myPlot.Axes.SetLimits(-.5, 4.5, -.5, 3.5);
        myPlot.SaveTestImage();
    }
}
