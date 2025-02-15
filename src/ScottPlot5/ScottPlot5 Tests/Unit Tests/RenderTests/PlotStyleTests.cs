namespace ScottPlotTests.RenderTests;

internal class PlotStyleTests
{
    [Test]
    public void Test_PlotStyle_Apply()
    {
        // create a plot with everything customized
        Plot plot1 = new();
        plot1.DataBackground.Color = Colors.Blue;
        plot1.FigureBackground.Color = Colors.Red;
        plot1.Add.Palette = new ScottPlot.Palettes.Tsitsulin();
        plot1.Axes.Color(Colors.Orange);
        plot1.Grid.MajorLineColor = Colors.Orange.WithAlpha(.5);
        plot1.Legend.BackgroundColor = Colors.Green;
        plot1.Legend.FontColor = Colors.White;
        plot1.Legend.OutlineColor = Colors.Magenta;

        // create a second plot and apply the first plot's style to it
        Plot plot2 = new();
        plot2.SetStyle(plot1);

        // add data to both plots
        plot1.Add.Signal(Generate.Sin());
        plot1.Add.Signal(Generate.Cos());
        plot2.Add.Signal(Generate.Sin());
        plot2.Add.Signal(Generate.Cos());

        // add legend to both plots
        plot1.GetPlottables<ScottPlot.Plottables.Signal>().ToList().ForEach(x => x.LegendText = "test");
        plot2.GetPlottables<ScottPlot.Plottables.Signal>().ToList().ForEach(x => x.LegendText = "test");

        // render an image for visual inspection
        plot1.SaveTestImage(subName: "1");
        plot2.SaveTestImage(subName: "2");

        // images should be identical
        plot1.Should().RenderIdenticallyTo(plot2);
    }

    [Test]
    public void Test_PlotStyle_Light()
    {
        // create a plot with everything customized
        Plot plot1 = new();
        plot1.DataBackground.Color = Colors.Blue;
        plot1.FigureBackground.Color = Colors.Red;
        plot1.Add.Palette = new ScottPlot.Palettes.Tsitsulin();
        plot1.Axes.Color(Colors.Orange);
        plot1.Grid.MajorLineColor = Colors.Orange.WithAlpha(.5);
        plot1.Legend.BackgroundColor = Colors.Green;
        plot1.Legend.FontColor = Colors.White;
        plot1.Legend.OutlineColor = Colors.Magenta;

        // apply light mode
        ScottPlot.PlotStyles.Light style = new();
        plot1.SetStyle(style);

        // create a default plot
        Plot plot2 = new();

        // add data to both plots
        plot1.Add.Signal(Generate.Sin());
        plot1.Add.Signal(Generate.Cos());
        plot2.Add.Signal(Generate.Sin());
        plot2.Add.Signal(Generate.Cos());

        // add legend to both plots
        plot1.GetPlottables<ScottPlot.Plottables.Signal>().ToList().ForEach(x => x.LegendText = "test");
        plot2.GetPlottables<ScottPlot.Plottables.Signal>().ToList().ForEach(x => x.LegendText = "test");

        // images should be identical
        plot1.Should().RenderIdenticallyTo(plot2);
    }

    [Test]
    public void Test_PlotStyle_Dark()
    {
        Plot plot1 = new();

        ScottPlot.PlotStyles.Dark style = new();
        plot1.SetStyle(style);

        plot1.Add.Signal(Generate.Sin());
        plot1.Add.Signal(Generate.Cos());
        plot1.GetPlottables<ScottPlot.Plottables.Signal>().ToList().ForEach(x => x.LegendText = "test");

        plot1.SaveTestImage();
    }
}
