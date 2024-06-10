namespace ScottPlotTests.RenderTests;

internal class LegendTests
{
    [Test]
    public void Test_Legend_Toggle()
    {
        Plot plt = new();
        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.LegendText = "Sine";
        sig2.LegendText = "Cosine";

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

        sig1.LegendText = "Sine";
        sig2.LegendText = "Cosine";

        plt.Legend.IsVisible = true;
        plt.Legend.FontSize = 26;
        plt.Legend.FontColor = Colors.Magenta;

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Legend_Image()
    {
        Plot plt = new();

        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.LegendText = "Sine";
        sig2.LegendText = "Cosine";

        Image img = plt.GetLegendImage();
        img.SaveTestImage();
    }

    [Test]
    public void Test_Legend_SvgImage()
    {
        Plot plt = new();

        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.LegendText = "Sine";
        sig2.LegendText = "Cosine";

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

        var leg2 = plt.Add.Legend();
        leg2.Alignment = Alignment.LowerCenter;

        for (int i = 0; i < 5; i++)
        {
            LegendItem item1 = new()
            {
                LabelText = $"ASDFgj",
                LabelFontColor = Colors.Category10[i],
                LabelFontSize = 22,
                LineColor = Colors.Category10[i],
                LineWidth = 3,
            };

            LegendItem item2 = new()
            {
                LabelText = $"ASDF",
                LabelFontColor = Colors.Category10[i],
                LabelFontSize = 22,
                LineColor = Colors.Category10[i],
                LineWidth = 3,
            };

            plt.Legend.ManualItems.Add(item1);
            leg2.ManualItems.Add(item2);
        }

        plt.ShowLegend();

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Legend_FontOverride()
    {
        ScottPlot.Plot plt = new();

        for (int i = 0; i < 5; i++)
        {
            LegendItem item = new()
            {
                LabelText = $"AgAgAg Item #{i + 1}",
                LabelFontColor = Colors.Category10[i],
                LabelFontSize = 22,
                LineColor = Colors.Category10[i],
                LineWidth = 3,
            };
            plt.Legend.ManualItems.Add(item);
        }

        plt.Legend.FontSize = 12;
        plt.Legend.FontColor = Colors.Magenta;
        plt.Legend.FontName = Fonts.Monospace;

        plt.ShowLegend();
        plt.SaveTestImage();
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
            ArrowFillColor = Colors.Blue,
            ArrowLineColor = Colors.Transparent,
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

        for (int i = 0; i < 5; i++)
        {
            var sig = plt.Add.Signal(Generate.Sin(phase: i / 20.0));
            sig.LineWidth = 2;
            sig.LegendText = i % 2 == 0 ? $"Single Line" : "Multi\nLine";
        }

        plt.ShowLegend();
        plt.SaveTestImage();
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
    public void Test_Legend_WrappingHorizontalTightWrap()
    {
        ScottPlot.Plot plt = new();

        for (int i = 0; i < 5; i++)
        {
            LegendItem item = new()
            {
                LabelText = new string('A', count: i * 5 + 1),
                LineColor = Colors.Blue,
                LineWidth = 3,
            };
            plt.Legend.TightHorizontalWrapping = true;
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

        plt.Add.Arrow(1, 2, 3, 4);
        plt.Add.VerticalLine(0);
        plt.Add.VerticalSpan(2, 3);
        plt.Add.Bar(2, 3);
        plt.Add.Box(new Box());
        plt.Add.DataLogger();
        plt.Add.DataStreamer(100);
        plt.Add.Ellipse(1, 2, 3, 4);
        plt.Add.ErrorBar(xs, ys, err);
        plt.Add.FillY(xs, ys, err);
        plt.Add.Function(SampleData.DunningKrugerCurve);
        plt.Add.Line(1, 2, 3, 4);
        plt.Add.Marker(1, 2);
        plt.Add.Markers(xs, ys);
        plt.Add.Polygon(xs, ys);
        plt.Add.Rectangle(1, 2, 3, 4);
        plt.Add.Scatter(xs, ys);
        plt.Add.Signal(ys);
        plt.Add.SignalConst(ys);
        plt.Add.SignalXY(xs, ys);

        foreach (var plottable in plt.GetPlottables())
        {
            if (plottable is IHasLegendText h)
            {
                h.LegendText = plottable.GetType().Name;
            }
            else
            {
                Assert.Fail($"${plottable} does not implement {nameof(IHasLegendText)}");
            }
        }

        // special cases of plottables with child legend items
        var pie = plt.Add.Pie(xs);
        foreach (var slice in pie.Slices)
            slice.LegendText = "pie slice";

        plt.GetLegendImage().SaveTestImage();
    }

    [Test]
    public void Test_Legend_Panel()
    {
        ScottPlot.Plot plt = new();

        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.LegendText = "Sine";
        sig2.LegendText = "Cosine";

        plt.HideLegend(); // hide the default legend

        ScottPlot.Panels.LegendPanel pan = new(plt.Legend)
        {
            Edge = Edge.Right,
            Alignment = Alignment.UpperCenter,
        };

        plt.Axes.AddPanel(pan);

        plt.SaveTestImage();
    }
}
