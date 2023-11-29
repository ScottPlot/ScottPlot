namespace ScottPlotTests;

internal class Sandbox
{
    [Test]
    public void Test_Sandbox()
    {
        Plot plt = new();
        plt.LeftAxis.Label.Text = "Vertical Axis";
        plt.LeftAxis.MinimumSize = 100;
        plt.SaveTestImage();
    }
}
