using ScottPlot;

namespace Sandbox.WinFormsFinance;

public partial class TradingViewForm : Form
{
    // TODO: make an abstraction for click-drag placement of new technical indicators
    ScottPlot.Plottables.LinePlot? LineBeingAdded = null;
    bool AddDrawingMode = false;

    public TradingViewForm()
    {
        InitializeComponent();
        formsPlot1.UserInputProcessor.DoubleLeftClickBenchmark(false);

        InitializePlot();

        button1.Click += (s, e) =>
        {
            Text = "Click to start placing a line";
            AddDrawingMode = true;
        };

        buttonClearAll.Click += (s, e) =>
        {
            Text = "All drawings cleared";
            formsPlot1.Plot.Remove<ScottPlot.Plottables.LinePlot>();
            formsPlot1.Refresh();
        };

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    void InitializePlot()
    {
        // reset the plot so we can call this multiple times as ticker or time period options changes
        formsPlot1.Plot.Clear();

        // place text on the background
        formsPlot1.Plot.Add.BackgroundText(
            line1: "MNQZ4",
            line2: "Micro E-mini Nasdaq-100 DEC 24 CME",
            color: Colors.White.WithAlpha(.07),
            size1: 96,
            size2: 36);

        // generate sample data
        List<OHLC> ohlcs = Generate.RandomOHLCs(75);
        DateTime start = new(2024, 10, 24);
        TimeSpan interval = TimeSpan.FromSeconds(10);
        for (int i = 0; i < ohlcs.Count; i++)
        {
            ohlcs[i] = ohlcs[i]
                .WithDate(start + interval * i)
                .WithTimeSpan(interval);
        }

        // add a candle plot using the right axis
        formsPlot1.Plot.Axes.DateTimeTicksBottom();
        var candlePlot = formsPlot1.Plot.Add.Candlestick(ohlcs);
        candlePlot.RisingColor = new ScottPlot.Color("#37dbba");
        candlePlot.FallingColor = new ScottPlot.Color("#eb602f");

        // add SMA lines
        int[] smaWindowSizes = { 8, 20 };
        foreach (int windowSize in smaWindowSizes)
        {
            ScottPlot.Finance.SimpleMovingAverage sma = new(ohlcs, windowSize);
            var sp = formsPlot1.Plot.Add.Scatter(sma.Dates, sma.Means);
            sp.Axes.YAxis = formsPlot1.Plot.Axes.Right;
            sp.LegendText = $"SMA {windowSize}";
            sp.MarkerSize = 0;
            sp.LineWidth = 1.5f;
            sp.LinePattern = LinePattern.Dotted;
            sp.Color = Colors.Cyan.WithAlpha(1 - windowSize / 30.0);
        }

        // tell the candles and grid lines to use the right axis
        candlePlot.Axes.YAxis = formsPlot1.Plot.Axes.Right;
        formsPlot1.Plot.Grid.YAxis = formsPlot1.Plot.Axes.Right;

        // customize format of right axis tick labels
        static string CustomFormatter(double price) => price.ToString("C");
        ScottPlot.TickGenerators.NumericAutomatic myTickGenerator = new() { LabelFormatter = CustomFormatter };
        formsPlot1.Plot.Axes.Right.TickGenerator = myTickGenerator;

        // style plot colors
        ScottPlot.Color backgroundColor = new("#131e28");
        ScottPlot.Color foregroundColor = new("#6e7780");
        BackColor = backgroundColor.ToSDColor();
        formsPlot1.Plot.FigureBackground.Color = backgroundColor;
        formsPlot1.Plot.DataBackground.Color = backgroundColor;
        formsPlot1.Plot.Axes.Color(foregroundColor);
        formsPlot1.Plot.Grid.MajorLineColor = foregroundColor.WithAlpha(.15);
        formsPlot1.Plot.Legend.BackgroundColor = backgroundColor.Lighten(.02);
        formsPlot1.Plot.Legend.OutlineColor = foregroundColor;
        formsPlot1.Plot.Legend.FontColor = foregroundColor;

        // set axis limits to fit the data
        formsPlot1.Plot.Axes.AutoScale();

        // force a redraw
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        if (LineBeingAdded is not null)
        {
            Text = "Drawing finished";
            AddDrawingMode = false;

            // this is the second click so place the second point here and release the line
            LineBeingAdded.End = formsPlot1.Plot.GetCoordinates(e.X, e.Y);
            LineBeingAdded = null;

            // request a redraw
            formsPlot1.Refresh();

            // re-enable click-drag pan and zoom
            formsPlot1.UserInputProcessor.Reset();
            formsPlot1.UserInputProcessor.Enable();
            return;
        }

        if (AddDrawingMode)
        {
            Text = "Drawing started";

            // disable mouse pan and zoom while click-dragging to add a new indicator
            formsPlot1.UserInputProcessor.Disable();

            // create a new drawing where the cursor is
            Coordinates cs = formsPlot1.Plot.GetCoordinates(e.X, e.Y);
            LineBeingAdded = formsPlot1.Plot.Add.Line(cs, cs);
            LineBeingAdded.LineWidth = 3;
            LineBeingAdded.LinePattern = LinePattern.Solid;
            LineBeingAdded.LineColor = Colors.Yellow.WithAlpha(.5);
            LineBeingAdded.MarkerShape = MarkerShape.FilledCircle;
            LineBeingAdded.MarkerFillColor = Colors.Yellow;
            LineBeingAdded.MarkerSize = 8;

            // request a redraw
            formsPlot1.Refresh();
        }
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        if (LineBeingAdded is not null)
        {
            // the second click hasn't happened yet so place the second point where the cursor is
            LineBeingAdded.End = formsPlot1.Plot.GetCoordinates(e.X, e.Y);

            // request a redraw
            formsPlot1.Refresh();
        }
    }
}
