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
    public partial class FormPadding : Form
    {
        public FormPadding()
        {
            InitializeComponent();
        }

        private void FormPadding_Load(object sender, EventArgs e)
        {
            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            formsPlot1.plt.PlotScatter(dataXs, dataSin);
            formsPlot1.plt.PlotScatter(dataXs, dataCos);
            formsPlot1.plt.Grid(false);
            formsPlot1.plt.Style(figBg: Color.LightBlue); // aid visibility
            formsPlot1.Render();
        }

        private void btnBoth_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Frame(left: true, right: true, bottom: true, top: true);
            formsPlot1.plt.Ticks(displayTicksY: true, displayTicksX: true);
            formsPlot1.Render();
        }

        private void btnHoriz_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Frame(left: false, right: false, bottom: true, top: false);
            formsPlot1.plt.Ticks(displayTicksY: false, displayTicksX: true);
            formsPlot1.Render();
        }

        private void btnVert_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Frame(left: true, right: false, bottom: false, top: false);
            formsPlot1.plt.Ticks(displayTicksY: true, displayTicksX: false);
            formsPlot1.Render();
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Frame(left: false, right: false, bottom: false, top: false);
            formsPlot1.plt.Ticks(displayTicksY: false, displayTicksX: false);
            formsPlot1.Render();
        }
    }
}
