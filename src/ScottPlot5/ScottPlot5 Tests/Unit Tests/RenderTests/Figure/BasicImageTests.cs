namespace ScottPlotTests.RenderTests.Figure;

internal class BasicImageTests
{
    [Test]
    public void Test_Render_SinCos()
    {
        Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));
        plt.Add.Signal(Generate.Cos(51));

        plt.SaveTestImage();
    }

    [Test]
    public void Test_No_Data()
    {
        Plot plt = new();
        plt.SaveTestImage();
    }
}
