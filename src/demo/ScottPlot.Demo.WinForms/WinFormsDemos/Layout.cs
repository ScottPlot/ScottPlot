using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class Layout : Form
    {
        public Layout()
        {
            InitializeComponent();

            // generate sample data
            Random rand = new Random(0);
            int[] xs = DataGen.RandomNormal(rand, 3000, 20, 10).Select(x => (int)x).ToArray();
            int[] ys = DataGen.RandomNormal(rand, 3000, 20, 10).Select(y => (int)y).ToArray();
            double[,] intensities = Tools.XYToIntensities(mode: IntensityMode.Gaussian,
                xs: xs, ys: ys, width: 100, height: 70, sampleWidth: 4);

            // main plot
            var hmc = mainPlot.Plot.AddHeatmap(intensities, lockScales: false);
            var cb = mainPlot.Plot.AddColorbar(hmc);
            mainPlot.Plot.Margins(0, 0);
            mainPlot.Plot.Title("Control Rod\nTemperature");
            mainPlot.Plot.XLabel("Horizontal Position");
            mainPlot.Plot.YLabel("Vertical Position");
            mainPlot.Refresh();

            // right plot
            double[] rowSums = SumHorizontally(intensities).Reverse().ToArray();
            var rightBars = rightPlot.Plot.AddBar(rowSums);
            rightBars.Orientation = Orientation.Horizontal;
            rightBars.PositionOffset = .5;
            rightPlot.Plot.Margins(0, 0);
            rightPlot.Refresh();

            // lower plot
            double[] colSums = SumVertically(intensities);
            var lowerBars = lowerPlot.Plot.AddBar(colSums);
            lowerBars.PositionOffset = .5;
            lowerPlot.Plot.Margins(0, 0);
            lowerPlot.Refresh();

            UpdateChildPlots();
        }

        private void mainPlot_AxesChanged(object sender, EventArgs e) => UpdateChildPlots();

        private void Layout_SizeChanged(object sender, EventArgs e) => UpdateChildPlots();

        private void UpdateChildPlots()
        {
            lowerPlot.Plot.MatchAxis(mainPlot.Plot, horizontal: true, vertical: false);
            rightPlot.Plot.MatchAxis(mainPlot.Plot, horizontal: false, vertical: true);

            lowerPlot.Plot.MatchLayout(mainPlot.Plot, horizontal: true, vertical: false);
            rightPlot.Plot.MatchLayout(mainPlot.Plot, horizontal: false, vertical: true);

            lowerPlot.Refresh();
            rightPlot.Refresh();
        }

        private double[] SumHorizontally(double[,] data)
        {
            double[] sums = new double[data.GetLength(0)];
            for (int y = 0; y < data.GetLength(0); y++)
            {
                double sum = 0;
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    sum += data[y, x];
                }
                sums[y] = sum; ;
            }
            return sums;
        }

        private double[] SumVertically(double[,] data)
        {
            double[] sums = new double[data.GetLength(1)];
            for (int x = 0; x < data.GetLength(1); x++)
            {
                double sum = 0;
                for (int y = 0; y < data.GetLength(0); y++)
                {
                    sum += data[y, x];
                }
                sums[x] = sum;
            }
            return sums;
        }
    }
}
