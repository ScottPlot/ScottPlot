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

namespace ScottPlotQuickstartWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // generate some data to plot
            int pointCount = 50;
            double[] dataXs = new double[pointCount];
            double[] dataSin = new double[pointCount];
            double[] dataCos = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                dataXs[i] = i;
                dataSin[i] = Math.Sin(i * 2 * Math.PI / pointCount);
                dataCos[i] = Math.Cos(i * 2 * Math.PI / pointCount);
            }

            // create the scottPlot and render it onto a Canvas
            ScottPlot.Plot plt = new ScottPlot.Plot((int)canvasPlot.ActualWidth, (int)canvasPlot.ActualHeight);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.AxisAuto();
            plt.Title("ScottPlot WPF Demo");
            imagePlot.Source = bmpImageFromBmp(plt.GetBitmap());
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

    }
}
