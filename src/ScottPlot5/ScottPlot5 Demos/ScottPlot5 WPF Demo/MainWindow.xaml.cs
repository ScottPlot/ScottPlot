using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF_Demo;

namespace WpfDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        string logoImagePath = System.IO.Path.GetFullPath("Resources/logo-128.png");
        Uri logoImageUri = new(logoImagePath, UriKind.Absolute);
        LogoImage.Source = new BitmapImage(logoImageUri);

        Subtitle.Content = $"ScottPlot.WPF Version {ScottPlot.Version.VersionString}";

        DemoItemPanel.Children.Clear();
        System.Reflection.Assembly.GetAssembly(typeof(MainWindow))!
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IDemoWindow)))
            .Where(x => !x.IsInterface)
            .ToList()
            .ForEach(x => DemoItemPanel.Children.Add(new DemoMenuItem(x)));
    }
}