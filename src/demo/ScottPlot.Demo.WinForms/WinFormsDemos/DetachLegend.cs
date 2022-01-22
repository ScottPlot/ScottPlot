using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot.Plottable;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class DetachLegend : Form
    {
        public DetachLegend()
        {
            InitializeComponent();
        }

        private void DetachLegend_Load(object sender, EventArgs e)
        {
            int pointCount = 51;
            Random rand = new Random(0);
            double[] dataXs = DataGen.Consecutive(pointCount);
            for (int i = 0; i < 5; i++)
            {
                var scatter = formsPlot1.Plot.AddScatter(dataXs, DataGen.RandomWalk(rand, pointCount));
                scatter.Label = $"Label n°{i}";
            }
            formsPlot1.Plot.Legend();
            formsPlot1.Refresh();
        }
    }
}
