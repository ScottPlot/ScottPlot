using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UcSignal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ucSignal1.showBenchmark = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // create one million points of signal data
            int pointCount = 1_000_000;
            double[] Ys = new double[pointCount];
            for (int i = 0; i < pointCount; i++) Ys[i] = Math.Sin((double)i / 5000);
            ucSignal1.fig.title = string.Format("{0:n0} Data Points", pointCount);

            // load the data into the user control (and auto-fit axis limits to the data)
            ucSignal1.Ys = Ys; 
            ucSignal1.ResetAxis();
        }
    }
}
