using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var xs = new double[] { 1, 2, 3, 4, 5 };
            var ys = new double[] { 1, 4, 9, 16, 25 };

            formsPlot1.plt.PlotScatter(xs, ys);
            formsPlot1.Render();
        }
    }
}
