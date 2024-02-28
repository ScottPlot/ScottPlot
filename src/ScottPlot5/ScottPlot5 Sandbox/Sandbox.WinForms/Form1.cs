using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        var ohlcs = Generate.RandomOHLCs(12_000);
        formsPlot1.Plot.Add.Candlestick(ohlcs);

        var label = formsPlot1.Plot.Add.LabelPlot("Test", 55_000, 300, 55_000, 320);

        label.Label.BorderColor = Colors.Blue;
        label.Label.BackColor = Colors.Blue.WithAlpha(.5);
        label.Label.Padding = 5;

        label.MarkerStyle = new()
        {
            IsVisible = true,
            Shape = MarkerShape.FilledTriangleDown,
            Size = 10
        };
    }
}
