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
        // disable vertical zoom/pan because it will always be auto-scaled
        formsPlot1.UserInputProcessor.LeftClickDragPan(enable: true, horizontal: true, vertical: false);
        formsPlot1.UserInputProcessor.RightClickDragZoom(enable: true, horizontal: true, vertical: false);

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
        OHLC[] ohlcs = Generate.RandomOHLCs(1000)
            .Select(x => x.WithDate(DateTime.MinValue) // ensure only price is used
            .WithTimeSpan(TimeSpan.Zero)).ToArray(); // ensure only price is used

        DateTime[] dates = Generate.ConsecutiveWeekdays(ohlcs.Length);

        // add a candle plot using the right axis
        var candlePlot = formsPlot1.Plot.Add.Candlestick(ohlcs);
        candlePlot.RisingColor = new ScottPlot.Color("#37dbba");
        candlePlot.FallingColor = new ScottPlot.Color("#eb602f");

        // disable the built in tick generator and add our own
        formsPlot1.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.EmptyTickGenerator();
        formsPlot1.Plot.Axes.Bottom.MinimumSize = 100;
        ScottPlot.Plottables.FinancialTimeAxis financeAxis = new(dates);
        formsPlot1.Plot.Add.Plottable(financeAxis);
        financeAxis.LabelStyle.ForeColor = new("#6e7780");

        // tell the candles and grid lines to use the right axis
        candlePlot.Axes.YAxis = formsPlot1.Plot.Axes.Right;
        candlePlot.Sequential = true;
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

        // set axis limits to fit the data to start
        formsPlot1.Plot.Axes.AutoScale();

        // autoscale vertically according to all the candles in view
        static void VerticalAutoscaleToCandlesInView(RenderPack rp)
        {
            var candle = rp.Plot.GetPlottables<ScottPlot.Plottables.CandlestickPlot>().FirstOrDefault();
            if (candle is null)
                return;

            var ohlcs = candle.Data.GetOHLCs();

            // TODO: move this logic into the candlestick plottable or OHLC data source
            int minViewIndex = (int)NumericConversion.Clamp(rp.Plot.Axes.Bottom.Min, 0, ohlcs.Count - 1);
            int maxViewIndex = (int)NumericConversion.Clamp(rp.Plot.Axes.Bottom.Max, 0, ohlcs.Count - 1);
            var ohlcsInView = ohlcs.Skip(minViewIndex).Take(maxViewIndex - minViewIndex);
            if (!ohlcsInView.Any())
                return;
            double yMin = ohlcsInView.Select(x => x.Low).Min();
            double yMax = ohlcsInView.Select(x => x.High).Max();

            rp.Plot.Axes.Right.Range.Set(yMin, yMax);
        }
        formsPlot1.Plot.Axes.ContinuouslyAutoscale = true;
        formsPlot1.Plot.Axes.ContinuousAutoscaleAction = VerticalAutoscaleToCandlesInView;

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
            LineBeingAdded.End = formsPlot1.Plot.GetCoordinates(e.X, e.Y, formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Right);
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
            Coordinates cs = formsPlot1.Plot.GetCoordinates(e.X, e.Y, formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Right);
            LineBeingAdded = formsPlot1.Plot.Add.Line(cs, cs);
            LineBeingAdded.LineWidth = 3;
            LineBeingAdded.LinePattern = LinePattern.Solid;
            LineBeingAdded.LineColor = Colors.Yellow.WithAlpha(.5);
            LineBeingAdded.MarkerShape = MarkerShape.FilledCircle;
            LineBeingAdded.MarkerFillColor = Colors.Yellow;
            LineBeingAdded.MarkerSize = 8;
            LineBeingAdded.Axes.YAxis = formsPlot1.Plot.Axes.Right;

            // request a redraw
            formsPlot1.Refresh();
        }
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        if (LineBeingAdded is not null)
        {
            // the second click hasn't happened yet so place the second point where the cursor is
            LineBeingAdded.End = formsPlot1.Plot.GetCoordinates(e.X, e.Y, formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Right);

            // request a redraw
            formsPlot1.Refresh();
        }
    }
}
