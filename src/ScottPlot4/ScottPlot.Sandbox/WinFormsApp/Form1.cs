using System;
using ScottPlot;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Random rand = new(0);
            var popSeries = new ScottPlot.Statistics.PopulationSeries[10];
            for (int i = 0; i < popSeries.Length; i++)
            {
                double[] values = DataGen.RandomNormal(rand, 320);
                ScottPlot.Statistics.Population[] populations = { new(values) };
                popSeries[i] = new(populations, $"Pop {i + 1}");
            }

            var multiSeries = new ScottPlot.Statistics.PopulationMultiSeries(popSeries);

            formsPlot1.Plot.Frameless();
            formsPlot1.Plot.AddPopulations(multiSeries);
            formsPlot1.Refresh();
        }
    }
}
