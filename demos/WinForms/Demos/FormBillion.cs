using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            if (!Environment.Is64BitProcess)
            {
                var result = MessageBox.Show(
                    "This demo is was built for 32-bit but requires 64-bit to function properly. " +
                    "Plots may render but will be EXTREMELY slow.\n\nDo you wish to continue?",
                    "x64 mode required",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                    );
                if (result == DialogResult.No)
                    Load += (s, e) => Close();
            }
        }

        private async Task GenerateData()
        {
            label1.Text = "allocating memory";
            oneBillionPoints = new byte[1_000_000_000];
            double density = 50;
            int processedCount = 0;

            var progress = new Progress<double>();
            progress.ProgressChanged += (sender, percent) => label1.Text = string.Format("creating data ({0:0.00}%)", percent);
            IProgress<double> progressReporter = progress;

            await Task.Run(() =>
            {
                Parallel.For(0, oneBillionPoints.Length, i =>
                {
                    if (i % 1_000_000 == 0)
                    {
                        Interlocked.Increment(ref processedCount);
                        double percentComplete = 1_000_000 * 100 * (double)processedCount / oneBillionPoints.Length;
                        progressReporter.Report(percentComplete);
                    }

                    double t = (double)i / oneBillionPoints.Length * density;
                    oneBillionPoints[i] = (byte)(Math.Sin(t * t) * 120 + 128);
                });
            });
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            await GenerateData();

            label1.Text = "calculating trees...";
            Application.DoEvents();

            formsPlot1.plt.Clear();
            var plot = formsPlot1.plt.PlotSignalConst(oneBillionPoints);
            if (plot.TreesReady == false)
            {
                label1.Text = "calculating trees - failed";
                var result = MessageBox.Show(
                    "Trees were not calculated because your system is not x64 or does not have 3GB of free memory.\n\n" +
                    "It may still be possible to render a plot but it will be be EXTREMELY slow.\n\nDo you want to continue?",
                    "Calculation Error",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    Close();
                    return;
                }
            }
            formsPlot1.Render();

            label1.Text = "Data generation complete.";
            button1.Enabled = true;
            formsPlot1.Visible = true;
        }
    }
}
