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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfLineTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand = new Random();
        DispatcherTimer dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += RedrawScottPlot;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RedrawScottPlot(null, null);
        }

        public BitmapImage bmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            ((System.Drawing.Bitmap)bmp).Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            bmpImage.StreamSource = stream;
            bmpImage.EndInit();
            return bmpImage;
        }

        private void RedrawScottPlot(object sender, EventArgs e)
        {
            // create random X/Y data
            int dataPointCount = 10_000;
            double[] ys1 = new double[dataPointCount];
            double[] ys2 = new double[dataPointCount];
            double[] ys3 = new double[dataPointCount];
            for (int i = 0; i < dataPointCount; i++)
            {
                ys1[i] = .75 * Math.Sin(i / (double)dataPointCount * 2 * Math.PI * 3) + rand.NextDouble() / 20;
                ys2[i] = .5 * Math.Cos(i / (double)dataPointCount * 2 * Math.PI * 3) + rand.NextDouble() / 20;
                ys3[i] = Math.Sin(i / (double)dataPointCount * 2 * Math.PI * 1) + rand.NextDouble() / 20;
            }

            // create the scottPlot and render it onto a Canvas
            ScottPlot.Plot sp1 = new ScottPlot.Plot((int)myCanvas.ActualWidth, (int)myCanvas.ActualHeight);
            sp1.data.AddSignal(ys1, 100);
            sp1.data.AddSignal(ys2, 100);
            sp1.data.AddSignal(ys3, 100);
            sp1.settings.title = "WPF Demonstration - Random Data";
            sp1.settings.benchmarkShow = true;
            sp1.settings.AxisFit();
            System.Drawing.Bitmap bmp = sp1.figure.GetBitmap(true);
            BitmapImage bmpImage = bmpImageFromBmp(bmp);
            imagePlot.Source = bmpImage;
        }

        private void CbContinuous_Checked(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Start();
        }

        private void CbContinuous_Unchecked(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
        }
    }
}
