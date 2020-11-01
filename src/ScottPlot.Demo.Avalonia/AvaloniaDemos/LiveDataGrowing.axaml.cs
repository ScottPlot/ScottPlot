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
        PlottableSignal signalPlot;
        Random rand = new Random(0);

        TextBox ReadingsTextbox;
        TextBox LatestValueTextbox;
        CheckBox AutoAxisCheckbox;

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
            signalPlot = avaPlot1.plt.PlotSignal(data);
            avaPlot1.plt.YLabel("Value");
            avaPlot1.plt.XLabel("Sample Number");

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
            signalPlot.maxRenderIndex = nextDataIndex;
            ReadingsTextbox.Text = $"{nextDataIndex + 1}";
            LatestValueTextbox.Text = $"{latestValue:0.000}";
            nextDataIndex += 1;
        }

        void Render(object sender, EventArgs e)
        {
            if (AutoAxisCheckbox.IsChecked == true)
                avaPlot1.plt.AxisAuto();
            avaPlot1.Render();
        }

        private void DisableAutoAxis(object sender, RoutedEventArgs e)
        {
            double[] autoAxisLimits = avaPlot1.plt.AxisAuto(verticalMargin: .5);
            double oldX2 = autoAxisLimits[1];
            avaPlot1.plt.Axis(x2: oldX2 + 1000);
        }
    }
}
