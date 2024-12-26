using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class MultiplotCollapsed : Form, IDemoWindow
{
    public string Title => "Multiplot with Collapsed Subplots";

    public string Description => "Subplots may be placed very close together by setting their padding to zero " +
        "to enhance the visual effect of multiple subplots sharing a single axis";

    public MultiplotCollapsed()
    {
        InitializeComponent();

        // setup a multiplot with 3 subplots
        formsPlot1.Multiplot.AddPlots(3);

        // add sample price data to the first plot
        Plot pricePlot = formsPlot1.Multiplot.GetPlot(0);
        pricePlot.Axes.Right.Label.Text = "Price";
        List<OHLC> ohlcs = Generate.RandomOHLCs(50);
        var candlestick = pricePlot.Add.Candlestick(ohlcs);
        candlestick.Axes.YAxis = pricePlot.Axes.Right;
        candlestick.Sequential = true;

        // add sample RSI data to the second plot
        Plot rsiPlot = formsPlot1.Multiplot.GetPlot(1);
        rsiPlot.Axes.Right.Label.Text = "RSI";
        double[] rsiValues = Generate.RandomWalk(ohlcs.Count);
        var rsiSig = rsiPlot.Add.Signal(rsiValues);
        rsiSig.Axes.YAxis = rsiPlot.Axes.Right;
        rsiSig.LineWidth = 2;

        // add sample RSI data to the second plot
        Plot volumePlot = formsPlot1.Multiplot.GetPlot(2);
        volumePlot.Axes.Right.Label.Text = "Volume";
        double[] volumes = Generate.RandomSample(50, 10, 90);
        var bars = volumePlot.Add.Bars(volumes);
        bars.Axes.YAxis = volumePlot.Axes.Right;
        volumePlot.Axes.Margins(bottom: 0);

        // give all plots identical left and right padding to ensure alignment
        float padLeft = 20;
        float padRight = 65;

        // set padding so there is no space between the middle and adjacent plots
        pricePlot.Layout.Fixed(new PixelPadding(padLeft, padRight, bottom: 0, top: 25));
        rsiPlot.Layout.Fixed(new PixelPadding(padLeft, padRight, bottom: 0, top: 0));
        volumePlot.Layout.Fixed(new PixelPadding(padLeft, padRight, bottom: 40, top: 0));

        // use custom logic to tell the multiplot how large to make each plot
        formsPlot1.Multiplot.SetPosition(pricePlot, new ExpandingTopRow(0));
        formsPlot1.Multiplot.SetPosition(rsiPlot, new ExpandingTopRow(1));
        formsPlot1.Multiplot.SetPosition(volumePlot, new ExpandingTopRow(2));

        // link horizontal axes across all plots
        formsPlot1.Multiplot.ShareX([pricePlot, rsiPlot, volumePlot]);
    }

    class ExpandingTopRow(int rowIndex) : ISubplotPosition
    {
        public float MiddlePlotHeight = 100;
        public float BottomPlotHeight = 100;

        public PixelRect GetRect(PixelRect figureRect)
        {
            float bottomPlotTop = figureRect.Bottom - BottomPlotHeight;
            float middlePlotTop = bottomPlotTop - MiddlePlotHeight;

            return rowIndex switch
            {
                0 => new(figureRect.Left, figureRect.Right, bottom: middlePlotTop, top: figureRect.Top),
                1 => new(figureRect.Left, figureRect.Right, bottom: bottomPlotTop, top: middlePlotTop),
                2 => new(figureRect.Left, figureRect.Right, bottom: figureRect.Bottom, top: bottomPlotTop),
                _ => throw new IndexOutOfRangeException(nameof(rowIndex)),
            };
        }
    }
}
