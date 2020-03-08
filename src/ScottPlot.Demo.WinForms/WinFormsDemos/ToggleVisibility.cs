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
    public partial class ToggleVisibility : Form
    {
        public ToggleVisibility()
        {
            InitializeComponent();
        }

        PlottableScatter sinPlot, cosPlot;
        PlottableVLine vline1, vline2;

        private void ToggleVisibility_Load(object sender, EventArgs e)
        {
            int pointCount = 51;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            sinPlot = formsPlot1.plt.PlotScatter(dataXs, dataSin);
            cosPlot = formsPlot1.plt.PlotScatter(dataXs, dataCos);
            vline1 = formsPlot1.plt.PlotVLine(0);
            vline2 = formsPlot1.plt.PlotVLine(50);

            formsPlot1.Render();
        }

        private void cbSin_CheckedChanged(object sender, EventArgs e)
        {
            sinPlot.visible = cbSin.Checked;
            formsPlot1.Render();
        }

        private void cbCos_CheckedChanged(object sender, EventArgs e)
        {
            cosPlot.visible = cbCos.Checked;
            formsPlot1.Render();
        }

        private void cbLines_CheckedChanged(object sender, EventArgs e)
        {
            vline1.visible = cbLines.Checked;
            vline2.visible = cbLines.Checked;
            formsPlot1.Render();
        }
    }
}
