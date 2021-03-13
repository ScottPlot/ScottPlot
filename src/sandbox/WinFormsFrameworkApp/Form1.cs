using ScottPlot.Plottable;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            int sampleRate = 48_000;
            Random rand = new Random(0);
            double[] data = ScottPlot.DataGen.RandomWalk(rand, sampleRate * 10);
            formsPlot1.Plot.AddSignal(data, sampleRate);

            formsPlot1.Configuration.Quality = ScottPlot.Control.QualityMode.LowWhileDragging;
        }
    }
}
