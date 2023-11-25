namespace ScottPlotTests.RenderTests;

internal class LayoutTests
{
    [Test]
    public void Test_Layout_Automatic()
    {
        ScottPlot.Plot plt = new();
        plt.Add.Signal(ScottPlot.Generate.Sin());
        plt.Add.Signal(ScottPlot.Generate.Cos());

        foreach (IPanel panel in plt.Axes)
        {
            panel.ShowDebugInformation = true;
        }

        plt.BottomAxis.Label.Text = "Horizontal Axis";
        plt.LeftAxis.Label.Text = "Vertical Axis";
        plt.RightAxis.Label.Text = "Secondary Vertical Axis";
        plt.TopAxis.Label.Text = "Secondary Horizontal Axis";
        plt.TitlePanel.Label.Text = "Title Panel";

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Layout_Automatic_MinPanelSize()
    {
        ScottPlot.Plot plt = new();
        plt.Add.Signal(ScottPlot.Generate.Sin());
        plt.Add.Signal(ScottPlot.Generate.Cos());

        foreach(IPanel panel in plt.Axes)
        {
            panel.MinimumSize = 100;
            panel.ShowDebugInformation = true;
        }

        plt.BottomAxis.Label.Text = "Horizontal Axis";
        plt.LeftAxis.Label.Text = "Vertical Axis";
        plt.RightAxis.Label.Text = "Secondary Vertical Axis";
        plt.TopAxis.Label.Text = "Secondary Horizontal Axis";
        plt.TitlePanel.Label.Text = "Title Panel";

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Layout_Automatic_MaxPanelSize()
    {
        ScottPlot.Plot plt = new();
        plt.Add.Signal(ScottPlot.Generate.Sin());
        plt.Add.Signal(ScottPlot.Generate.Cos());

        foreach (IPanel panel in plt.Axes)
        {
            panel.MaximumSize = 20;
            panel.ShowDebugInformation = true;
        }

        plt.BottomAxis.Label.Text = "Horizontal Axis";
        plt.LeftAxis.Label.Text = "Vertical Axis";
        plt.RightAxis.Label.Text = "Secondary Vertical Axis";
        plt.TopAxis.Label.Text = "Secondary Horizontal Axis";
        plt.TitlePanel.Label.Text = "Title Panel";

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Layout_Automatic_FixedPanelSize()
    {
        ScottPlot.Plot plt = new();
        plt.Add.Signal(ScottPlot.Generate.Sin());
        plt.Add.Signal(ScottPlot.Generate.Cos());

        foreach (IPanel panel in plt.Axes)
        {
            panel.MinimumSize = 50;
            panel.MaximumSize = 50;
            panel.ShowDebugInformation = true;
        }

        plt.BottomAxis.Label.Text = "Horizontal Axis";
        plt.LeftAxis.Label.Text = "Vertical Axis";
        plt.RightAxis.Label.Text = "Secondary Vertical Axis";
        plt.TopAxis.Label.Text = "Secondary Horizontal Axis";
        plt.TitlePanel.Label.Text = "Title Panel";

        plt.SaveTestImage();
    }
}
