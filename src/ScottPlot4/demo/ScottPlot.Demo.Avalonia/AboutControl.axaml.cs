using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ScottPlot.Demo.Avalonia
{
    public class AboutControl : UserControl
    {
        public AboutControl()
        {
            this.InitializeComponent();
            this.Find<TextBlock>("VersionLabel").Text = $"version {Plot.Version}";
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
