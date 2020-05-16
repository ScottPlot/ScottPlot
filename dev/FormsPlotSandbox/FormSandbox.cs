using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsPlotSandbox
{
    public partial class FormSandbox : Form
    {
        public FormSandbox()
        {
            InitializeComponent();

            double[] dataX = { 0, 0, 0 };
            double[] dataY = { 0, 0, 0 };
            formsPlot1.plt.PlotScatter(dataX, dataY);
            formsPlot1.Render();
        }
    }
}
