using ScottPlot.Plottable;
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
        SignalPlot signalPlot;
        Random rand = new Random(0);

        private DispatcherTimer _updateDataTimer;
        private DispatcherTimer _renderTimer;

        public LiveDataGrowing()
        {
            InitializeComponent();

            // plot the data array only once
            signalPlot = wpfPlot1.Plot.AddSignal(data);
            wpfPlot1.Plot.YLabel("Value");
            wpfPlot1.Plot.XLabel("Sample Number");
            wpfPlot1.Refresh();

            // create a timer to modify the data
            _updateDataTimer = new DispatcherTimer();
            _updateDataTimer.Interval = TimeSpan.FromMilliseconds(1);
            _updateDataTimer.Tick += UpdateData;
            _updateDataTimer.Start();

            // create a timer to update the GUI
            _renderTimer = new DispatcherTimer();
            _renderTimer.Interval = TimeSpan.FromMilliseconds(20);
            _renderTimer.Tick += Render;
            _renderTimer.Start();

            Closed += (sender, args) =>
            {
                _updateDataTimer?.Stop();
                _renderTimer?.Stop();
            };
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
            signalPlot.MaxRenderIndex = nextDataIndex;
            ReadingsTextbox.Text = $"{nextDataIndex + 1}";
            LatestValueTextbox.Text = $"{latestValue:0.000}";
            nextDataIndex += 1;
        }

        void Render(object sender, EventArgs e)
        {
            if (AutoAxisCheckbox.IsChecked == true)
                wpfPlot1.Plot.AxisAuto();
            wpfPlot1.Refresh();
        }

        private void DisableAutoAxis(object sender, RoutedEventArgs e)
        {
            wpfPlot1.Plot.AxisAuto(verticalMargin: .5);
            var oldLimits = wpfPlot1.Plot.GetAxisLimits();
            wpfPlot1.Plot.SetAxisLimits(xMax: oldLimits.XMax + 1000);
        }
    }
}
