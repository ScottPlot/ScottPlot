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

            Random rand = new Random(0);
            formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51));
            formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Cos(51));
            numericUpDown1_ValueChanged(null, null);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            formsPlot1.Plot.AxisAuto(horizontalMargin: (double)numericUpDown1.Value);
            formsPlot1.Refresh();
        }
    }
}
