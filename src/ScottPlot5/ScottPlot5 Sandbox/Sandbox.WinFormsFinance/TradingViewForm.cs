using ScottPlot;
using ScottPlot.Plottables;

namespace Sandbox.WinFormsFinance;

public partial class TradingViewForm : Form
{
    // TODO: make an abstraction for click-drag placement of new technical indicators
    LinePlot? LineBeingAdded = null;
    bool AddDrawingMode = false;

    readonly Crosshair Crosshair = new();
    CandlestickPlot? CandlePlot = null;

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
            formsPlot1.Plot.Remove<LinePlot>();
            formsPlot1.Refresh();
        };

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;

        checkBoxLockScale.CheckedChanged += (s, e) =>
        {
            if (checkBoxLockScale.Checked)
            {
                double pxPerUnit = formsPlot1.Plot.LastRender.DataRect.Width / formsPlot1.Plot.Axes.Bottom.Range.Span;
                FixedHorizontalScale rule = new(formsPlot1.Plot.Axes.Bottom, pxPerUnit);
                formsPlot1.Plot.Axes.Rules.Add(rule);
            }
            else
            {
                formsPlot1.Plot.Axes.Rules.Clear();
            }
        };
    }

    public class FixedHorizontalScale(IXAxis xAxis, double pxPerUnit) : IAxisRule
    {
        public readonly IXAxis XAxis = xAxis;
        public double PxPerUnit = pxPerUnit;

        public void Apply(RenderPack rp, bool beforeLayout)
        {
            double right = XAxis.Max;
            double width = rp.DataRect.Width / PxPerUnit;
            double left = right - width;
            XAxis.Range.Set(left, right);
        }
    }

    void InitializePlot()
    {
        // disable vertical zoom/pan because it will always be auto-scaled
        formsPlot1.UserInputProcessor.LeftClickDragPan(enable: true, horizontal: true, vertical: false);
        formsPlot1.UserInputProcessor.RightClickDragZoom(enable: true, horizontal: true, vertical: false);

        // reset the plot so we can call this multiple times as ticker or time period options changes
        formsPlot1.Plot.Clear();

        // disable left ticks
        formsPlot1.Plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.EmptyTickGenerator();

        // place text on the background
        formsPlot1.Plot.Add.BackgroundText(
            line1: "MNQZ4",
            line2: "Micro E-mini Nasdaq-100 DEC 24 CME",
            color: Colors.White.WithAlpha(.07),
            size1: 96,
            size2: 36);

        // generate sample data
        OHLC[] ohlcs = Generate.RandomOHLCs(5_000)
            .Select(x => x.WithDate(DateTime.MinValue) // ensure only price is used
            .WithTimeSpan(TimeSpan.Zero)).ToArray(); // ensure only price is used

        DateTime[] dates = Generate.ConsecutiveDays(ohlcs.Length);

        // add a candle plot using the right axis
        CandlePlot = formsPlot1.Plot.Add.Candlestick(ohlcs);
        CandlePlot.RisingColor = new ScottPlot.Color("#37dbba");
        CandlePlot.FallingColor = new ScottPlot.Color("#eb602f");

        // add a crosshair to track the cursor
        formsPlot1.Plot.Add.Plottable(Crosshair);
        Crosshair.Axes.YAxis = formsPlot1.Plot.Axes.Right;
        Crosshair.LineColor = Colors.Yellow;
        Crosshair.LinePattern = LinePattern.Dashed;
        Crosshair.IsVisible = false;
        Crosshair.TextBackgroundColor = Colors.Yellow;
        Crosshair.TextColor = Colors.Black;
        Crosshair.HorizontalLine.LabelOppositeAxis = true;

        // disable the built in tick generator and add our own
        formsPlot1.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.EmptyTickGenerator();
        formsPlot1.Plot.Axes.Bottom.MinimumSize = 100;
        FinancialTimeAxis financeAxis = new(dates);
        formsPlot1.Plot.Add.Plottable(financeAxis);
        financeAxis.LabelStyle.ForeColor = new("#6e7780");

        // tell the candles and grid lines to use the right axis
        CandlePlot.Axes.YAxis = formsPlot1.Plot.Axes.Right;
        CandlePlot.Sequential = true;
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
            var candle = rp.Plot.GetPlottables<CandlestickPlot>().FirstOrDefault();
            if (candle is null)
                return;

            CoordinateRange priceRange = candle.GetPriceRangeInView();
            rp.Plot.Axes.Right.Range.Set(priceRange);
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
        if (CandlePlot is null)
            return;

        double candleWidthPx = formsPlot1.Plot.LastRender.DataRect.Width / formsPlot1.Plot.Axes.Bottom.Range.Span;
        double sec = formsPlot1.Plot.LastRender.Elapsed.TotalSeconds;
        formsPlot1.Plot.Title($"Render time: {sec * 1000:0} ms ({1 / sec:0.0} FPS)\n" +
            $"Candle width: {candleWidthPx:0.00} px");

        Coordinates mouseCoordinates = formsPlot1.Plot.GetCoordinates(e.X, e.Y, formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Right);

        if (LineBeingAdded is not null)
        {
            Crosshair.IsVisible = false;

            // the second click hasn't happened yet so place the second point where the cursor is
            LineBeingAdded.End = mouseCoordinates;

            // request a redraw
            formsPlot1.Refresh();
            return;
        }

        // TODO: move this logic inside the plottable
        var mouseCandle = CandlePlot.GetOhlcNearX(mouseCoordinates.X);

        if (mouseCandle is null)
        {
            bool refreshNeeded = Crosshair.IsVisible;
            Crosshair.IsVisible = false;
            if (refreshNeeded)
                formsPlot1.Refresh();
            return;
        }

        // TODO: use the axis to format the date using the same units as the ticks
        var fa = formsPlot1.Plot.GetPlottables<FinancialTimeAxis>().First();
        DateTime dateUnderMouse = fa.DateTimes[mouseCandle.Value.index];

        Crosshair.IsVisible = true;
        Crosshair.Position = new(mouseCandle.Value.index, mouseCoordinates.Y);
        Crosshair.VerticalLine.LabelText = dateUnderMouse.ToShortDateString();
        Crosshair.HorizontalLine.LabelText = $"{mouseCoordinates.Y:N2}";
        formsPlot1.Refresh();
    }
}
