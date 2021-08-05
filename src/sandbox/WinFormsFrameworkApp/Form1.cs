using ScottPlot.Plottable;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(100));
            var vline = formsPlot1.Plot.AddVerticalLine(50);
            vline.DragEnabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            formsPlot1.Plot.AxisAutoX();
            formsPlot1.Render();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            formsPlot1.Plot.AxisAutoY();
            formsPlot1.Render();
        }
    }
}
