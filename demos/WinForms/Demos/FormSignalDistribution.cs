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
    public partial class FormSignalDistribution : Form
    {
        public FormSignalDistribution()
        {
            InitializeComponent();
            Random rand = new Random();

            int pointsCount = 100_000;

            double[] SinWithNormalNoise = ScottPlot.DataGen.Sin(pointsCount, 1)
                .Zip(ScottPlot.DataGen.RandomNormal(rand, pointsCount, 0, 5), (s, n) => s + n)
                .ToArray();

            // start with a few samples from the viridis palette
            Color[] colors = new Color[]
            {
                ColorTranslator.FromHtml("#440154"),
                ColorTranslator.FromHtml("#39568C"),
                ColorTranslator.FromHtml("#1F968B"),
                ColorTranslator.FromHtml("#73D055"),
            };

            // feed a distribution colormap
            formsPlot1.plt.PlotSignal(SinWithNormalNoise, colorByDensity: colors);

            formsPlot1.plt.Title("100,000 Data Points with 7 levels Distribution");
            formsPlot1.plt.YLabel("Sin with heavy gaussian noise");
            formsPlot1.plt.XLabel("Array Index");
            formsPlot1.Render();
        }
    }
}
