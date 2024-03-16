namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin());
        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos());

        button1.Click += (s, e) =>
        {
            formsPlot1.Plot.Axes.Left.TickLabelStyle.FontSize = 64;
            formsPlot1.Plot.Axes.Bottom.TickLabelStyle.FontSize = 64;
            formsPlot1.Refresh();
        };

        button2.Click += (s, e) =>
        {
            formsPlot1.Reset();
            formsPlot1.Refresh();
        };
    }
}
