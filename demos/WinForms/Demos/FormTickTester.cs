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
    public partial class FormTickTester : Form
    {
        double bigNumber = Math.Pow(23.456, 9);
        double smallNumber = Math.Pow(23.456, -9);

        public FormTickTester()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            formsPlot1.plt.PlotSignal(ScottPlot.DataGen.Sin(100));
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(-bigNumber, bigNumber, null, null);
            formsPlot1.Render();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(9876 * bigNumber, 9877 * bigNumber, null, null);
            formsPlot1.Render();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(-smallNumber, smallNumber, null, null);
            formsPlot1.Render();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(9876 * smallNumber, 9877 * smallNumber, null, null);
            formsPlot1.Render();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                formsPlot1.plt.Grid(xSpacing: 5, ySpacing: .5);
            else
                formsPlot1.plt.Grid();
            formsPlot1.Render();
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.Ticks(useMultiplierNotation: checkBox2.Checked);
            formsPlot1.Render();
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.Ticks(useOffsetNotation: checkBox3.Checked);
            formsPlot1.Render();
        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.Ticks(useExponentialNotation: checkBox4.Checked);
            formsPlot1.Render();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.Ticks(displayTickLabelsY: checkBox5.Checked);
            formsPlot1.Render();
        }
    }
}
