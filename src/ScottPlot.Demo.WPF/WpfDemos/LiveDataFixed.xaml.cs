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
    /// Interaction logic for LiveDataFixed.xaml
    /// </summary>
    public partial class LiveDataFixed : Window
    {
        Random rand = new Random();
        double[] liveData = DataGen.Sin(100, oscillations: 2, mult: 20);

        public LiveDataFixed()
        {
            InitializeComponent();

            // plot the data array only once
            wpfPlot1.plt.PlotSignal(liveData);
            wpfPlot1.plt.Axis(y1: -50, y2: 50);
            wpfPlot1.Render();

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
            for (int i = 0; i < liveData.Length; i++)
                liveData[i] += rand.NextDouble() - .5;
        }

        void Render(object sender, EventArgs e)
        {
            wpfPlot1.Render(skipIfCurrentlyRendering: true);
        }
    }
}
