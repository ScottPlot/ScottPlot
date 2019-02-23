using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormStyle : Form
    {
        public FormStyle()
        {
            InitializeComponent();
        }

        private double[] RandomWalkValues(int pointCount = 100)
        {
            double[] ys = new double[pointCount];
            Random rand = new Random();
            for (int i = 0; i < pointCount; i++)
            {
                if (i == 0)
                    ys[i] = rand.NextDouble() * 20 - 10;
                else
                    ys[i] = ys[i - 1] + rand.NextDouble() * 2 - 1;
            }
            return ys;
        }

        private void btnLight_Click(object sender, EventArgs e)
        {
            scottPlotUC1.Reset();
            scottPlotUC1.plt.data.AddScatter(null, RandomWalkValues());
            scottPlotUC1.plt.settings.AxisFit();

            scottPlotUC1.Render();
        }

        private void btnDark_Click(object sender, EventArgs e)
        {
            scottPlotUC1.Reset();
            scottPlotUC1.plt.data.AddScatter(null, RandomWalkValues(), markerColor: Color.Yellow, lineColor: Color.Yellow);
            scottPlotUC1.plt.settings.AxisFit();

            scottPlotUC1.plt.settings.title = "Dark Style";
            scottPlotUC1.plt.settings.figureBgColor = Color.DarkGray;
            scottPlotUC1.plt.settings.dataBgColor = Color.Gray;
            scottPlotUC1.plt.settings.gridColor = Color.Black;

            scottPlotUC1.Render();
        }

        private void btnWindows_Click(object sender, EventArgs e)
        {
            scottPlotUC1.Reset();
            scottPlotUC1.plt.data.AddScatter(null, RandomWalkValues());
            scottPlotUC1.plt.settings.AxisFit();

            scottPlotUC1.plt.settings.title = "Windows Colors";
            scottPlotUC1.plt.settings.figureBgColor = SystemColors.Control;
            scottPlotUC1.plt.settings.dataBgColor = SystemColors.Control;
            scottPlotUC1.plt.settings.gridColor = SystemColors.ControlLight;

            scottPlotUC1.Render();
        }

        private void btnSmallAxes_Click(object sender, EventArgs e)
        {
            scottPlotUC1.Reset();
            scottPlotUC1.plt.data.AddScatter(null, RandomWalkValues());
            scottPlotUC1.plt.settings.AxisFit();

            scottPlotUC1.plt.settings.figureBgColor = SystemColors.Control;
            scottPlotUC1.plt.settings.title = "";
            scottPlotUC1.plt.settings.SetDataPadding(40, 2, 40, 2);
            scottPlotUC1.plt.settings.fontAxis = new Font("Arial", 8);
            scottPlotUC1.plt.settings.fontTicks = new Font("Arial", 6);

            scottPlotUC1.Render();
        }

        private void btnBorderless_Click(object sender, EventArgs e)
        {
            scottPlotUC1.Reset();
            scottPlotUC1.plt.data.AddScatter(null, RandomWalkValues());
            scottPlotUC1.plt.settings.AxisFit();

            scottPlotUC1.plt.settings.figureBgColor = SystemColors.Control;
            scottPlotUC1.plt.settings.title = "";
            scottPlotUC1.plt.settings.axisLabelX = "";
            scottPlotUC1.plt.settings.axisLabelY = "";
            scottPlotUC1.plt.settings.SetDataPadding(0, 1, 1, 0);
            //scottPlotUC1.plt.settings.gridColor = 
            //scottPlotUC1.plt.settings.drawAxes = false;

            scottPlotUC1.Render();
        }
    }
}
