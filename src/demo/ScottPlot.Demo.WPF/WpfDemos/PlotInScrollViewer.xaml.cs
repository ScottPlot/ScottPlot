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
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for PlotInScrollViewer.xaml
    /// </summary>
    public partial class PlotInScrollViewer : Window
    {
        Random rand = new Random();

        public PlotInScrollViewer()
        {
            InitializeComponent();

            WpfPlot[] wpfPlots = { wpfPlot1, wpfPlot2, wpfPlot3 };

            foreach (WpfPlot wpfPlot in wpfPlots)
            {
                for (int i = 0; i < 3; i++)
                    wpfPlot.Plot.AddSignal(DataGen.RandomWalk(rand, 100));

                wpfPlot.Configuration.ScrollWheelZoom = false;
                wpfPlot.Render();
            }
        }
    }
}
