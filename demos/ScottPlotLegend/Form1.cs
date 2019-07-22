using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Legend
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] legendLocationStrings = Enum.GetNames(typeof(ScottPlot.legendLocation));
            cbLocations.Items.AddRange(legendLocationStrings);
            cbLocations.SelectedItem = cbLocations.Items[0];

            Random rand = new Random();
            int pointCount = 100;
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] ys1 = ScottPlot.DataGen.RandomWalk(rand, pointCount);
            double[] ys2 = ScottPlot.DataGen.RandomWalk(rand, pointCount);
            double[] ys3 = ScottPlot.DataGen.RandomWalk(rand, pointCount);
            scottPlotUC1.plt.PlotScatter(xs, ys1, label: "one");
            scottPlotUC1.plt.PlotScatter(xs, ys2, label: "two");
            scottPlotUC1.plt.PlotScatter(xs, ys3, label: "three");
            scottPlotUC1.Render();
        }

        private void CbLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            string locationString = cbLocations.SelectedItem.ToString();
            ScottPlot.legendLocation location = (ScottPlot.legendLocation)Enum.Parse(typeof(ScottPlot.legendLocation), locationString);
            Console.WriteLine($"legend location: {location}");
            scottPlotUC1.plt.Legend(location: location);
            scottPlotUC1.Render();
        }
    }
}
