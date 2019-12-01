using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppCore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // generate some data
            int pointCount = 100;
            double[] xs = new double[pointCount];
            double[] ys = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                xs[i] = i;
                ys[i] = Math.Pow(i, .5);
            }

            // plot the data
            formsPlot1.plt.PlotScatter(xs, ys);
            formsPlot1.Render();
        }
    }
}
