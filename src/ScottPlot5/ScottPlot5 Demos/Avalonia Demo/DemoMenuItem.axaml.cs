using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia_Demo.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Avalonia_Demo;

public partial class DemoMenuItem : UserControl
{
    public DemoMenuItem()
    {
        InitializeComponent();

        LaunchButton.Click += (s, e) => OpenDemo();
    }

    private void OpenDemo()
    {
        var demo = ((DataContext as DemoMenuItemViewModel)?.Demo) ?? throw new System.ArgumentNullException("No demo provided");
        demo.GetWindow().Show();
    }
}
