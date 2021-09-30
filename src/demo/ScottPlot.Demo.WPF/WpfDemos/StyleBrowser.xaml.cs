using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ScottPlot.Styles;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for StyleBrowser.xaml
    /// </summary>
    public partial class StyleBrowser : Window
    {
        public StyleBrowser()
        {
            InitializeComponent();

            foreach (var style in ScottPlot.Style.GetStyles())
                ListBox1.Items.Add(style);

            WpfPlot1.Plot.AddSignal(DataGen.Sin(51));
            WpfPlot1.Plot.AddSignal(DataGen.Cos(51));
            WpfPlot1.Plot.XLabel("Horizontal Axis");
            WpfPlot1.Plot.YLabel("Vertical Axis");
            WpfPlot1.Plot.Title("Default Style");
            WpfPlot1.Refresh();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var style = (ScottPlot.Styles.IStyle)ListBox1.SelectedItem;

            if (style is null)
                return;

            WpfPlot1.Plot.Style(style);
            WpfPlot1.Plot.Title(style.ToString());
            WpfPlot1.Refresh();
        }
    }
}
