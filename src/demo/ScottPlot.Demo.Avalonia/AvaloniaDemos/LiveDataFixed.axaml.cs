using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ScottPlot.Avalonia;
using System;
using System.Diagnostics;
using System.Threading;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class LiveDataFixed : Window
    {
        AvaPlot avaPlot1;
        Random rand = new Random();
        double[] liveData = new double[400];
        DataGen.Electrocardiogram ecg = new DataGen.Electrocardiogram();
        Stopwatch sw = Stopwatch.StartNew();

        public LiveDataFixed()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            avaPlot1 = this.Find<AvaPlot>("avaPlot1");

            avaPlot1.Configure(middleClickMarginX: 0);

            // plot the data array only once
            avaPlot1.plt.PlotSignal(liveData);
            avaPlot1.plt.AxisAutoX(margin: 0);
            avaPlot1.plt.Axis(y1: -1, y2: 2.5);

            // create a traditional timer to update the data
            _timer = new Timer(_ => UpdateData(), null, 0, 5);

            // create a separate timer to update the GUI
            DispatcherTimer renderTimer = new DispatcherTimer();
            renderTimer.Interval = TimeSpan.FromMilliseconds(10);
            renderTimer.Tick += Render;
            renderTimer.Start();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private System.Threading.Timer _timer;

        void UpdateData()
        {
            // "scroll" the whole chart to the left
            Array.Copy(liveData, 1, liveData, 0, liveData.Length - 1);

            // place the newest data point at the end
            double nextValue = ecg.GetVoltage(sw.Elapsed.TotalSeconds);
            liveData[liveData.Length - 1] = nextValue;
        }

        void Render(object sender, EventArgs e)
        {
            avaPlot1.Render();
        }
    }
}
