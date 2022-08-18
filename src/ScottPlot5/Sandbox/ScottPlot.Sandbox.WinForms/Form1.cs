using ScottPlot.Plottables;
using System;
using System.Windows.Forms;

namespace ScottPlot.Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

    }

    private void button2_Click(object sender, EventArgs e) => Add(10_000);
    private void button1_Click(object sender, EventArgs e) => Add(1_000_000);
    private void button3_Click(object sender, EventArgs e) => Add(10_000_000);

    private void Add(int count)
    {
        formsPlot1.Plot.Plottables.Clear();
        double[] ys = Generate.NoisySin(new Random(0), count);
        formsPlot1.Plot.Plottables.AddSignal(ys);
        formsPlot1.Plot.AutoScale();
        formsPlot1.Refresh();
    }
}
