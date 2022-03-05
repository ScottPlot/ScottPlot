using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for Layout.xaml
    /// </summary>
    public partial class Layout : Window
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

            this.SizeChanged += Layout_SizeChanged1;
            mainPlot.AxesChanged += MainPlot_AxesChanged;
        }

        private void MainPlot_AxesChanged(object sender, EventArgs e) => UpdateChildPlots();

        private void Layout_SizeChanged1(object sender, SizeChangedEventArgs e) => UpdateChildPlots();

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
