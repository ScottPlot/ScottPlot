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
            formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51));
            formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Cos(51));
            formsPlot1.Refresh();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ScottPlot.Drawing.GDI.ClearType(checkBox1.Checked);
            formsPlot1.Refresh();
        }
    }
}
