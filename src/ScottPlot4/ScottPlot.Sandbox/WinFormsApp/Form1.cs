using System;
using ScottPlot;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            double[,] data = DataGen.Random2D(new Random(0), rows: 10, columns: 1 << 15);
            formsPlot1.Plot.AddHeatmap(data, lockScales: false);

            formsPlot1.Refresh();
        }
    }
}
