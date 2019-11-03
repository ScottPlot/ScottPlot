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
    public partial class FormBillion : Form
    {
        byte[] oneBillionPoints;

        public FormBillion()
        {
            InitializeComponent();
            formsPlot1.Visible = false;
            formsPlot1.plt.Title("Interactive Display of ONE BILLION Data Points");
            formsPlot1.plt.Benchmark();
            label1.Text = "";
        }

        private void GenerateData()
        {
            label1.Text = "allocating memory";
            oneBillionPoints = new byte[1_000_000_000];

            double density = 50.0;
            for (int i = 0; i < oneBillionPoints.Length; i++)
            {
                if (i % 100_000 == 0)
                {
                    double percentComplete = 100 * (double)i / oneBillionPoints.Length;
                    label1.Text = string.Format("creating data ({0:0.00}%)", percentComplete);
                    Application.DoEvents();
                }

                double t = (double)i / oneBillionPoints.Length * density;
                double tSquared = Math.Pow(t, 2);
                oneBillionPoints[i] = (byte)(Math.Sin(tSquared) * 120 + 128);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            GenerateData();

            label1.Text = "calculating trees...";
            Application.DoEvents();

            formsPlot1.plt.Clear();
            formsPlot1.plt.PlotSignalConst(oneBillionPoints);
            formsPlot1.Render();

            label1.Text = "Data generation complete.";
            button1.Enabled = true;
            formsPlot1.Visible = true;
        }
    }
}
