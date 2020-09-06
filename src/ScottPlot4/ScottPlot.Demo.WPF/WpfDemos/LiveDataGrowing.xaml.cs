using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for LiveDataGrowing.xaml
    /// </summary>
    public partial class LiveDataGrowing : Window
    {
        public double[] data = new double[100_000];
        int nextDataIndex = 1;
        PlottableSignal signalPlot;
        Random rand = new Random(0);

        public LiveDataGrowing()
        {
            InitializeComponent();

            // plot the data array only once
            signalPlot = wpfPlot1.plt.PlotSignal(data);
            wpfPlot1.plt.YLabel("Value");
            wpfPlot1.plt.XLabel("Sample Number");

            // create a timer to modify the data
            DispatcherTimer updateDataTimer = new DispatcherTimer();
            updateDataTimer.Interval = TimeSpan.FromMilliseconds(1);
            updateDataTimer.Tick += UpdateData;
            updateDataTimer.Start();

            // create a timer to update the GUI
            DispatcherTimer renderTimer = new DispatcherTimer();
            renderTimer.Interval = TimeSpan.FromMilliseconds(20);
            renderTimer.Tick += Render;
            renderTimer.Start();
        }

        void UpdateData(object sender, EventArgs e)
        {
            if (nextDataIndex >= data.Length)
            {
                throw new OverflowException("data array isn't long enough to accomodate new data");
                // in this situation the solution would be:
                //   1. clear the plot
                //   2. create a new larger array
                //   3. copy the old data into the start of the larger array
                //   4. plot the new (larger) array
                //   5. continue to update the new array
            }

            double randomValue = Math.Round(rand.NextDouble() - .5, 3);
            double latestValue = data[nextDataIndex - 1] + randomValue;
            data[nextDataIndex] = latestValue;
            signalPlot.maxRenderIndex = nextDataIndex;
            ReadingsTextbox.Text = $"{nextDataIndex + 1}";
            LatestValueTextbox.Text = $"{latestValue:0.000}";
            nextDataIndex += 1;
        }

        void Render(object sender, EventArgs e)
        {
            if (AutoAxisCheckbox.IsChecked == true)
                wpfPlot1.plt.AxisAuto();
            wpfPlot1.Render(skipIfCurrentlyRendering: true);
        }

        private void DisableAutoAxis(object sender, RoutedEventArgs e)
        {
            double[] autoAxisLimits = wpfPlot1.plt.AxisAuto(verticalMargin: .5);
            double oldX2 = autoAxisLimits[1];
            wpfPlot1.plt.Axis(x2: oldX2 + 1000);
        }
    }
}
