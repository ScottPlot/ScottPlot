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

        foreach (IPanel panel in plt.Axes)
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

    [Test]
    public void Test_Layout_RightLabelAndTicks()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3104

        ScottPlot.Plot myPlot = new();

        // plottables use the standard X and Y axes by default
        var sig1 = myPlot.Add.Signal(Generate.Sin(51, mult: 0.01));
        sig1.Axes.XAxis = myPlot.XAxis;
        sig1.Axes.YAxis = myPlot.YAxis;
        myPlot.YAxis.Label.Text = "Primary Y Axis";

        // add a new plottable and tell it to use the right Y axis
        var sig2 = myPlot.Add.Signal(Generate.Cos(51, mult: 100));
        sig2.Axes.XAxis = myPlot.XAxis;
        sig2.Axes.YAxis = myPlot.RightAxis;
        myPlot.RightAxis.Label.Text = "Secondary Y Axis";

        // Use fixed layout
        myPlot.Layout.Fixed(new PixelPadding(100));

        myPlot.SaveTestImage();
    }
}
