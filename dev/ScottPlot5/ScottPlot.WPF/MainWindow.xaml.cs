using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
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

namespace ScottPlot.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Plot Plot = new();
        public MainWindow()
        {
            InitializeComponent();
            Plot.AddDemoSinAndCos();
        }

        private void SKElement1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            ICanvas canvas = new SkiaCanvas() { Canvas = e.Surface.Canvas };
            Plot.Draw(canvas, (float)SKElement1.ActualWidth, (float)SKElement1.ActualHeight);
        }
    }
}
