using ScottPlot.Plottables;
using System;
using System.Windows.Forms;

namespace ScottPlot.Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        double[] ys = Generate.NoisySin(new Random(0), 1000);
        formsPlot1.Plot.Plottables.AddSignal(ys);
    }
}
