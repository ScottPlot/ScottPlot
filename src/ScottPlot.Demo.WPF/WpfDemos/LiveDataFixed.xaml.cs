using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
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
    /// Interaction logic for LiveDataFixed.xaml
    /// </summary>
    public partial class LiveDataFixed : Window
    {
        Random rand = new Random();
        double[] liveData = new double[400];
        DataGen.Electrocardiogram ecg = new DataGen.Electrocardiogram();
        Stopwatch sw = Stopwatch.StartNew();

        private System.Threading.Timer _timer;

        public LiveDataFixed()
        {
            InitializeComponent();
            wpfPlot1.Configure(middleClickMarginX: 0);

            // plot the data array only once
            wpfPlot1.plt.PlotSignal(liveData);
            wpfPlot1.plt.AxisAutoX(margin: 0);
            wpfPlot1.plt.Axis(y1: -1, y2: 2.5);

            // create a traditional timer to update the data
            _timer = new Timer(_ => UpdateData(), null, 0, 5);

            // create a separate timer to update the GUI
            DispatcherTimer renderTimer = new DispatcherTimer();
            renderTimer.Interval = TimeSpan.FromMilliseconds(10);
            renderTimer.Tick += Render;
            renderTimer.Start();
        }

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
            wpfPlot1.Render(skipIfCurrentlyRendering: true);
        }
    }
}
