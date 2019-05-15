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
    public partial class FormMisc : Form
    {
        private Random rand = new Random();

        public FormMisc()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            scottPlotUC1.plt.data.Clear();
            scottPlotUC1.Render();
        }

        private void btnAddXY_Click(object sender, EventArgs e)
        {
            List<double> xs = new List<double>();
            List<double> ys = new List<double>();

            for (int i = -10; i < 10; i++)
            {
                xs.Add(i);
                if (ys.Count == 0)
                    ys.Add(rand.NextDouble() * 10 - 5);
                else
                    ys.Add(ys[ys.Count() - 1] + rand.NextDouble() - .5);
            }

            scottPlotUC1.plt.data.AddScatter(xs.ToArray(), ys.ToArray());
            scottPlotUC1.plt.settings.AxisFit();
            scottPlotUC1.Render();
        }

        private void btnAddHline_Click(object sender, EventArgs e)
        {
            double y = rand.NextDouble() * 20 - 10;
            scottPlotUC1.plt.data.AddHorizLine(y);
            scottPlotUC1.Render();
        }

        private void buttonAddVline_Click(object sender, EventArgs e)
        {
            double x = rand.NextDouble() * 20 - 10;
            scottPlotUC1.plt.data.AddVertLine(x);
            scottPlotUC1.Render();
        }

        private void BtnAddText_Click(object sender, EventArgs e)
        {
            string[] randomWords = new string[] { "Gnathostomulida", "Rotifera",
                "Dicyemida", "Orthonectida", "Gastrotricha", "Platyhelminthes",
                "Hyolitha †", "Annelida", "Brachiopoda", "Bryozoa", "Cycliophora",
                "Entoprocta", "Mollusca", "Nemertea", "Phoronida" };
            int randomWordIndex = (int)(rand.NextDouble() * randomWords.Length);
            string randomWord = randomWords[randomWordIndex];
            double positionX = rand.NextDouble() * 10;
            double positionY = rand.NextDouble() * 10;
            //scottPlotUC1.plt.data.AddText(randomWord, positionX, positionY);
            scottPlotUC1.Render();
        }

        private void AddSignal(int pointCount)
        {
            double[] ys = new double[pointCount];
            double sampleRate = pointCount / 10.0;
            double phaseShift = rand.NextDouble();
            double amplitude = 10 * rand.NextDouble();
            for (int i = 0; i < pointCount; i++)
                ys[i] = Math.Sin(((double)(i) / pointCount + phaseShift) * Math.PI * 2 * 5) * amplitude;
            scottPlotUC1.plt.data.AddSignal(ys, sampleRate);
            scottPlotUC1.plt.settings.AxisFit();
            scottPlotUC1.Render();
        }

        private void btnAddSin1k_Click(object sender, EventArgs e)
        {
            AddSignal(1_000);
        }

        private void btnAddSin1M_Click(object sender, EventArgs e)
        {
            AddSignal(1_000_000);
        }

        private void btnAddSin10M_Click(object sender, EventArgs e)
        {
            AddSignal(10_000_000);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "demo.png";
            savefile.Filter = "PNG Files (*.png)|*.png|All files (*.*)|*.*";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                string saveFilePath = savefile.FileName;
                scottPlotUC1.plt.figure.Save(saveFilePath);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //scottPlotUC1.plt.settings.title = "ScottPlot Demo: Misc";
            //scottPlotUC1.plt.settings.displayBenchmark = true;
        }

        private void btnWebsite_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/swharden/ScottPlot");
        }

    }
}

