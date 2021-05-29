using ScottPlot.Plottable;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Cos(51, offset: new Random().NextDouble()));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var plottables = formsPlot1.Plot.GetPlottables();
            if (plottables.Length == 0)
                return;

            formsPlot1.Plot.Remove(plottables[0]);

            formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Cos(51, offset: new Random().NextDouble()));
        }
    }
}
