using ScottPlot.Plottable;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        AxisLine VLine;

        public Form1()
        {
            InitializeComponent();
            var sig1 = formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51));
            sig1.LineWidth = 20;

            VLine = formsPlot1.Plot.AddVerticalLine(13);
            VLine.DragEnabled = true;
            VLine.LineWidth = 5;
        }

        private void button1_Click(object sender, EventArgs e) => formsPlot1.Plot.MoveFirst(VLine);

        private void button2_Click(object sender, EventArgs e) => formsPlot1.Plot.MoveLast(VLine);
    }
}
