using System.Windows;

namespace WPF_Demo.DemoWindows;

public partial class MultiThreading : Window, IDemoWindow
{
    public string DemoTitle => "WPF Multi-Threading";
    public string Description => "Demonstrate how to safely change data while rendering asynchronously.";

    public MultiThreading()
    {
        InitializeComponent();
    }
}
