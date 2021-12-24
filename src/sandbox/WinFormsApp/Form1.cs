using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var sig1 = formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51));
            sig1.XAxisIndex = 0;
            sig1.YAxisIndex = 0;
            sig1.Label = "primary";

            var sig2 = formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Cos(51));
            sig2.XAxisIndex = 1;
            sig2.YAxisIndex = 1;
            sig2.Label = "secondary";

            formsPlot1.Plot.Legend();

            formsPlot1.Refresh();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            if (cb1.Checked)
                formsPlot1.Plot.AxisAutoX(xAxisIndex: 0);

            if (cb2.Checked)
                formsPlot1.Plot.AxisAutoX(xAxisIndex: 1);

            formsPlot1.Refresh();
        }

        private void btnY_Click(object sender, EventArgs e)
        {
            if (cb1.Checked)
                formsPlot1.Plot.AxisAutoY(yAxisIndex: 0);

            if (cb2.Checked)
                formsPlot1.Plot.AxisAutoY(yAxisIndex: 1);

            formsPlot1.Refresh();
        }

        private void btnXY_Click(object sender, EventArgs e)
        {
            if (cb1.Checked)
                formsPlot1.Plot.AxisAuto(null, null, xAxisIndex: 0, yAxisIndex: 0);

            if (cb2.Checked)
                formsPlot1.Plot.AxisAuto(null, null, xAxisIndex: 1, yAxisIndex: 1);

            formsPlot1.Refresh();
        }
    }
}
