using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsPlotSandbox
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {
            //Test_Viewer();
        }

        private void Test_Viewer()
        {
            // create a styled plot
            var plt = new ScottPlot.Plot();
            plt.Style(ScottPlot.Style.Blue1);
            plt.PlotSignal(ScottPlot.DataGen.Sin(51));
            plt.PlotSignal(ScottPlot.DataGen.Cos(51));
            plt.SaveFig("test.png");

            // launch it in a new window
            new ScottPlot.FormsPlotViewer(plt).ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new FormSandbox().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new FormSlowPan().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new FormMouse().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new FormExperimentalTick().ShowDialog();
        }
    }
}
