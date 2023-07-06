using System;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            formsPlot1.Plot.AddSignal(ScottPlot.Generate.Sin());
            formsPlot1.Plot.AddSignal(ScottPlot.Generate.Cos());
            formsPlot1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ScottPlot.FormsPlotViewer(formsPlot1, parentUpdatesChld: true).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new ScottPlot.FormsPlotViewer(formsPlot1, childUpdatesParent: true).Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new ScottPlot.FormsPlotViewer(formsPlot1, parentUpdatesChld: true, childUpdatesParent: true).Show();
        }
    }
}
