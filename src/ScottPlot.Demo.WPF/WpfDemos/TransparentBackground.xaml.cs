using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for TransparentBackground.xaml
    /// </summary>
    public partial class TransparentBackground : Window
    {
        public TransparentBackground()
        {
            InitializeComponent();

            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            wpfPlot1.plt.PlotScatter(x, sin);
            wpfPlot1.plt.PlotScatter(x, cos);

            wpfPlot1.plt.Style(figBg: System.Drawing.Color.Transparent);
            wpfPlot1.plt.Style(dataBg: System.Drawing.Color.Transparent);
            wpfPlot1.Render();
        }
    }
}
