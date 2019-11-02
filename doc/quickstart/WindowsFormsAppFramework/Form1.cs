using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppFramework
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            formsPlot1.plt.PlotSignal(ScottPlot.DataGen.Sin(100));
            formsPlot1.plt.PlotSignal(ScottPlot.DataGen.Cos(100));
            formsPlot1.plt.Title("ScottPlot WinForms Quickstart");
            formsPlot1.Render();
        }
    }
}
