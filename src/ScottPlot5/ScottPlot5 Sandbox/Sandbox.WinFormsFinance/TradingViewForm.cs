using ScottPlot;

namespace Sandbox.WinFormsFinance;

public partial class TradingViewForm : Form
{
    // TODO: make an abstraction for click-drag placement of new technical indicators
    ScottPlot.Plottables.LinePlot? LineBeingAdded = null;

    public TradingViewForm()
    {
        InitializeComponent();
        InitializeSymbolComboBox();
        InitializeIntervalComboBox();
        InitializePlot();

        buttonClearAll.Click += ButtonClearAll_Click;

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
    }

    void InitializeSymbolComboBox()
    {
        comboBoxSymbol.Items.Add("AAPL");
        comboBoxSymbol.Items.Add("AMZN");
        comboBoxSymbol.Items.Add("GOOGL");
        comboBoxSymbol.Items.Add("META");
        comboBoxSymbol.Items.Add("MSFT");
        comboBoxSymbol.Items.Add("NVDA");
        comboBoxSymbol.SelectedIndex = 0;
    }

    void InitializeIntervalComboBox()
    {
        comboBoxInterval.Items.Add("1 second");
        comboBoxInterval.Items.Add("5 seconds");
        comboBoxInterval.Items.Add("10 seconds");
        comboBoxInterval.Items.Add("30 seconds");
        comboBoxInterval.Items.Add("1 minute");
        comboBoxInterval.Items.Add("5 minutes");
        comboBoxInterval.Items.Add("10 minutes");
        comboBoxInterval.Items.Add("30 minutes");
        comboBoxInterval.Items.Add("1 hour");
        comboBoxInterval.Items.Add("2 hours");
        comboBoxInterval.Items.Add("4 hours");
        comboBoxInterval.Items.Add("1 day");
        comboBoxInterval.Items.Add("1 week");
        comboBoxInterval.Items.Add("1 month");
        comboBoxInterval.Items.Add("1 year");
        comboBoxInterval.SelectedIndex = 5;
    }

    void InitializePlot()
    {
        // generate random walk data
        List<OHLC> ohlcs = Generate.RandomOHLCs(100);

        // use dates according to the selected interval
        DateTime start = new(2024, 10, 24);
        TimeSpan interval = TimeSpan.FromSeconds(10); // TODO: read value from combo box
        for (int i = 0; i < ohlcs.Count; i++)
        {
            ohlcs[i] = ohlcs[i]
                .WithDate(start + interval * i)
                .WithTimeSpan(interval);
        }

        formsPlot1.Plot.Clear();
        formsPlot1.Plot.Axes.DateTimeTicksBottom();
        formsPlot1.Plot.Add.Candlestick(ohlcs);
        formsPlot1.Refresh();
    }

    private void ButtonClearAll_Click(object? sender, EventArgs e)
    {
        formsPlot1.Plot.Remove<ScottPlot.Plottables.LinePlot>();
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        if (!checkBoxAddLine.Checked)
            return;

        // disable mouse pan and zoom while click-dragging to add a new indicator
        formsPlot1.UserInputProcessor.Disable();

        Coordinates cs = formsPlot1.Plot.GetCoordinates(e.X, e.Y);

        // TODO: make a custom plot type and interface for technical indicators
        LineBeingAdded = formsPlot1.Plot.Add.Line(cs, cs);
        LineBeingAdded.LineWidth = 3;
        LineBeingAdded.LinePattern = LinePattern.DenselyDashed;
        LineBeingAdded.LineColor = Colors.Black;
        LineBeingAdded.MarkerShape = MarkerShape.FilledCircle;
        LineBeingAdded.MarkerFillColor = Colors.Black;
        LineBeingAdded.MarkerSize = 8;
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        if (LineBeingAdded is null)
            return;

        Coordinates cs = formsPlot1.Plot.GetCoordinates(e.X, e.Y);
        LineBeingAdded.End = cs;

        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        LineBeingAdded = null;
        formsPlot1.Refresh();
        formsPlot1.UserInputProcessor.Reset();
        formsPlot1.UserInputProcessor.Enable();
    }
}
