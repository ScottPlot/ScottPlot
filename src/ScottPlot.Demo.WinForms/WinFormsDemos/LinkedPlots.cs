using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class LinkedPlots : Form
    {
        public LinkedPlots()
        {
            InitializeComponent();
        }

        private void LinkedPlots_Load(object sender, EventArgs e)
        {
            int pointCount = 51;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            formsPlot1.plt.PlotScatter(dataXs, dataSin);
            formsPlot1.Render();

            formsPlot2.plt.PlotScatter(dataXs, dataCos);
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
