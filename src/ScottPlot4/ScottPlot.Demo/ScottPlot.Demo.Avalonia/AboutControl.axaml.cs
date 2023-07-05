using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ScottPlot.Demo.Avalonia
{
    public partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            this.InitializeComponent();
            this.VersionLabel.Text = $"version {Plot.Version}";
        }
    }
}
