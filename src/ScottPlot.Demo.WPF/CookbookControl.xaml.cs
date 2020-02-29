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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF
{
    /// <summary>
    /// Interaction logic for DemoPlotControl.xaml
    /// </summary>
    public partial class CookbookControl : UserControl
    {
        public CookbookControl()
        {
            InitializeComponent();
            wpfPlot1.Rendered += WpfPlot1_Rendered;
        }

        private void WpfPlot1_Rendered(object sender, EventArgs e)
        {
            BenchmarkLabel.Content = wpfPlot1.plt.GetSettings(false).benchmark.ToString();
        }

        public void LoadDemo(string objectPath)
        {
            Debug.WriteLine($"Loading demo: {objectPath}");
            string fileName = "/src/" + objectPath.Split('+')[0].Replace(".", "/") + ".cs";
            fileName = fileName.Replace("ScottPlot/Demo", "ScottPlot.Demo");
            string methodName = objectPath.Split('+')[1];
            var demoPlot = Reflection.GetPlot(objectPath);

            DemoNameLabel.Content = demoPlot.name;
            DemoFileLabel.Content = $"{fileName} ({methodName})";
            DescriptionTextbox.Text = (demoPlot.description is null) ? "no descriton provided..." : demoPlot.description;

            wpfPlot1.Reset();
            demoPlot.Render(wpfPlot1.plt);
            wpfPlot1.Render();
        }

        private void ViewCode_MouseEnter(object sender, MouseEventArgs e)
        {
            ViewCodeLabel.Foreground = Brushes.Blue;
        }

        private void ViewCode_MouseLeave(object sender, MouseEventArgs e)
        {
            ViewCodeLabel.Foreground = Brushes.Black;
        }

        private void ViewCode_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string filePath = DemoFileLabel.Content.ToString().Split(' ')[0];
            string url = "https://github.com/swharden/ScottPlot/blob/master" + filePath;
            Tools.LaunchBrowser(url);
        }
    }
}
