using System.Diagnostics;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ScottPlot.Avalonia
{
    public class HelpWindow : Window
    {
        public HelpWindow()
        {
            this.InitializeComponent();
            this.Find<TextBlock>("VersionLabel").Text = Plot.Version;

            StringBuilder msg = new StringBuilder();
            msg.AppendLine("Left-click-drag: pan");
            msg.AppendLine("Right-click-drag: zoom");
            msg.AppendLine("Middle-click-drag: zoom region");
            msg.AppendLine("");
            msg.AppendLine("Right-click: show menu");
            msg.AppendLine("Middle-click: auto-axis");
            msg.AppendLine("Double-click: show benchmark");
            msg.AppendLine("");
            msg.AppendLine("Hold CTRL to lock vertical axis");
            msg.AppendLine("Hold ALT to lock horizontal axis");
            this.Find<TextBlock>("InfoTextBlock").Text = msg.ToString();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void LaunchScottPlotWebsite(object sender, RoutedEventArgs e)
        {
            using (Process proc = new Process())
            {
                proc.StartInfo.FileName = "https://ScottPlot.NET";
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
            }
        }

        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
