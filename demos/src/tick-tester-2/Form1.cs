using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tick_tester_2
{
    public partial class Form1 : Form
    {
        double bigNumber = Math.Pow(23.456, 9);
        double smallNumber = Math.Pow(23.456, -9);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            scottPlotUC1.plt.PlotSignal(ScottPlot.DataGen.Sin(100));
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            scottPlotUC1.plt.Axis(-bigNumber, bigNumber, null, null);
            scottPlotUC1.Render();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            scottPlotUC1.plt.Axis(9876 * bigNumber, 9877 * bigNumber, null, null);
            scottPlotUC1.Render();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            scottPlotUC1.plt.Axis(-smallNumber, smallNumber, null, null);
            scottPlotUC1.Render();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            scottPlotUC1.plt.Axis(9876 * smallNumber, 9877 * smallNumber, null, null);
            scottPlotUC1.Render();
        }
    }
}
