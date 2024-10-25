using ScottPlot;

namespace Sandbox.WinFormsFinance;

public partial class TradingViewForm : Form
{
    public TradingViewForm()
    {

        InitializeComponent();
        InitializeSymbolComboBox();
        InitializeIntervalComboBox();
        InitializePlot();
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
}
