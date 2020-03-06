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
            var demoPlot = Reflection.GetPlot(objectPath);

            DemoNameLabel.Content = demoPlot.name;
            SourceCodeLabel.Content = $"{demoPlot.sourceFile} ({demoPlot.categoryClass})";
            DescriptionTextbox.Text = (demoPlot.description is null) ? "no descriton provided..." : demoPlot.description;

            wpfPlot1.Reset();
            demoPlot.Render(wpfPlot1.plt);
            wpfPlot1.Render();

            string sourceCode = demoPlot.GetSourceCode("../../../../../src/ScottPlot.Demo/");
            SourceTextBox.Text = sourceCode;
        }
    }
}
