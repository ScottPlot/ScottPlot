using System;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ScottPlot.RandomDataGenerator gen = new();
            double[] xs = gen.Random(100);
            double[] ys = gen.Random(100);

            formsPlot1.Plot.AddScatterPoints(xs, ys, markerSize: 20);
            formsPlot1.Refresh();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.RightClickDragZoomFromMouseDown = checkBox1.Checked;
        }
    }
}
