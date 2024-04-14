using System.Windows;
using System.Windows.Media.Imaging;

namespace WPF_Demo;

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
        var demoWindows = System.Reflection.Assembly.GetAssembly(typeof(MainWindow))!
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IDemoWindow)))
            .Where(x => !x.IsInterface)
            .ToList();

        void MoveToTop(Type t) { demoWindows.Remove(t); demoWindows.Insert(0, t); }
        MoveToTop(typeof(DemoWindows.Quickstart));

        demoWindows.ForEach(x => DemoItemPanel.Children.Add(new DemoMenuItem(x)));
    }
}
