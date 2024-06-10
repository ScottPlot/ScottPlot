using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        double[] ys = Generate.SquareWave(5, 3);
        double[] xs = Generate.Consecutive(ys.Length);

        var sig1 = formsPlot1.Plot.Add.SignalXY(xs, ys);
        sig1.MarkerSize = 5;

        var sig2 = formsPlot1.Plot.Add.SignalXY(xs, ys);
        sig2.Data.YOffset = 2;
        sig2.Data.XOffset = .2;
        sig2.ConnectStyle = ConnectStyle.StepHorizontal;
        sig2.MarkerSize = 5;

        var sig3 = formsPlot1.Plot.Add.SignalXY(xs, ys);
        sig3.Data.YOffset = 4;
        sig3.Data.XOffset = .4;
        sig3.ConnectStyle = ConnectStyle.StepVertical;
        sig3.MarkerSize = 5;
    }
}
