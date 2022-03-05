using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ScottPlot.Avalonia;
using System;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class LiveDataGrowing : Window
    {
        AvaPlot avaPlot1;
        public double[] data = new double[100_000];
        int nextDataIndex = 1;
        Plottable.SignalPlot signalPlot;
        Random rand = new Random(0);

        TextBox ReadingsTextbox;
        TextBox LatestValueTextbox;
        CheckBox AutoAxisCheckbox;

        private DispatcherTimer _updateDataTimer;
        private DispatcherTimer _renderTimer;

        public LiveDataGrowing()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            avaPlot1 = this.Find<AvaPlot>("avaPlot1");
            ReadingsTextbox = this.Find<TextBox>("ReadingsTextbox");
            LatestValueTextbox = this.Find<TextBox>("LatestValueTextbox");
            AutoAxisCheckbox = this.Find<CheckBox>("AutoAxisCheckbox");

            // plot the data array only once
            signalPlot = avaPlot1.Plot.AddSignal(data);
            avaPlot1.Plot.YLabel("Value");
            avaPlot1.Plot.XLabel("Sample Number");

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

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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
                avaPlot1.Plot.AxisAuto();
            avaPlot1.Refresh();
        }

        private void DisableAutoAxis(object sender, RoutedEventArgs e)
        {
            avaPlot1.Plot.AxisAuto(verticalMargin: .5);
            var autoLimits = avaPlot1.Plot.GetAxisLimits();
            avaPlot1.Plot.SetAxisLimits(xMax: autoLimits.XMax + 1000);
        }
    }
}
