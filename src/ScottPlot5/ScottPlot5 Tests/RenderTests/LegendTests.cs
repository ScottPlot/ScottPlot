namespace ScottPlotTests.RenderTests;

internal class LegendTests
{
    [Test]
    public void Test_Legend_Toggle()
    {
        Plot plt = new();
        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.Label = "Sine";
        sig2.Label = "Cosine";

        plt.SaveTestImage(300, 200, "legend-default");

        plt.Legend.IsVisible = true;
        plt.SaveTestImage(300, 200, "legend-enabled");

        plt.Legend.IsVisible = false;
        plt.SaveTestImage(300, 200, "legend-disabled");
    }

    [Test]
    public void Test_Legend_FontStyle()
    {
        Plot plt = new();

        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.Label = "Sine";
        sig2.Label = "Cosine";

        plt.Legend.IsVisible = true;
        plt.Legend.Font.Size = 26;
        plt.Legend.Font.Color = Colors.Magenta;

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Legend_Image()
    {
        Plot plt = new();

        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.Label = "Sine";
        sig2.Label = "Cosine";

        Image img = plt.GetLegendImage();
        img.SaveTestImage();
    }

    [Test]
    public void Test_Legend_SvgImage()
    {
        Plot plt = new();

        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.Label = "Sine";
        sig2.Label = "Cosine";

        plt.Legend.IsVisible = true;

        string svgXml = plt.GetLegendSvgXml();
        svgXml.SaveTestString(".svg");
    }

    [Test]
    public void Test_Legend_EmptyWithoutEnabling()
    {
        Plot plt = new();
        plt.GetImage(300, 200);
        plt.GetLegendImage();
        plt.GetLegendSvgXml();
    }

    [Test]
    public void Test_Legend_EmptyWithEnabling()
    {
        Plot plt = new();
        plt.ShowLegend();
        plt.GetImage(300, 200);
        plt.GetLegendImage();
        plt.GetLegendSvgXml();
    }

    [Test]
    public void Test_Legend_Basic()
    {
        ScottPlot.Plot plt = new();

        for (int i = 0; i < 5; i++)
        {
            LegendItem item = new()
            {
                LabelText = $"Item #{i + 1}",
                LabelFontColor = Colors.Category10[i],
                LabelFontSize = 16,
                LineColor = Colors.Category10[i],
                LineWidth = 3,
            };
            plt.Legend.ManualItems.Add(item);
        }

        plt.ShowLegend();

        plt.SaveTestImage(300, 200);
    }

    private LegendItem[] GetSampleLegendItems(int fontSize = 16) =>
    [
        new LegendItem()
        {
            LabelText = $"Default",
            LabelFontSize = fontSize,
        },
        new LegendItem()
        {
            LabelText = $"Line",
            LabelFontSize = fontSize,
            LineWidth = 2,
            LineColor = Colors.Blue,
            LinePattern = LinePattern.Dotted,
        },
        new LegendItem()
        {
            LabelText = $"Fill",
            LabelFontSize = fontSize,
            FillColor = Colors.Green.WithAlpha(.5),
        },
        new LegendItem()
        {
            LabelText = $"Outline",
            LabelFontSize = fontSize,
            OutlineColor = Colors.Blue,
            OutlineWidth = 2,
        },
        new LegendItem()
        {
            LabelText = $"Fill+Outline",
            LabelFontSize = fontSize,
            FillColor = Colors.Green.WithAlpha(.5),
            OutlineColor = Colors.Blue,
            OutlineWidth = 2,
        },
        new LegendItem()
        {
            LabelText = $"Marker",
            LabelFontSize = fontSize,
            MarkerShape = MarkerShape.FilledDiamond,
            MarkerFillColor = Colors.Green.WithAlpha(.5),
            MarkerLineColor = Colors.Blue,
            MarkerLineWidth = 2,
            MarkerSize = 15,
        },
        new LegendItem()
        {
            LabelText = $"Marker+Line",
            LabelFontSize = fontSize,
            MarkerShape = MarkerShape.FilledCircle,
            MarkerFillColor = Colors.Green.WithAlpha(.5),
            MarkerSize = 10,
            LineWidth = 2,
            LineColor = Colors.Blue,
        },
        new LegendItem()
        {
            LabelText = $"Arrow",
            LabelFontSize = fontSize,
            ArrowLineWidth = 2,
            ArrowColor = Colors.Blue,
        },
    ];

    [Test]
    public void Test_Legend_Symbols()
    {
        ScottPlot.Plot plt = new();
        GetSampleLegendItems().ToList().ForEach(plt.Legend.ManualItems.Add);
        plt.ShowLegend();
        plt.SaveTestImage();
    }

    [Test]
    public void Test_LegendImage_Symbols()
    {
        ScottPlot.Plot plt = new();
        GetSampleLegendItems().ToList().ForEach(plt.Legend.ManualItems.Add);
        plt.GetLegendImage().SaveTestImage();
    }

    [Test]
    public void Test_Legend_MultiLine()
    {
        ScottPlot.Plot plt = new();

        plt.Legend.ManualItems.Add(new LegendItem() { Label = "one\nalpha" });
        plt.Legend.ManualItems.Add(new LegendItem() { Label = "two\nbeta" });
        plt.Legend.ManualItems.Add(new LegendItem() { Label = "three" }); ;
        plt.ShowLegend();

        plt.SaveTestImage(300, 200);
    }

    [Test]
    public void Test_Legend_WrappingHorizontal()
    {
        ScottPlot.Plot plt = new();

        for (int i = 0; i < 13; i++)
        {
            Color color = Color.RandomHue();
            LegendItem item = new()
            {
                LabelText = $"Item #{i + 1}",
                LabelFontColor = color,
                LabelFontSize = 16,
                LineColor = color,
                LineWidth = 3,
            };
            plt.Legend.ManualItems.Add(item);
        }

        plt.Legend.Orientation = Orientation.Horizontal;
        plt.ShowLegend();

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Legend_WrappingVertical()
    {
        ScottPlot.Plot plt = new();

        for (int i = 0; i < 33; i++)
        {
            Color color = Color.RandomHue();
            LegendItem item = new()
            {
                LabelText = $"Item #{i + 1}",
                LabelFontColor = color,
                LabelFontSize = 16,
                LineColor = color,
                LineWidth = 3,
            };
            plt.Legend.ManualItems.Add(item);
        }

        plt.Legend.Orientation = Orientation.Vertical;
        plt.ShowLegend();

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Plottables_AppearInLegend()
    {
        double[] xs = [1, 2, 3];
        double[] ys = [1, 2, 3];
        double[] err = [1, 2, 3];

        Plot plt = new();

        var arrow = plt.Add.Arrow(1, 2, 3, 4);
        arrow.Label = "arrow";

        var axLine = plt.Add.VerticalLine(0);
        axLine.LabelText = "axis line";

        var axSpan = plt.Add.VerticalSpan(2, 3);
        axSpan.Label = "axis span";

        var bar = plt.Add.Bar(2, 3);
        bar.Label = "bar";

        var box = plt.Add.Box(new Box());
        box.Label = "box";

        var dataLogger = plt.Add.DataLogger();
        dataLogger.Label = "data logger";

        var dataStreamer = plt.Add.DataStreamer(100);
        dataStreamer.Label = "data streamer";

        var ellipse = plt.Add.Ellipse(1, 2, 3, 4);
        ellipse.Label = "ellipse";

        var errorBar = plt.Add.ErrorBar(xs, ys, err);
        errorBar.Label = "error bar";

        var filly = plt.Add.FillY(xs, ys, err);
        filly.Label = "fill Y";

        var func = plt.Add.Function(SampleData.DunningKrugerCurve);
        func.Label = "function";

        var line = plt.Add.Line(1, 2, 3, 4);
        line.Label = "line";

        var marker = plt.Add.Marker(1, 2);
        marker.Label = "marker";

        var markers = plt.Add.Markers(xs, ys);
        markers.Label = "markers";

        var pie = plt.Add.Pie(xs);
        foreach (var slice in pie.Slices)
            slice.Label = "pie slice";

        var poly = plt.Add.Polygon(xs, ys);
        poly.Label = "polygon";

        var rect = plt.Add.Rectangle(1, 2, 3, 4);
        rect.Label = "rectangle";

        var scatter = plt.Add.Scatter(xs, ys);
        scatter.Label = "scatter";

        var sig = plt.Add.Signal(ys);
        sig.Label = "signal";

        var sigConst = plt.Add.SignalConst(ys);
        sigConst.Label = "signal const";

        var sigXY = plt.Add.SignalXY(xs, ys);
        sigXY.Label = "signal XY";

        plt.GetLegendImage().SaveTestImage();
    }
}
