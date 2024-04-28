namespace ScottPlotTests.RenderTests;

internal class LayoutTests
{
    [Test]
    public void Test_Layout_Automatic()
    {
        ScottPlot.Plot plt = new();
        plt.Add.Signal(ScottPlot.Generate.Sin());
        plt.Add.Signal(ScottPlot.Generate.Cos());

        foreach (IAxis panel in plt.Axes.GetAxes())
        {
            panel.ShowDebugInformation = true;
        }

        plt.Axes.Bottom.Label.Text = "Horizontal Axis";
        plt.Axes.Left.Label.Text = "Vertical Axis";
        plt.Axes.Right.Label.Text = "Secondary Vertical Axis";
        plt.Axes.Top.Label.Text = "Secondary Horizontal Axis";
        plt.Axes.Title.Label.Text = "Title Panel";

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Layout_Automatic_MinPanelSize()
    {
        ScottPlot.Plot plt = new();
        plt.Add.Signal(ScottPlot.Generate.Sin());
        plt.Add.Signal(ScottPlot.Generate.Cos());

        foreach (IAxis panel in plt.Axes.GetAxes())
        {
            panel.MinimumSize = 100;
            panel.ShowDebugInformation = true;
        }

        plt.Axes.Bottom.Label.Text = "Horizontal Axis";
        plt.Axes.Left.Label.Text = "Vertical Axis";
        plt.Axes.Right.Label.Text = "Secondary Vertical Axis";
        plt.Axes.Top.Label.Text = "Secondary Horizontal Axis";
        plt.Axes.Title.Label.Text = "Title Panel";

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Layout_Automatic_MaxPanelSize()
    {
        ScottPlot.Plot plt = new();
        plt.Add.Signal(ScottPlot.Generate.Sin());
        plt.Add.Signal(ScottPlot.Generate.Cos());

        foreach (IAxis panel in plt.Axes.GetAxes())
        {
            panel.MaximumSize = 20;
            panel.ShowDebugInformation = true;
        }

        plt.Axes.Bottom.Label.Text = "Horizontal Axis";
        plt.Axes.Left.Label.Text = "Vertical Axis";
        plt.Axes.Right.Label.Text = "Secondary Vertical Axis";
        plt.Axes.Top.Label.Text = "Secondary Horizontal Axis";
        plt.Axes.Title.Label.Text = "Title Panel";

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Layout_Automatic_FixedPanelSize()
    {
        ScottPlot.Plot plt = new();
        plt.Add.Signal(ScottPlot.Generate.Sin());
        plt.Add.Signal(ScottPlot.Generate.Cos());

        foreach (IAxis panel in plt.Axes.GetAxes())
        {
            panel.MinimumSize = 50;
            panel.MaximumSize = 50;
            panel.ShowDebugInformation = true;
        }

        plt.Axes.Bottom.Label.Text = "Horizontal Axis";
        plt.Axes.Left.Label.Text = "Vertical Axis";
        plt.Axes.Right.Label.Text = "Secondary Vertical Axis";
        plt.Axes.Top.Label.Text = "Secondary Horizontal Axis";
        plt.Axes.Title.Label.Text = "Title Panel";

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Layout_RightLabelAndTicks()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3104

        ScottPlot.Plot myPlot = new();

        // plottables use the standard X and Y axes by default
        var sig1 = myPlot.Add.Signal(Generate.Sin(51, mult: 0.01));
        sig1.Axes.XAxis = myPlot.Axes.Bottom;
        sig1.Axes.YAxis = myPlot.Axes.Left;
        myPlot.Axes.Left.Label.Text = "Primary Y Axis";

        // add a new plottable and tell it to use the right Y axis
        var sig2 = myPlot.Add.Signal(Generate.Cos(51, mult: 100));
        sig2.Axes.XAxis = myPlot.Axes.Bottom;
        sig2.Axes.YAxis = myPlot.Axes.Right;
        myPlot.Axes.Right.Label.Text = "Secondary Y Axis";

        // Use fixed layout
        myPlot.Layout.Fixed(new PixelPadding(100));

        myPlot.SaveTestImage();
    }
}
