using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    System.Windows.Forms.Timer Timer = new() { Interval = 10 };
    ScottPlot.DataGenerators.RandomWalker2D Walker = new();
    public Form1()
    {
        InitializeComponent();

        var logger = formsPlot1.Plot.Add.DataLogger();
        logger.Add(0, 0);
        logger.LineWidth = 2;

        Timer.Tick += (s, e) =>
        {
            Coordinates pt = Walker.Next();
            logger.Add(pt);
            formsPlot1.Refresh();
        };

        Timer.Start();

    }
}
