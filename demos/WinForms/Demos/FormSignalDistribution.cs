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

            Color[] colorMap = new Color[]
            {
                Color.LightGreen,
                Color.Green,
                Color.Blue,
                Color.Red,
                Color.Blue,
                Color.Green,
                Color.LightGreen
            };

            formsPlot1.plt.PlotSignal(SinWithNormalNoise, colorMap: colorMap);

            formsPlot1.plt.Title("100,000 Data Points with 7 levels Distribution");
            formsPlot1.plt.YLabel("Sin with heavy gaussian noise");
            formsPlot1.plt.XLabel("Array Index");
            formsPlot1.Render();
        }
    }
}
