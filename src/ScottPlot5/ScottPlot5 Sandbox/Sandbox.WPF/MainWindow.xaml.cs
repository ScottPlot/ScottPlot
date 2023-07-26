using System.Windows;
using ScottPlot;

#nullable enable

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        WpfPlot.Plot.Add.Signal(Generate.Sin());
        WpfPlot.Plot.Add.Signal(Generate.Cos());

        WpfPlot.Refresh();
    }

    private void SetLabelText(string text)
    {
        WpfPlot.Plot.Style.SetFontFromText(text);
        WpfPlot.Plot.Title(text, 24);
        WpfPlot.Plot.YLabel(text, 24);
        WpfPlot.Plot.XLabel(text, 24);
        WpfPlot.Refresh();
    }

    private void English_Click(object sender, RoutedEventArgs e) => SetLabelText("Test");

    private void Chinese_Click(object sender, RoutedEventArgs e) => SetLabelText("测试");

    private void Japanese_Click(object sender, RoutedEventArgs e) => SetLabelText("試験");

    private void Korean_Click(object sender, RoutedEventArgs e) => SetLabelText("테스트");
}
