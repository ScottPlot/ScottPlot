using ScottPlot.Plottable;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        private readonly Random Rand = new Random();

        public Form1()
        {
            InitializeComponent();
            AddSignal();
        }

        private void AddSignal() =>
            formsPlot1.Plot.AddSignal(
                ys: ScottPlot.DataGen.Cos(pointCount: 51, phase: new Random().NextDouble()),
                color: ScottPlot.DataGen.RandomColor(Rand));

        private void button1_Click(object sender, EventArgs e) => AddSignal();

        private void button2_Click(object sender, EventArgs e)
        {
            if (formsPlot1.Plot.GetPlottables().Length > 0)
                formsPlot1.Plot.RemoveAt(0);
            AddSignal();
        }
    }
}
