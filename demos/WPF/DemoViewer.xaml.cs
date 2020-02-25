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
using System.Windows.Threading;

namespace WPF
{
    /// <summary>
    /// Interaction logic for DemoBrowser.xaml
    /// </summary>
    public partial class DemoViewer : Window
    {
        public DemoViewer()
        {
            InitializeComponent();
            wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.RandomWalk(null, 100));


            DescriptionTextBlock.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc mattis commodo pulvinar. Mauris quam ipsum, vestibulum vel eleifend ut, pretium a ante. Maecenas imperdiet sodales diam, auctor ullamcorper velit auctor at. Morbi est mi, egestas aliquam maximus nec, mattis eget eros. Pellentesque placerat mi sit amet orci porttitor porttitor. Etiam sit amet ornare sem, vitae molestie orci. Donec dignissim porta eleifend.";
        }

        private void OnRender(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                BenchmarkLabel.Content = wpfPlot1.plt.GetSettings(false).benchmark.ToString();
            }), DispatcherPriority.Render);
        }
    }
}
