using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using ScottPlot.Plottable;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    public partial class ToggleVisibility : Form
    {
        public ToggleVisibility()
        {
            InitializeComponent();

            this.cbSin.CheckedChanged += this.cbSin_CheckedChanged;
            this.cbCos.CheckedChanged += this.cbCos_CheckedChanged;
            this.cbLines.CheckedChanged += this.cbLines_CheckedChanged;
            this.Load += this.ToggleVisibility_Load;
        }

        ScatterPlot sinPlot, cosPlot;
        VLine vline1, vline2;

        private void ToggleVisibility_Load(object sender, EventArgs e)
        {
            int pointCount = 51;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            sinPlot = formsPlot1.Plot.AddScatter(dataXs, dataSin);
            cosPlot = formsPlot1.Plot.AddScatter(dataXs, dataCos);
            vline1 = formsPlot1.Plot.AddVerticalLine(0);
            vline2 = formsPlot1.Plot.AddVerticalLine(50);

            formsPlot1.Refresh();
        }

        private void cbSin_CheckedChanged(object sender, EventArgs e)
        {
            sinPlot.IsVisible = cbSin.Checked ?? false;
            formsPlot1.Refresh();
        }

        private void cbCos_CheckedChanged(object sender, EventArgs e)
        {
            cosPlot.IsVisible = cbCos.Checked ?? false;
            formsPlot1.Refresh();
        }

        private void cbLines_CheckedChanged(object sender, EventArgs e)
        {
            vline1.IsVisible = cbLines.Checked ?? false;
            vline2.IsVisible = cbLines.Checked ?? false;
            formsPlot1.Refresh();
        }
    }
}
