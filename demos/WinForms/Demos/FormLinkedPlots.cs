using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormLinkedPlots : Form
    {
        public FormLinkedPlots()
        {
            InitializeComponent();
        }

        private void FormLinkedPlots_Load(object sender, EventArgs e)
        {
            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            formsPlot1.plt.PlotScatter(dataXs, dataSin, color: Color.Blue);
            formsPlot1.Render();

            formsPlot2.plt.PlotScatter(dataXs, dataCos, color: Color.Red);
            formsPlot2.Render();
        }

        private void formsPlot1_AxesChanged(object sender, EventArgs e)
        {
            formsPlot2.plt.MatchAxis(formsPlot1.plt);
            formsPlot2.Render();
        }

        private void formsPlot2_AxesChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.MatchAxis(formsPlot2.plt);
            formsPlot1.Render();
        }
    }
}
