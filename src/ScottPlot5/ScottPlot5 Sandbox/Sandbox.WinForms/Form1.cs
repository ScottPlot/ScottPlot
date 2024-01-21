using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        var ohlcs = Generate.RandomOHLCs(12_000);
        formsPlot1.Plot.Add.Candlestick(ohlcs);
    }
}
