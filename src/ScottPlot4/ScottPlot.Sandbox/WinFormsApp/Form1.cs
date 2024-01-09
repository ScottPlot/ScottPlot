using ScottPlot;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        List<double> xs = new();
        List<double> ys = new();

        xs.AddRange(Generate.Consecutive(1000, first: 0));
        ys.AddRange(Generate.Random(1000));

        xs.AddRange(Generate.Consecutive(1000, first: 10_000));
        ys.AddRange(Generate.Random(1000));

        ScottPlot.Plottable.SignalXY_EXPERIMENTAL sig = new()
        {
            Xs = xs.ToArray(),
            Ys = ys.ToArray(),
        };

        formsPlot1.Plot.Add(sig);
        formsPlot1.Refresh();
    }
}
