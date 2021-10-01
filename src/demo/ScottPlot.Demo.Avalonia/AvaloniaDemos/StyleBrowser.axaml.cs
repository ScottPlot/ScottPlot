using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Styles;
using System.Collections.Generic;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public partial class StyleBrowser : Window
    {
        private readonly AvaPlot avaPlot;
        private readonly ListBox listBox;
        public StyleBrowser()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            avaPlot = this.Find<AvaPlot>("AvaPlot1");
            listBox = this.Find<ListBox>("ListBox1");

            var listItems = new List<IStyle>();

            foreach (var style in ScottPlot.Style.GetStyles())
                listItems.Add(style);

            listBox.Items = listItems;

            avaPlot.Plot.AddSignal(DataGen.Sin(51));
            avaPlot.Plot.AddSignal(DataGen.Cos(51));
            avaPlot.Plot.XLabel("Horizontal Axis");
            avaPlot.Plot.YLabel("Vertical Axis");
            avaPlot.Plot.Title("Default Style");
            avaPlot.Refresh();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var style = (ScottPlot.Styles.IStyle)listBox.SelectedItem;

            if (style is null)
                return;

            avaPlot.Plot.Style(style);
            avaPlot.Plot.Title(style.ToString());
            avaPlot.Refresh();
        }
    }
}
