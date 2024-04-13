using Avalonia.Controls;

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
